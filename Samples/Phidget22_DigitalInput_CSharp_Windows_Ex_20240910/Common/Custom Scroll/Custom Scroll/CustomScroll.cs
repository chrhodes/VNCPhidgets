using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace PhidgetsControlLibrary {

	[DefaultEvent("ValueChange")]
	public partial class CustomScroll : UserControl {

		public enum Modes { RATE, INTERVAL }
		double minValue = -1; //min slider value
		double maxValue = -1; //max slider value
		double deviceMin = -1; //min hardware supported range
		double deviceMax = -1; //max hardware supported range
		double sliderValue = -1; //value of the slider
		SortedList<int, double> dataIntervalValues = new SortedList<int, double>();
		bool multimode = false;
		bool keyoverride = false; //to detect if we are taking manual arrow key input on slider
		Modes _mode = Modes.INTERVAL;
		double firmwareValue = -1; //value as returned by property update events
		double logBase = 0.00000005;

		double[] rangeSuperset = new double[20] { 1, 2, 5, 10, 25, 50, 100, 200, 250, 500, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800 };

		public CustomScroll() {
			InitializeComponent();
			toolTip2.SetToolTip(tooltipPic, "Data rate can change based on performance of the device or remote connections from other programs.  \nThe red value is the true rate returned from the hardware.");
			toolTip2.AutoPopDelay = 30000;
			Unit = "";
		}

		public event EventHandler LogScroll {
			add { slider.Scroll += value; }
			remove { slider.Scroll -= value; }
		}

		public event MouseEventHandler LogMouseUp {
			add { slider.MouseUp += value; }
			remove { slider.MouseUp -= value; }
		}

		public event EventHandler SetClick {
			add { setBtn.Click += value; }
			remove { setBtn.Click -= value; }
		}

		public event KeyEventHandler SetEnter {
			add { trkTxt.KeyDown += value; }
			remove { trkTxt.KeyDown -= value; }
		}

		public event EventHandler ModeSwitch {
			add { trkName.Click += value; }
			remove { trkName.Click -= value; }
		}

		public event KeyEventHandler KeyOverride {
			add { slider.KeyDown += value; }
			remove { slider.KeyDown -= value; }
		}

		public event EventHandler ValueChange;

		#region Properties

		public double Minimum {
			get {
				return this.minValue;
			}
			set {
				slider.Minimum = 0;
				this.minValue = value;
			}
		}

		public double Maximum {
			get {
				return this.maxValue;
			}
			set { //slider should have 100 positions that range between minValue and maxValue
				slider.Maximum = 99;
				this.maxValue = value;
			}
		}

		public double TrueValue {
			get {
				return this.firmwareValue;
			}
			set {
				this.firmwareValue = value;
				updateTxt();
			}
		}

		public double LogBase {
			get {
				return this.logBase;
			}
			set {
				this.logBase = value;
			}
		}

		public bool isIntValue { get; set; }

		public bool isFixedRange { get; set; }

		public bool multiMode {
			get {
				return multimode;
			}
			set {
				multimode = value;
				if (value == true) {
					trkName.ForeColor = Color.Blue;
					trkName.Font = new Font(trkName.Font, FontStyle.Underline);
					toolTip1.SetToolTip(trkName, "Click to change mode between Data Interval and Data Rate.");
				}
			}
		}

		public string Unit { get; set; }

		public string labelTxt {
			get {
				return trkName.Text;
			}
			set {
				trkName.Text = value;
			}
		}

		public string minTxt {
			get {
				return minLbl.Text;
			}
			set {
				minLbl.Text = value;
			}
		}

		public string maxTxt {
			get {
				return maxLbl.Text;
			}
			set {
				maxLbl.Text = value;
			}
		}

		public string trkText {
			get {
				return trkTxt.Text;
			}
			set {
				trkTxt.Text = value;
			}
		}

		public double Value {
			get {
				return this.sliderValue;
			}
			set {
				if (isFixedRange == false) {
					if (value < minValue)
						value = minValue;

					if (value > maxValue)
						value = maxValue;

					sliderValue = value;
					updateSliderFromValue();
					updateTxt();
				} else {
					if (_mode == Modes.INTERVAL) {
						if (value < deviceMin)
							value = deviceMin;
						if (value > deviceMax)
							value = deviceMax;
					} else if (_mode == Modes.RATE) {
						if (value < (1000 / deviceMax))
							value = 1000 / deviceMax;
						if (value > (1000 / deviceMin))
							value = 1000 / deviceMin;
					}
					sliderValue = value;
					updateSliderFromValue();
					updateTxt();
				}
			}
		}

		public Modes TimingMode {
			get {
				return this._mode;
			}
			set {
				_mode = value;
				firmwareValue = sliderValue;
				initializeRange(deviceMin, deviceMax);
				updateTxt();

			}
		}

		public void initializeRange(double min, double max) {
			int closestIndex = 0;
			dataIntervalValues.Clear(); //clear old data from list
			for (int k = 0; k < rangeSuperset.Length; k++) { //find index in range superset that is closest to minimum value of device
				if (Math.Abs(rangeSuperset[k] - min) < Math.Abs(rangeSuperset[closestIndex] - min))
					closestIndex = k;
			}

			slider.Minimum = 0;
			dataIntervalValues.Add(1, min); //always start the slider at the minimum interval for the device
			int i = 1;
			while (i < 10 && rangeSuperset[closestIndex + i] < max) { //populate the possible values with the appropriate subset of the range superset.
				dataIntervalValues.Add(i + 1, rangeSuperset[closestIndex + i]);
				i++;
			}
			dataIntervalValues.Add(i + 1, rangeSuperset[closestIndex + i]);
			slider.Maximum = dataIntervalValues.Count - 1; //set the maximum slider value to the number of elements in dataintervalvalues list
			minValue = min;
			maxValue = rangeSuperset[closestIndex + i];
			//set ui elements
			minLbl.Text = minValue.ToString();
			maxLbl.Text = maxValue.ToString();

			if (_mode == Modes.RATE) //if it's in rate mode we should reverse and invert the list
			{
				//need to reverse the order now
				SortedList<int, double> temp = new SortedList<int, double>();
				for (int j = 0; j < dataIntervalValues.Count; j++) {
					temp.Add(dataIntervalValues.Count - j, dataIntervalValues[j + 1]);
				}

				dataIntervalValues = temp;

				//then invert
				SortedList<int, double> ts = new SortedList<int, double>();
				foreach (KeyValuePair<int, double> kvp in dataIntervalValues) {
					ts.Add(kvp.Key, 1000.0 / kvp.Value);
				}

				dataIntervalValues = ts;
				//set UI stuff
				slider.Maximum = dataIntervalValues.Count - 1;
				minValue = 1000.0 / rangeSuperset[closestIndex + i];
				maxValue = 1000.0 / min;
				if (minValue < 1)
					minLbl.Text = minValue.ToString("F3");
				else
					minLbl.Text = minValue.ToString();
				maxLbl.Text = maxValue.ToString();
			}
			deviceMin = min;
			deviceMax = max;
		}

		private void updateTxt() {

			tooltipPic.Visible = false;
			firmwareValueLbl.Visible = false;
			if (!isIntValue) {
				if (maxValue <= 5)
					trkTxt.Text = sliderValue.ToString("F3") + Unit;
				else
					trkTxt.Text = sliderValue.ToString("F2") + Unit;
			} else {
				if (maxValue <= 5)
					trkTxt.Text = sliderValue.ToString("F0") + Unit;
				else
					trkTxt.Text = sliderValue.ToString("F0") + Unit;
			}
			if (Math.Abs(firmwareValue - sliderValue) > 1 && firmwareValue != -1) {
				if (!isIntValue) {
					if (maxValue <= 5)
						trkTxt.Text = firmwareValue.ToString("F3") + Unit;
					else
						trkTxt.Text = firmwareValue.ToString("F2") + Unit;
				} else {
					if (maxValue <= 5)
						trkTxt.Text = firmwareValue.ToString("F0") + Unit;
					else
						trkTxt.Text = firmwareValue.ToString("F0") + Unit;
				}

				firmwareValueLbl.Text = sliderValue.ToString("F1");
				tooltipPic.Visible = true;
				firmwareValueLbl.Visible = true;
			}
			if (_mode == Modes.INTERVAL) {
				if (deviceMax != -1 && deviceMin != -1)
					toolTip3.SetToolTip(trkTxt, deviceMin.ToString() + " - " + deviceMax.ToString() + Unit);
				else
					toolTip3.SetToolTip(trkTxt, Minimum.ToString() + " - " + Maximum.ToString() + Unit);
			} else {
				if (deviceMax != -1 && deviceMin != -1)
					toolTip3.SetToolTip(trkTxt, (1000 / deviceMax).ToString() + " - " + (1000 / deviceMin).ToString() + Unit);
				else
					toolTip3.SetToolTip(trkTxt, (1000 / Maximum).ToString() + " - " + (1000 / Minimum).ToString() + Unit);
			}

		}

		bool _updatingSlider = false;
		private void updateValueFromSlider() {
			if (isFixedRange == false) {
				if (minValue != maxValue)
					sliderValue = Math.Log(((slider.Value) * (logBase - 1)) / slider.Maximum + 1, logBase) * (maxValue - minValue) + minValue;
			} else if (minValue != maxValue && dataIntervalValues.Count > 0)
				sliderValue = dataIntervalValues[slider.Value + 1];
		}

		private void updateSliderFromValue() {
			_updatingSlider = true;
			if (isFixedRange == false) {
				if (minValue != maxValue)
					slider.Value = (int)((slider.Maximum * (Math.Pow(logBase, -(sliderValue - minValue) / (minValue - maxValue)) - 1)) / (logBase - 1));
			} else if (isFixedRange == true) {
				if (minValue != maxValue && dataIntervalValues.Count > 0) {
					double closest = (double)dataIntervalValues[1];
					int index = -1;
					for (int i = 1; i <= dataIntervalValues.Count; i++) {
						if (Math.Abs(dataIntervalValues[i] - sliderValue) <= Math.Abs(closest - sliderValue)) {
							closest = (double)dataIntervalValues[i];
							index = i;
						}
					}
					if (keyoverride == false) {
						slider.Value = index - 1;
					} else {
						if (index == dataIntervalValues.Count)
							slider.Value = index - 1;
						else if (Math.Abs(closest - sliderValue) < Math.Abs((double)dataIntervalValues[index + 1] - sliderValue))
							slider.Value = index - 1;
						updateTxt();
					}
				}
			}
			_updatingSlider = false;
		}


		#endregion

		private void slider_Scroll(object sender, EventArgs e) {

			if (_updatingSlider || keyoverride) {
				keyoverride = false;
				return;
			}
			updateValueFromSlider();
			this.firmwareValue = sliderValue;
			updateTxt();
			ValueChange?.Invoke(this, EventArgs.Empty);
		}

		private void setBtn_Click(object sender, EventArgs e) {
			try {
				this.Value = double.Parse(Regex.Replace(trkTxt.Text, "[^0-9.]", ""));
			} catch (Exception) {
				this.trkTxt.Text = "N/A";
			}
			firmwareValue = sliderValue;
			updateTxt();
			ValueChange?.Invoke(this, EventArgs.Empty);
		}

		private void trkTxt_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				try {
					this.Value = double.Parse(Regex.Replace(trkTxt.Text, "[^0-9.]", ""));
					e.Handled = true;
					e.SuppressKeyPress = true;
				} catch (Exception) {
					this.trkTxt.Text = "N/A";
				}
				firmwareValue = sliderValue;
				updateTxt();
				ValueChange?.Invoke(this, EventArgs.Empty);
			}
		}

		private void trkTxt_MouseClick(object sender, MouseEventArgs e) {
			trkTxt.SelectAll();
		}

		public void SwitchModes() {
			if (multimode) {
				if (trkName.Text == "Data Interval:") {
					trkName.Text = "Data Rate:";
					_mode = Modes.RATE;
					initializeRange(deviceMin, deviceMax);
					sliderValue = 1000.0 / (sliderValue);
					Unit = "Hz";
				} else {
					trkName.Text = "Data Interval:";
					_mode = Modes.INTERVAL;
					initializeRange(deviceMin, deviceMax);
					sliderValue = 1000.0 / (sliderValue);
					Unit = "ms";
				}
			}
			firmwareValue = sliderValue;
			if (deviceMax != -1 && deviceMin != -1)
				toolTip3.SetToolTip(trkTxt, deviceMin.ToString() + " - " + deviceMax.ToString() + Unit);
			else
				toolTip3.SetToolTip(trkTxt, Minimum.ToString() + " - " + Maximum.ToString() + Unit);
			updateTxt();
		}

		public void keyOverride(object sender, KeyEventArgs e) {
			if (_mode == Modes.INTERVAL) {
				if (e.KeyCode == Keys.Left) {
					if (sliderValue <= Minimum)
						return;
					keyoverride = true;
					sliderValue = sliderValue - 1; ;
				}
				if (e.KeyCode == Keys.Right) {
					if (deviceMax == -1) {
						if (sliderValue >= Maximum)
							return;
					} else {
						if (sliderValue >= deviceMax)
							return;
					}
					keyoverride = true;
					sliderValue = sliderValue + 1;
				}
			} else {
				if (e.KeyCode == Keys.Left) {
					if (sliderValue <= Math.Ceiling(1000.0 / deviceMax)) {
						sliderValue = dataIntervalValues[1];
					} else {
						keyoverride = true;
						sliderValue = sliderValue - 1; ;
					}
				}
				if (e.KeyCode == Keys.Right) {
					if (deviceMax == -1) {
						if (sliderValue >= Maximum)
							return;
					} else {
						if (sliderValue >= 1000.0 / deviceMin)
							return;
					}
					keyoverride = true;
					if (sliderValue < 1)
						sliderValue = 1;
					else
						sliderValue = sliderValue + 1;
				}
			}

			firmwareValue = sliderValue;
			updateSliderFromValue();
			updateTxt();
			ValueChange?.Invoke(this, EventArgs.Empty);
		}

		private void slider_MouseUp(object sender, MouseEventArgs e) {
			keyoverride = false;
			updateValueFromSlider();
			firmwareValue = sliderValue;
			updateTxt();
			ValueChange?.Invoke(this, EventArgs.Empty);
		}

		private void slider_KeyUp(object sender, KeyEventArgs e) {
			keyoverride = false;
		}
	}
}
