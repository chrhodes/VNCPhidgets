using System;

namespace VNC.Phidget22.Configuration.Performance
{
    public class Performance
    {
        public Performance()
        {
        }
        public Performance(Performance performance)
        {
            Name = performance.Name;
            Description = performance.Description;
            UsageNotes = performance.UsageNotes;
            BeforePerformanceLoopPerformances = performance.BeforePerformanceLoopPerformances;
            SerialNumber = performance.SerialNumber;
            DeviceChannelSequences = performance.DeviceChannelSequences;
            ExecuteDeviceChannelSequencesInParallel = performance.ExecuteDeviceChannelSequencesInParallel;
            Performances = performance.Performances;
            ExecutePerformancesInParallel = performance.ExecutePerformancesInParallel;
            Duration = performance.Duration;
            PerformanceLoops = performance.PerformanceLoops;
            AfterPerformanceLoopPerformances = performance.AfterPerformanceLoopPerformances;
            NextPerformance = performance.NextPerformance;
        }

        /// <summary>
        /// Name of Performance.  Can be null if using embedded performance
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of Performance
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Notes on using Performance (optional)
        /// </summary>
        public string? UsageNotes { get; set; }

        /// <summary>
        /// Performance[] to call before executing DeviceChannelSequences[] and/or Performances[]
        /// before calling NextPerformance
        /// </summary>
        public Performance[]? BeforePerformanceLoopPerformances { get; set; }

        /// <summary>
        /// SerialNumber of PhidgetDevice that will run DeviceChannelSequences
        /// If null, Name is used to lookup DeviceClassSequence 
        /// which must specify a SerialNumber
        /// </summary>
        public Int32? SerialNumber { get; set; } = null;

        /// <summary>
        /// DeviceChannelSequences to execute as part of this performance
        /// </summary>
        public DeviceChannelSequence[]? DeviceChannelSequences { get; set; }

        /// <summary>
        /// Play DeviceChannelSequences in Parallel or Sequential (false)
        /// </summary>
        public Boolean ExecuteDeviceChannelSequencesInParallel { get; set; } = false;

        /// <summary>
        /// Performances to execute as part of this performance
        /// </summary>
        public Performance[]? Performances { get; set; }

        /// <summary>
        /// Play Performances in Parallel or Sequential (false)
        /// </summary>
        public Boolean ExecutePerformancesInParallel { get; set; } = false;

        /// <summary>
        /// Duration in ms of sleep time after DeviceChannelSequences[] or Performances[] completed
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Number of loops of DeviceChannelSequences[] and/or Performances[]
        /// </summary>
        public Int32 PerformanceLoops { get; set; } = 1;

        /// <summary>
        /// Performance[] to call after executing DeviceChannelSequences[]
        /// before calling NextSequence
        /// </summary>
        public Performance[]? AfterPerformanceLoopPerformances { get; set; }

        /// <summary>
        /// Performance to invoke at end of Loops of DeviceChannelSequences or Performances
        /// none or null to stop
        /// </summary>
        public Performance? NextPerformance { get; set; } = null;
    }
}
