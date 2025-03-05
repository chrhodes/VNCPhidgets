namespace VNC.Phidget22.Configuration
{
    public class IRSequence : DeviceClassSequence
    {
        public IRSequence() : base("IR")
        {
        }

        /// <summary>
        /// Array of IR actions in sequence
        /// </summary>
        public IRAction[] Actions { get; set; }
    }
}
