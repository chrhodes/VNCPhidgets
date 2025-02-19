using System.Security.Cryptography;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequence : DeviceClassSequence
    {
        public DigitalOutputSequence() : base("DigitalOutput")
        {
            //base.DeviceClass = "DigitalOutput";
        }
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        //public string DeviceClass { get; } = "DigitalOutput";
        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalOutputAction[] Actions { get; set; }
    }
}
