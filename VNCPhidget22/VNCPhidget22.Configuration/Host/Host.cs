//using Phidget22;

namespace VNCPhidget22.Configuration
{
    public class Host
    {
        public string Name { get; set; } = "HOST NAME";
        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5661;
        public bool Enable { get; set; } = true;

        public AdvancedServo[]? AdvancedServos { get; set; }

        public InterfaceKit[]? InterfaceKits { get; set; }

        public Stepper[]? Steppers { get; set; }
    }
}

