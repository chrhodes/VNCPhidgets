namespace VNC.Phidget22.Configuration
{
    public class GPSSequenceConfig
    {
        public GPSSequence[] GPSSequences { get; set; } = new[]
        {
            new GPSSequence
            {
                //SerialNumber = 124744,
                Name="localhost_GPS 1",

                Actions = new[]
                {
                    new GPSAction { Duration=500 },
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