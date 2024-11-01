using Phidget22;
using Phidget22.Events;
using Phidget22.ExampleUtils;
using System;
using System.Linq;
using System.Windows.Forms;

namespace VoltageRatio_Example {
	public partial class Form1 : Form {
		CommandLineOpen openArgs = null;
		VoltageRatioInput openCh = null;
		formCleaner cleaner;
		VoltageRatioInput ratio = null;
		private ErrorEventBox errorBox;
		private DataGraphBox voltageGraphBox, sensorGraphBox, weightGraphBox;
		private userCalibration userCal;
		double gain, offset;
		bool gainStatus, offsetStatus = false;

		FastEventHandler<VoltageRatioInputSensorChangeEventArgs> sensorEventHandler;
		FastEventHandler<VoltageRatioInputVoltageRatioChangeEventArgs> voltageRatioEventHandler;

		public Form1() {
			InitializeComponent();
			openArgs = new CommandLineOpen(this);
			cleaner = new formCleaner(this);
		}

		public Form1(VoltageRatioInput ch) {
			InitializeComponent();
			openCh = ch;
			cleaner = new formCleaner(this);
		}

		private void Form1_Load(object sender, EventArgs e) {
			errorBox = new ErrorEventBox(this);

			sensorTypeCmb.DataSource = typeof(VoltageRatioSensorType).ToList();

			ratio = openCh ?? openArgs.makeChannel<VoltageRatioInput>();

			ratio.Attach += ratio_attach;
			ratio.Detach += ratio_detach;
			ratio.Error += ratio_error;
			ratio.PropertyChange += Ratio_PropertyChange;

			// This causes the events to get handled without invoke
			sensorEventHandler = new FastEventHandler<VoltageRatioInputSensorChangeEventArgs>();
			ratio.SensorChange += sensorEventHandler.Handler;
			sensorEventHandler.Event += Ratio_SensorChange;

			voltageRatioEventHandler = new FastEventHandler<VoltageRatioInputVoltageRatioChangeEventArgs>();
			ratio.VoltageRatioChange += voltageRatioEventHandler.Handler;
			voltageRatioEventHandler.Event += ratio_change;

			try {
				ratio.Open(); //open the device specified by the above parameters
			} catch (PhidgetException ex) { errorBox.addMessage("Error opening channel: " + ex.Message); }

			voltageGraphBox = new DataGraphBox(this, DataGraphBox.GenerateTitle("Voltage Ratio", (Phidget)ratio), "Ratio (V/V)");
			sensorGraphBox = new DataGraphBox(this, DataGraphBox.GenerateTitle("Sensor Value", (Phidget)ratio));
			weightGraphBox = new DataGraphBox(this, DataGraphBox.GenerateTitle("Weight", (Phidget)ratio), "Weight (Calibration Units)");
			userCal = new userCalibration(errorBox, ratio, this);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			ratio.Attach -= ratio_attach;
			ratio.Detach -= ratio_detach;
			ratio.Error -= ratio_error;
			ratio.PropertyChange -= Ratio_PropertyChange;
			ratio.SensorChange -= sensorEventHandler.Handler;
			ratio.VoltageRatioChange -= voltageRatioEventHandler.Handler;
			ratio.Close();
		}

