namespace VNC.Phidget22.Configuration
{
    public class DigitalInputSequenceConfig
    {
        public DigitalInputSequence[] DigitalInputSequences { get; set; } = new[]
        {
            new DigitalInputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_DIS 1",

                Actions = new[]
                {
                    new DigitalInputAction { Duration=500 },
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