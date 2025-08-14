using VNC.Phidget22.Configuration;

namespace VNC.Phidget22.Events
{
    public class SequenceEventArgs
    {

        public AccelerometerSequence? AccelerometerSequence { get; set; }
        public BLDCMotorSequence? BLDCMotorSequence { get; set; }
        public CapacitiveTouchSequence? CapacitiveTouchSequence { get; set; }
        public CurrentInputSequence? CurrentInputSequence { get; set; }
        public DCMotorSequence? DCMotorSequence { get; set; }
        public DigitalInputSequence? DigitalInputSequence { get; set; }
        public DigitalOutputSequence? DigitalOutputSequence { get; set; }
        public DistanceSensorSequence? DistanceSensorSequence { get; set; }
        public EncoderSequence? EncoderSequence { get; set; }
        public FrequencyCounterSequence? FrequencyCounterSequence { get; set; }
        public GPSSequence? GPSSequence { get; set; }
        public GyroscopeSequence? GyroscopeSequence { get; set; }
        public HubSequence? HubSequence { get; set; }
        public HumiditySensorSequence? HumiditySensorSequence { get; set; }
        public IRSequence? IRSequence { get; set; }
        public LCDSequence? LCDSequence { get; set; }
        public LightSensorSequence? LightSensorSequence { get; set; }
        public MagnetometerSequence? MagnetometerSequence { get; set; }
        public MotorPositionControllerSequence? MotorPositionControllerSequence { get; set; }
        public PHSensorSequence? PHSensorSequence { get; set; }
        public PowerGuardSequence? PowerGuardSequence { get; set; }
        public PressureSensorSequence? PressureSensorSequence { get; set; }
        public RCServoSequence? RCServoSequence { get; set; }
        public ResistanceInputSequence? ResistanceInputSequence { get; set; }
        public RFIDSequence? RFIDSequence { get; set; }
        public SoundSensorSequence? SoundSensorSequence { get; set; }
        public SpatialSequence? SpatialSequence { get; set; }
        public StepperSequence? StepperSequence { get; set; }
        public TemperatureSensorSequence? TemperatureSensorSequence { get; set; }
        public VoltageInputSequence? VoltageInputSequence { get; set; }
        public VoltageRatioInputSequence? VoltageRatioInputSequence { get; set; }
        public VoltageOutputSequence? VoltageOutputSequence { get; set; }
    }
}
