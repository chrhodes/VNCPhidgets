using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class MotorPositionControllerSequence : ChannelSequence
    {
        public MotorPositionControllerSequence() : base("MotorPositionController")
        {
        }

        public MotorPositionControllerSequence(MotorPositionControllerSequence sequence) : base("MotorPositionController", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of MotorPositionController actions in sequence
        /// </summary>
        public MotorPositionControllerAction[]? Actions { get; set; }
    }
}
