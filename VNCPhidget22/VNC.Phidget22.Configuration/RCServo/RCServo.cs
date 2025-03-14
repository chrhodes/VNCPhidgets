using System;

namespace VNC.Phidget22.Configuration
{
    public class RCServo
    {
        public string Name { get; set; } = "RCSERVO NAME";
        public Int32 SerialNumber { get; set; }
        public Boolean Open { get; set; } = false;
    }
}
