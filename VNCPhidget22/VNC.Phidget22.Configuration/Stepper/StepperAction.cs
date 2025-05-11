using System;

namespace VNC.Phidget22.Configuration
{
    public class StepperAction : ActionBase
    {
        #region Logging

        public Boolean? LogPositionChangeEvents { get; set; }
        public Boolean? LogVelocityChangeEvents { get; set; }
        public Boolean? LogStoppedEvents { get; set; }

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
        public Boolean? Open { get; set; }

        /// <summary>
        /// Close RCServo (optional)
        /// </summary>
        public Boolean? Close { get; set; }

        /// <summary>
        /// Engage Stepper (optional)
        /// </summary>
        public Boolean? Engaged { get; set; }

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

    }
}
