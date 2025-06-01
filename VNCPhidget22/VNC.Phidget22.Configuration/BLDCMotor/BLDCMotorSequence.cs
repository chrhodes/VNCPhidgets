using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class BLDCMotorSequence : ChannelSequence
    {
        public BLDCMotorSequence() : base("BLDCMotor")
        {
        }
        public BLDCMotorSequence(BLDCMotorSequence sequence) : base("BLDCMotor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of BLDCMotor actions in sequence
        /// </summary>
        public BLDCMotorAction[]? Actions { get; set; }
    }
}
