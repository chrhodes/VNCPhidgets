namespace VNC.Phidget22.Configuration
{
    public class RCServoSequenceConfig
    {
        public RCServoSequence[] RCServoSequences { get; set; } = new[]
        {
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="localhost_SequenceServo0",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo1", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="localhost_SequenceServo1",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServo2", SequenceType = "AS" },
            },
            new RCServoSequence
            {
                Name="localhost_SequenceServo2",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "localhost_SequenceServoFin", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="localhost_SequenceServoFin",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc11_SequenceServo0",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo1", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc11_SequenceServo1",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServo2", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc11_SequenceServo2",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc11_SequenceServoFin", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc11_SequenceServoFin",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo1",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo2",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0P Configure and Engage",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1P", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo1P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2P", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo2P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo3P", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo3P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo4P", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo4P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo5P", SequenceType = "AS" },
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo5P",
                ExecuteActionsInParallel = true,
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServoFin", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServoFin",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc21_SequenceServo0",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo1", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc21_SequenceServo1",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc21_SequenceServo2", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc22_SequenceServo2",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc22_SequenceServoFin", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc22_SequenceServoFin",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new RCServoSequence
            {
                //SerialNumber = 99415,
                Name="psbc23_SequenceServo0",
                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo1", SequenceType = "AS" }
            },
            new RCServoSequence
            {
                Name="psbc23_SequenceServo1",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServo2", SequenceType = "AS" },

                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 1, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 1, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                }
            },
            new RCServoSequence
            {
                Name="psbc23_SequenceServo2",
                NextSequence = new PerformanceSequence { Name = "psbc23_SequenceServoFin", SequenceType = "AS" },

                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 2, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 110 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 100 },
                    new RCServoAction { ServoIndex = 2, TargetPosition = 90 },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            },
            new RCServoSequence
            {
                Name="psbc23_SequenceServoFin",

                Actions = new[]
                {
                    new RCServoAction { ServoIndex = 0, Engaged = false },
                    new RCServoAction { ServoIndex = 1, Engaged = false },
                    new RCServoAction { ServoIndex = 2, Engaged = false },
                }
            }
        };
    }
}

