using Phidget22;
using Phidget22.Events;
using Phidget22.ExampleUtils;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DigitalOutputExample {
	public partial class Form1 : Form {
		CommandLineOpen openArgs;
		DigitalOutput openCh = null;
		DigitalOutput digout = null;
		private ErrorEventBox errorBox;
		formCleaner cleaner;

		public Form1() {
			InitializeComponent();
			openArgs = new CommandLineOpen(this);
			cleaner = new formCleaner(this);
		}

		public Form1(DigitalOutput ch) {
			InitializeComponent();
			openCh = ch;
			cleaner = new formCleaner(this);
		}

		private void Form1_Load(object sender, EventArgs e) {

			errorBox = new ErrorEventBox(this);
			digout = openCh ?? openArgs.makeChannel<DigitalOutput>();

			digout.Attach += output_attach;
			digout.Detach += output_detach;
			digout.Error += output_error;
			digout.PropertyChange += output_propertychange;

			try {
				digout.Open();
			} catch (PhidgetException ex) { errorBox.addMessage("Error opening device: " + ex.Message); }
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			digout.Attach -= output_attach;
			digout.Detach -= output_detach;
			digout.Error -= output_error;
			digout.PropertyChange -= output_propertychange;
			digout.Close();
		}

		void output_attach(object sender, Phidget22.Events.AttachEventArgs e) {
			phidgetInfoBox1.FillPhidgetInfo((Phidget)sender);

			DigitalOutput attachedDevice = (DigitalOutput)sender;

			outputSettings.Visible = true;

			switch (attachedDevice.ChannelSubclass) {
			case ChannelSubclass.DigitalOutputDutyCycle:
				dutyCyclePanel.Visible = true;
				frequencyPanel.Visible = false;
				ledPanel.Visible = false;
				dutyCycleTrk.Value = 0;
				dutyCycleTxt.Text = "0";
				switch (attachedDevice.DeviceID) {
				case DeviceID.PN_REL1101:
					dutyCycleTrk.SmallChange = 5;
					break;
				case DeviceID.PN_OUT1100:
				case DeviceID.PN_REL1100:
					dutyCycleTrk.SmallChange = 1;
					break;
				default:
					dutyCycleTrk.SmallChange = 8;
					break;
				}
				break;

			case ChannelSubclass.DigitalOutputFrequency:
				dutyCyclePanel.Visible = true;
				frequencyPanel.Visible = true;
				ledPanel.Visible = false;
				dutyCycleTrk.Value = 0;
				dutyCycleTxt.Text = "0";
				freqMinLbl.Text = attachedDevice.MinFrequency.ToString() + " Hz";
				freqMaxLbl.Text = attachedDevice.MaxFrequency.ToString() + " Hz";
				freqTrk.Minimum = (int)attachedDevice.MinFrequency;
				freqTrk.Maximum = (int)attachedDevice.MaxFrequency;
				freqTrk.Value = (int)attachedDevice.Frequency;
				freqTxt.Text = (digout.Frequency).ToString() + " Hz";
				switch (attachedDevice.DeviceID) {
				case DeviceID.PN_REL1101:
					dutyCycleTrk.SmallChange = 5;
					break;
				case DeviceID.PN_OUT1100:
				case DeviceID.PN_REL1100:
					dutyCycleTrk.SmallChange = 1;
					break;
				default:
					dutyCycleTrk.SmallChange = 8;
					break;
				}
				break;

			case ChannelSubclass.DigitalOutputLEDDriver:
				dutyCyclePanel.Visible = true;
				frequencyPanel.Visible = false;
				ledPanel.Visible = true;
				dutyCycleTrk.Value = 0;
				dutyCycleTxt.Text = "0";

				Enum[] supportedForwardVoltages;

				// Make sure we don't get a event from setting the DataSource
				voltageCombo.SelectedIndexChanged -= voltageCombo_SelectedIndexChanged;

				switch (attachedDevice.DeviceID) { //initialize form elements based on detected device
				case DeviceID.PN_1031://led controller
				case DeviceID.PN_1032://led controller
					supportedForwardVoltages = new Enum[] {LEDForwardVoltage.Volts_1_7,
															   LEDForwardVoltage.Volts_2_75,
															   LEDForwardVoltage.Volts_3_9,
															   LEDForwardVoltage.Volts_5_0};
					voltageCombo.DataSource = supportedForwardVoltages.ToList();
					break;
				case DeviceID.PN_LED1000://vint led controller
					supportedForwardVoltages = new Enum[] {LEDForwardVoltage.Volts_3_2,
															LEDForwardVoltage.Volts_4_0,
															LEDForwardVoltage.Volts_4_8,
															LEDForwardVoltage.Volts_5_6};
					voltageCombo.DataSource = supportedForwardVoltages.ToList();
					break;
				}

				initializeLED();

				voltageCombo.SelectedIndexChanged += voltageCombo_SelectedIndexChanged;

				break;

			default:
				dutyCyclePanel.Visible = false;
				frequencyPanel.Visible = false;
				break;
			}

			stateBtn.Enabled = true;
		}

		void output_detach(object sender, Phidget22.Events.DetachEventArgs e) {
			phidgetInfoBox1.Clear();
			cleaner.clean();
			foreach (Control i in this.Controls)
				i.Visible = false;
			phidgetInfoBox1.Visible = true;
		}

		void output_error(object sender, Phidget22.Events.ErrorEventArgs e) {
			errorBox.addMessage(e.Description);
		}

		private void output_propertychange(object sender, PropertyChangeEventArgs e) {

			switch (e.PropertyName) {
			case "DutyCycle":
				dutyCycleTrk.Value = (int)((digout.DutyCycle) * 1000);
				dutyCycleTxt.Text = digout.DutyCycle.ToString();
				break;
			case "Frequency":
				freqTrk.Value = (int)digout.Frequency;
				freqTxt.Text = digout.Frequency.ToString();
				break;
			}
		}
		private void currentTrk_Scroll(object sender, EventArgs e) {
			try {
				digout.LEDCurrentLimit = currentTrk.Value * digout.MaxLEDCurrentLimit / (double)currentTrk.Maximum;
				currentTxt.Text = (digout.LEDCurrentLimit * 1000.0).ToString() + " mA";
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting current limit: " + ex.Message);
			}
		}

		private void dutyCycleTrk_Scroll(object sender, EventArgs e) {
			try {
				digout.DutyCycle = dutyCycleTrk.Value / (double)dutyCycleTrk.Maximum;
				dutyCycleTxt.Text = digout.DutyCycle.ToString();
				if (dutyCycleTrk.Value > dutyCycleTrk.Maximum / 2)
					stateBtn.Text = "Turn Off";
				else
					stateBtn.Text = "Turn On";
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting duty cycle: " + ex.Message);
			}
		}

		private void freqTrk_Scroll(object sender, EventArgs e) {
			try {
				digout.Frequency = (double)freqTrk.Value;
				freqTxt.Text = (digout.Frequency).ToString() + " Hz";
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting frequency: " + ex.Message);
			}
		}

		private void voltageCombo_SelectedIndexChanged(object sender, EventArgs e) {
			if (((ComboBox)sender).SelectedIndex == -1 || digout == null)
				return;

			try {
				digout.LEDForwardVoltage = (LEDForwardVoltage)Enum.Parse(typeof(LEDForwardVoltage), voltageCombo.SelectedValue.ToString());
			} catch (PhidgetException ex) {
				voltageCombo.SelectedIndex = -1;
				errorBox.addMessage("Error setting board forward voltage: " + ex.Message);
			}
		}

		private void initializeLED() {
			try {
				currentMaxLbl.Text = (digout.MaxLEDCurrentLimit * 1000).ToString() + " mA";
				currentMinLbl.Text = (digout.MinLEDCurrentLimit * 1000).ToString() + " mA";

				try {
					currentTrk.Value = (int)((digout.LEDCurrentLimit / digout.MaxLEDCurrentLimit) * 1000);
					currentTxt.Text = (digout.LEDCurrentLimit * 1000.0).ToString() + " mA";
				} catch {
					currentTrk.Value = (int)(20 / (1000 * digout.MaxLEDCurrentLimit) * 1000);
					currentTxt.Text = "20 mA";
					digout.LEDCurrentLimit = 0.02;
				}

				try {
					voltageCombo.SelectedValue = digout.LEDForwardVoltage;
				} catch {
					voltageCombo.SelectedIndex = 0;
					digout.LEDForwardVoltage = (LEDForwardVoltage)Enum.Parse(typeof(LEDForwardVoltage), voltageCombo.SelectedValue.ToString());
				}
				digout.DutyCycle = 0;
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error initializing LED: " + ex.Message);
			}
		}

		private void stateBtn_Click(object sender, EventArgs e) {
			try {
				if (stateBtn.Text == "Turn On") {
					dutyCycleTrk.Value = 1000;
					dutyCycleTxt.Text = "1";
					digout.State = true;
					stateBtn.Text = "Turn Off";
				} else {
					dutyCycleTrk.Value = 0;
					dutyCycleTxt.Text = "0";
					digout.State = false;
					stateBtn.Text = "Turn On";
				}
			} catch (PhidgetException ex) {
				errorBox.addMessage("Error setting state: " + ex.Message);
			}
		}
	}
}