using System;

namespace VNC.Phidget22.Configuration
{
    public class TemperatureSensorAction
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
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
