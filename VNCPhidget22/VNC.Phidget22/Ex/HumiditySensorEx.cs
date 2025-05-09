using System;
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

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures HumiditySensorEx using HumiditySensorConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(HumiditySensorConfiguration configuration)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.DeviceInitalize) startTicks = Log.DEVICE_INITIALIZE($"Enter" +
                $"s#:{configuration.DeviceSerialNumber} hp:{configuration.HubPort} c:{configuration.Channel}", Common.LOG_CATEGORY);

            DeviceSerialNumber = configuration.DeviceSerialNumber;
            IsHubPortDevice = configuration.HubPortDevice;
            HubPort = configuration.HubPort;
            Channel = configuration.Channel;

            SerialHubPortChannel = new SerialHubPortChannel
            {
                SerialNumber = DeviceSerialNumber,
                HubPort = HubPort,
                Channel = Channel
            };

            IsRemote = true;

            Attach += HumiditySensorExEx_Attach;
            Detach += HumiditySensorExEx_Detach;
            Error += HumiditySensorExEx_Error;
            PropertyChange += HumiditySensorExEx_PropertyChange;

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
                //if (_serialHubPortChannel.Equals(value)) return;
                //if (_serialHubPortChannel == value)
                //    return;
                _serialHubPortChannel = value;
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

        #region Data Interval and Rate

        private Int32 _minDataInterval;
        public new Int32 MinDataInterval
        {
            get => _minDataInterval;
            set
            {
                //if (_minDataInterval == value)
                //    return;
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
                //if (_maxDataInterval == value)
                //    return;
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
                //if (_minDataRate == value)
                //    return;
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
                //if (_maxDataRate == value)
                //    return;
                _maxDataRate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region HumiditySensorEx




        #endregion

        #endregion

        #region Event Handlers

        private void HumiditySensorExEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.HumiditySensor HumiditySensor = sender as Phidgets.HumiditySensor;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorExEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
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
                if (pex.ErrorCode != Phidgets.ErrorCode.Unsupported)
                {
                    throw pex;
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
                    Log.EVENT_HANDLER($"Exit HumiditySensorExEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void HumiditySensorExEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorExEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"HumiditySensorExEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void HumiditySensorExEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"HumiditySensorExEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void HumiditySensorExEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
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
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open();

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached} timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Gather properties from Open Phidget Device
        /// </summary>
        public new void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            try
            {
                // TODO(crhodes)
                // Move stuff out of Attach unless absolutely need to be set
                // as some Phidgets do not provide values until Open
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

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(HumiditySensorSequence humiditySensorSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" name:>{humiditySensorSequence.Name}<" +
                        $" startActionLoopSequences:>{humiditySensorSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{humiditySensorSequence.ActionLoops}<" +
                        $" actions:>{humiditySensorSequence.Actions.Count()}<" +
                        $" actionsDuration:>{humiditySensorSequence?.ActionsDuration}<" +
                        $" endActionLoopSequences:>{humiditySensorSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (humiditySensorSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < humiditySensorSequence.ActionLoops; actionLoop++)
                    {
                        if (humiditySensorSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in humiditySensorSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (humiditySensorSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(humiditySensorSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (HumiditySensorAction action in humiditySensorSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (humiditySensorSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{humiditySensorSequence.ActionsDuration}<", Common.LOG_CATEGORY);
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

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        // FIX(crhodes)
        // 
        private async Task PerformAction(HumiditySensorAction action)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter HumiditySensorEx:{Channel}", Common.LOG_CATEGORY);
                actionMessage.Append($"HumiditySensorEx:{Channel}");
            }

            try
            {
                 // NOTE(crhodes)
                 // First make any logging changes

                #region Logging

                if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean)action.LogPhidgetEvents;
                if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean)action.LogErrorEvents;
                if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean)action.LogPropertyChangeEvents;

                if (action.LogDeviceChannelSequence is not null) LogDeviceChannelSequence = (Boolean)action.LogDeviceChannelSequence;
                if (action.LogChannelAction is not null) LogChannelAction = (Boolean)action.LogChannelAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion

                #region HumiditySensorEx Actions

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
