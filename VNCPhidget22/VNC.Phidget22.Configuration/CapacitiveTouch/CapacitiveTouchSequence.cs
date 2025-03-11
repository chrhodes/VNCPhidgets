using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class CapacitiveTouchSequence : ChannelSequence
    {
        public CapacitiveTouchSequence() : base("CapacitiveTouch")
        {
        }

        /// <summary>
        /// Array of CapacitiveTouch actions in sequence
        /// </summary>
        public CapacitiveTouchAction[] Actions { get; set; }
    }
}
