﻿using System;
using System.Threading.Channels;

namespace VNC.Phidget22.Configuration
{
    public class StepperConfiguration
    {
        public short Channel { get; set; }
        //public bool IsRemote { get; set; } = true;
        //public bool IsLocal { get; set; } = false;
    }
}