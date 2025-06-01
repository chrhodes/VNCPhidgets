using Phidget22;

using VNC.Phidget22.Configuration.Performance;


namespace VNC.Phidget22.Configuration
{
    public class VoltageRatioInputSequence : ChannelSequence
    {
        public VoltageRatioInputSequence() : base("VoltageRatioInput")
        {
        }
        public VoltageRatioInputSequence(VoltageRatioInputSequence sequence) : base("VoltageRatioInput")
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageRatioInputAction[]? Actions { get; set; }
    }
}
