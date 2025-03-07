using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class MotorPositionControllerSequence : ChannelClassSequence
    {
        public MotorPositionControllerSequence() : base("MotorPositionController")
        {
        }

        /// <summary>
        /// Array of MotorPositionController actions in sequence
        /// </summary>
        public MotorPositionControllerAction[] Actions { get; set; }
    }
}
