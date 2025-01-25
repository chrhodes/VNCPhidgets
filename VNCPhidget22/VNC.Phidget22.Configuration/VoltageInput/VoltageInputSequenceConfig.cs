namespace VNC.Phidget22.Configuration
{
    public class VoltageInputSequenceConfig
    {
        public VoltageInputSequence[] VoltageInputSequences { get; set; } = new[]
        {
            new VoltageInputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_VIS 1",

                Actions = new[]
                {
                    new VoltageInputAction { Duration=500 },
                    //new VoltageInputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    //new VoltageInputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    //new VoltageInputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    //new VoltageInputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    //new VoltageInputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
 
        };
    }
}