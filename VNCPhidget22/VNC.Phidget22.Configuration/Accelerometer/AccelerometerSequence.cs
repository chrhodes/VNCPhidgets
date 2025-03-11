﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class AccelerometerSequence : ChannelSequence
    {
        public AccelerometerSequence() : base("Accelerometer")
        {
        }

        /// <summary>
        /// Array of Accelerometer actions in sequence
        /// </summary>
        public AccelerometerAction[] Actions { get; set; }
    }
}
