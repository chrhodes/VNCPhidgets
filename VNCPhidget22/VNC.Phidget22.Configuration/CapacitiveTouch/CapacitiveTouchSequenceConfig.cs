namespace VNC.Phidget22.Configuration
{
    public class CapacitiveTouchtSequenceConfig
    {
        public CapacitiveTouchSequence[] CapacitiveTouchSequences { get; set; } = new[]
        {
            new CapacitiveTouchSequence
            {
                //SerialNumber = 124744,
                Name="localhost_CapacitiveTouch 1",

                Actions = new[]
                {
                    new CapacitiveTouchAction { Duration=500 },
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