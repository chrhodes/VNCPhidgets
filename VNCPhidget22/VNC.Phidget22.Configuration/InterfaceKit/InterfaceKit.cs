using System;

namespace VNC.Phidget22.Configuration
{
    public class InterfaceKit
    {
        public string Name { get; set; } = "INTERFACEKIT NAME";
        public Int32 SerialNumber { get; set; } = 12345;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
