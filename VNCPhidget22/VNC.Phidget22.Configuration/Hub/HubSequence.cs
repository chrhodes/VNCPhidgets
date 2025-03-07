﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class HubSequence : ChannelClassSequence
    {
        public HubSequence() : base("Hub")
        {
        }

        /// <summary>
        /// Array of Hub actions in sequence
        /// </summary>
        public HubAction[] Actions { get; set; }
    }
}
