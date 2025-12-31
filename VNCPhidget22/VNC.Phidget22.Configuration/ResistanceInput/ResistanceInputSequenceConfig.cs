namespace VNC.Phidget22.Configuration
{
    public class ResistanceInputSequenceConfig
    {
        public ResistanceInputSequence[] ResistanceInputSequences { get; set; } = new[]
        {
            new ResistanceInputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_ResistanceInput 1",

                Actions = new[]
                {
                    new ResistanceInputAction { Duration=500 },
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