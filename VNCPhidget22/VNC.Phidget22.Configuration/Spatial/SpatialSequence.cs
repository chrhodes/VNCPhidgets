namespace VNC.Phidget22.Configuration
{
    public class SpatialSequence : DeviceClassSequence
    {
        public SpatialSequence() : base("Spatial")
        {
        }

        /// <summary>
        /// Array of Spatial actions in sequence
        /// </summary>
        public SpatialAction[] Actions { get; set; }
    }
}
