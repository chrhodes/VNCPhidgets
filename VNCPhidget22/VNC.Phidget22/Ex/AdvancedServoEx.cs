﻿using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

using Prism.Events;

using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using VNC.Phidget22.Configuration;
using System.Runtime.CompilerServices;

namespace VNC.Phidget22.Ex
{
    public class AdvancedServoEx : PhidgetEx
    {
        #region Constructors, Initialization, and Load

        readonly DeviceChannels _deviceChannels;

        // TODO(crhodes)
        // Why is this public?

        public IEventAggregator EventAggregator { get; set; }

        public AdvancedServoEx(int serialNumber, DeviceChannels deviceChannels, IEventAggregator eventAggregator)
            : base(serialNumber)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter: serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            _deviceChannels = deviceChannels;

            InitializePhidget();

            EventAggregator.GetEvent<AdvancedServoSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // TODO(crhodes)
        // Don't think we need this anymore

        /// <summary>
        /// Initializes a new instance of the AdvancedServo class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public AdvancedServoEx(string ipAddress, int port, int serialNumber, IEventAggregator eventAggregator)
            : base(ipAddress, port, serialNumber)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter ipAdress:{ipAddress} port:{port} serialNumber:{serialNumber}", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            InitializePhidget();

            EventAggregator.GetEvent<AdvancedServoSequenceEvent>().Subscribe(TriggerSequence);

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            //AdvancedServo = new Phidget22.AdvancedServo();

            //AdvancedServo.Attach += Phidget_Attach;
            //AdvancedServo.Detach += Phidget_Detach;
            //AdvancedServo.Error += Phidget_Error;
            //AdvancedServo.ServerConnect += Phidget_ServerConnect;
            //AdvancedServo.ServerDisconnect += Phidget_ServerDisconnect;

            var rcServoCount = _deviceChannels.RCServoCount;
            var currentInputCount = _deviceChannels.CurrentInputCount;

            RCServos = new Phidgets.RCServo[rcServoCount];
            CurrentInputs = new Phidgets.CurrentInput[currentInputCount];

            // NOTE(crhodes)
            // Create channels and attach event handlers
            // Different events are fired from each type of channel

            // RCServos

            for (int i = 0; i < rcServoCount; i++)
            {
                RCServos[i] = new Phidgets.RCServo();
                var channel = RCServos[i];

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

            // CurrentInputs

            for (int i = 0; i < currentInputCount; i++)
            {
                CurrentInputs[i] = new Phidgets.CurrentInput();
                var channel = CurrentInputs[i];

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

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures

        public struct ServoMinMax
        {
            public Double AccelerationMin;
            public Double AccelerationMax;
            // Since Position can be changed
            // Save in DevicePosition{Min,Max}
            //public Double PositionMin;    
            //public Double PositionMax;
            public Double DevicePositionMin;
            public Double DevicePositionMax;
            public Double VelocityMin;
            public Double VelocityMax;
        }

        #endregion

        #region Fields and Properties

        public Phidgets.RCServo[] RCServos;
        public Phidgets.CurrentInput[] CurrentInputs;

        // FIX(crhodes)
        // There is no longer an AdvancedServo in Phidget22
        //public Phidget22.AdvancedServo AdvancedServo = null;

        // TODO(crhodes)
        // These can probably be simple get; set;


        private bool _logPositionChangeEvents;
        public bool LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value) return;
                _logPositionChangeEvents = value;

                //if (_logPositionChangeEvents = value)
                //{
                //    AdvancedServo.PositionChange += AdvancedServo_PositionChange;
                //}   
                //else
                //{
                //    AdvancedServo.PositionChange -= AdvancedServo_PositionChange;
                //}
            }
        }

