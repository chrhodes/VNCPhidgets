using System;
using System.Threading;

using Phidget22;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    // TODO(crhodes)
    // Not sure we need INPCBase.  See if any bindings reach for this class

    public class StepperProperties : INPCBase
    {
        #region Enums and Structures

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

        #endregion
         
        #region Fields and Properties

        public StepperEx StepperEx 
        { 
            get; 
            set; 
        }

        public int StepperIndex {
            get; 
            set; 
        }

        private Double? _stepAngle;

        public Double? StepAngle
        {
            get => _stepAngle;
            set
            {
                if (_stepAngle == value) return;
                _stepAngle = value;
                OnPropertyChanged();
            }
        }

        //private Phidget22.stepperstepper.stepperType? _stepperType;

        //public Phidget22.stepperstepper.stepperType? stepperType
        //{
        //    get => _stepperType;
        //    set
        //    {
        //        if (_stepperType == value)
        //        {
        //            // We may have closed and reopened the Stepper
        //            // without changing the type from the DEFAULT which is set by the PhidgetLibrary when device opened.
        //            // Refresh the UI properties as close sets all to null
        //            GetPropertiesFromstepper();
        //            return; 
        //        }

        //        _stepperType = value;
        //        OnPropertyChanged();

        //        if (StepperEx is not null)
        //        {
        //            if (StepperEx.Stepper is not null)
        //            {
        //                StepperEx.Stepper.steppers[StepperIndex].Type = (Phidget22.stepperstepper.stepperType)value;

        //                // Need to update all the properties since the type changed.
        //                // NB. Even setting to the same Type causes a refresh of the properties

        //                // TODO(crhodes)
        //                // Maybe we should sleep for a bit to allow info to propagate
        //                // Play with this by having a value in UI

        //                Thread.Sleep(1);
        //                GetPropertiesFromstepper();
        //            }
        //        }
        //    }
        //}

        public bool LogPhidgetEvents { get; set; }
        public bool LogSequenceAction { get; set; }

        #region StepperState

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


        // TODO(crhodes)
        // Don't think a Stepper has these
        //#region Configuration

        //private double _minimumPulseWidth = 1000;

        //public double MinimumPulseWidth
        //{
        //    get => _minimumPulseWidth;
        //    set
        //    {
        //        if (_minimumPulseWidth == value) return;
        //        _minimumPulseWidth = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private double _maximumPulseWidth = 1001;

        //public double MaximumPulseWidth
        //{
        //    get => _maximumPulseWidth;
        //    set
        //    {
        //        if (_maximumPulseWidth == value) return;
        //        _maximumPulseWidth = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private double _degrees;

        //public double Degrees
        //{
        //    get => _degrees;
        //    set
        //    {
        //        if (_degrees == value) return;
        //        _degrees = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion Configuration Properties



        #region Position

        // NOTE(crhodes)
        // Since Position{Min,Max} are ReadOnly, don't see need for these

        //private Int64? _devicePositionMin;

        ///// <summary>
        ///// Initial PositionMin after stepperType change
        ///// </summary>
        //public Int64? DevicePositionMin
        //{
        //    get => _devicePositionMin;
        //    set
        //    {
        //        if (_devicePositionMin == value) return;
        //        _devicePositionMin = value;
        //        OnPropertyChanged();
        //    }
        //}

        private Int64? _positionMin;

        public Int64? PositionMin
        {
            get => _positionMin;
            set
            {
                if (_positionMin == value) return;
                _positionMin = value;
                OnPropertyChanged();

                //    if (value is not null)
                //    {
                //        try
                //        {
                //            if (StepperEx.Stepper.steppers[StepperIndex].PositionMin != value)
                //            {
                //                StepperEx.SetPositionMin(
                //                    (Double)value,
                //                    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                //            }
                //        }
                //        catch (PhidgetException pex)
                //        {
                //            Log.Error(pex, Common.LOG_CATEGORY);
                //            StepperEx.Stepper.steppers[StepperIndex].Position = (double)value;
                //        }
                //    }
            }
        }

        private Int64? _currentPosition;
        public Int64? CurrentPosition
        {
            get => _currentPosition;
            set
            {
                if (_currentPosition == value) return;
                _currentPosition = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    // Do not check position if stepper not engaged.
                    // Exception will be thrown if stepper.Position not set.   

                    if (StepperEx.Stepper.Attached
                        && StepperEx.Stepper.steppers[StepperIndex].Engaged)
                    {                        
                        try
                        {
                            // Set new position if different from current.  Why would we bother to change if same?
                            if (StepperEx.Stepper.steppers[StepperIndex].CurrentPosition != value)
                            {                                
                                StepperEx.SetCurrentPosition(
                                    (Int64)value,
                                    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            // Hopefully this is never thrown.
                            Log.Error(pex, Common.LOG_CATEGORY);
                            StepperEx.Stepper.steppers[StepperIndex].CurrentPosition = (Int64)value;
                        }
                    }
                    else
                    {
                        // It is Ok to set position before engaging stepper.
                        //StepperEx.SetPosition(
                        //    (Double)value,
                        //    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                    }                    
                }
            }
        }

        private Int64? _targetPosition;
        public Int64? TargetPosition
        {
            get => _targetPosition;
            set
            {
                if (_targetPosition == value) return;
                _targetPosition = value;
                OnPropertyChanged();

                if (value is not null)
                {
                    // Do not check position if Stepper not engaged.
                    // Exception will be thrown if stepper.Position not set.   

                    if (StepperEx.Stepper.Attached
                        && StepperEx.Stepper.steppers[StepperIndex].Engaged)
                    {
                        try
                        {
                            // Set new position if different from current.  Why would we bother to change if same?
                            if (StepperEx.Stepper.steppers[StepperIndex].TargetPosition != value)
                            {
                                StepperEx.SetTargetPosition(
                                    (Int64)value,
                                    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                            }
                        }
                        catch (PhidgetException pex)
                        {
                            // Hopefully this is never thrown.
                            Log.Error(pex, Common.LOG_CATEGORY);
                            StepperEx.Stepper.steppers[StepperIndex].TargetPosition = (Int64)value;
                        }
                    }
                    else
                    {
                        // It is Ok to set position before engaging stepper.
                        //StepperEx.SetPosition(
                        //    (Double)value,
                        //    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                    }
                }
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

                //    if (value is not null)
                //    {
                //        try
                //        {
                //            if (StepperEx.Stepper.steppers[StepperIndex].PositionMax != value)
                //            {
                //                StepperEx.SetPositionMax(
                //                    (Double)value,
                //                    StepperEx.Stepper.steppers[StepperIndex], StepperIndex);
                //            }
                //        }
                //        catch (PhidgetException pex)
                //        {
                //            Log.Error(pex, Common.LOG_CATEGORY);
                //            StepperEx.Stepper.steppers[StepperIndex].Position = (double)value;
                //        }
                //    }
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

        // NOTE(crhodes)
        // Since Position{Min,Max} are ReadOnly, don't see need for these
        //private Double? _devicePositionMax;

        ///// <summary>
        ///// Initial PositionMax after stepperType change
        ///// </summary>
        //public Double? DevicePositionMax
        //{
        //    get => _devicePositionMax;
        //    set
        //    {
        //        if (_devicePositionMax == value) return;
        //        _devicePositionMax = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion Position

        #region Movement Control

        // NOTE(crhodes)
        // Don't think Stepper has this

        //private bool? _speedRamping;

        //public bool? SpeedRamping
        //{
        //    get => _speedRamping;
        //    set
        //    {
        //        if (_speedRamping == value) return;
        //        _speedRamping = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        #endregion Fields and Properties (None)

        /// <summary>
        /// Centers stepper based on Device{Min,Max} if position not set
        /// and Initializes Velocity
        /// </summary>
        /// <param name="motionScale"></param>
        public void InitializeVelocity(MotionScale motionScale)
        {
            StepperStepper stepper = null;

            try
            {
                stepper = StepperEx.Stepper.steppers[StepperIndex];

                if (LogPhidgetEvents)
                {
                    Log.Trace($"Begin stepper:{StepperIndex}" +
                        $" acceleration:{(stepper.Engaged ? stepper.Acceleration : "???")}"
                        + $" velocityLimit:{stepper.VelocityLimit}"
                        + $" currentPosition:{(stepper.Engaged ? stepper.CurrentPosition : "???")}"
                        + $" targetPosition:{(stepper.Engaged ? stepper.TargetPosition : "???")}",
                        Common.LOG_CATEGORY);
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

                //if (stepper.Position = )
                //{

                //}
                //Position = (DevicePositionMax - DevicePositionMin) / 2;

                if (LogPhidgetEvents)
                {
                    Log.Trace($"End stepper:{StepperIndex}" +
                        $" acceleration:{(stepper.Engaged ? stepper.Acceleration : "???")}"
                        + $" velocityLimit:{stepper.VelocityLimit}"
                        + $" currentPosition:{(stepper.Engaged ? stepper.CurrentPosition : "???")}"
                        + $" targetPosition:{(stepper.Engaged ? stepper.TargetPosition : "???")}",
                        Common.LOG_CATEGORY);
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
            StepperStepper stepper = null;

            try
            {
                stepper = StepperEx.Stepper.steppers[StepperIndex];

                if (LogPhidgetEvents)
                {
                    Log.Trace($"Begin stepper:{StepperIndex}" +
                        $" acceleration:{(stepper.Engaged ? stepper.Acceleration : "???")}"
                        + $" velocityLimit:{stepper.VelocityLimit}"
                        + $" currentPosition:{(stepper.Engaged ? stepper.CurrentPosition : "???")}"
                        + $" targetPosition:{(stepper.Engaged ? stepper.TargetPosition : "???")}",
                        Common.LOG_CATEGORY);
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

                //if (stepper.Position = )
                //{

                //}
                //Position = (DevicePositionMax - DevicePositionMin) / 2;

                if (LogPhidgetEvents)
                {
                    Log.Trace($"End stepper:{StepperIndex}" +
                        $" acceleration:{(stepper.Engaged ? stepper.Acceleration : "???")}"
                        + $" velocityLimit:{stepper.VelocityLimit}"
                        + $" currentPosition:{(stepper.Engaged ? stepper.CurrentPosition : "???")}"
                        + $" targetPosition:{(stepper.Engaged ? stepper.TargetPosition : "???")}",
                        Common.LOG_CATEGORY);
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
        /// Gets current values from stepper.  Use when stepperType has not changed
        /// to get current values.  Does not set DevicePosition{Min.Max}
        /// stepper must be Engaged.
        /// </summary>
        public void RefreshPropertiesFromStepper()
        {
            // TODO(crhodes)
            // Maybe we need a version for when stepper is Engaged and when it is not Engaged.
            // Get the stepper outside of the try so we look at it in Exception(s)

            var stepper = StepperEx.Stepper.steppers[StepperIndex];

            try
            {
                //stepperType = stepper.Type;

                Engaged = stepper.Engaged;
                Stopped = stepper.Stopped;
                Current = stepper.Current;

                //SpeedRamping = stepper.SpeedRamping;

                AccelerationMin = stepper.AccelerationMin;
                // NOTE(crhodes)
                // This is interesting.  stepper is not Engaged but stepper.Acceleration is set

                // NOTE(crhodes)
                // If RefreshPropertiesFromstepper immediately after Opening but before Engaged
                // stepper.Acceleration is not set and will throw exception if accessed

                try
                {
                    Acceleration = stepper.Acceleration;
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

                AccelerationMax = stepper.AccelerationMax;

                VelocityMin = stepper.VelocityMin;
                Velocity = stepper.Velocity;

                // NOTE(crhodes)
                // If RefreshPropertiesFromstepper immediately after Opening but before Engaged
                // stepper.Acceleration is not set and will throw exception if accessedk

                try
                {
                    VelocityLimit = stepper.VelocityLimit;
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

                VelocityMax = stepper.VelocityMax;

                PositionMin = stepper.PositionMin;

                // Position is not known if stepper is not engaged

                if (Engaged is true)
                {
                    CurrentPosition = stepper.CurrentPosition;
                    TargetPosition = stepper.TargetPosition;
                }
                else
                {
                    CurrentPosition = null;
                    TargetPosition = null;
                }

                PositionMax = stepper.PositionMax;

                if (LogPhidgetEvents)
                {
                    // NOTE(crhodes)
                    // We use the property for Acceleration, VelocityLimit, and Position to avoid exceptions if stepper not engaged

                    Log.Trace($"stepper:{StepperIndex} engaged:{stepper.Engaged} stopped:{stepper.Stopped} current:{stepper.Current}" +
                        $" accelerationMin:{stepper.AccelerationMin} acceleration:{Acceleration} accelerationMax:{stepper.AccelerationMax}" +
                        $" velocityMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{VelocityLimit} velocityMax:{stepper.VelocityMax}" +
                        $" positionMin:{stepper.PositionMin} currentPosition:{CurrentPosition} targetPosition:{TargetPosition} positionMax:{stepper.PositionMax}",
                        Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} engaged:{stepper.Engaged} stopped:{stepper.Stopped} current:{stepper.Current} speedRamping:{stepper.SpeedRamping}" +
                    //    $" accelerationMin:{stepper.AccelerationMin} acceleration:{stepper.Acceleration: " ??? ")} accelerationMax:{stepper.AccelerationMax}" +
                    //    $" velocityMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{(stepper.Engaged ? stepper.VelocityLimit : "???")} velocityMax:{stepper.VelocityMax}" +
                    //    $" positionMin:{stepper.PositionMin} position:{(stepper.Engaged ? stepper.Position : "???")} positionMax:{stepper.PositionMax}" +
                    //    $" devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} accelerationMin:{stepper.AccelerationMin} acceleration:{(stepper.Engaged ? stepper.Acceleration : "???")} accelerationMax:{stepper.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} velocityMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{(stepper.Engaged ? stepper.VelocityLimit : "???")} velocityMax:{stepper.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} positionMin:{stepper.PositionMin} position:{(stepper.Engaged ? stepper.Position : "???")} positionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
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
        /// Use before setting stepperType
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
                //var stepper = StepperEx.Stepper.steppers[StepperIndex];
                //stepperType = Phidget22.stepperstepper.stepperType.DE;

                Stopped = null;
                Engaged = null;
                //SpeedRamping = null;
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

                //DevicePositionMin = null;
                PositionMin = null;
                CurrentPosition = null;
                TargetPosition = null;
                PositionMax = null;
                //DevicePositionMax = null;
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
        /// Gets properties from stepper.  Use when stepperType changes (happens on open, too)
        /// Sets DevicePosition{Min,Max}
        /// </summary>
        private void GetPropertiesFromstepper()
        {
            // TODO(crhodes)
            // Maybe we need a version for when stepper is Engaged and when it is not Engaged.
            // Get the stepper outside of the try so we look at it in Exception(s)

            StepperStepper stepper = StepperEx.Stepper.steppers[StepperIndex];

            try
            {              
                if (LogPhidgetEvents)
                {
                    Log.Trace($"stepper:{StepperIndex} engaged:{stepper.Engaged}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} engaged:{stepper.Engaged} stopped:{stepper.Stopped} current:{stepper.Current} speedRamping:{stepper.SpeedRamping}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} accelerationMin:{stepper.AccelerationMin} acceleration:{stepper.Acceleration} accelerationMax:{stepper.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} velocityMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{stepper.VelocityLimit} velocityMax:{stepper.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} positionMin:{stepper.PositionMin} position:{(stepper.Engaged ? stepper.Position : "??")} positionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

                // TODO(crhodes)
                // Clean up all these comments

                // These may not be set depending on state of stepper
                // Phidget Library throws exceptions which are caught and ignored.
                // Setting to null keeps UI in sensible state

                //Double? safestepperAcceleration = null;
                //Double? safestepperPosition = null;

                //try
                //{
                //    initialstepperAcceleration = stepper.Acceleration;
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
                //    initialstepperPosition = stepper.Position;
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

                // Having said that, this code is only called when the stepperType changes
                // Seems like we should set these to some sensible value and avoid exceptions
                // Have to be careful to not step on things if stepper is engaged.

                Double? initialstepperAcceleration = stepper.AccelerationMin;
                Int64? initialstepperPosition = (stepper.PositionMax - stepper.PositionMin) / 2; // Midpoint seems reasonable

                Engaged = stepper.Engaged;
                Stopped = stepper.Stopped;
                Current = stepper.Current;

                //SpeedRamping = stepper.SpeedRamping;
                AccelerationMin = stepper.AccelerationMin;

                if (Engaged is true)
                {
                    // NOTE(crhodes)
                    // stepper.Acceleration is not set when first Opening Stepper
                    // and will throw exception if accessed

                    try
                    {
                        Acceleration = stepper.Acceleration;
                    }
                    catch (PhidgetException pex)
                    {
                        Acceleration = initialstepperAcceleration;

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
                    Acceleration = initialstepperAcceleration;
                }
                
                AccelerationMax = stepper.AccelerationMax;

                VelocityMin = stepper.VelocityMin;
                Velocity = stepper.Velocity;

                // NOTE(crhodes)
                // 
                // stepper Type has changed.  Let's set VelocityLimit to a small value
                // Make it possible to move stepper without using UI to set non-zero velocity
                //VelocityLimit = stepper.VelocityLimit == 0 ? 1 : stepper.VelocityLimit;
                //VelocityLimit = stepper.VelocityLimit;
                // Maybe this should onlyh happen if not Engaged

                VelocityLimit = stepper.VelocityLimit;
                //VelocityLimit = VelocityMin + 1;
                VelocityMax = stepper.VelocityMax;

                // DevicePosition{Min,Max} should only be set when stepperType changes

                //DevicePositionMin = stepper.PositionMin;
                PositionMin = stepper.PositionMin;

                if (Engaged is true)
                {
                    CurrentPosition = stepper.CurrentPosition;
                }
                else
                {
                    CurrentPosition = initialstepperPosition;
                }

                PositionMax = stepper.PositionMax;              
                //DevicePositionMax = stepper.PositionMax;

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
                    Log.Trace($"stepper:{StepperIndex} stopped:{stepper.Stopped}" +
                        $" aMin:{stepper.AccelerationMin} acceleration:{Acceleration} aMax:{stepper.AccelerationMax}" +
                        $" vMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{stepper.VelocityLimit} vMax:{stepper.VelocityMax}" +
                        $" posMin:{stepper.PositionMin} position:{initialstepperPosition} positionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} engaged:{stepper.Engaged} stopped:{stepper.Stopped} current:{stepper.Current} speedRamping:{stepper.SpeedRamping}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} accelerationMin:{stepper.AccelerationMin} acceleration:{stepper.Acceleration} accelerationMax:{stepper.AccelerationMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} velocityMin:{stepper.VelocityMin} velocity:{stepper.Velocity} velocityLimit:{stepper.VelocityLimit} velocityMax:{stepper.VelocityMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} positionMin:{stepper.PositionMin} position:{(stepper.Engaged ? stepper.Position : "??")} positionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
                    //Log.Trace($"stepper:{StepperIndex} devicePositionMin:{DevicePositionMin}  devicePositionMax:{DevicePositionMax}", Common.LOG_CATEGORY);
                }

            }
            catch (PhidgetException pex)
            {
                Log.Error($"stepper:{StepperIndex}-{pex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }
    }
}