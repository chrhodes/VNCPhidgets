using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequence : ChannelSequence
    {
        public DigitalOutputSequence() : base("DigitalOutput")
        {
            //base.DeviceClass = "DigitalOutput";
        }

        public DigitalOutputSequence(DigitalOutputSequence sequence) : base("DigitalOutput", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of DigitalOutput actions in sequence
        /// </summary>
        public DigitalOutputAction[]? Actions { get; set; }
    }
}
