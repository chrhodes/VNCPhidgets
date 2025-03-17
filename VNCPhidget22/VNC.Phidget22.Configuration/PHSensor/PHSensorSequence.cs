using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class PHSensorSequence : ChannelSequence
    {
        public PHSensorSequence() : base("PHSensor")
        {
        }

        /// <summary>
        /// Array of PHSensor actions in sequence
        /// </summary>
        public PHSensorAction[] Actions { get; set; }
    }
}
