using System;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputAction
    {
        #region Logging

        public bool? LogPhidgetEvents { get; set; }
        public bool? LogErrorEvents { get; set; }
        public bool? LogPropertyChangeEvents { get; set; }

        public bool? LogPerformanceSequence { get; set; }
        public bool? LogSequenceAction { get; set; }
        public bool? LogActionVerification { get; set; }

        #endregion

        /// <summary>
        /// Set DigitalOut value
        /// </summary>
        public bool? DigitalOut { get; set; }

        /// <summary>
        /// Duration of sleep in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
