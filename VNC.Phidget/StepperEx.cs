﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidgets;

using Prism.Events;

using VNC.Phidget.Events;
using VNC.Phidget.Players;

using VNCPhidget21.Configuration;

using static VNC.Phidget.AdvancedstepperEx;

namespace VNC.Phidget
{
    public class StepperEx : PhidgetEx
    {
        #region Constructors, Initialization, and Load

        public readonly IEventAggregator EventAggregator;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public StepperEx(string ipAddress, int port, int serialNumber, IEventAggregator eventAggregator)
            : base(ipAddress, port, serialNumber)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter ipAdress:{ipAddress} port:{port} serialNumber:{serialNumber}", Common.LOG_CATEGORY);


            EventAggregator = eventAggregator;
            InitializePhidget();

            EventAggregator.GetEvent<StepperSequenceEvent>().Subscribe(TriggerSequence);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidget()
        {
            Stepper = new Phidgets.Stepper();

            this.Stepper.Attach += Phidget_Attach;
            this.Stepper.Detach += Phidget_Detach;
            this.Stepper.Error += Phidget_Error;
            this.Stepper.ServerConnect += Phidget_ServerConnect;
            this.Stepper.ServerDisconnect += Phidget_ServerDisconnect;

            //this.InputChange += Stepper_InputChange;
            //this.OutputChange += Stepper_OutputChange;
            //this.SensorChange += Stepper_SensorChange;
        }

        #endregion

        #region Enums (None)


        #endregion

        #region 

        public struct StepperMinMax
        {
            //public enum LimitType
            //{
            //    //AccelerationMin,
            //    //AccelerationMax,
            //    DevicePositionMin,
            //    PositionMin,
            //    PositionMax,
            //    DevicePositionMax
            //    //VelocityMin,
            //    //VelocityMax,
            //}

            public Double AccelerationMin;
            public Double AccelerationMax;
            public Double DevicePositionMin;
            //public Double PositionMin;
            //public Double PositionMax;
            public Double DevicePositionMax;
            public Double VelocityMin;
            public Double VelocityMax;
        }

        #endregionctur

        #region Fields and Properties

        public Phidgets.Stepper Stepper = null;

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        public StepperMinMax[] InitialStepperLimits { get; set; } = new StepperMinMax[8];

        #endregion

        #region Commands (None)

        #endregion

        #region Event Handlers

        //private void Stepper_SensorChange(object sender, SensorChangeEventArgs e)
        //{
        //    if (LogSensorChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    if (LogOutputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void Stepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    if (LogInputChangeEvents)
        //    {
        //        try
        //        {
        //            InterfaceKit ifk = (InterfaceKit)sender;
        //            var a = e;
        //            var b = e.GetType();
        //            Log.Trace($"Stepper_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

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
                Stepper.open(SerialNumber, Host.IPAddress, Host.Port);

