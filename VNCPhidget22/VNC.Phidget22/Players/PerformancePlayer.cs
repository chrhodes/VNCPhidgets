﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNC.Core.Mvvm;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;

namespace VNC.Phidget22.Players
{
    public class PerformancePlayer : INPCBase
    {
        #region Constructors, Initialization, and Load
        
        public IEventAggregator EventAggregator { get; set; }

        public PerformancePlayer(IEventAggregator eventAggregator)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        public DeviceChannelSequencePlayer ActivePerformanceSequencePlayer { get; set; }

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
                        $"\r beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                        $"\r deviceClassSequences:{performance.DeviceChannelSequences?.Count()} playSequencesInParallel:{performance.PlayDeviceChannelSequencesInParallel}" +
                        $"\r performances:{performance.Performances?.Count()} playPerformancesInParallel:{performance.PlayPerformancesInParallel}" +
                        $"\r loops:{performance.PerformanceLoops}" +
                        $"\r afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                        $"\r nextPerformance:>{performance.NextPerformance?.Name}<", Common.LOG_CATEGORY);
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
                startTicks = Log.Trace($"Enter RunPerformanceLoops(:>{performance.Name}<) description:>{performance.Description}< serialNumber:>{performance.SerialNumber}<" +
                    $"\r beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                    $"\r deviceClassSequences:{performance.DeviceChannelSequences?.Count()} playSequencesInParallel:{performance.PlayDeviceChannelSequencesInParallel}" +
                    $"\r performances:{performance.Performances?.Count()} playPerformancesInParallel:{performance.PlayPerformancesInParallel}" +
                    $"\r loops:{performance.PerformanceLoops}" +
                    $"\r afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                    $"\r nextPerformance:>{performance.NextPerformance?.Name}<", Common.LOG_CATEGORY);
            }

            // NOTE(crhodes)
            // Execute BeforePerformanceLoopPerformances if any

            if (performance.BeforePerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(performance.BeforePerformanceLoopPerformances);
            }

            // NOTE(crhodes)
            // Then Execute DeviceChannelSequences loops if any

            if (performance.DeviceChannelSequences is not null)
            {
                for (Int32 performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
                {
                    if (performance.PlayDeviceChannelSequencesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.DeviceChannelSequences, async sequence =>
                        {
                            // TODO(crhodes)
                            // Maybe create a new DeviceChannelSequencePlayer
                            // instead of reaching for the Property.  If we do that have to initialize all the logging.
                            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();

                            await player.ExecuteDeviceChannelSequence(sequence, performance.SerialNumber);
                        });
                    }
                    else
                    {
                        if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        DeviceChannelSequencePlayer player = GetPerformanceSequencePlayer();

                        foreach (DeviceChannelSequence sequence in performance.DeviceChannelSequences)
                        {
                            await player.ExecuteDeviceChannelSequence(sequence, performance.SerialNumber);
                        }
                    }
                }

                // NOTE(crhodes)
                // Then Sleep if necessary before next loop

                if (performance.Duration is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Zzzzz End of Performance Sleeping:>{performance.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)performance.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute Performances loops if any

            if (performance.Performances is not null)
            {
                // TODO(crhodes)
                // Maybe create a new PerformancePlayer
                // instead of reaching for the Property.  If we do that have to initialize all the logging.

                //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

                DeviceChannelSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

                for (Int32 performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
                {
                    if (performance.PlayPerformancesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.Performances, async perf =>
                        {
                            if (PerformanceLibrary.AvailablePerformances.ContainsKey(perf.Name ?? ""))
                            {
                                Performance loadedPerf = PerformanceLibrary.AvailablePerformances[perf.Name];

                                if (perf.SerialNumber is not null)
                                {
                                    loadedPerf.SerialNumber = perf.SerialNumber;
                                }

                                await RunPerformanceLoops(loadedPerf);
                            }
                            else
                            {
                                Log.Error($"Cannot find performance:>{perf.Name}<", Common.LOG_CATEGORY);
                            }
                        });
                    }
                    else
                    {
                        if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        foreach (Performance perf in performance.Performances)
                        {
                            if (PerformanceLibrary.AvailablePerformances.ContainsKey(perf.Name ?? ""))
                            {
                                Performance loadedPerf = PerformanceLibrary.AvailablePerformances[perf.Name];

                                if (perf.SerialNumber is not null)
                                {
                                    loadedPerf.SerialNumber = perf.SerialNumber;
                                }

                                await RunPerformanceLoops(loadedPerf);
                            }
                            else
                            {
                                Log.Error($"Cannot find performance:>{perf.Name}<", Common.LOG_CATEGORY);
                            }
                       }
                    }
                }

                // NOTE(crhodes)
                // Then Sleep if necessary before next loop

                if (performance.Duration is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Zzzzz End of Performance Sleeping:>{performance.Duration}<", Common.LOG_CATEGORY);
                    }
                    Thread.Sleep((Int32)performance.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute AfterPerformanceLoopPerformances if any

            if (performance.AfterPerformanceLoopPerformances is not null)
            {
                await ExecutePerfomanceSequences(performance.AfterPerformanceLoopPerformances);
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task ExecutePerfomanceSequences(Performance[] performanceSequences)
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.Trace($"performanceSequences.Count:{performanceSequences.Count()}", Common.LOG_CATEGORY);

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

        private DeviceChannelSequencePlayer GetPerformanceSequencePlayer()
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            if (ActivePerformanceSequencePlayer == null)
            {
                ActivePerformanceSequencePlayer = new DeviceChannelSequencePlayer(EventAggregator);
            }

            ActivePerformanceSequencePlayer.LogDeviceChannelSequence = LogDeviceChannelSequence;
            ActivePerformanceSequencePlayer.LogChannelAction = LogChannelAction;
            ActivePerformanceSequencePlayer.LogActionVerification = LogActionVerification;

            ActivePerformanceSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
            ActivePerformanceSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
            ActivePerformanceSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;
            ActivePerformanceSequencePlayer.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            ActivePerformanceSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
            ActivePerformanceSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;

            ActivePerformanceSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

            ActivePerformanceSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);

            return ActivePerformanceSequencePlayer;
        }

        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer()
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            DeviceChannelSequencePlayer player = new DeviceChannelSequencePlayer(EventAggregator);

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

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);

            return player;
        }

        #endregion
    }
}
