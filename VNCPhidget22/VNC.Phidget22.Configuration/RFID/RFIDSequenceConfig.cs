namespace VNC.Phidget22.Configuration
{
    public class RFIDSequenceConfig
    {
        public RFIDSequence[] RFIDSequences { get; set; } = new[]
        {
            new RFIDSequence
            {
                //SerialNumber = 124744,
                Name="localhost_RFID 1",

                Actions = new[]
                {
                    new RFIDAction { Duration=500 },
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