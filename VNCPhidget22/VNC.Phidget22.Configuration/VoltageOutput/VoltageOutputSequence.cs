﻿using Phidget22;

namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequence : DeviceClassSequence
    {
        /// <summary>
        /// Phidet22.DeviceClass
        /// </summary>
        public string DeviceClass { get; } = "VoltageOutput";

        /// <summary>
        /// Array of VoltageOutput actions in sequence
        /// </summary>
        public VoltageOutputAction[] Actions { get; set; }
    }
}
