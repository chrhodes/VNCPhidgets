using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class RCServoSequence : ChannelClassSequence
    {
        public RCServoSequence() : base("RCServo")
        {
        }
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        //public string DeviceClass { get; } = "RCServo";
        /// <summary>
        /// Array of RCServo actions for channel in sequence
        /// </summary>
        public RCServoAction[]? Actions { get; set; }
    }
}
