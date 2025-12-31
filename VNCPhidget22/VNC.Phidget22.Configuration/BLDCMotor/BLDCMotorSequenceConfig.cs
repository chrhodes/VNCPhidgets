namespace VNC.Phidget22.Configuration
{
    public class BLDCMotorSequenceConfig
    {
        public BLDCMotorSequence[] BLDCMotorSequences { get; set; } = new[]
        {
            new BLDCMotorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_BLDCMotor 1",

                Actions = new[]
                {
                    new BLDCMotorAction { Duration=500 },
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