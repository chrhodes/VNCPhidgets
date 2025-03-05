using System;

namespace VNC.Phidget22.Configuration
{
    public class Spatial
    {
        public string Name { get; set; } = "Spatial NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public bool Open { get; set; } = false;
        public bool Embedded { get; set; } = false;
    }
}
