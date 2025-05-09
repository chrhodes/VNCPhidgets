using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DigitalInputSequence : ChannelSequence
    {
        public DigitalInputSequence() : base("DigitalInput")
        {
        }

        public DigitalInputSequence(DigitalInputSequence sequence) : base("DigitalInput", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalInputAction[] Actions { get; set; }
    }
}
