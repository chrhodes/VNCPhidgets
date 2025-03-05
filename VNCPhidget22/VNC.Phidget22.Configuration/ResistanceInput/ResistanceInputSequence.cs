namespace VNC.Phidget22.Configuration
{
    public class ResistanceInputSequence : DeviceClassSequence
    {
        public ResistanceInputSequence() : base("ResistanceInput")
        {
        }

        /// <summary>
        /// Array of ResistanceInput actions in sequence
        /// </summary>
        public ResistanceInputAction[] Actions { get; set; }
    }
}
