namespace VNC.Phidget22.Configuration
{
    public class VoltageInputSequence : DeviceClassSequence
    {
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        public string DeviceClass { get; } = "VoltageInput";
        /// <summary>
        /// Array of VoltageInput actions in sequence
        /// </summary>
        public VoltageInputAction[] Actions { get; set; }
    }
}
