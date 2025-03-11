using System;

using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class PhidgetDeviceClassSequence
    {
        /// <summary>
        /// Name of Sequence.  If null, must specify a ChannelClassSequence
        /// </summary>
        public string? Name { get; set; } = null;

        // NOTE(crhodes)
        // This did not work out
        // Would have to have a derived type, e.g. RCServoClassSequence

        ///// <summary>
        ///// ChannelClassSequence.  If null, must specify a Name
        ///// </summary>
        //public ChannelClassSequence? ChannelClassSequence { get; set; } = null;

        /// <summary>
        /// SerialNumber of PhidgetDevice that will run sequencee
        /// </summary>
        public Int32 SerialNumber { get; set; }

        // NOTE(crhodes)
        // Not sure we need this as SerialNumber is unique to device
        // and we likely look things up by Host,SerialNumber
        /// <summary>
        /// Phidget DeviceClass
        /// </summary>
        //public string DeviceClass { get; set; } = "Generic";

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

        /// <summary>
        /// Close Phidget at end of sequence loops
        /// </summary>
        public Boolean ClosePhidget { get; set; } = false;
    }
}
