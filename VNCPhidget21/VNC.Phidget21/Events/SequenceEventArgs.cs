using VNCPhidget21.Configuration;

namespace VNC.Phidget.Events
{
    public class SequenceEventArgs
    {
        public AdvancedServoSequence AdvancedServoSequence { get; set; }
        public InterfaceKitSequence InterfaceKitSequence { get; set; }
        public StepperSequence stepperSequence { get; set; }
    }
}
