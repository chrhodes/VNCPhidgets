using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageInput
    {
        public string Name { get; set; } = "VOLTAGEINPUT NAME";
        public Int32 SerialNumber { get; set; } = 12345;
        public bool Open { get; set; } = false;
        public bool Embedded { get; set; } = false;
    }
}
