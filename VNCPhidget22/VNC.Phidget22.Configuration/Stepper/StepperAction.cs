using System;

namespace VNC.Phidget22.Configuration
{
    public class StepperAction
    {
        #region Logging

        public bool? LogPhidgetEvents { get; set; }
        public bool? LogErrorEvents { get; set; }
        public bool? LogPropertyChangeEvents { get; set; }

        public bool? LogPositionChangeEvents { get; set; }
        public bool? LogVelocityChangeEvents { get; set; }
        public bool? LogStoppedEvents { get; set; }

        public bool? LogPerformanceSequence { get; set; }
        public bool? LogSequenceAction { get; set; }
        public bool? LogActionVerification { get; set; }

        #endregion

        /// <summary>
        /// Degrees of rotation for one full step
        /// which is 16 micro steps (1/16)
        /// </summary>
        public Double? StepAngle { get; set; }

        /// <summary>
        /// Makes it easier to control Stepper
        /// </summary>
        public Double? RescaleFactor { get; set; }

        /// <summary>
        /// Open RCServo (optional)
        /// </summary>
        public bool? Open { get; set; }

        /// <summary>
        /// Close RCServo (optional)
        /// </summary>
        public bool? Close { get; set; }

        /// <summary>
        /// Engage Stepper (optional)
        /// </summary>
        public bool? Engaged { get; set; }

        /// <summary>
        /// Servo Acceleration (optional)
        /// </summary>
        public Double? Acceleration { get; set; }

        /// <summary>
        /// Acceleration (+/-) from current Acceleration (optional)
        /// </summary>
        public Double? RelativeAcceleration { get; set; }

        /// <summary>
        /// Servo Velocity (optional)
        /// </summary>
        public Double? VelocityLimit { get; set; }

        /// <summary>
        /// VelocityLimit (+/-) from current VelocityLimit (optional)
        /// </summary>
        public Double? RelativeVelocityLimit { get; set; }

        /// <summary>
        /// Servo Velocity (optional)
        /// </summary>
        public Double? CurrentLimit { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public Int64? CurrentPosition { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public Int64? TargetPosition { get; set; }

        /// <summary>
        /// Position (+/-)  from current Position (optional)
        /// in 1/16 step Micro Steps
        /// </summary>
        public Int64? RelativePosition { get; set; }

        /// <summary>
        /// AddPositionOffset() (optional)
        /// </summary>
        public Double? AddPositionOffset { get; set; }

        /// <summary>
        /// Position (+/-)  from current Position (optional)
        /// in degrees
        /// </summary>
        public Int64? RelativeTargetDegrees { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        //public Double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
