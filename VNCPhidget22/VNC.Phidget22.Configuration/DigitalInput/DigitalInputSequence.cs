using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DigitalInputSequence : ChannelClassSequence
    {
        public DigitalInputSequence() : base("DigitalInput")
        {
        }

        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalInputAction[] Actions { get; set; }
    }
}
