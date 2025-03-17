namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequenceConfig
    {
        public VoltageOutputSequence[] VoltageOutputSequences { get; set; } = new[]
        {
            new VoltageOutputSequence
            {
                Name = "VoltgeOutputSequence 0",
                Channel = 0,
                Actions = new[]
                {
                    new VoltageOutputAction { Duration=500 },
                }
            },
        };
    }
}