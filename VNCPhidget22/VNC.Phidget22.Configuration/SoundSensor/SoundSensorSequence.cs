using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class SoundSensorSequence : ChannelSequence
    {
        public SoundSensorSequence() : base("SoundSensor")
        {
        }

        public SoundSensorSequence(SoundSensorSequence sequence) : base("SoundSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of SoundSensor actions in sequence
        /// </summary>
        public SoundSensorAction[] Actions { get; set; }
    }
}
