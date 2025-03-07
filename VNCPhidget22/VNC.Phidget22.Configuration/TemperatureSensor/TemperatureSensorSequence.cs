using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class TemperatureSensorSequence : ChannelClassSequence
    {
        public TemperatureSensorSequence() : base("TemperatureSensor")
        {
        }

        /// <summary>
        /// Array of TemperatureSensor actions in sequence
        /// </summary>
        public TemperatureSensorAction[] Actions { get; set; }
    }
}
