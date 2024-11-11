namespace VNC.Phidget22.Configuration
{
    public class DigitalOutputSequenceConfig
    {
        public DigitalOutputSequence[] DigitalOutputSequences { get; set; } = new[]
        {
            new DigitalOutputSequence
            {
                //SerialNumber = 124744,
                Name="localhost_SequenceIK 1",

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new DigitalOutputSequence
            {
                //SerialNumber = 46049,
                Name="psbc11_SequenceIK 1",

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new DigitalOutputSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceIK 1", SequenceType = "IK" },

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new DigitalOutputSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1 Parallel",
                ExecuteActionsInParallel = true,

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new DigitalOutputSequence
            {
                //SerialNumber = 251831,
                Name="psbc22_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceIK 1", SequenceType = "IK" },

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new DigitalOutputSequence
            {
                //SerialNumber = 48284,
                Name="psbc23_SequenceIK 1",

                Actions = new[]
                {
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new DigitalOutputAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
        };
    }
}