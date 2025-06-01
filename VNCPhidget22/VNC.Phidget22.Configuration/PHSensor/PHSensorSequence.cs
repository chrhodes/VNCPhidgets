using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class PHSensorSequence : ChannelSequence
    {
        public PHSensorSequence() : base("PHSensor")
        {
        }

        public PHSensorSequence(PHSensorSequence sequence) : base("PHSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of PHSensor actions in sequence
        /// </summary>
        public PHSensorAction[]? Actions { get; set; }
    }
}
