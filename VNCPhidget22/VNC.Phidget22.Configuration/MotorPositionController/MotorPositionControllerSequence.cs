namespace VNC.Phidget22.Configuration
{
    public class MotorPositionControllerSequence : DeviceClassSequence
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
