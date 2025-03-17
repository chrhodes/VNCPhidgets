namespace VNC.Phidget22.Configuration
{
    public class MotorPositionControllertSequenceConfig
    {
        public MotorPositionControllerSequence[] MotorPositionControllerSequences { get; set; } = new[]
        {
            new MotorPositionControllerSequence
            {
                //SerialNumber = 124744,
                Name="localhost_MotorPositionController 1",

                Actions = new[]
                {
                    new MotorPositionControllerAction { Duration=500 },
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