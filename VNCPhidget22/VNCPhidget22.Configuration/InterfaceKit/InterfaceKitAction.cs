using System;

namespace VNCPhidget22.Configuration
{
    public class InterfaceKitAction
    {
        /// <summary>
        /// Index of DigitalOutput on board 
        /// </summary>
        public int DigitalOutIndex { get; set; }
   
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
        //public double? PositionMin { get; set; }

        ///// <summary>
        ///// TargetPosition (optional)
        ///// </summary>
        //public double? TargetPosition { get; set; }

        ///// <summary>
        ///// TargetPosition (optional)
        ///// </summary>
        //public double? PositionMax { get; set; }

        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
