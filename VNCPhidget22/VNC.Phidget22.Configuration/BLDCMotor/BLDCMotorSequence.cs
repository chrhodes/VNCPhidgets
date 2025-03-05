namespace VNC.Phidget22.Configuration
{
    public class BLDCMotorSequence : DeviceClassSequence
    {
        public BLDCMotorSequence() : base("BLDCMotor")
        {
        }

        /// <summary>
        /// Array of BLDCMotor actions in sequence
        /// </summary>
        public BLDCMotorAction[] Actions { get; set; }
    }
}
