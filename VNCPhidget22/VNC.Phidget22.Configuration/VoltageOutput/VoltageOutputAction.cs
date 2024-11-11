using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputAction
    {
        /// <summary>
        /// Index of VoltageOutput on board 
        /// </summary>
        public int VoltageOutIndex { get; set; }
   
        /// <summary>
        /// Set DigitalOut value
        /// </summary>
        public bool? VoltageOut { get; set; }


        /// <summary>
        /// Duration of step in ms (sleep time after step)
        /// </summary>
        public Int32? Duration { get; set; } // ms
    }
}
