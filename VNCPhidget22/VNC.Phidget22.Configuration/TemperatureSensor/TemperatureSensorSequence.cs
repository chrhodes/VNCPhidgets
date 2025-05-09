using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class TemperatureSensorSequence : ChannelSequence
    {
        public TemperatureSensorSequence() : base("TemperatureSensor")
        {
        }
        public TemperatureSensorSequence(TemperatureSensorSequence sequence) : base("TemperatureSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of TemperatureSensor actions in sequence
        /// </summary>
        public TemperatureSensorAction[] Actions { get; set; }
    }
}
