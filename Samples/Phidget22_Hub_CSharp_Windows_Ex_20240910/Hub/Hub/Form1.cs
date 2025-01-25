using Phidget22;
using Phidget22.ExampleUtils;
using System;
using System.Windows.Forms;

namespace HubExample {
	public partial class Form1 : Form {
		CommandLineOpen openArgs;
		Hub openCh = null;
		Hub digout = null;
		private ErrorEventBox errorBox;
		formCleaner cleaner;

		public Form1() {
			InitializeComponent();
			openArgs = new CommandLineOpen(this);
			cleaner = new formCleaner(this);
		}

		public Form1(Hub ch) {
			InitializeComponent();
			openCh = ch;
			cleaner = new formCleaner(this);
		}

		private void Form1_Load(object sender, EventArgs e) {

			errorBox = new ErrorEventBox(this);
			digout = openCh ?? openArgs.makeChannel<Hub>();

			digout.Attach += hub_attach;
			digout.Detach += hub_detach;
			digout.Error += hub_error;

			try {
				digout.Open();
			} catch (PhidgetException ex) { errorBox.addMessage("Error opening device: " + ex.Message); }
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

			digout.Attach -= hub_attach;
			digout.Detach -= hub_detach;
			digout.Error -= hub_error;
			digout.Close();
		}

		void hub_attach(object sender, Phidget22.Events.AttachEventArgs e) {
			phidgetInfoBox1.FillPhidgetInfo((Phidget)sender);

			Hub attachedDevice = (Hub)sender;
		}

		void hub_detach(object sender, Phidget22.Events.DetachEventArgs e) {
			phidgetInfoBox1.Clear();
			cleaner.clean();
			foreach (Control i in this.Controls)
				i.Visible = false;
			phidgetInfoBox1.Visible = true;
		}

		void hub_error(object sender, Phidget22.Events.ErrorEventArgs e) {
			errorBox.addMessage(e.Description);
		}

	}
}