using VNC.Phidget22.Configuration;

namespace VNC.Phidget22.Events
{
    public class SequenceEventArgs
    {
        //public AdvancedServoSequence AdvancedServoSequence { get; set; }
        public DigitalInputSequence DigitalInputSequence { get; set; }
        public DigitalOutputSequence DigitalOutputSequence { get; set; }
        public RCServoSequence RCServoSequence { get; set; }
        public StepperSequence StepperSequence { get; set; }
        public VoltageInputSequence VoltageInputSequence { get; set; }
        public VoltageRatioInputSequence VoltageRatioInputSequence { get; set; }
        public VoltageOutputSequence VoltageOutputSequnce { get; set; }
    }
}
