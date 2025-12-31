namespace VNC.Phidget22.Configuration
{
    public class SpatialSequenceConfig
    {
        public SpatialSequence[] SpatialSequences { get; set; } = new[]
        {
            new SpatialSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Spatial 1",

                Actions = new[]
                {
                    new SpatialAction { Duration=500 },
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