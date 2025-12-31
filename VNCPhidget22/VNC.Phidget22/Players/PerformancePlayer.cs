using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

using Prism.Events;

using VNC.Core.Mvvm;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Players
{
    public class PerformancePlayer : INPCBase
    {
        #region Constructors, Initialization, and Load

        private static readonly object _lock = new object();

        public IEventAggregator EventAggregator { get; set; }

        public PerformancePlayer(IEventAggregator eventAggregator, PerformanceLibrary performanceLibrary, Int32? serialNumber = null)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            SerialNumber = serialNumber;
            PerformanceLibrary = performanceLibrary;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        public Int32? SerialNumber { get; set; } = null;

        public PerformanceLibrary PerformanceLibrary { get; set; }

        #region Logging

        #region Performance Logging

        // NOTE(crhodes)
        // Don't think we need INPC on these

        private Boolean _logPerformance = false;
        public Boolean LogPerformance
        {
            get => _logPerformance;
            set
            {
                if (_logPerformance == value)
                    return;
                _logPerformance = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logDeviceChannelSequence = false;
        public Boolean LogDeviceChannelSequence
        {
            get => _logDeviceChannelSequence;
            set
            {
                if (_logDeviceChannelSequence == value)
                    return;
                _logDeviceChannelSequence = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logChannelAction = false;
        public Boolean LogChannelAction
        {
            get => _logChannelAction;
            set
            {
                if (_logChannelAction == value)
                    return;
                _logChannelAction = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logActionVerification = false;
        public Boolean LogActionVerification
        {
            get => _logActionVerification;
            set
            {
                if (_logActionVerification == value)
                    return;
                _logActionVerification = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Phidget Device Logging

        private Boolean _logPhidgetEvents = false;

        public Boolean LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        #region AdvancedServo

        private Boolean _logCurrentChangeEvents = false;
        public Boolean LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value)
                    return;
                _logCurrentChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logPositionChangeEvents = false;
        public Boolean LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value)
                    return;
                _logPositionChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logVelocityChangeEvents = false;
        public Boolean LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value)
                    return;
                _logVelocityChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logTargetPositionReachedEvents = false;
        public Boolean LogTargetPositionReachedEvents
        {
            get => _logTargetPositionReachedEvents;
            set
            {
                if (_logTargetPositionReachedEvents == value)
                    return;
                _logTargetPositionReachedEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region InterfaceKit

        private Boolean _displayInputChangeEvents = false;

        public Boolean LogInputChangeEvents
        {
            get => _displayInputChangeEvents;
            set
            {
                if (_displayInputChangeEvents == value)
                    return;
                _displayInputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _displayOutputChangeEvents = false;

        public Boolean LogOutputChangeEvents
        {
            get => _displayOutputChangeEvents;
            set
            {
                if (_displayOutputChangeEvents == value)
                    return;
                _displayOutputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _sensorChangeEvents = false;

        public Boolean LogSensorChangeEvents
        {
            get => _sensorChangeEvents;
            set
            {
                if (_sensorChangeEvents == value)
                    return;
                _sensorChangeEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Stepper



        #endregion

        #endregion

        #endregion

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands (none)



        #endregion

        #region Public Methods

        /// <summary>
        /// Executes a Performance by calling RunPerformanceLoops() and calls NextPerformance if any
        /// </summary>
        /// <param name="performance"></param>
        /// <returns></returns>
        public async Task PlayPerformance(Performance performance)
        {
            Performance? nextPerformance = performance;

            while (nextPerformance is not null)
            {
                if (LogPerformance)
                {
                    Log.TRACE($"PlayPerformance(>{performance.Name}<) description:>{performance.Description}<" +
                        $" beforePerformanceLoopPerformances:>{performance.BeforePerformanceLoopPerformances?.Count()}<" +
                        $" deviceChannelSequences:>{performance.DeviceChannelSequences?.Count()}<" +
                        $" playSequencesInParallel:>{performance.ExecuteDeviceChannelSequencesInParallel}<" +
                        $" performances:>{performance.Performances?.Count()}<" +
                        $" playPerformancesInParallel:>{performance.ExecutePerformancesInParallel}<" +
                        $" loops:>{performance.PerformanceLoops}<" +
                        $" afterPerformanceLoopPerformances:>{performance.AfterPerformanceLoopPerformances?.Count()}<" +
                        $" duration:>{performance.Duration}<" +
                        $" nextPerformance:>{performance.NextPerformance?.Name}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                string? performanceName = nextPerformance?.Name;

                if (performanceName != null)
                {
                    if (PerformanceLibrary.AllPerformances.ContainsKey(performanceName))
                    {
                        nextPerformance = PerformanceLibrary.AllPerformances[performanceName];

                        await ExecutePerformance(nextPerformance);

                        nextPerformance = nextPerformance?.NextPerformance;
                    }
                }
                else
                {
                    Log.ERROR($"Cannot find performance:>{performanceName}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }
        }

        public async Task ExecutePerformance(Performance performance)
        {
            long startTicks = 0;

            if (LogPerformance)
            {
                startTicks = Log.TRACE($"Enter ExecutePerformance(:>{performance.Name}<)" +
                    $" description:>{performance.Description}<" +
                    $" playerSerialNumber:>{SerialNumber}<" +
                    $" serialNumber:>{performance.SerialNumber}<" +
                    $" beforePerformanceLoopPerformances:>{performance.BeforePerformanceLoopPerformances?.Count()}<" +
                    $" deviceClassSequences:>{performance.DeviceChannelSequences?.Count()}<" +
                    $" playSequencesInParallel:>{performance.ExecuteDeviceChannelSequencesInParallel}<" +
                    $" performances:>{performance.Performances?.Count()}<" +
                    $" playPerformancesInParallel:>{performance.ExecutePerformancesInParallel}<" +
                    $" performanceLoops:>{performance.PerformanceLoops}<" +
                    $" afterPerformanceLoopPerformances:>{performance.AfterPerformanceLoopPerformances?.Count()}<" +
                    $" duration:>{performance.Duration}<" +
                    $" nextPerformance:>{performance.NextPerformance?.Name}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
            }

            Performance? configuredPerf;

            if (performance.Name is not null)
            {
                configuredPerf = RetrieveAndConfigurePerformance(performance.Name, SerialNumber);
            }
            else
            {
                // Handle Performance without a name
                configuredPerf = CreateInlinePerformance(performance, SerialNumber);
            }

            if (configuredPerf is null)
            {
                Log.ERROR("Aborting RunPerformanceLoops configuredPerf is null", Common.LOG_CATEGORY);
                return;
            }

            // NOTE(crhodes)
            // Execute BeforePerformanceLoopPerformances[] if any

            if (configuredPerf.BeforePerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(configuredPerf.BeforePerformanceLoopPerformances);
            }

            // NOTE(crhodes)
            // Then Execute DeviceChannelSequences[] loops if any

            if (configuredPerf.DeviceChannelSequences is not null)
            {
                for (Int32 performanceLoop = 0; performanceLoop < configuredPerf.PerformanceLoops; performanceLoop++)
                {
                    if (configuredPerf.ExecuteDeviceChannelSequencesInParallel)
                    {                       
                        Parallel.ForEach(configuredPerf.DeviceChannelSequences, async deviceChannelSequence =>
                        {
                            // TODO(crhodes)
                            // If we have configured a SerialNumber override, we need to use that
                            // Otherwise we use the sequence SerialNumber

                            //DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                            //    configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber);

                            // TODO(crhodes)
                            // Need to handle case with SerialNumberHupPortChannel 

                            Int32? serialNumber = configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber;

                            if (serialNumber is null && deviceChannelSequence.SerialNumberHubPortChannelName is not null)
                            {

                                if (PerformanceLibrary.SerialNumberHubPortChannels.TryGetValue(
                                        deviceChannelSequence.SerialNumberHubPortChannelName,
                                        out SerialNumberHubPortChannel? serialNumberHubPortChannel))
                                {
                                    serialNumber = serialNumberHubPortChannel.SerialNumber;
                                    //nextPhidgetDeviceChannelSequence.HubPort = serialNumberHubPortChannel.HubPort;
                                    //nextPhidgetDeviceChannelSequence.Channel = serialNumberHubPortChannel.Channel;

                                    //if (LogDeviceChannelSequence) Log.TRACE($"Set from SerialNumberHubPortChannel:>{deviceChannelSequence.SerialNumberHubPortChannelName}<" +
                                    //    $" dcsSerialNumber:>{nextPhidgetDeviceChannelSequence.SerialNumber}<" +
                                    //    $" dcsHubPort:>{nextPhidgetDeviceChannelSequence.HubPort}<" +
                                    //    $" dcsChannel:>{nextPhidgetDeviceChannelSequence.Channel}<" +
                                    //    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                                }
                                else
                                {
                                    Log.ERROR($"Cannot find SerialNumberHubPortChannel:>{deviceChannelSequence.SerialNumberHubPortChannelName}<", Common.LOG_CATEGORY);
                                }
                            }
                            else
                            {
                                // TODO(crhodes)
                                // Throw error no serial number
                            }

                            DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                                serialNumber);

                            if (LogPerformance) Log.TRACE($"Parallel DeviceChannelSequences" +
                                $" performance:>{deviceChannelSequence.Name}<" +
                                $" configuredPerfSerialNumber:>{configuredPerf.SerialNumber}<" +
                                $" sequenceSerialNumber:>{deviceChannelSequence.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" hubPort:>{deviceChannelSequence.HubPort}<" +
                                $" channel:>{deviceChannelSequence.Channel}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{deviceChannelSequence.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await deviceChannelSequencePlayer.ExecuteDeviceChannelSequence(deviceChannelSequence);
                        });
                    }
                    else
                    {
                        foreach (DeviceChannelSequence deviceChannelSequence in configuredPerf.DeviceChannelSequences)
                        {
                            // TODO(crhodes)
                            // If we have configured a SerialNumber override, we need to use that
                            // Otherwise we use the sequence SerialNumber

                            //DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                            //    configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber);

                            // TODO(crhodes)
                            // Need to handle case with SerialNumberHubPortChannel 

                            Int32? serialNumber = configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber;

                            if (serialNumber is null && deviceChannelSequence.SerialNumberHubPortChannelName is not null)
                            {

                                if (PerformanceLibrary.SerialNumberHubPortChannels.TryGetValue(
                                        deviceChannelSequence.SerialNumberHubPortChannelName,
                                        out SerialNumberHubPortChannel? serialNumberHubPortChannel))
                                {
                                    serialNumber = serialNumberHubPortChannel.SerialNumber;
                                    //nextPhidgetDeviceChannelSequence.HubPort = serialNumberHubPortChannel.HubPort;
                                    //nextPhidgetDeviceChannelSequence.Channel = serialNumberHubPortChannel.Channel;

                                    //if (LogDeviceChannelSequence) Log.TRACE($"Set from SerialNumberHubPortChannel:>{deviceChannelSequence.SerialNumberHubPortChannelName}<" +
                                    //    $" dcsSerialNumber:>{nextPhidgetDeviceChannelSequence.SerialNumber}<" +
                                    //    $" dcsHubPort:>{nextPhidgetDeviceChannelSequence.HubPort}<" +
                                    //    $" dcsChannel:>{nextPhidgetDeviceChannelSequence.Channel}<" +
                                    //    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                                }
                                else
                                {
                                    Log.ERROR($"Cannot find SerialNumberHubPortChannel:>{deviceChannelSequence.SerialNumberHubPortChannelName}<", Common.LOG_CATEGORY);
                                }
                            }
                            else
                            {
                                // TODO(crhodes)
                                // Throw error no serial number
                            }

                            DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                                serialNumber);
                            if (LogPerformance) Log.TRACE($"Sequential DeviceChannelSequences" +
                                $" performance:>{deviceChannelSequence.Name}<" +
                                $" configuredPerfSerialNumber:>{configuredPerf.SerialNumber}<" +
                                $" dcsSerialNumber:>{deviceChannelSequence.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" hubPort:>{deviceChannelSequence.HubPort}<" +
                                $" channel:>{deviceChannelSequence.Channel}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{deviceChannelSequence.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await deviceChannelSequencePlayer.ExecuteDeviceChannelSequence(deviceChannelSequence);
                        }
                    }
                }
            }

            // NOTE(crhodes)
            // Then Execute Performances[] loops if any

            if (configuredPerf.Performances is not null)
            {
                for (Int32 performanceLoop = 0; performanceLoop < configuredPerf.PerformanceLoops; performanceLoop++)
                {
                    if (configuredPerf.ExecutePerformancesInParallel)
                    {
                        Parallel.ForEach(configuredPerf.Performances, async perf =>
                        {
                            PerformancePlayer performancePlayer = GetNewPerformancePlayer(perf.SerialNumber);

                            if (LogPerformance) Log.TRACE($"Parallel ExecutePerformance(>{perf.Name}<)" +
                                $" serialNumber:>{perf.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{perf.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await performancePlayer.ExecutePerformance(perf);
                        });
                    }
                    else
                    {
                        foreach (Performance perf in configuredPerf.Performances)
                        {
                            // HACK(crhodes)
                            // Ugh.  If playing sequentially, we need to set ourselves to the right serial number

                            SerialNumber = perf.SerialNumber;

                            if (LogPerformance) Log.TRACE($"Sequential ExecutePerformance(>{perf.Name}<)" +
                                $" serialNumber:>{perf.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{perf.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await ExecutePerformance(perf);
                        }
                    }
                }
            }

            // NOTE(crhodes)
            // Then Execute AfterPerformanceLoopPerformances[] if any

            if (configuredPerf.AfterPerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(configuredPerf.AfterPerformanceLoopPerformances);
            }

            if (performance.Duration is not null)
            {
                if (LogPerformance)
                {
                    Log.TRACE($"Zzzz - End of Performance:>{performance.Name}<" +
                        $" Sleeping:>{performance.Duration}<", Common.LOG_CATEGORY);
                }
                Thread.Sleep((Int32)performance.Duration);
            }

            // NOTE(crhodes)
            // Play the nextPerformance if any

            if (configuredPerf.NextPerformance is not null)
            {
                await ExecutePerformance(configuredPerf.NextPerformance);
            }

            if (LogPerformance) Log.TRACE($"Exit" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);
        }

        private Performance CreateInlinePerformance(Performance performance, int? serialNumber)
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.TRACE($"Enter" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

            Performance createdPerformance = new Performance(performance);

            if (serialNumber is not null)
            {
                createdPerformance.SerialNumber = serialNumber;
            }

            if (LogPerformance) Log.TRACE($"Exit" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);

            return createdPerformance;
        }

        private Performance? RetrieveAndConfigurePerformance(string performanceName, Int32? serialNumber)
        {
            Int64 startTicks = 0;
            var ads = PerformanceLibrary.AvailableDeviceSettings;
            var ap = PerformanceLibrary.AvailablePerformances;
            var aperf = PerformanceLibrary.AllPerformances;

            if (PerformanceLibrary.AllPerformances.TryGetValue(performanceName, out Performance? retrievedPerfrormance))
            {
                if (LogPerformance) startTicks = Log.TRACE($"Retrieved performance:>{retrievedPerfrormance.Name}<" +
                    $" serialNumber:>{retrievedPerfrormance.SerialNumber}< serialNumber?:>{serialNumber}<" +
                    $" duration:>{retrievedPerfrormance.Duration}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                // NOTE(crhodes)
                // Create a new performance so we do not modify the one in the dictionary.

                Performance configuredPerformance = new Performance(retrievedPerfrormance);

                if (serialNumber is not null)
                {
                    configuredPerformance.SerialNumber = serialNumber;
                }

                if (LogPerformance) Log.TRACE($"Configured performance:>{retrievedPerfrormance.Name}<" +
                    $" serialNumber:>{configuredPerformance.SerialNumber}<" +
                    $" duration:>{configuredPerformance.Duration}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);

                return configuredPerformance;
            }
            else
            {
                Log.ERROR($"Cannot find performance:>{performanceName}<", Common.LOG_CATEGORY);
                return null;
            }
        }

        private async Task ExecutePerfomanceSequences(Performance[] performanceSequences)
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.TRACE($"performanceSequences.Count:>{performanceSequences.Count()}<", Common.LOG_CATEGORY);

            foreach (Performance callPerformance in performanceSequences)
            {
                //Performance? nextPerformance = null;

                // TODO(crhodes)
                // Do we want to handle embedded InLine Performance without a name?

                if (PerformanceLibrary.AllPerformances.TryGetValue(callPerformance.Name, out Performance? nextPerformance))
                {
                    //nextPerformance = value;

                    await ExecutePerformance(nextPerformance);

                    // TODO(crhodes)
                    // Should we process Next Performance if exists.
                    // Recursive implications need to be considered.
                    // May have to detect loops.

                    nextPerformance = nextPerformance?.NextPerformance;
                }
                else
                {
                    Log.ERROR($"Cannot find performance:>{callPerformance?.Name}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }

            if (LogPerformance) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        private PerformancePlayer GetNewPerformancePlayer(Int32? serialNumber = null)
        {
            //Int64 startTicks = 0;
            //if (LogPerformance) startTicks = Log.TRACE($"Enter", Common.LOG_CATEGORY);

            PerformancePlayer player = new PerformancePlayer(EventAggregator, PerformanceLibrary, serialNumber);

            player.LogPerformance = LogPerformance;
            player.LogPhidgetEvents = LogPhidgetEvents;
            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            //if (LogPerformance) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);
            // NOTE(crhodes)
            // We log so we can see ThreadID
            if (LogPerformance) Log.TRACE($"Enter/Exit serialNumber:>{serialNumber}<" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

            return player;
        }

        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer(Int32? serialNumber = null)
        {
            //Int64 startTicks = 0;
            //if (LogPerformance) startTicks = Log.TRACE($"Enter", Common.LOG_CATEGORY);

            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(EventAggregator, serialNumber);

            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            player.LogCurrentChangeEvents = LogCurrentChangeEvents;
            player.LogPositionChangeEvents = LogPositionChangeEvents;
            player.LogVelocityChangeEvents = LogVelocityChangeEvents;
            player.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            player.LogInputChangeEvents = LogInputChangeEvents;
            player.LogOutputChangeEvents = LogOutputChangeEvents;

            player.LogSensorChangeEvents = LogSensorChangeEvents;

            player.LogPhidgetEvents = LogPhidgetEvents;

            //if (LogPerformance) Log.TRACE("Exit", Common.LOG_CATEGORY, startTicks);
            // NOTE(crhodes)
            // We log so we can see ThreadID
            if (LogPerformance) Log.TRACE($"Enter/Exit serialNumber:>{serialNumber}<" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

            return player;
        }

        #endregion
    }
}
