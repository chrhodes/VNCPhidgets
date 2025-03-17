using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class MagnetometerSequence : ChannelSequence
    {
        public MagnetometerSequence() : base("Magnetometer")
        {
        }

        /// <summary>
        /// Array of Magnetometer actions in sequence
        /// </summary>
        public MagnetometerAction[] Actions { get; set; }
    }
}
