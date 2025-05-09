using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class FrequencyCounterSequence : ChannelSequence
    {
        public FrequencyCounterSequence() : base("FrequencyCounter")
        {
        }

        public FrequencyCounterSequence(FrequencyCounterSequence sequence) : base("FrequencyCounter", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of FrequencyCounter actions in sequence
        /// </summary>
        public FrequencyCounterAction[] Actions { get; set; }
    }
}
