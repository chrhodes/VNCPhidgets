using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class GPSSequence : ChannelSequence
    {
        public GPSSequence() : base("GPS")
        {
        }

        public GPSSequence(GPSSequence sequence) : base("GPS", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of GPS actions in sequence
        /// </summary>
        public GPSAction[] Actions { get; set; }
    }
}
