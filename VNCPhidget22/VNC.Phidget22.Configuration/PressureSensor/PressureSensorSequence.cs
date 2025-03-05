namespace VNC.Phidget22.Configuration
{
    public class PressureSensorSequence : DeviceClassSequence
    {
        public PressureSensorSequence() : base("PressureSensor")
        {
        }

        /// <summary>
        /// Array of PressureSensor actions in sequence
        /// </summary>
        public PressureSensorAction[] Actions { get; set; }
    }
}
