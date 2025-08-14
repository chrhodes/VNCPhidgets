using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class CurrentInputSequence : ChannelSequence
    {
        public CurrentInputSequence() : base("CurrentInputSequence")
        {
        }

        public CurrentInputSequence(CurrentInputSequence sequence) : base("CurrentInputSequence", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of CurrentInput actions in sequence
        /// </summary>
        public CurrentInputAction[]? Actions { get; set; }
    }
}
