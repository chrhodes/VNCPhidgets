namespace VNC.Phidget22.Configuration
{
    public class StepperSequence : DeviceClassSequence
    {
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        public string DeviceClass { get; } = "Stepper";
        /// <summary>
        /// Array of Stepper actions in sequence
        /// </summary>
        public StepperAction[]? Actions { get; set; }
    }
}
