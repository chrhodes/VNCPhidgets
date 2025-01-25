using System;

namespace VNC.Phidget22.Configuration
{
    public class PerformanceSequence
    {
        /// <summary>
        /// Name of Sequence
        /// </summary>
        public string Name { get; set; } = "SEQUENCE NAME";

        /// <summary>
        /// SerialNumber of Phidget that will run PerformanceSequence
        /// </summary>
        public Int32 SerialNumber { get; set; }

        /// <summary>
        /// Type of Sequence {AS, IK, RCS, ST}
        /// Maybe make this enum
        /// </summary>
        public string SequenceType { get; set; }

        /// <summary>
        /// Number of loops of Sequence
        /// </summary>
        public Int32 SequenceLoops { get; set; } = 1;

        /// <summary>
        /// Duration of Sequence in ms (sleep time after Loops completed)
        /// </summary>
        public Int32? Duration { get; set; }

        /// <summary>
        /// Close Phidget at end of sequence loops
        /// </summary>
        public Boolean ClosePhidget { get; set; } = false;
    }
}
