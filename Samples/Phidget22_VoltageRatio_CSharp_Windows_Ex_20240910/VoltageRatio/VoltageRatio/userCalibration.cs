using Phidget22;
using Phidget22.ExampleUtils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VoltageRatio_Example {
	public partial class userCalibration : Form {
		VoltageRatioInput calibrationDevice;
		double[] lastValue;
		int i = 0;
		ErrorEventBox errorBox;
		int state = 0;
		int samplesToCollect = 0;
		public double v1, v2, gain, offset;
		public userCalibration() {
			InitializeComponent();
		}

		public userCalibration(ErrorEventBox errorStream, VoltageRatioInput loadCellChannel, Form parent) {
			InitializeComponent();

			calibrationDevice = loadCellChannel;
			errorBox = errorStream;
		}

		private void CalibrationDevice_VoltageRatioChange(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e) {
			if (state == 0) {
				double tempOffset = e.VoltageRatio;
				try {
					if (Math.Abs((tempOffset - lastValue.Average()) / lastValue.Average()) * 100 > 1) //if new measurement is more than 0.2% different from average of last 5 samples
					{
						nextBtn.Enabled = false;
						nextBtn.Text = "Waiting...";
					} else {
						nextBtn.Enabled = true;
						nextBtn.Text = "Continue";
						v1 = tempOffset;
					}
				} catch (PhidgetException ex) { errorBox.addMessage("Error capturing sample: " + ex.Message); }
				lastValue[i++ % samplesToCollect] = tempOffset;
			}
			if (state == 2) {
				double tempGain = e.VoltageRatio;
				try {
					if (Math.Abs((tempGain - lastValue.Average()) / lastValue.Average()) * 100 > 0.5) //if new measurement is more than 0.2% different from average of last 5 samples
					{
						nextBtn.Enabled = false;
						nextBtn.Text = "Waiting...";
					} else {
						nextBtn.Enabled = true;
						nextBtn.Text = "Continue";
						v2 = tempGain;
					}
				} catch (PhidgetException ex) { errorBox.addMessage("Error capturing sample: " + ex.Message); }
				lastValue[i++ % samplesToCollect] = tempGain;
			}

		}
		private void capacityTxt_TextChanged(object sender, EventArgs e) {
			if (state == 1) {
				if (dataTxt.Text != "" && capacityTxt.Text != "")
					nextBtn.Text = "Calibrate";
				else
					nextBtn.Text = "Skip";
			}
		}

		private void dataTxt_TextChanged(object sender, EventArgs e) {
			if (state == 1) {
				if (dataTxt.Text != "" && capacityTxt.Text != "")
					nextBtn.Text = "Calibrate";
				else
					nextBtn.Text = "Skip";
			}
		}

		private void nextBtn_Click(object sender, EventArgs e) {
			switch (state) {
			case 0://we have our zero point data
				state = 1; //we are now capturing our non-zero data
				dataTxt.Visible = true;
				dataLbl.Visible = true;
				dataLbl.Text = "Rated Output:";
				capacityLbl.Visible = true;
				capacityTxt.Visible = true;
				unitLbl.Visible = true;
				linkLabel.Visible = true;
				nextBtn.Text = "Skip";
				instructionTxt.Text = Properties.Settings.Default.state1;
				title.Text = Properties.Settings.Default.title1;
				title.Font = new Font(this.Font, FontStyle.Bold);
				break;
			case 1:
				if (dataTxt.Text == "" || capacityTxt.Text == "") //we are skipping rated output and going to data input instead
				{
					state = 2;
					instructionTxt.Text = Properties.Settings.Default.state2;
					title.Text = Properties.Settings.Default.title2;
					nextBtn.Text = "Calibrate";
					dataLbl.Text = "Input Weight:";
					capacityLbl.Visible = false;
					capacityTxt.Visible = false;
					linkLabel.Visible = false;
					unitLbl.Visible = false;
				} else //we are using rated output to calibrate
				  {
					try {
						gain = double.Parse(capacityTxt.Text) / ((double.Parse(dataTxt.Text) / 1000.0) - v1);
					} catch (Exception ex) { errorBox.addMessage(ex.Message); return; }
					offset = -v1;
					state = 3;
					dataLbl.Text = "Input Weight:";
					capacityLbl.Visible = false;
					capacityTxt.Visible = false;
					unitLbl.Visible = false;
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				break;
			case 2: //use second measurement to get calibration parameters
				try {
					gain = double.Parse(dataTxt.Text) / (v2 - v1);
				} catch (Exception ex) { errorBox.addMessage(ex.Message); return; }
				offset = -v1;
				dataLbl.Visible = false;
				state = 3;
				this.DialogResult = DialogResult.OK;
				this.Close();
				break;
			default:
				break;
			}

		}

		private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(Properties.Settings.Default.ratedOutputURL);
			linkLabel.LinkVisited = true;
		}

		private void resetForm() {
			title.Text = Properties.Settings.Default.title0;
			instructionTxt.Text = Properties.Settings.Default.state0;
			dataTxt.Visible = false;
			dataLbl.Visible = false;
			dataLbl.Text = "Input Weight:";
			capacityLbl.Visible = false;
			capacityTxt.Visible = false;
			unitLbl.Visible = false;
			linkLabel.Visible = false;
			nextBtn.Text = "Continue";
			dataTxt.Clear();
			capacityTxt.Clear();
		}
		private void userCalibration_FormClosing(object sender, FormClosingEventArgs e) {
			calibrationDevice.VoltageRatioChange -= CalibrationDevice_VoltageRatioChange;
			resetForm();
		}

		private void userCalibration_Load(object sender, EventArgs e) {
			calibrationDevice.VoltageRatioChange += CalibrationDevice_VoltageRatioChange;
			samplesToCollect = 0;

			if (1 <= calibrationDevice.DataInterval && calibrationDevice.DataInterval <= 50) {
				samplesToCollect = 10;
			} else if (51 <= calibrationDevice.DataInterval && calibrationDevice.DataInterval <= 500) {
				samplesToCollect = 5;
			} else if (501 <= calibrationDevice.DataInterval && calibrationDevice.DataInterval <= 1000) {
				samplesToCollect = 2;
			} else if (calibrationDevice.DataInterval > 1000) {
				errorBox.addMessage("The calibration process requires a significant amount of data in order to be useful.  You currently have a data interval/rate selected that will making gathering this data very slow.  We recommend you adjust the data interval/rate to something faster (<1000ms/1Hz) and then try calibrating.");
				resetForm();
				this.DialogResult = DialogResult.Abort;
				this.Close();
			}
			lastValue = new double[samplesToCollect];
			state = 0;
			instructionTxt.Text = Properties.Settings.Default.state0;
			title.Text = Properties.Settings.Default.title0;
			title.Font = new Font(this.Font, FontStyle.Bold);
			dataTxt.Visible = false;
			dataLbl.Visible = false;

		}
	}
}
