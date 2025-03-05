namespace VNC.Phidget22.Configuration
{
    public class HubSequence : DeviceClassSequence
    {
        public HubSequence() : base("Hub")
        {
        }

        /// <summary>
        /// Array of Hub actions in sequence
        /// </summary>
        public HubAction[] Actions { get; set; }
    }
}
