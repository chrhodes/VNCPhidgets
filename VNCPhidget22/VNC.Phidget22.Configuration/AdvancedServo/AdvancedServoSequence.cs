namespace VNC.Phidget22.Configuration
{
    public class AdvancedServoSequence : DeviceClassSequence
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
