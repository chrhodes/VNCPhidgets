namespace VNC.Phidget22.Configuration
{
    public class LightSensorSequence : DeviceClassSequence
    {
        public LightSensorSequence() : base("LightSensor")
        {
        }

        /// <summary>
        /// Array of LightSensor actions in sequence
        /// </summary>
        public LightSensorAction[] Actions { get; set; }
    }
}
