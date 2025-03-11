using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class VoltageInputSequence : ChannelSequence
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
