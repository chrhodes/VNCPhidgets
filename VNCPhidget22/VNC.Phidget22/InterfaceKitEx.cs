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
using Phidget22.Events;
using System.Runtime.CompilerServices;

namespace VNC.Phidget22
{
    public class InterfaceKitEx : PhidgetEx // InterfaceKit
    {
        #region Constructors, Initialization, and Load

        readonly DeviceChannels _deviceChannels;

        // TODO(crhodes)
        // Why is this public?

        public readonly IEventAggregator EventAggregator;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="enabled"></param>
        public InterfaceKitEx(int serialNumber, DeviceChannels deviceChannels, IEventAggregator eventAggregator)
            : base(serialNumber)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            _deviceChannels = deviceChannels;

            InitializePhidget();

            EventAggregator.GetEvent<InterfaceKitSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // TODO(crhodes)
        // Don't think we need this anymore

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="enabled"></param>
        public InterfaceKitEx(string ipAddress, int port, int serialNumber, IEventAggregator eventAggregator) 
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter ipAddress:{ipAddress} port:{port} serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            InitializePhidget();

            EventAggregator.GetEvent<InterfaceKitSequenceEvent>().Subscribe(TriggerSequence);

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

            var digitalInputCount = _deviceChannels.DigitalInputCount;
            var digitalOutputCount = _deviceChannels.DigitalOutputCount;
            var voltageInputCount = _deviceChannels.VoltageInputCount;
            var voltageRatioInputCount = _deviceChannels.VoltageRatioInputCount;
            var voltageOutputCount = _deviceChannels.VoltageOutputCount;

            DigitalInputs = new Phidgets.DigitalInput[digitalInputCount];
            DigitalOutputs = new Phidgets.DigitalOutput[digitalOutputCount];
            VoltageInputs = new Phidgets.VoltageInput[voltageInputCount];
            VoltageRatioInputs = new Phidgets.VoltageRatioInput[voltageRatioInputCount];
            VoltageOutputs = new Phidgets.VoltageOutput[voltageOutputCount];

            // NOTE(crhodes)
            // Create channels and attach event handlers
            // Different events are fired from each type of channel

            // DigitalInputs

            for (int i = 0; i < digitalInputCount; i++)
            {
                DigitalInputs[i] = new Phidgets.DigitalInput();
                var channel = DigitalInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;
                
                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
                //channel.StateChange += Channel_DigitalInputStateChange;
            }

            // DigitalOutputs

            for (int i = 0; i < digitalOutputCount; i++)
            {
                DigitalOutputs[i] = new Phidgets.DigitalOutput();
                var channel = DigitalOutputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
            }

            // VoltageInputs

            for (int i = 0; i < voltageInputCount; i++)
            {
                VoltageInputs[i] = new Phidgets.VoltageInput();
                var channel = VoltageInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
                //channel.SensorChange += Channel_VoltageInputSensorChange;
                //channel.VoltageChange += Channel_VoltageChange;
            }

            // VoltageRatioInputs

            for (int i = 0; i < voltageRatioInputCount; i++)
            {
                VoltageRatioInputs[i] = new Phidgets.VoltageRatioInput();
                var channel = VoltageRatioInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
                //channel.SensorChange += Phidget_VoltageRatioInputSensorChange;
                //channel.VoltageRatioChange += Channel_VoltageRatioChange;
            }

            // VoltageOutputs

            for (int i = 0; i < voltageOutputCount; i++)
            {
                VoltageOutputs[i] = new Phidgets.VoltageOutput();
                var channel = VoltageOutputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
            }

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public Phidgets.DigitalInput[] DigitalInputs;
        public Phidgets.DigitalOutput[] DigitalOutputs;

        public Phidgets.VoltageInput[] VoltageInputs;
        public Phidgets.VoltageRatioInput[] VoltageRatioInputs;
        public Phidgets.VoltageOutput[] VoltageOutputs;

        // FIX(crhodes)
        // There is no InterfaceKit in Phidget22.
        //public Phidget22.InterfaceKit InterfaceKit = null;

        // TODO(crhodes)
        // These can probably be simple get; set;

        private bool _logInputChangeEvents;
        public bool LogInputChangeEvents 
        { 
            get => _logInputChangeEvents; 
            set
            {
                if (_logInputChangeEvents == value) return;
                _logInputChangeEvents = value;

                //// FIX(crhodes)
                //// 
                //if (_logInputChangeEvents = value)
                //{
                //    InterfaceKit.InputChange += InterfaceKit_InputChange;
                //}
                //else
                //{
                //    InterfaceKit.InputChange -= InterfaceKit_InputChange;
                //}
            }
        }

        private bool _logOutputChangeEvents;
        public bool LogOutputChangeEvents 
        { 
            get => _logOutputChangeEvents;
            set
            {
                if (_logOutputChangeEvents == value) return;
                _logOutputChangeEvents = value;

                //// FIX(crhodes)
                //// 
                //if (_logOutputChangeEvents = value)
                //{
                //    InterfaceKit.OutputChange += InterfaceKit_OutputChange;
                //}
                //else
                //{
                //    InterfaceKit.OutputChange -= InterfaceKit_OutputChange;
                //}
            }
        }

