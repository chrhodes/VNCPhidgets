namespace VNC.Phidget22.Configuration
{
    public class IRtSequenceConfig
    {
        public IRSequence[] IRSequences { get; set; } = new[]
        {
            new IRSequence
            {
                //SerialNumber = 124744,
                Name="localhost_IR 1",

                Actions = new[]
                {
                    new IRAction { Duration=500 },
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