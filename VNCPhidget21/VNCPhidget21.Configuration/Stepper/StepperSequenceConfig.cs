namespace VNCPhidget21.Configuration
{
    public class StepperSequenceConfig
    {
        public StepperSequence[] StepperSequences { get; set; } = new[]
        {
            new StepperSequence
            {
                //SerialNumber = 46049,
                Name="SequenceStepper 1",

                Actions = new[]
                {
                    new StepperAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true }
                }
            } 
        };
    }
}
