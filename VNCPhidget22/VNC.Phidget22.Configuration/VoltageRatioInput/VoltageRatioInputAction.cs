using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageRatioInputAction
    {
        #region Logging

        public bool? LogPhidgetEvents { get; set; }
        public bool? LogErrorEvents { get; set; }
        public bool? LogPropertyChangeEvents { get; set; }

        public bool? LogSensorChangeEvents { get; set; }
        public bool? LogVoltageChangeEvents { get; set; }

        public bool? LogPerformanceSequence { get; set; }
        public bool? LogSequenceAction { get; set; }
        public bool? LogActionVerification { get; set; }

        #endregion


    }
}
