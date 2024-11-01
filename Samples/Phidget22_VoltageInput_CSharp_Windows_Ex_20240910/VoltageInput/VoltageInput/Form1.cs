using Phidget22;
using Phidget22.Events;
using Phidget22.ExampleUtils;
using System;
using System.Linq;
using System.Windows.Forms;

namespace VoltageInput_Example {
	public partial class Form1 : Form {
		CommandLineOpen openArgs;
		VoltageInput openCh = null;
		formCleaner cleaner;
		VoltageInput vin = null;
		private ErrorEventBox errorBox;
		private DataGraphBox voltageGraphBox;

		private DataGraphBox sensorGraphBox;

		FastEventHandler<VoltageInputSensorChangeEventArgs> sensorEventHandler;
		FastEventHandler<VoltageInputVoltageChangeEventArgs> voltageRatioEventHandler;

		public Form1() {
			InitializeComponent();
			openArgs = new CommandLineOpen(this);
			cleaner = new formCleaner(this);
		}

		public Form1(VoltageInput ch) {
			InitializeComponent();
			openCh = ch;
			cleaner = new formCleaner(this);
		}

		protected override bool ProcessDialogKey(Keys keyData) {
			if (keyData == (Keys.W | Keys.Control)) {
				this.Close();
				return true;
			} else
				return base.ProcessDialogKey(keyData);
		}

		private void Form1_Load(object sender, EventArgs e) {
			Clear();

			errorBox = new ErrorEventBox(this);
			vin = openCh ?? openArgs.makeChannel<VoltageInput>();

			vin.Attach += vin_Attach;
			vin.Detach += vin_Detach;
			vin.Error += vin_Error;
			vin.PropertyChange += Vin_PropertyChange;

			// This causes the events to get handled without invoke
			sensorEventHandler = new FastEventHandler<VoltageInputSensorChangeEventArgs>();
			vin.SensorChange += sensorEventHandler.Handler;
			sensorEventHandler.Event += Vin_SensorChange;
			voltageRatioEventHandler = new FastEventHandler<VoltageInputVoltageChangeEventArgs>();
			vin.VoltageChange += voltageRatioEventHandler.Handler;
			voltageRatioEventHandler.Event += vin_VoltageChange;

			try {
				vin.Open();
			} catch (PhidgetException ex) { errorBox.addMessage("Error opening device: " + ex.Message); }

			voltageGraphBox = new DataGraphBox(this, DataGraphBox.GenerateTitle("Voltage", (Phidget)vin), "Voltage (V)");
			sensorGraphBox = new DataGraphBox(this, DataGraphBox.GenerateTitle("Sensor Value", (Phidget)vin));
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			vin.Attach -= vin_Attach;
			vin.Detach -= vin_Detach;
			vin.Error -= vin_Error;
			vin.PropertyChange -= Vin_PropertyChange;
			vin.SensorChange -= sensorEventHandler.Handler;
			vin.VoltageChange -= voltageRatioEventHandler.Handler;
			vin.Close();
		}

