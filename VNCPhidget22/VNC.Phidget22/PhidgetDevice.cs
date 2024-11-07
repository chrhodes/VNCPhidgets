using System;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22
{

    public class PhidgetDevice
    {
        //public PhidgetDevice()
        //{
        //}

        public PhidgetDevice(string ipAddress, int port, Phidgets.DeviceClass deviceClass, int serialNumber)
        {
            IPAddress = ipAddress;
            Port = port;
            DeviceClass = deviceClass;
            SerialNumber = serialNumber;
        }

        public PhidgetDevice(string serverPeerName, Phidgets.DeviceClass deviceClass, int serialNumber)
        {
            ServerPeerName = serverPeerName;
            // TODO(crhodes)
            // See if we can switch to serverPeerName or need to keep IPAddress and Port
            //IPAddress = ipAddress;
            //Port = port;
            DeviceClass = deviceClass;
            SerialNumber = serialNumber;
        }

        // TODO(crhodes)
        // How is this used?
        public SerialHost SerialHostKey { get; set; }

        public string ServerPeerName { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 5661;

        public Phidgets.DeviceClass DeviceClass { get; set; } = Phidgets.DeviceClass.None;

        public Int32 ChannelCount { get; set; }

        public DeviceChannels DeviceChannels { get; set; } = new DeviceChannels();

        public int SerialNumber { get; set; } = 0;

        public PhidgetEx PhidgetEx {  get; set; }
    }
}

