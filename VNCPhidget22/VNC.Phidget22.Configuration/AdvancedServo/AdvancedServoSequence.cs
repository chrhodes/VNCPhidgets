using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class AdvancedServoSequence : ChannelClassSequence
    {
        public AdvancedServoSequence() : base("RCServo")
        {
        }
        /// <summary>
        /// Array of AdvancedServoServo actions in sequence
        /// </summary>
        public AdvancedServoServoAction[]? Actions { get; set; }
    }
}
