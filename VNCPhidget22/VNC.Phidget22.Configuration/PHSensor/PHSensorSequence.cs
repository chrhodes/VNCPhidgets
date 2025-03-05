namespace VNC.Phidget22.Configuration
{
    public class PHSensorSequence : DeviceClassSequence
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
