using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequence : ChannelClassSequence
    {
        public DigitalOutputSequence() : base("DigitalOutput")
        {
            //base.DeviceClass = "DigitalOutput";
        }

        /// <summary>
        /// Array of DigitalInput actions in sequence
        /// </summary>
        public DigitalOutputAction[] Actions { get; set; }
    }
}
