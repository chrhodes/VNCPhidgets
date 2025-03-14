using System;

namespace VNC.Phidget22.Configuration
{
    public class CapacitiveTouch
    {
        public string Name { get; set; } = "CapacitiveTouch NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
