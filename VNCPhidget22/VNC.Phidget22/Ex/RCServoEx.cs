﻿using System;
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
    public partial class RCServoEx : Phidgets.RCServo, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new RCServo and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="rcServoConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public RCServoEx(Int32 serialNumber, Configuration.RCServoConfiguration configuration, IEventAggregator eventAggregator)
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
        /// Configures RCServo using RCServoConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(RCServoConfiguration configuration)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.DeviceInitalize) startTicks = Log.DEVICE_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            HubPort = configuration.HubPort;
            Channel = configuration.Channel;

            IsRemote = true;

            // NOTE(crhodes)
            // Having these passed in is handy for Performance stuff where there is no UI

            LogPhidgetEvents = configuration.LogPhidgetEvents;
            LogErrorEvents = configuration.LogErrorEvents;
            LogPropertyChangeEvents = configuration.LogPropertyChangeEvents;

            LogPositionChangeEvents = configuration.LogPositionChangeEvents;
            LogVelocityChangeEvents = configuration.LogVelocityChangeEvents;
            LogTargetPositionReachedEvents = configuration.LogTargetPositionReachedEvents;

            LogDeviceChannelSequence = configuration.LogDeviceChannelSequence;
            LogChannelAction = configuration.LogChannelAction;
            LogActionVerification = configuration.LogActionVerification;

            Attach += RCServoEx_Attach;
            Detach += RCServoEx_Detach;
            Error += RCServoEx_Error;
            PropertyChange += RCServoEx_PropertyChange;

            PositionChange += RCServoEx_PositionChange;
            TargetPositionReached += RCServoEx_TargetPositionReached;
            VelocityChange += RCServoEx_VelocityChange;

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

        Boolean _logTargetPositionReachedEvents;
        public Boolean LogTargetPositionReachedEvents
        {
            get { return _logTargetPositionReachedEvents; }
            set { _logTargetPositionReachedEvents = value; OnPropertyChanged(); }
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

        private Int32 _serialNumber;
        public Int32 SerialNumber
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

        private RCServoType _rCServoType = RCServoType.DEFAULT;

        public RCServoType RCServoType
        {
            get => _rCServoType;
            set
            {
                //if (_rCServoType == value)
                //    return;
                _rCServoType = value;
                OnPropertyChanged();
            }
        }

        private Boolean _attached;
        public Boolean Attached
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

        private Boolean _engaged;
        public new Boolean Engaged
        {
            get => _engaged;
            set
            {
                if (_engaged == value)
                    return;
                _engaged = value;

                if (Attached)
                {
                    base.Engaged = value;
                }

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
                    base.Acceleration = (Int32)value;
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

        private Int32 _minDataInterval;
        public new Int32 MinDataInterval
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

        private Double _minPositionServo;
        public Double MinPositionServo
        {
            get => _minPositionServo;
            set
            {
                if (_minPositionServo == value)
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

                if (Attached)
                {
                    base.MinPosition = value;
                }                

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

                if (Attached)
                {
                    base.MaxPosition = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxPositionServo;
        public new Double MaxPositionServo
        {
            get => _maxPositionServo;
            set
            {
                if (_maxPositionServo == value)
                    return;
                _maxPositionServo = value;

                OnPropertyChanged();
            }
        }

        private Double _minPulseWidth;
        public new Double MinPulseWidth
        {
            get => _minPulseWidth;
            set
            {
                if (_minPulseWidth == value)
                    return;
                _minPulseWidth = value;

                base.MinPulseWidth = value;

                OnPropertyChanged();
            }
        }

        private Double _maxPulseWidth;
        public new Double MaxPulseWidth
        {
            get => _maxPulseWidth;
            set
            {
                if (_maxPulseWidth == value)
                    return;
                _maxPulseWidth = value;

                base.MaxPulseWidth = value; ;

                OnPropertyChanged();
            }
        }

        private Double _minPulseWidthLimit;
        public new Double MinPulseWidthLimit
        {
            get => _minPulseWidthLimit;
            set
            {
                if (_minPulseWidthLimit == value)
                    return;
                _minPulseWidthLimit = value;
                OnPropertyChanged();
            }
        }

        private Double _maxPulseWidthLimit;
        public new Double MaxPulseWidthLimit
        {
            get => _maxPulseWidthLimit;
            set
            {
                if (_maxPulseWidthLimit == value)
                    return;
                _maxPulseWidthLimit = value;
                OnPropertyChanged();
            }
        }

        private Boolean _speedRampingState;
        public new Boolean SpeedRampingState
        {
            get => _speedRampingState;
            set
            {
                if (_speedRampingState == value)
                    return;
                _speedRampingState = value;

                base.SpeedRampingState = value;

                OnPropertyChanged();
            }
        }

        private Double _minTorque;
        public new Double MinTorque
        {
            get => _minTorque;
            set
            {
                if (_minTorque == value)
                    return;
                _minTorque = value;
                OnPropertyChanged();
            }
        }

        private Double _torque;
        public new Double Torque
        {
            get => _torque;
            set
            {
                if (_torque == value)
                    return;
                _torque = value;
                OnPropertyChanged();
            }
        }

        private Double _maxTorque;
        public new Double MaxTorque
        {
            get => _maxTorque;
            set
            {
                if (_maxTorque == value)
                    return;
                _maxTorque = value;
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

        private Phidgets.RCServoVoltage _voltage;
        public new Phidgets.RCServoVoltage Voltage
        {
            get => _voltage;
            set
            {
                if (_voltage == value)
                    return;
                _voltage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void RCServoEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.RCServo rcServo = sender as Phidgets.RCServo;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            try
            {
                // Set properties to values from Phidget

                MinPulseWidth = rcServo.MinPulseWidth;
                MaxPulseWidth = rcServo.MaxPulseWidth;

                MinPulseWidthLimit = rcServo.MinPulseWidthLimit;
                MaxPulseWidthLimit = rcServo.MaxPulseWidthLimit;

                SpeedRampingState = rcServo.SpeedRampingState;

                // TODO(crhodes)
                // 
                // This needs to be set before being read
                //SensorUnit = RCServo.SensorUnit;

                MinAcceleration = rcServo.MinAcceleration;
                Acceleration = rcServo.Acceleration;
                MaxAcceleration = rcServo.MaxAcceleration;

                Velocity = rcServo.Velocity;

                MinVelocityLimit = rcServo.MinVelocityLimit;
                VelocityLimit = rcServo.VelocityLimit;
                MaxVelocityLimit = rcServo.MaxVelocityLimit;

                //MinDataInterval = rcServo.MinDataInterval;
                //DataInterval = rcServo.DataInterval;
                //MaxDataInterval = rcServo.MaxDataInterval;

                //MinDataRate = rcServo.MinDataRate;
                //DataRate = rcServo.DataRate;
                //MaxDataRate = rcServo.MaxDataRate;

                // MinPosition can be set.  Save initial limit
                MinPositionServo = MinPosition = MinPositionStop = rcServo.MinPosition;

                // MaxPosition can be set.  Save initial limit
                MaxPositionServo = MaxPosition = MaxPositionStop = rcServo.MaxPosition;

                // NOTE(crhodes)
                // Position cannot be read until initially set
                // Initialize in middle of range
                Position = (rcServo.MaxPosition - rcServo.MinPosition) / 2;
                // Have to set TargetPosition before engaging
                TargetPosition = Position;
            
                Voltage = rcServo.Voltage;

                // NOTE(crhodes)
                // Shockingly, the Attached property is not set until after Attach Event
                // It is also R/O so we cannot set.

                //Attached = rcServo.Attached;

                // Just set it so UI behaves well
                Attached = true;

                Engaged = rcServo.Engaged;

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // Not all RCServo support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
            //    MinFailsafeTime = rcServo.MinFailsafeTime;
            //    MaxFailsafeTime = rcServo.MaxFailsafeTime;
            //    MinTorque = rcServo.MinTorque;
            //    Torque = rcServo.Torque;
            //    MaxTorque = rcServo.MaxTorque;
            //}
            //catch (Phidgets.PhidgetException ex)
            //{
            //    if (ex.ErrorCode != Phidgets.ErrorCode.Unsupported)
            //    {
            //        throw ex;
            //    }
            //}
        }

        private void RCServoEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void RCServoEx_PositionChange(object sender, PhidgetsEvents.RCServoPositionChangeEventArgs e)
        {
            Phidgets.RCServo rcServo = sender as Phidgets.RCServo;

            if (LogPositionChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_PositionChange: sender:{sender} position:{e.Position}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Position = e.Position;
            IsMoving = rcServo.IsMoving;
        }

        private void RCServoEx_VelocityChange(object sender, PhidgetsEvents.RCServoVelocityChangeEventArgs e)
        {
            Phidgets.RCServo rcServo = sender as Phidgets.RCServo;

            if (LogVelocityChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_VelocityChange: sender:{sender} velocity:{e.Velocity}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Velocity = e.Velocity;
            IsMoving = rcServo.IsMoving;
        }

        private void RCServoEx_TargetPositionReached(object sender, PhidgetsEvents.RCServoTargetPositionReachedEventArgs e)
        {
            Phidgets.RCServo rcServo = sender as Phidgets.RCServo;

            if (LogTargetPositionReachedEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_TargetPositionReached: sender:{sender} position:{e.Position}", Common.LOG_CATEGORY, StartTargetPositionTime);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Position = e.Position;
            IsMoving = rcServo.IsMoving;
            NewPositionAchieved = true;
        }

        private void RCServoEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void RCServoEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"RCServoEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter(timeout:{timeout}) isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;
            RefreshProperties();

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

        public new void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isAttached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);

            try
            {
                // NOTE(crhodes)
                // These are not supported by the 16x RC Servo Phidget
                // Going to remove for all

                //MinDataInterval = base.MinDataInterval;
                //DataInterval = base.DataInterval;
                ////DataInterval = 100; // 100ms (10Hz)
                //MaxDataInterval = base.MaxDataInterval;

                //MinDataRate = base.MinDataRate;
                //DataRate = base.DataRate;
                ////DataRate = 10; // 10 Hz (100ms)
                //MaxDataRate = base.MaxDataRate;
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents) Log.Trace($"Exit isAttached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(RCServoSequence rcServoSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops:>{rcServoSequence.Name}<" +
                        $"\r startActionLoopSequences:>{rcServoSequence.StartActionLoopSequences?.Count()}<" +
                        $"\r actionLoops:>{rcServoSequence.ActionLoops}<" +
                        $" actions:>{rcServoSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{rcServoSequence.ActionsDuration}<" +
                        $"\r endActionLoopSequences:>{rcServoSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (rcServoSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < rcServoSequence.ActionLoops; actionLoop++)
                    {
                        if (rcServoSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            DeviceChannelSequencePlayer player = DeviceChannelSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in rcServoSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (rcServoSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}< actions:{rcServoSequence.Actions.Count()}", Common.LOG_CATEGORY);

                            Parallel.ForEach(rcServoSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}< actions:{rcServoSequence.Actions.Count()}", Common.LOG_CATEGORY);

                            foreach (RCServoAction action in rcServoSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (rcServoSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{rcServoSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)rcServoSequence.ActionsDuration);
                        }

                        if (rcServoSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in rcServoSequence.EndActionLoopSequences)
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

        private async Task PerformAction(RCServoAction action)
        {
            Int64 startTicks = 0;

            var channel = Channel;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter hubPort:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);
                //actionMessage.Append($"servo:{Channel}");
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
                if (action.LogTargetPositionReachedEvents is not null) LogTargetPositionReachedEvents = (Boolean)action.LogTargetPositionReachedEvents;

                if (action.LogDeviceChannelSequence is not null) LogDeviceChannelSequence = (Boolean)action.LogDeviceChannelSequence;
                if (action.LogChannelAction is not null) LogChannelAction = (Boolean)action.LogChannelAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion

                if (action.RCServoType is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" servoType:>{action.RCServoType}<");

                    RCServoType = (RCServoType)action.RCServoType;

                    // TODO(crhodes)
                    // 

                    RCServoPulseWidths rcServoPulseWidths = VNC.Phidget22.PhidgetDeviceLibrary.RCServoTypes[RCServoType];

                    // TODO(crhodes)
                    // Do we need to disengage first?
                    MinPulseWidth = rcServoPulseWidths.MinPulseWidth;
                    MaxPulseWidth = rcServoPulseWidths.MaxPulseWidth;

                    // NOTE(crhodes)
                    // Maybe we should sleep for a little bit to allow this to happen
                    Thread.Sleep(1);


                    // Save the refreshed values
                    // FIX(crhodes)
                    // 
                    //SaveServoLimits(servo, index);
                }

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

                if (action.PositionScaleMin is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" positionMin:>{action.PositionScaleMin}<");

                    SetPositionScaleMin((Double)action.PositionScaleMin);
                }

                if (action.PositionScaleMax is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" positionMax:>{action.PositionScaleMax}<");

                    SetPositionScaleMax((Double)action.PositionScaleMax);
                }

                if (action.PositionStopMin is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" positionMin:>{action.PositionStopMin}<");

                    SetPositionScaleMin((Double)action.PositionStopMin);
                }

                if (action.PositionStopMax is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" positionMax:>{action.PositionStopMax}<");

                    SetPositionStopMax((Double)action.PositionStopMax);
                }

                if (action.SpeedRampingState is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" speedRampingState:>{action.SpeedRampingState}<");

                    SpeedRampingState = (Boolean)action.SpeedRampingState;
                }

                // NOTE(crhodes)
                // Engage the servo before doing other actions as some,
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
                    
                    VerifyNewPositionAchieved(targetPosition);
                }

                if (action.RelativePosition is not null)
                {
                    var targetPosition = TargetPosition + (Double)action.RelativePosition;
                    if (LogChannelAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({targetPosition})");

                    NewPositionAchieved = false;    // TargetPositionReached Eventhandler will set true;

                    TargetPosition = targetPosition;

                    VerifyNewPositionAchieved(targetPosition);
                }

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }

                if (LogChannelAction)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
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

            var rcServoSequence = args.RCServoSequence;

            await RunActionLoops(rcServoSequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //private void VerifyServoEngaged(Phidget22.AdvancedServoServo servo, Int32 index)
        //{
        //    Int64 startTicks = 0;
        //    var msSleep = 0;

        //    try
        //    {
        //        if (LogActionVerification)
        //        {
        //            startTicks = Log.Trace($"Enter servo:{index} engaged:{servo.Engaged}", Common.LOG_CATEGORY);
        //        }

        //        do
        //        {
        //            Thread.Sleep(1);
        //            msSleep++;
        //        } while (servo.Engaged != true);

        //        if (LogActionVerification)
        //        {
        //            Log.Trace($"Exit servo:{index} engaged:{servo.Engaged} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        private void VerifyNewPositionAchieved(Double targetPosition)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogActionVerification)
                {
                    startTicks = Log.Trace($"Enter targetPosition:{targetPosition}", Common.LOG_CATEGORY);
                }

                do
                {
                    if (LogActionVerification) Log.Trace($"velocity:{Velocity,8:0.000} position:{Position,7:0.000}" +
                        $" - isMoving:{IsMoving}", Common.LOG_CATEGORY);
                    Thread.Sleep(1);
                    msSleep++;
                }
                while (NewPositionAchieved is false);

                if (LogActionVerification)
                {
                    Log.Trace($"Exit Position:{Position,7:0.000} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
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
