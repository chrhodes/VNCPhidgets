using VNCPhidget22.Configuration;

namespace VNC.Phidget22.Events
{
    public class SequenceEventArgs
    {
        public AdvancedServoSequence AdvancedServoSequence { get; set; }
        public InterfaceKitSequence InterfaceKitSequence { get; set; }
        public StepperSequence stepperSequence { get; set; }
    }
}
