using System;

namespace VNC.Phidget22.Configuration
{
    public class Stepper
    {
        public string Name { get; set; } = "STEPPER NAME";
        public Int32 SerialNumber { get; set; }
        public Boolean Open { get; set; } = true;
    }
}
