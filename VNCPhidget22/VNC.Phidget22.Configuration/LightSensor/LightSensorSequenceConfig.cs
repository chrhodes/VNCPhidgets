namespace VNC.Phidget22.Configuration
{
    public class LightSensortSequenceConfig
    {
        public LightSensorSequence[] LightSensorSequences { get; set; } = new[]
        {
            new LightSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_LightSensor 1",

                Actions = new[]
                {
                    new LightSensorAction { Duration=500 },
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