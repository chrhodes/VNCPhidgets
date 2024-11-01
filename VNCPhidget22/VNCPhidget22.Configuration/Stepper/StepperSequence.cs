namespace VNCPhidget22.Configuration
{
    public class StepperSequence : Phidget22equenceBase
    {
        /// <summary>
        /// Array of Stepper actions in sequence
        /// </summary>
        public StepperAction[]? Actions { get; set; }
    }
}
