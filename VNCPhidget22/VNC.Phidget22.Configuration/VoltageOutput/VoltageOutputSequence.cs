using Phidget22;

namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequence : DeviceClassSequence
    {
        public VoltageOutputSequence() : base("VoltageOutput")
        {
        }

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageOutputAction[] Actions { get; set; }
    }
}
