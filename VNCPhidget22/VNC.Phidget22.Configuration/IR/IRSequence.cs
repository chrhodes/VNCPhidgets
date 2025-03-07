using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class IRSequence : ChannelClassSequence
    {
        public IRSequence() : base("IR")
        {
        }

        /// <summary>
        /// Array of IR actions in sequence
        /// </summary>
        public IRAction[] Actions { get; set; }
    }
}
