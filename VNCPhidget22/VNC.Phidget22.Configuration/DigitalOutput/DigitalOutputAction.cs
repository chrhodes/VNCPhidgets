using System;

namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputAction : ActionBase
    {
        #region Logging

        // TODO(crhodes)
        // Add Device specific logging

        #endregion

        /// <summary>
        /// Set DigitalOut value
        /// </summary>
        public Boolean? DigitalOut { get; set; }
    }
}
