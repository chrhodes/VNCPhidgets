using System;
using System.Drawing;
using System.Windows.Forms;

namespace Phidget22.ExampleUtils {
	public partial class ErrorEventBox : Form {
		int errorCount = 0;

		public ErrorEventBox(Form owner) {

			InitializeComponent();
			this.Owner = owner;
			IntPtr dummy = this.Handle; //Force the form to load before it's shown so we can call invoke on it.
			logBox.SelectionColor = Color.Black;
		}

		private void ErrorEventBox_FormClosing(object sender, FormClosingEventArgs e) {

			if (e.CloseReason == CloseReason.UserClosing) {
				this.Hide();
				e.Cancel = true;
			}
		}

		public delegate void StringDelegate(String str);
		private string lastErrorMsg = "";
		private int duplicateMsgCnt = 0;
		public void addMessage(String message) {

			if (logBox.InvokeRequired) {
				try {
					logBox.BeginInvoke(new StringDelegate(addMessage), new Object[] { message });
				} catch { }
			} else {
				if (lastErrorMsg != message) {
					if (duplicateMsgCnt > 0)
						printDuplicates();
					duplicateMsgCnt = 0;
					timer1.Stop();

					addLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " - " + message);

					if (!this.Visible)
						this.Show();
				} else {
					timer1.Stop();
					timer1.Start();

					duplicateMsgCnt++;
					if (duplicateMsgCnt % 100 == 0)
						printDuplicates();
				}

				lastErrorMsg = message;

				errorCount++;
				errorCountLbl.Text = errorCount.ToString();
			}
		}
		private void printDuplicates() {

			addLine("last message repeated " + duplicateMsgCnt + " times");
		}
		private void addLine(string line) {

			if (!logBox.Text.EndsWith("\n") && logBox.Text.Length > 0)
				logBox.AppendText("\n");
			logBox.AppendText(line);
			logBox.SelectionStart = logBox.Text.Length;
			logBox.ScrollToCaret();
		}
		public void displayErrorBox() {

			//We need to show() the form in the context of the parent's main thread to determine initial affinity
			if (this.InvokeRequired) {
				this.BeginInvoke(new MethodInvoker(displayErrorBox));
			} else {
				if (!this.Visible)
					this.Show();
			}
		}

		private void clearBtn_Click(object sender, EventArgs e) {

			errorCount = 0;
			duplicateMsgCnt = 0;
			lastErrorMsg = "";
			timer1.Stop();
			logBox.Clear();
		}

		private void closeBtn_Click(object sender, EventArgs e) {

			this.Hide();
		}

		private void timer1_Tick(object sender, EventArgs e) {

			printDuplicates();
			duplicateMsgCnt = 0;
			lastErrorMsg = "";

			timer1.Stop();
		}
	}
}