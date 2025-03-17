//using Phidget22;

using System;

namespace VNC.Phidget22.Configuration
{
    /// <summary>
    /// An IP addressable device
    /// (typically a SBC)
    /// that provides one or more types of Phidgets
    /// </summary>
    public class Host
    {
        public string Name { get; set; } = "HOST NAME";
        public string IPAddress { get; set; } = "127.0.0.1";
        public Int32 Port { get; set; } = 5661;
        public Boolean Enable { get; set; } = true;

        // FIX(crhodes)
        // Don't think we needs this anymore now that Manager_Attach is all tuned up

        public AdvancedServo[]? AdvancedServos { get; set; }

        public DigitalInput[]? DigitalInputs { get; set; }

        public DigitalOutput[]? DigitalOutputs { get; set; }

        public InterfaceKit[]? InterfaceKits { get; set; }

        public RCServo[]? RCServos { get; set; }

        public Stepper[]? Steppers { get; set; }

        public VoltageInput[]? VoltageInputs { get; set; }

        public VoltageOutput[]? VoltageOutputs { get; set; }
    }
}

