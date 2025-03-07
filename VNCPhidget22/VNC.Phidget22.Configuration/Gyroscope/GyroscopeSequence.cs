using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class GyroscopeSequence : ChannelClassSequence
    {
        public GyroscopeSequence() : base("Gyroscope")
        {
        }

        /// <summary>
        /// Array of Gyroscope actions in sequence
        /// </summary>
        public GyroscopeAction[] Actions { get; set; }
    }
}
