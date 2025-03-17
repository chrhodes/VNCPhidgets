namespace VNC.Phidget22.Configuration
{
    public class FrequencyCountertSequenceConfig
    {
        public FrequencyCounterSequence[] FrequencyCounterSequences { get; set; } = new[]
        {
            new FrequencyCounterSequence
            {
                //SerialNumber = 124744,
                Name="localhost_FrequencyCounter 1",

                Actions = new[]
                {
                    new FrequencyCounterAction { Duration=500 },
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