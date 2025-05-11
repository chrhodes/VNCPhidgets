using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
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

        public PerformancePlayer(IEventAggregator eventAggregator, Int32? serialNumber = null)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;
            SerialNumber = serialNumber;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        public Int32? SerialNumber { get; set; } = null;

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

            // NOTE(crhodes)
            // Why would we need to check given UI brought us here.
            // Might be useful generally

            //if (AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
            //{ 

            //}

            while (nextPerformance is not null)
            {
                if (LogPerformance)
                {
                    Log.Trace($"PlayPerformance(>{performance.Name}<) description:>{performance.Description}<" +
                        $" beforePerformanceLoopPerformances:>{performance.BeforePerformanceLoopPerformances?.Count()}<" +
                        $" deviceChannelSequences:>{performance.DeviceChannelSequences?.Count()}<" +
                        $" playSequencesInParallel:>{performance.PlayDeviceChannelSequencesInParallel}<" +
                        $" performances:>{performance.Performances?.Count()}<" +
                        $" playPerformancesInParallel:>{performance.PlayPerformancesInParallel}<" +
                        $" loops:>{performance.PerformanceLoops}<" +
                        $" afterPerformanceLoopPerformances:>{performance.AfterPerformanceLoopPerformances?.Count()}<" +
                        $" nextPerformance:>{performance.NextPerformance?.Name}<" +
                        $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
                }

                if (PerformanceLibrary.AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
                {
                    nextPerformance = PerformanceLibrary.AvailablePerformances[nextPerformance.Name];

                    await RunPerformanceLoops(nextPerformance);

                    nextPerformance = nextPerformance?.NextPerformance;
                }
                else
                {
                    Log.Error($"Cannot find performance:>{nextPerformance.Name}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }
        }

        public async Task RunPerformanceLoops(Performance performance)
        {
            long startTicks = 0;

            if (LogPerformance)
            {
                startTicks = Log.Trace($"Enter RunPerformanceLoops(:>{performance.Name}<)" +
                    $" description:>{performance.Description}<" +
                    $" playerSerialNumber:>{SerialNumber}<" +
                    $" serialNumber:>{performance.SerialNumber}<" +
                    $" beforePerformanceLoopPerformances:>{performance.BeforePerformanceLoopPerformances?.Count()}<" +
                    $" deviceClassSequences:>{performance.DeviceChannelSequences?.Count()}<" +
                    $" playSequencesInParallel:>{performance.PlayDeviceChannelSequencesInParallel}<" +
                    $" performances:>{performance.Performances?.Count()}<" +
                    $" playPerformancesInParallel:>{performance.PlayPerformancesInParallel}<" +
                    $" loops:>{performance.PerformanceLoops}<" +
                    $" afterPerformanceLoopPerformances:>{performance.AfterPerformanceLoopPerformances?.Count()}<" +
                    $" duration:>{performance.Duration}<" +
                    $" nextPerformance:>{performance.NextPerformance?.Name}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);
            }

            // TODO(crhodes)
            // This is likely where to handle performance without a name

            Performance configuredPerf = RetrieveAndConfigurePerformance(performance.Name, SerialNumber);

            if (configuredPerf is null)
            {
                Log.Error("Aborting RunPerformanceLoops", Common.LOG_CATEGORY);
                return;
            }

            // NOTE(crhodes)
            // Execute BeforePerformanceLoopPerformances if any

            if (configuredPerf.BeforePerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(configuredPerf.BeforePerformanceLoopPerformances);
            }

            // NOTE(crhodes)
            // Then Execute DeviceChannelSequences loops if any

            if (configuredPerf.DeviceChannelSequences is not null)
            {
                for (Int32 performanceLoop = 0; performanceLoop < configuredPerf.PerformanceLoops; performanceLoop++)
                {
                    if (configuredPerf.PlayDeviceChannelSequencesInParallel)
                    {                       
                        Parallel.ForEach(configuredPerf.DeviceChannelSequences, async deviceChannelSequence =>
                        {
                            // TODO(crhodes)
                            // If we have configured a SerialNumber override, we need to use that
                            // Otherwise we use the sequence SerialNumber

                            DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                                configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber);
                            //DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(configuredPerf.SerialNumber);

                            if (LogPerformance) Log.Trace($"Parallel DeviceChannelSequences" +
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

                            // NOTE(crhodes)
                            // Don't think it makes sense if parallel DeviceChannelSequences

                            //if (deviceChannelSequence.Duration is not null)
                            //{
                            //    if (LogPerformance)
                            //    {
                            //        Log.Trace($"Zz-z-z End of DeviceChanelSequence Sleeping:>{deviceChannelSequence.Duration}<", Common.LOG_CATEGORY);
                            //    }
                            //    Thread.Sleep((Int32)deviceChannelSequence.Duration);
                            //}
                        });
                    }
                    else
                    {
                        foreach (DeviceChannelSequence deviceChannelSequence in configuredPerf.DeviceChannelSequences)
                        {
                            // TODO(crhodes)
                            // If we have configured a SerialNumber override, we need to use that
                            // Otherwise we use the sequence SerialNumber

                            DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(
                                configuredPerf.SerialNumber.HasValue ? configuredPerf.SerialNumber : deviceChannelSequence.SerialNumber);
                            //DeviceChannelSequencePlayer deviceChannelSequencePlayer = GetNewDeviceChannelSequencePlayer(configuredPerf.SerialNumber);

                            if (LogPerformance) Log.Trace($"Sequential DeviceChannelSequences" +
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

                            // NOTE(crhodes)
                            // Then Sleep if necessary before next sequential DeviceChannelSequence

                            if (deviceChannelSequence.Duration is not null)
                            {
                                if (LogPerformance)
                                {
                                    Log.Trace($"Zz-z-z End of DeviceChanelSequence Sleeping:>{deviceChannelSequence.Duration}<", Common.LOG_CATEGORY);
                                }
                                Thread.Sleep((Int32)deviceChannelSequence.Duration);
                            }
                        }
                    }
                }

                // NOTE(crhodes)
                // Then Sleep if necessary before next loop

                if (configuredPerf.Duration is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Zzzzz End of DeviceChanelSequences Sleeping:>{configuredPerf.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)configuredPerf.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute Performances loops if any

            if (configuredPerf.Performances is not null)
            {
                for (Int32 performanceLoop = 0; performanceLoop < configuredPerf.PerformanceLoops; performanceLoop++)
                {
                    if (configuredPerf.PlayPerformancesInParallel)
                    {
                        Parallel.ForEach(configuredPerf.Performances, async perf =>
                        {
                            PerformancePlayer performancePlayer = GetNewPerformancePlayer(perf.SerialNumber);

                            if (LogPerformance) Log.Trace($"Parallel Performance" +
                                $" performance:>{perf.Name}< " +
                                $" serialNumber:>{perf.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{performance.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await performancePlayer.RunPerformanceLoops(perf);

                            // NOTE(crhodes)
                            // Don't think it makes sense if parallel performances

                            //if (configuredPerf.Duration is not null)
                            //{
                            //    if (LogPerformance)
                            //    {
                            //        Log.Trace($"Zzzzz End of Performance Sleeping:>{configuredPerf.Duration}<", Common.LOG_CATEGORY);
                            //    }
                            //    Thread.Sleep((Int32)configuredPerf.Duration);
                            //}
                        });
                    }
                    else
                    {
                        foreach (Performance perf in configuredPerf.Performances)
                        {
                            // HACK(crhodes)
                            // Ugh.  If playing sequentially, we need to set ourselves to the right serial number

                            SerialNumber = perf.SerialNumber;

                            if (LogPerformance) Log.Trace($"Sequential Performance" +
                                $" performance:>{perf.Name}< " +
                                $" serialNumber:>{perf.SerialNumber}<" +
                                $" performancePlayerSerialNumber:>{SerialNumber}<" +
                                $" performanceLoop:>{performanceLoop + 1}<" +
                                $" duration:>{performance.Duration}<" +
                                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                            await RunPerformanceLoops(perf);

                            // NOTE(crhodes)
                            // Then Sleep if necessary before next sequential performance

                            if (configuredPerf.Duration is not null)
                            {
                                if (LogPerformance)
                                {
                                    Log.Trace($"Zz-z-z End of Performance Sleeping:>{configuredPerf.Duration}<", Common.LOG_CATEGORY);
                                }
                                Thread.Sleep((Int32)configuredPerf.Duration);
                            }
                        }
                    }
                }

                // NOTE(crhodes)
                // Then Sleep if necessary before next loop

                if (configuredPerf.Duration is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Zzzzz End of Performances Sleeping:>{configuredPerf.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)configuredPerf.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute AfterPerformanceLoopPerformances if any

            if (configuredPerf.AfterPerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(configuredPerf.AfterPerformanceLoopPerformances);
            }

            if (LogPerformance) Log.Trace($"Exit" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);
        }

        private Performance? RetrieveAndConfigurePerformance(string performanceName, Int32? serialNumber)
        {
            Int64 startTicks = 0;

            if (PerformanceLibrary.AvailablePerformances.ContainsKey(performanceName ?? ""))
            {
                Performance retrievedPerfrormance = PerformanceLibrary.AvailablePerformances[performanceName];

                if (LogPerformance) startTicks = Log.Trace($"Retrieved  performance:>{retrievedPerfrormance.Name}<" +
                    $" serialNumber:>{retrievedPerfrormance.SerialNumber}< serialNumber?:>{serialNumber}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

                Performance configuredPerformance = new Performance(retrievedPerfrormance);

                //Performance performance = PerformanceLibrary.AvailablePerformances[performanceName];

                if (serialNumber is not null)
                {
                    configuredPerformance.SerialNumber = serialNumber;
                }

                if (LogPerformance) Log.Trace($"Configured performance:>{retrievedPerfrormance.Name}<" +
                    $" serialNumber:>{configuredPerformance.SerialNumber}<" +
                    $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY, startTicks);

                return configuredPerformance;
            }
            else
            {
                Log.Error($"Cannot find performance:>{performanceName}<", Common.LOG_CATEGORY);
                return null;
            }
        }

        private async Task ExecutePerfomanceSequences(Performance[] performanceSequences)
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.Trace($"performanceSequences.Count:>{performanceSequences.Count()}<", Common.LOG_CATEGORY);

            foreach (Performance callPerformance in performanceSequences)
            {
                Performance nextPerformance = null;

                if (PerformanceLibrary.AvailablePerformances.ContainsKey(callPerformance.Name ?? ""))
                {
                    nextPerformance = PerformanceLibrary.AvailablePerformances[callPerformance.Name];

                    await RunPerformanceLoops(nextPerformance);

                    // TODO(crhodes)
                    // Should we process Next Performance if exists.
                    // Recursive implications need to be considered.
                    // May have to detect loops.

                    nextPerformance = nextPerformance?.NextPerformance;
                }
                else
                {
                    Log.Error($"Cannot find performance:>{callPerformance?.Name}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        //private DeviceChannelSequencePlayer GetPerformanceSequencePlayer()
        //{
        //    Int64 startTicks = 0;
        //    if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

        //    if (ActivePerformanceSequencePlayer == null)
        //    {
        //        ActivePerformanceSequencePlayer = new DeviceChannelSequencePlayer(EventAggregator);
        //    }

        //    ActivePerformanceSequencePlayer.LogDeviceChannelSequence = LogDeviceChannelSequence;
        //    ActivePerformanceSequencePlayer.LogChannelAction = LogChannelAction;
        //    ActivePerformanceSequencePlayer.LogActionVerification = LogActionVerification;

        //    ActivePerformanceSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //    ActivePerformanceSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
        //    ActivePerformanceSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;
        //    ActivePerformanceSequencePlayer.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

        //    ActivePerformanceSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
        //    ActivePerformanceSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;

        //    ActivePerformanceSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

        //    ActivePerformanceSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;

        //    if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);

        //    return ActivePerformanceSequencePlayer;
        //}

        private PerformancePlayer GetNewPerformancePlayer(Int32? serialNumber = null)
        {
            //Int64 startTicks = 0;
            //if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            PerformancePlayer player = new PerformancePlayer(EventAggregator, serialNumber);

            player.LogPerformance = LogPerformance;
            player.LogPhidgetEvents = LogPhidgetEvents;
            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            //if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            // NOTE(crhodes)
            // We log so we can see ThreadID
            if (LogPerformance) Log.Trace($"Enter/Exit serialNumber:>{serialNumber}<" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

            return player;
        }

        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer(Int32? serialNumber = null)
        {
            //Int64 startTicks = 0;
            //if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

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

            //if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
            // NOTE(crhodes)
            // We log so we can see ThreadID
            if (LogPerformance) Log.Trace($"Enter/Exit serialNumber:>{serialNumber}<" +
                $" thread:>{System.Environment.CurrentManagedThreadId}<", Common.LOG_CATEGORY);

            return player;
        }

        #endregion
    }
}
