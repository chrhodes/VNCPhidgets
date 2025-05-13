using System;

namespace VNC.Phidget22.Configuration.Performance
{
    public class ChannelSequence
    {
        private static readonly object _lock = new object();

        public ChannelSequence(string channelClass)
        {
            ChannelClass = channelClass;
        }

        public ChannelSequence(string channelClass, ChannelSequence channelSequence)
        {
            lock (_lock)
            {
                ChannelClass = channelClass;

                Name = channelSequence.Name;
                Description = channelSequence.Description;
                UsageNotes = channelSequence.UsageNotes;
                HubPort = channelSequence.HubPort;
                Channel = channelSequence.Channel;
                BeforeActionLoopSequences = channelSequence.BeforeActionLoopSequences;
                StartActionLoopSequences = channelSequence.StartActionLoopSequences;
                ActionLoops = channelSequence.ActionLoops;
                ExecuteActionsInParallel = channelSequence.ExecuteActionsInParallel;
                ActionsDuration = channelSequence.ActionsDuration;
                EndActionLoopSequences = channelSequence.EndActionLoopSequences;
                SequenceLoops = channelSequence.SequenceLoops;
                AfterActionLoopSequences = channelSequence.AfterActionLoopSequences;
                SequenceDuration = channelSequence.SequenceDuration;
                NextSequence = channelSequence.NextSequence;
            }
        }

        /// <summary>
        /// Name of sequence. Must be unique in ChannelClass
        /// Can be null if using embedded ChannelSequence
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of ChannelSequence (optional)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Notes on using ChannelSequence (optional)
        /// </summary>
        public string? UsageNotes { get; set; }

        /// <summary>
        /// ChannelClass.  Set in constructor.
        /// </summary>
        public string ChannelClass { get; } = "Generic";

        /// <summary>
        /// HubPort on Phidget Device.
        /// If null, must be overriden in DeviceChannelSequence
        /// </summary>
        public Int32? HubPort { get; set; } = null;

        /// <summary>
        /// Channel on Phidget Device.
        /// If null, must be overriden in DeviceChannelSequence 
        /// </summary>
        public Int32? Channel { get; set; } = null;

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
        /// Number of loops of sequence to execute
        /// </summary>
        public Int32 SequenceLoops { get; set; } = 1;

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
