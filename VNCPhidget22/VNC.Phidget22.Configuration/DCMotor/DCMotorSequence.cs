using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DCMotorSequence : ChannelClassSequence
    {
        public DCMotorSequence() : base("DCMotor")
        {
        }

        /// <summary>
        /// Array of DCMotor actions in sequence
        /// </summary>
        public DCMotorAction[] Actions { get; set; }
    }
}