		private void Vin_PropertyChange(object sender, PropertyChangeEventArgs e) {
			switch (e.PropertyName) {
			case "DataInterval":
				if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
					dataIntervalTrk.Value = vin.DataInterval;
					updateGraphDataInterval();
				}
				break;
			case "MinDataInterval":
				dataIntervalTrk.initializeRange(vin.MinDataInterval, vin.MaxDataInterval);
				dataIntervalTrk.Value = vin.DataInterval;
				break;
			case "DataRate":
				if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
					dataIntervalTrk.Value = vin.DataRate;
					updateGraphDataInterval();
				}
				break;
			case "PowerSupply":
				powerSupplyCmb.SelectedValue = vin.PowerSupply;
				break;
			}
		}

		void Clear() {
			phidgetInfoBox.Clear();

			settingsBox.Visible = false;

			voltage_range_panel.Visible = false;
			dataBox.Visible = false;
			sensorTypeLbl.Visible = false;
			sensorTypeCmb.Visible = false;
			sensor_value_panel.Visible = false;
			sensorValueGraphButton.Visible = false;

			PowerSupplyPanel.Visible = false;
		}

		void vin_Attach(object sender, Phidget22.Events.AttachEventArgs e) {
			phidgetInfoBox.FillPhidgetInfo((Phidget)sender);

			VoltageInput attachedDevice = (VoltageInput)sender;

			voltageGraphBox.SetTitle(DataGraphBox.GenerateTitle("Voltage", (Phidget)attachedDevice));
			sensorGraphBox.SetTitle(DataGraphBox.GenerateTitle("Sensor Value", (Phidget)attachedDevice));

			this.sensorTypeCmb.SelectedIndexChanged -= sensorTypeCmb_SelectedIndexChanged;
			this.voltageRangeCmb.SelectedIndexChanged -= voltageRangeCmb_SelectedIndexChanged;

			try {
				dataIntervalTrk.Unit = "ms";
				dataIntervalTrk.initializeRange(attachedDevice.MinDataInterval, attachedDevice.MaxDataInterval);

				//differing max/min means the interval is a settable configuration
				if (attachedDevice.MaxDataInterval == attachedDevice.MinDataInterval) {
					dataIntervalTrk.Enabled = false;
				} else {
					dataIntervalTrk.Enabled = true;
					dataIntervalTrk.Value = attachedDevice.DataInterval;
				}
			} catch (PhidgetException ex) { errorBox.addMessage("Error setting interval: " + ex.Message); }

			try {
				changeTriggerTrk.Unit = "V";
				changeTriggerTrk.minTxt = attachedDevice.MinVoltageChangeTrigger.ToString();
				changeTriggerTrk.Minimum = attachedDevice.MinVoltageChangeTrigger;
				changeTriggerTrk.Maximum = attachedDevice.MaxVoltageChangeTrigger;
				changeTriggerTrk.maxTxt = attachedDevice.MaxVoltageChangeTrigger.ToString();


				//differing max/min means the trigger is a settable configuration
				if (attachedDevice.MaxVoltageChangeTrigger == attachedDevice.MinVoltageChangeTrigger) {
					changeTriggerTrk.Enabled = false;
				} else {
					changeTriggerTrk.Value = attachedDevice.VoltageChangeTrigger;
				}
			} catch (PhidgetException ex) { errorBox.addMessage("Error setting change trigger: " + ex.Message); }

			// Set up form according to device. Note that we set combo box values, which set the Phidget properties in the combo box change events.
			Enum[] supportedVoltageRanges;
			switch (vin.DeviceID) {

			case DeviceID.PN_ADP1000:
				voltage_range_panel.Visible = true;
				supportedVoltageRanges = new Enum[] {VoltageRange.MilliVolts_400,
														 VoltageRange.Volts_2};
				voltageRangeCmb.DataSource = supportedVoltageRanges.ToList();
				voltageRangeCmb.SelectedValue = attachedDevice.VoltageRange;
				break;

			case DeviceID.PN_VCP1000:
				voltage_range_panel.Visible = true;
				supportedVoltageRanges = new Enum[] {VoltageRange.MilliVolts_312_5,
														 VoltageRange.Volts_40};
				voltageRangeCmb.DataSource = supportedVoltageRanges.ToList();
				voltageRangeCmb.SelectedValue = attachedDevice.VoltageRange;
				break;

			case DeviceID.PN_VCP1001:
				voltage_range_panel.Visible = true;
				supportedVoltageRanges = new Enum[] {VoltageRange.Volts_5,
														 VoltageRange.Volts_15,
														 VoltageRange.Volts_40,
														 VoltageRange.Auto};
				voltageRangeCmb.DataSource = supportedVoltageRanges.ToList();
				voltageRangeCmb.SelectedValue = attachedDevice.VoltageRange;
				break;

			case DeviceID.PN_VCP1002:
				voltage_range_panel.Visible = true;
				supportedVoltageRanges = new Enum[] {VoltageRange.MilliVolts_10,
														 VoltageRange.MilliVolts_40,
														 VoltageRange.MilliVolts_200,
														 VoltageRange.MilliVolts_1000,
														 VoltageRange.Auto};
				voltageRangeCmb.DataSource = supportedVoltageRanges.ToList();
				voltageRangeCmb.SelectedValue = attachedDevice.VoltageRange;
				break;

			case DeviceID.PN_DAQ1400:
				Enum[] supportedPowerSupplies;
				PowerSupplyPanel.Visible = true;
				supportedPowerSupplies = new Enum[] {PowerSupply.Off,
														PowerSupply.Volts_12,
														PowerSupply.Volts_24};
				powerSupplyCmb.SelectedIndexChanged -= powerSupplyCmb_SelectedIndexChanged;
				powerSupplyCmb.DataSource = supportedPowerSupplies.ToList();
				powerSupplyCmb.SelectedValue = attachedDevice.PowerSupply;
				powerSupplyCmb.SelectedIndexChanged += powerSupplyCmb_SelectedIndexChanged;
				break;

			default:  // Standard 5V Sensor Ports
				if (attachedDevice.ChannelSubclass == ChannelSubclass.VoltageInputSensorPort) {
					sensorTypeLbl.Visible = true;
					sensor_value_panel.Visible = true;
					sensorValueGraphButton.Visible = true;
					sensorTypeCmb.DataSource = typeof(VoltageSensorType).ToList();
					sensorTypeCmb.SelectedValue = attachedDevice.SensorType;
					sensorTypeCmb.Visible = true;
				}
				break;
			}

			this.voltageRangeCmb.SelectedIndexChanged += voltageRangeCmb_SelectedIndexChanged;
			this.sensorTypeCmb.SelectedIndexChanged += sensorTypeCmb_SelectedIndexChanged;
			settingsBox.Visible = true;
			dataBox.Visible = true;

			updateGraphDataInterval();

			guiUpdateTimer.Start();
		}

		void vin_Detach(object sender, Phidget22.Events.DetachEventArgs e) {

			guiUpdateTimer.Stop();
			_voltageTxt = "";
			_sensorTxt = "";

			Clear();
			cleaner.clean();
			dataIntervalTrk.TrueValue = -1;
		}

		void vin_Error(object sender, Phidget22.Events.ErrorEventArgs e) {
			errorBox.addMessage(e.Description);

			if (e.Code == ErrorEventCode.Saturation || e.Code == ErrorEventCode.OutOfRange) {
				if (vin.ChannelSubclass == ChannelSubclass.VoltageInputSensorPort && vin.SensorType != VoltageSensorType.Voltage)
					sensorGraphBox.addGap();
				else
					voltageGraphBox.addGap();
			}
		}

		private string _voltageTxt = "";
		private string _sensorTxt = "";
		private void guiUpdateTimer_Tick(object sender, EventArgs e) {
			voltageTxt.Text = _voltageTxt;
			sensorValueTxt.Text = _sensorTxt;
		}

		void vin_VoltageChange(object sender, Phidget22.Events.VoltageInputVoltageChangeEventArgs e) {
			_voltageTxt = e.Voltage.ToString() + " V";
			voltageGraphBox.addData(e.Voltage);
		}
		private void Vin_SensorChange(object sender, VoltageInputSensorChangeEventArgs e) {
			_sensorTxt = e.SensorValue.ToString() + " " + e.SensorUnit.Symbol;
			sensorGraphBox.addData(e.SensorValue);
		}

		private void updateGraphDataInterval() {
			try {
				if (vin.VoltageChangeTrigger == 0)
					voltageGraphBox.DataInterval = vin.DataInterval;
				else
					voltageGraphBox.DataInterval = null;
				if (vin.ChannelSubclass == ChannelSubclass.VoltageInputSensorPort) {
					if (vin.SensorValueChangeTrigger == 0)
						sensorGraphBox.DataInterval = vin.DataInterval;
					else
						sensorGraphBox.DataInterval = null;
				}
			} catch (PhidgetException ex) { errorBox.addMessage("Error updating data interval: " + ex.Message); }
		}

		private void sensorTypeCmb_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				vin.SensorType = (VoltageSensorType)Enum.Parse(typeof(VoltageSensorType), ((ComboBox)sender).SelectedValue.ToString());
			} catch (Exception ex) {
				errorBox.addMessage("Error setting sensor type: " + ex.Message);
			}

			if (sensorTypeCmb.SelectedIndex == 0) {
				label5.Enabled = true; //standard voltage ratio events (i.e. generic sensor type selected)
				voltageTxt.Enabled = true;
				sensor_value_panel.Enabled = false;
			} else {
				label5.Enabled = false; //specific event for sensor type (this disables normal events)
				voltageTxt.Enabled = false;
				sensor_value_panel.Enabled = true;
				if (vin.SensorUnit.Unit == Unit.None)
					sensorGraphBox.SetAxisYTitle("");
				else
					sensorGraphBox.SetAxisYTitle(vin.SensorUnit.Name + " (" + vin.SensorUnit.Symbol + ")");
			}

			if (sensorTypeCmb.SelectedValue.ToString().Contains("MOT2002")) {
				dataIntervalTrk.Enabled = false; // disable the dataInterval track since it's frozen at 200ms for these enums
				dataIntervalTrk.Value = 200;
			} else {
				dataIntervalTrk.Enabled = true;
			}

		}

		private void voltageRangeCmb_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				vin.VoltageRange = (VoltageRange)Enum.Parse(typeof(VoltageRange), ((ComboBox)sender).SelectedValue.ToString());
			} catch (Exception ex) {
				errorBox.addMessage("Error setting voltage range: " + ex.Message);
			}
		}
		private void powerSupplyCmb_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				vin.PowerSupply = (PowerSupply)Enum.Parse(typeof(PowerSupply), ((ComboBox)sender).SelectedValue.ToString());
			} catch (Exception ex) {
				errorBox.addMessage("Error setting voltage range: " + ex.Message);
			}
		}

		private void VoltageGraphButton_Click(object sender, EventArgs e) {
			if (!voltageGraphBox.Visible)
				voltageGraphBox.Show();
			else
				voltageGraphBox.Hide();
		}

		private void sensorValueGraphButton_Click(object sender, EventArgs e) {
			if (!sensorGraphBox.Visible)
				sensorGraphBox.Show();
			else
				sensorGraphBox.Hide();
		}

		private void dataIntervalTrk_KeyOverride(object sender, KeyEventArgs e) {
			e.SuppressKeyPress = true;
			dataIntervalTrk.keyOverride(sender, e);
		}

		private void slider_ValueChangeEvent(object sender, EventArgs e) {
			if (((PhidgetsControlLibrary.CustomScroll)sender).isFixedRange) //datainterval control
			{
				if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
					try {
						double temp = dataIntervalTrk.Value;
						vin.DataInterval = (int)temp;
						if (vin.DataInterval < (int)temp) {
							while (vin.DataInterval != (int)temp) {
								temp++;
								vin.DataInterval = (int)temp;
							}
						} else {
							while (vin.DataInterval != (int)temp) {
								temp--;
								vin.DataInterval = (int)temp;
							}
						}

						dataIntervalTrk.TrueValue = vin.DataInterval;
						dataIntervalTrk.Value = vin.DataInterval;



						updateGraphDataInterval();
					} catch (Exception ex) { errorBox.addMessage("Error setting data interval: " + ex.Message); }
				} else if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
					try {
						vin.DataRate = dataIntervalTrk.Value;
						updateGraphDataInterval();
					} catch (Exception ex) { errorBox.addMessage("Error setting data rate: " + ex.Message); }
				}
			} else //changetrigger control
			  {
				try {
					vin.VoltageChangeTrigger = changeTriggerTrk.Value;
					updateGraphDataInterval();
				} catch (Exception ex) { errorBox.addMessage("Error setting change trigger: " + ex.Message); }
			}
		}
		private void dataIntervalTrk_ModeSwitch(object sender, EventArgs e) {
			dataIntervalTrk.SwitchModes();
			if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.RATE) {
				try {
					dataIntervalTrk.isIntValue = false;
					dataIntervalTrk.Unit = "Hz";
					dataIntervalTrk.trkText = vin.DataRate.ToString() + "Hz";
					dataIntervalTrk.Value = vin.DataRate;
				} catch (Exception ex) { errorBox.addMessage("Error setting data rate: " + ex.Message); }

			} else if (dataIntervalTrk.TimingMode == PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL) {
				try {
					dataIntervalTrk.isIntValue = true;
					if (vin.DeviceID != DeviceID.PN_1046)
						dataIntervalTrk.Unit = "ms";
					dataIntervalTrk.trkText = vin.DataInterval.ToString() + "ms";
					dataIntervalTrk.Value = vin.DataInterval;
				} catch (Exception ex) { errorBox.addMessage("Error setting data interval: " + ex.Message); }
			}
			dataIntervalTrk.TrueValue = dataIntervalTrk.Value;
		}
	}
}

