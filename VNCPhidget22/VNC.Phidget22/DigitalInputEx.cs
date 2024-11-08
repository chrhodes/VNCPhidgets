﻿using System;
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
using System.IO;
using System.Threading.Channels;
using System.Data;

namespace VNC.Phidget22
{
    public class DigitalInputEx : Phidgets.DigitalInput, INotifyPropertyChanged
    {
        #region Constructors, Initialization, and Load

        private readonly DigitalInputConfiguration _digitalInputConfiguration;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="enabled"></param>
        public DigitalInputEx(int serialNumber, DigitalInputConfiguration digitalInputConfiguration, IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            _serialNumber = serialNumber;
            _digitalInputConfiguration = digitalInputConfiguration;
            _eventAggregator = eventAggregator;

            InitializePhidget();

            _eventAggregator.GetEvent<DigitalOutputSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void TriggerSequence(SequenceEventArgs args)
        {
            Log.EVENT_HANDLER("Called", Common.LOG_CATEGORY);
        }

        private void InitializePhidget()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            DeviceSerialNumber = SerialNumber;
            Channel = _digitalInputConfiguration.Channel;
            IsRemote = true;

            this.Attach += DigitalInputEx_Attach;
            this.Detach += DigitalInputEx_Detach;
            this.Error += DigitalInputEx_Error;
            this.PropertyChange += DigitalInputEx_PropertyChange;

            this.StateChange += DigitalInputEx_StateChange;


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

        public bool LogStateChangeEvents { get; set; } = true;

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

        private Phidgets.InputMode _inputMode;
        public Phidgets.InputMode InputMode
        {
            get => _inputMode;
            set
            {
                if (_inputMode == value)
                    return;
                _inputMode = value;

                if (base.Attached)
                {
                    base.InputMode = value;
                }

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

        private bool? _state = null;
        public new bool? State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                _state = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers (none)


        private void DigitalInputEx_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        {
            Phidgets.DigitalInput dInput = sender as Phidgets.DigitalInput;

            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Attach: sender:{sender} attached:{dInput.Attached}", Common.LOG_CATEGORY);
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

            InputMode = dInput.InputMode;
            PowerSupply = dInput.PowerSupply;

            State = dInput.State;

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

        private void DigitalInputEx_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        {
            if (LogPropertyChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void DigitalInputEx_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        {
            if (LogStateChangeEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_StateChange: sender:{sender} {e.State}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            State = e.State;
        }

        private void DigitalInputEx_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Detach: sender:{sender}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            IsAttached = false;
        }

        private void DigitalInputEx_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        {
            if (LogErrorEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"DigitalInputEx_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
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

       
        //override protected void OnPhidgetDeviceAttached()
        //{
        //    OnPhidgetDeviceAttached();
        //    PhidgetDeviceAttached?.Invoke();
        //}

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

        public async Task RunActionLoops(InterfaceKitSequence interfaceKitSequence)
        {
            try
            {
                Int64 startTicks = 0;

                if (LogSequenceAction)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" interfaceKitSequence:>{interfaceKitSequence.Name}<" +
                        $" startActionLoopSequences:>{interfaceKitSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{interfaceKitSequence.ActionLoops}<" +
                        $" actions:>{interfaceKitSequence.Actions.Count()}<" +
                        $" actionsDuration:>{interfaceKitSequence?.ActionsDuration}<" +
                        $" endActionLoopSequences:>{interfaceKitSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (interfaceKitSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < interfaceKitSequence.ActionLoops; actionLoop++)
                    {
                        if (interfaceKitSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in interfaceKitSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (interfaceKitSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(interfaceKitSequence.Actions, async action =>
                            {
                                // TODO(crhodes)
                                // Decide if want to close everything or pass in config to only open what we need
                                //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (InterfaceKitAction action in interfaceKitSequence.Actions)
                            {
                                // FIX(crhodes)
                                // 
                                //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                            }
                        }

                        if (interfaceKitSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{interfaceKitSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)interfaceKitSequence.ActionsDuration);
                        }

                        if (interfaceKitSequence.EndActionLoopSequences is not null)
                        {
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(_eventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in interfaceKitSequence.EndActionLoopSequences)
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
