using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class EncoderSequence : ChannelClassSequence
    {
        public EncoderSequence() : base("Encoder")
        {
        }

        /// <summary>
        /// Array of Encoder actions in sequence
        /// </summary>
        public EncoderAction[] Actions { get; set; }
    }
}
