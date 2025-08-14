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

        //public PhidgetDevice(string ipAddress, Int32 hostPort, Phidgets.DeviceClass deviceClass, Int32 serialNumber)
        //{
        //    IPAddress = ipAddress;
        //    HostPort = hostPort;
        //    DeviceClass = deviceClass.ToString();
        //    SerialNumber = serialNumber;
        //}

        public PhidgetDevice(string serverPeerName, Int32 deviceSerialNumber)
        {
            HostComputer = serverPeerName;
            DeviceSerialNumber = deviceSerialNumber;
        }

        //// TODO(crhodes)
        //// How is this used?
        //public SerialHost SerialHostKey { get; set; }


        public string? HostComputer { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public Int32 HostPort { get; set; } = 5661;

        public Int32 DeviceSerialNumber { get; set; } = 0;

        public Boolean IsLocal { get; set; }
        public Boolean IsRemote { get; set; }

        public string? GrandParent { get; set; }
        public string? Parent { get; set; }

        public bool IsHubPortDevice { get; set; }
        public Int32 HubPort { get; set; }

        public string? DeviceClass { get; set; }
        public string? DeviceClassName { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceSKU { get; set; }

        public string? DeviceID { get; set; }
        public string? DeviceVINTID { get; set; }
        public string? DeviceVersion { get; set; }

        public bool IsChannel { get; set; }
        public Int32 Channel { get; set; }
        public string? ChannelClass { get; set; }
        public string? ChannelName { get; set; }
        public string? ChannelSubclass { get; set; }


        // TODO(crhodes)
        // How is this used.  Only place I see is in AddPhidgetDevice()
        public Int32 ChannelCount { get; set; }

        // FIX(crhodes)
        // UI Would be better if kept track of Channels present on each PhidgetDevice
        // Figure out how to put this back in

        public DeviceChannels DeviceChannels { get; set; } = new DeviceChannels();

        //// TODO(crhodes)
        //// How is this used?
        //public PhidgetEx PhidgetEx {  get; set; }
    }
}

