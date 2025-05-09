using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class RCServoSequence : ChannelSequence
    {
        public RCServoSequence() : base("RCServo")
        {
        }

        public RCServoSequence(RCServoSequence sequence) : base("RCServo", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of RCServo actions for channel in sequence
        /// </summary>
        public RCServoAction[]? Actions { get; set; }
    }
}
