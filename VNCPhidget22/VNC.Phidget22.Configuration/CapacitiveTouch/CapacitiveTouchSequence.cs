using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class CapacitiveTouchSequence : ChannelSequence
    {
        public CapacitiveTouchSequence() : base("CapacitiveTouch")
        {
        }

        public CapacitiveTouchSequence(CapacitiveTouchSequence sequence) : base("CapacitiveTouch", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of CapacitiveTouch actions in sequence
        /// </summary>
        public CapacitiveTouchAction[] Actions { get; set; }
    }
}
