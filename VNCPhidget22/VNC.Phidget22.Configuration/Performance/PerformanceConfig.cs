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

        public Performance.Performance[] Performances { get; set; } // = new[] // PerformanceSequence[0];
        //{
            //new Performance.Performance
            //{
            //    Name = "psbc21_AS Performance 1",
            //    Description = "psbc21_AS Performance 1 Description",

            //    PhidgetDeviceClassSequences = new[] // PerformanceSequence[0];
            //    {
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceServo0", DeviceClass = "AS" },
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceServo0P Configure and Engage", DeviceClass = "AS" }
            //    }
            //},
            //new Performance.Performance
            //{
            //    Name = "IK Performance 1",
            //    Description = "psbc{21,22,23}_SequenceIK 1 in Parallel",
            //    PlaySequencesInParallel = true,

            //    PhidgetDeviceClassSequences = new[]
            //    {
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceIK 1", DeviceClass = "IK" },
            //        new PhidgetDeviceClassSequence { Name = "psbc22_SequenceIK 1", DeviceClass = "IK" },
            //        new PhidgetDeviceClassSequence { Name = "psbc23_SequenceIK 1", DeviceClass = "IK" }
            //    }
            //},
            //new Performance.Performance
            //{
            //    Name = "AS and IK",
            //    Description = "psbc21_{SequenceServo0,SequenceIK 1} in Sequence",

            //    PhidgetDeviceClassSequences = new[]
            //    {
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceServo0", DeviceClass = "AS" },
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceIK 1", DeviceClass = "IK" }
            //    }
            //},
            //new Performance.Performance
            //{
            //    Name = "AS and IK Parallel",
            //    Description = "psbc21_{SequenceServo0,SequenceIK 1} in Parallel",
            //    PlaySequencesInParallel = true,

            //    PhidgetDeviceClassSequences = new[]
            //    {
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceServo0", DeviceClass = "AS" },
            //        new PhidgetDeviceClassSequence { Name = "psbc21_SequenceIK 1", DeviceClass = "IK" }
            //    }
            //}
        //};
    }
}
