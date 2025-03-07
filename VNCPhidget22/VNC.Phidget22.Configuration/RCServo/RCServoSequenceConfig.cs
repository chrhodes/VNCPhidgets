namespace VNC.Phidget22.Configuration
{
    public class RCServoSequenceConfig
    {
        public RCServoSequence[] RCServoSequences { get; set; } = new[]
        {
            new RCServoSequence
            {
                Name="RCServoSequence0",
                Channel = 0,
                Actions = new[]
                {
                    new RCServoAction { Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new RCServoAction { TargetPosition = 90 },
                    new RCServoAction { TargetPosition = 100 },
                    new RCServoAction { TargetPosition = 110 },
                    new RCServoAction { TargetPosition = 100 },
                    new RCServoAction { TargetPosition = 90 },
                    new RCServoAction { Engaged = false },
                },
                NextSequence = new PerformanceSequence { Name = "SequenceServo1", DeviceClass = "RCS" }
            },
        };
    }
}

