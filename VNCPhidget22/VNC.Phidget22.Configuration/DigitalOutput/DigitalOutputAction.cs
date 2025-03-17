using System;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputAction
    {
        #region Logging

        public Boolean? LogPhidgetEvents { get; set; }
        public Boolean? LogErrorEvents { get; set; }
        public Boolean? LogPropertyChangeEvents { get; set; }

        public Boolean? LogDeviceChannelSequence { get; set; }
        public Boolean? LogChannelAction { get; set; }
        public Boolean? LogActionVerification { get; set; }

        #endregion

        /// <summary>
        /// Set DigitalOut value
        /// </summary>
        public Boolean? DigitalOut { get; set; }

        /// <summary>
        /// Duration of sleep in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
