namespace VNC.Phidget22.Configuration
{
    public class SoundSensorSequence : DeviceClassSequence
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
