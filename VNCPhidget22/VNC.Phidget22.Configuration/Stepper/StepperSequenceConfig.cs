namespace VNC.Phidget22.Configuration
{
    public class StepperSequenceConfig
    {
        public StepperSequence[] StepperSequences { get; set; } = new[]
        {
            new StepperSequence
            {
                Name = "StepperSequence 0",
                Channel = 0,
                Actions = new[]
                {
                    new StepperAction { Acceleration = 5000, VelocityLimit = 200, Engaged = true }
                }
            } 
        };
    }
}
