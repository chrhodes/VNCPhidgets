namespace VNC.Phidget22.Configuration
{
    public class MagnetometertSequenceConfig
    {
        public MagnetometerSequence[] MagnetometerSequences { get; set; } = new[]
        {
            new MagnetometerSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Magnetometer 1",

                Actions = new[]
                {
                    new MagnetometerAction { Duration=500 },
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