﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class LCDSequence : ChannelSequence
    {
        public LCDSequence() : base("LCD")
        {
        }

        /// <summary>
        /// Array of LCD actions in sequence
        /// </summary>
        public LCDAction[] Actions { get; set; }
    }
}
