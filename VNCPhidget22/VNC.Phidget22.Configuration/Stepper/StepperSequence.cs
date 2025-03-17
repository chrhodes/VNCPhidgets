using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class StepperSequence : ChannelSequence
    {
        public StepperSequence() : base("Stepper")
        {
        }

        //public string DeviceClass { get; } = "Stepper";
        /// <summary>
        /// Array of Stepper actions in sequence
        /// </summary>
        public StepperAction[]? Actions { get; set; }
    }
}
