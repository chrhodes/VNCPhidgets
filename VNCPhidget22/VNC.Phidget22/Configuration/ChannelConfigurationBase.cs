using System;
using System.Threading.Channels;

namespace VNC.Phidget22.Configuration
{
    /// <summary>
    /// Properties common to all Channels
    /// </summary>
    public class ChannelConfigurationBase
    {
        public Int32 DeviceSerialNumber { get; set; }
        public Boolean HubPortDevice { get; set; }
        public Int32 HubPort { get; set; }
        public Int32 Channel { get; set; }

        public Boolean LogPhidgetEvents { get; set; }
        public Boolean LogErrorEvents { get; set; } = true;    // Probably always want to see errors
        public Boolean LogPropertyChangeEvents { get; set; }

        public Boolean LogDeviceChannelSequence { get; set; }
        public Boolean LogChannelAction { get; set; }
        public Boolean LogActionVerification { get; set; }
    }
}