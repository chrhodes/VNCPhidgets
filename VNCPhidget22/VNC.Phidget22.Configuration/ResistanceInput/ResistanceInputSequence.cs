using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class ResistanceInputSequence : ChannelSequence
    {
        public ResistanceInputSequence() : base("ResistanceInput")
        {
        }

        public ResistanceInputSequence(ResistanceInputSequence sequence) : base("ResistanceInput", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of ResistanceInput actions in sequence
        /// </summary>
        public ResistanceInputAction[] Actions { get; set; }
    }
}
