using Phidget22;

using VNC.Phidget22.Configuration.Performance;


namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequence : ChannelClassSequence
    {
        public VoltageOutputSequence() : base("VoltageOutput")
        {
        }

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageOutputAction[] Actions { get; set; }
    }
}
