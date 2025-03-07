using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class CurrentInputSequence : ChannelClassSequence
    {
        public CurrentInputSequence() : base("CurrentInput")
        {
        }

        /// <summary>
        /// Array of CurrentInput actions in sequence
        /// </summary>
        public CurrentInputAction[] Actions { get; set; }
    }
}
