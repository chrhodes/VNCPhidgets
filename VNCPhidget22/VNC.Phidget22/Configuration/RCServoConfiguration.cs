using System;
using System.Threading.Channels;

namespace VNC.Phidget22.Configuration
{
    public class RCServoConfiguration : ChannelConfigurationBase
    {
        // TODO(crhodes)
        // Add any channel specific configuration

        // TODO(crhodes)
        // We should probably add ServoType and maybe Acceleration/Velocity stuff

        public Boolean LogPositionChangeEvents { get; set; }
        public Boolean LogVelocityChangeEvents { get; set; }

        public Boolean LogTargetPositionReachedEvents { get; set; }
    }
}