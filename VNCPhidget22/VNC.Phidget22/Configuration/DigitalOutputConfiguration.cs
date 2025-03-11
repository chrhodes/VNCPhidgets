using System;
using System.Threading.Channels;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputConfiguration
    {
        public short Channel { get; set; }
        //public bool IsRemote { get; set; } = true;
        //public bool IsLocal { get; set; } = false;

        //public bool IsLocal { get; set; } = false;
        public bool LogPhidgetEvents { get; set; }
        public bool LogErrorEvents { get; set; } = true;    // Probably always want to see errors
        public bool LogPropertyChangeEvents { get; set; }

        public bool LogDeviceChannelSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

    }
}