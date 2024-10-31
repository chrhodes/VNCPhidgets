using System;

namespace VNCPhidget21.Configuration
{
    public class InterfaceKit
    {
        public string Name { get; set; } = "INTERFACEKIT NAME";
        public Int32 SerialNumber { get; set; } = 12345;
        public bool Open { get; set; } = false;
        public bool Embedded { get; set; } = false;
    }
}
