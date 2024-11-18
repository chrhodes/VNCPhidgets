using System;
using System.Threading.Channels;

namespace VNC.Phidget22.Configuration
{
    public class RCServoConfiguration
    {
        // TODO(crhodes)
        // How is this used?
        public short Channel { get; set; }

        // TODO(crhodes)
        // We should probably add ServoType and maybe Acceleration/Velocity stuff
        //public bool IsRemote { get; set; } = true;
        //public bool IsLocal { get; set; } = false;
        public bool LogPhidgetEvents { get; set; }
        public bool LogErrorEvents { get; set; }
        public bool LogPropertyChangeEvents { get; set; }

        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }

        public bool LogTargetPositionReachedEvents { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }
    }
}