using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageInputAction
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


        /// <summary>
        /// Set DigitalOut value
        /// </summary>
        public bool? DigitalOut { get; set; }

        ///// <summary>
        ///// Servo Acceleration (optional)
        ///// </summary>
        //public Double? Acceleration { get; set; }

        ///// <summary>
        ///// Servo Velocity (optional)
        ///// </summary>
        //public Double? VelocityLimit { get; set; }

        ///// <summary>
        ///// TargetPosition (optional)
        ///// </summary>
        //public Double? PositionMin { get; set; }

        ///// <summary>
        ///// TargetPosition (optional)
        ///// </summary>
        //public Double? TargetPosition { get; set; }

        ///// <summary>
        ///// TargetPosition (optional)
        ///// </summary>
        //public Double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
