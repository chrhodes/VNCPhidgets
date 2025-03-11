using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class BLDCMotorSequence : ChannelSequence
    {
        public BLDCMotorSequence() : base("BLDCMotor")
        {
        }

        /// <summary>
        /// Array of BLDCMotor actions in sequence
        /// </summary>
        public BLDCMotorAction[] Actions { get; set; }
    }
}
