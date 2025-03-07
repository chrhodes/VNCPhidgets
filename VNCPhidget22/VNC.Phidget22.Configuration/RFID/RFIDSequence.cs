using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class RFIDSequence : ChannelClassSequence
    {
        public RFIDSequence() : base("RFID")
        {
        }

        /// <summary>
        /// Array of RFID actions in sequence
        /// </summary>
        public RFIDAction[] Actions { get; set; }
    }
}
