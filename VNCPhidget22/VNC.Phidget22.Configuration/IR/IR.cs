using System;

namespace VNC.Phidget22.Configuration
{
    public class IR
    {
        public string Name { get; set; } = "IR NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public bool Open { get; set; } = false;
        public bool Embedded { get; set; } = false;
    }
}
