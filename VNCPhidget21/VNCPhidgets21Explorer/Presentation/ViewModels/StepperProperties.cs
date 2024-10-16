using System;
using System.Threading;

using Phidgets;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    #region Fields and Properties

    // TODO(crhodes)
    // Not sure we need INPCBase.  See if any bindings reach for this class

    public class StepperProperties : INPCBase
    {
        public StepperEx StepperEx 
        { 
            get; 
            set; 
        }

        public int StepperIndex { get; set; }

        //private Phidgets.stepperservo.ServoType? _servoType;

        //public Phidgets.stepperservo.ServoType? ServoType
        //{
        //    get => _servoType;
        //    set
        //    {
        //        if (_servoType == value)
        //        {
        //            // We may have closed and reopened the Stepper
        //            // without changing the type from the DEFAULT which is set by the PhidgetLibrary when device opened.
        //            // Refresh the UI properties as close sets all to null
        //            GetPropertiesFromServo();
        //            return; 
        //        }

        //        _servoType = value;
        //        OnPropertyChanged();

        //        if (StepperEx is not null)
        //        {
        //            if (StepperEx.Stepper is not null)
        //            {
        //                StepperEx.Stepper.steppers[StepperIndex].Type = (Phidgets.stepperservo.ServoType)value;

        //                // Need to update all the properties since the type changed.
        //                // NB. Even setting to the same Type causes a refresh of the properties

        //                // TODO(crhodes)
        //                // Maybe we should sleep for a bit to allow info to propagate
        //                // Play with this by having a value in UI

        //                Thread.Sleep(1);
        //                GetPropertiesFromServo();
        //            }
        //        }
        //    }
        //}

        public bool LogPhidgetEvents { get; set; }

        #region Configuration

        private double _minimumPulseWidth = 1000;

        public double MinimumPulseWidth
        {
            get => _minimumPulseWidth;
            set
            {
                if (_minimumPulseWidth == value) return;
                _minimumPulseWidth = value;
                OnPropertyChanged();
            }
        }

        private double _maximumPulseWidth = 1001;

        public double MaximumPulseWidth
        {
            get => _maximumPulseWidth;
            set
            {
                if (_maximumPulseWidth == value) return;
                _maximumPulseWidth = value;
                OnPropertyChanged();
            }
        }

        private double _degrees;

        public double Degrees
        {
            get => _degrees;
            set
            {
                if (_degrees == value) return;
                _degrees = value;
                OnPropertyChanged();
            }
        }

        #endregion Configuration Properties

        #region Position

        private Double? _devicePositionMin;

        /// <summary>
        /// Initial PositionMin after ServoType change
        /// </summary>
        public Double? DevicePositionMin
        {
            get => _devicePositionMin;
            set
            {
                if (_devicePositionMin == value) return;
                _devicePositionMin = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMin;

        public Double? PositionMin
        {
            get => _positionMin;
            set
            {
                if (_positionMin == value) return;
                _positionMin = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (StepperEx.Stepper.steppers[StepperIndex].PositionMin != value)
                        {
                            StepperEx.SetPositionMin(
                                (Double)value,
                                StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        StepperEx.Stepper.steppers[StepperIndex].Position = (double)value;
                    }
                }
            }
        }

        private Double? _position;
        public Double? Position
        {
            get => _position;
            set
            {
                if (_position == value) return;
                _position = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    // Do not check position if Servo not engaged.
                    // Exception will be thrown if servo.Position not set.   

                    if (StepperEx.Stepper.Attached
                        && StepperEx.Stepper.steppers[StepperIndex].Engaged)
                    {                        
                        try
                        {
                            // Set new position if different from current.  Why would we bother to change if same?
                            if (StepperEx.Stepper.steppers[StepperIndex].Position != value)
                            {                                
                                StepperEx.SetPosition(
                                    (Double)value,
                                    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            // Hopefully this is never thrown.
                            Log.Error(pex, Common.LOG_CATEGORY);
                            StepperEx.Stepper.steppers[StepperIndex].Position = (double)value;
                        }
                    }
                    else
                    {
                        // It is Ok to set position before engaging servo.
                        //StepperEx.SetPosition(
                        //    (Double)value,
                        //    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                    }                    
                }
            }
        }

        private int _positionRange = 10;

        public int PositionRange
        {
            get => _positionRange;
            set
            {
                if (_positionRange == value) return;
                _positionRange = value;
                OnPropertyChanged();
            }
        }

        private Double? _positionMax;

        public Double? PositionMax
        {
            get => _positionMax;
            set
            {
                if (_positionMax == value) return;
                _positionMax = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    try
                    {
                        if (StepperEx.Stepper.steppers[StepperIndex].PositionMax != value)
                        {
                            StepperEx.SetPositionMax(
                                (Double)value,
                                StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                        }
                    }
                    catch (PhidgetException pex)
                    {
                        Log.Error(pex, Common.LOG_CATEGORY);
                        StepperEx.Stepper.steppers[StepperIndex].Position = (double)value;
                    }
                }
            }
        }

        private Double? _devicePositionMax;

        /// <summary>
        /// Initial PositionMax after ServoType change
        /// </summary>
        public Double? DevicePositionMax
        {
            get => _devicePositionMax;
            set
            {
                if (_devicePositionMax == value) return;
                _devicePositionMax = value;
                OnPropertyChanged();
            }
        }

        #endregion Position

        #region Movement Control

        private bool? _speedRamping;

        public bool? SpeedRamping
        {
            get => _speedRamping;
            set
            {
                if (_speedRamping == value) return;
                _speedRamping = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityMin;

        public Double? VelocityMin
        {
            get => _velocityMin;
            set
            {
                if (_velocityMin == value) return;
                _velocityMin = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocity;

        public Double? Velocity
        {
            get => _velocity;
            set
            {
                if (_velocity == value) return;
                _velocity = value;
                OnPropertyChanged();
            }
        }

        private Double? _velocityLimit;

        public Double? VelocityLimit
        {
            get => _velocityLimit;
            set
            {
                if (_velocityLimit == value) return;
                _velocityLimit = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    StepperEx.SetVelocityLimit(
                        (Double)value,
                        StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                }
            }
        }

        private Double? _velocityMax;

        public Double? VelocityMax
        {
            get => _velocityMax;
            set
            {
                if (_velocityMax == value) return;
                _velocityMax = value;
                OnPropertyChanged();
            }
        }

        private Double? _accelerationMin;

        public Double? AccelerationMin
        {
            get => _accelerationMin;
            set
            {
                if (_accelerationMin == value) return;
                _accelerationMin = value;
                OnPropertyChanged();
            }
        }

        private Double? _acceleration;

        public Double? Acceleration
        {
            get => _acceleration;
            set
            {
                if (_acceleration == value) return;
                _acceleration = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    StepperEx.SetAcceleration(
                        (Double)value,
                        StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                }
            }
        }

        private Double? _accelerationMax;

        public Double? AccelerationMax
        {
            get => _accelerationMax;
            set
            {
                if (_accelerationMax == value) return;
                _accelerationMax = value;
                OnPropertyChanged();
            }
        }

        #endregion Movement Control

        #region stepperstate

        private bool? _engaged;

        public bool? Engaged
        {
            get => _engaged;
            set
            {
                if (_engaged == value) return;
                _engaged = value;
                OnPropertyChanged();

                if (value is not null) StepperEx.Stepper.steppers[StepperIndex].Engaged = (Boolean)value;
            }
        }

        private bool? _stopped;

        public bool? Stopped
        {
            get => _stopped;
            set
            {
                if (_stopped == value) return;
                _stopped = value;
                OnPropertyChanged();
            }
        }

        private Double? _current;

        public Double? Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                OnPropertyChanged();
            }
        }

        #endregion stepperstate Properties

        #endregion Fields and Properties (None)

        public enum MotionScale
        {
            Min = 1,
            Percent01 = 1,
            Percent02 = 2,
            Percent03 = 3,
            Percent04 = 4,
            Percent05 = 5,
            Percent10 = 10,
            Percent15 = 15,
            Percent20 = 20,
            Percent25 = 25,
            Percent35 = 35,
            Percent50 = 50,
            Percent75 = 75,
            Max
        }

        /// <summary>
        /// Centers servo based on Device{Min,Max} if position not set
        /// and Initializes Velocity
        /// </summary>
        /// <param name="motionScale"></param>
        public void InitializeVelocity(MotionScale motionScale)
        {
            StepperServo servo = null;

            try
            {
                servo = StepperEx.Stepper.steppers[StepperIndex];

                if (LogPhidgetEvents)
                {
                    Log.Trace($"Begin servo:{StepperIndex} speedRamping:{servo.SpeedRamping} acceleration:{(servo.Engaged ? servo.Acceleration : "???")} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex} acceleration:{(servo.Engaged ? servo.Acceleration : "??")}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex}  ", Common.LOG_CATEGORY);
                    Log.Trace($"Begin servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  position:{(servo.Engaged ? servo.Position : "???")}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

                switch (motionScale)
                {
                    case MotionScale.Min:
                        //Acceleration = AccelerationMin;
                        VelocityLimit = 1; // Zero will not move
                        break;

                    case MotionScale.Max:
                        //Acceleration = AccelerationMax;
                        VelocityLimit = VelocityMax;
                        break;

                    default:
                        //var accelerationRange = AccelerationMax - AccelerationMin;
                        var velocityRange = VelocityMax - VelocityMin;
                        double percentage = (Int32)motionScale / 100.0;

                        //Acceleration = AccelerationMin + (accelerationRange * percentage);
                        VelocityLimit = VelocityMin + (velocityRange * percentage);
                        break;
                }

                //if (servo.Position = )
                //{

                //}
                //Position = (DevicePositionMax - DevicePositionMin) / 2;

                if (LogPhidgetEvents)
                {
                    Log.Trace($"End servo:{StepperIndex} speedRamping:{servo.SpeedRamping} acceleration:{(servo.Engaged ? servo.Acceleration : "???")} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex} acceleration:{(servo.Engaged ? servo.Acceleration : "??")}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex}  ", Common.LOG_CATEGORY);
                    Log.Trace($"End servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  position:{(servo.Engaged ? servo.Position : "???")}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Initializes Acceleration
        /// </summary>
        /// <param name="motionScale"></param>
        public void InitializeAcceleration (MotionScale motionScale)
        {
            StepperServo servo = null;

            try
            {
                servo = StepperEx.Stepper.steppers[StepperIndex];

                if (LogPhidgetEvents)
                {
                    Log.Trace($"Begin servo:{StepperIndex} speedRamping:{servo.SpeedRamping} acceleration:{(servo.Engaged ? servo.Acceleration : "???")} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex} acceleration:{(servo.Engaged ? servo.Acceleration : "??")}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"Begin servo:{StepperIndex}  ", Common.LOG_CATEGORY);
                    Log.Trace($"Begin servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  position:{(servo.Engaged ? servo.Position : "???")}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

                switch (motionScale)
                {
                    case MotionScale.Min:
                        Acceleration = AccelerationMin;
                        //VelocityLimit = 1; // Zero will not move
                        break;

                    case MotionScale.Max:
                        Acceleration = AccelerationMax;
                        //VelocityLimit = VelocityMax;
                        break;

                    default:
                        var accelerationRange = AccelerationMax - AccelerationMin;
                        //var velocityRange = VelocityMax - VelocityMin;
                        double percentage = (Int32)motionScale / 100.0;

                        Acceleration = AccelerationMin + (accelerationRange * percentage);
                        //VelocityLimit = VelocityMin + (velocityRange * percentage);
                        break;
                }

                //if (servo.Position = )
                //{

                //}
                //Position = (DevicePositionMax - DevicePositionMin) / 2;

                if (LogPhidgetEvents)
                {
                    Log.Trace($"End servo:{StepperIndex} speedRamping:{servo.SpeedRamping} acceleration:{(servo.Engaged ? servo.Acceleration : "???")} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex} acceleration:{(servo.Engaged ? servo.Acceleration : "??")}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
                    //Log.Trace($"End servo:{StepperIndex}  ", Common.LOG_CATEGORY);
                    Log.Trace($"End servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  position:{(servo.Engaged ? servo.Position : "???")}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Gets current values from Servo.  Use when ServoType has not changed
        /// to get current values.  Does not set DevicePosition{Min.Max}
        /// Servo must be Engaged.
        /// </summary>
        public void RefreshPropertiesFromServo()
        {
            // TODO(crhodes)
            // Maybe we need a version for when Servo is Engaged and when it is not Engaged.
            // Get the servo outside of the try so we look at it in Exception(s)

            var servo = StepperEx.Stepper.steppers[StepperIndex];

            try
            {
                ServoType = servo.Type;

                Engaged = servo.Engaged;
                Stopped = servo.Stopped;
                Current = servo.Current;

                SpeedRamping = servo.SpeedRamping;

                AccelerationMin = servo.AccelerationMin;
                // NOTE(crhodes)
                // This is interesting.  Servo is not Engaged but servo.Acceleration is set

                // NOTE(crhodes)
                // If RefreshPropertiesFromServo immediately after Opening but before Engaged
                // servo.Acceleration is not set and will throw exception if accessed

                try
                {
                    Acceleration = servo.Acceleration;
                }
                catch (PhidgetException pex)
                {
                    Acceleration = null;

                //    var t = pex.Type;           // PHIDGET_ERR_UNKNOWLVAL
                //    var d = pex.Description;    // Value is Unknown (State not yet received from device, or not yet set by user).
                //    var c = pex.Code;           // 9
                //    var m = pex.Message;        // PhidgetException 9 (Value is Unknown (State not yet received from device, or not yet set by user).)
                //    var s = pex.Source;         // Phidget21.NET
                //    var ts = pex.TargetSite;    // {Double get_Acceleration()}
                //    // Quietly Ignore
                //    // Value is Unknown(State not yet received from device, or not yet set by user).
                //    //Log.Error(pex, Common.LOG_CATEGORY);
                }

                AccelerationMax = servo.AccelerationMax;

                VelocityMin = servo.VelocityMin;
                Velocity = servo.Velocity;

                // NOTE(crhodes)
                // If RefreshPropertiesFromServo immediately after Opening but before Engaged
                // servo.Acceleration is not set and will throw exception if accessedk

                try
                {
                    VelocityLimit = servo.VelocityLimit;
                }
                catch (PhidgetException pex)
                {
                    VelocityLimit = null;

                    //    var t = pex.Type;           // PHIDGET_ERR_UNKNOWLVAL
                    //    var d = pex.Description;    // Value is Unknown (State not yet received from device, or not yet set by user).
                    //    var c = pex.Code;           // 9
                    //    var m = pex.Message;        // PhidgetException 9 (Value is Unknown (State not yet received from device, or not yet set by user).)
                    //    var s = pex.Source;         // Phidget21.NET
                    //    var ts = pex.TargetSite;    // {Double get_Acceleration()}
                    //    // Quietly Ignore
                    //    // Value is Unknown(State not yet received from device, or not yet set by user).
                    //    //Log.Error(pex, Common.LOG_CATEGORY);
                }

                VelocityMax = servo.VelocityMax;

                PositionMin = servo.PositionMin;

                // Position is not known if servo is not engaged

                if (Engaged is true)
                {
                    Position = servo.Position;
                }
                else
                {
                    Position = null;
                }

                PositionMax = servo.PositionMax;

                if (LogPhidgetEvents)
                {
                    // NOTE(crhodes)
                    // We use the property for Acceleration, VelocityLimit, and Position to avoid exceptions if servo not engaged

                    Log.Trace($"servo:{StepperIndex} engaged:{servo.Engaged} stopped:{servo.Stopped} current:{servo.Current} speedRamping:{servo.SpeedRamping}" +
                        $" accelerationMin:{servo.AccelerationMin} acceleration:{Acceleration} accelerationMax:{servo.AccelerationMax}" +
                        $" velocityMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{VelocityLimit} velocityMax:{servo.VelocityMax}" +
                        $" positionMin:{servo.PositionMin} position:{Position} positionMax:{servo.PositionMax}" +
                        $" devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} engaged:{servo.Engaged} stopped:{servo.Stopped} current:{servo.Current} speedRamping:{servo.SpeedRamping}" +
                    //    $" accelerationMin:{servo.AccelerationMin} acceleration:{servo.Acceleration: " ??? ")} accelerationMax:{servo.AccelerationMax}" +
                    //    $" velocityMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{(servo.Engaged ? servo.VelocityLimit : "???")} velocityMax:{servo.VelocityMax}" +
                    //    $" positionMin:{servo.PositionMin} position:{(servo.Engaged ? servo.Position : "???")} positionMax:{servo.PositionMax}" +
                    //    $" devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} accelerationMin:{servo.AccelerationMin} acceleration:{(servo.Engaged ? servo.Acceleration : "???")} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} velocityMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{(servo.Engaged ? servo.VelocityLimit : "???")} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} positionMin:{servo.PositionMin} position:{(servo.Engaged ? servo.Position : "???")} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Initializes properties to null.  
        /// Use before setting ServoType
        /// to update any UI bindings
        /// </summary>
        public void InitializePropertiesToNull()
        {
            Int64 startTicks = 0;

            if (LogPhidgetEvents)
            {
                startTicks = Log.Trace($"Enter StepperIndex:{StepperIndex}", Common.LOG_CATEGORY);
            }

            try
            {
                //var servo = StepperEx.Stepper.steppers[StepperIndex];
                //ServoType = Phidgets.stepperservo.ServoType.DE;

                Stopped = null;
                Engaged = null;
                SpeedRamping = null;
                Current = null;

                // NOTE(crhodes)
                // Have to clear Acceleration before Min/Max as UI triggers an update

                Acceleration = null;
                AccelerationMin = null;
                AccelerationMax = null;

                // NOTE(crhodes)
                // Handle VelocityLimit same way as Acceleration
                // Have not confirmed this is an issue

                VelocityLimit = null;
                VelocityMin = null;
                Velocity = null;
                VelocityMax = null;

                DevicePositionMin = null;
                PositionMin = null;
                Position = null;
                PositionMax = null;
                DevicePositionMax = null;
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents)
            {
                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
        }

        /// <summary>
        /// Gets properties from Servo.  Use when ServoType changes (happens on open, too)
        /// Sets DevicePosition{Min,Max}
        /// </summary>
        private void GetPropertiesFromServo()
        {
            // TODO(crhodes)
            // Maybe we need a version for when Servo is Engaged and when it is not Engaged.
            // Get the servo outside of the try so we look at it in Exception(s)

            StepperServo servo = StepperEx.Stepper.steppers[StepperIndex];

            try
            {              
                if (LogPhidgetEvents)
                {
                    Log.Trace($"servo:{StepperIndex} engaged:{servo.Engaged}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} engaged:{servo.Engaged} stopped:{servo.Stopped} current:{servo.Current} speedRamping:{servo.SpeedRamping}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} accelerationMin:{servo.AccelerationMin} acceleration:{servo.Acceleration} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} velocityMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{servo.VelocityLimit} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} positionMin:{servo.PositionMin} position:{(servo.Engaged ? servo.Position : "??")} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

                // TODO(crhodes)
                // Clean up all these comments

                // These may not be set depending on state of servo
                // Phidget Library throws exceptions which are caught and ignored.
                // Setting to null keeps UI in sensible state

                //Double? safeServoAcceleration = null;
                //Double? safeServoPosition = null;

                //try
                //{
                //    initialServoAcceleration = servo.Acceleration;
                //}
                //catch (PhidgetException pex)
                //{
                //    var t = pex.Type;           // PHIDGET_ERR_UNKNOWLVAL
                //    var d = pex.Description;    // Value is Unknown (State not yet received from device, or not yet set by user).
                //    var c = pex.Code;           // 9
                //    var m = pex.Message;        // PhidgetException 9 (Value is Unknown (State not yet received from device, or not yet set by user).)
                //    var s = pex.Source;         // Phidget21.NET
                //    var ts = pex.TargetSite;    // {Double get_Acceleration()}
                //    // Quietly Ignore
                //    // Value is Unknown(State not yet received from device, or not yet set by user).
                //    //Log.Error(pex, Common.LOG_CATEGORY);
                //}

                //try
                //{
                //    initialServoPosition = servo.Position;
                //}
                //catch (PhidgetException pex)
                //{
                //    var t = pex.Type;           // PHIDGET_ERR_UNKNOWLVAL
                //    var d = pex.Description;    // Value is Unknown (State not yet received from device, or not yet set by user).
                //    var c = pex.Code;           // 9
                //    var m = pex.Message;        // PhidgetException 9 (Value is Unknown (State not yet received from device, or not yet set by user).)
                //    var s = pex.Source;         // Phidget21.NET
                //    var ts = pex.TargetSite;    // {Double get_Position()}
                //    //Quietly Ignore
                //    //PhidgetException 9 (Value is Unknown(State not yet received from device, or not yet set by user).)
                //    //Log.Error(pex, Common.LOG_CATEGORY);
                //}

                // Having said that, this code is only called when the ServoType changes
                // Seems like we should set these to some sensible value and avoid exceptions
                // Have to be careful to not step on things if servo is engaged.

                Double? initialServoAcceleration = servo.AccelerationMin;
                Double? initialServoPosition = (servo.PositionMax - servo.PositionMin) / 2; // Midpoint seems reasonable

                Engaged = servo.Engaged;
                Stopped = servo.Stopped;
                Current = servo.Current;

                SpeedRamping = servo.SpeedRamping;
                AccelerationMin = servo.AccelerationMin;

                if (Engaged is true)
                {
                    // NOTE(crhodes)
                    // servo.Acceleration is not set when first Opening Stepper
                    // and will throw exception if accessed

                    try
                    {
                        Acceleration = servo.Acceleration;
                    }
                    catch (PhidgetException pex)
                    {
                        Acceleration = initialServoAcceleration;

                        //    var t = pex.Type;           // PHIDGET_ERR_UNKNOWLVAL
                        //    var d = pex.Description;    // Value is Unknown (State not yet received from device, or not yet set by user).
                        //    var c = pex.Code;           // 9
                        //    var m = pex.Message;        // PhidgetException 9 (Value is Unknown (State not yet received from device, or not yet set by user).)
                        //    var s = pex.Source;         // Phidget21.NET
                        //    var ts = pex.TargetSite;    // {Double get_Acceleration()}
                        //    // Quietly Ignore
                        //    // Value is Unknown(State not yet received from device, or not yet set by user).
                        //    //Log.Error(pex, Common.LOG_CATEGORY);
                    }
                }
                else
                {
                    Acceleration = initialServoAcceleration;
                }
                
                AccelerationMax = servo.AccelerationMax;

                VelocityMin = servo.VelocityMin;
                Velocity = servo.Velocity;

                // NOTE(crhodes)
                // 
                // Servo Type has changed.  Let's set VelocityLimit to a small value
                // Make it possible to move servo without using UI to set non-zero velocity
                //VelocityLimit = servo.VelocityLimit == 0 ? 1 : servo.VelocityLimit;
                //VelocityLimit = servo.VelocityLimit;
                // Maybe this should onlyh happen if not Engaged

                VelocityLimit = servo.VelocityLimit;
                //VelocityLimit = VelocityMin + 1;
                VelocityMax = servo.VelocityMax;

                // DevicePosition{Min,Max} should only be set when ServoType changes

                DevicePositionMin = servo.PositionMin;
                PositionMin = servo.PositionMin;

                if (Engaged is true)
                {
                    Position = servo.Position;
                }
                else
                {
                    Position = initialServoPosition;
                }

                PositionMax = servo.PositionMax;              
                DevicePositionMax = servo.PositionMax;

                // NOTE(crhodes)
                // This is useful but where to put?
                //Double? halfRange;
                //Double? percent = 0.20;
                //Double? midPoint;

                //midPoint = (DevicePositionMax - DevicePositionMin) / 2;
                //halfRange = midPoint * percent;
                //PositionMin = midPoint - halfRange;
                //PositionMax = midPoint + halfRange;
                //Position = midPoint;

                // TODO(crhodes)
                // Not sure we need to log this if UI is ok now

                if (LogPhidgetEvents)
                {
                    Log.Trace($"servo:{StepperIndex} stopped:{servo.Stopped} speedRamping:{servo.SpeedRamping}" +
                        $" aMin:{servo.AccelerationMin} acceleration:{Acceleration} aMax:{servo.AccelerationMax}" +
                        $" vMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{servo.VelocityLimit} vMax:{servo.VelocityMax}" +
                        $" posMin:{servo.PositionMin} position:{initialServoPosition} positionMax:{servo.PositionMax}" +
                        $" devPosMin:{DevicePositionMin}  devPosMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} engaged:{servo.Engaged} stopped:{servo.Stopped} current:{servo.Current} speedRamping:{servo.SpeedRamping}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} accelerationMin:{servo.AccelerationMin} acceleration:{servo.Acceleration} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} velocityMin:{servo.VelocityMin} velocity:{servo.Velocity} velocityLimit:{servo.VelocityLimit} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} positionMin:{servo.PositionMin} position:{(servo.Engaged ? servo.Position : "??")} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"servo:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

            }
            catch (PhidgetException pex)
            {
                Log.Error($"servo:{StepperIndex}-{pex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }
    }
}