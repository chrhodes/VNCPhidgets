namespace VNC.Phidget22.Configuration
{
    public class LCDSequenceConfig
    {
        public LCDSequence[] LCDSequences { get; set; } = new[]
        {
            new LCDSequence
            {
                //SerialNumber = 124744,
                Name="localhost_LCD 1",

                Actions = new[]
                {
                    new LCDAction { Duration=500 },
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