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
        // This needs to be something fancier as we can have multiple RCServoHost per IP and multiple IP's
        //public RCServoEx ActiveRCServoHost { get; set; }
        //public StepperEx ActiveStepperHost { get; set; }

        //public DigitalOutputEx ActiveDigitalOutputHost { get; set; }

        public Boolean LogDeviceChannelSequence { get; set; }
        public Boolean LogChannelAction { get; set; }
        public Boolean LogActionVerification { get; set; }

        // Phidget Events

        public Boolean LogPhidgetEvents { get; set; }
        public Boolean LogErrorEvents { get; set; } = true;    // Probably always want to see errors
        public Boolean LogPropertyChangeEvents { get; set; }

        // AdvancedServo and RCServo events

        public Boolean LogCurrentChangeEvents { get; set; }
        public Boolean LogPositionChangeEvents { get; set; }
        public Boolean LogVelocityChangeEvents { get; set; }
        public Boolean LogTargetPositionReachedEvents { get; set; }

        // InterfaceKit events

        public Boolean LogInputChangeEvents { get; set; }
        public Boolean LogOutputChangeEvents { get; set; }
        public Boolean LogSensorChangeEvents { get; set; }

        //

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispatches deviceChannelSequence
        /// to Execute<ChannelClass>ChannelSequence
        /// while deviceChannelSequence.NextPerformanceSequence
        /// is not null
        /// </summary>
        /// <param name="deviceChannelSequence"></param>
        /// <returns></returns>
        public async Task ExecuteDeviceChannelSequence(DeviceChannelSequence deviceChannelSequence, Int32? performanceSerialNumber = null)
        {
            Int64 startTicks = 0;

            DeviceChannelSequence nextPhidgetDeviceChannelSequence = null;

            try
            {
                if (LogDeviceChannelSequence)
                {
                    startTicks = Log.Trace($"Executing DeviceChannel Sequence:>{deviceChannelSequence?.Name}" +
                        $" channelClass:>{deviceChannelSequence?.ChannelClass}<" +
                        $" performanceSerialNumber:>{performanceSerialNumber}< dcsSerialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                        $" dcsHubPort:>{deviceChannelSequence?.HubPort}< dcsChannel:>{deviceChannelSequence?.Channel}<" +
                        $"\r loops:>{deviceChannelSequence?.SequenceLoops}<" +
                        $" duration:>{deviceChannelSequence?.Duration}<" +
                        $"\r closePhidget:>{deviceChannelSequence?.ClosePhidget}<", Common.LOG_CATEGORY);
                }

                for (Int32 sequenceLoop = 0; sequenceLoop < deviceChannelSequence.SequenceLoops; sequenceLoop++)
                {
                    if (LogDeviceChannelSequence) Log.Trace($"Running DeviceChannelSequence Loop:{sequenceLoop + 1}", Common.LOG_CATEGORY);

                    // NOTE(crhodes)
                    // Each loop starts back at the initial sequence
                    nextPhidgetDeviceChannelSequence = deviceChannelSequence;

                    // NOTE(crhodes)
                    // This allows reuse of a DeviceChannelSequence that only varies by SerialNumber

                    if (performanceSerialNumber is not null)
                    {
                        nextPhidgetDeviceChannelSequence.SerialNumber = (Int32)performanceSerialNumber;
                    }

                    do
                    {
                        switch (nextPhidgetDeviceChannelSequence.ChannelClass)
                        {
                             case "DigitalOutput":
                                nextPhidgetDeviceChannelSequence = await ExecuteDigitalOutputChannelSequence(nextPhidgetDeviceChannelSequence);
                                break;

                            case "RCServo":
                                nextPhidgetDeviceChannelSequence = await ExecuteRCServoChannelSequence(nextPhidgetDeviceChannelSequence);
                                break;

                            case "Stepper":
                                nextPhidgetDeviceChannelSequence = await ExecuteStepperChannelSequence(nextPhidgetDeviceChannelSequence);
                                break;

                            default:
                                Log.Error($"Unsupported SequenceType:>{nextPhidgetDeviceChannelSequence.ChannelClass}<", Common.LOG_CATEGORY);
                                nextPhidgetDeviceChannelSequence = null;
                                break;
                        }
                    } while (nextPhidgetDeviceChannelSequence is not null);
                }

                if (deviceChannelSequence.Duration is not null)
                {
                    if (LogDeviceChannelSequence)
                    {
                        Log.Trace($"Zzzzz Sleeping:>{deviceChannelSequence.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)deviceChannelSequence.Duration);
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

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by HubPort or Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.HubPort is not null)
                    {
                        digitalOutputSequence.HubPort = deviceChannelSequence.HubPort;
                    }

                    if (deviceChannelSequence.Channel is not null)
                    {
                        digitalOutputSequence.Channel = deviceChannelSequence.Channel;
                    }

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing DigitalOutput Channel Sequence:>{digitalOutputSequence?.Name}<" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" deviceHubPort:>{deviceChannelSequence.HubPort}< hubPort:>{digitalOutputSequence?.HubPort}<" +
                            $" deviceChannel:>{deviceChannelSequence.Channel}< channel:>{digitalOutputSequence.Channel}< " +
                            //$" sequenceLoops:>{rcServoSequence?.SequenceLoops}<" +
                            $"\r beforeActionLoopSequences:>{digitalOutputSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{rcServoSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{rcServoSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{rcServoSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{rcServoSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{rcServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{digitalOutputSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{rcServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{digitalOutputSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    phidgetHost = GetDigitalOutputHost(
                        (Int32)deviceChannelSequence.SerialNumber, 
                        (Int32)digitalOutputSequence.HubPort, 
                        (Int32)digitalOutputSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}" +
                            $" hubPort:{deviceChannelSequence.HubPort} channel:{deviceChannelSequence.Channel}", Common.LOG_CATEGORY);

                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (digitalOutputSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.BeforeActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override

                                await ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(digitalOutputSequence);

                        if (digitalOutputSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in digitalOutputSequence.AfterActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override

                                await ExecuteDeviceChannelSequence(sequence);
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

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by HubPort or Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.HubPort is not null)
                    {
                        rcServoSequence.HubPort = deviceChannelSequence.HubPort;
                    }

                    if (deviceChannelSequence.Channel is not null)
                    {
                        rcServoSequence.Channel = deviceChannelSequence.Channel;
                    }

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing RCServo Channel Sequence:>{rcServoSequence?.Name}<" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" deviceHubPort:>{deviceChannelSequence.HubPort}< hubPort:>{rcServoSequence?.HubPort}<" +
                            $" deviceChannel:>{deviceChannelSequence.Channel}< channel:>{rcServoSequence.Channel}< " +
                            //$" sequenceLoops:>{rcServoSequence?.SequenceLoops}<" +
                            $"\r beforeActionLoopSequences:>{rcServoSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{rcServoSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{rcServoSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{rcServoSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{rcServoSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{rcServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{rcServoSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{rcServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{rcServoSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    phidgetHost = GetRCServoHost(
                        deviceChannelSequence.SerialNumber, 
                        (Int32)rcServoSequence.HubPort, 
                        (Int32)rcServoSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}" +
                            $" hubPort:{deviceChannelSequence.HubPort} channel:{deviceChannelSequence.Channel}", Common.LOG_CATEGORY);

                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (rcServoSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in rcServoSequence.BeforeActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override

                                await ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(rcServoSequence);

                        if (rcServoSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in rcServoSequence.AfterActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override

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

                    // NOTE(crhodes)
                    // This allows reuse of a ChannelSequence that only varies by HubPort or Channel
                    // Useful during initialization of common Channels on a Phidget Device

                    if (deviceChannelSequence.HubPort is not null)
                    {
                        stepperSequence.HubPort = deviceChannelSequence.HubPort;
                    }

                    if (deviceChannelSequence.Channel is not null)
                    {
                        stepperSequence.Channel = deviceChannelSequence.Channel;
                    }

                    if (LogDeviceChannelSequence)
                    {
                        startTicks = Log.Trace($"Executing Stepper Channel Sequence:>{stepperSequence?.Name}<" +
                            $" serialNumber:>{deviceChannelSequence?.SerialNumber}<" +
                            $" deviceHubPort:>{deviceChannelSequence.HubPort}< hubPort:>{stepperSequence?.HubPort}<" +
                            $" deviceChannel:>{deviceChannelSequence.Channel}< channel:>{stepperSequence.Channel}< " +
                            //$" sequenceLoops:>{rcServoSequence?.SequenceLoops}<" +
                            $"\r beforeActionLoopSequences:>{stepperSequence?.BeforeActionLoopSequences?.Count()}<" +
                            //$" startActionLoopSequences:>{rcServoSequence?.StartActionLoopSequences?.Count()}<" +
                            //$" actionLoops:>{rcServoSequence?.ActionLoops}<" +
                            //$" executeActionsInParallel:>{rcServoSequence?.ExecuteActionsInParallel}<" +
                            //$" actionDuration:>{rcServoSequence?.ActionsDuration}<" +
                            //$" endActionLoopSequences:>{rcServoSequence?.EndActionLoopSequences?.Count()}<" +
                            $" afterActionLoopSequences:>{stepperSequence?.AfterActionLoopSequences?.Count()}<" +
                            //$" sequenceDuration:>{rcServoSequence?.SequenceDuration}<" +
                            $" nextSequence:>{stepperSequence?.NextSequence?.Name}<", Common.LOG_CATEGORY);
                    }

                    phidgetHost = GetStepperHost(
                        (Int32)deviceChannelSequence.SerialNumber, 
                        (Int32)stepperSequence.HubPort, 
                        (Int32)stepperSequence.Channel);

                    if (phidgetHost == null)
                    {
                        Log.Error($"Cannot locate host to execute SerialNumber:{deviceChannelSequence.SerialNumber}" +
                            $" hubPort:{deviceChannelSequence.HubPort} channel:{deviceChannelSequence.Channel}", Common.LOG_CATEGORY);

                        nextDeviceChannelSequence = null;
                    }

                    if (phidgetHost is not null)
                    {
                        if (stepperSequence.BeforeActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in stepperSequence.BeforeActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override

                                await ExecuteDeviceChannelSequence(sequence);
                            }
                        }

                        await phidgetHost.RunActionLoops(stepperSequence);

                        if (stepperSequence.AfterActionLoopSequences is not null)
                        {
                            foreach (DeviceChannelSequence sequence in stepperSequence.AfterActionLoopSequences)
                            {
                                // NOTE(crhodes)
                                // We do not pass in a SerialNumber override
                                await ExecuteDeviceChannelSequence(sequence);
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

        private DigitalOutputEx GetDigitalOutputHost(Int32 serialNumber, Int32 hubPort, Int32 channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            SerialHubPortChannel serialHubPortChannel = new SerialHubPortChannel()
            {
                SerialNumber = serialNumber,
                HubPort = hubPort,
                Channel = channel
            };

            DigitalOutputEx digitalOutputHost = Common.PhidgetDeviceLibrary.DigitalOutputChannels[serialHubPortChannel];

            digitalOutputHost.LogPhidgetEvents = LogPhidgetEvents;
            digitalOutputHost.LogErrorEvents = LogErrorEvents;
            digitalOutputHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            digitalOutputHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
            digitalOutputHost.LogChannelAction = LogChannelAction;
            digitalOutputHost.LogActionVerification = LogActionVerification;

            // NOTE(crhodes)
            // Opening is handled by the Performance/DeviceChannelSequence/ChannelSequence

            // TODO(crhodes)
            // Maybe we just let the Action.Open do the open

            //if (digitalOutputHost.Attached is false)
            //{
            // NOTE(crhodes)
            // Things that work and things that don't
            //
            // This does work
            //digitalOutputHost.Open(500);

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

        private RCServoEx GetRCServoHost(Int32 serialNumber, Int32 hubPort, Int32 channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            SerialHubPortChannel serialHubPortChannel = new SerialHubPortChannel()
            {
                SerialNumber = serialNumber, 
                HubPort = hubPort,
                Channel = channel 
            };

            RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.RCServoChannels[serialHubPortChannel];

            rcServoHost.LogPhidgetEvents = LogPhidgetEvents;
            rcServoHost.LogErrorEvents = LogErrorEvents;
            rcServoHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            rcServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
            rcServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            rcServoHost.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            rcServoHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
            rcServoHost.LogChannelAction = LogChannelAction;
            rcServoHost.LogActionVerification = LogActionVerification;

            // NOTE(crhodes)
            // Opening is handled by the Performance/DeviceChannelSequence/ChannelSequence

            // TODO(crhodes)
            // Maybe we just let the Action.Open do the open

            //if (rcServoHost.Attached is false)
            //{
            // NOTE(crhodes)
            // Things that work and things that don't
            //
            // This does work
            //rcServoHost.Open(500);

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

        private StepperEx GetStepperHost(Int32 serialNumber, Int32 hubPort, Int32 channel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            SerialHubPortChannel serialHubPortChannel = new SerialHubPortChannel()
            {
                SerialNumber = serialNumber,
                HubPort = hubPort,
                Channel = channel
            };

            StepperEx stepperHost = Common.PhidgetDeviceLibrary.StepperChannels[serialHubPortChannel];

            stepperHost.LogPhidgetEvents = LogPhidgetEvents;
            stepperHost.LogErrorEvents = LogErrorEvents;
            stepperHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //stepperHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            stepperHost.LogPositionChangeEvents = LogPositionChangeEvents;
            stepperHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

            stepperHost.LogDeviceChannelSequence = LogDeviceChannelSequence;
            stepperHost.LogChannelAction = LogChannelAction;
            stepperHost.LogActionVerification = LogActionVerification;

            // NOTE(crhodes)
            // Opening is handled by the Performance/DeviceChannelSequence/ChannelSequence

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
