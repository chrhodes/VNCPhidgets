using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DistanceSensorSequence : ChannelClassSequence
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
