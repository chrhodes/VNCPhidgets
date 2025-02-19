namespace VNC.Phidget22.Configuration
{
    public class InterfaceKitSequence : DeviceClassSequence
    {
        public InterfaceKitSequence() : base("InterfaceKit")
        {
        }
        /// <summary>
        /// Array of InterfaceKit actions in sequence
        /// </summary>
        public InterfaceKitAction[] Actions { get; set; }
    }
}
