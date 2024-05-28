using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Phidget22;

namespace Phidget22.ExampleUtils {
	public partial class PhidgetInfoBox : UserControl {

		LocationData locationData;
		Phidget attachedDevice;

		public PhidgetInfoBox() {
			InitializeComponent();

			Clear();
		}

		public void FillPhidgetInfo(Phidget phidget) {
			attachedDevice = phidget;
			locationData = new LocationData(attachedDevice);
			channelTxt.Text = phidget.Channel.ToString();
			versionTxt.Text = phidget.DeviceVersion.ToString();

			if (phidget.IsHubPortDevice)
				attachedDeviceNameTxt.Text = phidget.Parent.Parent.DeviceSKU + " - " + phidget.Parent.Parent.DeviceName;
			else
				attachedDeviceNameTxt.Text = phidget.DeviceSKU + " - " + phidget.DeviceName;

			if (phidget.DeviceClass == DeviceClass.VINT) {

				serialNumberLbl.Text = "Hub Serial Number:";
				serialNumberTxt.Text = phidget.DeviceSerialNumber.ToString();

				hubPortLbl.Visible = true;
				hubPortTxt.Visible = true;
				hubPortTxt.Text = phidget.HubPort.ToString();
			} else {
				serialNumberTxt.Text = phidget.DeviceSerialNumber.ToString();
			}

			try {
				String sku;

				if (phidget.IsHubPortDevice)
					sku = phidget.Parent.Parent.DeviceSKU.Split(new char[] { '/' })[0];
				else
					sku = phidget.DeviceSKU.Split(new char[] { '/' })[0];

				//Special attention to the 1018 because it's so common
				if (phidget.DeviceSKU.Contains("1018"))
					sku = "1018";

				Assembly _assembly = Assembly.GetExecutingAssembly();
				String imageName = _assembly.GetManifestResourceNames().Where(img => img.EndsWith(sku + ".png")).FirstOrDefault();

				if (imageName != null) {
					pictureBox1.Image = Image.FromStream(_assembly.GetManifestResourceStream(imageName));
					line1.Size = new System.Drawing.Size(groupBox1.Size.Width - 101, 1);
					pictureBox1.Visible = true;
				}

				if (phidget.IsRemote)
					remoteLbl.Visible = true;
				else
					remoteLbl.Visible = false;
			} catch { }

			Control par = this.Parent;
			while (!(par is Form))
				par = par.Parent;

			((Form)par).Text = phidget.ChannelName;
		}

		public void Clear() {
			attachedDeviceNameTxt.Text = "Nothing";
			channelTxt.Clear();
			versionTxt.Clear();
			serialNumberTxt.Clear();
			hubPortTxt.Clear();
			serialNumberLbl.Text = "Serial Number:";
			line1.Size = new System.Drawing.Size(groupBox1.Size.Width - 12, 1);
			pictureBox1.Visible = false;

			hubPortLbl.Visible = false;
			hubPortTxt.Visible = false;
			remoteLbl.Visible = false;
			if (locationData != null)
				locationData.Hide();
		}

		private void locationButton_Click(object sender, EventArgs e) {
			if (serialNumberTxt.Text != "") {
				locationData.ShowDialog();
			}
		}
	}
}
