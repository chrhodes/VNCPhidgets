using Phidget22;

using VNC.Phidget22.Configuration.Performance;


namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequence : ChannelSequence
    {
        public VoltageOutputSequence() : base("VoltageOutput")
        {
        }

        public VoltageOutputSequence(VoltageOutputSequence sequence) : base("VoltageOutput", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageOutputAction[] Actions { get; set; }
    }
}
