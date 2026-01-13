using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidget22;

using Prism.Events;

using VNC.Phidget22.ChannelConfiguration;
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
        public StepperEx(Int32 serialNumber, StepperConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter:", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<StepperSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures Stepper using StepperConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(StepperConfiguration configuration)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.DeviceInitializeLow) startTicks = Log.DEVICE_INITIALIZE_LOW($"Enter" +
                $"s#:{configuration.DeviceSerialNumber} hp:{configuration.HubPort} c:{configuration.Channel}", Common.LOG_CATEGORY);

            DeviceSerialNumber = configuration.DeviceSerialNumber;
            IsHubPortDevice = configuration.HubPortDevice;
            HubPort = configuration.HubPort;
            Channel = configuration.Channel;

            SerialHubPortChannel = new SerialHubPortChannel
            {
                SerialNumber = DeviceSerialNumber,
                HubPort = HubPort,
                Channel = Channel,
                IsHubPortDevice = IsHubPortDevice
            };

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

            if (Core.Common.VNCLogging.DeviceInitializeLow) Log.DEVICE_INITIALIZE_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        // NOTE(crhodes)
        // UI binds to these properties so need to use INPC
        // as UI is bound before Attach fires to update properties from Phidget

        #region Logging

        Boolean _logPhidgetEvents;
        public Boolean LogPhidgetEvents
        {
            get { return _logPhidgetEvents; }
            set { _logPhidgetEvents = value; OnPropertyChanged(); }
        }

        Boolean _logErrorEvents = true;    // probably always want to see Errors
        public Boolean LogErrorEvents
        {
            get { return _logErrorEvents; }
            set { _logErrorEvents = value; OnPropertyChanged(); }
        }

        Boolean _logPropertyChangeEvents;
        public Boolean LogPropertyChangeEvents
        {
            get { return _logPropertyChangeEvents; }
            set { _logPropertyChangeEvents = value; OnPropertyChanged(); }
        }

        Boolean _logPositionChangeEvents;
        public Boolean LogPositionChangeEvents
        {
            get { return _logPositionChangeEvents; }
            set { _logPositionChangeEvents = value; OnPropertyChanged(); }
        }

        Boolean _logVelocityChangeEvents;
        public Boolean LogVelocityChangeEvents
        {
            get { return _logVelocityChangeEvents; }
            set { _logVelocityChangeEvents = value; OnPropertyChanged(); }
        }

        Boolean _logStoppedEvents;
        public Boolean LogStoppedEvents
        {
            get { return _logStoppedEvents; }
            set { _logStoppedEvents = value; OnPropertyChanged(); }
        }

        Boolean _logDeviceChannelSequence;
        public Boolean LogDeviceChannelSequence
        {
            get { return _logDeviceChannelSequence; }
            set { _logDeviceChannelSequence = value; OnPropertyChanged(); }
        }

        Boolean _logChannelAction;
        public Boolean LogChannelAction
        {
            get { return _logChannelAction; }
            set { _logChannelAction = value; OnPropertyChanged(); }
        }

        Boolean _logActionVerification;
        public Boolean LogActionVerification
        {
            get { return _logActionVerification; }
            set { _logActionVerification = value; OnPropertyChanged(); }
        }

        #endregion

        private SerialHubPortChannel _serialHubPortChannel;
        public SerialHubPortChannel SerialHubPortChannel
        {
            get => _serialHubPortChannel;
            set
            {
                _serialHubPortChannel = value;
                OnPropertyChanged();
            }
        }

        private Boolean _attached;
        public new Boolean Attached
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

        #region Data Interval and Rate

        private Int32 _minDataInterval;
        public new Int32 MinDataInterval
        {
            get => _minDataInterval;
            set
            {
                _minDataInterval = value;
                OnPropertyChanged();
            }
        }

        private Int32 _DataInterval;
        public new Int32 DataInterval
        {
            get => _DataInterval;
            set
            {
                if (_DataInterval == value)
                    return;
                _DataInterval = value;

                if (Attached)
                {
                    base.DataInterval = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        private Int32 _maxDataInterval;
        public new Int32 MaxDataInterval
        {
            get => _maxDataInterval;
            set
            {
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
                    base.DataRate = (Int32)value;
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
                _maxDataRate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region StepperEx

        private Boolean _engaged;
        public new Boolean Engaged
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

        private Boolean _isMoving;
        public new Boolean IsMoving
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
                _minPositionStepper = value;
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
                // Always have to set TargetPosition before engaging.
                // The wrapped property could also be reset on Close()
                // decided to fix here

                //if (_targetPosition == value)
                //    return;

                if (value < MinPositionStop)
                {
                    Log.WARNING($"Attempt to set targetPostion:{value} below MinPositionStop:{MinPositionStop}", Common.LOG_CATEGORY);
                    base.TargetPosition = _targetPosition = MinPositionStop;
                }
                else if (value > MaxPositionStop)
                {
                    Log.WARNING($"Attempt to set targetPostion:{value} above MaxPositionStop:{MaxPositionStop}", Common.LOG_CATEGORY);
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
        public Double MaxPositionStepper
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
        public Double MinVelocity
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
        public Double MaxVelocity
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
                MinPosition = base.MinPosition;
                Position = base.Position;
                MaxPosition = base.MaxPosition;

                OnPropertyChanged();
            }
        }

        // This is for UI.  The Phidget.Stepper does not have this property
        // This might go away once we better understand RescaleFactor

        private Double _stepAngle;
        public Double StepAngle
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
        public new Phidgets.StepperControlMode ControlMode
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

        #endregion

        #region Event Handlers

        private void StepperEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.Stepper stepper = (Phidgets.Stepper)sender;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"StepperEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            try
            {
                // TODO(crhodes)
                // Put things here that need to be initialized
                // Use constructor configuration if need to pass things in
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                if (pex.ErrorCode != Phidgets.ErrorCode.Unsupported)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Exit StepperEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            switch (e.PropertyName)
            {
                case "DataRate":
                    DataRate = base.DataRate;
                    break;

                case "DataInterval":
                    DataInterval = base.DataInterval;
                    break;

                default:
                    Log.EVENT_HANDLER($"StepperEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
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
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open();

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen}  timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Gather properties from Open Phidget Device
        /// </summary>
        public void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            try
            {
                // Set properties to values from Phidget
                // We do not use Attach as some Phidgets do not provide values until Open

                Engaged = base.Engaged;

                // TODO(crhodes)
                // 
                // This needs to be set before being read
                //SensorUnit = stepper.SensorUnit;

                MinAcceleration = base.MinAcceleration;
                Acceleration = base.Acceleration;
                MaxAcceleration = base.MaxAcceleration;

                Velocity = base.Velocity;

                MinVelocityLimit = base.MinVelocityLimit;
                VelocityLimit = base.VelocityLimit;
                MaxVelocityLimit = base.MaxVelocityLimit;

                MinDataInterval = base.MinDataInterval;
                DataInterval = base.DataInterval;
                MaxDataInterval = base.MaxDataInterval;

                MinDataRate = base.MinDataRate;
                DataRate = base.DataRate;
                MaxDataRate = base.MaxDataRate;

                // MinPosition can be set.  Save initial limit
                MinPositionStepper = MinPosition = MinPositionStop = base.MinPosition;

                // MaxPosition can be set.  Save initial limit
                MaxPositionStepper = MaxPosition = MaxPositionStop = base.MaxPosition;

                //// NOTE(crhodes)
                //// Position cannot be read until initially set
                //// Initialize in middle of range
                //Position = (rcServo.MaxPosition - stepper.MinPosition) / 2;
                //// Have to set TargetPosition before engaging
                //TargetPosition = Position;

                MinCurrentLimit = base.MinCurrentLimit;
                CurrentLimit = base.CurrentLimit;
                MaxCurrentLimit = base.MaxCurrentLimit;

                ControlMode = base.ControlMode;
                RescaleFactor = base.RescaleFactor;
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void AddPositionOffset(Double offset)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter isOpen:{IsOpen} attached:{Attached} attached:{Attached}", Common.LOG_CATEGORY);

            base.AddPositionOffset(offset);

            // NOTE(crhodes)
            // Unfortunately no events are fired by AddPositionOffset()
            // Go get new values;

            MinPosition = base.MinPosition;
            Position = base.Position;
            MaxPosition = base.MaxPosition;

            TargetPosition = base.TargetPosition;

            if (LogPhidgetEvents) Log.TRACE($"Exit isOpen:{IsOpen} attached:{Attached} attached:{Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(StepperSequence stepperSequence)
        {
            long startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.TRACE(
                        $"RunActionLoops(>{stepperSequence.Name}<)" +
                        $" startActionLoopSequences:>{stepperSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{stepperSequence.ActionLoops}<" +
                        $" serialNumber:>{DeviceSerialNumber}<" +
                        $" hubPort:>{HubPort}< >{stepperSequence.HubPort}<" +
                        $" channel:>{Channel}< >{stepperSequence.Channel}<" +
                        $" actions:>{stepperSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{stepperSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{stepperSequence.EndActionLoopSequences?.Count()}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (stepperSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < stepperSequence.ActionLoops; actionLoop++)
                    {
                        if (stepperSequence.StartActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in stepperSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (stepperSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.TRACE($"Parallel Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{stepperSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            // Parallel.ForEachAsync automatically waits for all iterations
                            // Requires a CancellationToken parameter in the lambda

                            CancellationToken cancellationToken = CancellationToken.None;
                            await Parallel.ForEachAsync(stepperSequence.Actions, cancellationToken, async (action, token) =>
                            {
                                ExecuteAction(action);
                                await Task.CompletedTask;
                            });

                            //Parallel.ForEach(stepperSequence.Actions, action =>
                            //{
                            //     ExecuteAction(action);
                            //});
                        }
                        else
                        {
                            if (LogChannelAction) Log.TRACE($"Sequential Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{stepperSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            foreach (StepperAction action in stepperSequence.Actions)
                            {
                                ExecuteAction(action);
                            }
                        }

                        if (stepperSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.TRACE($"Zzzz End of Actions" +
                                    $" Sleeping:>{stepperSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)stepperSequence.ActionsDuration);
                        }

                        if (stepperSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

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
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }

            if (LogChannelAction) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods
        
        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer()
        {
            Int64 startTicks = 0;
            if (LogDeviceChannelSequence) startTicks = Log.TRACE($"Enter", Common.LOG_CATEGORY);

            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);

            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            // TODO(crhodes)
            // Add appropriate events for this device

            player.LogPhidgetEvents = LogPhidgetEvents;

            if (LogDeviceChannelSequence) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);

            return player;
        }

        private void ExecuteAction(StepperAction action)
        {
            Int64 startTicks = 0;

             StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.TRACE($"Enter DeviceSerialNumber:{DeviceSerialNumber}" +
                    $" hubPort:{HubPort} channel:{Channel}" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
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
                    Open(Phidget.DefaultTimeout);
                }

                if (action.Close is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" close:>{action.Close}<");

                    Close();
                }

                #region StepperEx Actions

                // NOTE(crhodes)
                // Not sure if these can be done without Stepper engaged

                if (action.StepAngle is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" stepAngle:>{action.StepAngle}<");

                    StepAngle = (Double)action.StepAngle;
                }

                if (action.RescaleFactor is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" rescaleFactor:>{action.RescaleFactor}<");

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

                    SetAcceleration ((Double)acceleration);
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

                //if (action.MinPositionStop is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMin:>{action.MinPositionStop}<");

                //    SetPositionScaleMin((Double)action.MinPositionStop);
                //}

                //if (action.MaxPositionStop is not null)
                //{
                //    if (LogChannelAction) actionMessage.Append($" positionMax:>{action.MaxPositionStop}<");

                //    SetMaxPositionStop((Double)action.MaxPositionStop);
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

                    SetAcceleration (newAcceleration);
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

                    //    Log.TRACE($"Before currentp:{currentp} targetPosition:{targetPosition} newp:{newp} ", Common.LOG_CATEGORY);

                    //    IAsyncResult result = BeginSetTargetPosition(targetPosition, delegate (IAsyncResult result)
                    //    {
                    //        try
                    //        {
                    //            EndSetTargetPosition(result);
                    //            newp = Position;
                    //            asyncCallComplete = true;
                    //            Log.TRACE($"IAsync currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted}", Common.LOG_CATEGORY);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Log.ERROR(ex, Common.LOG_CATEGORY);
                    //        }
                    //    }, null);

                    //    Log.TRACE($"After currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted}", Common.LOG_CATEGORY);

                    //var msSleep = 0;
                    //while (! asyncCallComplete)
                    //{
                    //    Thread.Sleep(1);
                    //    msSleep++;
                    //}

                    //Log.TRACE($"After2 currentp:{currentp} targetPosition:{targetPosition} newp:{newp} result:{result.IsCompleted} msSleep:{msSleep}", Common.LOG_CATEGORY);

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

                #endregion

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" Zzzz - End of Action Sleeping:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                //Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"PhidgetException deviceSerialNumber:>{DeviceSerialNumber}<" +
                    $" hubPort:>{HubPort}< channel:>{Channel}<" +
                    $" {actionMessage}" +
                    $" source:{pex.Source}" +
                    $" description:{pex.Description}" +
                    $" inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogChannelAction)
                {
                    Log.TRACE($"Exit deviceSerialNumber:{DeviceSerialNumber}" +
                        $" hubPort:{HubPort} channel:{Channel} {actionMessage}" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        private async void TriggerSequence(SequenceEventArgs args)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var sequence = args.StepperSequence;

            if (sequence is not null) await RunActionLoops(sequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="servo"></param>
        private void SetAcceleration(Double acceleration)
        {
            try
            {
                if (LogChannelAction)
                {
                    Log.TRACE($"Begin acceleration:{acceleration}" +
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
                    Log.TRACE($"End acceleration:{Acceleration}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.ERROR($"acceleration:{acceleration}" +
                //    $" minAcceleration:{servo.AccelerationMin}" +
                //    //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
                //    $" maxAcceleration:{servo.AccelerationMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }


        /// <summary>
        /// Bounds check and set velocity
        /// </summary>
        /// <param name="velocityLimit"></param>
        /// <param name="servo"></param>
        private void SetVelocityLimit(Double velocityLimit)
        {
            try
            {
                if (LogChannelAction)
                {
                    Log.TRACE($"Begin velocityLimit:{velocityLimit}" +
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
                    Log.TRACE($"End velocityLimit:{VelocityLimit}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.ERROR($"index:{index}" +
                //    $" velocity:{velocityLimit}" +
                //    $" servo.velocityMin:{servo.VelocityMin}" +
                //    $" servo.velocityLimit:{servo.VelocityLimit}" +
                //    $" servo.velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMin"></param>
        /// <param name="servo"></param>
        private void SetPositionScaleMin(Double positionMin)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.TRACE($"Begin positionMin:{positionMin}" +
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
                    Log.TRACE($"End positionMin:{positionMin} MinPosition:{MinPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.ERROR($"index:{index} positionMin:{positionMin}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMin"></param>
        /// <param name="servo"></param>
        private void SetMinPositionStop(Double positionStopMin)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.TRACE($"Begin positionMin:{positionMin}" +
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
                    Log.TRACE($"End positionMin:{positionStopMin} MinPosition:{MinPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.ERROR($"index:{index} positionMin:{positionMin}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="servo"></param>
        private Double SetPosition(Double position)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.TRACE($"Begin servo:{index} position:{position}" +
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
                    Log.TRACE($"End position:{position} servo.Position:{Position}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                //Log.ERROR($"servo:{index} servo.position:{servo.Position}" +
                //    $" servo.PositionMin:{servo.PositionMin}" +
                //    $" servo.PositionMax:{servo.PositionMax}" +
                //    $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
                //    $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }

            return position;
        }


        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMax"></param>
        /// <param name="servo"></param>
        private void SetMaxPositionStop(Double positionMax)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.TRACE($"Begin servo:{index} positionMax:{positionMax}" +
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
                    Log.TRACE($"End positionMax:{positionMax} MaxPosition:{MaxPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMax"></param>
        /// <param name="servo"></param>
        private void SetPositionScaleMax(Double positionMax)
        {
            try
            {
                if (LogChannelAction)
                {
                    //Log.TRACE($"Begin servo:{index} positionMax:{positionMax}" +
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
                    Log.TRACE($"End positionMax:{positionMax} MaxPosition:{MaxPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
                Log.ERROR($"source:{pex.Source} type:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
#if LOGGING
            long startTicks = 0;
            if (Common.VNCCoreLogging.INPC) startTicks = Log.VIEWMODEL_LOW($"Enter ({propertyName})", Common.LOG_CATEGORY);
#endif
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#if LOGGING
            if (Common.VNCCoreLogging.INPC) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
#endif
        }

        #endregion
    }
}
