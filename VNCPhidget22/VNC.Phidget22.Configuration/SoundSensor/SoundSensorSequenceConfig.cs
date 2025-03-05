namespace VNC.Phidget22.Configuration
{
    public class SoundSensortSequenceConfig
    {
        public SoundSensorSequence[] SoundSensorSequences { get; set; } = new[]
        {
            new SoundSensorSequence
            {
                //SerialNumber = 124744,
                Name="localhost_SoundSensor 1",

                Actions = new[]
                {
                    new SoundSensorAction { Duration=500 },
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