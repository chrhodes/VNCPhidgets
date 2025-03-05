namespace VNC.Phidget22.Configuration
{
    public class DistanceSensorSequence : DeviceClassSequence
    {
        public DistanceSensorSequence() : base("DistanceSensor")
        {
        }

        /// <summary>
        /// Array of DistanceSensor actions in sequence
        /// </summary>
        public DistanceSensorAction[] Actions { get; set; }
    }
}
