namespace VNC.Phidget22.Configuration
{
    public class RCServoSequence : PhidgetSequenceBase
    {
        /// <summary>
        /// Array of RCServo actions for channel in sequence
        /// </summary>
        public RCServoAction[]? Actions { get; set; }
    }
}
