using System;

namespace VNC.Phidget22.Configuration
{
    public class Performance
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "PERFORMANCE NAME";

        /// <summary>
        /// Description of Performance
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Performance[] to call before executing PerformanceSequences[] and/or Performances[]
        /// before calling NextPerformance
        /// </summary>
        public Performance[]? BeforePerformanceLoopPerformances { get; set; }

        /// <summary>
        /// Number of loops of PerformanceSequences[] and/or Performances[]
        /// </summary>
        public Int32 PerformanceLoops { get; set; } = 1;

        /// <summary>
        /// Play PerformanceSequences in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlaySequencesInParallel { get; set; } = false;

        public PerformanceSequence[]? PerformanceSequences { get; set; }

        /// <summary>
        /// Play Performances in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayPerformancesInParallel { get; set; } = false;

        public Performance[]? Performances { get; set; }

        /// <summary>
        /// Duration in ms of sleep time after PerformanceSequences[] or Performances[] completed
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Performance[] to call after executing PerformanceSequences[]
        /// before calling NextSequence
        /// </summary>
        public Performance[]? AfterPerformanceLoopPerformances { get; set; }

        /// <summary>
        /// Performance to invoke at end of Loops of PerformanceSequences or Performances
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; }
    }
}
