namespace VNC.Phidget22.Configuration
{
    public class RFIDSequence : DeviceClassSequence
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
