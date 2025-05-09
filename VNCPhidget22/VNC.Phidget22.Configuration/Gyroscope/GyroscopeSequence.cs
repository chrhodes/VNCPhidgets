using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class GyroscopeSequence : ChannelSequence
    {
        public GyroscopeSequence() : base("Gyroscope")
        {
        }

        public GyroscopeSequence(GyroscopeSequence sequence) : base("Gyroscope", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of Gyroscope actions in sequence
        /// </summary>
        public GyroscopeAction[] Actions { get; set; }
    }
}
