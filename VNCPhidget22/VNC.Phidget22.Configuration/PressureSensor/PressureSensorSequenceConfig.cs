namespace VNC.Phidget22.Configuration
{
    public class PressureSensorSequenceConfig
    {
        public PressureSensorSequence[] PressureSensorSequences { get; set; } = new[]
        {
            new PressureSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_PressureSensor 1",

                Actions = new[]
                {
                    new PressureSensorAction { Duration=500 },
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