using System;
using VNC.Phidget22.Ex;
using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22
{

    /// <summary>
    /// A physical Phidget Board of a specific DeviceClass
    /// providing one or more DeviceChannels
    /// A PhidgetDevice is hosted on some computer with an IPAddress
    /// and is uniquely identified by a SerialNumber
    /// </summary>
    public class PhidgetDevice
    {
        //public PhidgetDevice()
        //{
        //}

        public PhidgetDevice(string ipAddress, int hostPort, Phidgets.DeviceClass deviceClass, int serialNumber)
        {
            IPAddress = ipAddress;
            HostPort = hostPort;
            DeviceClass = deviceClass.ToString();
            SerialNumber = serialNumber;
        }

        public PhidgetDevice(string serverPeerName, Phidgets.DeviceClass deviceClass, int serialNumber)
        {
            HostComputer = serverPeerName;
            // TODO(crhodes)
            // See if we can switch to serverPeerName or need to keep IPAddress and Port
            //IPAddress = ipAddress;
            //Port = port;
            DeviceClass = deviceClass.ToString();
            SerialNumber = serialNumber;
        }

        // TODO(crhodes)
        // How is this used?
        public SerialHost SerialHostKey { get; set; }

        string _parent;
        public string Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        public string HostComputer { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public int HostPort { get; set; } = 5661;

        //public Phidgets.DeviceClass DeviceClass { get; set; } = Phidgets.DeviceClass.None;
        public string DeviceClass { get; set; }
        public string ChannelClass { get; set; }
        public Int32 Channel { get; set; }

        public Int32 HubPort { get; set; }

        // TODO(crhodes)
        // Hos is this used.  Only place I see is in AddPhidgetDevice()
        public Int32 ChannelCount { get; set; }

        public DeviceChannels DeviceChannels { get; set; } = new DeviceChannels();

        public int SerialNumber { get; set; } = 0;

        //// TODO(crhodes)
        //// How is this used?
        //public PhidgetEx PhidgetEx {  get; set; }
    }
}

