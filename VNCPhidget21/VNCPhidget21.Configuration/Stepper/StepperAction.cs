using System;

namespace VNCPhidget21.Configuration
{
    public class StepperAction
    {
        /// <summary>
        /// Index of servo on board 
        /// </summary>
        public int ServoIndex { get; set; }

        /// <summary>
        /// Engage Servo (optional)
        /// </summary>
        public bool? Engaged { get; set; }

        /// <summary>
        /// Servo Acceleration (optional)
        /// </summary>
        public Double? Acceleration { get; set; }

        /// <summary>
        /// Servo Velocity (optional)
        /// </summary>
        public Double? VelocityLimit { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public double? PositionMin { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public double? TargetPosition { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
