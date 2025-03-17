using System;

namespace VNC.Phidget22.Configuration.Performance
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
        /// Performance[] to call before executing DeviceClassSequences[] and/or Performances[]
        /// before calling NextPerformance
        /// </summary>
        public Performance[]? BeforePerformanceLoopPerformances { get; set; }

        /// <summary>
        /// Number of loops of DeviceClassSequences[] and/or Performances[]
        /// </summary>
        public Int32 PerformanceLoops { get; set; } = 1;

        /// <summary>
        /// Play DeviceClassSequences in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlaySequencesInParallel { get; set; } = false;

        /// <summary>
        /// SerialNumber of PhidgetDevice that will run DeviceClassSequences
        /// If null, Name is used to lookup DeviceClassSequence 
        /// which must specify a SerialNumber
        /// </summary>
        public Int32? SerialNumber { get; set; } = null;

        /// <summary>
        /// DeviceClassSequences to execute as part of this performance
        /// </summary>
        public DeviceChannelSequence[]? DeviceClassSequences { get; set; }

        /// <summary>
        /// Play Performances in Parallel or Sequential (false)
        /// </summary>
        public Boolean PlayPerformancesInParallel { get; set; } = false;

        /// <summary>
        /// Performances to execute as part of this performance
        /// </summary>
        public Performance[]? Performances { get; set; }

        /// <summary>
        /// Duration in ms of sleep time after DeviceClassSequences[] or Performances[] completed
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Performance[] to call after executing DeviceClassSequences[]
        /// before calling NextSequence
        /// </summary>
        public Performance[]? AfterPerformanceLoopPerformances { get; set; }

        /// <summary>
        /// Performance to invoke at end of Loops of DeviceClassSequences or Performances
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; } = null;
    }
}
