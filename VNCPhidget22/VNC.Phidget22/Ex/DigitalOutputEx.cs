using System;
using System.ComponentModel;
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

        private int _minFailsafeTime;
        public new int MinFailsafeTime
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

        private int _maxFailsafeTime;
        public new int MaxFailsafeTime
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

            //Attached = dOutput.Attached;

            // Just set it so UI behaves well
            Attached = true;
            Attached = true;

            try
            {
                MinDutyCycle = digitalOutput.MinDutyCycle;
                DutyCycle = digitalOutput.DutyCycle;
                MaxDutyCycle = digitalOutput.MaxDutyCycle;

                State = digitalOutput.State;
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

        public async Task RunActionLoops(DigitalOutputSequence digitalOutputSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogPerformanceSequence)
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
                    for (int actionLoop = 0; actionLoop < digitalOutputSequence.ActionLoops; actionLoop++)
                    {
                        if (digitalOutputSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in digitalOutputSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (digitalOutputSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(digitalOutputSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (DigitalOutputAction action in digitalOutputSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (digitalOutputSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{digitalOutputSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }

                            Thread.Sleep((Int32)digitalOutputSequence.ActionsDuration);
                        }

                        if (digitalOutputSequence.EndActionLoopSequences is not null)
                        {
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(_eventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in digitalOutputSequence.EndActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }
                    }
                }

                if (LogSequenceAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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

            if (LogSequenceAction)
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

                if (action.LogPerformanceSequence is not null) LogPerformanceSequence = (Boolean)action.LogPerformanceSequence;
                if (action.LogSequenceAction is not null) LogSequenceAction = (Boolean)action.LogSequenceAction;
                if (action.LogActionVerification is not null) LogActionVerification = (Boolean)action.LogActionVerification;

                #endregion
               
                if (action.DigitalOut is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" digitalOut:{action.DigitalOut}");

                    State = (Boolean)action.DigitalOut;
                }

                if (action.Duration > 0)
                {
                    if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
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
