using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class PowerGuardSequence : ChannelSequence
    {
        public PowerGuardSequence() : base("PowerGuard")
        {
        }

        public PowerGuardSequence(PowerGuardSequence sequence) : base("PowerGuard", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of PowerGuard actions in sequence
        /// </summary>
        public PowerGuardAction[] Actions { get; set; }
    }
}
