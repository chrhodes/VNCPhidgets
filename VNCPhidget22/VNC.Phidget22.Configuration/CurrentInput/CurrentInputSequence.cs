namespace VNC.Phidget22.Configuration
{
    public class CurrentInputSequence : DeviceClassSequence
    {
        public CurrentInputSequence() : base("CurrentInput")
        {
        }

        /// <summary>
        /// Array of CurrentInput actions in sequence
        /// </summary>
        public CurrentInputAction[] Actions { get; set; }
    }
}
