namespace VNC.Phidget22.Configuration
{
    public class PowerGuardSequenceConfig
    {
        public PowerGuardSequence[] PowerGuardSequences { get; set; } = new[]
        {
            new PowerGuardSequence
            {
                //SerialNumber = 124744,
                Name="localhost_PowerGuard 1",

                Actions = new[]
                {
                    new PowerGuardAction { Duration=500 },
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