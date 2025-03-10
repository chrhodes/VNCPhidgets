using System;

namespace VNC.Phidget22.Configuration.Performance
{
    public class ChannelClassSequence
    {
        public ChannelClassSequence(string channelClass)
        {
            ChannelClass = channelClass;
        }

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

        public string ChannelClass { get; } = "Generic";

        /// <summary>
        /// PhidgetChannel
        /// </summary>
        public int Channel { get; set; }

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
        public int SequenceLoops { get; set; } = 1;

        /// <summary>
        /// PerformanceSequence[] to call before executing Actions
        /// </summary>
        public PhidgetDeviceClassSequence[]? BeforeActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call at start of each SequenceLoop
        /// </summary>
        public PhidgetDeviceClassSequence[]? StartActionLoopSequences { get; set; }

        /// <summary>
        /// Number of loops of sequence Actions[] to execute
        /// </summary>
        public int ActionLoops { get; set; } = 1;

        /// <summary>
        /// Play Actions[] in Parallel or Sequentially (false)
        /// </summary>
        public bool ExecuteActionsInParallel { get; set; } = false;

        /// <summary>
        /// Duration of Action[] in ms (sleep time after Actions completed)
        /// </summary>
        public int? ActionsDuration { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call at start of each SequenceLoop
        /// </summary>
        public PhidgetDeviceClassSequence[]? EndActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call after executing Actions
        /// before calling NextSequence
        /// </summary>
        public PhidgetDeviceClassSequence[]? AfterActionLoopSequences { get; set; }

        /// <summary>
        /// Duration of Action[] in ms (sleep time after Actions completed)
        /// </summary>
        public int? SequenceDuration { get; set; }

        /// <summary>
        /// PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public PhidgetDeviceClassSequence? NextSequence { get; set; }
    }
}
