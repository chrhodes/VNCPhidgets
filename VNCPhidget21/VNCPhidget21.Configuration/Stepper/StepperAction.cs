using System;

namespace VNCPhidget21.Configuration
{
    public class StepperAction
    {
        /// <summary>
        /// Index of stepper on board (likley 1)
        /// </summary>
        public int StepperIndex { get; set; }

        /// <summary>
        /// Degrees of rotation for one full step
        /// 16 micro steps (1/16)
        /// </summary>
        public int StepAngle { get; set; }

        /// <summary>
        /// Engage Servo (optional)
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
        public Int64? RelativeTargetPosition { get; set; }

        /// <summary>
        /// Position (+/-)  from current Position (optional)
        /// in degrees
        /// </summary>
        public Int64? RelativeTargetDegrees { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        //public double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
