using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

using Prism.Events;

using VNC.Core.Mvvm;

using VNC.Phidget22.Configuration;

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

            //PerformanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);
            //PerformanceSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;
            //PerformanceSequencePlayer.LogPerformanceSequence = LogPerformanceSequence;
            //PerformanceSequencePlayer.LogSequenceAction = LogSequenceAction;
            //PerformanceSequencePlayer.LogActionVerification = LogActionVerification;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        // NOTE(crhodes)
        // Don't think we need INPC on these

        private bool _logPerformance = false;
        public bool LogPerformance
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

        private bool _logPerformanceSequence = false;
        public bool LogPerformanceSequence
        {
            get => _logPerformanceSequence;
            set
            {
                if (_logPerformanceSequence == value)
                    return;
                _logPerformanceSequence = value;
                OnPropertyChanged();
            }
        }

        private bool _logSequenceAction = false;
        public bool LogSequenceAction
        {
            get => _logSequenceAction;
            set
            {
                if (_logSequenceAction == value)
                    return;
                _logSequenceAction = value;
                OnPropertyChanged();
            }
        }

        private bool _logActionVerification = false;
        public bool LogActionVerification
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

        #region Phidget Device Logging Events

        private bool _logPhidgetEvents = false;

        public bool LogPhidgetEvents
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

        private bool _logCurrentChangeEvents = false;
        public bool LogCurrentChangeEvents
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

        private bool _logPositionChangeEvents = false;
        public bool LogPositionChangeEvents
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

        private bool _logVelocityChangeEvents = false;
        public bool LogVelocityChangeEvents
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

        #endregion

        #region InterfaceKit

        private bool _displayInputChangeEvents = false;

        public bool LogInputChangeEvents
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

        private bool _displayOutputChangeEvents = false;

        public bool LogOutputChangeEvents
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

        private bool _sensorChangeEvents = false;

        public bool LogSensorChangeEvents
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

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public async Task RunPerformanceLoops(Performance performance)
        {
            long startTicks = 0;

            if (LogPerformance)
            {
                startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

                Log.Trace($"Running performance:{performance.Name} description:{performance.Description}" +
                    $" beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                    $" performanceSequences:{performance.PerformanceSequences?.Count()} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                    $" performances:{performance.Performances?.Count()} playPerformancesInParallel:{performance.PlayPerformancesInParallel}" +
                    $" afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                    $" loops:{performance.PerformanceLoops} duration:{performance.Duration}" +
                    $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
            }

            // NOTE(crhodes)
            // Execute BeforePerformanceLoopPerformances if any

            if (performance.BeforePerformanceLoopPerformances is not null)
            {
                await ExecutePerformanceSequences(performance.BeforePerformanceLoopPerformances);
            }

            // NOTE(crhodes)
            // Then Execute PerformanceSequence loops

            if (performance.PerformanceSequences is not null)
            {
                // TODO(crhodes)
                // Maybe create a new PerformanceSequencePlayer
                // instead of reaching for the Property.  If we do that have to initialize all the logging.

                //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

                PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

                for (int performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
                {
                    if (performance.PlaySequencesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.PerformanceSequences, async sequence =>
                        {
                            await performanceSequencePlayer.ExecutePerformanceSequence(sequence);
                        });
                    }
                    else
                    {
                        if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        foreach (PerformanceSequence sequence in performance.PerformanceSequences)
                        {
                            // TODO(crhodes)
                            // What is this loop doing?  Doesn't PerformanceSequencePlayer handle looping

                            //for (int sequenceLoop = 0; sequenceLoop < sequence.SequenceLoops; sequenceLoop++)
                            //{
                                await performanceSequencePlayer.ExecutePerformanceSequence(sequence);
                            //}
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
                    Thread.Sleep((int)performance.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute Performances loops

            if (performance.Performances is not null)
            {
                // TODO(crhodes)
                // Maybe create a new PerformancePlayer
                // instead of reaching for the Property.  If we do that have to initialize all the logging.

                //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

                PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

                for (int performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
                {
                    if (performance.PlayPerformancesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.Performances, async perf =>
                        {
                            if (PerformanceLibrary.AvailablePerformances.ContainsKey(perf.Name ?? ""))
                            {
                                Performance loadedPerf = PerformanceLibrary.AvailablePerformances[perf.Name];

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
                    Thread.Sleep((int)performance.Duration);
                }
            }

            // NOTE(crhodes)
            // Then Execute AfterPerformanceLoopPerformances if any

            if (performance.AfterPerformanceLoopPerformances is not null)
            {
                await ExecutePerformanceSequences(performance.AfterPerformanceLoopPerformances);
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task ExecutePerformanceSequences(Performance[] performanceSequences)
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
                    Log.Error($"Cannot find performance:>{nextPerformance?.Name}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (None)



        #endregion

        #region Private Methods

        private PerformanceSequencePlayer GetPerformanceSequencePlayer()
        {
            Int64 startTicks = 0;
            if (LogPerformance) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            if (ActivePerformanceSequencePlayer == null)
            {
                ActivePerformanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);
            }

            ActivePerformanceSequencePlayer.LogPerformanceSequence = LogPerformanceSequence;
            ActivePerformanceSequencePlayer.LogSequenceAction = LogSequenceAction;
            ActivePerformanceSequencePlayer.LogActionVerification = LogActionVerification;

            ActivePerformanceSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
            ActivePerformanceSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
            ActivePerformanceSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;

            ActivePerformanceSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
            ActivePerformanceSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;

            ActivePerformanceSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

            ActivePerformanceSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;

            if (LogPerformance) Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);

            return ActivePerformanceSequencePlayer;
        }

        #endregion
    }
}
