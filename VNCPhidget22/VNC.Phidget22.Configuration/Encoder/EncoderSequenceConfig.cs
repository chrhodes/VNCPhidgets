namespace VNC.Phidget22.Configuration
{
    public class EncodertSequenceConfig
    {
        public EncoderSequence[] EncoderSequences { get; set; } = new[]
        {
            new EncoderSequence
            {
                //SerialNumber = 124744,
                Name="localhost_Encoder 1",

                Actions = new[]
                {
                    new EncoderAction { Duration=500 },
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