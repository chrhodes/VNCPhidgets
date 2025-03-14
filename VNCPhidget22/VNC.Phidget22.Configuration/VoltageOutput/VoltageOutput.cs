using System;

namespace VNC.Phidget22.Configuration
{
    public class VoltageOutput
    {
        public string Name { get; set; } = "VOLTAGEOUTPUT NAME";
        public Int32 SerialNumber { get; set; } = 12345;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
