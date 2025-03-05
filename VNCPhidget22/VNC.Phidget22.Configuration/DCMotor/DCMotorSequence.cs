namespace VNC.Phidget22.Configuration
{
    public class DCMotorSequence : DeviceClassSequence
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
