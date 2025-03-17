namespace VNC.Phidget22.Configuration
{
    public class DistanceSensortSequenceConfig
    {
        public DistanceSensorSequence[] DistanceSensorSequences { get; set; } = new[]
        {
            new DistanceSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_DistanceSensor 1",

                Actions = new[]
                {
                    new DistanceSensorAction { Duration=500 },
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