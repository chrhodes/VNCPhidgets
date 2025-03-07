namespace VNC.Phidget22.Configuration
{
    public class InterfaceKitSequenceConfig
    {
        public InterfaceKitSequence[] InterfaceKitSequences { get; set; } = new[]
        {
            new InterfaceKitSequence
            {
                //SerialNumber = 124744,
                Name="localhost_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                //SerialNumber = 46049,
                Name="psbc11_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceIK 1", DeviceClass = "IK" },

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                //SerialNumber = 48301,
                Name="psbc21_SequenceIK 1 Parallel",
                ExecuteActionsInParallel = true,

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                //SerialNumber = 251831,
                Name="psbc22_SequenceIK 1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceIK 1", DeviceClass = "IK" },

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
            new InterfaceKitSequence
            {
                //SerialNumber = 48284,
                Name="psbc23_SequenceIK 1",

                Actions = new[]
                {
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = true, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 0, DigitalOut = false, Duration=500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 1, DigitalOut = false, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = true, Duration = 500 },
                    new InterfaceKitAction { DigitalOutIndex = 2, DigitalOut = false, Duration = 500 }
                }
            },
        };
    }
}