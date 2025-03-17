using System;

namespace VNC.Phidget22.Configuration.Performance
{
    public class ChannelSequence
    {
        public ChannelSequence(string channelClass)
        {
            ChannelClass = channelClass;
        }

        /// <summary>
        /// Name of sequence.  Must be unique in ChannelClass
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// ChannelClass.  Set in constructor.
        /// </summary>
        public string ChannelClass { get; } = "Generic";

        /// <summary>
        /// HubPort on Phidget Device.
        /// </summary>
        public Int32? HubPort { get; set; } = null;

        /// <summary>
        /// Channel on Phidget Device.
        /// </summary>
        public Int32? Channel { get; set; } = null;

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
        public DeviceChannelSequence[]? BeforeActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call at start of each SequenceLoop
        /// </summary>
        public DeviceChannelSequence[]? StartActionLoopSequences { get; set; }

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
        public DeviceChannelSequence[]? EndActionLoopSequences { get; set; }

        /// <summary>
        /// PerformanceSequence[] to call after executing Actions
        /// before calling NextSequence
        /// </summary>
        public DeviceChannelSequence[]? AfterActionLoopSequences { get; set; }

        /// <summary>
        /// Duration of Action[] in ms (sleep time after Actions completed)
        /// </summary>
        public Int32? SequenceDuration { get; set; }

        /// <summary>
        /// PerformanceSequence to invoke at end of sequence loops (optional)
        /// none or null to stop
        /// </summary>
        public DeviceChannelSequence? NextSequence { get; set; }
    }
}
