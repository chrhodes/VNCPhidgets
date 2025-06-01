using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class RFIDSequence : ChannelSequence
    {
        public RFIDSequence() : base("RFID")
        {
        }

        public RFIDSequence(RFIDSequence sequence) : base("RFID", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of RFID actions in sequence
        /// </summary>
        public RFIDAction[]? Actions { get; set; }
    }
}