                if (timeOut is not null) { Stepper.waitForAttachment((Int32)timeOut); }
                else { Stepper.waitForAttachment(); }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void Close()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            try
            {
                this.Stepper.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public async Task RunActionLoops(StepperSequence stepperSequence)
        {
            Int64 startTicks = 0;

            try
            {
                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace(
                        $"Running Action Loops" +
                        $" stepperSequence:>{stepperSequence.Name}<" +
                        $" startActionLoopSequences:>{stepperSequence.StartActionLoopSequences?.Count()}<" +
                        $" actionLoops:>{stepperSequence.ActionLoops}<" +
                        $" actions:>{stepperSequence.Actions?.Count()}<" +
                        $" actionsDuration:>{stepperSequence.ActionsDuration}<" +
                        $" endActionLoopSequences:>{stepperSequence.EndActionLoopSequences?.Count()}<", Common.LOG_CATEGORY);
                }

                if (stepperSequence.Actions is not null)
                {
                    for (int actionLoop = 0; actionLoop < stepperSequence.ActionLoops; actionLoop++)
                    {
                        if (stepperSequence.StartActionLoopSequences is not null)
                        {
                            // TODO(crhodes)
                            // May want to create a new player instead of reaching for the property.

                            PerformanceSequencePlayer player = PerformanceSequencePlayer.ActivePerformanceSequencePlayer;
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in stepperSequence.StartActionLoopSequences)
                            {
                                await player.ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (stepperSequence.ExecuteActionsInParallel)
                        {
                            if (LogSequenceAction) Log.Trace($"Parallel Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            Parallel.ForEach(stepperSequence.Actions, async action =>
                            {
                                await PerformAction(action);
                            });
                        }
                        else
                        {
                            if (LogSequenceAction) Log.Trace($"Sequential Actions Loop:>{actionLoop + 1}<", Common.LOG_CATEGORY);

                            foreach (AdvancedstepperstepperAction action in stepperSequence.Actions)
                            {
                                await PerformAction(action);
                            }
                        }

                        if (stepperSequence.ActionsDuration is not null)
                        {
                            if (LogSequenceAction)
                            {
                                Log.Trace($"Zzzzz Action:>{stepperSequence.ActionsDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)stepperSequence.ActionsDuration);
                        }

                        if (stepperSequence.EndActionLoopSequences is not null)
                        {
                            PerformanceSequencePlayer player = new PerformanceSequencePlayer(EventAggregator);
                            player.LogPerformanceSequence = LogPerformanceSequence;
                            player.LogSequenceAction = LogSequenceAction;

                            foreach (PerformanceSequence sequence in stepperSequence.EndActionLoopSequences)
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


        /// <summary>
        /// Bounds check and set acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="stepper"></param>
        public void SetAcceleration(Double acceleration, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin index:{index} acceleration:{acceleration}" +
                        $" accelerationMin:{stepper.AccelerationMin}" +
                        //$" acceleration:{stepper.Acceleration}" + // Can't check this as it may not have been set yet
                        $" accelerationMax:{stepper.AccelerationMax}", Common.LOG_CATEGORY);
                }

                if (acceleration < stepper.AccelerationMin)
                {
                    stepper.Acceleration = stepper.AccelerationMin;
                }
                else if (acceleration > stepper.AccelerationMax)
                {
                    stepper.Acceleration = stepper.AccelerationMax;
                }
                else
                {
                    stepper.Acceleration = acceleration;
                }

                if (LogSequenceAction)
                {
                    Log.Trace($"End index:{index} stepperAcceleration:{stepper.Acceleration}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index} acceleration:{acceleration}" +
                    $" accelerationMin:{stepper.AccelerationMin}" +
                    //$" acceleration:{stepper.Acceleration}" + // Can't check this as it may not have been set yet
                    $" accelerationMax:{stepper.AccelerationMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set velocity
        /// </summary>
        /// <param name="velocityLimit"></param>
        /// <param name="stepper"></param>
        public void SetVelocityLimit(Double velocityLimit, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin index:{index}" +
                        $" velocityLimit:{velocityLimit}" +
                        $" stepper.velocityMin:{stepper.VelocityMin}" +
                        $" stepper.velocityLimit:{stepper.VelocityLimit}" +
                        $" stepper.velocityMax:{stepper.VelocityMax}", Common.LOG_CATEGORY);
                }

                if (velocityLimit < stepper.VelocityMin)
                {
                    stepper.VelocityLimit = stepper.VelocityMin;
                }
                else if (velocityLimit > stepper.VelocityMax)
                {
                    stepper.VelocityLimit = stepper.VelocityMax;
                }
                else
                {
                    stepper.VelocityLimit = velocityLimit;
                }

                if (LogSequenceAction)
                {
                    Log.Trace($"End index:{index} velocityLimit:{velocityLimit} velocityLimit:{stepper.VelocityLimit}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index}" +
                    $" velocity:{velocityLimit}" +
                    $" stepper.velocityMin:{stepper.VelocityMin}" +
                    $" stepper.velocityLimit:{stepper.VelocityLimit}" +
                    $" stepper.velocityMax:{stepper.VelocityMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMin"></param>
        /// <param name="stepper"></param>
        public void SetPositionMin(Double positionMin, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin index:{index} positionMin:{positionMin}" +
                        $" stepper.PositionMin:{stepper.PositionMin}" +
                        $" stepper.PositionMax:{stepper.PositionMax}" +
                        $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (positionMin < 0)
                {
                    positionMin = InitialStepperLimits[index].DevicePositionMin;
                }
                else if (positionMin < InitialStepperLimits[index].DevicePositionMin)
                {
                    positionMin = InitialStepperLimits[index].DevicePositionMin;
                }
                else if (positionMin > stepper.PositionMax)
                {
                    positionMin = stepper.PositionMax;
                }

                if (stepper.PositionMin != positionMin) stepper.PositionMin = positionMin;

                if (LogSequenceAction)
                {
                    Log.Trace($"End index:{index} positionMin:{positionMin} stepper.PositionMin:{stepper.PositionMin}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} {pex.Description} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"index:{index} positionMin:{positionMin}" +
                    $" stepper.PositionMin:{stepper.PositionMin}" +
                    $" stepper.PositionMax:{stepper.PositionMax}" +
                    $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
                    $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="stepper"></param>
        public Double SetPosition(Double position, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin stepper:{index} position:{position}" +
                        $" stepper.PositionMin:{stepper.PositionMin}" +
                        $" stepper.PositionMax:{stepper.PositionMax}" +
                        $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (position < stepper.PositionMin)
                {
                    position = stepper.PositionMin;
                }
                else if (position > stepper.PositionMax)
                {
                    position = stepper.PositionMax;
                }

                // TODO(crhodes)
                // Maybe save last position set and not bother checking stepper.Position is same
                if (stepper.Position != position) stepper.Position = position;

                if (LogSequenceAction)
                {
                    Log.Trace($"End stepper:{index} position:{position} stepper.Position:{stepper.Position}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} stepper.position:{stepper.Position}" +
                    $" stepper.PositionMin:{stepper.PositionMin}" +
                    $" stepper.PositionMax:{stepper.PositionMax}" +
                    $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
                    $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return position;
        }

        /// <summary>
        /// Bounds check and set position
        /// </summary>
        /// <param name="positionMax"></param>
        /// <param name="stepper"></param>
        public void SetPositionMax(Double positionMax, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin stepper:{index} positionMax:{positionMax}" +
                        $" stepper.PositionMin:{stepper.PositionMin}" +
                        $" stepper.PositionMax:{stepper.PositionMax}" +
                        $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
                        $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
                }

                if (positionMax < 0)
                {
                    positionMax = InitialStepperLimits[index].DevicePositionMax;
                }
                else if (positionMax < stepper.PositionMin)
                {
                    positionMax = stepper.PositionMin;
                }
                else if (positionMax > InitialStepperLimits[index].DevicePositionMax)
                {
                    positionMax = InitialStepperLimits[index].DevicePositionMax;
                }

                if (stepper.PositionMax != positionMax) stepper.PositionMax = positionMax;

                if (LogSequenceAction)
                {
                    Log.Trace($"End stepper:{index} positionMax:{positionMax} stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
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

        private async Task PerformAction(StepperAction action)
        {
            Int64 startTicks = 0;

            Int32 index = action.StepperIndex;

            StringBuilder actionMessage = new StringBuilder();

            if (LogSequenceAction)
            {
                startTicks = Log.Trace($"Enter stepper:{index}", Common.LOG_CATEGORY);
                actionMessage.Append($"stepper:{index}");
            }

            StepperStepper stepper = Stepper.steppers[index];

            try
            {
                //if (action.stepperType is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" stepperType:>{action.stepperType}<");

                //    stepper.Type = (Phidgets.stepperstepper.stepperType)action.stepperType;

                //    // NOTE(crhodes)
                //    // Maybe we should sleep for a little bit to allow this to happen
                //    Thread.Sleep(1);


                //    // Save the refreshed values
                //    SavestepperLimits(stepper, index);
                //}

                // NOTE(crhodes)
                // These can be performed without the stepper being engaged.  This helps address
                // previous values that get applied when stepper engaged.
                // The stepper still snaps to last position when enabled.  No known way to address that.

                if (action.Acceleration is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");
                    var acceleration = action.Acceleration;

                    if (acceleration < 0)
                    {
                        if (acceleration == -1)        // -1 is magic number for AccelerationMin :)
                        {
                            acceleration = InitialStepperLimits[index].AccelerationMin;
                        }
                        else if (acceleration == -2)   // -2 is magic number for AccelerationMax :)
                        {
                            acceleration = InitialStepperLimits[index].AccelerationMax;
                        }
                    }

                    SetAcceleration((Double)acceleration, stepper, index);
                }

                if (action.VelocityLimit is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");
                    var velocityLimit = action.VelocityLimit;

                    if (velocityLimit < 0)
                    {
                        if (velocityLimit == -1)        // -1 is magic number for VelocityMin :)
                        {
                            velocityLimit = InitialStepperLimits[index].VelocityMin;
                        }
                        else if (velocityLimit == -2)   // -2 is magic number for VelocityMax :)
                        {
                            velocityLimit = InitialStepperLimits[index].VelocityMax;
                        }
                    }

                    SetVelocityLimit((Double)velocityLimit, stepper, index);
                }

                if (action.PositionMin is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" positionMin:>{action.PositionMin}<");

                    SetPositionMin((Double)action.PositionMin, stepper, index);
                }

                if (action.PositionMax is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" positionMax:>{action.PositionMax}<");

                    SetPositionMax((Double)action.PositionMax, stepper, index);
                }

                // NOTE(crhodes)
                // Engage the stepper before doing other actions as some,
                // e.g. TargetPosition, requires stepper to be engaged.

                if (action.Engaged is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" engaged:>{action.Engaged}<");

                    stepper.Engaged = (Boolean)action.Engaged;

                    if ((Boolean)action.Engaged) VerifystepperEngaged(stepper, index);
                }

                //if (action.Acceleration is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" acceleration:>{action.Acceleration}<");

                //    SetAcceleration((Double)action.Acceleration, stepper, index);
                //}

                if (action.RelativeAcceleration is not null)
                {
                    var newAcceleration = stepper.Acceleration + (Double)action.RelativeAcceleration;
                    if (LogSequenceAction) actionMessage.Append($" relativeAcceleration:>{action.RelativeAcceleration}< ({newAcceleration})");

                    SetAcceleration(newAcceleration, stepper, index);
                }

                //if (action.VelocityLimit is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" velocityLimit:>{action.VelocityLimit}<");

                //    SetVelocityLimit((Double)action.VelocityLimit, stepper, index);
                //}

                if (action.RelativeVelocityLimit is not null)
                {
                    var newVelocityLimit = stepper.VelocityLimit + (Double)action.RelativeVelocityLimit;
                    if (LogSequenceAction) actionMessage.Append($" relativeVelocityLimit:>{action.RelativeVelocityLimit}< ({newVelocityLimit})");

                    SetVelocityLimit(newVelocityLimit, stepper, index);
                }

                if (action.TargetPosition is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" targetPosition:>{action.TargetPosition}<");

                    Double targetPosition = (Double)action.TargetPosition;

                    if (targetPosition < 0)
                    {
                        if (action.TargetPosition == -1)        // -1 is magic number for DevicePostionMin :)
                        {
                            targetPosition = InitialStepperLimits[index].DevicePositionMin;
                        }
                        else if (action.TargetPosition == -2)   // -2 is magic number for DevicePostionMax :)
                        {
                            targetPosition = InitialStepperLimits[index].DevicePositionMax;
                        }
                    }

                    VerifyNewPositionAchieved(stepper, index, SetPosition(targetPosition, stepper, index));
                }

                if (action.RelativePosition is not null)
                {
                    var newPosition = stepper.Position + (Double)action.RelativePosition;
                    if (LogSequenceAction) actionMessage.Append($" relativePosition:>{action.RelativePosition}< ({newPosition})");

                    VerifyNewPositionAchieved(stepper, index, SetPosition(newPosition, stepper, index));
                }

                if (action.Duration > 0)
                {
                    if (LogSequenceAction) actionMessage.Append($" duration:>{action.Duration}<");

                    Thread.Sleep((Int32)action.Duration);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
            finally
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Exit {actionMessage}", Common.LOG_CATEGORY, startTicks);
                }
            }
        }

        private async void TriggerSequence(SequenceEventArgs args)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            var stepperSequence = args.stepperSequence;

            await RunActionLoops(stepperSequence);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void VerifyStepperEngaged(Phidgets.StepperStepper stepper, Int32 index)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogActionVerification)
                {
                    startTicks = Log.Trace($"Enter stepper:{index} engaged:{stepper.Engaged}", Common.LOG_CATEGORY);
                }

                do
                {
                    Thread.Sleep(1);
                    msSleep++;
                } while (stepper.Engaged != true);

                if (LogActionVerification)
                {
                    Log.Trace($"Exit stepper:{index} engaged:{stepper.Engaged} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        /// <summary>
        /// This may not be needed for a Stepper
        /// </summary>
        /// <param name="stepper"></param>
        /// <param name="index"></param>
        /// <param name="targetPosition"></param>
        private void VerifyNewPositionAchieved(Phidgets.StepperStepper stepper, Int32 index, double targetPosition)
        {
            Int64 startTicks = 0;
            var msSleep = 0;

            try
            {
                if (LogSequenceAction)
                {
                    startTicks = Log.Trace($"Enter stepper:{index} targetPosition:{targetPosition}", Common.LOG_CATEGORY);
                }

                //while (stepper.Position != targetPosition)
                //{
                //    Thread.Sleep(1);
                //    msSleep++;
                //}

                // NOTE(crhodes)
                // Maybe poll velocity != 0
                do
                {
                    if (LogActionVerification) Log.Trace($"stepper:{index}" +
                        $" - velocity:{stepper.Velocity,8:0.000} position:{stepper.CurrentPosition,7:0.000}" +
                        $" - stopped:{stepper.Stopped}", Common.LOG_CATEGORY);
                    Thread.Sleep(1);
                    msSleep++;
                }
                while (stepper.CurrentPosition != targetPosition);
                // NOTE(crhodes)
                // Stopped does not mean we got there.
                //while (! stepper.Stopped ) ;

                if (LogActionVerification)
                {
                    Log.Trace($"Exit stepper:{index} stepperPosition:{stepper.CurrentPosition,7:0.000} ms:{msSleep}", Common.LOG_CATEGORY, startTicks);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion
    }
}
