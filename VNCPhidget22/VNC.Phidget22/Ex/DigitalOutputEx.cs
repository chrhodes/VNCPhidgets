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

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures DigitalOutput using DigitalOutputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget(DigitalOutputConfiguration configuration)
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

            Attach += DigitalOutputEx_Attach;
            Detach += DigitalOutputEx_Detach;
            Error += DigitalOutputEx_Error;
            PropertyChange += DigitalOutputEx_PropertyChange;

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

            try
            {
                // TODO(crhodes)
                // Put things here that need to be initialized
                // Use constructor configuration is need to pass things in
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
                    Log.EVENT_HANDLER($"Exit DigitalOutputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}" +
                $" s#:{DeviceSerialNumber} hubport:{HubPort} channel:{Channel}", Common.LOG_CATEGORY);
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

        public new void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isAttached:{Attached} isOpen:{IsOpen}", Common.LOG_CATEGORY);

            try
            {
                // TODO(crhodes)
                // Move stuff out of Attach unless absolutely need to be set
                // as some Phidgets do not provide values until Open

                MinDutyCycle = base.MinDutyCycle;
                DutyCycle = base.DutyCycle;
                MaxDutyCycle = base.MaxDutyCycle;

                State = base.State;
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
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY);

            base.Close();

            Attached = base.Attached;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen} attached:{base.Attached}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(DigitalOutputSequence digitalOutputSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogChannelAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" digitalOutputSequence:>{digitalOutputSequence.Name}<" +
                        $" startActionLoopSequences:>{digitalOutputSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{digitalOutputSequence.ActionLoops}<" +
                        $" actions:>{digitalOutputSequence.Actions.Count()}<" +
                        $" actionsDuration:>{digitalOutputSequence?.ActionsDuration}<" +
                        $" endActionLoopSequences:>{digitalOutputSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (digitalOutputSequence.Actions is not null)
                {
                    for (Int32 actionLoop = 0; actionLoop < digitalOutputSequence.ActionLoops; actionLoop++)
                    {
                        if (digitalOutputSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            DeviceChannelSequencePlayer player = DeviceChannelSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.StartActionLoopSequences)
                            {
                                await player.ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (digitalOutputSequence.ExecuteActionsInParallel)
                        {
                            if (LogChannelAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(digitalOutputSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogChannelAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (DigitalOutputAction action in digitalOutputSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (digitalOutputSequence.ActionsDuration is not null)
                        {
                            if (LogChannelAction)
                            {
                                Log.Trace($"Zzzzz Action:>{digitalOutputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }

                            Thread.Sleep((Int32)digitalOutputSequence.ActionsDuration);
                        }

                        if (digitalOutputSequence.EndActionLoopSequences is not null)
                        {
                            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(_eventAggregator);
                            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
                            player.LogChannelAction = LogChannelAction;

                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.EndActionLoopSequences)
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

        // FIX(crhodes)
        // 
        private async Task PerformAction(DigitalOutputAction action)
        {
            Int64 startTicks = 0;

            StringBuilder actionMessage = new StringBuilder();

            if (LogChannelAction)
            {
                startTicks = Log.Trace($"Enter digitalOutput:{Channel}", Common.LOG_CATEGORY);
                actionMessage.Append($"digitalOutput:{Channel}");
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
               
                if (action.DigitalOut is not null)
                {
                    if (LogChannelAction) actionMessage.Append($" digitalOut:{action.DigitalOut}");

                    State = (Boolean)action.DigitalOut;
                }

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
