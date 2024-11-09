using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Phidget22;

using Prism.Events;

using VNC.Phidget22.Events;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22
{
    public class VoltageRatioInputEx : Phidgets.VoltageRatioInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly VoltageRatioInputConfiguration _voltageRatioInputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new VoltageRatioInput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageRatioInputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageRatioInputEx(int serialNumber, VoltageRatioInputConfiguration voltageRatioInputConfiguration, IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _voltageRatioInputConfiguration = voltageRatioInputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<VoltageRatioInputSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures VoltageRatioInput using VoltageRatioInputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _voltageRatioInputConfiguration.Channel;
            IsRemote = true;

            this.Attach += VoltageRatioInputEx_Attach;
            this.Detach += VoltageRatioInputEx_Detach;
            this.Error += VoltageRatioInputEx_Error;
            this.PropertyChange += VoltageRatioInputEx_PropertyChange;

            this.SensorChange += VoltageRatioInputEx_SensorChange;
            this.VoltageRatioChange += VoltageRatioInputEx_VoltageRatioChange;

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

        public bool LogSensorChangeEvents { get; set; } = true;
        public bool LogVoltageRatioChangeEvents { get; set; } = true;

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

        private Phidgets.VoltageRatioSensorType _sensorType;
        public new Phidgets.VoltageRatioSensorType SensorType
        {
            get => _sensorType;
            set
            {
                if (_sensorType == value)
                    return;
                _sensorType = value;

                if (base.Attached)
                {
                    base.SensorType = value;
                }

                OnPropertyChanged();
            }
        }

        private Phidgets.UnitInfo _sensorUnit;
        public new Phidgets.UnitInfo SensorUnit
        {
            get => _sensorUnit;
            set
            {
                if (_sensorUnit.Unit == value.Unit)
                    return;
                _sensorUnit = value;
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

                if (base.Attached)
                {
                    base.SensorValueChangeTrigger = (Double)value;
                }

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

        private Int32 _minDataInterval;
        public new Int32 MinDataInterval
        {
            get => _minDataInterval;
            set
            {
                if (_minDataInterval == value)
                    return;
                _minDataInterval = value;
                OnPropertyChanged();
            }
        }

        private Int32? _DataInterval;
        public new Int32? DataInterval
        {
            get => _DataInterval;
            set
            {
                if (_DataInterval == value)
                    return;
                _DataInterval = value;

                if (base.Attached)
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
                if (_maxDataInterval == value)
                    return;
                _maxDataRate = value;
                OnPropertyChanged();
            }
        }

        private Double _minDataRate;
        public new Double MinDataRate
        {
            get => _minDataRate;
            set
            {
                if (_minDataRate == value)
                    return;
                _minDataRate = value;
                OnPropertyChanged();
            }
        }

        private Double? _DataRate;
        public new Double? DataRate
        {
            get => _DataRate;
            set
            {
                if (_DataRate == value)
                    return;
                _DataRate = value;

                if (base.Attached)
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
                if (_maxDataRate == value)
                    return;
                _maxDataRate = value;
                OnPropertyChanged();
            }
        }

        private Double _minVoltageRatio;
        public new Double MinVoltageRatio
        {
            get => _minVoltageRatio;
            set
            {
                if (_minVoltageRatio == value)
                    return;
                _minVoltageRatio = value;
                OnPropertyChanged();
            }
        }

        private Double? _VoltageRatio;
        public new Double? VoltageRatio
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
                if (_maxVoltageRatio == value)
                    return;
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
                if (_minVoltageRatioChangeTrigger == value)
                    return;
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

                if (base.Attached)
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
                if (_maxVoltageRatioChangeTrigger == value)
                    return;
                _maxVoltageRatioChangeTrigger = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void VoltageRatioInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageRatioInput voltageRatioInput = sender as Phidgets.VoltageRatioInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_Attach: sender:{sender} attached:{voltageRatioInput.Attached}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //IsAttached = VoltageRatioInput.Attached;

            // Just set it so UI behaves well
            IsAttached = true;

            SensorType = voltageRatioInput.SensorType;
            SensorValue = voltageRatioInput.SensorValue;
            SensorValueChangeTrigger = voltageRatioInput.SensorValueChangeTrigger;
            SensorUnit = voltageRatioInput.SensorUnit;

            MinDataInterval = voltageRatioInput.MinDataInterval;
            DataInterval = voltageRatioInput.DataInterval;
            MaxDataInterval = voltageRatioInput.MaxDataInterval;

            MinDataRate = voltageRatioInput.MinDataRate;
            DataRate = voltageRatioInput.DataRate;
            MaxDataRate = voltageRatioInput.MaxDataRate;

            MinVoltageRatio = voltageRatioInput.MinVoltageRatio;
            VoltageRatio = voltageRatioInput.VoltageRatio;
            MaxVoltageRatio = voltageRatioInput.MaxVoltageRatio;

            MinVoltageRatioChangeTrigger = voltageRatioInput.MinVoltageRatioChangeTrigger;
            VoltageRatioChangeTrigger = voltageRatioInput.VoltageRatioChangeTrigger;
            MaxVoltageRatioChangeTrigger = voltageRatioInput.MaxVoltageRatioChangeTrigger;

            // Not all VoltageRatioInput support all properties
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

            IsAttached = false;
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
        }

        #endregion

        #region Commands (none)


        #endregion

        #region Public Methods

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