		private void Ratio_PropertyChange(object sender, PropertyChangeEventArgs e) {
			if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
				switch (e.PropertyName) {
				case "DataInterval":
					dataIntervalTrk.Value = ratio.DataInterval;
					updateGraphDataInterval();
					break;
				case "MinDataInterval":
					dataIntervalTrk.initializeRange(ratio.MinDataInterval, ratio.MaxDataInterval);
					dataIntervalTrk.Value = ratio.DataInterval;
					updateGraphDataInterval();
					break;
				}
			} else if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
				switch (e.PropertyName) {
				case "DataRate":
					dataIntervalTrk.Value = ratio.DataRate;
					updateGraphDataInterval();
					break;
				case "MinDataRate":
					dataIntervalTrk.initializeRange(ratio.MinDataRate, ratio.MaxDataRate);
					dataIntervalTrk.Value = ratio.DataRate;
					updateGraphDataInterval();
					break;
				}
			}
		}

		void ratio_attach(object sender, Phidget22.Events.AttachEventArgs e) {
			settingsBox.Visible = true;
			outputBox.Visible = true;

			phidgetInfoBox1.FillPhidgetInfo((Phidget)sender);

			voltageGraphBox.SetTitle(DataGraphBox.GenerateTitle("Voltage Ratio", (Phidget)ratio));
			sensorGraphBox.SetTitle(DataGraphBox.GenerateTitle("Sensor Value", (Phidget)ratio));
			weightGraphBox.SetTitle(DataGraphBox.GenerateTitle("Weight", (Phidget)ratio));

			VoltageRatioInput attachedDevice = (VoltageRatioInput)sender;

			dataIntervalTrk.Unit = "ms";
			dataIntervalTrk.initializeRange(attachedDevice.MinDataInterval, attachedDevice.MaxDataInterval);
			dataIntervalTrk.Value = attachedDevice.DataInterval;

			//Settings specific to a channel subclass
			switch (attachedDevice.ChannelSubclass) {
			case ChannelSubclass.VoltageRatioInputBridge:
				bridgeSettings.Visible = true;
				calibrationBox.Visible = true;
				urlTip.SetToolTip(linkLabel1, Properties.Settings.Default.calibrationURL);
				//not all gain selections are valid for all bridge devices
				gainCombo.SelectedIndexChanged -= gainCombo_SelectedIndexChanged;

				switch (attachedDevice.DeviceID) {
				case DeviceID.PN_1046: {
					Enum[] supportedGains = new Enum[] {BridgeGain.Gain_1x,
															BridgeGain.Gain_8x,
															BridgeGain.Gain_16x,
															BridgeGain.Gain_32x,
															BridgeGain.Gain_64x,
															BridgeGain.Gain_128x};
					gainCombo.DataSource = supportedGains.ToList();
					break;
				}
				case DeviceID.PN_DAQ1500: {
					Enum[] supportedGains = new Enum[] {BridgeGain.Gain_1x,
															BridgeGain.Gain_2x,
															BridgeGain.Gain_64x,
															BridgeGain.Gain_128x};
					gainCombo.DataSource = supportedGains.ToList();
					break;
				}
				default:
					errorBox.addMessage("Unexpected device. update example.");
					break;
				}

				gainCombo.SelectedValue = attachedDevice.BridgeGain;
				gainCombo.SelectedIndexChanged += this.gainCombo_SelectedIndexChanged;

				enabledChk.Visible = true;
				enabledChk.CheckedChanged -= enabledChk_CheckedChanged;
				enabledChk.Checked = attachedDevice.BridgeEnabled;
				enabledChk.CheckedChanged += enabledChk_CheckedChanged;

				bridge_gain_panel.Show();
				sensor_type_panel.Hide();
				sensor_value_panel.Hide();
				break;
			default:
				try {
					bridgeSettings.Visible = false;
					calibrationBox.Visible = false;
				} catch (PhidgetException ex) { errorBox.addMessage("Error initializing device: " + ex.Message); }

				bridge_gain_panel.Hide();
				if (attachedDevice.ChannelSubclass == ChannelSubclass.VoltageRatioInputSensorPort) {
					sensor_value_panel.Visible = true;
					sensor_type_panel.Visible = true;
				}
				break;
			}

			try {
				changeTriggerTrk.Unit = "V/V";
				changeTriggerTrk.minTxt = attachedDevice.MinVoltageRatioChangeTrigger.ToString();
				changeTriggerTrk.Minimum = attachedDevice.MinVoltageRatioChangeTrigger;
				changeTriggerTrk.Maximum = attachedDevice.MaxVoltageRatioChangeTrigger;
				changeTriggerTrk.maxTxt = attachedDevice.MaxVoltageRatioChangeTrigger.ToString();
				changeTriggerTrk.Value = attachedDevice.VoltageRatioChangeTrigger;
			} catch (PhidgetException ex) { errorBox.addMessage("Error initializing device: " + ex.Message); }

			updateGraphDataInterval();

			guiUpdateTimer.Start();
		}

		void ratio_detach(object sender, Phidget22.Events.DetachEventArgs e) {
			guiUpdateTimer.Stop();
			_voltageTxt = "";
			_sensorTxt = "";

			phidgetInfoBox1.Clear();
			cleaner.clean();

			bridge_gain_panel.Hide();
			enabledChk.Visible = false;
			settingsBox.Visible = false;
			outputBox.Visible = false;
			calibrationBox.Visible = false;
			dataIntervalTrk.TrueValue = -1;
		}

		void ratio_error(object sender, Phidget22.Events.ErrorEventArgs e) {
			errorBox.addMessage(e.Description);

			if (e.Code == ErrorEventCode.Saturation || e.Code == ErrorEventCode.OutOfRange) {
				if (ratio.ChannelSubclass == ChannelSubclass.VoltageRatioInputSensorPort && ratio.SensorType != VoltageRatioSensorType.VoltageRatio)
					sensorGraphBox.addGap();
				else
					voltageGraphBox.addGap();
			}
		}

		private string _voltageTxt = "";
		private string _sensorTxt = "";
		private string _weightTxt = "";
		private void guiUpdateTimer_Tick(object sender, EventArgs e) {
			voltageTxt.Text = _voltageTxt;
			sensorTxt.Text = _sensorTxt;
			weightTxt.Text = _weightTxt;
			calibrationPanel.Visible = (gainStatus & offsetStatus);
		}

		void ratio_change(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e) {
			if (ratio.ChannelSubclass == ChannelSubclass.VoltageRatioInputBridge) {
				_voltageTxt = e.VoltageRatio.ToString("E3") + " V/V";
				if (gainStatus & offsetStatus == true) {
					_weightTxt = ((e.VoltageRatio + offset) * gain).ToString("F3");
				}
			} else
				_voltageTxt = e.VoltageRatio.ToString("F4") + " V/V";

			voltageGraphBox.addData(e.VoltageRatio);
			if (gainStatus & offsetStatus == true) {
				weightGraphBox.addData((e.VoltageRatio + offset) * gain);
			}
		}

		private void Ratio_SensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e) {
			_sensorTxt = e.SensorValue.ToString() + " " + e.SensorUnit.Symbol;
			sensorGraphBox.addData(e.SensorValue);
		}

		private void updateGraphDataInterval() {
			try {
				if (ratio.VoltageRatioChangeTrigger == 0) {
					voltageGraphBox.DataInterval = (1000.0 / ratio.DataRate);
					weightGraphBox.DataInterval = (1000.0 / ratio.DataRate);
				} else
					voltageGraphBox.DataInterval = null;
				if (ratio.ChannelSubclass == ChannelSubclass.VoltageRatioInputSensorPort) {
					if (ratio.SensorValueChangeTrigger == 0)
						sensorGraphBox.DataInterval = (1000.0 / ratio.DataRate);
					else
						sensorGraphBox.DataInterval = null;
				}
			} catch (PhidgetException ex) { errorBox.addMessage("Error updating data interval: " + ex.Message); }
		}

		private void enabledChk_CheckedChanged(object sender, EventArgs e) {
			try {
				ratio.BridgeEnabled = enabledChk.Checked;
			} catch (PhidgetException ex) { errorBox.addMessage("Error enabling: " + ex.Message); }
		}

		private void gainCombo_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				ratio.BridgeGain = (BridgeGain)Enum.Parse(typeof(BridgeGain), gainCombo.SelectedValue.ToString());
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting gain: " + ex.Message);
			}
		}

		private void sensorTypeCmb_SelectedIndexChanged(object sender, EventArgs e) {

			if (ratio == null)
				return;

			try {
				ratio.SensorType = (VoltageRatioSensorType)Enum.Parse(typeof(VoltageRatioSensorType), sensorTypeCmb.SelectedValue.ToString());
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting sensor type: " + ex.Message);
			}
			if (sensorTypeCmb.SelectedIndex == 0) {
				panel1.Enabled = true; //standard voltage ratio events (i.e. generic sensor type selected)
				sensor_value_panel.Enabled = false;
			} else {
				panel1.Enabled = false; //specific event for sensor type (this disables normal events)
				sensor_value_panel.Enabled = true;
				if (ratio.SensorUnit.Unit == Unit.None)
					sensorGraphBox.SetAxisYTitle("");
				else
					sensorGraphBox.SetAxisYTitle(ratio.SensorUnit.Name + " (" + ratio.SensorUnit.Symbol + ")");
			}
		}
		private void voltageGraphButton_Click(object sender, EventArgs e) {
			if (!voltageGraphBox.Visible)
				voltageGraphBox.Show();
		}

		private void sensorGraphButton_Click(object sender, EventArgs e) {
			if (!sensorGraphBox.Visible)
				sensorGraphBox.Show();
		}

		private void tareBtn_Click(object sender, EventArgs e) {
			disableAll(true);
			offset = 0;
			int numSamplesToCollect = 0;
			if (1 <= ratio.DataInterval && ratio.DataInterval <= 50) {
				numSamplesToCollect = 16;
			} else if (51 <= ratio.DataInterval && ratio.DataInterval <= 500) {
				numSamplesToCollect = 8;
			} else if (501 <= ratio.DataInterval && ratio.DataInterval <= 1000) {
				numSamplesToCollect = 3;
			} else if (ratio.DataInterval > 1000) {
				errorBox.addMessage("The zeroing process requires a significant amount of data in order to be accurate.  You currently have a data interval/rate selected that will making gathering this data very slow.  We recommend you adjust the data interval/rate to something faster (<1000ms/1Hz) and then try zeroing.");
			}
			/*int temp = ratio.DataInterval;
			ratio.DataInterval = ratio.MinDataInterval;*/
			int i = 0;
			for (i = 0; i < numSamplesToCollect; i++) {
				offset -= ratio.VoltageRatio;
				System.Threading.Thread.Sleep(ratio.DataInterval);
			}
			offset /= i;
			offsetTxt.Text = (offset).ToString("E4");
			offsetStatus = true;
			//ratio.DataInterval = temp;
			disableAll(false);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(Properties.Settings.Default.calibrationURL);
			linkLabel1.LinkVisited = true;
		}

		private void gainSet_Click(object sender, EventArgs e) {
			setGain();
		}

		private void gainTxt_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				setGain();
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void cpyBtn_Click(object sender, EventArgs e) {
			System.Windows.Forms.Clipboard.SetText(gain.ToString());
		}

		private void setGain() {
			gainStatus = false;
			try {
				gain = double.Parse(gainTxt.Text);
				gainStatus = true;
			} catch (Exception ex) { errorBox.addMessage(ex.Message); }
		}


		private void weightGraphBtn_Click(object sender, EventArgs e) {
			if (!weightGraphBox.Visible)
				weightGraphBox.Show();
		}



		private void calibrateBtn_Click(object sender, EventArgs e) {
			gainStatus = false;
			offsetStatus = false;
			var result = userCal.ShowDialog();
			if (result == DialogResult.OK) {
				gain = userCal.gain;
				offset = userCal.offset;
				gainTxt.Text = gain.ToString("E4");
				offsetTxt.Text = offset.ToString("E4");
				gainStatus = true;
				offsetStatus = true;
			}
		}


		private void disableAll(bool mode) { //temporarily disable calibration box while collecting samples to average for tare operation
			if (mode == true) {
				foreach (Control control in calibrationBox.Controls)
					control.Enabled = false;
			} else {
				foreach (Control control in calibrationBox.Controls)
					control.Enabled = true;
			}
		}


		private void dataIntervalTrk_KeyOverride(object sender, KeyEventArgs e) {
			e.SuppressKeyPress = true;
			dataIntervalTrk.keyOverride(sender, e);
		}

		private void slider_ValueChangeEvent(object sender, EventArgs e) {
			if (object.ReferenceEquals(sender, dataIntervalTrk)) //datainterval control
			{
				if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
					try {
						double temp = dataIntervalTrk.Value;
						ratio.DataInterval = (int)temp;
						if (ratio.DataInterval < (int)temp) {
							while (ratio.DataInterval != (int)temp) {
								temp++;
								ratio.DataInterval = (int)temp;
							}
						} else {
							while (ratio.DataInterval != (int)temp) {
								temp--;
								ratio.DataInterval = (int)temp;
							}
						}

						dataIntervalTrk.TrueValue = ratio.DataInterval;
						dataIntervalTrk.Value = ratio.DataInterval;

						updateGraphDataInterval();
					} catch (Exception ex) { errorBox.addMessage("Error setting data interval: " + ex.Message); }
				} else if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
					try {
						ratio.DataRate = dataIntervalTrk.Value;
						updateGraphDataInterval();
					} catch (Exception ex) { errorBox.addMessage("Error setting data rate: " + ex.Message); }
				}
			} else //changetrigger control
			  {
				try {
					ratio.VoltageRatioChangeTrigger = changeTriggerTrk.Value;
					updateGraphDataInterval();
				} catch (Exception ex) { errorBox.addMessage("Error setting change trigger: " + ex.Message); }
			}
		}
		private void dataIntervalTrk_ModeSwitch(object sender, EventArgs e) {
			dataIntervalTrk.SwitchModes();
			if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
				try {
					dataIntervalTrk.isIntValue = false;
					dataIntervalTrk.initializeRange(1000.0 / ratio.MaxDataRate, 1000.0 / ratio.MinDataRate);
					dataIntervalTrk.Unit = "Hz";
					dataIntervalTrk.trkText = ratio.DataRate.ToString() + "Hz";
					dataIntervalTrk.Value = ratio.DataRate;
				} catch (Exception ex) { errorBox.addMessage("Error setting data rate: " + ex.Message); }

			} else if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
				try {
					dataIntervalTrk.isIntValue = true;
					dataIntervalTrk.initializeRange(ratio.MinDataInterval, ratio.MaxDataInterval);
					if (ratio.DeviceID != DeviceID.PN_1046)
						dataIntervalTrk.Unit = "ms";
					dataIntervalTrk.trkText = ratio.DataInterval.ToString() + "ms";
					dataIntervalTrk.Value = ratio.DataInterval;
				} catch (Exception ex) { errorBox.addMessage("Error setting data interval: " + ex.Message); }
			}
			dataIntervalTrk.TrueValue = dataIntervalTrk.Value;
		}

	}
}