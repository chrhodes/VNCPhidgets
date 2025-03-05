namespace VNC.Phidget22.Configuration
{
    public class GPSSequence : DeviceClassSequence
    {
        public GPSSequence() : base("GPS")
        {
        }

        /// <summary>
        /// Array of GPS actions in sequence
        /// </summary>
        public GPSAction[] Actions { get; set; }
    }
}
