﻿using System;
using System.ComponentModel;
using System.Configuration;
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
    public class DigitalInputEx : Phidgets.DigitalInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new DigitalInput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="digitalInputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public DigitalInputEx(int serialNumber, DigitalInputConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<DigitalOutputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures DigitalInput using DigitalInputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(DigitalInputConfiguration configuration)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.DeviceInitalize) startTicks = Log.DEVICE_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            HostComputer = configuration.HostComputer;
            DeviceSerialNumber = SerialNumber;
            Channel = configuration.Channel;

            IsRemote = true;

            Attach += DigitalInputEx_Attach;
            Detach += DigitalInputEx_Detach;
            Error += DigitalInputEx_Error;
            PropertyChange += DigitalInputEx_PropertyChange;

            StateChange += DigitalInputEx_StateChange;

            if (Core.Common.VNCLogging.DeviceInitalize) Log.DEVICE_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
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

        bool _logStateChangeEvents;
        public bool LogStateChangeEvents 
        {
            get { return _logStateChangeEvents; } 
            set { _logStateChangeEvents = value; OnPropertyChanged(); }
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

        private Phidgets.InputMode _inputMode;
        public new Phidgets.InputMode InputMode
        {
            get => _inputMode;
            set
            {
                if (_inputMode == value)
                    return;
                _inputMode = value;

                if (Attached)
                {
                    base.InputMode = value;
                }

                OnPropertyChanged();
            }
        }

        private Phidgets.PowerSupply _powerSupply;
        public new Phidgets.PowerSupply PowerSupply
        {
            get => _powerSupply;
            set
            {
                if (_powerSupply == value)
                    return;
                _powerSupply = value;

                if (Attached)
                {
                    base.PowerSupply = value;
                }

                OnPropertyChanged();
            }
        }

        private bool _state;
        public new bool State
        {
            get => _state;
            set
            {
                //if (_state == value)
                //    return;
                _state = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void DigitalInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.DigitalInput digitalInput = sender as Phidgets.DigitalInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //Attached = digitalInput.Attached;

            // Just set it so UI behaves well
            Attached = true;

            try
            {
                State = digitalInput.State;
            }
            catch (Phidgets.PhidgetException pex)
            {
                if (pex.ErrorCode != Phidgets.ErrorCode.Unsupported)
                {
                    throw pex;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // Not all DigitalInput support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
            //    InputMode = digitalInput.InputMode;
            //    PowerSupply = digitalInput.PowerSupply;
            //}
            //catch (Phidgets.PhidgetException ex)
            //{
            //    if (ex.ErrorCode != Phidgets.ErrorCode.Unsupported)
            //    {
            //        throw ex;
            //    }
            //}

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Exit DigitalInputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void DigitalInputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"DigitalOutputEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void DigitalInputEx_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        {
            if (LogStateChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_StateChange: sender:{sender} {e.State}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            State = e.State;
        }

        private void DigitalInputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void DigitalInputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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

        public async Task RunActionLoops(DigitalInputSequence digtialInputSequence)
        {
            try
            {
                long startTicks = 0;

                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" name:>{digtialInputSequence.Name}<" +
                        $" startActionLoopSequences:>{digtialInputSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{digtialInputSequence.ActionLoops}<" +
                        $" actions:>{digtialInputSequence.Actions.Count()}<" +
                        $" actionsDuration:>{digtialInputSequence?.ActionsDuration}<" +
                        $" endActionLoopSequences:>{digtialInputSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (digtialInputSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < digtialInputSequence.ActionLoops; actionLoop++)
                    {
                        if (digtialInputSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            DeviceChannelSequencePlayer player = DeviceChannelSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in digtialInputSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (digtialInputSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(digtialInputSequence.Actions, async action =>
                            {
                                // TODO(crhodes)
                                // Decide if want to close everything or pass in config to only open what we need
                                //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (DigitalInputAction action in digtialInputSequence.Actions)
                            {
                                // FIX(crhodes)
                                // 
                                //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                            }
                        }

                        if (digtialInputSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{digtialInputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((int)digtialInputSequence.ActionsDuration);
                        }

                        if (digtialInputSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in digtialInputSequence.EndActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }
                    }
                }

                if (LogChannelAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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

        private async Task PerformAction(DigitalInputAction action)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter digitalInputAction:{Channel}", Common.LOG_CATEGORY);
                actionMessage.Append($"digitalInputAction:{Channel}");
            }

            try
            {
                // NOTE(crhodes)
                // First make any logging changes

                #region Logging

                if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean)action.LogPhidgetEvents;
                if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean)action.LogErrorEvents;
                if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean)action.LogPropertyChangeEvents;

                //if (action.LogPositionChangeEvents is not null) LogPositionChangeEvents = (Boolean)action.LogPositionChangeEvents;
                //if (action.LogVelocityChangeEvents is not null) LogVelocityChangeEvents = (Boolean)action.LogVelocityChangeEvents;
                //if (action.LogTargetPositionReachedEvents is not null) LogTargetPositionReachedEvents = (Boolean)action.LogTargetPositionReachedEvents;

                if (action.LogDeviceChannelSequence is not null) LogDeviceChannelSequence = (Boolean)action.LogDeviceChannelSequence;
                if (action.LogChannelAction is not null) LogChannelAction = (Boolean)action.LogChannelAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion

                #region DigitalInput Actions

                // TODO(crhodes)
                // Implement

                #endregion

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
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
