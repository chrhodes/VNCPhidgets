using System;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

using Prism.Events;

using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using VNC.Phidget22.Configuration;
using System.Net;

using System.Diagnostics.Metrics;

using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace VNC.Phidget22
{
    public class VoltageOutputEx : Phidgets.VoltageOutput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly VoltageOutputConfiguration _voltageOutputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new DigitalOutput and conf
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageOutputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageOutputEx(int serialNumber, DigitalOutputConfiguration voltageOutputConfiguration, IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _voltageOutputConfiguration = voltageOutputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<DigitalOutputSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _voltageOutputConfiguration.Channel;
            IsRemote = true;

            this.Attach += VoltageOutputEx_Attach;
            this.Detach += VoltageOutputEx_Detach;
            this.Error += VoltageOutputEx_Error;
            this.PropertyChange += VoltageOutputEx_PropertyChange;

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public bool LogPhidgetEvents { get; set; } = true;
        public bool LogErrorEvents { get; set; } = true;

        public bool LogPropertyChangeEvents { get; set; } = true;


        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }

        private int _serialNumber;
        public int SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber == value)
                    return;
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // Create wrapper properties for all Properties (of interest) in Phidgets.DigitalOutput

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

        private double _dutyCycle;
        public new double DutyCycle
        {
            get => _dutyCycle;
            set
            {
                if (_dutyCycle == value)
                    return;
                _dutyCycle = value;

                if (base.Attached)
                {
                    base.DutyCycle = value;
                }

                OnPropertyChanged();
            }
        }

        private double? _frequency;
        public new double? Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency == value)
                    return;
                _frequency = value;

                if (base.Attached)
                {
                    base.Frequency = (double)value;
                }

                OnPropertyChanged();
            }
        }

        private double _ledCurrentLimit;
        public new double LEDCurrentLimit
        {
            get => _ledCurrentLimit;
            set
            {
                if (_ledCurrentLimit == value)
                    return;
                _ledCurrentLimit = value;

                if (base.Attached)
                {
                    base.LEDCurrentLimit = value;
                }

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

                if (base.Attached)
                {
                    base.LEDForwardVoltage = value;
                }

                OnPropertyChanged();
            }
        }

        private double _maxDutyCycle;
        public double MaxDutyCycle
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

        private int _maxFailsafeTime;
        public int MaxFailsafeTime
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

        private double _maxFrequency;
        public double MaxFrequency
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

        private double _maxLEDCurrentLimit;
        public double MaxLEDCurrentLimit
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

        private double _minDutyCycle;
        public double MinDutyCycle
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

        private int _minFailsafeTime;
        public int MinFailsafeTime
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

        private double _minFrequecy;
        public double MinFrequecy
        {
            get => _minFrequecy;
            set
            {
                if (_minFrequecy == value)
                    return;
                _minFrequecy = value;
                OnPropertyChanged();
            }
        }

        private double _minLEDCurrentLimit;

        public double MinLEDCurrentLimit
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

        private bool? _state = null;
        public new bool? State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                _state = value;
                if (base.Attached)
                {
                    base.State = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }        

        #endregion

        #region Event Handlers

        private void VoltageOutputEx_Attach(object sender, AttachEventArgs e)
        {
            Phidgets.DigitalOutput dOutput = sender as Phidgets.DigitalOutput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageOutputEx_Attach: sender:{sender} attached:{dOutput.Attached}", Common.LOG_CATEGORY);
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

            MinDutyCycle = dOutput.MinDutyCycle;
            DutyCycle = dOutput.DutyCycle;
            MaxDutyCycle = dOutput.MaxDutyCycle;
            
            State = dOutput.State;

            // Not all DigitalOutput support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
            //    Frequency = dOutput.Frequency;
            //    LEDCurrentLimit = dOutput.LEDCurrentLimit;
            //    LEDForwardVoltage = dOutput.LEDForwardVoltage;
            //    MaxLEDCurrentLimit = dOutput.MaxLEDCurrentLimit;
            //    MinLEDCurrentLimit = dOutput.MinLEDCurrentLimit;
            //    MaxFailsafeTime = dOutput.MaxFailsafeTime;
            //    MaxFrequency = dOutput.MaxFrequency;
            //    MinFailsafeTime = dOutput.MinFailsafeTime;
            //    MinFrequecy = dOutput.MinFrequency;
            //}
            //catch (Phidgets.PhidgetException ex)
            //{
            //    if (ex.ErrorCode != Phidgets.ErrorCode.Unsupported)
            //    {
            //        throw ex;
            //    }
            //}
        }

        private void VoltageOutputEx_PropertyChange(object sender, PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Phidget_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void VoltageOutputEx_Detach(object sender, DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageOutputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            IsAttached = false;
        }

        private void VoltageOutputEx_Error(object sender, ErrorEventArgs e)
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

        //public async Task RunActionLoops(InterfaceKitSequence interfaceKitSequence)
        //{
        //    try
        //    {
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
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

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
            Int64 startTicks = 0;
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
