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
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter ipAddress:{ipAddress} port:{port} serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            //// TODO(crhodes)
            //// 
            //InterfaceKit = new Phidget22.InterfaceKit();

            //InterfaceKit.Attach += Phidget_Attach;
            //InterfaceKit.Detach += Phidget_Detach;
            //InterfaceKit.Error += Phidget_Error;
            //InterfaceKit.ServerConnect += Phidget_ServerConnect;
            //InterfaceKit.ServerDisconnect += Phidget_ServerDisconnect;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        // TODO(crhodes)
        // 
        //public Phidget22.InterfaceKit InterfaceKit = null;

        private bool _logInputChangeEvents;
        public bool LogInputChangeEvents 
        { 
            get => _logInputChangeEvents; 
            set
            {
                if (_logInputChangeEvents == value) return;

                //// TODO(crhodes)
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

                //// TODO(crhodes)
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

                // TODO(crhodes)
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

            // TODO(crhodes)
            // 
            //try
            //{
            //    InterfaceKit.open(SerialNumber, Host.IPAddress, Host.Port);

            //    if (timeOut is not null)
            //    { 
            //        InterfaceKit.waitForAttachment((Int32)timeOut); 
            //    }
            //    else 
            //    { 
            //        InterfaceKit.waitForAttachment(); 
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, Common.LOG_CATEGORY);
            //}

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
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

                // TODO(crhodes)
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
                                // TODO(crhodes)
                                // 
                                //await PerformAction(InterfaceKit.outputs, action, action.DigitalOutIndex);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (InterfaceKitAction action in interfaceKitSequence.Actions)
                            {
                                // TODO(crhodes)
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
