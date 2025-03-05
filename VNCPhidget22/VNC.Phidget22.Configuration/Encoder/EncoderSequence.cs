namespace VNC.Phidget22.Configuration
{
    public class EncoderSequence : DeviceClassSequence
    {
        public EncoderSequence() : base("Encoder")
        {
        }

        /// <summary>
        /// Array of Encoder actions in sequence
        /// </summary>
        public EncoderAction[] Actions { get; set; }
    }
}
