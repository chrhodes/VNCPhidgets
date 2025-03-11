using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class SoundSensorSequence : ChannelSequence
    {
        public SoundSensorSequence() : base("SoundSensor")
        {
        }

        /// <summary>
        /// Array of SoundSensor actions in sequence
        /// </summary>
        public SoundSensorAction[] Actions { get; set; }
    }
}
