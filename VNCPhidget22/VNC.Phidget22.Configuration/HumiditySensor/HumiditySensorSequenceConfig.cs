namespace VNC.Phidget22.Configuration
{
    public class HumiditySensortSequenceConfig
    {
        public HumiditySensorSequence[] HumiditySensorSequences { get; set; } = new[]
        {
            new HumiditySensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_HumiditySensor 1",

                Actions = new[]
                {
                    new HumiditySensorAction { Duration=500 },
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