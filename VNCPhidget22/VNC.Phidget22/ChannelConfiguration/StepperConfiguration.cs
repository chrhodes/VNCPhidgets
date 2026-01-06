using System;

namespace VNC.Phidget22.ChannelConfiguration
{
    public class StepperConfiguration : ChannelConfigurationBase
    {
        // TODO(crhodes)
        // Add any channel specific configuration

        public Boolean LogPositionChangeEvents { get; set; }
        public Boolean LogVelocityChangeEvents { get; set; }

        public Boolean LogStoppedEvents { get; set; }
    }
}