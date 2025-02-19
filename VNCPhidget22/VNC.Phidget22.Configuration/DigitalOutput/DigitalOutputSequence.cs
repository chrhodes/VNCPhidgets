namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequence : DeviceClassSequence
    {
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        public string DeviceClass { get; } = "DigitalOutput";
        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalOutputAction[] Actions { get; set; }
    }
}
