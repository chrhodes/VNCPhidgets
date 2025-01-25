using System;
using System.Collections.Generic;

namespace VNC.Phidget22.Configuration
{ 
    public enum RCServoType
    {
        DEFAULT,
        HITEC_HS422,
        SG90,
        USER_DEFINED
    }

    public struct RCServoPulseWidths
    {
        public RCServoType RCServoType;
        public Double MinPulseWidth;
        public Double MaxPulseWidth;
    }
}
