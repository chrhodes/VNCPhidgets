﻿using System;

namespace VNC.Phidget22.Configuration
{
    public class PhidgetDeviceClassSequence
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

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
        public string DeviceClass { get; set; } = "Generic";

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
