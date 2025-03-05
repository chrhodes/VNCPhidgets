namespace VNC.Phidget22.Configuration
{
    public class HumiditySensorSequence : DeviceClassSequence
    {
        public HumiditySensorSequence() : base("HumiditySensor")
        {
        }

        /// <summary>
        /// Array of HumiditySensor actions in sequence
        /// </summary>
        public HumiditySensorAction[] Actions { get; set; }
    }
}
