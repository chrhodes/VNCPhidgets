using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Phidget22;
using Phidget22.Events;

using Prism.Events;

using VNC.Phidget.Events;
using VNC.Phidget.Players;

using VNCPhidget22.Configuration;

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
            Stepper = new Phidget22.Stepper();

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

        #region Structures

        public struct StepperMinMax
        {
            public Double StepAngle;

            public Double AccelerationMin;
            public Double AccelerationMax;

            public Int64 PositionMin;
            public Int64 PositionMax;

            public Double VelocityMin;
            public Double VelocityMax;
        }

        #endregion

        #region Fields and Properties

        public Phidget22.Stepper Stepper = null;

        private bool _logPositionChangeEvents;
        public bool LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value) return;

                if (_logPositionChangeEvents = value)
                {
                    Stepper.PositionChange += Stepper_PositionChange;
                }
                else
                {
                    Stepper.PositionChange -= Stepper_PositionChange;
                }
            }
        }

        private bool _logVelocityChangeEvents;
        public bool LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value) return;

                if (_logVelocityChangeEvents = value)
                {
                    Stepper.VelocityChange += Stepper_VelocityChange;
                }
                else
                {
                    Stepper.VelocityChange -= Stepper_VelocityChange;
                }
            }
        }

        private bool _logCurrentChangeEvents;
        public bool LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value) return;

                if (_logCurrentChangeEvents = value)
                {
                    Stepper.CurrentChange += Stepper_CurrentChange;
                }
                else
                {
                    Stepper.CurrentChange -= Stepper_CurrentChange;
                }
            }
        }

        private bool _logInputChangeEvents;
        public bool LogInputChangeEvents
        {
            get => _logInputChangeEvents;
            set
            {
                if (_logInputChangeEvents == value) return;

                if (_logInputChangeEvents = value)
                {
                    Stepper.InputChange += Stepper_InputChange;
                }
                else
                {
                    Stepper.InputChange -= Stepper_InputChange;
                }
            }
        }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        public StepperMinMax[] InitialStepperLimits { get; set; } = new StepperMinMax[8];

        #endregion

        #region Event Handlers

        private void Stepper_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            try
            {
                Phidget22.Stepper Stepper = sender as Phidget22.Stepper;
                Phidget22.StepperStepper stepper = Stepper.steppers[e.Index];
                Log.EVENT_HANDLER($"CurrentChange {Stepper.Address},{Stepper.SerialNumber},servo:{e.Index}" +
                    $" - current:{e.Current:00.000} - stopped:{stepper.Stopped}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Stepper_PositionChange(object sender, StepperPositionChangeEventArgs e)
        {            
            try
            {
                Phidget22.Stepper Stepper = sender as Phidget22.Stepper;
                Phidget22.StepperStepper stepper = Stepper.steppers[e.Index];
                Log.EVENT_HANDLER($"PositionChange {Stepper.Address},{Stepper.SerialNumber},servo:{e.Index}" +
                    $" - velocity:{stepper.Velocity,8:0.000} position:{e.Position,7:0.000} current:{stepper.Current:00.000}" +
                    $" - stopped:{stepper.Stopped} ", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Stepper_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            try
            {
                Phidget22.Stepper Stepper = sender as Phidget22.Stepper;
                Phidget22.StepperStepper stepper = Stepper.steppers[e.Index];
                Log.EVENT_HANDLER($"VelocityChange {Stepper.Address},{Stepper.SerialNumber},servo:{e.Index}" +
                    $" - velocity:{e.Velocity,8:0.000} position:{stepper.CurrentPosition,7:0.000} current:{stepper.Current:00.000}" +
                    $" - stopped:{stepper.Stopped}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

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

        //private void Stepper_OutputChange(object sender, Phidget22.Events.OutputChangeEventArgs e)
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

        private void Stepper_InputChange(object sender, InputChangeEventArgs e)
        {
            try
            {
                Phidget22.Stepper stepper = (Phidget22.Stepper)sender;
                Log.Trace($"Stepper_InputChange {stepper.Address},{stepper.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

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

                            foreach (StepperAction action in stepperSequence.Actions)
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

        ///// <summary>
        ///// Bounds check and set position
        ///// </summary>
        ///// <param name="positionMin"></param>
        ///// <param name="stepper"></param>
        //public void SetPositionMin(Double positionMin, StepperStepper stepper, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin index:{index} positionMin:{positionMin}" +
        //                $" stepper.PositionMin:{stepper.PositionMin}" +
        //                $" stepper.PositionMax:{stepper.PositionMax}" +
        //                $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
        //                $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //        }

        //        if (positionMin < 0)
        //        {
        //            positionMin = InitialStepperLimits[index].DevicePositionMin;
        //        }
        //        else if (positionMin < InitialStepperLimits[index].DevicePositionMin)
        //        {
        //            positionMin = InitialStepperLimits[index].DevicePositionMin;
        //        }
        //        else if (positionMin > stepper.PositionMax)
        //        {
        //            positionMin = stepper.PositionMax;
        //        }

        //        if (stepper.PositionMin != positionMin) stepper.PositionMin = positionMin;

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End index:{index} positionMin:{positionMin} stepper.PositionMin:{stepper.PositionMin}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"stepper:{index} {pex.Description} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //        Log.Error($"index:{index} positionMin:{positionMin}" +
        //            $" stepper.PositionMin:{stepper.PositionMin}" +
        //            $" stepper.PositionMax:{stepper.PositionMax}" +
        //            $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
        //            $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        /// <summary>
        /// Bounds check and set CurrentPosition
        /// </summary>
        /// <param name="position"></param>
        /// <param name="stepper"></param>
        public Int64 SetCurrentPosition(Int64 position, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin stepper:{index} position:{position}" +
                        $" stepper.PositionMin:{stepper.PositionMin}" +
                        $" stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
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
                if (stepper.CurrentPosition != position) stepper.CurrentPosition = position;

                if (LogSequenceAction)
                {
                    Log.Trace($"End stepper:{index} position:{position}"
                        + $" stepper.CurrentPosition:{stepper.CurrentPosition}"
                        + $" stepper.TargetPosition:{stepper.TargetPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} stepper.CurrentPosition:{stepper.CurrentPosition}" +
                    $" stepper.TargetPosition:{stepper.TargetPosition}" +
                    $" stepper.PositionMin:{stepper.PositionMin}" +
                    $" stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return position;
        }

        /// <summary>
        /// Bounds check and set TargetPosition
        /// </summary>
        /// <param name="position"></param>
        /// <param name="stepper"></param>
        public Int64 SetTargetPosition(Int64 position, StepperStepper stepper, Int32 index)
        {
            try
            {
                if (LogSequenceAction)
                {
                    Log.Trace($"Begin stepper:{index} position:{position}" +
                        $" stepper.PositionMin:{stepper.PositionMin}" +
                        $" stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
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
                // Maybe save last TargetPosition set and not bother checking stepper.Position is same
                if (stepper.TargetPosition != position) stepper.TargetPosition = position;

                if (LogSequenceAction)
                {
                    Log.Trace($"End stepper:{index} position:{position}"
                        + $" stepper.CurrentPosition:{stepper.CurrentPosition}"
                        + $" stepper.TargetPosition:{stepper.TargetPosition}", Common.LOG_CATEGORY);
                }
            }
            catch (PhidgetException pex)
            {
                Log.Error(pex, Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
                Log.Error($"stepper:{index} stepper.CurrentPosition:{stepper.CurrentPosition}" +
                    $" stepper.TargetPosition:{stepper.TargetPosition}" +
                    $" stepper.PositionMin:{stepper.PositionMin}" +
                    $" stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return position;
        }

        ///// <summary>
        ///// Bounds check and set position
        ///// </summary>
        ///// <param name="positionMax"></param>
        ///// <param name="stepper"></param>
        //public void SetPositionMax(Double positionMax, StepperStepper stepper, Int32 index)
        //{
        //    try
        //    {
        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"Begin stepper:{index} positionMax:{positionMax}" +
        //                $" stepper.PositionMin:{stepper.PositionMin}" +
        //                $" stepper.PositionMax:{stepper.PositionMax}" +
        //                $" DevicePositionMin:{InitialStepperLimits[index].DevicePositionMin}" +
        //                $" DevicePositionMax:{InitialStepperLimits[index].DevicePositionMax}", Common.LOG_CATEGORY);
        //        }

        //        if (positionMax < 0)
        //        {
        //            positionMax = InitialStepperLimits[index].DevicePositionMax;
        //        }
        //        else if (positionMax < stepper.PositionMin)
        //        {
        //            positionMax = stepper.PositionMin;
        //        }
        //        else if (positionMax > InitialStepperLimits[index].DevicePositionMax)
        //        {
        //            positionMax = InitialStepperLimits[index].DevicePositionMax;
        //        }

        //        if (stepper.PositionMax != positionMax) stepper.PositionMax = positionMax;

        //        if (LogSequenceAction)
        //        {
        //            Log.Trace($"End stepper:{index} positionMax:{positionMax} stepper.PositionMax:{stepper.PositionMax}", Common.LOG_CATEGORY);
        //        }
        //    }
        //    catch (PhidgetException pex)
        //    {
        //        Log.Error(pex, Common.LOG_CATEGORY);
        //        Log.Error($"stepper:{index} source:{pex.Source} type:{pex.Type} inner:{pex.InnerException}", Common.LOG_CATEGORY);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        private void SaveStepperLimits(StepperStepper stepper, Int32 index)
        {
            //if (LogPhidgetEvents)
            //{
            //    Log.Trace($"servo:{index} type:{servo.Type} engaged:{servo.Engaged}" +
            //        $" accelerationMin:{servo.AccelerationMin} accelerationMax:{servo.AccelerationMax}" +
            //        $" positionMin:{servo.PositionMin} positionMax:{servo.PositionMax}" +
            //        $" velocityMin:{servo.VelocityMin} velocityMax:{servo.VelocityMax}", Common.LOG_CATEGORY);
            //}

            // NOTE(crhodes)
            // We do not need to save Acceleration Min,Max and Velocity Min,Max,
            // they cannot change, but, useful to have available
            // when setting Acceleration/VelocityLimit to Min/Max in PerformAction

            //InitialServoLimits[index].AccelerationMax = servo.AccelerationMax;
            //InitialServoLimits[index].DevicePositionMin = servo.PositionMin;
            ////InitialServoLimits[i].PositionMin = servo.PositionMin; 
            ////InitialServoLimits[i].PositionMax = servo.PositionMax;
            //InitialServoLimits[index].DevicePositionMax = servo.PositionMax;
            //InitialServoLimits[index].VelocityMin = servo.VelocityMin + 1; // 0 won't move
            //InitialServoLimits[index].VelocityMax = servo.VelocityMax;
        }

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
                if (action.StepAngle is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" stepAngle:>{action.StepAngle}<");

                    //stepper.Type = (Phidget22.stepperstepper.stepperType)action.stepperType;

                    // NOTE(crhodes)
                    // Maybe we should sleep for a little bit to allow this to happen
                    //Thread.Sleep(1);

                    InitialStepperLimits[index].StepAngle = (Double)action.StepAngle;
                    // Save the refreshed values
                    //SaveStepperLimits(stepper, index);
                }

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

                //if (action.PositionMin is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" positionMin:>{action.PositionMin}<");

                //    SetPositionMin((Double)action.PositionMin, stepper, index);
                //}

                //if (action.PositionMax is not null)
                //{
                //    if (LogSequenceAction) actionMessage.Append($" positionMax:>{action.PositionMax}<");

                //    SetPositionMax((Double)action.PositionMax, stepper, index);
                //}

                // NOTE(crhodes)
                // Engage the stepper before doing other actions as some,
                // e.g. TargetPosition, requires stepper to be engaged.

                if (action.Engaged is not null)
                {
                    if (LogSequenceAction) actionMessage.Append($" engaged:>{action.Engaged}<");

                    stepper.Engaged = (Boolean)action.Engaged;

                    if ((Boolean)action.Engaged) VerifyStepperEngaged(stepper, index);
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

                    Int64 targetPosition = (Int64)action.TargetPosition;

                    if (targetPosition < 0)
                    {
                        if (action.TargetPosition == -1)        // -1 is magic number for DevicePostionMin :)
                        {
                            targetPosition = InitialStepperLimits[index].PositionMin;
                        }
                        else if (action.TargetPosition == -2)   // -2 is magic number for DevicePostionMax :)
                        {
                            targetPosition = InitialStepperLimits[index].PositionMax;
                        }
                    }

                    VerifyNewPositionAchieved(stepper, index, SetTargetPosition(targetPosition, stepper, index));
                }

                if (action.RelativeTargetPosition is not null)
                {
                    var newPosition = stepper.TargetPosition + (Int64)action.RelativeTargetPosition;
                    if (LogSequenceAction) actionMessage.Append($" relativeTargetPosition:>{action.RelativeTargetPosition}< ({newPosition})");

                    VerifyNewPositionAchieved(stepper, index, SetTargetPosition(newPosition, stepper, index));
                }

                if (action.RelativeTargetDegrees is not null)
                {
                    // TODO(crhodes)
                    // Do math using StepAngle
                    // Check if StepAngle null.  Can't do relative without
                    // Maybe we can save off above and just retrieve

                    Double stepAngle = InitialStepperLimits[index].StepAngle;
                    //Double stepAngle = (Double)action.StepAngle;
                    Int64 degrees = (Int64)action.RelativeTargetDegrees;

                    Int64 stepsToMove = (Int64)(degrees / stepAngle);

                    stepsToMove = stepsToMove * 16; // 1/16 steps

                    var newPosition = stepper.TargetPosition + stepsToMove;
                    if (LogSequenceAction) actionMessage.Append($" relativeTargetDegrees:>{action.RelativeTargetDegrees}< ({newPosition})");

                    VerifyNewPositionAchieved(stepper, index, SetTargetPosition(newPosition, stepper, index));
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

        private void VerifyStepperEngaged(Phidget22.StepperStepper stepper, Int32 index)
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
        private void VerifyNewPositionAchieved(Phidget22.StepperStepper stepper, Int32 index, double targetPosition)
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
