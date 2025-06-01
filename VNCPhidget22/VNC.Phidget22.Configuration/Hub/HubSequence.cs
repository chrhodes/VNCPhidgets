using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class HubSequence : ChannelSequence
    {
        public HubSequence() : base("Hub")
        {
        }

        public HubSequence(HubSequence sequence) : base("Hub", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of Hub actions in sequence
        /// </summary>
        public HubAction[]? Actions { get; set; }
    }
}
