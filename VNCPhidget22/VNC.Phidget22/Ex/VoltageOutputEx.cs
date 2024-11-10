using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Prism.Events;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Events;
using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

namespace VNC.Phidget22.Ex
{
    public class VoltageOutputEx : Phidgets.VoltageOutput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly VoltageOutputConfiguration _voltageOutputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new VoltageOutput and adds Event handlers
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="voltageOutputConfiguration"></param>
        /// <param name="eventAggregator"></param>
        public VoltageOutputEx(int serialNumber, VoltageOutputConfiguration voltageOutputConfiguration, IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _voltageOutputConfiguration = voltageOutputConfiguration;
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
        /// Configures VoltageOutput using VoltageOutputConfiguration
        /// and establishes event handlers
        /// </summary>
        private void InitializePhidget()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _voltageOutputConfiguration.Channel;
            IsRemote = true;

            Attach += VoltageOutputEx_Attach;
            Detach += VoltageOutputEx_Detach;
            Error += VoltageOutputEx_Error;
            PropertyChange += VoltageOutputEx_PropertyChange;

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public bool LogPhidgetEvents { get; set; }
        public bool LogErrorEvents { get; set; }
        public bool LogPropertyChangeEvents { get; set; }

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

        private int _minFailsafeTime;
        public new int MinFailsafeTime
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

        private int _maxFailsafeTime;
        public new int MaxFailsafeTime
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

        private Phidgets.VoltageOutputRange _voltageOutputRange;
        public new Phidgets.VoltageOutputRange VoltageOutputRange
        {
            get => _voltageOutputRange;
            set
            {
                if (_voltageOutputRange == value)
                    return;
                _voltageOutputRange = value;

                if (Attached)
                {
                    base.VoltageOutputRange = value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void VoltageOutputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.VoltageOutput voltageOutput = sender as Phidgets.VoltageOutput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageOutputEx_Attach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Set properties to values from Phidget

            // NOTE(crhodes)
            // Shockingly, this is not set until after Attach Event

            //IsAttached = voltageOutput.Attached;

            // Just set it so UI behaves well
            IsAttached = true;

            MinVoltage = voltageOutput.MinVoltage;
            Voltage = voltageOutput.Voltage;
            MaxVoltage = voltageOutput.MaxVoltage;

            VoltageOutputRange = voltageOutput.VoltageOutputRange;

            // Not all VoltageOutput support all properties
            // Maybe just ignore or protect behind an if or switch
            // based on DeviceClass or DeviceID

            //try
            //{
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
        }

        private void VoltageOutputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageOutputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void VoltageOutputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
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

        private void VoltageOutputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"VoltageOutputEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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
