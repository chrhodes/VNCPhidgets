namespace VNC.Phidget22.Configuration
{
    public class PerformanceConfig
    {
        /// <summary>
        /// Name of file
        /// </summary>
        public string Name { get; set; } = "PerformanceConfig NAME";

        /// <summary>
        /// Description of this file
        /// </summary>
        public string Description { get; set; } = "PerformanceConfig DESCRIPTION";

        public Performance.Performance[]? Performances { get; set; }

    }
}
