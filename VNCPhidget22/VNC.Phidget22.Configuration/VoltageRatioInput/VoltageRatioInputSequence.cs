using Phidget22;

namespace VNC.Phidget22.Configuration
{
    public class VoltageRatioInputSequence : DeviceClassSequence
    {
        public VoltageRatioInputSequence() : base("VoltageRatioInput")
        {
        }

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageRatioInputAction[] Actions { get; set; }
    }
}
