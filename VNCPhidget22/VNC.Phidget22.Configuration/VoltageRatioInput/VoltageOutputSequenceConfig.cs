namespace VNC.Phidget22.Configuration
{
    public class VoltageRatioInputSequenceConfig
    {
        public VoltageRatioInputSequence[] VoltageOutputSequences { get; set; } = new[]
        {
            new VoltageRatioInputSequence
            {
                Name="VRI 0",
                Channel = 0,

                //Actions = new[]
                //{
                //    new VoltageOutputAction { VoltageOut = true, Duration=500 },
                //    new VoltageOutputAction { VoltageOut = false, Duration=500 },
                //    new VoltageOutputAction { VoltageOut = true, Duration = 500 },
                //    new VoltageOutputAction { VoltageOut = false, Duration = 500 },
                //    new VoltageOutputAction { VoltageOut = true, Duration = 500 },
                //    new VoltageOutputAction { VoltageOut = false, Duration = 500 }
                //}
            },
        };
    }
}