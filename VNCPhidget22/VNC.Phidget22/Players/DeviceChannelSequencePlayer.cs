using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;
using Prism.Regions.Behaviors;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;
using VNC.Phidget22.Ex;

namespace VNC.Phidget22.Players
{
    public class DeviceChannelSequencePlayer
    {
        #region Constructors, Initialization, and Load

        public IEventAggregator EventAggregator { get; set; }

        public DeviceChannelSequencePlayer(IEventAggregator eventAggregator)
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

        public static DeviceChannelSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        // TODO(crhodes)
        //// 
        //public AdvancedServoEx ActiveAdvancedServoHost { get; set; }
        //public InterfaceKitEx ActiveInterfaceKitHost { get; set; }

        // TODO(crhodes)
        // This needs to be something fancier as we can have multiple RCServoHost per IP and multiple IP's
        public RCServoEx ActiveRCServoHost { get; set; }
        public StepperEx ActiveStepperHost { get; set; }

        public DigitalOutputEx ActiveDigitalOutputHost { get; set; }

        public bool LogDeviceChannelSequence { get; set; }
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
        /// Dispatches deviceChannelSequence
        /// to Execution<TYPE>PerformanceSequence
        /// while deviceChannelSequence.NextPerformanceSequence
        /// is not null
        /// </summary>
        /// <param name="deviceChannelSequence"></param>
        /// <returns></returns>
        public async Task ExecuteDeviceChannelSequence(DeviceChannelSequence deviceChannelSequence)
        {
            Int64 startTicks = 0;

            DeviceChannelSequence nextPhidgetDeviceClassSequence = null;

            try
            {
                if (LogDeviceChannelSequence)
                {
                    startTicks = Log.Trace($"Executing PhidgetDevice Sequence" +
                        $" name:>{deviceChannelSequence?.Name}<" +
                        $" channel{deviceChannelSequence?.Channel}<" +
                        $" serialNumber{deviceChannelSequence?.SerialNumber}<" +
                        $" channelClass:>{deviceChannelSequence?.ChannelClass}<" +
                        $" loops:>{deviceChannelSequence?.SequenceLoops}<" +
                        $" duration:>{deviceChannelSequence?.Duration}<" +
                        $" closePhidget:>{deviceChannelSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                for (int sequenceLoop = 0; sequenceLoop < deviceChannelSequence.SequenceLoops; sequenceLoop++)
                {
                    if (LogDeviceChannelSequence) Log.Trace($"Running PhidgetDeviceSequence Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                    // NOTE(crhodes)
                    // Each loop starts back at the initial sequence
                    nextPhidgetDeviceClassSequence = deviceChannelSequence;

                    do
                    {
                        switch (nextPhidgetDeviceClassSequence.ChannelClass)   // DeviceClass
                        {
                             case "DigitalOutput":
                                nextPhidgetDeviceClassSequence = await ExecuteDigitalOutputChannelSequence(nextPhidgetDeviceClassSequence);
                                break;

                            case "RCServo":
                                nextPhidgetDeviceClassSequence = await ExecuteRCServoChannelSequence(nextPhidgetDeviceClassSequence);
                                break;

                            case "Stepper":
                                nextPhidgetDeviceClassSequence = await ExecuteStepperChannelSequence(nextPhidgetDeviceClassSequence);
                                break;

                            default:
                                Log.Error($"Unsupported SequenceType:>{nextPhidgetDeviceClassSequence.ChannelClass}<", Common.LOG_CATEGORY);
                                nextPhidgetDeviceClassSequence = null;
                                break;
                        }
                    } while (nextPhidgetDeviceClassSequence is not null);
                }

                if (deviceChannelSequence.Duration is not null)
                {
                    if (LogDeviceChannelSequence)
                    {
                        Log.Trace($"Zzzzz Sleeping:>{deviceChannelSequence.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((int)deviceChannelSequence.Duration);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (LogDeviceChannelSequence)
            {
                Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            }
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        #region Execute<Channel>Sequence

        // TODO(crhodes)
        // Might be able to make generic

        private async Task<DeviceChannelSequence> ExecuteDigitalOutputChannelSequence(DeviceChannelSequence deviceChannelSequence)
        {
            Int64 startTicks = 0;
            DeviceChannelSequence nextDeviceChannelSequence = null;

            try
            {
                DigitalOutputEx phidgetHost = null;

                if (PerformanceLibrary.AvailableDigitalOutputSequences.ContainsKey(deviceChannelSequence.Name))
                {
                    var digitalOutputSequence = PerformanceLibrary.AvailableDigitalOutputSequences[deviceChannelSequence.Name];

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing DigitalOutput Channel Sequence" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" name:>{digitalOutputSequence?.Name}< channel:>{digitalOutputSequence.Channel}< deviceChannel:>{deviceChannelSequence.Channel}<" +
                            //$" sequenceLoops:>{digitalOutputSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{digitalOutputSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{digitalOutputSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{digitalOutputSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{digitalOutputSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{digitalOutputSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{digitalOutputSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{digitalOutputSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{digitalOutputSequence?.SequenceDuration}<" +
                            $" nextSequence:>{digitalOutputSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.Channel is not null)
                    {
                        digitalOutputSequence.Channel = deviceChannelSequence.Channel;
                    }

                    phidgetHost = GetDigitalOutputHost((int)deviceChannelSequence.SerialNumber, (Int32)digitalOutputSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (digitalOutputSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.BeforeActionLoopSequences)
                            {
                                ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(digitalOutputSequence);

                        if (digitalOutputSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.AfterActionLoopSequences)
                            {
                                ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (digitalOutputSequence.SequenceDuration is not null)
                        {
                            if (LogDeviceChannelSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{digitalOutputSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)digitalOutputSequence.SequenceDuration);
                        }

                        nextDeviceChannelSequence = digitalOutputSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Trace($"Cannot find deviceChannelSequence:{deviceChannelSequence.Name}", Common.LOG_CATEGORY);
                    nextDeviceChannelSequence = null;
                }

                if (LogDeviceChannelSequence) Log.Trace($"Exit nextDeviceChannelSequence:{nextDeviceChannelSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextDeviceChannelSequence;
        }

        private async Task<DeviceChannelSequence> ExecuteRCServoChannelSequence(DeviceChannelSequence deviceChannelSequence)
        {
            Int64 startTicks = 0;
            DeviceChannelSequence nextDeviceChannelSequence = null;

            try
            {
                RCServoEx phidgetHost = null;

                RCServoSequence rcServoSequence;

                if (PerformanceLibrary.AvailableRCServoSequences.ContainsKey(deviceChannelSequence.Name ?? ""))
                {
                    rcServoSequence = PerformanceLibrary.AvailableRCServoSequences[deviceChannelSequence.Name];

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing RCServo Channel Sequence" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" name:>{rcServoSequence?.Name}< channel:>{rcServoSequence.Channel}< deviceChannel:>{deviceChannelSequence.Channel}<" +
                            //$" sequenceLoops:>{rcServoSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{rcServoSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{rcServoSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{rcServoSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{rcServoSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{rcServoSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{rcServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{rcServoSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{rcServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{rcServoSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.Channel is not null)
                    {
                        rcServoSequence.Channel = deviceChannelSequence.Channel;
                    }

                    phidgetHost = GetRCServoHost((int)deviceChannelSequence.SerialNumber, (Int32)rcServoSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (rcServoSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in rcServoSequence.BeforeActionLoopSequences)
                            {
                                await ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(rcServoSequence);

                        if (rcServoSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in rcServoSequence.AfterActionLoopSequences)
                            {
                                await ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (rcServoSequence.SequenceDuration is not null)
                        {
                            if (LogDeviceChannelSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{rcServoSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)rcServoSequence.SequenceDuration);
                        }

                        nextDeviceChannelSequence = rcServoSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Error($"Cannot find deviceChannelSequence:>{deviceChannelSequence.Name}<", Common.LOG_CATEGORY);
                    nextDeviceChannelSequence = null;
                }

                if (LogDeviceChannelSequence) Log.Trace($"Exit nextDeviceChannelSequence:{nextDeviceChannelSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextDeviceChannelSequence;
        }

        private async Task<DeviceChannelSequence> ExecuteStepperChannelSequence(DeviceChannelSequence deviceChannelSequence)
        {
            Int64 startTicks = 0;
            DeviceChannelSequence nextDeviceChannelSequence = null;

            try
            {
                StepperEx phidgetHost = null;

                if (PerformanceLibrary.AvailableStepperSequences.ContainsKey(deviceChannelSequence.Name ?? ""))
                {
                    var stepperSequence = PerformanceLibrary.AvailableStepperSequences[deviceChannelSequence.Name];

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing Stepper Channel Sequence" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" name:>{stepperSequence?.Name}< channel:>{stepperSequence.Channel}< deviceChannel:>{deviceChannelSequence.Channel}<" +
                            //$" sequenceLoops:>{stepperSequence?.SequenceLoops}<" +
                            $" beforeActionLoopSequences:>{stepperSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{stepperSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{stepperSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{stepperSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{stepperSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{stepperSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{stepperSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{stepperSequence?.SequenceDuration}<" +
                            $" nextSequence:>{stepperSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.Channel is not null)
                    {
                        stepperSequence.Channel = deviceChannelSequence.Channel;
                    }

                    phidgetHost = GetStepperHost((int)deviceChannelSequence.SerialNumber, (Int32)stepperSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}", Common.LOG_CATEGORY);
                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (stepperSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in stepperSequence.BeforeActionLoopSequences)
                            {
                                ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(stepperSequence);

                        if (stepperSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in stepperSequence.AfterActionLoopSequences)
                            {
                                ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        if (stepperSequence.SequenceDuration is not null)
                        {
                            if (LogDeviceChannelSequence)
                            {
                                Log.Trace($"Zzzzz Sequence:>{stepperSequence.SequenceDuration}<", Common.LOG_CATEGORY);
                            }
                            Thread.Sleep((Int32)stepperSequence.SequenceDuration);
                        }

                        nextDeviceChannelSequence = stepperSequence.NextSequence;
                    }
                }
                else
                {
                    Log.Trace($"Cannot find deviceChannelSequence:{deviceChannelSequence.Name}", Common.LOG_CATEGORY);
                    nextDeviceChannelSequence = null;
                }

                if (LogDeviceChannelSequence) Log.Trace($"Exit nextDeviceChannelSequence:{nextDeviceChannelSequence?.Name}", Common.LOG_CATEGORY, startTicks);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            return nextDeviceChannelSequence;
        }

        #endregion

        #region Get<Channel>Host

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

            digitalOutputHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
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

            rcServoHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
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

            stepperHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
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

        #endregion
    }
}
