namespace VNC.Phidget22.Configuration
{
    public class TemperatureSensorSequenceConfig
    {
        public TemperatureSensorSequence[] TemperatureSensorSequences { get; set; } = new[]
        {
            new TemperatureSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_TemperatureSensor 1",

                Actions = new[]
                {
                    new TemperatureSensorAction { Duration=500 },
                    //new DigitalInputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    //new DigitalInputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    //new DigitalInputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    //new DigitalInputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    //new DigitalInputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
        };
    }
}