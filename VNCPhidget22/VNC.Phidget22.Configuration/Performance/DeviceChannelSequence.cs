using System;

using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DeviceChannelSequence
    {
        /// <summary>
        /// Name of Sequence.
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Description of DeviceChannelSequence (optional)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Notes on using DeviceChannelSequence (optional)
        /// </summary>
        public string? UsageNotes { get; set; }

        /// <summary>
        /// SerialNumber of PhidgetDevice that will run sequencee
        /// </summary>
        public Int32? SerialNumber { get; set; }

        /// <summary>
        /// HubPort of PhidgetDevice that will run sequence
        /// If null, Name is used to lookup sequence 
        /// which must specify a channel
        /// </summary>
        public Int32? HubPort { get; set; } = null;

        /// <summary>
        /// Channel of PhidgetDevice that will run sequence
        /// If null, Name is used to lookup sequence 
        /// which must specify a channel
        /// </summary>
        public Int32? Channel { get; set; } = null;

        // NOTE(crhodes)
        // May want to bring more parameters up here, e.g. Logging
        // DataRate DataInterval, etc.

        /// <summary>
        /// Phidget ChannelClass
        /// </summary>
        public string ChannelClass { get; set; } = "Generic";

        /// <summary>
        /// Number of loops of Sequence
        /// </summary>
        public Int32 SequenceLoops { get; set; } = 1;

        /// <summary>
        /// Duration of Sequence in ms (sleep time after Loops completed)
        /// </summary>
        public Int32? Duration { get; set; }

        // TODO(crhodes)
        // Do we really need this here?

        /// <summary>
        /// Close Phidget at end of sequence loops
        /// </summary>
        public Boolean ClosePhidget { get; set; } = false;
    }
}
