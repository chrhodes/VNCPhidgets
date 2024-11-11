using System;

namespace VNC.Phidget22.Configuration
{
    public class PhidgetSequenceBase
    {
        ///// <summary>
        ///// Host on which to run sequence (optional)
        ///// </summary>
        //public Host? Host { get; set; }

        /// <summary>
        /// Phidget on which to run sequence (optional)
        /// Use current Phidget if null
        /// </summary>
        //public Int32? SerialNumber { get; set; }

        /// <summary>
        /// Name of sequence
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// Description of sequence (optional)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Description of sequence (optional)
        /// </summary>
        public string? UsageNotes { get; set; }

        /// <summary>
        /// Number of loops of sequence to execute
        /// </summary>
        public Int32 SequenceLoops { get; set; } = 1;

        /// <summary>
        /// PerformanceSequence[] to call before executing Actions
        /// </summary>
        public PerformanceSequence[]? BeforeActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call at start of each SequenceLoop
        /// </summary>
        public PerformanceSequence[]? StartActionLoopSequences { get; set; }

        /// <summary>
        /// Number of loops of sequence Actions[] to execute
        /// </summary>
        public Int32 ActionLoops { get; set; } = 1;

        /// <summary>
        /// Play Actions[] in Parallel or Sequentially (false)
        /// </summary>
        public Boolean ExecuteActionsInParallel { get; set; } = false;

        /// <summary>
        /// Duration of Action[] in ms (sleep time after Actions completed)
        /// </summary>
        public Int32? ActionsDuration { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call at start of each SequenceLoop
        /// </summary>
        public PerformanceSequence[]? EndActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call after executing Actions
        /// before calling NextSequence
        /// </summary>
        public PerformanceSequence[]? AfterActionLoopSequences { get; set; }

        /// <summary>
        /// Duration of Action[] in ms (sleep time after Actions completed)
        /// </summary>
        public Int32? SequenceDuration { get; set; }

        /// <summary>
        /// PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public PerformanceSequence? NextSequence { get; set; }
    }
}
