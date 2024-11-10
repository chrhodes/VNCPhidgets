using System;

namespace VNC.Phidget22.Configuration
{
    public class AdvancedServoServoAction
    {
        /// <summary>
        /// Index of servo on board 
        /// </summary>
        public int ServoIndex { get; set; }

        // TODO(crhodes)
        // How to do this going forward.  May need our own enum
        //public Phidget22.ServoServo.ServoType? ServoType { get; set; }

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
        /// TargetPosition (optional)
        /// </summary>
        public Double? PositionMin { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public Double? TargetPosition { get; set; }

        /// <summary>
        /// Position (+/-)  from current Position (optional)
        /// </summary>
        public Double? RelativePosition { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public Double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
