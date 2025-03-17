using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageInput
    {
        public string Name { get; set; } = "VOLTAGEINPUT NAME";
        public Int32 SerialNumber { get; set; } = 12345;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
