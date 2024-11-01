using System;
using System.Windows.Forms;

namespace Phidget22.ExampleUtils {
	public partial class LocationData : Form {
		string serial, port, channel, hubport, server, ip, _class, label;
		bool remoteFlag = false;

		public LocationData() {
			InitializeComponent();
		}

		public LocationData(Phidget22.Phidget device) {
			serial = device.DeviceSerialNumber.ToString();
			channel = device.Channel.ToString();
			if (device.DeviceClass == DeviceClass.VINT) {
				port = device.HubPort.ToString();
				hubport = device.IsHubPortDevice.ToString();
			} else {
				port = "Not a VINT device";
				hubport = "Not a VINT device";
			}
			_class = device.ChannelClass.ToString();
			if (device.IsRemote) {
				remoteFlag = true;
				server = device.ServerHostname.ToString();
				ip = device.ServerPeerName.ToString();
			}
			label = device.DeviceLabel;

			InitializeComponent();
		}

		private void LocationData_Load(object sender, EventArgs e) {

			serialTxt.Text = serial;
			channelTxt.Text = channel;
			classTxt.Text = _class;
			portTxt.Text = port;
			hubportTxt.Text = hubport;
			if (remoteFlag) {
				serverTxt.Text = server;
				ipTxt.Text = ip;
			} else {
				serverTxt.Text = "Locally attached";
				ipTxt.Text = "Locally attached";
			}
			labelTxt.Text = label;
		}
	}
}
