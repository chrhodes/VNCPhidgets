﻿using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Configuration
{
    public class EncoderSequence : ChannelSequence
    {
        public EncoderSequence() : base("Encoder")
        {
        }

        public EncoderSequence(EncoderSequence sequence) : base("Encoder", sequence)
        {
            Actions = sequence.Actions;
        }

        /// <summary>
        /// Array of Encoder actions in sequence
        /// </summary>
        public EncoderAction[]? Actions { get; set; }
    }
}
