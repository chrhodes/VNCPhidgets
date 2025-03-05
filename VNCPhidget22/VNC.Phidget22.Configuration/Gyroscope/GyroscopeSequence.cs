namespace VNC.Phidget22.Configuration
{
    public class GyroscopeSequence : DeviceClassSequence
    {
        public GyroscopeSequence() : base("Gyroscope")
        {
        }

        /// <summary>
        /// Array of Gyroscope actions in sequence
        /// </summary>
        public GyroscopeAction[] Actions { get; set; }
    }
}
