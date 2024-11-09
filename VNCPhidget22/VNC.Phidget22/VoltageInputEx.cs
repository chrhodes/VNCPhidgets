using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Prism.Events;

using VNC.Phidget22.Events;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22
{
    public class VoltageInputEx : Phidgets.VoltageInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly VoltageInputConfiguration _voltageInputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new VoltageInput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageInputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageInputEx(int serialNumber, VoltageInputConfiguration voltageInputConfiguration, IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _voltageInputConfiguration = voltageInputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<VoltageInputSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        /// <summary>
        /// Configures VoltageInput using VoltageInputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _voltageInputConfiguration.Channel;
            IsRemote = true;

            this.Attach += VoltageInputEx_Attach;
            this.Detach += VoltageInputEx_Detach;
            this.Error += VoltageInputEx_Error;
            this.PropertyChange += VoltageInputEx_PropertyChange;

            this.SensorChange += VoltageInputEx_SensorChange;
            this.VoltageChange += VoltageInputEx_VoltageChange;

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
        public bool LogVoltageChangeEvents { get; set; } = true;

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

        private Phidgets.VoltageSensorType _sensorType;
        public new Phidgets.VoltageSensorType SensorType
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

        private Phidgets.PowerSupply _powerSupply;
        public Phidgets.PowerSupply PowerSupply
        {
            get => _powerSupply;
            set
            {
                if (_powerSupply == value)
                    return;
                _powerSupply = value;

                if (base.Attached)
                {
                    base.PowerSupply = value;
                }

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

        private Double _minVoltage;
        public new Double MinVoltage
        {
            get => _minVoltage;
            set
            {
                if (_minVoltage == value)
                    return;
                _minVoltage = value;
                OnPropertyChanged();
            }
        }

        private Double? _Voltage;
        public new Double? Voltage
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
                if (_maxVoltage == value)
                    return;
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
                if (_minVoltageChangeTrigger == value)
                    return;
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

                if (base.Attached)
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
                if (_maxVoltageChangeTrigger == value)
                    return;
                _maxVoltageChangeTrigger = value;
                OnPropertyChanged();
            }
        }

        private Phidgets.VoltageRange _voltageRange;
        public new Phidgets.VoltageRange VoltageRange
        {
            get => _voltageRange;
            set
            {
                if (_voltageRange == value)
                    return;
                _voltageRange = value;

                if (base.Attached)
                {
                    base.VoltageRange = value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void VoltageInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageInput voltageInput = sender as Phidgets.VoltageInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageInputEx_Attach: sender:{sender} attached:{voltageInput.Attached}", Common.LOG_CATEGORY);
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

            SensorType = voltageInput.SensorType;
            SensorValue = voltageInput.SensorValue;
            SensorValueChangeTrigger = voltageInput.SensorValueChangeTrigger;
            SensorUnit = voltageInput.SensorUnit;

            MinDataInterval = voltageInput.MinDataInterval;
            DataInterval = voltageInput.DataInterval;
            MaxDataInterval = voltageInput.MaxDataInterval;

            MinDataRate = voltageInput.MinDataRate;
            DataRate = voltageInput.DataRate;
            MaxDataRate = voltageInput.MaxDataRate;

            MinVoltage = voltageInput.MinVoltage;
            Voltage = voltageInput.Voltage;
            MaxVoltage = voltageInput.MaxVoltage;

            MinVoltageChangeTrigger = voltageInput.MinVoltageChangeTrigger;
            VoltageChangeTrigger = voltageInput.VoltageChangeTrigger;
            MaxVoltageChangeTrigger = voltageInput.MaxVoltageChangeTrigger;

            PowerSupply = voltageInput.PowerSupply;
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
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
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
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
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
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
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
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            IsAttached = false;
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
