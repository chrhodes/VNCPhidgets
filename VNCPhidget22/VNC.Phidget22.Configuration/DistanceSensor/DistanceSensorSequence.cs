﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class DistanceSensorSequence : ChannelSequence
    {
        public DistanceSensorSequence() : base("DistanceSensor")
        {
        }

        public DistanceSensorSequence(DistanceSensorSequence sequence) : base("DistanceSensor", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of DistanceSensor actions in sequence
        /// </summary>
        public DistanceSensorAction[]? Actions { get; set; }
    }
}
