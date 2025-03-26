using System;
using System.Runtime.CompilerServices;

namespace VNC.Phidget22.Configuration
{
    public class RCServoAction
    {
        #region Logging

        public Boolean? LogPhidgetEvents { get; set; }
        public Boolean? LogErrorEvents { get; set; }
        public Boolean? LogPropertyChangeEvents { get; set; }

        public Boolean? LogPositionChangeEvents { get; set; }
        public Boolean? LogVelocityChangeEvents { get; set; }
        public Boolean? LogTargetPositionReachedEvents { get; set; }

        public Boolean? LogDeviceChannelSequence { get; set; }
        public Boolean? LogChannelAction { get; set; }
        public Boolean? LogActionVerification { get; set; }

        #endregion

        public RCServoType? RCServoType { get; set; }

        // TODO(crhodes)
        // Do we want to support changing MinPulseWidth and MaxPulseWidth?

        /// <summary>
        /// Open RCServo (optional)
        /// </summary>
        public Boolean? Open { get; set; }

        /// <summary>
        /// Close RCServo (optional)
        /// </summary>
        public Boolean? Close { get; set; }

        /// <summary>
        /// Engage Servo (optional)
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
        /// Scaled Minimum TargetPosition (optional)
        /// </summary>
        public Double? PositionScaleMin { get; set; }

        /// <summary>
        /// Minimum TargetPosition (optional)
        /// </summary>
        public Double? PositionStopMin { get; set; }

        /// <summary>
        /// TargetPosition (optional)
        /// </summary>
        public Double? TargetPosition { get; set; }

        /// <summary>
        /// Position (+/-)  from current Position (optional)
        /// </summary>
        public Double? RelativePosition { get; set; }

        /// <summary>
        /// Maximum TargetPosition (optional)
        /// </summary>
        public Double? PositionStopMax { get; set; }

        /// <summary>
        /// Scaled Maximum TargetPosition (optional)
        /// </summary>
        public Double? PositionScaleMax { get; set; }

        public Boolean? SpeedRampingState { get; set; }

        /// <summary>
        /// Duration of sleep in ms (sleep time after Action) (optional)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
