using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Prism.Events;

using VNCPhidget21.Configuration;

namespace VNC.Phidget.Players
{
    public class PerformancePlayer
    {
        #region Constructors, Initialization, and Load
        public IEventAggregator EventAggregator { get; set; }

        public PerformancePlayer(IEventAggregator eventAggregator)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            EventAggregator = eventAggregator;

            PerformanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public bool LogPerformance { get; set; }

        public PerformanceSequencePlayer PerformanceSequencePlayer { get; set; }

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
            // Then Execute PerformanceSequences loops
            //
            // Not sure there would ever be no PerformanceSequences

            if (performance.PerformanceSequences is not null)
            {
                // TODO(crhodes)
                // Mabye create a new PerformanceSequencePlayer
                // instead of reaching for the Property.

                PerformanceSequencePlayer performanceSequence = new PerformanceSequencePlayer(EventAggregator);

                for (int performanceLoop = 0; performanceLoop < performance.PerformanceLoops; performanceLoop++)
                {
                    if (performance.PlaySequencesInParallel)
                    {
                        if (LogPerformance) Log.Trace($"Parallel Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        Parallel.ForEach(performance.PerformanceSequences, async sequence =>
                        {
                            await PerformanceSequencePlayer.ExecutePerformanceSequence(sequence);
                        });
                    }
                    else
                    {
                        if (LogPerformance) Log.Trace($"Sequential Actions performanceLoop:{performanceLoop + 1}", Common.LOG_CATEGORY);

                        foreach (PerformanceSequence sequence in performance.PerformanceSequences)
                        {
                            for (int sequenceLoop = 0; sequenceLoop < sequence.SequenceLoops; sequenceLoop++)
                            {
                                await PerformanceSequencePlayer.ExecutePerformanceSequence(sequence);
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
            foreach (Performance callPerformance in performanceSequences)
            {
                Performance nextPerformance = null;

                if (PerformanceLibrary.AvailablePerformances.ContainsKey(callPerformance.Name ?? ""))
                {
                    nextPerformance = PerformanceLibrary.AvailablePerformances[callPerformance.Name];

                    await RunPerformanceLoops(nextPerformance);

                    // TODO(crhodes)
                    // Should we process Next Performance if exists.  Recursive implications need to be considered.
                    // May have to detect loops.

                    nextPerformance = nextPerformance?.NextPerformance;
                }
                else
                {
                    Log.Error($"Cannot find performance:>{nextPerformance?.Name}<", Common.LOG_CATEGORY);
                    nextPerformance = null;
                }
            }
        }

        #endregion

        #region Protected Methods (None)



        #endregion

        #region Private Methods (None)



        #endregion
    }
}
