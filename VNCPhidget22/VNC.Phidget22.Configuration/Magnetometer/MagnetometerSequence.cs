namespace VNC.Phidget22.Configuration
{
    public class MagnetometerSequence : DeviceClassSequence
    {
        public MagnetometerSequence() : base("Magnetometer")
        {
        }

        /// <summary>
        /// Array of Magnetometer actions in sequence
        /// </summary>
        public MagnetometerAction[] Actions { get; set; }
    }
}
