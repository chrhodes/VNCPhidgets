namespace VNC.Phidget22.Configuration
{
    public class AccelerometertSequenceConfig
    {
        public AccelerometerSequence[] AccelerometerSequences { get; set; } = new[]
        {
            new AccelerometerSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Accelerometer 1",

                Actions = new[]
                {
                    new AccelerometerAction { Duration=500 },
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