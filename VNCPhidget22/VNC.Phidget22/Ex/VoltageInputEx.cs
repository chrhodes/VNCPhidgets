using System;
using System.ComponentModel;
using System.Configuration;
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

using VNCPhidgetConfig = VNC.Phidget22.Configuration;

namespace VNC.Phidget22.Ex
{
    public class VoltageInputEx : Phidgets.VoltageInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new VoltageInput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageInputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageInputEx(VoltageInputConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter:", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<VoltageInputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures VoltageInput using VoltageInputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(VoltageInputConfiguration configuration)
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

            Attach += VoltageInputEx_Attach;
            Detach += VoltageInputEx_Detach;
            Error += VoltageInputEx_Error;
            PropertyChange += VoltageInputEx_PropertyChange;

            SensorChange += VoltageInputEx_SensorChange;
            VoltageChange += VoltageInputEx_VoltageChange;

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

        Boolean _logSensorChangeEvents;
        public Boolean LogSensorChangeEvents
        {
            get { return _logSensorChangeEvents; }
            set { _logSensorChangeEvents = value; OnPropertyChanged(); }
        }

        Boolean _logVoltageChangeEvents;
        public Boolean LogVoltageChangeEvents
        {
            get { return _logVoltageChangeEvents; }
            set { _logVoltageChangeEvents = value; OnPropertyChanged(); }
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

        #region VoltageInputEx

        private VoltageSensorType _sensorType;
        public new VoltageSensorType SensorType
        {
            get => _sensorType;
            set
            {
                if (_sensorType == value)
                    return;
                _sensorType = value;

                if (Attached)
                {
                    base.SensorType = value;

                    // Update values, SensorType changed

                    SensorUnit_Unit = base.SensorUnit.Unit;
                    SensorUnit_Name = base.SensorUnit.Name;
                    SensorUnit_Symbol = base.SensorUnit.Symbol;
                }

                OnPropertyChanged();
            }
        }

        private Unit? _sensorUnit_Unit;
        public Unit? SensorUnit_Unit
        {
            get => _sensorUnit_Unit;
            set
            {
                _sensorUnit_Unit = value;
                OnPropertyChanged();
            }
        }

        private string? _sensorUnit_Name;
        public string? SensorUnit_Name
        {
            get => _sensorUnit_Name;
            set
            {
                _sensorUnit_Name = value;
                OnPropertyChanged();
            }
        }

        private string? _sensorUnit_Symbol;
        public string? SensorUnit_Symbol
        {
            get => _sensorUnit_Symbol;
            set
            {
                _sensorUnit_Symbol = value;
                OnPropertyChanged();
            }
        }

        private Double _minVoltage;
        public new Double MinVoltage
        {
            get => _minVoltage;
            set
            {
                //if (_minVoltage == value)
                //    return;
                _minVoltage = value;
                OnPropertyChanged();
            }
        }

        private Double _Voltage;
        public new Double Voltage
        {
            get => _Voltage;
            set
            {
                if (_Voltage == value)
                    return;
                _Voltage = value;
                OnPropertyChanged();
            }
        }

        private Double _maxVoltage;
        public new Double MaxVoltage
        {
            get => _maxVoltage;
            set
            {
            //    if (_maxVoltage == value)
            //        return;
                _maxVoltage = value;
                OnPropertyChanged();
            }
        }

        private Double _minVoltageChangeTrigger;
        public new Double MinVoltageChangeTrigger
        {
            get => _minVoltageChangeTrigger;
            set
            {
                //if (_minVoltageChangeTrigger == value)
                //    return;
                _minVoltageChangeTrigger = value;
                OnPropertyChanged();
            }
        }

        private Double _voltageChangeTrigger;
        public new Double VoltageChangeTrigger
        {
            get => _voltageChangeTrigger;
            set
            {
                if (_voltageChangeTrigger == value)
                    return;
                _voltageChangeTrigger = value;

                if (Attached)
                {
                    base.VoltageChangeTrigger = (Double)value;
                }

                OnPropertyChanged();
            }
        }

        private Double _maxVoltageChangeTrigger;
        public new Double MaxVoltageChangeTrigger
        {
            get => _maxVoltageChangeTrigger;
            set
            {
                //if (_maxVoltageChangeTrigger == value)
                //    return;
                _maxVoltageChangeTrigger = value;
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

        private VoltageRange _voltageRange;
        public new VoltageRange VoltageRange
        {
            get => _voltageRange;
            set
            {
                if (_voltageRange == value)
                    return;
                _voltageRange = value;

                if (Attached)
                {
                    base.VoltageRange = value;
                }

                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // Flag OutOfRange Errors if in Sensor mode
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

        private PowerSupply _powerSupply;
        public new PowerSupply PowerSupply
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

        #endregion

        #endregion

        #region Event Handlers

        private void VoltageInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageInput voltageInput =  (Phidgets.VoltageInput)sender;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"Exit VoltageInputEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void VoltageInputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"VoltageInputEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void VoltageInputEx_SensorChange(object sender, PhidgetsEvents.VoltageInputSensorChangeEventArgs e)
        {
            if (LogSensorChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_SensorChange: sender:{sender} {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            SensorValue = e.SensorValue;
        }

        private void VoltageInputEx_VoltageChange(object sender, PhidgetsEvents.VoltageInputVoltageChangeEventArgs e)
        {
            if (LogVoltageChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_VoltageChange: sender:{sender} {e.Voltage}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            Voltage = e.Voltage;
        }

        private void VoltageInputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
        }

        private void VoltageInputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen}  timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            base.Open(timeout);

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
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
                // We do not use Attach Event as some Phidgets do not provide values until Open

                SensorValueChangeTrigger = base.SensorValueChangeTrigger;

                MinDataInterval = base.MinDataInterval;
                DataInterval = base.DataInterval;
                //DataInterval = 100; // 100ms (10Hz)
                MaxDataInterval = base.MaxDataInterval;

                MinDataRate = base.MinDataRate;
                DataRate = base.DataRate;
                //DataRate = 10; // 10 Hz (100ms)
                MaxDataRate = base.MaxDataRate;

                MinVoltage = base.MinVoltage;
                Voltage = base.Voltage;
                SensorValue = base.SensorValue;
                MaxVoltage = base.MaxVoltage;

                MinVoltageChangeTrigger = base.MinVoltageChangeTrigger;
                VoltageChangeTrigger = base.VoltageChangeTrigger;
                MaxVoltageChangeTrigger = base.MaxVoltageChangeTrigger;
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.ERROR(pex, Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }

            if (LogPhidgetEvents) Log.TRACE($"Exit", Common.LOG_CATEGORY, startTicks);
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

        public async Task RunActionLoops(VoltageInputSequence voltageInputSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.TRACE(
                        $"RunActionLoops(>{voltageInputSequence.Name}<)" +
                        $" startActionLoopSequences:>{voltageInputSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{voltageInputSequence.ActionLoops}<" +
                        $" serialNumber:>{DeviceSerialNumber}<" +
                        $" hubPort:>{HubPort}< >{voltageInputSequence.HubPort}<" +
                        $" channel:>{Channel}< >{voltageInputSequence.Channel}<" +
                        $" actions:>{voltageInputSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{voltageInputSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{voltageInputSequence.EndActionLoopSequences?.Count()}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (voltageInputSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < voltageInputSequence.ActionLoops; actionLoop++)
                    {
                        if (voltageInputSequence.StartActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in voltageInputSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (voltageInputSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.TRACE($"Parallel Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{voltageInputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(voltageInputSequence.Actions, action =>
                            {
                                ExecuteAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.TRACE($"Sequential Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{voltageInputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            foreach (VoltageInputAction action in voltageInputSequence.Actions)
                            {
                                ExecuteAction(action);
                            }
                        }

                        if (voltageInputSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.TRACE($"Zzzz End of Actions" +
                                    $" Sleeping:>{voltageInputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)voltageInputSequence.ActionsDuration);
                        }

                        if (voltageInputSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in voltageInputSequence.EndActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }
                    }
                }

                if (LogChannelAction) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
            }
        }

        public void RaisePlayPerformanceEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RaisePlayPerformanceEvent) Enter", Common.LOG_CATEGORY);

            VNCPhidgetConfig.Performance.Performance performance = new VNCPhidgetConfig.Performance.Performance();
            performance.Name = "Sequential Sequences InterfaceKit 124744";

            _eventAggregator.GetEvent<ExecutePerformanceEvent>().Publish(
                new PerformanceEventArgs()
                {
                    Performance = performance
                });

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RaisePlayPerformanceEvent) Exit", Common.LOG_CATEGORY, startTicks);
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

        private void ExecuteAction(VoltageInputAction action)
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

                #region VoltageInput Actions

                // TODO(crhodes)
                // Implement

                #endregion

                if (action.Duration > 0)
                {
                    if (LogChannelAction) actionMessage.Append($"Zzzz - End of Action Sleeping:>{action.Duration}<");

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

            var sequence = args.VoltageInputSequence;

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
