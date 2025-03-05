namespace VNC.Phidget22.Configuration
{
    public class FrequencyCounterSequence : DeviceClassSequence
    {
        public FrequencyCounterSequence() : base("FrequencyCounter")
        {
        }

        /// <summary>
        /// Array of FrequencyCounter actions in sequence
        /// </summary>
        public FrequencyCounterAction[] Actions { get; set; }
    }
}
