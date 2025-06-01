using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class PressureSensorSequence : ChannelSequence
    {
        public PressureSensorSequence() : base("PressureSensor")
        {
        }

        public PressureSensorSequence(PressureSensorSequence sequence) : base("PressureSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of PressureSensor actions in sequence
        /// </summary>
        public PressureSensorAction[]? Actions { get; set; }
    }
}
