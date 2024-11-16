using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;
using Phidget22;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VNC.Phidget22.Ex
{
    public partial class RCServoEx : Phidgets.RCServo, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly Configuration.RCServoConfiguration _rcServoConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new RCServo and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="rcServoConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public RCServoEx(int serialNumber, Configuration.RCServoConfiguration rcServoConfiguration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _rcServoConfiguration = rcServoConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<RCServoSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures RCServo using RCServoConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _rcServoConfiguration.Channel;
            IsRemote = true;

            Attach += RCServoEx_Attach;
            Detach += RCServoEx_Detach;
            Error += RCServoEx_Error;
            PropertyChange += RCServoEx_PropertyChange;

            PositionChange += RCServoEx_PositionChange;
            TargetPositionReached += RCServoEx_TargetPositionReached;
            VelocityChange += RCServoEx_VelocityChange;

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)

        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        #region Logging

        public bool LogPhidgetEvents { get; set; }
        public bool LogErrorEvents { get; set; }
        public bool LogPropertyChangeEvents { get; set; }

        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }

        public bool LogTargetPositionReachedEvents { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        #endregion

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
        
        private bool _isAttached;
        public bool IsAttached
        {
            get => _isAttached;
            set
            {
                if (_isAttached == value)
                    return;
                _isAttached = value;
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
                    base.Acceleration = (int)value;
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
                    base.DataInterval = (int)value;
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
                    base.DataRate = (int)value;
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

        private Double _minPositionServo;
        public new Double MinPositionServo
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
                if (_targetPosition == value)
                    return;
                _targetPosition = value;

                base.TargetPosition = (Double)value;

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

        private bool _speedRampingState;
        public new bool SpeedRampingState
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

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //IsAttached = dOutput.Attached;

            // Just set it so UI behaves well
            IsAttached = true;

            Engaged = rcServo.Engaged;

            // TODO(crhodes)
            // 
            // This needs to be set before being read
            //SensorUnit = RCServo.SensorUnit;

            MinAcceleration = rcServo.MinAcceleration;
            Acceleration = rcServo.Acceleration;
            MaxAcceleration = rcServo.MaxAcceleration;

            MinDataInterval = rcServo.MinDataInterval;
            DataInterval = rcServo.DataInterval;
            MaxDataInterval = rcServo.MaxDataInterval;

            MinDataRate = rcServo.MinDataRate;
            DataRate = rcServo.DataRate;
            MaxDataRate = rcServo.MaxDataRate;

            // MinPosition can be set.  Save initial limit
            MinPositionServo = MinPosition = rcServo.MinPosition;

            // NOTE(crhodes)
            // Position cannot be read until initially set
            // Initialize in middle of range
            Position = (rcServo.MaxPosition - rcServo.MinPosition) / 2;
            // Have to set TargetPosition before engaging
            TargetPosition = Position;

            // MaxPosition can be set.  Save initial limit
            MaxPositionServo = MaxPosition = rcServo.MaxPosition;

            MinPulseWidth = rcServo.MinPulseWidth;
            MaxPulseWidth = rcServo.MaxPulseWidth;

            MinPulseWidthLimit = rcServo.MinPulseWidthLimit;
            MaxPulseWidthLimit = rcServo.MaxPulseWidthLimit;

            Velocity = rcServo.Velocity;

            MinVelocityLimit = rcServo.MinVelocityLimit;
            VelocityLimit = rcServo.VelocityLimit;
            MaxVelocityLimit = rcServo.MaxVelocityLimit;

            SpeedRampingState = rcServo.SpeedRampingState;
            
            Voltage = rcServo.Voltage;

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
                    Log.EVENT_HANDLER($"RCServoEx_TargetPositionReached: sender:{sender} position:{e.Position}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Position = e.Position;
            IsMoving = rcServo.IsMoving;
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

            IsAttached = false;
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

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        //public new void Open(int? timeOut = null)
        //{
        //    long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        // FIX(crhodes)
        //        // There is no AdvancedServo in Phidget22.
        //        // This is where we probably open all the stuff on the Phidget
        //        // or we can just open what we use

        //        var rcServoCount = _deviceChannels.RCServoCount;
        //        var currentInputCount = _deviceChannels.CurrentInputCount;

        //        // HACK(crhodes)
        //        // Trying to get new RCC0004_0 AdvancedServo board to work with
        //        // Legacy SBC1 or SBC2.  No luck.
        //        // Only seems to work it directly attached, sigh.
        //        //AdvancedServo.open(SerialNumber);
        //        //AdvancedServo.open(-1);

        //        // TODO(crhodes)
        //        // Decide if want to open everything or pass in config to only open what we need

        //        for (int i = 0; i < rcServoCount; i++)
        //        {
        //            RCServos[i].Open();
        //        }

        //        for (int i = 0; i < currentInputCount; i++)
        //        {
        //            CurrentInputs[i].Open();
        //        }

        //        //AdvancedServo.open(SerialNumber, Host.IPAddress, Host.Port);

        //        //if (timeOut is not null)
        //        //{
        //        //    AdvancedServo.waitForAttachment((Int32)timeOut); 
        //        //}
        //        //else 
        //        //{ 
        //        //    AdvancedServo.waitForAttachment();
        //        //}

        //    }
        //    catch (Phidgets.PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"source:{pex.Source} message:{pex.Message} description:{pex.Description} detail:{pex.Detail} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public event EventHandler PhidgetDeviceAttached;

        //override protected void PhidgetDeviceIsAttached()
        //{
        //    OnPhidgetDeviceAttached(new EventArgs());
        //}

        //// NOTE(crhodes)
        //// This tells the UI that we have an attached Phidget

        //protected virtual void OnPhidgetDeviceAttached(EventArgs e)
        //{
        //    PhidgetDeviceAttached?.Invoke(this, e);
        //}

        //public void Close()
        //{
        //    long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

        //    try
        //    {
        //        // FIX(crhodes)
        //        // There is no AdvancedServo in Phidget22.
        //        // This is where we probably close all the stuff on the Phidget
        //        // or we can just close what we use

        //        var rcServoCount = _deviceChannels.RCServoCount;
        //        var currentInputCount = _deviceChannels.CurrentInputCount;

        //        // NOTE(crhodes)
        //        // We may be logging events.  Remove them before closing

        //        if (LogCurrentChangeEvents) LogCurrentChangeEvents = false;
        //        if (LogPositionChangeEvents) LogPositionChangeEvents = false;
        //        if (LogVelocityChangeEvents) LogVelocityChangeEvents = false;

        //        // TODO(crhodes)
        //        // Decide if want to close everything or pass in config to only open what we need

        //        for (int i = 0; i < rcServoCount; i++)
        //        {
        //            RCServos[i].Close();
        //        }

        //        for (int i = 0; i < currentInputCount; i++)
        //        {
        //            CurrentInputs[i].Close();
        //        }

        //        //AdvancedServo.close();
        //    }
        //    catch (Phidgets.PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"source:{pex.Source} message:{pex.Message} description:{pex.Description} detail:{pex.Detail} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        public async Task RunActionLoops(RCServoSequence rcServoSequence)
        {
            long startTicks = 0;

            try
            {
                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" rcServoSequence:>{rcServoSequence.Name}<" +
                        $" startActionLoopSequences:>{rcServoSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{rcServoSequence.ActionLoops}<" +
                        $" actions:>{rcServoSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{rcServoSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{rcServoSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (rcServoSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < rcServoSequence.ActionLoops; actionLoop++)
                    {
                        if (rcServoSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in rcServoSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (rcServoSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(rcServoSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (RCServoAction action in rcServoSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (rcServoSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{rcServoSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((int)rcServoSequence.ActionsDuration);
                        }

                        if (rcServoSequence.EndActionLoopSequences is not null)
                        {
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(_eventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in rcServoSequence.EndActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogSequenceAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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
                if (LogSequenceAction)
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

                if (LogSequenceAction)
                {
                    Log.Trace($"End acceleration:{Acceleration}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
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
                if (LogSequenceAction)
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

                if (LogSequenceAction)
                {
                    Log.Trace($"End velocityLimit:{VelocityLimit}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
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
        public void SetPositionMin(Double positionMin)
        {
            try
            {
                if (LogSequenceAction)
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

                if (LogSequenceAction)
                {
                    Log.Trace($"End positionMin:{positionMin} MinPosition:{MinPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
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
                if (LogSequenceAction)
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

                if (LogSequenceAction)
                {
                    Log.Trace($"End position:{position} servo.Position:{Position}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
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
        public void SetPositionMax(Double positionMax)
        {
            try
            {
                if (LogSequenceAction)
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

                if (LogSequenceAction)
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

        // FIX(crhodes)
        // Ugh, lots to fix

        // TODO(crhodes)
        // Do we really need this as all it does is SaveServoLimits

        //private void SaveServoLimits()
        //{
        //    try
        //    {
        //        AdvancedServoServoCollection servos = AdvancedServo.servos;
        //        AdvancedServoServo servo = null;

        //        // Save the device position min/max before any changes are made

        //        for (int index = 0; index < servos.Count; index++)
        //        {
        //            servo = servos[index];

        //            //if (LogPhidgetEvents)
        //            //{
        //            //    Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged}", Common.LOG_CATEGORY);

        //            //    //string currentPosition = servo.Engaged ? servo.Position.ToString() : "???";

        //            //    //Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged} position:{currentPosition}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} velocityMin:{servo.VelocityMin} positionMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //            //}

        //            // NOTE(crhodes)
        //            // Force the initial Servo Type to avoid opening something that has
        //            // been set to an unexpected Type, e.g. RAW_us_MODE or 
        //            // Which have crazy values

        //            // NOTE(crhodes)
        //            // What happens if we don't set type?

        //            //if (servo.Type == ServoServo.ServoType.RAW_us_MODE)
        //            //{
        //            //servo.Type = ServoServo.ServoType.DEFAULT;
        //            //}

        //            SaveServoLimits(servo, index);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void SaveServoLimits(AdvancedServoServo servo, Int32 index)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged}" +
        //            $" accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}" +
        //            $" positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}" +
        //            $" velocityMin:{servo.VelocityMin} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} velocityMin:{servo.VelocityMin} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //    }

        //    // NOTE(crhodes)
        //    // We do not need to save Acceleration Min,Max and Velocity Min,Max,
        //    // they cannot change, but, useful to have available
        //    // when setting Acceleration/VelocityLimit to Min/Max in PerformAction

        //    InitialServoLimits[index].AccelerationMin = servo.AccelerationMin;
        //    InitialServoLimits[index].AccelerationMax = servo.AccelerationMax;
        //    InitialServoLimits[index].DevicePositionMin = servo.PositionMin;
        //    //InitialServoLimits[i].PositionMin = servo.PositionMin; 
        //    //InitialServoLimits[i].PositionMax = servo.PositionMax;
        //    InitialServoLimits[index].DevicePositionMax = servo.PositionMax;
        //    InitialServoLimits[index].VelocityMin = servo.VelocityMin + 1; // 0 won't move
        //    InitialServoLimits[index].VelocityMax = servo.VelocityMax;
        //}

        private async Task PerformAction(RCServoAction action)
        {
            Int64 startTicks = 0;

            Int32 index = action.ServoIndex;
            var channel = Channel;

            StringBuilder actionMessage = new StringBuilder();

            if (LogSequenceAction)
            {
                startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"servo:{index}");
            }

            //RCServo servo = AdvancedServo.servos[index];

            try
            {
                if (action.RCServoType is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" servoType:>{action.RCServoType}<");

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

                // NOTE(crhodes)
                // These can be performed without the servo being engaged.  This helps address
                // previous values that get applied when servo engaged.
                // The servo still snaps to last position when enabled.  No known way to address that.

                if (action.Acceleration is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");
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
                    if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");
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

                if (action.PositionMin is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" positionMin:>{action.PositionMin}<");

                    SetPositionMin((Double)action.PositionMin);
                }

                if (action.PositionMax is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" positionMax:>{action.PositionMax}<");

                    SetPositionMax((Double)action.PositionMax);
                }

                // NOTE(crhodes)
                // Engage the servo before doing other actions as some,
                // e.g. TargetPosition, requires servo to be engaged.

                if (action.Engaged is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" engaged:>{action.Engaged}<");

                    Engaged = (Boolean)action.Engaged;

                    //if ((Boolean)action.Engaged) VerifyServoEngaged(servo, index);
                }

                //if (action.Acceleration is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");

                //    SetAcceleration((Double)action.Acceleration, servo, index);
                //}

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = Acceleration + (Double)action.RelativeAcceleration;
                    if (LogSequenceAction) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration);
                }

                //if (action.VelocityLimit is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

                //    SetVelocityLimit((Double)action.VelocityLimit, servo, index);
                //}

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = VelocityLimit + (Double)action.RelativeVelocityLimit;
                    if (LogSequenceAction) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocityLimit(newVelocityLimit);
                }

                if (action.TargetPosition is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");

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

                    TargetPosition = targetPosition;

                    // TODO(crhodes)
                    // Figure out how to use new event

                    //VerifyNewPositionAchieved(servo, index, SetPosition(targetPosition, servo, index));
                }

                if (action.RelativePosition is not null)
                {
                    var targetPosition = TargetPosition + (Double)action.RelativePosition;
                    if (LogSequenceAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({targetPosition})");

                    TargetPosition = targetPosition;
                    // TODO(crhodes)
                    // Figure out how to use new event

                    //VerifyNewPositionAchieved(servo, index, SetPosition(targetPosition, servo, index));
                }

                if (action.Duration > 0)
                {
                    if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"servo:{index} source:{pex.Source} description:{pex.Description} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogSequenceAction)
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

        //private void VerifyNewPositionAchieved(AdvancedServoServo servo, Int32 index, Double targetPosition)
        //{
        //    Int64 startTicks = 0;
        //    var msSleep = 0;

        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            startTicks = Log.Trace($"Enter servo:{index} targetPosition:{targetPosition}", Common.LOG_CATEGORY);
        //        }

        //        //while (servo.Position != targetPosition)
        //        //{
        //        //    Thread.Sleep(1);
        //        //    msSleep++;
        //        //}

        //        // NOTE(crhodes)
        //        // Maybe poll velocity != 0
        //        do
        //        {
        //            if (LogActionVerification) Log.Trace($"servo:{index}" +
        //                $" - velocity:{servo.Velocity,8:0.000} position:{servo.Position,7:0.000}" +
        //                $" - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
        //            Thread.Sleep(1);
        //            msSleep++;
        //        }
        //        while (servo.Position != targetPosition);
        //        // NOTE(crhodes)
        //        // Stopped does not mean we got there.
        //        //while (! servo.Stopped ) ;

        //        if (LogActionVerification)
        //        {
        //            Log.Trace($"Exit servo:{index} servoPosition:{servo.Position,7:0.000} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
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