        private bool _logVelocityChangeEvents;
        public bool LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value) return;
                _logVelocityChangeEvents = value;

                //if (_logVelocityChangeEvents = value)
                //{
                //    AdvancedServo.VelocityChange += AdvancedServo_VelocityChange;
                //}
                //else
                //{
                //    AdvancedServo.VelocityChange -= AdvancedServo_VelocityChange;
                //}
            }
        }

        private bool _logCurrentChangeEvents;
        public bool LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value) return;
                _logCurrentChangeEvents = value;

                //if (_logCurrentChangeEvents = value)
                //{
                //    AdvancedServo.CurrentChange += AdvancedServo_CurrentChange;
                //}
                //else
                //{
                //    AdvancedServo.CurrentChange -= AdvancedServo_CurrentChange;
                //}
            }
        }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        public ServoMinMax[] InitialServoLimits { get; set; } = new ServoMinMax[8];

        #endregion

        #region Event Handlers (none)

        //internal override void Phidget_Attach(object sender, AttachEventArgs e)
        //{
        //    // This handles the logging
        //    base.Phidget_Attach(sender, e);

        //    SaveServoLimits();
        //}

        //private void AdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.AdvancedServo advancedServo = sender as Phidget22.AdvancedServo;
        //        Phidget22.AdvancedServoServo servo = advancedServo.servos[e.Index];
        //        Log.EVENT_HANDLER($"CurrentChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
        //            $" - current:{e.Current:00.000} - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void AdvancedServo_PositionChange(object sender, PositionChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.AdvancedServo advancedServo = sender as Phidget22.AdvancedServo;
        //        Phidget22.AdvancedServoServo servo = advancedServo.servos[e.Index];
        //        Log.EVENT_HANDLER($"PositionChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
        //            $" - velocity:{servo.Velocity,8:0.000} position:{e.Position,7:0.000} current:{servo.Current:00.000}" +
        //            $" - stopped:{servo.Stopped} ", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void AdvancedServo_VelocityChange(object sender, VelocityChangeEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.AdvancedServo advancedServo = sender as Phidget22.AdvancedServo;
        //        Phidget22.AdvancedServoServo servo = advancedServo.servos[e.Index];
        //        Log.EVENT_HANDLER($"VelocityChange {advancedServo.Address},{advancedServo.SerialNumber},servo:{e.Index}" +
        //            $" - velocity:{e.Velocity,8:0.000} position:{servo.Position,7:0.000} current:{servo.Current:00.000}" +
        //            $" - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods

        /// <summary>
        /// Open Phidget and waitForAttachment
        /// </summary>
        /// <param name="timeOut">Optionally time out after timeOut(ms)</param>
        public new void Open(int? timeOut = null)
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // FIX(crhodes)
                // There is no AdvancedServo in Phidget22.
                // This is where we probably open all the stuff on the Phidget
                // or we can just open what we use

                var rcServoCount = _deviceChannels.RCServoCount;
                var currentInputCount = _deviceChannels.CurrentInputCount;

                // HACK(crhodes)
                // Trying to get new RCC0004_0 AdvancedServo board to work with
                // Legacy SBC1 or SBC2.  No luck.
                // Only seems to work it directly attached, sigh.
                //AdvancedServo.open(SerialNumber);
                //AdvancedServo.open(-1);

                // TODO(crhodes)
                // Decide if want to open everything or pass in config to only open what we need

                for (int i = 0; i < rcServoCount; i++)
                {
                    RCServos[i].Open();
                }

                for (int i = 0; i < currentInputCount; i++)
                {
                    CurrentInputs[i].Open();
                }

                //AdvancedServo.open(SerialNumber, Host.IPAddress, Host.Port);

                //if (timeOut is not null)
                //{
                //    AdvancedServo.waitForAttachment((Int32)timeOut); 
                //}
                //else 
                //{ 
                //    AdvancedServo.waitForAttachment();
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
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                // FIX(crhodes)
                // There is no AdvancedServo in Phidget22.
                // This is where we probably close all the stuff on the Phidget
                // or we can just close what we use

                var rcServoCount = _deviceChannels.RCServoCount;
                var currentInputCount = _deviceChannels.CurrentInputCount;

                // NOTE(crhodes)
                // We may be logging events.  Remove them before closing

                if (LogCurrentChangeEvents) LogCurrentChangeEvents = false;
                if (LogPositionChangeEvents) LogPositionChangeEvents = false;
                if (LogVelocityChangeEvents) LogVelocityChangeEvents = false;

                // TODO(crhodes)
                // Decide if want to close everything or pass in config to only open what we need

                for (int i = 0; i < rcServoCount; i++)
                {
                    RCServos[i].Close();
                }

                for (int i = 0; i < currentInputCount; i++)
                {
                    CurrentInputs[i].Close();
                }

                //AdvancedServo.close();
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

        public async Task RunActionLoops(AdvancedServoSequence advancedServoSequence)
        {
            long startTicks = 0;

            try
            {
                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" advancedServoSequence:>{advancedServoSequence.Name}<" +
                        $" startActionLoopSequences:>{advancedServoSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{advancedServoSequence.ActionLoops}<" +
                        $" actions:>{advancedServoSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{advancedServoSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{advancedServoSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (advancedServoSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < advancedServoSequence.ActionLoops; actionLoop++)
                    {
                        if (advancedServoSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in advancedServoSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (advancedServoSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(advancedServoSequence.Actions, async action =>
                            {
                                // FIX(crhodes)
                                // 
                                //await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (AdvancedServoServoAction action in advancedServoSequence.Actions)
                            {
                                // FIX(crhodes)
                                // 
                                //await PerformAction(action);
                            }
                        }

                        if (advancedServoSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{advancedServoSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((int)advancedServoSequence.ActionsDuration);
                        }

                        if (advancedServoSequence.EndActionLoopSequences is not null)
                        {
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(EventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in advancedServoSequence.EndActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogSequenceAction) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // FIX(crhodes)
        // Ugh, lots to fix

        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="servo"></param>
        //public void SetAcceleration(Double acceleration, AdvancedServoServo servo, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin index:{index} acceleration:{acceleration}" +
        //                $" accelerationMin:{servo.AccelerationMin}" +
        //                //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
        //                $" accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //        }

        //        if (acceleration < servo.AccelerationMin)
        //        {
        //            servo.Acceleration = servo.AccelerationMin;
        //        }
        //        else if (acceleration > servo.AccelerationMax)
        //        {
        //            servo.Acceleration = servo.AccelerationMax;
        //        }
        //        else
        //        {
        //            servo.Acceleration = acceleration;
        //        }

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End index:{index} servoAcceleration:{servo.Acceleration}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Error($"index:{index} acceleration:{acceleration}" +
        //            $" accelerationMin:{servo.AccelerationMin}" +
        //            //$" acceleration:{servo.Acceleration}" + // Can't check this as it may not have been set yet
        //            $" accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        ///// <summary>
        ///// Bounds check and set velocity
        ///// </summary>
        ///// <param name="velocityLimit"></param>
        ///// <param name="servo"></param>
        //public void SetVelocityLimit(Double velocityLimit, AdvancedServoServo servo, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin index:{index}" +
        //                $" velocityLimit:{velocityLimit}" +
        //                $" servo.velocityMin:{servo.VelocityMin}" +
        //                $" servo.velocityLimit:{servo.VelocityLimit}" +
        //                $" servo.velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //        }

        //        if (velocityLimit < servo.VelocityMin)
        //        {
        //            servo.VelocityLimit = servo.VelocityMin;
        //        }
        //        else if (velocityLimit > servo.VelocityMax)
        //        {
        //            servo.VelocityLimit = servo.VelocityMax;
        //        }
        //        else
        //        {
        //            servo.VelocityLimit = velocityLimit;
        //        }

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End index:{index} velocityLimit:{velocityLimit} velocityLimit:{servo.VelocityLimit}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Error($"index:{index}" +
        //            $" velocity:{velocityLimit}" +
        //            $" servo.velocityMin:{servo.VelocityMin}" +
        //            $" servo.velocityLimit:{servo.VelocityLimit}" +
        //            $" servo.velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        ///// <summary>
        ///// Bounds check and set position
        ///// </summary>
        ///// <param name="positionMin"></param>
        ///// <param name="servo"></param>
        //public void SetPositionMin(Double positionMin, AdvancedServoServo servo, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin index:{index} positionMin:{positionMin}" +
        //                $" servo.PositionMin:{servo.PositionMin}" +
        //                $" servo.PositionMax:{servo.PositionMax}" +
        //                $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
        //                $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //        }

        //        if (positionMin < 0)
        //        {
        //            positionMin = InitialServoLimits[index].DevicePositionMin;
        //        }
        //        else if (positionMin < InitialServoLimits[index].DevicePositionMin)
        //        {
        //            positionMin = InitialServoLimits[index].DevicePositionMin;
        //        }
        //        else if (positionMin > servo.PositionMax)
        //        {
        //            positionMin = servo.PositionMax;
        //        }

        //        if (servo.PositionMin != positionMin) servo.PositionMin = positionMin;

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End index:{index} positionMin:{positionMin} servo.PositionMin:{servo.PositionMin}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} {pex.Description} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Error($"index:{index} positionMin:{positionMin}" +
        //            $" servo.PositionMin:{servo.PositionMin}" +
        //            $" servo.PositionMax:{servo.PositionMax}" +
        //            $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
        //            $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        ///// <summary>
        ///// Bounds check and set position
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="servo"></param>
        //public Double SetPosition(Double position, AdvancedServoServo servo, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin servo:{index} position:{position}" +
        //                $" servo.PositionMin:{servo.PositionMin}" +
        //                $" servo.PositionMax:{servo.PositionMax}" +
        //                $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
        //                $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //        }

        //        if (position < servo.PositionMin)
        //        {
        //            position = servo.PositionMin;
        //        }
        //        else if (position > servo.PositionMax)
        //        {
        //            position = servo.PositionMax;
        //        }

        //        // TODO(crhodes)
        //        // Maybe save last position set and not bother checking servo.Position is same
        //        if (servo.Position != position) servo.Position = position;

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End servo:{index} position:{position} servo.Position:{servo.Position}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} servo.position:{servo.Position}" +
        //            $" servo.PositionMin:{servo.PositionMin}" +
        //            $" servo.PositionMax:{servo.PositionMax}" +
        //            $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
        //            $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    return position;
        //}

        ///// <summary>
        ///// Bounds check and set position
        ///// </summary>
        ///// <param name="positionMax"></param>
        ///// <param name="servo"></param>
        //public void SetPositionMax(Double positionMax, AdvancedServoServo servo, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin servo:{index} positionMax:{positionMax}" +
        //                $" servo.PositionMin:{servo.PositionMin}" +
        //                $" servo.PositionMax:{servo.PositionMax}" +
        //                $" DevicePositionMin:{InitialServoLimits[index].DevicePositionMin}" +
        //                $" DevicePositionMax:{InitialServoLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //        }

        //        if (positionMax < 0)
        //        {
        //            positionMax = InitialServoLimits[index].DevicePositionMax;
        //        }
        //        else if (positionMax < servo.PositionMin)
        //        {
        //            positionMax = servo.PositionMin;
        //        }
        //        else if (positionMax > InitialServoLimits[index].DevicePositionMax)
        //        {
        //            positionMax = InitialServoLimits[index].DevicePositionMax;
        //        }

        //        if (servo.PositionMax != positionMax) servo.PositionMax = positionMax;

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End servo:{index} positionMax:{positionMax} servo.PositionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (Phidgets.PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
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
        // Ugh, lots to fix

        // TODO(crhodes)
        // Do we really need this as all it does is SaveServoLimits

        //private void SaveServoLimits()
        //{
        //    try
        //    {
        //        AdvancedServoServoCollection servos = AdvancedServo.servos;
        //        AdvancedServoServo servo = null;

        //        // Save the device position min/max before any changes are made

        //        for (int index = 0; index < servos.Count; index++)
        //        {
        //            servo = servos[index];

        //            //if (LogPhidgetEvents)
        //            //{
        //            //    Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged}", Common.LOG_CATEGORY);

        //            //    //string currentPosition = servo.Engaged ? servo.Position.ToString() : "???";

        //            //    //Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged} position:{currentPosition}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        //            //    //Log.Trace($"servo:{index} velocityMin:{servo.VelocityMin} positionMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //            //}

        //            // NOTE(crhodes)
        //            // Force the initial Servo Type to avoid opening something that has
        //            // been set to an unexpected Type, e.g. RAW_us_MODE or 
        //            // Which have crazy values

        //            // NOTE(crhodes)
        //            // What happens if we don't set type?

        //            //if (servo.Type == ServoServo.ServoType.RAW_us_MODE)
        //            //{
        //            //servo.Type = ServoServo.ServoType.DEFAULT;
        //            //}

        //            SaveServoLimits(servo, index);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void SaveServoLimits(AdvancedServoServo servo, Int32 index)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged}" +
        //            $" accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}" +
        //            $" positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}" +
        //            $" velocityMin:{servo.VelocityMin} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}", Common.LOG_CATEGORY);
        //        //Log.Trace($"servo:{index} velocityMin:{servo.VelocityMin} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
        //    }

        //    // NOTE(crhodes)
        //    // We do not need to save Acceleration Min,Max and Velocity Min,Max,
        //    // they cannot change, but, useful to have available
        //    // when setting Acceleration/VelocityLimit to Min/Max in PerformAction

        //    InitialServoLimits[index].AccelerationMin = servo.AccelerationMin;
        //    InitialServoLimits[index].AccelerationMax = servo.AccelerationMax;
        //    InitialServoLimits[index].DevicePositionMin = servo.PositionMin;
        //    //InitialServoLimits[i].PositionMin = servo.PositionMin; 
        //    //InitialServoLimits[i].PositionMax = servo.PositionMax;
        //    InitialServoLimits[index].DevicePositionMax = servo.PositionMax;
        //    InitialServoLimits[index].VelocityMin = servo.VelocityMin + 1; // 0 won't move
        //    InitialServoLimits[index].VelocityMax = servo.VelocityMax;
        //}

        //private async Task PerformAction(AdvancedServoServoAction action)
        //{             
        //    Int64 startTicks = 0;

        //    Int32 index = action.ServoIndex;

        //    StringBuilder actionMessage = new StringBuilder();

        //    if (LogSequenceAction)
        //    {
        //        startTicks = Log.Trace($"Enter servo:{index}", Common.LOG_CATEGORY);
        //        actionMessage.Append($"servo:{index}");
        //    }

        //    AdvancedServoServo servo = AdvancedServo.servos[index];

        //    try
        //    {
        //        if (action.ServoType is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" servoType:>{action.ServoType}<");

        //            servo.Type = (Phidget22.ServoServo.ServoType)action.ServoType;

        //            // NOTE(crhodes)
        //            // Maybe we should sleep for a little bit to allow this to happen
        //            Thread.Sleep(1);


        //            // Save the refreshed values
        //            SaveServoLimits(servo, index);
        //        }

        //        // NOTE(crhodes)
        //        // These can be performed without the servo being engaged.  This helps address
        //        // previous values that get applied when servo engaged.
        //        // The servo still snaps to last position when enabled.  No known way to address that.

        //        if (action.Acceleration is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");
        //            var acceleration = action.Acceleration;

        //            if (acceleration < 0)
        //            {
        //                if (acceleration == -1)        // -1 is magic number for AccelerationMin :)
        //                {
        //                    acceleration = InitialServoLimits[index].AccelerationMin;
        //                }
        //                else if (acceleration == -2)   // -2 is magic number for AccelerationMax :)
        //                {
        //                    acceleration = InitialServoLimits[index].AccelerationMax;
        //                }
        //            }

        //            SetAcceleration((Double)acceleration, servo, index);
        //        }

        //        if (action.VelocityLimit is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");
        //            var velocityLimit = action.VelocityLimit;

        //            if (velocityLimit < 0)
        //            {
        //                if (velocityLimit == -1)        // -1 is magic number for VelocityMin :)
        //                {
        //                    velocityLimit = InitialServoLimits[index].VelocityMin;
        //                }
        //                else if (velocityLimit == -2)   // -2 is magic number for VelocityMax :)
        //                {
        //                    velocityLimit = InitialServoLimits[index].VelocityMax;
        //                }
        //            }

        //            SetVelocityLimit((Double)velocityLimit, servo, index);
        //        }

        //        if (action.PositionMin is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" positionMin:>{action.PositionMin}<");

        //            SetPositionMin((Double)action.PositionMin, servo, index);
        //        }

        //        if (action.PositionMax is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" positionMax:>{action.PositionMax}<");

        //            SetPositionMax((Double)action.PositionMax, servo, index);
        //        }

        //        // NOTE(crhodes)
        //        // Engage the servo before doing other actions as some,
        //        // e.g. TargetPosition, requires servo to be engaged.

        //        if (action.Engaged is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" engaged:>{action.Engaged}<");

        //            servo.Engaged = (Boolean)action.Engaged;

        //            if ((Boolean)action.Engaged) VerifyServoEngaged(servo, index);
        //        }

        //        //if (action.Acceleration is not null)
        //        //{
        //        //    if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");

        //        //    SetAcceleration((Double)action.Acceleration, servo, index);
        //        //}

        //        if (action.RelativeAcceleration is not null)
        //        {
        //            var newAcceleration = servo.Acceleration + (Double)action.RelativeAcceleration;
        //            if (LogSequenceAction) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

        //            SetAcceleration(newAcceleration, servo, index);
        //        }

        //        //if (action.VelocityLimit is not null)
        //        //{
        //        //    if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

        //        //    SetVelocityLimit((Double)action.VelocityLimit, servo, index);
        //        //}

        //        if (action.RelativeVelocityLimit is not null)
        //        {
        //            var newVelocityLimit = servo.VelocityLimit + (Double)action.RelativeVelocityLimit;
        //            if (LogSequenceAction) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

        //            SetVelocityLimit(newVelocityLimit, servo, index);
        //        }

        //        if (action.TargetPosition is not null)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");

        //            Double targetPosition = (Double)action.TargetPosition;

        //            if (targetPosition < 0)
        //            {
        //                if (action.TargetPosition == -1)        // -1 is magic number for DevicePostionMin :)
        //                { 
        //                    targetPosition = InitialServoLimits[index].DevicePositionMin; 
        //                }
        //                else if (action.TargetPosition == -2)   // -2 is magic number for DevicePostionMax :)
        //                { 
        //                    targetPosition = InitialServoLimits[index].DevicePositionMax;
        //                }
        //            }

        //            VerifyNewPositionAchieved(servo, index, SetPosition(targetPosition, servo, index));
        //        }

        //        if (action.RelativePosition is not null)
        //        {
        //            var newPosition = servo.Position + (Double)action.RelativePosition;
        //            if (LogSequenceAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({newPosition})");

        //            VerifyNewPositionAchieved(servo, index, SetPosition(newPosition, servo, index));                
        //        }

        //        if (action.Duration > 0)
        //        {
        //            if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

        //            Thread.Sleep((Int32)action.Duration);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
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

        private async void TriggerSequence(SequenceEventArgs args)
        {
            long startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var advancedServoSequence = args.AdvancedServoSequence;

            await RunActionLoops(advancedServoSequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //private void VerifyServoEngaged(Phidget22.AdvancedServoServo servo, Int32 index)
        //{
        //    Int64 startTicks = 0;
        //    var msSleep = 0;

        //    try
        //    {
        //        if (LogActionVerification)
        //        {
        //            startTicks = Log.Trace($"Enter servo:{index} engaged:{servo.Engaged}", Common.LOG_CATEGORY);
        //        }

        //        do
        //        {
        //            Thread.Sleep(1);
        //            msSleep++;
        //        } while (servo.Engaged != true);

        //        if (LogActionVerification)
        //        {
        //            Log.Trace($"Exit servo:{index} engaged:{servo.Engaged} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void VerifyNewPositionAchieved(AdvancedServoServo servo, Int32 index, Double targetPosition)
        //{
        //    Int64 startTicks = 0;
        //    var msSleep = 0;

        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            startTicks = Log.Trace($"Enter servo:{index} targetPosition:{targetPosition}", Common.LOG_CATEGORY);
        //        }

        //        //while (servo.Position != targetPosition)
        //        //{
        //        //    Thread.Sleep(1);
        //        //    msSleep++;
        //        //}

        //        // NOTE(crhodes)
        //        // Maybe poll velocity != 0
        //        do
        //        {
        //            if (LogActionVerification) Log.Trace($"servo:{index}" +
        //                $" - velocity:{servo.Velocity,8:0.000} position:{servo.Position,7:0.000}" +
        //                $" - stopped:{servo.Stopped}", Common.LOG_CATEGORY);
        //            Thread.Sleep(1);
        //            msSleep++;
        //        }
        //        while (servo.Position != targetPosition);
        //        // NOTE(crhodes)
        //        // Stopped does not mean we got there.
        //        //while (! servo.Stopped ) ;

        //        if (LogActionVerification)
        //        {
        //            Log.Trace($"Exit servo:{index} servoPosition:{servo.Position,7:0.000} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"servo:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        #endregion
    }
}
