using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;
using VNC.Phidget22.Ex;

namespace VNC.Phidget22.Players
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

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public static PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        // TODO(crhodes)
        //// 
        //public AdvancedServoEx ActiveAdvancedServoHost { get; set; }
        //public InterfaceKitEx ActiveInterfaceKitHost { get; set; }

        // TODO(crhodes)
        // This needs to be something fancier as we can have multiple RCServoHost per IP and multiple IP's
        public RCServoEx ActiveRCServoHost { get; set; }
        public StepperEx ActiveStepperHost { get; set; }

        public DigitalOutputEx ActiveDigitalOutputHost { get; set; }

        public bool LogPerformanceSequence { get; set; }
        public bool LogSequenceAction { get; set; }
        public bool LogActionVerification { get; set; }

        // Phidget Events

        public bool LogPhidgetEvents { get; set; }
        public bool LogErrorEvents { get; set; } = true;    // Probably always want to see errors
        public bool LogPropertyChangeEvents { get; set; }

        // AdvancedServo and RCServo events

        public bool LogCurrentChangeEvents { get; set; }
        public bool LogPositionChangeEvents { get; set; }
        public bool LogVelocityChangeEvents { get; set; }
        public bool LogTargetPositionReachedEvents { get; set; }

        // InterfaceKit events

        public bool LogInputChangeEvents { get; set; }
        public bool LogOutputChangeEvents { get; set; }
        public bool LogSensorChangeEvents { get; set; }

        //

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispatches performanceSequence
        /// to Execution<TYPE>PerformanceSequence
        /// while performanceSequence.NextPerformanceSequence
        /// is not null
        /// </summary>
        /// <param name="performanceSequence"></param>
        /// <returns></returns>
        public async Task ExecutePerformanceSequence(DeviceClassSequence performanceSequence)
        {
            Int64 startTicks = 0;

            DeviceClassSequence nextPerformanceSequence = null;

            try
            {
                if (LogPerformanceSequence)
                {
                    startTicks = Log.Trace($"Executing Performance Sequence" +
                        $" name:>{performanceSequence?.Name}<" +
                        $" type:>{performanceSequence?.DeviceClass}<" +
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
                        switch (nextPerformanceSequence.DeviceClass)   // DeviceClass
                        {
                             case "DigitalOutput":
                                nextPerformanceSequence = await ExecuteDigitalOutputPerformanceSequence(nextPerformanceSequence);
                                break;

                            case "RCServo":
                                nextPerformanceSequence = await ExecuteRCServoPerformanceSequence(nextPerformanceSequence);
                                break;

                            case "Stepper":
                                nextPerformanceSequence = await ExecuteStepperPerformanceSequence(nextPerformanceSequence);
                                break;

                            default:
                                Log.Error($"Unsupported SequenceType:>{nextPerformanceSequence.DeviceClass}<", Common.LOG_CATEGORY);
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

        private async Task<DeviceClassSequence> ExecuteRCServoPerformanceSequence(DeviceClassSequence performanceSequence)
        {
            Int64 startTicks = 0;
            DeviceClassSequence nextPerformanceSequence = null;

            try
            {
                RCServoEx phidgetHost = null;

                if (PerformanceLibrary.AvailableRCServoSequences.ContainsKey(performanceSequence.Name ?? ""))
                {
                    var rcServoSequence = PerformanceLibrary.AvailableRCServoSequences[performanceSequence.Name];

                    if (LogPerformanceSequence)
                    {
                        startTicks = Log.Trace($"Executing RCS Performance Sequence" +
                            //$" serialNumber:>{advancedServoSequence?.SerialNumber}<" +
                            //$" serialNumber:>{performanceSequence?.SerialNumber}<" +
                            $" name:>{rcServoSequence?.Name}<" +
                            $" sequenceLoops:>{rcServoSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{rcServoSequence?.BeforeActionLoopSequences?.Count()}<" +
                            $" startActionLoopSequences:>{rcServoSequence?.StartActionLoopSequences?.Count()}<" +
                            $" actionLoops:>{rcServoSequence?.ActionLoops}<" +
                            $" executeActionsInParallel:>{rcServoSequence?.ExecuteActionsInParallel}<" +
                            $" actionDuration:>{rcServoSequence?.ActionsDuration}<" +
                            $" endActionLoopSequences:>{rcServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{rcServoSequence?.AfterActionLoopSequences?.Count()}<" +
                            $" sequenceDuration:>{rcServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{rcServoSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    //if (advancedServoSequence.SerialNumber is not null)
                    //{
                    //    phidgetHost = GetRCServoHost((int)advancedServoSequence.SerialNumber);
                    //}
                    //else if (ActiveRCServoHost is not null)
                    //{
                    //    phidgetHost = ActiveRCServoHost;
                    //}
                    //else
                    //{
                    //    Log.Error($"Cannot locate host to execute SerialNumber:{advancedServoSequence.SerialNumber}", Common.LOG_CATEGORY);
                    //    nextPerformanceSequence = null;
                    //}

                    phidgetHost = GetRCServoHost((int)performanceSequence.SerialNumber, rcServoSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (rcServoSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in rcServoSequence.BeforeActionLoopSequences)
                            {
                                await ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(rcServoSequence);

                        if (rcServoSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in rcServoSequence.AfterActionLoopSequences)
                            {
                                await ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (rcServoSequence.SequenceDuration is not null)
                        {
                            if (LogPerformanceSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{rcServoSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)rcServoSequence.SequenceDuration);
                        }

                        nextPerformanceSequence = rcServoSequence.NextSequence;
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

        private async Task<DeviceClassSequence> ExecuteDigitalOutputPerformanceSequence(DeviceClassSequence performanceSequence)
        {
            Int64 startTicks = 0;
            DeviceClassSequence nextPerformanceSequence = null;

            // FIX(crhodes)
            // 

            try
            {
                DigitalOutputEx phidgetHost = null;

                if (PerformanceLibrary.AvailableDigitalOutputSequences.ContainsKey(performanceSequence.Name))
                {
                    var digitalOutputSequence = PerformanceLibrary.AvailableDigitalOutputSequences[performanceSequence.Name];

                    if (LogPerformanceSequence)
                    {
                        startTicks = Log.Trace($"Executing DO Performance Sequence" +
                            $" serialNumber:>{performanceSequence?.SerialNumber}<" +
                            $" name:>{digitalOutputSequence?.Name}<" +
                            $" sequenceLoops:>{digitalOutputSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{digitalOutputSequence?.BeforeActionLoopSequences?.Count()}<" +
                            $" startActionLoopSequences:>{digitalOutputSequence?.StartActionLoopSequences?.Count()}<" +
                            $" actionLoops:>{digitalOutputSequence?.ActionLoops}<" +
                            $" executeActionsInParallel:>{digitalOutputSequence?.ExecuteActionsInParallel}<" +
                            $" actionDuration:>{digitalOutputSequence?.ActionsDuration}<" +
                            $" endActionLoopSequences:>{digitalOutputSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{digitalOutputSequence?.AfterActionLoopSequences?.Count()}<" +
                            $" sequenceDuration:>{digitalOutputSequence?.SequenceDuration}<" +
                            $" nextSequence:>{digitalOutputSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    //if (digitalOutputSequence.SerialNumber is not null)
                    //{
                    //    phidgetHost = GetDigitalOutputHost((int)digitalOutputSequence.SerialNumber);
                    //}
                    //else if (ActiveDigitalOutputHost is not null)
                    //{
                    //    phidgetHost = ActiveDigitalOutputHost;
                    //}
                    //else
                    //{
                    //    Log.Error($"Cannot locate host to execute SerialNumber:{digitalOutputSequence.SerialNumber}", Common.LOG_CATEGORY);
                    //    nextPerformanceSequence = null;
                    //}

                    phidgetHost = GetDigitalOutputHost((int)performanceSequence.SerialNumber, digitalOutputSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (digitalOutputSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in digitalOutputSequence.BeforeActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(digitalOutputSequence);

                        if (digitalOutputSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in digitalOutputSequence.AfterActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        if (digitalOutputSequence.SequenceDuration is not null)
                        {
                            if (LogPerformanceSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{digitalOutputSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)digitalOutputSequence.SequenceDuration);
                        }

                        nextPerformanceSequence = digitalOutputSequence.NextSequence;
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

        private async Task<DeviceClassSequence> ExecuteStepperPerformanceSequence(DeviceClassSequence performanceSequence)
        {
            Int64 startTicks = 0;
            DeviceClassSequence nextPerformanceSequence = null;

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

                    phidgetHost = GetStepperHost((int)performanceSequence.SerialNumber, stepperSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{performanceSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextPerformanceSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (stepperSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in stepperSequence.BeforeActionLoopSequences)
                            {
                                ExecutePerformanceSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(stepperSequence);

                        if (stepperSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceClassSequence sequence in stepperSequence.AfterActionLoopSequences)
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

        // FIX(crhodes)
        // 

        //private AdvancedServoEx GetAdvancedServoHost(int serialNumber)
        //{
        //    PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidget22[serialNumber];

        //    AdvancedServoEx advancedServoHost = null;

        //    if (phidgetDevice?.PhidgetEx is not null)
        //    {
        //        advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

        //        advancedServoHost.LogPhidgetEvents = LogPhidgetEvents;

        //        advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //        advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //        advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //        advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
        //        advancedServoHost.LogSequenceAction = LogSequenceAction;
        //        advancedServoHost.LogActionVerification = LogActionVerification;
        //    }
        //    else
        //    {
        //        phidgetDevice.PhidgetEx = new AdvancedServoEx(
        //            phidgetDevice.IPAddress,
        //            phidgetDevice.Port,
        //            serialNumber,
        //            EventAggregator);

        //        advancedServoHost = (AdvancedServoEx)phidgetDevice.PhidgetEx;

        //        advancedServoHost.LogPhidgetEvents = LogPhidgetEvents;

        //        advancedServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //        advancedServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //        advancedServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //        advancedServoHost.LogPerformanceSequence = LogPerformanceSequence;
        //        advancedServoHost.LogSequenceAction = LogSequenceAction;
        //        advancedServoHost.LogActionVerification = LogActionVerification;

        //        // TODO(crhodes)
        //        // Should we do open somewhere else?
        //        // If this times out we need to clear phidgetDevice

        //        advancedServoHost.Open(Common.PhidgetOpenTimeout);
        //    }

        //    // NOTE(crhodes)
        //    // Save this so we can use it in other commands
        //    // that don't specify a SerialNumber

        //    ActiveAdvancedServoHost = advancedServoHost;

        //    return advancedServoHost;
        //}

        //private InterfaceKitEx GetInterfaceKitHost(int serialNumber)
        //{
            // FIX(crhodes)
            // 

            //InterfaceKitEx interfaceKitHost = null;

            //PhidgetDevice phidgetDevice = PhidgetLibrary.AvailablePhidget22[serialNumber];

            //if (phidgetDevice?.PhidgetEx is not null)
            //{
            //    interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

            //    interfaceKitHost.LogPhidgetEvents = LogPhidgetEvents;

            //    interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
            //    interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
            //    interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

            //    interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
            //    interfaceKitHost.LogSequenceAction = LogSequenceAction;
            //}
            //else
            //{
            //    phidgetDevice.PhidgetEx = new InterfaceKitEx(
            //        phidgetDevice.IPAddress,
            //        phidgetDevice.Port,
            //        serialNumber,
            //        true,
            //        EventAggregator);

            //    interfaceKitHost = (InterfaceKitEx)phidgetDevice.PhidgetEx;

            //    interfaceKitHost.LogPhidgetEvents = LogPhidgetEvents;

            //    interfaceKitHost.LogInputChangeEvents = LogInputChangeEvents;
            //    interfaceKitHost.LogOutputChangeEvents = LogOutputChangeEvents;
            //    interfaceKitHost.LogSensorChangeEvents = LogSensorChangeEvents;

            //    interfaceKitHost.LogPerformanceSequence = LogPerformanceSequence;
            //    interfaceKitHost.LogSequenceAction = LogSequenceAction;

            //    // TODO(crhodes)
            //    // Should we do open somewhere else?
            //    // If this times out we need to clear phidgetDevice

            //    interfaceKitHost.Open(Common.PhidgetOpenTimeout);
            //}

            //// NOTE(crhodes)
            //// Save this so we can use it in other commands
            //// that don't specify a SerialNumber

            //ActiveInterfaceKitHost = interfaceKitHost;

            //return interfaceKitHost;
        //}

        private DigitalOutputEx GetDigitalOutputHost(int serialNumber, int channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            //PhidgetDevice phidgetDevice = Common.PhidgetDeviceLibrary.AvailablePhidgets[serialNumber];

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

            DigitalOutputEx digitalOutputHost = Common.PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];

            digitalOutputHost.LogPhidgetEvents = LogPhidgetEvents;
            digitalOutputHost.LogErrorEvents = LogErrorEvents;
            digitalOutputHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //digitalOutputHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //digitalOutputHost.LogPositionChangeEvents = LogPositionChangeEvents;
            //digitalOutputHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //digitalOutputHost.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            digitalOutputHost.LogPerformanceSequence = LogPerformanceSequence;
            digitalOutputHost.LogSequenceAction = LogSequenceAction;
            digitalOutputHost.LogActionVerification = LogActionVerification;

            // TODO(crhodes)
            // Maybe we just let the Action.Open do the open

            //if (digitalOutputHost.Attached is false)
            //{
            // NOTE(crhodes)
            // Things that work and things that don't
            //
            // This does work
            digitalOutputHost.Open(500);

            // This does not work.
            //digitalOutputHost.Open();

            // This does work

            //digitalOutputHost.Open();
            //Thread.Sleep(500);

            // This does not work.
            //await Task.Run(() => digitalOutputHost.Open());

            //}

            if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

            return digitalOutputHost;
        }

        private RCServoEx GetRCServoHost(int serialNumber, int channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            //PhidgetDevice phidgetDevice = Common.PhidgetDeviceLibrary.AvailablePhidgets[serialNumber];

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

            RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.RCServoChannels[serialChannel];

            rcServoHost.LogPhidgetEvents = LogPhidgetEvents;
            rcServoHost.LogErrorEvents = LogErrorEvents;
            rcServoHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            rcServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
            rcServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            rcServoHost.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            rcServoHost.LogPerformanceSequence = LogPerformanceSequence;
            rcServoHost.LogSequenceAction = LogSequenceAction;
            rcServoHost.LogActionVerification = LogActionVerification;

            // TODO(crhodes)
            // Maybe we just let the Action.Open do the open

            //if (rcServoHost.Attached is false)
            //{
            // NOTE(crhodes)
            // Things that work and things that don't
            //
            // This does work
            rcServoHost.Open(500);

            // This does not work.
            //rcServoHost.Open();

            // This does work

            //rcServoHost.Open();
            //Thread.Sleep(500);

            // This does not work.
            //await Task.Run(() => rcServoHost.Open());

            //}

            if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

            return rcServoHost;
        }

        private StepperEx GetStepperHost(int serialNumber, int channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            //PhidgetDevice phidgetDevice = Common.PhidgetDeviceLibrary.AvailablePhidgets[serialNumber];

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

            // TODO(crhodes)
            // This throws exception if serialChannel not found

            StepperEx stepperHost = Common.PhidgetDeviceLibrary.StepperChannels[serialChannel];

            stepperHost.LogPhidgetEvents = LogPhidgetEvents;
            stepperHost.LogErrorEvents = LogErrorEvents;
            stepperHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //stepperHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            stepperHost.LogPositionChangeEvents = LogPositionChangeEvents;
            stepperHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            stepperHost.LogPerformanceSequence = LogPerformanceSequence;
            stepperHost.LogSequenceAction = LogSequenceAction;
            stepperHost.LogActionVerification = LogActionVerification;

            // TODO(crhodes)
            // Maybe we just let the Action.Open do the open

            //if (stepperHost.Attached is false)
            //{
            // NOTE(crhodes)
            // Things that work and things that don't
            //
            // This does work
            stepperHost.Open(500);

            // This does not work.
            //stepperHost.Open();

            // This does work

            //stepperHost.Open();
            //Thread.Sleep(500);

            // This does not work.
            //await Task.Run(() => stepperHost.Open());

            //}

            if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

            return stepperHost;
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods (none)



        #endregion
    }
}
