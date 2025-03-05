namespace VNC.Phidget22.Configuration
{
    public class GyroscopetSequenceConfig
    {
        public GyroscopeSequence[] GyroscopeSequences { get; set; } = new[]
        {
            new GyroscopeSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Gyroscope 1",

                Actions = new[]
                {
                    new GyroscopeAction { Duration=500 },
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