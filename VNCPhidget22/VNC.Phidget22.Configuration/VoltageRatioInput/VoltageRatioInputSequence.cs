using Phidget22;

namespace VNC.Phidget22.Configuration
{
    public class VoltageRatioInputSequence : DeviceClassSequence
    {
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        public string DeviceClass { get; } = "VoltageRatioInput";

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageRatioInputAction[] Actions { get; set; }
    }
}
