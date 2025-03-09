using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class RCServoChannelSequence : ChannelClassSequence
    {
        public RCServoChannelSequence() : base("RCServo")
        {
        }

        /// <summary>
        /// Array of RCServo actions for channel
        /// </summary>
        public RCServoAction[]? Actions { get; set; }
    }
}
