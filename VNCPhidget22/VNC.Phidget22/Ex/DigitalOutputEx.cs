﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Prism.Events;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    public class DigitalOutputEx : Phidgets.DigitalOutput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly DigitalOutputConfiguration _digitalOutputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new DigitalOutput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="digitalOutputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public DigitalOutputEx(int serialNumber, DigitalOutputConfiguration digitalOutputConfiguration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _digitalOutputConfiguration = digitalOutputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<DigitalOutputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures DigitalOutput using DigitalOutputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _digitalOutputConfiguration.Channel;
            IsRemote = true;

            Attach += DigitalOutputEx_Attach;
            Detach += DigitalOutputEx_Detach;
            Error += DigitalOutputEx_Error;
            PropertyChange += DigitalOutputEx_PropertyChange;

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

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

        bool _logPerformanceSequence;
        public bool LogPerformanceSequence
        {
            get { return _logPerformanceSequence; }
            set { _logPerformanceSequence = value; OnPropertyChanged(); }
        }

        bool _logSequenceAction;
        public bool LogSequenceAction
        {
            get { return _logSequenceAction; }
            set { _logSequenceAction = value; OnPropertyChanged(); }
        }

        bool _logActionVerification;
        public bool LogActionVerification
        {
            get { return _logActionVerification; }
            set { _logActionVerification = value; OnPropertyChanged(); }
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

        private Double _minFrequency;
        public new Double MinFrequency
        {
            get => _minFrequency;
            set
            {
                if (_minFrequency == value)
                    return;
                _minFrequency = value;
                OnPropertyChanged();
            }
        }

        private Double _frequency;
        public new Double Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency == value)
                    return;
                _frequency = value;

                if (Attached)
                {
                    base.Frequency = (Double)value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxFrequency;
        public new Double MaxFrequency
        {
            get => _maxFrequency;
            set
            {
                if (_maxFrequency == value)
                    return;
                _maxFrequency = value;
                OnPropertyChanged();
            }
        }

        private Phidgets.LEDForwardVoltage _ledForwardVoltage;
        public new Phidgets.LEDForwardVoltage LEDForwardVoltage
        {
            get => _ledForwardVoltage;
            set
            {
                if (_ledForwardVoltage == value)
                    return;
                _ledForwardVoltage = value;

                if (Attached)
                {
                    base.LEDForwardVoltage = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _minDutyCycle;
        public new Double MinDutyCycle
        {
            get => _minDutyCycle;
            set
            {
                if (_minDutyCycle == value)
                    return;
                _minDutyCycle = value;
                OnPropertyChanged();
            }
        }

        private Double _dutyCycle;
        public new Double DutyCycle
        {
            get => _dutyCycle;
            set
            {
                if (_dutyCycle == value)
                    return;
                _dutyCycle = value;

                if (Attached)
                {
                    base.DutyCycle = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxDutyCycle;
        public new Double MaxDutyCycle
        {
            get => _maxDutyCycle;
            set
            {
                if (_maxDutyCycle == value)
                    return;
                _maxDutyCycle = value;
                OnPropertyChanged();
            }
        }

        private int _minFailsafeTime;
        public new int MinFailsafeTime
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

        private int _maxFailsafeTime;
        public new int MaxFailsafeTime
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

        private Double _minLEDCurrentLimit;
        public new Double MinLEDCurrentLimit
        {
            get => _minLEDCurrentLimit;
            set
            {
                if (_minLEDCurrentLimit == value)
                    return;
                _minLEDCurrentLimit = value;
                OnPropertyChanged();
            }
        }

        private Double _ledCurrentLimit;
        public new Double LEDCurrentLimit
        {
            get => _ledCurrentLimit;
            set
            {
                if (_ledCurrentLimit == value)
                    return;
                _ledCurrentLimit = value;

                if (Attached)
                {
                    base.LEDCurrentLimit = value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxLEDCurrentLimit;
        public new Double MaxLEDCurrentLimit
        {
            get => _maxLEDCurrentLimit;
            set
            {
                if (_maxLEDCurrentLimit == value)
                    return;
                _maxLEDCurrentLimit = value;
                OnPropertyChanged();
            }
        }

        private bool _state;
        public new bool State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                _state = value;
                if (Attached)
                {
                    base.State = (bool)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void DigitalOutputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.DigitalOutput digitalOutput = sender as Phidgets.DigitalOutput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalOutputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
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

            MinDutyCycle = digitalOutput.MinDutyCycle;
            DutyCycle = digitalOutput.DutyCycle;
            MaxDutyCycle = digitalOutput.MaxDutyCycle;

            State = digitalOutput.State;

            // Not all DigitalOutput support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
            //    MinFrequency = dOutput.MinFrequency;
            //    Frequency = dOutput.Frequency;
            //    MaxFrequency = dOutput.MaxFrequency;

            //    LEDForwardVoltage = dOutput.LEDForwardVoltage;

            //    MinLEDCurrentLimit = dOutput.MinLEDCurrentLimit;
            //    LEDCurrentLimit = dOutput.LEDCurrentLimit;
            //    MaxLEDCurrentLimit = dOutput.MaxLEDCurrentLimit;

            //    MinFailsafeTime = dOutput.MinFailsafeTime;
            //    MaxFailsafeTime = dOutput.MaxFailsafeTime;
            //}
            //catch (Phidgets.PhidgetException ex)
            //{
            //    if (ex.ErrorCode != Phidgets.ErrorCode.Unsupported)
            //    {
            //        throw ex;
            //    }
            //}
        }

        private void DigitalOutputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalOutputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void DigitalOutputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalOutputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            IsAttached = false;
        }

        private void DigitalOutputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Open();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Open(timeout);

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Close();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }
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

        public async Task RunActionLoops(InterfaceKitSequence interfaceKitSequence)
        {
            try
            {
                //        Int64 startTicks = 0;

                //        if (LogSequenceAction)
                //        {
                //            startTicks = Log.Trace(
                //                $"Running Action Loops" +
                //                $" interfaceKitSequence:>{interfaceKitSequence.Name}<" +
                //                $" startActionLoopSequences:>{interfaceKitSequence.StartActionLoopSequences?.Count()}<" +
                //                $" actionLoops:>{interfaceKitSequence.ActionLoops}<" +
                //                $" actions:>{interfaceKitSequence.Actions.Count()}<" +
                //                $" actionsDuration:>{interfaceKitSequence?.ActionsDuration}<" +
                //                $" endActionLoopSequences:>{interfaceKitSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                //        }

                //        if (interfaceKitSequence.Actions is not null)
                //        {
                //            for (int actionLoop = 0; actionLoop < interfaceKitSequence.ActionLoops; actionLoop++)
                //            {
                //                if (interfaceKitSequence.StartActionLoopSequences is not null)
                //                {
                //                    // TODO(crhodes)
                //                    // May want to create a new player instead of reaching for the property.

                //                    PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                //                    player.LogPerformanceSequence = LogPerformanceSequence;
                //                    player.LogSequenceAction = LogSequenceAction;

                //                    foreach (PerformanceSequence sequence in interfaceKitSequence.StartActionLoopSequences)
                //                    {
                //                        await player.ExecutePerformanceSequence(sequence);
                //                    }
                //                }

                //                if (interfaceKitSequence.ExecuteActionsInParallel)
                //                {
                //                    if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                //                    Parallel.ForEach(interfaceKitSequence.Actions, async action =>
                //                    {
                //                        // TODO(crhodes)
                //                        // Decide if want to close everything or pass in config to only open what we need
                //                        //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                //                    });
                //                }
                //                else
                //                {
                //                    if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                //                    foreach (InterfaceKitAction action in interfaceKitSequence.Actions)
                //                    {
                //                        // FIX(crhodes)
                //                        // 
                //                        //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                //                    }
                //                }

                //                if (interfaceKitSequence.ActionsDuration is not null)
                //                {
                //                    if (LogSequenceAction)
                //                    {
                //                        Log.Trace($"Zzzzz Action:>{interfaceKitSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                //                    }
                //                    Thread.Sleep((Int32)interfaceKitSequence.ActionsDuration);
                //                }

                //                if (interfaceKitSequence.EndActionLoopSequences is not null)
                //                {
                //                    PerformanceSequencePlayer player = new PerformanceSequencePlayer(_eventAggregator);
                //                    player.LogPerformanceSequence = LogPerformanceSequence;
                //                    player.LogSequenceAction = LogSequenceAction;

                //                    foreach (PerformanceSequence sequence in interfaceKitSequence.EndActionLoopSequences)
                //                    {
                //                        await player.ExecutePerformanceSequence(sequence);
                //                    }
                //                }
                //            }
                //        }

                //        if (LogSequenceAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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
        // 
        //private async Task PerformAction(InterfaceKitDigitalOutputCollection ifkDigitalOutputs, InterfaceKitAction action, Int32 index)
        //{
        //    Int64 startTicks = 0;

        //    StringBuilder actionMessage = new StringBuilder();

        //    if (LogSequenceAction)
        //    {
        //        startTicks = Log.Trace($"Enter index:{index}", Common.LOG_CATEGORY);
        //        actionMessage.Append($"index:{index}");
        //    }

        //    try
        //    {
        // NOTE(crhodes)
        // First make any logging changes

        //        #region Logging

        //        if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean) action.LogPhidgetEvents;
        //        if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean) action.LogErrorEvents;
        //        if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean) action.LogPropertyChangeEvents;

        //        if (action.LogPositionChangeEvents is not null) LogPositionChangeEvents = (Boolean) action.LogPositionChangeEvents;
        //        if (action.LogVelocityChangeEvents is not null) LogVelocityChangeEvents = (Boolean) action.LogVelocityChangeEvents;
        //        if (action.LogTargetPositionReachedEvents is not null) LogTargetPositionReachedEvents = (Boolean) action.LogTargetPositionReachedEvents;

        //        if (action.LogPerformanceSequence is not null) LogPerformanceSequence = (Boolean) action.LogPerformanceSequence;
        //        if (action.LogSequenceAction is not null) LogSequenceAction = (Boolean) action.LogSequenceAction;
        //        if (action.LogActionVerification is not null) LogActionVerification = (Boolean) action.LogActionVerification;

        //#endregion
        //        if (action.DigitalOut is not null)
        //        { 
        //            if (LogSequenceAction) actionMessage.Append($" digitalOut:{action.DigitalOut}");

        //            ifkDigitalOutputs[index] = (Boolean)action.DigitalOut; 
        //        }

        //        if (action.Duration > 0)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

        //            Thread.Sleep((Int32)action.Duration);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //    finally
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
        //        }
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
