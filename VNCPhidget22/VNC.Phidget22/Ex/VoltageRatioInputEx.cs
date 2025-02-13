using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Phidget22;

using Prism.Events;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
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
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _voltageRatioInputConfiguration = voltageRatioInputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<VoltageRatioInputSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
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
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _voltageRatioInputConfiguration.Channel;
            IsRemote = true;

            Attach += VoltageRatioInputEx_Attach;
            Detach += VoltageRatioInputEx_Detach;
            Error += VoltageRatioInputEx_Error;
            PropertyChange += VoltageRatioInputEx_PropertyChange;

            SensorChange += VoltageRatioInputEx_SensorChange;
            VoltageRatioChange += VoltageRatioInputEx_VoltageRatioChange;

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

        bool _logSensorChangeEvents;
        public bool LogSensorChangeEvents 
        { 
            get { return _logSensorChangeEvents; } 
            set { _logSensorChangeEvents = value; OnPropertyChanged(); } 
        }

        bool _logVoltageRatioChangeEvents;
        public bool LogVoltageRatioChangeEvents
        { 
            get { return _logVoltageRatioChangeEvents; } 
            set { _logVoltageRatioChangeEvents = value; OnPropertyChanged(); }
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
                    base.SensorType = value;

                    // Update values, SensorType changed

                    SensorUnit_Unit = base.SensorUnit.Unit;
                    SensorUnit_Name = base.SensorUnit.Name;
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

        private string _sensorUnit_Name;
        public string SensorUnit_Name
        {
            get => _sensorUnit_Name;
            set
            {
                _sensorUnit_Name = value;
                OnPropertyChanged();
            }
        }

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

        private int _minDataInterval;
        public new int MinDataInterval
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

        private int? _DataInterval;
        public new int? DataInterval
        {
            get => _DataInterval;
            set
            {
                if (_DataInterval == value)
                    return;
                _DataInterval = value;

                if (Attached)
                {
                    base.DataInterval = (int)value;
                }

                OnPropertyChanged();
            }
        }

        private int _maxDataInterval;
        public new int MaxDataInterval
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

        private Double? _DataRate;
        public new Double? DataRate
        {
            get => _DataRate;
            set
            {
                if (_DataRate == value)
                    return;
                _DataRate = value;

                if (Attached)
                {
                    base.DataRate = (int)value;
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

        #endregion

        #region Event Handlers

        private void VoltageRatioInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageRatioInput voltageRatioInput = sender as Phidgets.VoltageRatioInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageRatioInputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
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

            // TODO(crhodes)
            // 
            // SensorType needs to be set before SensorUnit and SensorValue can be read
            //SensorType = VoltageSensorType.Voltage;
            //SensorUnit = voltageInput.SensorUnit;
            //SensorValue = voltageInput.SensorValue;

            try
            {
                SensorValueChangeTrigger = voltageRatioInput.SensorValueChangeTrigger;

                MinDataInterval = voltageRatioInput.MinDataInterval;
                //DataInterval = voltageRatioInput.DataInterval;
                DataInterval = 100; // 100ms (10Hz)
                MaxDataInterval = voltageRatioInput.MaxDataInterval;

                MinDataRate = voltageRatioInput.MinDataRate;
                //DataRate = voltageRatioInput.DataRate;
                DataRate = 10; // 10 Hz (100ms)
                MaxDataRate = voltageRatioInput.MaxDataRate;

                MinVoltageRatio = voltageRatioInput.MinVoltageRatio;
                VoltageRatio = voltageRatioInput.VoltageRatio;
                SensorValue = voltageRatioInput.SensorValue;
                MaxVoltageRatio = voltageRatioInput.MaxVoltageRatio;

                MinVoltageRatioChangeTrigger = voltageRatioInput.MinVoltageRatioChangeTrigger;
                VoltageRatioChangeTrigger = voltageRatioInput.VoltageRatioChangeTrigger;
                MaxVoltageRatioChangeTrigger = voltageRatioInput.MaxVoltageRatioChangeTrigger;
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

            // Not all VoltageRatioInput support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{

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
                    Log.EVENT_HANDLER($"Exit VoltageRatioInputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
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
                    Log.EVENT_HANDLER($"DigitalOutputEx_PropertyChange: sender:{sender} {e.PropertyName} - Update switch()", Common.LOG_CATEGORY);
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

        public new void Open()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Open();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Open(Int32 timeout)
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Open(timeout);

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void Close()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            base.Close();

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public new void RefreshProperties()
        {
            Int64 startTicks = 0;
            if (LogPhidgetEvents) startTicks = Log.Trace($"Enter isOpen:{IsOpen}", Common.LOG_CATEGORY);

            DataInterval = base.DataInterval;
            DataRate = base.DataRate;

            if (LogPhidgetEvents) Log.Trace($"Exit isOpen:{IsOpen}", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(InterfaceKitSequence interfaceKitSequence)
        {
            try
            {
                Int64 startTicks = 0;

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
                // NOTE(crhodes)
                // First make any logging changes

                //        #region Logging

                //        if (action.LogPhidgetEvents is not null) LogPhidgetEvents = (Boolean) action.LogPhidgetEvents;
                //        if (action.LogErrorEvents is not null) LogErrorEvents = (Boolean) action.LogErrorEvents;
                //        if (action.LogPropertyChangeEvents is not null) LogPropertyChangeEvents = (Boolean) action.LogPropertyChangeEvents;

                //        if (action.LogPositionChangeEvents is not null) LogPositionChangeEvents = (Boolean) action.LogPositionChangeEvents;
                //        if (action.LogVelocityChangeEvents is not null) LogVelocityChangeEvents = (Boolean) action.LogVelocityChangeEvents;
                //        if (action.LogTargetPositionReachedEvents is not null) LogTargetPositionReachedEvents = (Boolean) action.LogTargetPositionReachedEvents;

                //        if (action.LogPerformanceSequence is not null) LogPerformanceSequence = (Boolean) action.LogPerformanceSequence;
                //        if (action.LogSequenceAction is not null) LogSequenceAction = (Boolean) action.LogSequenceAction;
                //        if (action.LogActionVerification is not null) LogActionVerification = (Boolean) action.LogActionVerification;

                //#endregion
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
