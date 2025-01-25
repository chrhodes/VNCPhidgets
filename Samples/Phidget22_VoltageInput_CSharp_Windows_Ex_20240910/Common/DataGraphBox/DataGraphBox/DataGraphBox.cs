using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Phidget22.ExampleUtils {
	public partial class DataGraphBox : Form {
		private enum ChartType {
			[Description("Raw Data (Default)")]
			DataPoints,
			[Description("FFT (Frequency)")]
			FFT,
			[Description("Allan Deviation (Noise)")]
			AllanDeviation
		};

		private enum FilterType {
			[Description("None")]
			None,
			[Description("Low Pass")]
			LPF,
			[Description("High Pass")]
			HPF
		};

		private double chartTime = 20;
		private bool pause = false;

		DateTime prevGraphTime = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));

		DateTime lastRedraw = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
		double redrawInterval = 50; // ms

		DateTime? initialTimestamp = null;
		int dataTimeCnt = 0;

		double maxTimeError = 50; // max allowed error between real and calculated time in ms
		TimeSpan? latestError = null;

		private ChartType chartType = ChartType.DataPoints;

		private String filename = "";

		struct DataPoint {
			public DateTime Date;
			public double Data;
		}

		List<DataPoint> allDataPoints = new List<DataPoint>();

		System.IO.StreamWriter fs;

		private int filterLength = 0;
		private FilterType filterType = FilterType.None;

		private bool? _calculateTimeFromDataInterval = null;
		private int dataIntervalErrorCnt = 0;

		object accumulatorLock = new object();
		DateTime? accumulateStart = null;
		double timeAccumulator = 0;
		int dataPointAccumulator = 0;

		double? measuredRate = null;

		//check regional settings to set delimiter character for csv files
		string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

		public bool CalculateTimeFromDataInterval {
			get {
				if (_calculateTimeFromDataInterval.HasValue)
					return _calculateTimeFromDataInterval.Value;
				if (dataIntervalErrorCnt > 0)
					return false;
				return true;
			}
			set {
				_calculateTimeFromDataInterval = value;
			}
		}

		public bool CorrectTimeDivergence {
			get {
				if (_calculateTimeFromDataInterval.HasValue && _calculateTimeFromDataInterval.Value)
					return false;
				return true;
			}
		}

		public DataGraphBox(Form owner, String title, String yTitle = "") {
			InitializeComponent();
			Owner = owner;
			Text = title;
			graph.Titles[0].Text = title;
			YTitle = yTitle;

			ChartTimeTrackbar.Value = (int)chartTime;
			updateTimeBarMax();

			chartTypeBox.DataSource = typeof(ChartType).ToList();
			chartTypeBox.SelectedValue = chartType;

			filterTypeBox.DataSource = typeof(FilterType).ToList();
			filterTypeBox.SelectedValue = FilterType.None;
			filterWarningLabel.Text = "";

			FileSizeLabel.Text = "";
			FileNameLabel.Text = "";

			dataRateLbl.Text = "";
		}

		private void DataGraphBox_Load(object sender, EventArgs e) {

			updateGuiTimer.Interval = (int)redrawInterval;
			updateGuiTimer.Start();
		}

		public static String GenerateTitle(String desc, Phidget phid) {
			/*String newTitle = (desc + " | Serial: " + phid.DeviceSerialNumber.ToString());
			if (phid.IsHubPortDevice || (phid.HubPort != -1))
				newTitle += " | HubPort: " + phid.HubPort.ToString();
			newTitle += " | Channel: " + phid.Channel.ToString();
			return (newTitle);*/

			return (desc + " [" + phid.ToString() + "]");

		}

		public void SetTitle(String newTitle) {
			Text = newTitle;
			graph.Titles[0].Text = newTitle;
		}

		string YTitle = "";
		public void SetAxisYTitle(String newTitle) {
			YTitle = newTitle;

			if (chartType == ChartType.DataPoints)
				graph.ChartAreas[0].AxisY.Title = YTitle;
		}

		private void updateGuiTimer_Tick(object sender, EventArgs e) {

			updateGuiTimer.Stop();

			GUIUpdateItem item;
			try {
				if (guiUpdateQueue.Count > 0)
					Suspend();
				while (guiUpdateQueue.TryDequeue(out item)) {
					if (item.type == GUIUpdateItem.GUIUpdateType.data)
						addDataInternal(item.data, item.dataTime);
					else if (item.type == GUIUpdateItem.GUIUpdateType.dataInterval)
						DataInterval = item.data;
					else if (item.type == GUIUpdateItem.GUIUpdateType.dataGap)
						addGapInternal(item.dataTime);
				}
			} catch { /* XXX */ } finally {
				Resume();
			}

			lock (accumulatorLock) {
				if (measuredRate.HasValue) {
					double interval = measuredRate.Value;
					measuredRate = null;

					StringBuilder dr = new StringBuilder();

					if (DataInterval.HasValue) {
						if (CalculateTimeFromDataInterval)
							dr.Append("Time Base: Device  |  ");
						else
							dr.Append("Time Base: System  |  ");

						if (latestError.HasValue && Math.Abs(latestError.Value.TotalMilliseconds) >= 1)
							dr.Append("Clock Offset: " + latestError.Value.TotalMilliseconds.ToString("F1") + "ms  |  ");

						if (DataInterval.Value >= 32) {
							dr.Append("Data Interval: " + Math.Round(DataInterval.Value, 1).ToString("F1") + "ms (Measured: ");
							dr.Append(Math.Round(interval, 1).ToString("F1") + "ms)");
						} else {
							dr.Append("Data Rate: " + Math.Round(1000.0 / DataInterval.Value, 1).ToString("F1") + "Hz (Measured: ");
							dr.Append(Math.Round(1000.0 / interval, 1).ToString("F1") + "Hz)");
						}
					} else {
						if (interval >= 31.9) {
							dr.Append("Data Interval (Measured): " + Math.Round(interval, 1).ToString("F1") + "ms");
						} else {
							dr.Append("Data Rate (Measured): " + Math.Round(1000.0 / interval, 1).ToString("F1") + "Hz");
						}
					}

					dataRateLbl.Text = dr.ToString();
				}
			}

			updateGuiTimer.Start();
		}

		double? dataInterval = null;
		int intervalChanged = 0;
		public double? DataInterval {
			get { return dataInterval; }
			set {
				var oldInterval = dataInterval;

				if (value.HasValue && value.Value > 0) {
					dataInterval = value;
				} else {
					dataInterval = null;
				}

				updateTimeBarMax();

				// No change
				if (oldInterval == dataInterval)
					return;

				dataRateLbl.Text = "";
				dataIntervalErrorCnt = 0;
				initialTimestamp = null;
				intervalChanged = 1;
				lock (accumulatorLock) {
					accumulateStart = null;
					measuredRate = null;
					dataPointAccumulator = 0;
					timeAccumulator = 0;
				}

				// Interval changed
				if (oldInterval.HasValue && dataInterval.HasValue) {
					updateFilterBarMax();
				}

				if (!oldInterval.HasValue && dataInterval.HasValue) {
					// from no interval to interval
					graphTypeLabel.Show();
					chartTypeBox.Show();
					chartTypeBox.SelectedValue = ChartType.DataPoints;
					filterPanel.Show();
				} else if (oldInterval.HasValue && !dataInterval.HasValue) {
					// from interval to no interval
					graphTypeLabel.Hide();
					chartTypeBox.Hide();
					chartTypeBox.SelectedValue = ChartType.DataPoints;
					filterPanel.Hide();
				}
			}
		}

		bool suspended = false;
		public void Suspend() {
			if (pause || suspended)
				return;

			suspended = true;
			graph.Series.SuspendUpdates();
			graph.SuspendLayout();
		}
		public void Resume() {
			if (pause || !suspended)
				return;

			graph.ResumeLayout();
			graph.Series.ResumeUpdates();
			suspended = false;
			graph.ResetAutoValues();
		}

		public void addData(double dataPoint, double? dataInterval, double timestamp) {
			enqueueData(dataPoint, timestamp, dataInterval);
		}
		public void addData(double dataPoint, double dataInterval) {
			enqueueData(dataPoint, null, dataInterval);
		}
		public void addData(double dataPoint) {
			enqueueData(dataPoint);
		}
		public void addGap() {
			enqueueData(null);
		}

		// This is meant to be thread safe and quick. Actually drawing the graph is done in the GUI thread
		private void enqueueData(double? dataPoint, double? timestamp = null, double? dataInterval = null) {

			if (pause)
				return;

			if (!Visible)
				return;

			DateTime now = PreciseClock.Now;
			TimeSpan error;

			double? interval = DataInterval;
			if (dataInterval.HasValue && dataInterval != interval) {
				guiUpdateQueue.Enqueue(new GUIUpdateItem { type = GUIUpdateItem.GUIUpdateType.dataInterval, data = dataInterval.Value });
				interval = dataInterval;
				dataIntervalErrorCnt = 0;
				initialTimestamp = null;
				intervalChanged = 1;
				lock (accumulatorLock) {
					accumulateStart = null;
					measuredRate = null;
					dataPointAccumulator = 0;
					timeAccumulator = 0;
				}
			}

			// consider error right after interval changes but only for 5 intervals / 100ms at most.
			if (intervalChanged > 0) {
				if (interval.HasValue) {
					if (intervalChanged < 5 || intervalChanged < (100 / interval.Value))
						intervalChanged++;
					else
						intervalChanged = 0;
				} else {
					intervalChanged = 0;
				}
			}

			// We try to use the dataInterval to increment the data time, because actual time is going to be skewed by system load, etc.
			DateTime dataTime;
			if (timestamp.HasValue) {
				// If we have a timebase from the firmware, use that directly
				if (!initialTimestamp.HasValue)
					initialTimestamp = now.AddMilliseconds(-timestamp.Value);
				dataTime = initialTimestamp.Value.AddMilliseconds(timestamp.Value);

				//Make sure we give a data interval change time to propogate
				if (intervalChanged > 0) {
					// If the error is > 1ms reset
					if (Math.Abs(now.Subtract(dataTime).TotalMilliseconds) > 1) {
						initialTimestamp = now.AddMilliseconds(-timestamp.Value);
						dataTime = initialTimestamp.Value.AddMilliseconds(timestamp.Value);
					}
				}
			} else if (interval.HasValue && CalculateTimeFromDataInterval) {
				// otherwise, if we know the data interval, use that
				if (!initialTimestamp.HasValue) {
					initialTimestamp = now;
					dataTimeCnt = 0;
				}
				dataTime = initialTimestamp.Value.AddTicks((long)(interval.Value * TimeSpan.TicksPerMillisecond * dataTimeCnt));
				dataTimeCnt++;

				//Make sure we give a data interval change time to propogate
				error = now.Subtract(dataTime);
				if (intervalChanged > 0) {
					// If the error is > 1ms reset
					if (Math.Abs(error.TotalMilliseconds) > 1) {
						initialTimestamp = now;
						dataTime = now;
						dataTimeCnt = 1;
					}
				}

			} else {
				// otherwise, just use the system time
				dataTime = now;
			}

			// compare dataTime and now, and make sure they don't diverge too much
			//   dataTime being incremented by dataInterval depends on the dataInterval being close to reality
			// NOTE: It's pretty normal to have a static offset of up to 8ms for devices which send multiple data per packet
			error = now.Subtract(dataTime);
			if (CorrectTimeDivergence && Math.Abs(error.TotalMilliseconds) > maxTimeError) {
				dataIntervalErrorCnt++;
				dataTime = now;
				initialTimestamp = null;
			}

			if (dataPoint.HasValue)
				guiUpdateQueue.Enqueue(new GUIUpdateItem { type = GUIUpdateItem.GUIUpdateType.data, data = dataPoint.Value, dataTime = dataTime });
			else
				guiUpdateQueue.Enqueue(new GUIUpdateItem { type = GUIUpdateItem.GUIUpdateType.dataGap, dataTime = dataTime });


			lock (accumulatorLock) {
				latestError = error;
				if (!accumulateStart.HasValue) {
					accumulateStart = now;
					dataPointAccumulator = 0;
					timeAccumulator = 0;
				} else {
					dataPointAccumulator++;
					TimeSpan diff = now.Subtract(accumulateStart.Value);
					timeAccumulator = diff.TotalMilliseconds;

					if (timeAccumulator >= 1000) {
						measuredRate = timeAccumulator / (double)dataPointAccumulator;
						accumulateStart = now;
						dataPointAccumulator = 0;
						timeAccumulator = 0;
					}
				}
			}
		}

		private void trimDataPointGraph() {

			if (graph.Series[0].Points.Count > 5) {
				DateTime endTime = DateTime.FromOADate(graph.Series[0].Points.Last().XValue);
				double seconds;

				do {
					DateTime startTime = DateTime.FromOADate(graph.Series[0].Points.First().XValue);
					seconds = endTime.Subtract(startTime).TotalSeconds;
					if (seconds > chartTime) {
						graph.Series[0].Points.RemoveAt(0);
					}
				} while (seconds > chartTime);
			}
		}

		private void addGapInternal(DateTime dataTime) {

			if (chartType == ChartType.DataPoints) {
				trimDataPointGraph();
				graph.Series[0].Points.AddXY(dataTime, double.NaN);
				graph.Series[0].Points.Last().IsEmpty = true;
			}
		}
		double? graphInterval = null;
		private void addDataInternal(double dataPoint, DateTime dataTime) {

			if (graphInterval.HasValue == false)
				graphInterval = DataInterval;

			if (measuredRate.HasValue) {
				if (!DataInterval.HasValue || Math.Abs(measuredRate.Value - DataInterval.Value) > 2)
					graphInterval = measuredRate;
				else
					graphInterval = DataInterval;
			}

			try {
				if (allDataPoints.Count > 5) {
					DateTime endTime = allDataPoints.Last().Date;
					double seconds;
					do {
						DateTime startTime = allDataPoints.First().Date;
						seconds = endTime.Subtract(startTime).TotalSeconds;
						if (seconds > chartTime) {
							allDataPoints.RemoveAt(0);
						}
					} while (seconds > chartTime);
				}
				allDataPoints.Add(new DataPoint { Data = dataPoint, Date = dataTime });

				if (filename != "") {
					fs.WriteLine(dataTime.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + delimiter + dataPoint.ToString());
					Int64 lengthKB = fs.BaseStream.Length / 1024;
					FileSizeLabel.Text = "File Size: " + lengthKB.ToString() + "kB";
				}

				switch (chartType) {
				case ChartType.DataPoints:
					trimDataPointGraph();
					graph.BackColor = Color.White;
					badIntervalText.Visible = false;
					double filteredPoint = 0;
					if (filterLength != 0) {
						if (allDataPoints.Count < filterLength)
							return;
						for (int i = 0; i < filterLength; i++) {
							filteredPoint += allDataPoints[(allDataPoints.Count - 1) - i].Data;
						}
						filteredPoint /= filterLength;
						if (filterType == FilterType.HPF)
							filteredPoint = dataPoint - filteredPoint; //slightly hacky, but improved feel to the user.
					} else
						filteredPoint = dataPoint;

					graph.Series[0].Points.AddXY(dataTime, filteredPoint);
					break;

				case ChartType.FFT:
					if (dataTime.Subtract(prevGraphTime).TotalSeconds < (chartTime / 120))
						return;
					prevGraphTime = dataTime;
					doFFTGraph(graphInterval.Value);
					break;

				case ChartType.AllanDeviation:
					if (dataTime.Subtract(prevGraphTime).TotalSeconds < ((chartTime <= 900) ? (0.5) : (chartTime / 1800)))
						return;
					prevGraphTime = dataTime;
					calculateAllanDeviation(graphInterval.Value);
					break;
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
			}

		}

		public void doFFTGraph(double dataInterval) {
			//count must be a power of 2.
			int count = 1;
			while (count <= allDataPoints.Count)
				count *= 2;
			count /= 2;

			// This is a limit of the FFT library
			if (count > (1024 * 16))
				count = 1024 * 16;

			if (count < 4)
				return;

			double ms = 0;
			double highDiff = -1;
			double lowDiff = 1e6;
			for (int i = 1; i < allDataPoints.Count; i++) {
				double timeDiff = (allDataPoints[i].Date - allDataPoints[i - 1].Date).TotalMilliseconds;
				if (timeDiff > highDiff)
					highDiff = timeDiff;
				if (timeDiff < lowDiff)
					lowDiff = timeDiff;

				ms += timeDiff;
			}
			ms /= allDataPoints.Count - 1;
			Math.Round(ms);
			if (Math.Abs(dataInterval - ms) > 2) {
				graph.BackColor = Color.LightCoral;
				badIntervalText.Visible = true;
			} else {
				graph.BackColor = Color.White;
				badIntervalText.Visible = false;
			}
			ms = dataInterval;

			double maxFrequency = (1000.0 / ms) / 2; //half of the datarate
			double resolution = maxFrequency / (count / 2.0);

			Exocortex.DSP.ComplexF[] complexData = new Exocortex.DSP.ComplexF[count];

			for (int i = 0; i < count; ++i) {
				// Fill the complex data
				complexData[i].Re = (float)allDataPoints[(allDataPoints.Count - count) + i].Data; // Add your real part here
			}

			// FFT the time domain data to get frequency domain data
			Exocortex.DSP.Fourier.FFT(complexData, Exocortex.DSP.FourierDirection.Forward);

			float[] mag_dat_buffer = new float[complexData.Length];

			// Loop through FFT'ed data and do something with it
			graph.Series[0].Points.Clear();

			//only the first half - 2nd half is the data reversed
			List<freqIntensity> intensityList = new List<freqIntensity>();
			for (int i = 1; i < complexData.Length / 2; ++i) {
				double intensity = Math.Sqrt(complexData[i].Re * complexData[i].Re + complexData[i].Im * complexData[i].Im);
				intensity /= complexData.Length / 2;
				double frequency = i * resolution;

				intensityList.Add(new freqIntensity(frequency, intensity));

				// Calculate magnitude or do something with the new complex data
				graph.Series[0].Points.AddXY(frequency, intensity);
			}

			var max = intensityList.IndexOf(intensityList.Max());
			//graph.Series[0].Points[max].IsValueShownAsLabel = true;
			graph.Series[0].Points[max].Label = intensityList[max].freq.ToString("0.####") + "Hz";

			graph.ChartAreas[0].AxisX.MinorGrid.Interval = 5;
			graph.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
		}

		private void DataGraphBox_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing) {
				this.Hide();
				e.Cancel = true;
				if (fs != null)
					fs.Close();
				filename = "";
				SaveButton.Text = "Save to CSV";
			}
		}

		private void updateFilterBarMax() {

			if (!dataInterval.HasValue)
				return;

			if (filterType == FilterType.None) {
				maxSamplesLabel.Text = "1";
				filterLabel.Text = "0 (0ms)";
				return;
			}

			int tmpMax = (int)((chartTime * 1000) / dataInterval.Value);
			if (tmpMax > 1000)
				tmpMax = 1000;
			if (tmpMax < filterBar.Minimum)
				tmpMax = filterBar.Minimum;
			filterBar.Maximum = tmpMax;
			if (filterBar.Value > filterBar.Maximum)
				filterBar.Value = filterBar.Minimum;
			maxSamplesLabel.Text = tmpMax.ToString();
			filterLength = filterBar.Value;
			filterLabel.Text = filterLength.ToString() + " (" + (filterLength * dataInterval.Value).ToString() + "ms)";
			if (filterLength * dataInterval.Value > chartTime * 1000)
				filterWarningLabel.Text = " WARN: Filter longer than Chart Length. No data will be shown.";
			else
				filterWarningLabel.Text = "";
		}

		private void updateTimeBarMax() {

			TimeSpan maxTime;
			switch (chartType) {
			case ChartType.FFT:
				if (dataInterval == null) {
					maxTime = new TimeSpan(0, 1, 0); // 1 minute
				} else {
					// We can use at most 1024*16 samples
					int maxSeconds = (int)(dataInterval.Value * 1024 * 16 * 0.001);
					// Max is 5 minutes
					if (maxSeconds >= (60 * 5))
						maxSeconds = 60 * 5;
					maxTime = new TimeSpan(0, 0, maxSeconds);
				}
				break;
			case ChartType.AllanDeviation:
				maxTime = new TimeSpan(0, 30, 0); // 30 minutes
				break;
			case ChartType.DataPoints:
			default:
				if (dataInterval == null) {
					// Do data interval? probably a digital input, and we can probably support a fairly long interval
					maxTime = new TimeSpan(0, 5, 0); // 5 minutes
				} else {
					int maxSeconds = (int)(dataInterval.Value * 10); // 10,000 data points
					if (maxSeconds < 60)
						maxSeconds = 60; // but at least 60 seconds
										 // After 5 minutes round to nearest minute
					if (maxSeconds >= (60 * 5))
						maxSeconds = ((int)(maxSeconds / 60) * 60);
					// After 2 hours round to nearest hour
					if (maxSeconds >= (60 * 60 * 2))
						maxSeconds = ((int)(maxSeconds / (60 * 60)) * (60 * 60));
					maxTime = new TimeSpan(0, 0, maxSeconds);
				}
				break;
			}
			ChartTimeTrackbar.Maximum = (int)maxTime.TotalSeconds;
			maxTimeLabel.Text = maxTime.ToString();
			chartTime = ChartTimeTrackbar.Value;
			TimeSpan cTime = new TimeSpan(0, 0, (int)chartTime);
			ChartTimeLabel.Text = cTime.ToString();
		}

		private void trackBar1_Scroll(object sender, EventArgs e) {
			chartTime = ChartTimeTrackbar.Value;
			TimeSpan cTime = new TimeSpan(0, 0, (int)chartTime);
			ChartTimeLabel.Text = cTime.ToString();
			updateFilterBarMax();
		}

		private void ClearButton_Click(object sender, EventArgs e) {
			graph.Series[0].Points.Clear();
			allDataPoints.Clear();
			dataIntervalErrorCnt = 0;
			initialTimestamp = null;
			intervalChanged = 1;
			lock (accumulatorLock) {
				accumulateStart = null;
				measuredRate = null;
				dataPointAccumulator = 0;
				timeAccumulator = 0;
			}
			graph.ChartAreas[0].AxisX.IsLogarithmic = false;
			graph.ChartAreas[0].AxisY.IsLogarithmic = false;
			graph.BackColor = Color.White;
			badIntervalText.Visible = false;
		}

		private void SaveButton_Click(object sender, EventArgs e) {
			if (filename != "") {
				filename = "";
				fs.Close();
				SaveButton.Text = "Save to CSV";
				return;
			}

			// Displays a SaveFileDialog so the user can save the Image
			// assigned to Button2.
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "CSV file (.csv)|*.csv| All Files (.)|*.*";
			saveFileDialog1.Title = "Save to CSV File";
			saveFileDialog1.ShowDialog();

			Int64 lengthKB;

			// If the file name is not an empty string open it for saving.
			if (saveFileDialog1.FileName != "") {
				fs = new System.IO.StreamWriter(saveFileDialog1.OpenFile());
				filename = saveFileDialog1.FileName;
				FileNameLabel.Text = "File: " + filename;

				switch (chartType) {
				case ChartType.DataPoints:
					fs.WriteLine("Date," + this.Text);
					foreach (DataPoint dataPoint in allDataPoints) {
						DateTime pointTime = dataPoint.Date;
						fs.WriteLine(pointTime.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + delimiter + dataPoint.Data.ToString());
					}

					lengthKB = fs.BaseStream.Length / 1024;
					FileSizeLabel.Text = "File Size: " + lengthKB.ToString() + "kB";

					if (pause) { //Don't continue saving to a file if the data was paused
						filename = "";
						fs.Close();
					} else {
						SaveButton.Text = "Stop Saving";
					}
					break;

				case ChartType.FFT:
					fs.WriteLine("Frequency," + this.Text);
					foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint in graph.Series[0].Points) {
						fs.WriteLine(dataPoint.XValue.ToString() + "," + dataPoint.YValues[0].ToString());
					}

					lengthKB = fs.BaseStream.Length / 1024;
					FileSizeLabel.Text = "File Size: " + lengthKB.ToString() + "kB";

					fs.Close();
					filename = "";
					break;

				case ChartType.AllanDeviation:
					fs.WriteLine("Timespan (s)," + "Allan Deviation");
					foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint in graph.Series[0].Points) {
						fs.WriteLine(dataPoint.XValue.ToString() + "," + dataPoint.YValues[0].ToString());
					}

					lengthKB = fs.BaseStream.Length / 1024;
					FileSizeLabel.Text = "File Size: " + lengthKB.ToString() + "kB";

					fs.Close();
					filename = "";
					break;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			if (pause == false) {
				pause = true;
				PauseButton.Text = "Resume";
			} else {
				pause = false;
				addGap();
				dataIntervalErrorCnt = 0;
				initialTimestamp = null;
				intervalChanged = 1;
				lock (accumulatorLock) {
					accumulateStart = null;
					measuredRate = null;
					dataPointAccumulator = 0;
					timeAccumulator = 0;
				}
				PauseButton.Text = "Pause";
			}
		}

		private void chartTypeBox_SelectedIndexChanged(object sender, EventArgs e) {

			if (filename != "") {
				filename = "";
				fs.Close();
				SaveButton.Text = "Save to CSV";
				return;
			}

			chartType = (ChartType)Enum.Parse(typeof(ChartType), chartTypeBox.SelectedValue.ToString());
			switch (chartType) {
			case ChartType.DataPoints:
				graph.Series[0].Points.Clear();
				graph.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss tt";
				graph.ChartAreas[0].AxisX.Interval = 0;
				graph.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
				graph.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
				graph.ChartAreas[0].AxisX.Title = "Time";
				graph.ChartAreas[0].AxisY.Title = YTitle;
				graph.ChartAreas[0].AxisX.IsLogarithmic = false;
				graph.ChartAreas[0].AxisY.IsLogarithmic = false;
				graph.ChartAreas[0].AxisX.MinorGrid.Interval = 0;
				graph.ChartAreas[0].AxisY.MinorGrid.Interval = 0;
				graph.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
				graph.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
				graph.ChartAreas[0].AxisX.Minimum = Double.NaN;
				graph.ChartAreas[0].AxisX.Maximum = Double.NaN;
				graph.ChartAreas[0].AxisY.Minimum = Double.NaN;
				graph.ChartAreas[0].AxisY.Maximum = Double.NaN;
				graph.ChartAreas[0].InnerPlotPosition.Auto = false;
				graph.ChartAreas[0].InnerPlotPosition.Width = 93;
				graph.ChartAreas[0].InnerPlotPosition.X = 5;
				graph.ChartAreas[0].InnerPlotPosition.Height = 80;
				graph.ChartAreas[0].InnerPlotPosition.Y = 3;
				graph.ChartAreas[0].AxisY.LabelStyle.Format = "SI";
				if (pointsBox.Checked) {
					graph.Series[0].ChartType = SeriesChartType.FastPoint;
					graph.Series[0].MarkerStyle = MarkerStyle.Diamond;
				} else {
					graph.Series[0].ChartType = SeriesChartType.FastLine;
					graph.Series[0].MarkerStyle = MarkerStyle.None;
				}
				updateTimeBarMax();
				if (DataInterval.HasValue)
					filterPanel.Show();
				filterBar.Enabled = filterType != FilterType.None;
				break;

			case ChartType.FFT:
				graph.Series[0].Points.Clear();
				graph.Series[0].ChartType = SeriesChartType.Line;
				//graph.Series[0].LabelFormat = "{1}Hz";
				graph.ChartAreas[0].AxisX.LabelStyle.Format = "";
				graph.ChartAreas[0].AxisX.Interval = 0;
				graph.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
				graph.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
				graph.ChartAreas[0].AxisX.Title = "Frequency (Hz)";
				graph.ChartAreas[0].AxisY.Title = "";
				graph.ChartAreas[0].AxisX.IsLogarithmic = false;
				graph.ChartAreas[0].AxisY.IsLogarithmic = false;
				graph.ChartAreas[0].AxisX.MinorGrid.Interval = 0;
				graph.ChartAreas[0].AxisY.MinorGrid.Interval = 0;
				graph.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
				graph.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
				graph.ChartAreas[0].AxisX.Minimum = Double.NaN;
				graph.ChartAreas[0].AxisX.Maximum = Double.NaN;
				graph.ChartAreas[0].AxisY.Minimum = Double.NaN;
				graph.ChartAreas[0].AxisY.Maximum = Double.NaN;
				graph.ChartAreas[0].InnerPlotPosition.Auto = true;
				graph.ChartAreas[0].AxisY.LabelStyle.Format = "SI";

				updateTimeBarMax();
				filterPanel.Hide();
				filterTypeBox.SelectedValue = FilterType.None;
				break;

			case ChartType.AllanDeviation:
				graph.Series[0].Points.Clear();
				graph.Series[0].ChartType = SeriesChartType.Line;
				graph.ChartAreas[0].AxisX.LabelStyle.Format = "";
				graph.ChartAreas[0].AxisX.Interval = 0;
				graph.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet;
				graph.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
				graph.ChartAreas[0].AxisX.Title = "Time Period (s)";
				graph.ChartAreas[0].AxisY.Title = "Allan Standard Deviation";
				graph.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
				graph.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
				graph.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
				graph.ChartAreas[0].AxisY.MinorGrid.Interval = 2;
				graph.ChartAreas[0].InnerPlotPosition.Auto = true;
				graph.ChartAreas[0].AxisY.LabelStyle.Format = "SI";

				updateTimeBarMax();
				filterPanel.Hide();
				filterTypeBox.SelectedValue = FilterType.None;
				break;
			}
		}

		public class freqIntensity : IComparable<freqIntensity> {
			public double freq, intensity;
			public freqIntensity(double freq, double intensity) {
				this.freq = freq;
				this.intensity = intensity;
			}

			public int CompareTo(freqIntensity other) {
				return intensity.CompareTo(other.intensity);
			}
		}

		public void calculateAllanDeviation(double dataInterval) {
			double tau0 = dataInterval / 1000.0; //Calculate the sampling period
			int dataLength = allDataPoints.Count; //Calculate N
			List<double> dev = new List<double>(); //Create empty array to store the output.
			List<double> actualTau = new List<double>();

			if (dataLength < 5)
				return;

			double ms = 0;
			double highDiff = -1;
			double lowDiff = 1e6;
			for (int i = 1; i < allDataPoints.Count; i++) {
				double timeDiff = (allDataPoints[i].Date - allDataPoints[i - 1].Date).TotalMilliseconds;
				if (timeDiff > highDiff)
					highDiff = timeDiff;
				if (timeDiff < lowDiff)
					lowDiff = timeDiff;

				ms += timeDiff;
			}
			ms /= allDataPoints.Count - 1;
			Math.Round(ms);
			if (Math.Abs(dataInterval - ms) > 2) {
				if (Math.Abs(dataInterval - ms) > 2) {
					graph.BackColor = Color.LightCoral;
					badIntervalText.Visible = true;
				} else {
					graph.BackColor = Color.White;
					badIntervalText.Visible = false;
				}
				return;
			}

			int numDataPoints = dataLength / 2;
			if (numDataPoints > 25) {
				numDataPoints = 25;
			}

			int prevM = 0;
			for (int i = 0; i < numDataPoints; i++) {
				int m = (int)Math.Pow(dataLength / 2, (double)i / numDataPoints); //Calculate m given a tau value.
				if (m == 0)
					m = 1; //Use minimal m if tau is less than the sampling period.
				if (m > dataLength / 2)
					m = dataLength / 2;
				if (m == prevM)
					continue;
				double currentSum = 0; //Initialize the sum
				for (int j = 0; j < dataLength - (2 * m); j += m) {
					double localsum = 0;
					for (int k = j; k < j + m; k++) {
						localsum += allDataPoints[k + m].Data - allDataPoints[k].Data;
					}
					localsum /= m;
					localsum *= localsum;
					currentSum += localsum;
				}
				//Cumulate the sum squared
				double devAtThisTau = currentSum / (2 * ((double)dataLength / m) - 1);
				//Divide by the coefficient
				dev.Add(Math.Sqrt(devAtThisTau));
				actualTau.Add(m * tau0);
			}

			// Loop through data and do something with it
			graph.ChartAreas[0].AxisX.IsLogarithmic = false;
			graph.ChartAreas[0].AxisY.IsLogarithmic = false;
			graph.Series[0].Points.Clear();
			if (actualTau.Count() < 5)
				return;
			graph.ChartAreas[0].AxisX.IsLogarithmic = true;
			graph.ChartAreas[0].AxisY.IsLogarithmic = true;
			for (int i = 0; i < actualTau.Count; ++i) {
				// Calculate magnitude or do something with the new complex data
				graph.Series[0].Points.AddXY(actualTau[i], dev[i]);
			}

			if (actualTau[0] >= 10)
				graph.ChartAreas[0].AxisX.Minimum = 10;
			else if (actualTau[0] >= 1)
				graph.ChartAreas[0].AxisX.Minimum = 1;
			else if (actualTau[0] >= 0.1)
				graph.ChartAreas[0].AxisX.Minimum = 0.1;
			else if (actualTau[0] >= 0.01)
				graph.ChartAreas[0].AxisX.Minimum = 0.01;
			else
				graph.ChartAreas[0].AxisX.Minimum = 0.001;
			graph.ChartAreas[0].AxisX.Maximum = actualTau.Last();

			graph.ChartAreas[0].AxisX.Title = "Time Period (s)";
			graph.ChartAreas[0].AxisY.Title = "Allan Standard Deviation";
		}

		private void filterBar_Scroll(object sender, EventArgs e) {
			filterLength = filterBar.Value;
			if (dataInterval.HasValue)
				filterLabel.Text = filterLength.ToString() + " (" + (filterLength * dataInterval.Value).ToString() + "ms)";
			else
				filterLabel.Text = filterLength.ToString();
		}

		private void filterTypeBox_SelectedIndexChanged(object sender, EventArgs e) {

			FilterType prevFilter = filterType;
			filterType = (FilterType)Enum.Parse(typeof(FilterType), filterTypeBox.SelectedValue.ToString());

			switch (filterType) {
			case FilterType.None:
				if (prevFilter == FilterType.HPF) {
					graph.Series[0].Points.Clear();
					allDataPoints.Clear();
				}
				filterBar.Minimum = 1;
				minSamplesLabel.Text = filterBar.Minimum.ToString();
				filterBar.Enabled = false;
				filterLength = 0;
				break;

			case FilterType.LPF:
				if (prevFilter == FilterType.HPF) {
					graph.Series[0].Points.Clear();
					allDataPoints.Clear();
				}
				filterBar.Minimum = 1;
				minSamplesLabel.Text = filterBar.Minimum.ToString();
				filterLength = filterBar.Value;
				filterBar.Enabled = true;
				break;

			case FilterType.HPF:
				if (prevFilter == FilterType.LPF || prevFilter == FilterType.None) {
					graph.Series[0].Points.Clear();
					allDataPoints.Clear();
				}
				filterBar.Minimum = 2;
				minSamplesLabel.Text = filterBar.Minimum.ToString();
				filterLength = filterBar.Value;
				filterBar.Enabled = true;

				break;
			}
			updateFilterBarMax();
		}

		class GUIUpdateItem {
			public enum GUIUpdateType {
				data,
				dataInterval,
				dataGap
			};
			public GUIUpdateType type { get; set; }
			public double data { get; set; }
			public DateTime dataTime { get; set; }
		}

		ConcurrentQueue<GUIUpdateItem> guiUpdateQueue = new ConcurrentQueue<GUIUpdateItem>();

		private string ToSI(double d, string format = null) {
			char[] incPrefixes = new[] { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };
			char[] decPrefixes = new[] { 'm', '\u03bc', 'n', 'p', 'f', 'a', 'z', 'y' };

			double scaled = d;
			char? prefix = null;
			if (d != 0) {
				int degree = (int)Math.Floor(Math.Log10(Math.Abs(d)) / 3);
				scaled = d * Math.Pow(1000, -degree);

				switch (Math.Sign(degree)) {
				case 1:
					// NOTE: For big numbers, if the range is small, the SI scaling doesn't make much sense.
					return d.ToString(format);
				//prefix = incPrefixes[degree - 1];
				//break;
				case -1:
					prefix = decPrefixes[-degree - 1];
					break;
				}
			}

			return scaled.ToString(format) + prefix;
		}

		private void graph_FormatNumber(object sender, FormatNumberEventArgs e) {
			switch (e.Format) {
			case "SI":
				e.LocalizedValue = ToSI(e.Value, "G");
				break;
			}
		}

		private void pointsBox_CheckedChanged(object sender, EventArgs e) {
			if (pointsBox.Checked) {
				graph.Series[0].ChartType = SeriesChartType.FastPoint;
				graph.Series[0].MarkerStyle = MarkerStyle.Diamond;
			} else {
				graph.Series[0].ChartType = SeriesChartType.FastLine;
				graph.Series[0].MarkerStyle = MarkerStyle.None;
			}
		}
	}

	/// <summary>
	/// This class provides a high resolution clock by using the new API available in <c>Windows 8</c>/
	/// <c>Windows Server 2012</c> and higher. In all other operating systems it returns time by using
	/// a manually tuned and compensated <c>DateTime</c> which takes advantage of the high resolution
	/// available in <see cref="Stopwatch"/>.
	/// </summary>
	public sealed class PreciseClock {

		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
		private static extern void GetSystemTimePreciseAsFileTime(out long filetime);


		/// <summary>
		/// Gets the date and time in <c>UTC</c>.
		/// </summary>
		public static DateTime Now {
			get {
				long preciseTime;
				GetSystemTimePreciseAsFileTime(out preciseTime);
				return DateTime.FromFileTimeUtc(preciseTime).ToLocalTime();
			}
		}

		public static String TimestampForFilename {
			get {
				return Now.ToString("yyyy'-'MM'-'dd'T'HH'_'mm'_'ss'.'fffzz");
			}
		}
	}
}
