namespace VNC.Phidget22.Configuration
{
    public class HubSequenceConfig
    {
        public HubSequence[] HubSequences { get; set; } = new[]
        {
            new HubSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Hub 1",

                Actions = new[]
                {
                    new HubAction { Duration=500 },
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