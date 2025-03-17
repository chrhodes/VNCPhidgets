namespace VNC.Phidget22.Configuration
{
    public class HostConfig
    {
        public Host[] Hosts { get; set; } = new[]
        {
            new Host
            {
                Name = "localhost", IPAddress = "127.0.0.1", Port = 5661, Enable = true,
                //InterfaceKits = new[]
                //{
                //    new InterfaceKit { Name = "InterfaceKit 1", SerialNumber = 124744, Embedded = false, Open = true }
                //},
                //AdvancedServos = new[]
                //{
                //    new AdvancedServo { Name = "AdvancedServo 99415", SerialNumber = 99415, Open = true },
                //    new AdvancedServo { Name = "AdvancedServo 593480", SerialNumber = 593480, Open = true }
                //}
            },
            new Host
            {
                Name = "psbc11", IPAddress = "192.168.150.11", Port = 5661, Enable = true,
                //InterfaceKits = new[]
                //{
                //    new InterfaceKit { Name = "psbc11 InterfaceKit", SerialNumber = 46049, Embedded = true, Open = true }
                //},
                //AdvancedServos = new[]
                //{
                //    new AdvancedServo { Name = "AdvancedServo 99415", SerialNumber = 99415, Open = true },
                //    new AdvancedServo { Name = "AdvancedServo 593480", SerialNumber = 593480, Open = true }
                //}
            },
            new Host
            {
                Name = "psbc21", IPAddress = "192.168.150.21", Port = 5661, Enable = true,
                //InterfaceKits = new[]
                //{
                //    new InterfaceKit { Name = "psbc21 InterfaceKit", SerialNumber = 48301, Embedded = true, Open = true }
                //},
                //AdvancedServos = new[]
                //{
                //    new AdvancedServo { Name = "AdvancedServo 99415", SerialNumber = 99415, Open = true },
                //    new AdvancedServo { Name = "AdvancedServo 593480", SerialNumber = 593480, Open = true }
                //}
            },
            new Host
            {
                Name = "psbc22", IPAddress = "192.168.150.22", Port = 5661, Enable = true,
                //InterfaceKits = new[]
                //{
                //    new InterfaceKit { Name = "psbc22 InterfaceKit", SerialNumber = 251831, Embedded = true, Open = true },
                //},
                //AdvancedServos = new[]
                //{
                //    new AdvancedServo { Name = "AdvancedServo 99415", SerialNumber = 99415, Open = true },
                //    new AdvancedServo { Name = "AdvancedServo 593480", SerialNumber = 593480, Open = true }
                //}
            },
            new Host
            {
                Name = "psbc23", IPAddress = "192.168.150.23", Port = 5661, Enable = true,
                //AdvancedServos = new[]
                //{
                //    new AdvancedServo { Name = "AdvancedServo 99415", SerialNumber = 99415, Open = true },
                //    new AdvancedServo { Name = "AdvancedServo 593480", SerialNumber = 593480, Open = true }
                //},
                DigitalInputs = new[]
                {
                    new DigitalInput { Name = "DigitalInput 124744", SerialNumber = 124744, Open = true },
                },
                DigitalOutputs = new[]
                {
                    new DigitalOutput { Name = "DigitalOutput 124744", SerialNumber = 124744, Open = true },
                },
                //InterfaceKits = new[]
                //{
                //    new InterfaceKit { Name = "psbc23 InterfaceKit 48284", SerialNumber = 48284, Embedded = true, Open = true }
                //},
                RCServos = new[]
                {
                    new RCServo { Name = "RCServo 99415", SerialNumber = 99415, Open = true },
                },
                VoltageInputs = new[]
                {
                    new VoltageInput { Name = "VoltageInput 124744", SerialNumber = 124744, Open = true },
                },
                VoltageOutputs = new[]
                {
                    new VoltageOutput { Name = "VoltageOutput 124744", SerialNumber = 124744, Open = true },
                },
            }

        };


        // NOTE(crhodes)
        // We could put Sensors inside InterfaceKit but lets learn if can discover
        // This is where we can put characteristics of Sensors if needed.

        public Sensor[] Sensors { get; set; } = new[]
        {
            new Sensor {  Name="1101 - IR Distance Adapter 4-30cm" },
            new Sensor {  Name="1101 - IR Distance Adapter 10-80cm" },
            new Sensor {  Name="1101 - IR Distance Adapter 20-150cm" },
            new Sensor {  Name="1102 - IR Reflective Sensor 5mm" },
            new Sensor {  Name="1103 - IR Reflective Sensor 10cm" },
            new Sensor {  Name="1104 - Vibration Sensor" },
            new Sensor {  Name="1105 - Light Sensor" },
            new Sensor {  Name="1106 - Force Sensor" },
            new Sensor {  Name="1107 - Humidity Sensor" },
            new Sensor {  Name="1108 - Magnetic Sensor" },
            new Sensor {  Name="1109 - Rotation Sensor" },
            new Sensor {  Name="1110 - Touch Sensor" },
            new Sensor {  Name="1111 - Motion Sensor" },
            new Sensor {  Name="1112 - Slider Sensor" },
            new Sensor {  Name="1113 - Mini Joystick Sensor [X]" },
            new Sensor {  Name="1113 - Mini Joystick Sensor [Y]" },
            new Sensor {  Name="1115 - Pressure Sensor" },
            new Sensor {  Name="1116 - Multi-turn Rotation Sensor" },
            new Sensor {  Name="1117 - Voltage Sensor" },
            new Sensor {  Name="1118 - 50Amp Current Sensor [AC]" },
            new Sensor {  Name="1118 - 50Amp Current Sensor [DC]" },
            new Sensor {  Name="1119 - 20Amp Current Sensor [AC]" },
            new Sensor {  Name="1119 - 20Amp Current Sensor [DC]" },
            new Sensor {  Name="1120 - FlexiForce Adapter Board" },
            new Sensor {  Name="1121 - Voltage Divider" },
            new Sensor {  Name="1122 - 30Amp Current Sensor [AC]" },
            new Sensor {  Name="1122 - 30Amp Current Sensor [DC]" },
            new Sensor {  Name="1123 - Precision Voltage Sensor" },
            new Sensor {  Name="1124 - Precision Temperature Sensor" },
            new Sensor {  Name="1125 - Humidity/Temperature Sensor [H]" },
            new Sensor {  Name="1125 - Humidity/Temperature Sensor [T]" },
            new Sensor {  Name="1126 - Differential Gas Pressure Sensor" },
            new Sensor {  Name="1127 - Precision Light Sensor" },
            new Sensor {  Name="1128 - Sonar Sensor" },
            new Sensor {  Name="1129 - Touch Sensor" },
            new Sensor {  Name="1130 - pH Adapter Board [pH]" },
            new Sensor {  Name="1130 - pH Adapter Board [ORP]" },
            new Sensor {  Name="1131 - Thin Force Sensor" },
            new Sensor {  Name="1132 - 4-20 mA Adapter" },
            new Sensor {  Name="1133 - Sound Sensor" },
            new Sensor {  Name="1134 - Switchable Voltage Divider" },
            new Sensor {  Name="1135 - Precision Voltage Sensor" },
            new Sensor {  Name="1136 - Differential Gas Pressure Sensor ±2 kPa" },
            new Sensor {  Name="1137 - Differential Gas Pressure Sensor ±7 kPa" },
            new Sensor {  Name="1138 - Differential Gas Pressure Sensor 50 kPa" },
            new Sensor {  Name="1139 - Differential Gas Pressure Sensor 100 kPa" },
            new Sensor {  Name="1140 - Absolute Gas Pressure Sensor 20-400 kPa" },
            new Sensor {  Name="1141 - Absolute Gas Pressure Sensor 15-115 kPa" },
            new Sensor {  Name="1142 - Light Sensor 1000 lux" },
            new Sensor {  Name="1143 - Light Sensor 70000 lux" },
            new Sensor {  Name="3120-3123 - FC22 Compression Load Cell" },
            new Sensor {  Name="3500-3503 - i-Snail Current Sensor" }
        };
    }
}
