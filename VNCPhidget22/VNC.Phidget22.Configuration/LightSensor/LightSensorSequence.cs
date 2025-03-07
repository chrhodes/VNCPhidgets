﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class LightSensorSequence : ChannelClassSequence
    {
        public LightSensorSequence() : base("LightSensor")
        {
        }

        /// <summary>
        /// Array of LightSensor actions in sequence
        /// </summary>
        public LightSensorAction[] Actions { get; set; }
    }
}
