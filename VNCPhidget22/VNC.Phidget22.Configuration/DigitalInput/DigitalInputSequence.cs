namespace VNC.Phidget22.Configuration
{
    public class DigitalInputSequence : DeviceClassSequence
    {
        public DigitalInputSequence() : base("DigitalInput")
        {
        }
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        //public string DeviceClass { get; } = "DigitalInput";
        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalInputAction[] Actions { get; set; }
    }
}
