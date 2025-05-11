using System;
using System.ComponentModel;
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
    public class VoltageRatioInputEx : Phidgets.VoltageRatioInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new VoltageRatioInput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageRatioInputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageRatioInputEx(VoltageRatioInputConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter:", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<VoltageRatioInputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures VoltageRatioInput using VoltageRatioInputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(VoltageRatioInputConfiguration configuration)
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

            Attach += VoltageRatioInputEx_Attach;
            Detach += VoltageRatioInputEx_Detach;
            Error += VoltageRatioInputEx_Error;
            PropertyChange += VoltageRatioInputEx_PropertyChange;

            SensorChange += VoltageRatioInputEx_SensorChange;
            VoltageRatioChange += VoltageRatioInputEx_VoltageRatioChange;

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

        Boolean _logSensorChangeEvents;
        public Boolean LogSensorChangeEvents
        {
            get { return _logSensorChangeEvents; }
            set { _logSensorChangeEvents = value; OnPropertyChanged(); }
        }

        Boolean _logVoltageRatioChangeEvents;
        public Boolean LogVoltageRatioChangeEvents
        {
            get { return _logVoltageRatioChangeEvents; }
            set { _logVoltageRatioChangeEvents = value; OnPropertyChanged(); }
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

        #region VoltageRatioInputEx

        // TODO(crhodes)
        // 
        // There are two more properties that are not available on InterfaceKit1018
        // Implement when we get a board that supports them
        // BridgeEnabled
        // BridgeGain

        private VoltageRatioSensorType _sensorType;
        public new VoltageRatioSensorType SensorType
        {
            get => _sensorType;
            set
            {
                if (_sensorType == value)
                    return;
                _sensorType = value;

                if (Attached)
                {
                    base.SensorType = (VoltageRatioSensorType)value;

                    // Update values, SensorType changed

                    SensorUnit_Unit = base.SensorUnit.Unit;
                    //SensorUnit_Name = base.SensorUnit.Name;
                    SensorUnit_Symbol = base.SensorUnit.Symbol;
                }

                OnPropertyChanged();
            }
        }

        private Unit _sensorUnit_Unit;
        public Unit SensorUnit_Unit
        {
            get => _sensorUnit_Unit;
            set
            {
                _sensorUnit_Unit = value;
                OnPropertyChanged();
            }
        }

        //private string _sensorUnit_Name;
        //public string SensorUnit_Name
        //{
        //    get => _sensorUnit_Name;
        //    set
        //    {
        //        _sensorUnit_Name = value;
        //        OnPropertyChanged();
        //    }
        //}

        private string _sensorUnit_Symbol;
        public string SensorUnit_Symbol
        {
            get => _sensorUnit_Symbol;
            set
            {
                _sensorUnit_Symbol = value;
                OnPropertyChanged();
            }
        }

        private Double _minVoltageRatio;
        public new Double MinVoltageRatio
        {
            get => _minVoltageRatio;
            set
            {
                //if (_minVoltageRatio == value)
                //    return;
                _minVoltageRatio = value;
                OnPropertyChanged();
            }
        }

        private Double _VoltageRatio;
        public new Double VoltageRatio
        {
            get => _VoltageRatio;
            set
            {
                if (_VoltageRatio == value)
                    return;
                _VoltageRatio = value;
                OnPropertyChanged();
            }
        }

        private Double _maxVoltageRatio;
        public new Double MaxVoltageRatio
        {
            get => _maxVoltageRatio;
            set
            {
                //if (_maxVoltageRatio == value)
                //    return;
                _maxVoltageRatio = value;
                OnPropertyChanged();
            }
        }

        private Double _minVoltageRatioChangeTrigger;
        public new Double MinVoltageRatioChangeTrigger
        {
            get => _minVoltageRatioChangeTrigger;
            set
            {
                //if (_minVoltageRatioChangeTrigger == value)
                //    return;
                _minVoltageRatioChangeTrigger = value;
                OnPropertyChanged();
            }
        }

        private Double _voltageRatioChangeTrigger;
        public new Double VoltageRatioChangeTrigger
        {
            get => _voltageRatioChangeTrigger;
            set
            {
                if (_voltageRatioChangeTrigger == value)
                    return;
                _voltageRatioChangeTrigger = value;

                if (Attached)
                {
                    base.VoltageRatioChangeTrigger = (Double)value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxVoltageRatioChangeTrigger;
        public new Double MaxVoltageRatioChangeTrigger
        {
            get => _maxVoltageRatioChangeTrigger;
            set
            {
                //if (_maxVoltageRatioChangeTrigger == value)
                //    return;
                _maxVoltageRatioChangeTrigger = value;
                OnPropertyChanged();
            }
        }

        private Double _sensorValue;
        public new Double SensorValue
        {
            get => _sensorValue;
            set
            {
                if (_sensorValue == value)
                    return;
                _sensorValue = value;
                OnPropertyChanged();
            }
        }

        private Double _sensorValueChangeTrigger;
        public new Double SensorValueChangeTrigger
        {
            get => _sensorValueChangeTrigger;
            set
            {
                if (_sensorValueChangeTrigger == value)
                    return;
                _sensorValueChangeTrigger = value;

                if (Attached)
                {
                    base.SensorValueChangeTrigger = (Double)value;
                }

                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // Flag OutOfRange Errors
        // Detected in Error Event
        // Cleared in SensorValueChanged Event

        private Boolean _sensorValueOutOfRange;
        public Boolean SensorValueOutOfRange
        {
            get => _sensorValueOutOfRange;
            set
            {
                if (_sensorValueOutOfRange == value)
                    return;
                _sensorValueOutOfRange = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        private void VoltageRatioInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageRatioInput voltageRatioInput = sender as Phidgets.VoltageRatioInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_Attach: sender:{sender} isAttached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
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

                Log.Error(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Exit VoltageRatioInputEx_Attach: sender:{sender} isAttached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void VoltageRatioInputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void VoltageRatioInputEx_SensorChange(object sender, PhidgetsEvents.VoltageRatioInputSensorChangeEventArgs e)
        {
            if (LogSensorChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_SensorChange: sender:{sender} {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            SensorValue = e.SensorValue;

            if (SensorValueOutOfRange) SensorValueOutOfRange = false;
        }

        private void VoltageRatioInputEx_VoltageRatioChange(object sender, PhidgetsEvents.VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            if (LogVoltageRatioChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_VoltageChange: sender:{sender} {e.VoltageRatio}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            VoltageRatio = e.VoltageRatio;
        }

        private void VoltageRatioInputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void VoltageRatioInputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            switch (e.Code)
            {
                case Phidgets.ErrorEventCode.OutOfRange:
                    SensorValueOutOfRange = true;
                    break;

                default:
                    break;
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

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached} timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Thread.Sleep(timeout);
            RefreshProperties();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);
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
                // Set properties to values from Phidget
                // We do not use Attach as some Phidgets do not provide values until Open

                SensorType = base.SensorType;

                MinDataInterval = base.MinDataInterval;
                DataInterval = base.DataInterval;
                //DataInterval = 100; // 100ms (10Hz)
                MaxDataInterval = base.MaxDataInterval;

                MinDataRate = base.MinDataRate;
                DataRate = base.DataRate;
                //DataRate = 10; // 10 Hz (100ms)
                MaxDataRate = base.MaxDataRate;

                MinVoltageRatio = base.MinVoltageRatio;
                VoltageRatio = base.VoltageRatio;
                //SensorValue = base.SensorValue;
                MaxVoltageRatio = base.MaxVoltageRatio;

                MinVoltageRatioChangeTrigger = base.MinVoltageRatioChangeTrigger;
                VoltageRatioChangeTrigger = base.VoltageRatioChangeTrigger;
                MaxVoltageRatioChangeTrigger = base.MaxVoltageRatioChangeTrigger;

                SensorValueChangeTrigger = base.SensorValueChangeTrigger;
                SensorValue = base.SensorValue;
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
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(VoltageRatioInputSequence voltageRatioInputSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"RunActionLoops(>{voltageRatioInputSequence.Name}<)" +
                        $" startActionLoopSequences:>{voltageRatioInputSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{voltageRatioInputSequence.ActionLoops}<" +
                        $" serialNumber:>{DeviceSerialNumber}<" +
                        $" hubPort:>{HubPort}< >{voltageRatioInputSequence.HubPort}<" +
                        $" channel:>{Channel}< >{voltageRatioInputSequence.Channel}<" +
                        $" actions:>{voltageRatioInputSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{voltageRatioInputSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{voltageRatioInputSequence.EndActionLoopSequences?.Count()}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (voltageRatioInputSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < voltageRatioInputSequence.ActionLoops; actionLoop++)
                    {
                        if (voltageRatioInputSequence.StartActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in voltageRatioInputSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (voltageRatioInputSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{voltageRatioInputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(voltageRatioInputSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{voltageRatioInputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            foreach (VoltageRatioInputAction action in voltageRatioInputSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (voltageRatioInputSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{voltageRatioInputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)voltageRatioInputSequence.ActionsDuration);
                        }

                        if (voltageRatioInputSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in voltageRatioInputSequence.EndActionLoopSequences)
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

            if (LogChannelAction) Log.Trace($"Exit" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);
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

        private async Task PerformAction(VoltageRatioInputAction action)
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

                #region VoltageRatioInput Actions

                // TODO(crhodes)
                // Implement

                #endregion

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"deviceSerialNumber:{DeviceSerialNumber}" +
                     $" hubPort:{HubPort} channel:{Channel}" +
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

            var sequence = args.VoltageRatioInputSequence;

            await RunActionLoops(sequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            long startTicks = 0;
#if LOGGING
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
