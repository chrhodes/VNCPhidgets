namespace VNC.Phidget22.Configuration
{
    public class CurrentInputtSequenceConfig
    {
        public CurrentInputSequence[] CurrentInputSequences { get; set; } = new[]
        {
            new CurrentInputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_CurrentInput 1",

                Actions = new[]
                {
                    new CurrentInputAction { Duration=500 },
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