using System;

namespace VNC.Phidget22.Configuration
{
    /// <summary>
    /// Actions supported by all Phidget devices.
    /// </summary>
    public class ActionBase
    {
        #region Logging

        public bool? LogPhidgetEvents { get; set; }
        public bool? LogErrorEvents { get; set; }
        public bool? LogPropertyChangeEvents { get; set; }

        public bool? LogDeviceChannelSequence { get; set; }
        public bool? LogChannelAction { get; set; }
        public bool? LogActionVerification { get; set; }

        #endregion

        /// <summary>
        /// Open Phidget (optional)
        /// </summary>
        public bool? Open { get; set; }

        /// <summary>
        /// Close Phidget (optional)
        /// </summary>
        public bool? Close { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
