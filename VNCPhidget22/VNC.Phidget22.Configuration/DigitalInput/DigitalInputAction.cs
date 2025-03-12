using System;

namespace VNC.Phidget22.Configuration
{
    public class DigitalInputAction
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
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
