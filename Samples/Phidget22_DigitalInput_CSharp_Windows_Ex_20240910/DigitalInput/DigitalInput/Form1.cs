using Phidget22;
using Phidget22.Events;
using Phidget22.ExampleUtils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DigitalInput_Example {
	public partial class Form1 : Form {
		CommandLineOpen openArgs;
		DigitalInput openCh = null;
		DigitalInput digin = null;
		private ErrorEventBox errorBox;
		formCleaner cleaner;

		private DataGraphBox stateGraphBox;

		public Form1() {
			InitializeComponent();
			openArgs = new CommandLineOpen(this);
			cleaner = new formCleaner(this);
		}

		public Form1(DigitalInput ch) {
			InitializeComponent();
			openCh = ch;
			cleaner = new formCleaner(this);
		}

		private void Form1_Load(object sender, EventArgs e) {

			errorBox = new ErrorEventBox(this);
			digin = openCh ?? openArgs.makeChannel<DigitalInput>();

			stateGraphBox = new DataGraphBox(this, "State");

			digin.Attach += input_attach;
			digin.Detach += input_detach;
			digin.Error += input_error;
			digin.PropertyChange += Digin_PropertyChange;

			// This causes the event to get handled without invoke
			FastEventHandler<DigitalInputStateChangeEventArgs> stateEventHandler = new FastEventHandler<DigitalInputStateChangeEventArgs>();
			digin.StateChange += stateEventHandler.Handler;
			stateEventHandler.Event += input_change;

			try {
				digin.Open();
			} catch (PhidgetException ex) { errorBox.addMessage("Error opening device: " + ex.Message); }

		}

		private void Digin_PropertyChange(object sender, PropertyChangeEventArgs e) {

			switch (e.PropertyName) {
			case "PowerSupply":
				powerSupplyCmb.SelectedValue = digin.PowerSupply;
				break;
			case "InputMode":
				inputModeCombo.SelectedValue = digin.InputMode;
				break;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			digin.Attach -= input_attach;
			digin.Detach -= input_detach;
			digin.Error -= input_error;
			digin.PropertyChange -= Digin_PropertyChange;
			digin.Close();
		}

		//handlers

		void input_attach(object sender, Phidget22.Events.AttachEventArgs e) {
			settingsBox.Visible = true;
			PowerSupplyPanel.Visible = true;
			inputModeBox.Visible = true;
			phidgetInfoBox1.FillPhidgetInfo((Phidget)sender);

			DigitalInput attachedDevice = (DigitalInput)sender;

			stateGraphBox.SetTitle(DataGraphBox.GenerateTitle("State", (Phidget)digin));

			inputState.BackColor = Control.DefaultBackColor;
			inputState.ForeColor = Color.Black;
			inputState.Text = Environment.NewLine + "Unknown";

			try {
				Enum[] supportedPowerSupplies;
				supportedPowerSupplies = new Enum[] {PowerSupply.Off,
														 PowerSupply.Volts_12,
														 PowerSupply.Volts_24};
				powerSupplyCmb.SelectedIndexChanged -= powerSupplyCmb_SelectedIndexChanged;
				powerSupplyCmb.DataSource = supportedPowerSupplies.ToList();
				powerSupplyCmb.SelectedValue = attachedDevice.PowerSupply;
				powerSupplyCmb.SelectedIndexChanged += powerSupplyCmb_SelectedIndexChanged;
			} catch (PhidgetException ex) {
				if (ex.ErrorCode == ErrorCode.Unsupported) {
					PowerSupplyPanel.Visible = false;

				} else
					errorBox.addMessage("Error setting power supply: " + ex.Message);
			}

			try {
				Enum[] supportedInputModes;
				supportedInputModes = new Enum[] {InputMode.NPN,
													InputMode.PNP};
				inputModeCombo.SelectedIndexChanged -= inputModeCombo_SelectedIndexChanged;
				inputModeCombo.DataSource = supportedInputModes.ToList();
				inputModeCombo.SelectedValue = attachedDevice.InputMode;
				inputModeCombo.SelectedIndexChanged += inputModeCombo_SelectedIndexChanged;
			} catch (PhidgetException ex) {
				if (ex.ErrorCode == ErrorCode.Unsupported) {
					inputModeBox.Visible = false;
				} else
					errorBox.addMessage("Error setting input mode: " + ex.Message);
			}



			flowLayoutPanel1.Visible = true;
			dataBox.Visible = true;
			if (PowerSupplyPanel.Visible == false && inputModeBox.Visible == false)
				settingsBox.Visible = false;
			else
				settingsBox.Visible = true;
		}

		void input_detach(object sender, Phidget22.Events.DetachEventArgs e) {
			guiUpdateTimer.Stop();
			phidgetInfoBox1.Clear();
			cleaner.clean();
			flowLayoutPanel1.Visible = false;
			dataBox.Visible = false;
			settingsBox.Visible = false;
			PowerSupplyPanel.Visible = false;
			inputModeBox.Visible = false;
		}

		void input_error(object sender, Phidget22.Events.ErrorEventArgs e) {
			errorBox.addMessage(e.Description);
		}

		//input data handler
		int stateChangeCnt = 0;
		void input_change(object sender, Phidget22.Events.DigitalInputStateChangeEventArgs e) {
			try {
				stateGraphBox.addData(e.State == true ? 0 : 1);
				stateGraphBox.addData(e.State == true ? 1 : 0);
				stateChangeCnt++;
				if (!guiUpdateTimer.Enabled)
					this.Invoke(new MethodInvoker(delegate () { guiUpdateTimer.Start(); }));

			} catch { }
		}

		private void powerSupplyCmb_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				digin.PowerSupply = (PowerSupply)Enum.Parse(typeof(PowerSupply), ((ComboBox)sender).SelectedValue.ToString());
			} catch (Exception ex) {
				errorBox.addMessage("Error setting voltage range: " + ex.Message);
			}
		}

		private void inputModeCombo_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				digin.InputMode = (InputMode)Enum.Parse(typeof(InputMode), ((ComboBox)sender).SelectedValue.ToString());
			} catch (Exception ex) {
				errorBox.addMessage("Error setting input mode: " + ex.Message);
			}
		}

		private void stateGraphButton_Click(object sender, EventArgs e) {
			if (!stateGraphBox.Visible)
				stateGraphBox.Show();
		}

		bool? currentState = null;
		private void guiUpdateTimer_Tick(object sender, EventArgs e) {

			try {
				bool state = digin.State;

				if (currentState.HasValue) {
					// missed one
					if (stateChangeCnt > 1) {
						// just swap
						state = !currentState.Value;
					}
				}

				// When the state isn't changing, keep updating the graph
				if (stateChangeCnt == 0)
					stateGraphBox.addData(state == true ? 1 : 0);

				stateChangeCnt = 0;

				switch (state) {
				case true:
					inputState.BackColor = Color.Black;
					inputState.ForeColor = Color.White;
					inputState.Text = Environment.NewLine + "True";
					break;
				case false:
					inputState.BackColor = Color.White;
					inputState.ForeColor = Color.Black;
					inputState.Text = Environment.NewLine + "False";
					break;
				}

				currentState = state;
			} catch (PhidgetException) {
				inputState.BackColor = Control.DefaultBackColor;
				inputState.ForeColor = Color.Black;
				inputState.Text = Environment.NewLine + "Unknown";
				guiUpdateTimer.Stop();
			}
			// Try to randomize the interval a bit, so we don't end up locked-in if our interval matches the incoming data
		}
	}
}
