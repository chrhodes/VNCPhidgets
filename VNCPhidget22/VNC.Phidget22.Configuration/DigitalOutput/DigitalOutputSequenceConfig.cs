namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequenceConfig
    {
        //public DigitalOutputSequence[] DigitalOutputSequences { get; set; }

        public DigitalOutputSequence[] DigitalOutputSequences { get; set; } = new[]
        {
            new DigitalOutputSequence
            {
                Name="DigitalOutputSequence 0",
                Channel = 0,

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOut = false, Duration = 500 }
                }
            },
        };
    }
}