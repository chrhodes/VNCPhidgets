﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidget22;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    public class HumiditySensorEx : Phidgets.HumiditySensor, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new HumiditySensorEx and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="HumiditySensorConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public HumiditySensorEx(HumiditySensorConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter:", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<HumiditySensorSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures HumiditySensorEx using HumiditySensorConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(HumiditySensorConfiguration configuration)
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

            // TODO(crhodes)
            // Add and device specific logging options

            LogDeviceChannelSequence = configuration.LogDeviceChannelSequence;
            LogChannelAction = configuration.LogChannelAction;
            LogActionVerification = configuration.LogActionVerification;

            Attach += HumiditySensorEx_Attach;
            Detach += HumiditySensorEx_Detach;
            Error += HumiditySensorEx_Error;
            PropertyChange += HumiditySensorEx_PropertyChange;

            // TODO(crhodes)
            // Add any device specific events

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

        #region HumiditySensorEx




        #endregion

        #endregion

        #region Event Handlers

        private void HumiditySensorEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.HumiditySensor? HumiditySensor = sender as Phidgets.HumiditySensor;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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
                Log.Error(pex, Common.LOG_CATEGORY);
                if (pex.ErrorCode != Phidgets.ErrorCode.Unsupported)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Exit HumiditySensorEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void HumiditySensorEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"HumiditySensorEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void HumiditySensorEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void HumiditySensorEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
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
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open();

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter attached:{base.Attached} isOpen:{IsOpen}  timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Gather properties from Open Phidget Device
        /// </summary>
        public void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            try
            {
                // Set properties to values from Phidget
                // We do not use Attach as some Phidgets do not provide values until Open
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents) Log.Trace($"Exit attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter attached:{base.Attached} isOpen:{IsOpen} " +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(HumiditySensorSequence humiditySensorSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                         $"RunActionLoops(>{humiditySensorSequence.Name}<)" +
                         $" startActionLoopSequences:>{humiditySensorSequence.StartActionLoopSequences?.Count()}<" +
                         $" actionLoops:>{humiditySensorSequence.ActionLoops}<" +
                         $" serialNumber:>{DeviceSerialNumber}<" +
                         $" hubPort:>{HubPort}< >{humiditySensorSequence.HubPort}<" +
                         $" channel:>{Channel}< >{humiditySensorSequence.Channel}<" +
                         $" actions:>{humiditySensorSequence.Actions?.Count()}<" +
                         $" actionsDuration:>{humiditySensorSequence.ActionsDuration}<" +
                         $" endActionLoopSequences:>{humiditySensorSequence.EndActionLoopSequences?.Count()}<" +
                         $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (humiditySensorSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < humiditySensorSequence.ActionLoops; actionLoop++)
                    {
                        if (humiditySensorSequence.StartActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in humiditySensorSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (humiditySensorSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{humiditySensorSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(humiditySensorSequence.Actions, action =>
                            {
                                ExecuteAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{humiditySensorSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            foreach (HumiditySensorAction action in humiditySensorSequence.Actions)
                            {
                                ExecuteAction(action);
                            }
                        }

                        if (humiditySensorSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzz End of Actions" +
                                    $" Sleeping:>{humiditySensorSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }

                            Thread.Sleep((Int32)humiditySensorSequence.ActionsDuration);
                        }

                        if (humiditySensorSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in humiditySensorSequence.EndActionLoopSequences)
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

        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer()
        {
            Int64 startTicks = 0;
            if (LogDeviceChannelSequence) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);

            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            // TODO(crhodes)
            // Add appropriate events for this device

            player.LogPhidgetEvents = LogPhidgetEvents;

            if (LogDeviceChannelSequence) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);

            return player;
        }

        private void ExecuteAction(HumiditySensorAction action)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter DeviceSerialNumber:{DeviceSerialNumber}" +
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

                // TODO(crhodes)
                // Add Device specific logging options

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

                #region HumiditySensorEx Actions

                // TODO(crhodes)
                // Implement

                #endregion

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" Zzzz - End of Action Sleeping:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                //Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"PhidgetException deviceSerialNumber:>{DeviceSerialNumber}<" +
                    $" hubPort:>{HubPort}< channel:>{Channel}<" +
                    $" {actionMessage}" +
                    $" source:{pex.Source}" +
                    $" description:{pex.Description}" +
                    $" inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogChannelAction)
                {
                    Log.Trace($"Exit deviceSerialNumber:{DeviceSerialNumber}" +
                        $" hubPort:{HubPort} channel:{Channel} {actionMessage}" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        private async void TriggerSequence(SequenceEventArgs args)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var sequence = args.HumiditySensorSequence;

            if (sequence is not null) await RunActionLoops(sequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
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
