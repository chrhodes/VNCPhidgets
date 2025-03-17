using System;

namespace VNC.Phidget22.Configuration
{
    public class Magnetometer
    {
        public string Name { get; set; } = "Magnetometer NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