        private bool _logSensorChangeEvents;
        public bool LogSensorChangeEvents 
        {
            get => _logSensorChangeEvents;
            set
            {
                if (_logSensorChangeEvents == value) return;
                _logSensorChangeEvents = value;
                // FIX(crhodes)
                // 
                //if (_logSensorChangeEvents = value)
                //{
                //    InterfaceKit.SensorChange += InterfaceKit_SensorChange;
                //}
                //else
                //{
                //    InterfaceKit.SensorChange -= InterfaceKit_SensorChange;
                //}
            }
        }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Comands (none)


        #endregion

        #region Public Methods

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        public new void Open(Int32? timeOut = null)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // FIX(crhodes)
                // There is no InterfaceKit in Phidget22.
                // This is where we probably open all the stuff on the Phidget
                // or we can just open what we use

                var digitalInputCount = _deviceChannels.DigitalInputCount;
                var digitalOutputCount = _deviceChannels.DigitalOutputCount;
                var voltageInputCount = _deviceChannels.VoltageInputCount;
                var voltageRatioInputCount = _deviceChannels.VoltageRatioInputCount;
                var voltageOutputCount = _deviceChannels.VoltageOutputCount;

                // TODO(crhodes)
                // Decide if want to open everything or pass in config to only open what we need

                for (int i = 0; i < digitalInputCount; i++)
                {
                    DigitalInputs[i].Open();
                }

                for (int i = 0; i < digitalOutputCount; i++)
                {
                    DigitalOutputs[i].Open();
                }

                // TODO(crhodes)
                // Figure out which type of voltageInput to use

                for (int i = 0; i < voltageInputCount; i++)
                {
                    VoltageInputs[i].Open();
                }

                for (int i = 0; i < voltageRatioInputCount; i++)
                {
                    VoltageRatioInputs[i].Open();
                }

                for (int i = 0; i < voltageOutputCount; i++)
                {
                    VoltageOutputs[i].Open();
                }

                //DigitalOutputs[0].Open();

                //DigitalOutputs[0].Open(500);

                //InterfaceKit.open(SerialNumber, Host.IPAddress, Host.Port);

                //if (timeOut is not null)
                //{
                //    InterfaceKit.waitForAttachment((Int32)timeOut);
                //}
                //else
                //{
                //    InterfaceKit.waitForAttachment();
                //}
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} message:{pex.Message} description:{pex.Description} detail:{pex.Detail} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //override protected void OnPhidgetDeviceAttached()
        //{
        //    OnPhidgetDeviceAttached();
        //    PhidgetDeviceAttached?.Invoke();
        //}

        public event EventHandler PhidgetDeviceAttached;

        override protected void PhidgetDeviceIsAttached()
        {
            OnPhidgetDeviceAttached(new EventArgs());
        }

        // NOTE(crhodes)
        // This tells the UI that we have an attached Phidget

        protected virtual void OnPhidgetDeviceAttached(EventArgs e)
        {
            PhidgetDeviceAttached?.Invoke(this, e);
        }

        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // FIX(crhodes)
                // There is no InterfaceKit in Phidget22.
                // This is where we probably close all the stuff on the Phidget
                // or we can just close what we use

                var digitalInputCount = _deviceChannels.DigitalInputCount;
                var digitalOutputCount = _deviceChannels.DigitalOutputCount;
                var voltageInputCount = _deviceChannels.VoltageInputCount;
                var voltageRatioInputCount = _deviceChannels.VoltageRatioInputCount;
                var voltageOutputCount = _deviceChannels.VoltageOutputCount;

                // NOTE(crhodes)
                // We may be logging events.  Remove them before closing.

                if (LogInputChangeEvents) LogInputChangeEvents = false;
                if (LogOutputChangeEvents) LogOutputChangeEvents = false;
                if (LogSensorChangeEvents) LogSensorChangeEvents = false;

                // TODO(crhodes)
                // Decide if want to close everything or pass in config to only open what we need

                for (int i = 0; i < digitalInputCount; i++)
                {
                    DigitalInputs[i].Close();
                }

                for (int i = 0; i < digitalOutputCount; i++)
                {
                    DigitalOutputs[i].Close();
                }

                for (int i = 0; i < voltageInputCount; i++)
                {
                    VoltageInputs[i].Close();
                }

                for (int i = 0; i < voltageRatioInputCount; i++)
                {
                    VoltageRatioInputs[i].Open();
                }

                for (int i = 0; i < voltageOutputCount; i++)
                {
                    VoltageOutputs[i].Close();
                }
            }
            catch (Phidgets.PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"source:{pex.Source} message:{pex.Message} description:{pex.Description} detail:{pex.Detail} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

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
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(EventAggregator);
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
    }
}
