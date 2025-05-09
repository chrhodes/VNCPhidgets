using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class SpatialSequence : ChannelSequence
    {
        public SpatialSequence() : base("Spatial")
        {
        }

        public SpatialSequence(SpatialSequence sequence) : base("Spatial", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of Spatial actions in sequence
        /// </summary>
        public SpatialAction[] Actions { get; set; }
    }
}
