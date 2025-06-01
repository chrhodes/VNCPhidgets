using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DCMotorSequence : ChannelSequence
    {
        public DCMotorSequence() : base("DCMotorSequence")
        {
        }

        public DCMotorSequence(DCMotorSequence sequence) : base("DCMotorSequence", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of DCMotor actions in sequence
        /// </summary>
        public DCMotorAction[]? Actions { get; set; }
    }
}
