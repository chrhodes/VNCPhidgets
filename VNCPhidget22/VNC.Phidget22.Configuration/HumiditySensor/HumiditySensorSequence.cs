﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class HumiditySensorSequence : ChannelSequence
    {
        public HumiditySensorSequence() : base("HumiditySensor")
        {
        }

        public HumiditySensorSequence(HumiditySensorSequence sequence) : base("HumiditySensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of HumiditySensor actions in sequence
        /// </summary>
        public HumiditySensorAction[]? Actions { get; set; }
    }
}
