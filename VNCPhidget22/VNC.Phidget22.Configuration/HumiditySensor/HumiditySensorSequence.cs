using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class HumiditySensorSequence : ChannelClassSequence
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
