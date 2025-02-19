namespace VNC.Phidget22.Configuration
{
    public class VoltageInputSequence : DeviceClassSequence
    {
        public VoltageInputSequence() : base("VoltageInput")
        {
        }
        /// <summary>
        /// Array of VoltageInput actions in sequence
        /// </summary>
        public VoltageInputAction[] Actions { get; set; }
    }
}
