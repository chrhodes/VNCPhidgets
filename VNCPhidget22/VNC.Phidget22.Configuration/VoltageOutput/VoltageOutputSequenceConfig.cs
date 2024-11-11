namespace VNC.Phidget22.Configuration
{
    public class VoltageOutputSequenceConfig
    {
        public VoltageOutputSequence[] VoltageOutputSequences { get; set; } = new[]
        {
            new VoltageOutputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_SequenceIK 1",

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
            new VoltageOutputSequence
            {
                //SerialNumber = 46049,
                Name="psbc11_SequenceIK 1",

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
            new VoltageOutputSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceIK 1", SequenceType = "IK" },

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
            new VoltageOutputSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1 Parallel",
                ExecuteActionsInParallel = true,

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
            new VoltageOutputSequence
            {
                //SerialNumber = 251831,
                Name="psbc22_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceIK 1", SequenceType = "IK" },

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
            new VoltageOutputSequence
            {
                //SerialNumber = 48284,
                Name="psbc23_SequenceIK 1",

                Actions = new[]
                {
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = true, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 0, VoltageOut = false, Duration=500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 1, VoltageOut = false, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = true, Duration = 500 },
                    new VoltageOutputAction { VoltageOutIndex = 2, VoltageOut = false, Duration = 500 }
                }
            },
        };
    }
}