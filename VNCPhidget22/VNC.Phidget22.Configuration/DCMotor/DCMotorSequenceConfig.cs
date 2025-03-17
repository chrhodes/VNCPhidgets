namespace VNC.Phidget22.Configuration
{
    public class DCMotortSequenceConfig
    {
        public DCMotorSequence[] DCMotorSequences { get; set; } = new[]
        {
            new DCMotorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_DCMotor 1",

                Actions = new[]
                {
                    new DCMotorAction { Duration=500 },
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