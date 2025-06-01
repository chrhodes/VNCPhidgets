using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class IRSequence : ChannelSequence
    {
        public IRSequence() : base("IR")
        {
        }

        public IRSequence(IRSequence sequence) : base("IR", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of IR actions in sequence
        /// </summary>
        public IRAction[]? Actions { get; set; }
    }
}
