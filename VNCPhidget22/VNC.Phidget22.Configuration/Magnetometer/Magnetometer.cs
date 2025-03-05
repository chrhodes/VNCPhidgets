using System;

namespace VNC.Phidget22.Configuration
{
    public class Magnetometer
    {
        public string Name { get; set; } = "Magnetometer NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public bool Open { get; set; } = false;
        public bool Embedded { get; set; } = false;
    }
}
