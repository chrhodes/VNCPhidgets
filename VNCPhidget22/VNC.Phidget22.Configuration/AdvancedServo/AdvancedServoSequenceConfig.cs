namespace VNC.Phidget22.Configuration
{
    public class AdvancedServoSequenceConfig
    {
        public AdvancedServoSequence[] AdvancedServoSequences { get; set; } = new[]
        {
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="localhost_SequenceServo0",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo1", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServo1",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo2", ChannelClass = "AS" },
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServo2",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServoFin", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="localhost_SequenceServoFin",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc11_SequenceServo0",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo1", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServo1",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo2", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServo2",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServoFin", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc11_SequenceServoFin",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo2",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0P Configure and Engage",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1P", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2P", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo2P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo3P", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo3P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo4P", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo4P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo5P", ChannelClass = "AS" },
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo5P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServoFin",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc21_SequenceServo1",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc22_SequenceServo2",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceServoFin", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc22_SequenceServoFin",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc23_SequenceServo0",
                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo1", ChannelClass = "AS" }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo2", ChannelClass = "AS" },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServoFin", ChannelClass = "AS" },

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new AdvancedServoServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new AdvancedServoSequence
            {
                Name="psbc23_SequenceServoFin",

                Actions = new[]
                {
                    new AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 1, Engaged = false },
                    new AdvancedServoServoAction { ServoIndex = 2, Engaged = false },
                }
            }
        };
    }
}

