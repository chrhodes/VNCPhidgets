using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    public class StepperEx : Phidgets.Stepper, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new Stepper and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="stepperConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public StepperEx(int serialNumber, StepperConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<RCServoSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures Stepper using StepperConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(StepperConfiguration configuration)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.DeviceInitalize) startTicks = Log.DEVICE_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            HostComputer = configuration.HostComputer;
            DeviceSerialNumber = SerialNumber;
            Channel = configuration.Channel;

            IsRemote = true;

            // NOTE(crhodes)
            // Having these passed in is handy for Performance stuff where there is no UI

            LogPhidgetEvents = configuration.LogPhidgetEvents;
            LogErrorEvents = configuration.LogErrorEvents;
            LogPropertyChangeEvents = configuration.LogPropertyChangeEvents;

            LogPositionChangeEvents = configuration.LogPositionChangeEvents;
            LogVelocityChangeEvents = configuration.LogVelocityChangeEvents;
            LogStoppedEvents = configuration.LogStoppedEvents;

            LogDeviceChannelSequence = configuration.LogDeviceChannelSequence;
            LogChannelAction = configuration.LogChannelAction;
            LogActionVerification = configuration.LogActionVerification;

            Attach += StepperEx_Attach;
            Detach += StepperEx_Detach;
            Error += StepperEx_Error;
            PropertyChange += StepperEx_PropertyChange;

            PositionChange += StepperEx_PositionChange;
            Stopped += StepperEx_Stopped;
            VelocityChange += StepperEx_VelocityChange;

            if (Core.Common.VNCLogging.DeviceInitalize) Log.DEVICE_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        #region Logging

        // NOTE(crhodes)
        // UI binds to these properties so need to use INPC

        bool _logPhidgetEvents;
        public bool LogPhidgetEvents
        {
            get { return _logPhidgetEvents; }
            set { _logPhidgetEvents = value; OnPropertyChanged(); }
        }

        bool _logErrorEvents = true;    // probably always want to see Errors
        public bool LogErrorEvents
        {
            get { return _logErrorEvents; }
            set { _logErrorEvents = value; OnPropertyChanged(); }
        }

        bool _logPropertyChangeEvents;
        public bool LogPropertyChangeEvents
        {
            get { return _logPropertyChangeEvents; }
            set { _logPropertyChangeEvents = value; OnPropertyChanged(); }
        }

        bool _logPositionChangeEvents;
        public bool LogPositionChangeEvents
        {
            get { return _logPositionChangeEvents; }
            set { _logPositionChangeEvents = value; OnPropertyChanged(); }
        }

        bool _logVelocityChangeEvents;
        public bool LogVelocityChangeEvents
        {
            get { return _logVelocityChangeEvents; }
            set { _logVelocityChangeEvents = value; OnPropertyChanged(); }
        }

        bool _logStoppedEvents;
        public bool LogStoppedEvents
        {
            get { return _logStoppedEvents; }
            set { _logStoppedEvents = value; OnPropertyChanged(); }
        }

        bool _logDeviceChannelSequence;
        public bool LogDeviceChannelSequence
        {
            get { return _logDeviceChannelSequence; }
            set { _logDeviceChannelSequence = value; OnPropertyChanged(); }
        }

        bool _logChannelAction;
        public bool LogChannelAction
        {
            get { return _logChannelAction; }
            set { _logChannelAction = value; OnPropertyChanged(); }
        }

        bool _logActionVerification;
        public bool LogActionVerification
        {
            get { return _logActionVerification; }
            set { _logActionVerification = value; OnPropertyChanged(); }
        }

        #endregion

        private string _hostComputer;
        public string HostComputer
        {
            get => _hostComputer;
            set
            {
                _hostComputer = value;
                OnPropertyChanged();
            }

        }
        private int _serialNumber;
        public int SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber == value)
                    return;
                _serialNumber = value;
                DeviceSerialNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _attached;
        public bool Attached
        {
            get => _attached;
            set
            {
                if (_attached == value)
                    return;
                _attached = value;
                OnPropertyChanged();
            }
        }

        private bool _engaged;
        public new bool Engaged
        {
            get => _engaged;
            set
            {
                if (_engaged == value)
                    return;
                _engaged = value;

                base.Engaged = value;

                OnPropertyChanged();
            }
        }

        private Double _minAcceleration;
        public new Double MinAcceleration
        {
            get => _minAcceleration;
            set
            {
                if (_minAcceleration == value)
                    return;
                _minAcceleration = value;
                OnPropertyChanged();
            }
        }

        private Double _Acceleration;
        public new Double Acceleration
        {
            get => _Acceleration;
            set
            {
                if (_Acceleration == value)
                    return;
                _Acceleration = value;

                if (Attached)
                {
                    base.Acceleration = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxAcceleration;
        public new Double MaxAcceleration
        {
            get => _maxAcceleration;
            set
            {
                if (_maxAcceleration == value)
                    return;
                _maxAcceleration = value;
                OnPropertyChanged();
            }
        }

        private Double _minCurrentLimit;
        public new Double MinCurrentLimit
        {
            get => _minCurrentLimit;
            set
            {
                if (_minCurrentLimit == value)
                    return;
                _minCurrentLimit = value;
                OnPropertyChanged();
            }
        }

        private Double _CurrentLimit;
        public new Double CurrentLimit
        {
            get => _CurrentLimit;
            set
            {
                if (_CurrentLimit == value)
                    return;
                _CurrentLimit = value;

                if (Attached)
                {
                    base.CurrentLimit = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxCurrentLimit;
        public new Double MaxCurrentLimit
        {
            get => _maxCurrentLimit;
            set
            {
                if (_maxCurrentLimit == value)
                    return;
                _maxCurrentLimit = value;
                OnPropertyChanged();
            }
        }

        private int _minDataInterval;
        public new int MinDataInterval
        {
            get => _minDataInterval;
            set
            {
                if (_minDataInterval == value)
                    return;
                _minDataInterval = value;
                OnPropertyChanged();
            }
        }

        private int _DataInterval;
        public new int DataInterval
        {
            get => _DataInterval;
            set
            {
                if (_DataInterval == value)
                    return;
                _DataInterval = value;

                if (Attached)
                {
                    base.DataInterval = value;
                }

                OnPropertyChanged();
            }
        }

        private int _maxDataInterval;
        public new int MaxDataInterval
        {
            get => _maxDataInterval;
            set
            {
                if (_maxDataInterval == value)
                    return;
                _maxDataInterval = value;
                OnPropertyChanged();
            }
        }

        private Double _minDataRate;
        public new Double MinDataRate
        {
            get => _minDataRate;
            set
            {
                if (_minDataRate == value)
                    return;
                _minDataRate = value;
                OnPropertyChanged();
            }
        }

        private Double _DataRate;
        public new Double DataRate
        {
            get => _DataRate;
            set
            {
                if (_DataRate == value)
                    return;
                _DataRate = value;

                if (Attached)
                {
                    base.DataRate = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxDataRate;
        public new Double MaxDataRate
        {
            get => _maxDataRate;
            set
            {
                if (_maxDataRate == value)
                    return;
                _maxDataRate = value;
                OnPropertyChanged();
            }
        }

        private Double _minFailsafeTime;
        public new Double MinFailsafeTime
        {
            get => _minFailsafeTime;
            set
            {
                if (_minFailsafeTime == value)
                    return;
                _minFailsafeTime = value;
                OnPropertyChanged();
            }
        }

        private Double _maxFailsafeTime;
        public new Double MaxFailsafeTime
        {
            get => _maxFailsafeTime;
            set
            {
                if (_maxFailsafeTime == value)
                    return;
                _maxFailsafeTime = value;
                OnPropertyChanged();
            }
        }

        private Double _holdingCurrentLimit;
        public new Double HoldingCurrentLimit
        {
            get => _holdingCurrentLimit;
            set
            {
                if (_holdingCurrentLimit == value)
                    return;
                _holdingCurrentLimit = value;

                if (Attached)
                {
                    base.HoldingCurrentLimit = value;
                }

                OnPropertyChanged();
            }
        }

        private bool _isMoving;
        public new bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value)
                    return;
                _isMoving = value;
                OnPropertyChanged();
            }
        }

        private Double _minPositionStepper;
        public Double MinPositionStepper
        {
            get => _minPositionStepper;
            set
            {
                if (_minPositionStepper == value)
                    return;
                _minPosition = value;
                OnPropertyChanged();
            }
        }

        private Double _minPositionStop;
        public Double MinPositionStop
        {
            get => _minPositionStop;
            set
            {
                if (_minPositionStop == value)
                    return;
                _minPositionStop = value;
                OnPropertyChanged();
            }
        }

        private Double _minPosition;
        public new Double MinPosition
        {
            get => _minPosition;
            set
            {
                if (_minPosition == value)
                    return;
                _minPosition = value;
                OnPropertyChanged();
            }
        }

        private Double _Position;
        public new Double Position
        {
            get => _Position;
            set
            {
                if (_Position == value)
                    return;
                _Position = value;
                OnPropertyChanged();
            }
        }

        private Double _targetPosition;
        public new Double TargetPosition
        {
            get => _targetPosition;
            set
            {
                // NOTE(crhodes)
                // Always have to set TargetPostion before engaging.
                // The wrapped property could also be reset on Close()
                // decided to fix here

                //if (_targetPosition == value)
                //    return;

                if (value < MinPositionStop)
                {
                    Log.Warning($"Attempt to set targetPostion:{value} below MinPositionStop:{MinPositionStop}", Common.LOG_CATEGORY);
                    base.TargetPosition = _targetPosition = MinPositionStop;
                }
                else if (value > MaxPositionStop)
                {
                    Log.Warning($"Attempt to set targetPostion:{value} above MaxPositionStop:{MaxPositionStop}", Common.LOG_CATEGORY);
                    base.TargetPosition = _targetPosition = MaxPositionStop;
                }
                else
                {
                    _targetPosition = value;

                    base.TargetPosition = (Double)value;
                }

                OnPropertyChanged();
            }
        }

        Int64 StartTargetPositionTime;

        Boolean NewPositionAchieved = false;

        private Double _maxPositionStop;
        public Double MaxPositionStop
        {
            get => _maxPositionStop;
            set
            {
                if (_maxPositionStop == value)
                    return;
                _maxPositionStop = value;
                OnPropertyChanged();
            }
        }

        private Double _maxPosition;
        public new Double MaxPosition
        {
            get => _maxPosition;
            set
            {
                if (_maxPosition == value)
                    return;
                _maxPosition = value;
                OnPropertyChanged();
            }
        }

        private Double _maxPositionStepper;
        public new Double MaxPositionStepper
        {
            get => _maxPositionStepper;
            set
            {
                if (_maxPositionStepper == value)
                    return;
                _maxPositionStepper = value;

                OnPropertyChanged();
            }
        }

        private Double _minVelocity;
        public new Double MinVelocity
        {
            get => _minVelocity;
            set
            {
                if (_minVelocity == value)
                    return;
                _minVelocity = value;
                OnPropertyChanged();
            }
        }

        private Double _maxVelocity;
        public new Double MaxVelocity
        {
            get => _maxVelocity;
            set
            {
                if (_maxVelocity == value)
                    return;
                _maxVelocity = value;
                OnPropertyChanged();
            }
        }

        private Double _Velocity;
        public new Double Velocity
        {
            get => _Velocity;
            set
            {
                if (_Velocity == value)
                    return;
                _Velocity = value;
                OnPropertyChanged();
            }
        }

        private Double _minVelocityLimit;
        public new Double MinVelocityLimit
        {
            get => _minVelocityLimit;
            set
            {
                if (_minVelocityLimit == value)
                    return;
                _minVelocityLimit = value;
                OnPropertyChanged();
            }
        }

        private Double _VelocityLimit;
        public new Double VelocityLimit
        {
            get => _VelocityLimit;
            set
            {
                if (_VelocityLimit == value)
                    return;
                _VelocityLimit = value;

                base.VelocityLimit = value;

                OnPropertyChanged();
            }
        }

        private Double _maxVelocityLimit;
        public new Double MaxVelocityLimit
        {
            get => _maxVelocityLimit;
            set
            {
                if (_maxVelocityLimit == value)
                    return;
                _maxVelocityLimit = value;
                OnPropertyChanged();
            }
        }

        private Double _rescaleFactor;
        public new Double RescaleFactor
        {
            get => _rescaleFactor;
            set
            {
                if (_rescaleFactor == value)
                    return;
                _rescaleFactor = value;

                base.RescaleFactor = (Double)value;

                // NOTE(crhodes)
                // Unfortunately no events are fired by setting new RescaleFactor
                // Go get new values;
                RefreshServoProperties();

                OnPropertyChanged();
            }
        }

        // This is for UI.  The Phidget.Stepper does not have this property
        // This might go away once we better understand RescaleFactor

        private Double _stepAngle;
        public new Double StepAngle
        {
            get => _stepAngle;
            set
            {
                if (_stepAngle == value)
                    return;
                _stepAngle = value;

                //base.RescaleFactor = (Double)value;

                //// NOTE(crhodes)
                //// Unfortunately no events are fired by setting new RescaleFactor
                //// Go get new values;
                //RefreshServoProperties();

                OnPropertyChanged();
            }
        }

        private Phidgets.StepperControlMode _controlMode;
        public Phidgets.StepperControlMode ControlMode
        {
            get => _controlMode;
            set
            {
                if (_controlMode == value)
                    return;
                _controlMode = value;

                base.ControlMode = value;

                OnPropertyChanged();
            }
        }        

        #endregion

        #region Event Handlers

        private void StepperEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.Stepper stepper = sender as Phidgets.Stepper;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //Attached = dOutput.Attached;

            // Just set it so UI behaves well
            Attached = true;

            Engaged = stepper.Engaged;

            // TODO(crhodes)
            // 
            // This needs to be set before being read
            //SensorUnit = stepper.SensorUnit;

            MinAcceleration = stepper.MinAcceleration;
            Acceleration = stepper.Acceleration;
            MaxAcceleration = stepper.MaxAcceleration;

            Velocity = stepper.Velocity;

            MinVelocityLimit = stepper.MinVelocityLimit;
            VelocityLimit = stepper.VelocityLimit;
            MaxVelocityLimit = stepper.MaxVelocityLimit;

            MinDataInterval = stepper.MinDataInterval;
            DataInterval = stepper.DataInterval;
            MaxDataInterval = stepper.MaxDataInterval;

            MinDataRate = stepper.MinDataRate;
            DataRate = stepper.DataRate;
            MaxDataRate = stepper.MaxDataRate;

            // MinPosition can be set.  Save initial limit
            MinPositionStepper = MinPosition = MinPositionStop = stepper.MinPosition;

            // MaxPosition can be set.  Save initial limit
            MaxPositionStepper = MaxPosition = MaxPositionStop = stepper.MaxPosition;

            //// NOTE(crhodes)
            //// Position cannot be read until initially set
            //// Initialize in middle of range
            //Position = (rcServo.MaxPosition - stepper.MinPosition) / 2;
            //// Have to set TargetPosition before engaging
            //TargetPosition = Position;

            MinCurrentLimit = stepper.MinCurrentLimit;
            CurrentLimit = stepper.CurrentLimit;
            MaxCurrentLimit = stepper.MaxCurrentLimit;

            ControlMode = stepper.ControlMode;
            RescaleFactor = stepper.RescaleFactor;

            // Not all RCServo support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
            //    MinFailsafeTime = stepper.MinFailsafeTime;
            //    MaxFailsafeTime = stepper.MaxFailsafeTime;
            //}
            //catch (Phidgets.PhidgetException ex)
            //{
            //    if (ex.ErrorCode != Phidgets.ErrorCode.Unsupported)
            //    {
            //        throw ex;
            //    }
            //}
        }

        private void StepperEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void StepperEx_PositionChange(object sender, PhidgetsEvents.StepperPositionChangeEventArgs e)
        {
            if (LogPositionChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_PositionChange: sender:{sender} {e.Position}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Position = e.Position;
        }

        private void StepperEx_Stopped(object sender, PhidgetsEvents.StepperStoppedEventArgs e)
        {
            if (LogStoppedEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_Stopped: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void StepperEx_VelocityChange(object sender, PhidgetsEvents.StepperVelocityChangeEventArgs e)
        {
            if (LogVelocityChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_VelocityChange: sender:{sender} {e.Velocity}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Velocity = e.Velocity;
        }

        private void StepperEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void StepperEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        #endregion

        #region Commands (none)


        #endregion

        #region Public Methods

        public new void Open()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Open();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void AddPositionOffset(Double offset)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{Attached} isAttached:{Attached}", Common.LOG_CATEGORY);

            base.AddPositionOffset(offset);
            // NOTE(crhodes)
            // Unfortunately no events are fired by AddPositionOffset()
            // Go get new values;
            RefreshServoProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{Attached} isAttached:{Attached}", Common.LOG_CATEGORY, startTicks);
        }

        void RefreshServoProperties()
        {
            MinAcceleration = base.MinAcceleration;
            Acceleration = base.Acceleration;
            MaxAcceleration = base.MaxAcceleration;

            MinVelocityLimit = base.MinVelocityLimit;
            VelocityLimit = base.VelocityLimit;
            MaxVelocityLimit = base.MaxVelocityLimit;

            MinPosition = base.MinPosition;
            Position = base.Position;
            MaxPosition = base.MaxPosition;

            TargetPosition = base.TargetPosition;
        }

        public async Task RunActionLoops(StepperSequence stepperSequence)
        {
            long startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" name:>{stepperSequence.Name}<" +
                        $" startActionLoopSequences:>{stepperSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{stepperSequence.ActionLoops}<" +
                        $" actions:>{stepperSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{stepperSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{stepperSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (stepperSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < stepperSequence.ActionLoops; actionLoop++)
                    {
                        if (stepperSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            DeviceChannelSequencePlayer player = DeviceChannelSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in stepperSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (stepperSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(stepperSequence.Actions, async action =>
                            {
                                 await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (StepperAction action in stepperSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (stepperSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{stepperSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((int)stepperSequence.ActionsDuration);
                        }

                        if (stepperSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in stepperSequence.EndActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogChannelAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // FIX(crhodes)
        // Ugh, lots to fix

        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="servo"></param>
        public void SetAcceleration(Double acceleration)
        {
            try
            {
                if (LogChannelAction)
                {
                    Log.Trace($"Begin acceleration:{acceleration}" +
                        $" minAcceleration:{MinAcceleration}" +
                        $" acceleration:{Acceleration}" +
                        $" maxAcceleration:{MaxAcceleration}", Common.LOG_CATEGORY);
                }

                if (acceleration < MinAcceleration)
                {
                    Acceleration = MinAcceleration;
                }
                else if (acceleration > MaxAcceleration)
                {
                    Acceleration = MaxAcceleration;
                }
                else
                {
                    Acceleration = acceleration;
                }

                if (LogChannelAction)
                {
                    Log.Trace($"End acceleration:{Acceleration}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.Error($"acceleration:{acceleration}" +
                //    $" minAcceleration:{servo.AccelerationMin}" +
                //    //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
                //    $" maxAcceleration:{servo.AccelerationMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }


        /// <summary>
        /// Bounds check and set velocity
        /// </summary>
        /// <param name="velocityLimit"></param>
        /// <param name="servo"></param>
        public void SetVelocityLimit(Double velocityLimit)
        {
            try
            {
                if (LogChannelAction)
                {
                    Log.Trace($"Begin velocityLimit:{velocityLimit}" +
                        $" MinVelocityLimit:{MinVelocityLimit}" +
                        $" VelocityLimit:{VelocityLimit}" +
                        $" MaxVelocityLimit:{MaxVelocityLimit}", Common.LOG_CATEGORY);
                }

                if (velocityLimit < MinVelocityLimit)
                {
                    VelocityLimit = MinVelocityLimit;
                }
                else if (velocityLimit > MaxVelocityLimit)
                {
                    VelocityLimit = MaxVelocityLimit;
                }
                else
                {
                    VelocityLimit = velocityLimit;
                }

                if (LogChannelAction)
                {
                    Log.Trace($"End velocityLimit:{VelocityLimit}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.Error($"index:{index}" +
                //    $" velocity:{velocityLimit}" +
                //    $" servo.velocityMin:{servo.VelocityMin}" +
                //    $" servo.velocityLimit:{servo.VelocityLimit}" +
                //    $" servo.velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMin"></param>
        /// <param name="servo"></param>
        public void SetPositionScaleMin(Double positionMin)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.Trace($"Begin positionMin:{positionMin}" +
                    //    $" PositionMin:{PositionMin}" +
                    //    $" PositionMax:{servo.PositionMax}" +
                    //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                //if (positionMin < 0)
                //{
                //    positionMin = InitialServoLimits[index].DevicePositionMin;
                //}
                //else if (positionMin < InitialServoLimits[index].DevicePositionMin)
                //{
                //    positionMin = InitialServoLimits[index].DevicePositionMin;
                //}
                //else if (positionMin > servo.PositionMax)
                //{
                //    positionMin = servo.PositionMax;
                //}

                //if (servo.PositionMin != positionMin) servo.PositionMin = positionMin;

                // TODO(crhodes)
                // Figure out if we need any of the above

                MinPosition = positionMin;

                if (LogChannelAction)
                {
                    Log.Trace($"End positionMin:{positionMin} MinPosition:{MinPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.Error($"index:{index} positionMin:{positionMin}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMin"></param>
        /// <param name="servo"></param>
        public void SetPositionStopMin(Double positionStopMin)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.Trace($"Begin positionMin:{positionMin}" +
                    //    $" PositionMin:{PositionMin}" +
                    //    $" PositionMax:{servo.PositionMax}" +
                    //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                //if (positionMin < 0)
                //{
                //    positionMin = InitialServoLimits[index].DevicePositionMin;
                //}
                //else if (positionMin < InitialServoLimits[index].DevicePositionMin)
                //{
                //    positionMin = InitialServoLimits[index].DevicePositionMin;
                //}
                //else if (positionMin > servo.PositionMax)
                //{
                //    positionMin = servo.PositionMax;
                //}

                //if (servo.PositionMin != positionMin) servo.PositionMin = positionMin;

                // TODO(crhodes)
                // Figure out if we need any of the above

                MinPositionStop = positionStopMin;

                if (LogChannelAction)
                {
                    Log.Trace($"End positionMin:{positionStopMin} MinPosition:{MinPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.Error($"index:{index} positionMin:{positionMin}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        public Double SetPosition(Double position)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.Trace($"Begin servo:{index} position:{position}" +
                    //    $" servo.PositionMin:{servo.PositionMin}" +
                    //    $" servo.PositionMax:{servo.PositionMax}" +
                    //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                //if (position < servo.PositionMin)
                //{
                //    position = servo.PositionMin;
                //}
                //else if (position > servo.PositionMax)
                //{
                //    position = servo.PositionMax;
                //}

                //// TODO(crhodes)
                //// Maybe save last position set and not bother checking servo.Position is same
                //if (servo.Position != position) servo.Position = position;

                if (LogChannelAction)
                {
                    Log.Trace($"End position:{position} servo.Position:{Position}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.Error($"servo:{index} servo.position:{servo.Position}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return position;
        }


        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMax"></param>
        /// <param name="servo"></param>
        public void SetPositionStopMax(Double positionMax)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.Trace($"Begin servo:{index} positionMax:{positionMax}" +
                    //    $" servo.PositionMin:{servo.PositionMin}" +
                    //    $" servo.PositionMax:{servo.PositionMax}" +
                    //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                //if (positionMax < 0)
                //{
                //    positionMax = InitialServoLimits[index].DevicePositionMax;
                //}
                //else if (positionMax < servo.PositionMin)
                //{
                //    positionMax = servo.PositionMin;
                //}
                //else if (positionMax > InitialServoLimits[index].DevicePositionMax)
                //{
                //    positionMax = InitialServoLimits[index].DevicePositionMax;
                //}

                //if (servo.PositionMax != positionMax) servo.PositionMax = positionMax;

                MaxPosition = positionMax;

                if (LogChannelAction)
                {
                    Log.Trace($"End positionMax:{positionMax} MaxPosition:{MaxPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMax"></param>
        /// <param name="servo"></param>
        public void SetPositionScaleMax(Double positionMax)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.Trace($"Begin servo:{index} positionMax:{positionMax}" +
                    //    $" servo.PositionMin:{servo.PositionMin}" +
                    //    $" servo.PositionMax:{servo.PositionMax}" +
                    //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                    //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                //if (positionMax < 0)
                //{
                //    positionMax = InitialServoLimits[index].DevicePositionMax;
                //}
                //else if (positionMax < servo.PositionMin)
                //{
                //    positionMax = servo.PositionMin;
                //}
                //else if (positionMax > InitialServoLimits[index].DevicePositionMax)
                //{
                //    positionMax = InitialServoLimits[index].DevicePositionMax;
                //}

                //if (servo.PositionMax != positionMax) servo.PositionMax = positionMax;

                MaxPosition = positionMax;

                if (LogChannelAction)
                {
                    Log.Trace($"End positionMax:{positionMax} MaxPosition:{MaxPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        private async Task PerformAction(StepperAction action)
        {
            Int64 startTicks = 0;

            var channel = Channel;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter servo:{Channel}", Common.LOG_CATEGORY);
                actionMessage.Append($"servo:{Channel}");
            }

            try
            {
                // NOTE(crhodes)
                // First make any logging changes

                #region Logging

                if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean)action.LogPhidgetEvents;
                if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean)action.LogErrorEvents;
                if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean)action.LogPropertyChangeEvents;

                if (action.LogPositionChangeEvents is not null) LogPositionChangeEvents = (Boolean)action.LogPositionChangeEvents;
                if (action.LogVelocityChangeEvents is not null) LogVelocityChangeEvents = (Boolean)action.LogVelocityChangeEvents;
                if (action.LogStoppedEvents is not null) LogStoppedEvents = (Boolean)action.LogStoppedEvents;

                if (action.LogDeviceChannelSequence is not null) LogDeviceChannelSequence = (Boolean)action.LogDeviceChannelSequence;
                if (action.LogChannelAction is not null) LogChannelAction = (Boolean)action.LogChannelAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion

                if (action.Open is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" open:>{action.Open}<");

                    // TODO(crhodes)
                    // Do we need a delay here?
                    // This is where a call back from Attach event would be great!
                    Open();
                }

                if (action.Close is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" close:>{action.Close}<");

                    Close();
                }

                // NOTE(crhodes)
                // Not sure if these can be done without Stepper engaged

                if (action.StepAngle is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" stepAngle:>{action.StepAngle}<");

                    StepAngle = (Double)action.StepAngle;
                }

                if (action.RescaleFactor is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" rescaleFactor:>{action.StepAngle}<");

                    RescaleFactor = (Double)action.RescaleFactor;
                }

                // NOTE(crhodes)
                // These can be performed without the servo being engaged.  This helps address
                // previous values that get applied when servo engaged.
                // The servo still snaps to last position when enabled.  No known way to address that.

                if (action.Acceleration is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");
                    var acceleration = action.Acceleration;

                    if (acceleration < 0)
                    {
                        if (acceleration == -1)        // -1 is magic number for AccelerationMin :)
                        {
                            acceleration = MinAcceleration;
                            //acceleration = InitialServoLimits[index].AccelerationMin;
                        }
                        else if (acceleration == -2)   // -2 is magic number for AccelerationMax :)
                        {
                            acceleration = MaxAcceleration;
                            //acceleration = InitialServoLimits[index].AccelerationMax;
                        }
                    }

                    SetAcceleration((Double)acceleration);
                }

                if (action.VelocityLimit is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");
                    var velocityLimit = action.VelocityLimit;

                    if (velocityLimit < 0)
                    {
                        if (velocityLimit == -1)        // -1 is magic number for VelocityMin :)
                        {
                            velocityLimit = MinVelocityLimit;
                        }
                        else if (velocityLimit == -2)   // -2 is magic number for VelocityMax :)
                        {
                            velocityLimit = MaxVelocityLimit;
                        }
                    }

                    SetVelocityLimit((Double)velocityLimit);
                }

                //if (action.PositionScaleMin is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMin:>{action.PositionScaleMin}<");

                //    SetPositionScaleMin((Double)action.PositionScaleMin);
                //}

                //if (action.PositionScaleMax is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMax:>{action.PositionScaleMax}<");

                //    SetPositionScaleMax((Double)action.PositionScaleMax);
                //}

                //if (action.PositionStopMin is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMin:>{action.PositionStopMin}<");

                //    SetPositionScaleMin((Double)action.PositionStopMin);
                //}

                //if (action.PositionStopMax is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMax:>{action.PositionStopMax}<");

                //    SetPositionStopMax((Double)action.PositionStopMax);
                //}

                //if (action.SpeedRampingState is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" speedRampingState:>{action.SpeedRampingState}<");

                //    SpeedRampingState = (Boolean)action.SpeedRampingState;
                //}

                // NOTE(crhodes)
                // Engage the stepper before doing other actions as some,
                // e.g. TargetPosition, requires servo to be engaged.

                if (action.Engaged is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" engaged:>{action.Engaged}<");

                    Engaged = (Boolean)action.Engaged;

                    //if ((Boolean)action.Engaged) VerifyServoEngaged(servo, index);
                }

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = Acceleration + (Double)action.RelativeAcceleration;
                    if (LogChannelAction) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration);
                }

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = VelocityLimit + (Double)action.RelativeVelocityLimit;
                    if (LogChannelAction) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocityLimit(newVelocityLimit);
                }

                if (action.TargetPosition is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");

                    Double targetPosition = (Double)action.TargetPosition;

                    if (targetPosition < 0)
                    {
                        if (action.TargetPosition == -1)        // -1 is magic number for MinPosition :)
                        {
                            targetPosition = MinPosition;
                        }
                        else if (action.TargetPosition == -2)   // -2 is magic number for MaxPosition :)
                        {
                            targetPosition = MaxPosition;
                        }
                    }

                    //var currentp = Position;
                    //var newp = Position;

                    //Boolean asyncCallComplete = false;

                    //    Log.Trace($"Before currentp:{currentp} targetPosition:{targetPosition} newp:{newp} ", Common.LOG_CATEGORY);

                    //    IAsyncResult result = BeginSetTargetPosition(targetPosition, delegate (IAsyncResult result)
                    //    {
                    //        try
                    //        {
                    //            EndSetTargetPosition(result);
                    //            newp = Position;
                    //            asyncCallComplete = true;
                    //            Log.Trace($"IAsync currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted}", Common.LOG_CATEGORY);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Log.Error(ex, Common.LOG_CATEGORY);
                    //        }
                    //    }, null);

                    //    Log.Trace($"After currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted}", Common.LOG_CATEGORY);

                    //var msSleep = 0;
                    //while (! asyncCallComplete)
                    //{
                    //    Thread.Sleep(1);
                    //    msSleep++;
                    //}

                    //Log.Trace($"After2 currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted} msSleep:{msSleep}", Common.LOG_CATEGORY);

                    //TargetPosition = targetPosition;

                    // TODO(crhodes)
                    // Figure out how to use new event

                    NewPositionAchieved = false;    // TargetPositionReached Eventhandler will set true;
                    StartTargetPositionTime = Stopwatch.GetTimestamp();

                    TargetPosition = targetPosition;

                    //VerifyNewPositionAchieved(targetPosition);
                }

                if (action.RelativePosition is not null)
                {
                    var targetPosition = TargetPosition + (Double)action.RelativePosition;
                    if (LogChannelAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({targetPosition})");

                    NewPositionAchieved = false;    // TargetPositionReached Eventhandler will set true;
                    StartTargetPositionTime = Stopwatch.GetTimestamp();

                    TargetPosition = targetPosition;

                    //VerifyNewPositionAchieved(targetPosition);
                }

                if (action.RelativeTargetDegrees is not null)
                {
                    Double circle = 360;
                    var circleSteps = circle / StepAngle;

                    var degrees = action.RelativeTargetDegrees;

                    var stepsToMove = (degrees / StepAngle) * 16; // 1/16 steps

                    var targetPosition = TargetPosition + stepsToMove;

                    if (LogChannelAction) actionMessage.Append($" relativeTargetDegress:>{action.RelativeTargetDegrees}< ({targetPosition})");

                    NewPositionAchieved = false;    // TargetPositionReached Eventhandler will set true;
                    StartTargetPositionTime = Stopwatch.GetTimestamp();

                    TargetPosition = (Double)targetPosition;

                    //VerifyNewPositionAchieved(targetPosition);
                }

                if (action.AddPositionOffset is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" addPositionOffset:>{action.AddPositionOffset}<");

                    AddPositionOffset((Double)action.AddPositionOffset);
                }

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{Channel} source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogChannelAction)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        private async void TriggerSequence(SequenceEventArgs args)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var stepperSequence = args.StepperSequence;

            await RunActionLoops(stepperSequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }  

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        // This is the traditional approach - requires string name to be passed in

        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            long startTicks = 0;
#if LOGGING
            if (Common.VNCCoreLogging.INPC) startTicks = Log.VIEWMODEL_LOW($"Enter ({propertyName})", Common.LOG_CATEGORY);
#endif
            // This is the new CompilerServices attribute!

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#if LOGGING
            if (Common.VNCCoreLogging.INPC) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
#endif
        }

        #endregion
    }
}
