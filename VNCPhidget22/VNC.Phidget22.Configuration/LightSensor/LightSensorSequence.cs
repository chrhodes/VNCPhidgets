using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class LightSensorSequence : ChannelSequence
    {
        public LightSensorSequence() : base("LightSensor")
        {
        }

        public LightSensorSequence(LightSensorSequence sequence) : base("LightSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of LightSensor actions in sequence
        /// </summary>
        public LightSensorAction[] Actions { get; set; }
    }
}
