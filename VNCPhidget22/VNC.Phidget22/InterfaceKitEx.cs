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

using VNCPhidget22.Configuration;
using System.Net;
using Phidget22;
using System.Diagnostics.Metrics;
using Phidget22.Events;

namespace VNC.Phidget22
{
    public class InterfaceKitEx : PhidgetEx // InterfaceKit
    {
        #region Constructors, Initialization, and Load

        public readonly IEventAggregator EventAggregator;

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

            //InterfaceKit = new Phidget22.InterfaceKit();

            //Attach += Phidget_Attach;
            //Detach += Phidget_Detach;
            //PropertyChange += Phidget_PropertyChange;
            //Error += Phidget_Error;

            //InterfaceKit.ServerConnect += Phidget_ServerConnect;
            //InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;

            // FIX(crhodes)
            // There is no InterfaceKit in Phidget22.  This is where we probably open channel 0
            // and figure out what kind of InterfaceKit we have.  Can be embedded, 8/8/8 16/16 etc
            // Then we can declare number of inputs, outputs, and sensors.

            // Alternatively just declare enough for the biggest InterfaceKit

            // HACK(crhodes)
            // For now just create one of each

            var digitialInputCount = 1;
            var digitialOutputCount = 1;
            var voltageInputCount = 1;
            var voltageRatioInputCount = 1;
            var voltageOutputCount = 1;

            DigitalInputs = new DigitalInput[digitialInputCount];
            DigitalOutputs = new DigitalOutput[digitialInputCount];
            VoltageInputs = new VoltageInput[digitialInputCount];
            VoltageRatioInputs = new VoltageRatioInput[digitialInputCount];
            VoltageOutputs = new VoltageOutput[digitialInputCount];

            // NOTE(crhodes)
            // Create channels and attach event handlers
            // Different events are fired from each type of channel

            // DigitalInputs

            for (int i = 0; i < digitialInputCount; i++)
            {
                DigitalInputs[i] = new DigitalInput();
                var channel = DigitalInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;
                
                channel.Attach += Phidget_Attach;
                channel.Detach += Phidget_Detach;
                channel.Error += Phidget_Error;
                channel.PropertyChange += Phidget_PropertyChange;
                channel.StateChange += Phidget_DigitalInputStateChange;
            }

            // DigitalOutputs

            for (int i = 0; i < digitialOutputCount; i++)
            {
                DigitalOutputs[i] = new DigitalOutput();
                var channel = DigitalOutputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                channel.Attach += Phidget_Attach;
                channel.Detach += Phidget_Detach;
                channel.Error += Phidget_Error;
                channel.PropertyChange += Phidget_PropertyChange;
            }

            // VoltageInputs

            for (int i = 0; i < voltageInputCount; i++)
            {
                VoltageInputs[i] = new VoltageInput();
                var channel = VoltageInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                channel.Attach += Phidget_Attach;
                channel.Detach += Phidget_Detach;
                channel.Error += Phidget_Error;
                channel.PropertyChange += Phidget_PropertyChange;
                channel.SensorChange += Phidget_VoltageInputSensorChange;
            }

            // VoltageRatioInputs

            for (int i = 0; i < voltageRatioInputCount; i++)
            {
                VoltageRatioInputs[i] = new VoltageRatioInput();
                var channel = VoltageRatioInputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                channel.Attach += Phidget_Attach;
                channel.Detach += Phidget_Detach;
                channel.Error += Phidget_Error;
                channel.PropertyChange += Phidget_PropertyChange;
                channel.SensorChange += Phidget_VoltageRatioInputSensorChange;
            }

            // VoltageOutputs

            for (int i = 0; i < voltageInputCount; i++)
            {
                VoltageOutputs[i] = new VoltageOutput();
                var channel = VoltageOutputs[i];

                channel.DeviceSerialNumber = SerialNumber;
                channel.Channel = i;
                channel.IsHubPortDevice = false;
                channel.IsRemote = true;

                channel.Attach += Phidget_Attach;
                channel.Detach += Phidget_Detach;
                channel.Error += Phidget_Error;
                channel.PropertyChange += Phidget_PropertyChange;
            }

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InterfaceKitEx_SensorChange(object sender, PhidgetsEvents.VoltageRatioInputSensorChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public DigitalInput[] DigitalInputs;
        public DigitalOutput[] DigitalOutputs;

        public VoltageInput[] VoltageInputs;
        public VoltageRatioInput[] VoltageRatioInputs;
        public VoltageOutput[] VoltageOutputs;

        // FIX(crhodes)
        // There is no InterfaceKit in Phidget22.
        //public Phidget22.InterfaceKit InterfaceKit = null;

        private bool _logInputChangeEvents;
        public bool LogInputChangeEvents 
        { 
            get => _logInputChangeEvents; 
            set
            {
                if (_logInputChangeEvents == value) return;

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

        #region Event Handlers

        // TODO(crhodes)
        // 
        //private void InterfaceKit_SensorChange(object sender, PhidgetsEvents.SensorChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.InterfaceKit ifk = (Phidget22.InterfaceKit)sender;
        //        Log.EVENT_HANDLER($"SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void InterfaceKit_OutputChange(object sender, Phidget22.Events.OutputChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.InterfaceKit ifk = (Phidget22.InterfaceKit)sender;
        //        Log.EVENT_HANDLER($"OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void InterfaceKit_InputChange(object sender, Phidget22.Events.InputChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.InterfaceKit ifk = (Phidget22.InterfaceKit)sender;
        //        Log.EVENT_HANDLER($"InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        public new void Open(Int32? timeOut = null)
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);
            
            // FIX(crhodes)
            // There is no InterfaceKit in Phidget22.
            // This is where we probably open all the stuff on the Phidget or we can just open what we use

            try
            {
                // HACK(crhodes)
                // For now just open digitalOutput0;

                Phidgets.DigitalOutput dout = DigitalOutputs[0];

                Phidgets.Phidget ik = dout.Parent;

                ik.Open();

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
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            //var pd = PhysicalPhidget;
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
            // NOTE(crhodes)
            // This is probably a good place to find out what kind of InterfaceKit we have
            // And fully populate things and then tell the world

            var digitalInputCount = PhysicalPhidget.GetDeviceChannelCount(ChannelClass.DigitalInput);
            OnPhidgetDeviceAttached(new EventArgs());
        }

        protected virtual void OnPhidgetDeviceAttached(EventArgs e)
        {
            PhidgetDeviceAttached?.Invoke(this, e);
        }



        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // NOTE(crhodes)
                // We may be logging events.  Remove them before closing.

                if (LogInputChangeEvents) LogInputChangeEvents = false;
                if (LogOutputChangeEvents) LogOutputChangeEvents = false;
                if (LogSensorChangeEvents) LogSensorChangeEvents = false;

                // FIX(crhodes)
                // 
                //this.InterfaceKit.close();
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
                                // FIX(crhodes)
                                // 
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

        #region Protected Methods (None)



        #endregion

        #region Private Methods

        // TODO(crhodes)
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
