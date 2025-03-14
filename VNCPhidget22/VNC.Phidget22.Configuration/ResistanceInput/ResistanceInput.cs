using System;

namespace VNC.Phidget22.Configuration
{
    public class ResistanceInput
    {
        public string Name { get; set; } = "ResistanceInput NAME";
        public Int32 SerialNumber { get; set; } = 0;
        public Boolean Open { get; set; } = false;
        public Boolean Embedded { get; set; } = false;
    }
}
