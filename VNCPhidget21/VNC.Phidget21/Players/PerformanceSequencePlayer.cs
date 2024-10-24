using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget.Players
{
    // TODO(crhodes)
    // Figure out how to make this singleton
    // Maybe as simple as creating one if needed 
    // in the get of ActivePerformanceSequencePlayer

    public class PerformanceSequencePlayer
    {
        #region Constructors, Initialization, and Load

        public IEventAggregator EventAggregator { get; set; }

        public PerformanceSequencePlayer(IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;

            ActivePerformanceSequencePlayer = this;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public static PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        public AdvancedServoEx ActiveAdvancedServoHost { get; set; }
        public InterfaceKitEx ActiveInterfaceKitHost { get; set; }
        public StepperEx ActiveStepperHost { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        // AdvancedServo events

        public bool LogCurrentChangeEvents { get; set; }
        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }

        // InterfaceKit events

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        // Phidget Events

        public bool LogPhidgetEvents { get; set; }

        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public async Task ExecutePerformanceSequence(PerformanceSequence performanceSequence)
        {
            Int64 startTicks = 0;

            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace($"Executing Performance Sequence" +
                        $" name:>{performanceSequence?.Name}<" +
                        $" type:>{performanceSequence?.SequenceType}<" +
                        $" loops:>{performanceSequence?.SequenceLoops}<" +
                        $" duration:>{performanceSequence?.Duration}<" +
                        $" closePhidget:>{performanceSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                for (int sequenceLoop = 0; sequenceLoop < performanceSequence.SequenceLoops; sequenceLoop++)
                {
                    if (LogPerformanceSequence) Log.Trace($"Running PerformanceSequence Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                    // NOTE(crhodes)
                    // Each loop starts back at the initial sequence
                    nextPerformanceSequence = performanceSequence;

                    do
                    {
                        switch (nextPerformanceSequence.SequenceType)
                        {
                            case "AS":
                                nextPerformanceSequence = await ExecuteAdvancedServoPerformanceSequence(nextPerformanceSequence);
                                break;

                            case "IK":
                                nextPerformanceSequence = await ExecuteInterfaceKitPerformanceSequence(nextPerformanceSequence);
                                break;

                            case "ST":
                                nextPerformanceSequence = await ExecuteStepperPerformanceSequence(nextPerformanceSequence);
                                break;

                            default:
                                Log.Error($"Unsupported SequenceType:>{nextPerformanceSequence.SequenceType}<", Common.LOG_CATEGORY);
                                nextPerformanceSequence = null;
                                break;
                        }
                    } while (nextPerformanceSequence is not null);
                }

                if (performanceSequence.Duration is not null)
                {
                    if (LogPerformanceSequence)
                    {
                        Log.Trace($"Zzzzz Sleeping:>{performanceSequence.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((int)performanceSequence.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogPerformanceSequence)
            {
                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
        }

        private async Task<PerformanceSequence> ExecuteAdvancedServoPerformanceSequence(PerformanceSequence performanceSequence)
        {
            Int64 startTicks = 0;
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                AdvancedServoEx phidgetHost = null;

                if (PerformanceLibrary.AvailableAdvancedServoSequences.ContainsKey(performanceSequence.Name ?? ""))
                {
                    var advancedServoSequence = PerformanceLibrary.AvailableAdvancedServoSequences[performanceSequence.Name];

                    if (LogPerformanceSequence)
                    {
                        startTicks = Log.Trace($"Executing AS Performance Sequence" +
                            //$" serialNumber:>{advancedServoSequence?.SerialNumber}<" +
                            $" serialNumber:>{performanceSequence?.SerialNumber}<" +
                            $" name:>{advancedServoSequence?.Name}<" +
                            $" sequenceLoops:>{advancedServoSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{advancedServoSequence?.BeforeActionLoopSequences?.Count()}<" +
                            $" startActionLoopSequences:>{advancedServoSequence?.StartActionLoopSequences?.Count()}<" +
                            $" actionLoops:>{advancedServoSequence?.ActionLoops}<" +
                            $" executeActionsInParallel:>{advancedServoSequence?.ExecuteActionsInParallel}<" +
                            $" actionDuration:>{advancedServoSequence?.ActionsDuration}<" +
                            $" endActionLoopSequences:>{advancedServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{advancedServoSequence?.AfterActionLoopSequences?.Count()}<" +
                            $" sequenceDuration:>{advancedServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{advancedServoSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    //if (advancedServoSequence.SerialNumber is not null)
                    //{
                    //    phidgetHost = GetAdvancedServoHost((int)advancedServoSequence.SerialNumber);
                    //}
                    //else if (ActiveAdvancedServoHost is not null)
                    //{
                    //    phidgetHost = ActiveAdvancedServoHost;
                    //}
                    //else
                    //{
                    //    Log.Error($"Cannot locate host to execute SerialNumber:{advancedServoSequence.SerialNumber}", Common.LOG_CATEGORY);
                    //    nextPerformanceSequence = null;
                    //}

                    phidgetHost = GetAdvancedServoHost((int)performanceSequence.SerialNumber);

                    if (phidgetHost == null) 
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (advancedServoSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in advancedServoSequence.BeforeActionLoopSequences)
                            {
                                await ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(advancedServoSequence);

                        if (advancedServoSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in advancedServoSequence.AfterActionLoopSequences)
                            {
                                await ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (advancedServoSequence.SequenceDuration is not null)
                        {
                            if (LogPerformanceSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{advancedServoSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)advancedServoSequence.SequenceDuration);
                        }

                        nextPerformanceSequence = advancedServoSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Error($"Cannot find performanceSequence:>{performanceSequence.Name}<", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }

                if (LogPerformanceSequence) Log.Trace($"Exit nextPerformanceSequence:{nextPerformanceSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteInterfaceKitPerformanceSequence(PerformanceSequence performanceSequence)
        {
            Int64 startTicks = 0;
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                InterfaceKitEx phidgetHost = null;

                if (PerformanceLibrary.AvailableInterfaceKitSequences.ContainsKey(performanceSequence.Name))
                {
                    var interfaceKitSequence = PerformanceLibrary.AvailableInterfaceKitSequences[performanceSequence.Name];

                    if (LogPerformanceSequence)
                    {
                        startTicks = Log.Trace($"Executing IK Performance Sequence" +
                            $" serialNumber:>{performanceSequence?.SerialNumber}<" +
                            $" name:>{interfaceKitSequence?.Name}<" +
                            $" sequenceLoops:>{interfaceKitSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{interfaceKitSequence?.BeforeActionLoopSequences?.Count()}<" +
                            $" startActionLoopSequences:>{interfaceKitSequence?.StartActionLoopSequences?.Count()}<" +
                            $" actionLoops:>{interfaceKitSequence?.ActionLoops}<" +
                            $" executeActionsInParallel:>{interfaceKitSequence?.ExecuteActionsInParallel}<" +
                            $" actionDuration:>{interfaceKitSequence?.ActionsDuration}<" +
                            $" endActionLoopSequences:>{interfaceKitSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{interfaceKitSequence?.AfterActionLoopSequences?.Count()}<" +
                            $" sequenceDuration:>{interfaceKitSequence?.SequenceDuration}<" +
                            $" nextSequence:>{interfaceKitSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    //if (interfaceKitSequence.SerialNumber is not null)
                    //{
                    //    phidgetHost = GetInterfaceKitHost((int)interfaceKitSequence.SerialNumber);
                    //}
                    //else if (ActiveInterfaceKitHost is not null)
                    //{
                    //    phidgetHost = ActiveInterfaceKitHost;
                    //}
                    //else
                    //{
                    //    Log.Error($"Cannot locate host to execute SerialNumber:{interfaceKitSequence.SerialNumber}", Common.LOG_CATEGORY);
                    //    nextPerformanceSequence = null;
                    //}

                    phidgetHost = GetInterfaceKitHost((int)performanceSequence.SerialNumber);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (interfaceKitSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in interfaceKitSequence.BeforeActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(interfaceKitSequence);

                        if (interfaceKitSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in interfaceKitSequence.AfterActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (interfaceKitSequence.SequenceDuration is not null)
                        {
                            if (LogPerformanceSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{interfaceKitSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)interfaceKitSequence.SequenceDuration);
                        }

                        nextPerformanceSequence = interfaceKitSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Trace($"Cannot find performanceSequence:{performanceSequence.Name}", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }

                if (LogPerformanceSequence) Log.Trace($"Exit nextPerformanceSequence:{nextPerformanceSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextPerformanceSequence;
        }

        private async Task<PerformanceSequence> ExecuteStepperPerformanceSequence(PerformanceSequence performanceSequence)
        {
            Int64 startTicks = 0;
            PerformanceSequence nextPerformanceSequence = null;

            try
            {
                StepperEx phidgetHost = null;

                if (PerformanceLibrary.AvailableStepperSequences.ContainsKey(performanceSequence.Name ?? ""))
                {
                    var stepperSequence = PerformanceLibrary.AvailableStepperSequences[performanceSequence.Name];

                    if (LogPerformanceSequence)
                    {
                        startTicks = Log.Trace($"Executing IK Performance Sequence" +
                            $" serialNumber:>{performanceSequence?.SerialNumber}<" +
                            $" name:>{stepperSequence?.Name}<" +
                            $" sequenceLoops:>{stepperSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{stepperSequence?.BeforeActionLoopSequences?.Count()}<" +
                            $" startActionLoopSequences:>{stepperSequence?.StartActionLoopSequences?.Count()}<" +
                            $" actionLoops:>{stepperSequence?.ActionLoops}<" +
                            $" executeActionsInParallel:>{stepperSequence?.ExecuteActionsInParallel}<" +
                            $" actionDuration:>{stepperSequence?.ActionsDuration}<" +
                            $" endActionLoopSequences:>{stepperSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{stepperSequence?.AfterActionLoopSequences?.Count()}<" +
                            $" sequenceDuration:>{stepperSequence?.SequenceDuration}<" +
                            $" nextSequence:>{stepperSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    //if (stepperSequence.SerialNumber is not null)
                    //{
                    //    phidgetHost = GetStepperHost((int)stepperSequence.SerialNumber);
                    //}
                    //else if (ActiveInterfaceKitHost is not null)
                    //{
                    //    phidgetHost = ActiveStepperHost;
                    //}
                    //else
                    //{
                    //    Log.Error($"Cannot locate host to execute SerialNumber:{stepperSequence.SerialNumber}", Common.LOG_CATEGORY);
                    //    nextPerformanceSequence = null;
                    //}

                    phidgetHost = GetStepperHost((int)performanceSequence.SerialNumber);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (stepperSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in stepperSequence.BeforeActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(stepperSequence);

                        if (stepperSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (PerformanceSequence sequence in stepperSequence.AfterActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (stepperSequence.SequenceDuration is not null)
                        {
                            if (LogPerformanceSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{stepperSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)stepperSequence.SequenceDuration);
                        }

                        nextPerformanceSequence = stepperSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Trace($"Cannot find performanceSequence:{performanceSequence.Name}", Common.LOG_CATEGORY);
                    nextPerformanceSequence = null;
                }

                if (LogPerformanceSequence) Log.Trace($"Exit nextPerformanceSequence:{nextPerformanceSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextPerformanceSequence;
        }

        private AdvancedServoEx GetAdvancedServoHost(int serialNumber)
        {
            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            AdvancedServoEx advancedServoHost = null;

            if (phidgetDevice?.PhidgetEx is not null)
            {
                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost.LogPhidgetEvents = LogPhidgetEvents;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogSequenceAction = LogSequenceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;
            }
            else
            {
                phidgetDevice.PhidgetEx = new AdvancedServoEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    EventAggregator);

                advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

                advancedServoHost.LogPhidgetEvents = LogPhidgetEvents;

                advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
                advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
                advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

                advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
                advancedServoHost.LogSequenceAction = LogSequenceAction;
                advancedServoHost.LogActionVerification = LogActionVerification;

                // TODO(crhodes)
                // Should we do open somewhere else?
                // If this times out we need to clear phidgetDevice

                advancedServoHost.Open(Common.PhidgetOpenTimeout);
            }

            // NOTE(crhodes)
            // Save this so we can use it in other commands
            // that don't specify a SerialNumber

            ActiveAdvancedServoHost = advancedServoHost;

            return advancedServoHost;
        }

        private InterfaceKitEx GetInterfaceKitHost(int serialNumber)
        {
            InterfaceKitEx interfaceKitHost = null;

            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            if (phidgetDevice?.PhidgetEx is not null)
            {
                interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

                interfaceKitHost.LogPhidgetEvents = LogPhidgetEvents;

                interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
                interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
                interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

                interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
                interfaceKitHost.LogSequenceAction = LogSequenceAction;
            }
            else
            {
                phidgetDevice.PhidgetEx = new InterfaceKitEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    true,
                    EventAggregator);

                interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

                interfaceKitHost.LogPhidgetEvents = LogPhidgetEvents;

                interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
                interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
                interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

                interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
                interfaceKitHost.LogSequenceAction = LogSequenceAction;

                // TODO(crhodes)
                // Should we do open somewhere else?
                // If this times out we need to clear phidgetDevice

                interfaceKitHost.Open(Common.PhidgetOpenTimeout);
            }

            // NOTE(crhodes)
            // Save this so we can use it in other commands
            // that don't specify a SerialNumber

            ActiveInterfaceKitHost = interfaceKitHost;

            return interfaceKitHost;
        }
        private StepperEx GetStepperHost(int serialNumber)
        {
            StepperEx stepperHost = null;

            PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidgets[serialNumber];

            if (phidgetDevice?.PhidgetEx is not null)
            {
                stepperHost = (StepperEx)phidgetDevice.PhidgetEx;

                stepperHost.LogPhidgetEvents = LogPhidgetEvents;

                //stepperHost.LogInputChangeEvents = LogInputChangeEvents;
                //stepperHost.LogOutputChangeEvents = LogOutputChangeEvents;
                //stepperHost.LogSensorChangeEvents = LogSensorChangeEvents;

                stepperHost.LogPerformanceSequence = LogPerformanceSequence;
                stepperHost.LogSequenceAction = LogSequenceAction;
                stepperHost.LogActionVerification = LogActionVerification;

            }
            else
            {
                phidgetDevice.PhidgetEx = new StepperEx(
                    phidgetDevice.IPAddress,
                    phidgetDevice.Port,
                    serialNumber,
                    EventAggregator);

                stepperHost = (StepperEx)phidgetDevice.PhidgetEx;

                stepperHost.LogPhidgetEvents = LogPhidgetEvents;

                //stepperHost.LogInputChangeEvents = LogInputChangeEvents;
                //stepperHost.LogOutputChangeEvents = LogOutputChangeEvents;
                //stepperHost.LogSensorChangeEvents = LogSensorChangeEvents;

                stepperHost.LogPerformanceSequence = LogPerformanceSequence;
                stepperHost.LogSequenceAction = LogSequenceAction;
                stepperHost.LogActionVerification = LogActionVerification;

                // TODO(crhodes)
                // Should we do open somewhere else?
                // If this times out we need to clear phidgetDevice

                stepperHost.Open(Common.PhidgetOpenTimeout);
            }

            // NOTE(crhodes)
            // Save this so we can use it in other commands
            // that don't specify a SerialNumber

            ActiveStepperHost = stepperHost;

            return stepperHost;
        }

        #endregion

        #region Protected Methods (None)



        #endregion

        #region Private Methods (None)



        #endregion
    }
}
