using System;
using System.CodeDom;
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
    public class DigitalOutputEx : Phidgets.DigitalOutput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new DigitalOutput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="digitalOutputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public DigitalOutputEx(DigitalOutputConfiguration configuration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter:", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;

            InitializePhidget(configuration);

            _eventAggregator.GetEvent<DigitalOutputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        /// <summary>
        /// Configures DigitalOutput using DigitalOutputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(DigitalOutputConfiguration configuration)
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

            Attach += DigitalOutputEx_Attach;
            Detach += DigitalOutputEx_Detach;
            Error += DigitalOutputEx_Error;
            PropertyChange += DigitalOutputEx_PropertyChange;

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


        #region DigitalOutputEx

        private Double _minFrequency;
        public new Double MinFrequency
        {
            get => _minFrequency;
            set
            {
                //if (_minFrequency == value)
                //    return;
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
                //if (_maxFrequency == value)
                //    return;
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
                //if (_minDutyCycle == value)
                //    return;
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
                //if (_maxDutyCycle == value)
                //    return;
                _maxDutyCycle = value;
                OnPropertyChanged();
            }
        }

        private Int32 _minFailsafeTime;
        public new Int32 MinFailsafeTime
        {
            get => _minFailsafeTime;
            set
            {
                //if (_minFailsafeTime == value)
                //    return;
                _minFailsafeTime = value;
                OnPropertyChanged();
            }
        }

        private Int32 _maxFailsafeTime;
        public new Int32 MaxFailsafeTime
        {
            get => _maxFailsafeTime;
            set
            {
                //if (_maxFailsafeTime == value)
                //    return;
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
                //if (_minLEDCurrentLimit == value)
                //    return;
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
                //if (_maxLEDCurrentLimit == value)
                //    return;
                _maxLEDCurrentLimit = value;
                OnPropertyChanged();
            }
        }

        private Boolean _state;
        public new Boolean State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                _state = value;

                if (Attached)
                {
                    base.State = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        private void DigitalOutputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.DigitalOutput? digitalOutput = sender as Phidgets.DigitalOutput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalOutputEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"Exit DigitalOutputEx_Attach: sender:{sender} attached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }
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
                    Log.EVENT_HANDLER($"DigitalOutputEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
                    break;
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
                    Log.ERROR(ex, Common.LOG_CATEGORY);
                }
            }

            Attached = false;
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

            // TODO(crhodes)
            // Where to handle not opening if already open.

            base.Open();

            Attached = base.Attached;
            RefreshProperties();

            if (LogPhidgetEvents) Log.TRACE($"Exit attached:{base.Attached} isOpen:{IsOpen}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.TRACE($"Enter attached:{base.Attached} isOpen:{IsOpen}  timeout:{timeout}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Where to handle not opening if already open.

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

                MinDutyCycle = base.MinDutyCycle;
                DutyCycle = base.DutyCycle;
                MaxDutyCycle = base.MaxDutyCycle;

                State = base.State;
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

        public async Task RunActionLoops(DigitalOutputSequence digitalOutputSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogChannelAction)
                {
                    startTicks = Log.TRACE(
                        $"RunActionLoops(>{digitalOutputSequence.Name}<)" +
                        $" startActionLoopSequences:>{digitalOutputSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{digitalOutputSequence.ActionLoops}<" +
                        $" serialNumber:>{DeviceSerialNumber}<" +
                        $" hubPort:>{HubPort}< >{digitalOutputSequence.HubPort}<" +
                        $" channel:>{Channel}< >{digitalOutputSequence.Channel}<" +
                        $" actions:>{digitalOutputSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{digitalOutputSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{digitalOutputSequence.EndActionLoopSequences?.Count()}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (digitalOutputSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < digitalOutputSequence.ActionLoops; actionLoop++)
                    {
                        if (digitalOutputSequence.StartActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (digitalOutputSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.TRACE($"Parallel Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{digitalOutputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(digitalOutputSequence.Actions, action =>
                            {
                                ExecuteAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.TRACE($"Sequential Actions Loop:>{actionLoop + 1}<" +
                                $" actions:{digitalOutputSequence.Actions.Count()}" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            foreach (DigitalOutputAction action in digitalOutputSequence.Actions)
                            {
                                ExecuteAction(action);
                            }
                        }

                        if (digitalOutputSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.TRACE($"Zzzz End of Actions" +
                                    $" Sleeping:>{digitalOutputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }

                            Thread.Sleep((Int32)digitalOutputSequence.ActionsDuration);
                        }

                        if (digitalOutputSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.EndActionLoopSequences)
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

        private void ExecuteAction(DigitalOutputAction action)
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

                    // TODO(crhodes)
                    // Where to handle not opening if already open.

                    Open(Phidget.DefaultTimeout);
                }

                if (action.Close is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" close:>{action.Close}<");

                    Close();
                }

                #region DigitalOutputEx Actions

                if (action.DigitalOut is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" digitalOut:{action.DigitalOut}");

                    State = (Boolean)action.DigitalOut;
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

            var sequence = args.DigitalOutputSequence;

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
