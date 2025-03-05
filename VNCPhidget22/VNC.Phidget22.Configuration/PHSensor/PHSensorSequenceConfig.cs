namespace VNC.Phidget22.Configuration
{
    public class PHSensortSequenceConfig
    {
        public PHSensorSequence[] PHSensorSequences { get; set; } = new[]
        {
            new PHSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_PHSensor 1",

                Actions = new[]
                {
                    new PHSensorAction { Duration=500 },
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