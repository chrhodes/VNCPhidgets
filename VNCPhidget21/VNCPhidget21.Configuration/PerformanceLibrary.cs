using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using VNC;

namespace VNCPhidget21.Configuration
{
    public class PerformanceLibrary
    {
        #region Constructors, Initialization, and Load

        // TODO(crhodes)
        // Turn this into a singleton

        public PerformanceLibrary()
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            LoadHostConfig();

            LoadPerformances();
            LoadAdvancedServoSequences();
            LoadInterfaceKitSequences();
            LoadStepperSequences();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        //public bool LogPerformance { get; set; }

        //public PerformanceSequencePlayer PerformanceSequencePlayer { get; set; }

        public static List<Host> Hosts {  get; private set; } = new List<Host>();

        public static Dictionary<string, Performance> AvailablePerformances { get; set; } =
            new Dictionary<string, Performance>();

        //public IEnumerable<Performance> Performances { get; set; }

        public static Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; } =
            new Dictionary<string, AdvancedServoSequence>();

        public static Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; } =
            new Dictionary<string, InterfaceKitSequence>();

        public static Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; } =
            new Dictionary<string, StepperSequence>();

        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods

        public void LoadHostConfig()
        {
            string HostConfigFileName = "hostconfig.json";

            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            HostConfig? hostConfig
                = JsonSerializer.Deserialize<HostConfig>
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadPerformances()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailablePerformances.Clear();

            foreach (string configFile in GetListOfPerformanceConfigFiles())
            {
                string jsonString = File.ReadAllText(configFile);

                PerformanceConfig? performanceConfig
                    = JsonSerializer.Deserialize<PerformanceConfig>
                    (jsonString, GetJsonSerializerOptions());

                foreach (var performance in performanceConfig.Performances.ToDictionary(k => k.Name, v => v))
                {
                    AvailablePerformances.Add(performance.Key, performance.Value);
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }


        public void LoadAdvancedServoSequences()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableAdvancedServoSequences.Clear();

            foreach (string configFile in GetListOfAdvancedServoConfigFiles())
            {
                string jsonString = File.ReadAllText(configFile);

                AdvancedServoSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<AdvancedServoSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                foreach (var sequence in sequenceConfig.AdvancedServoSequences.ToDictionary(k => k.Name, v => v))
                {
                    AvailableAdvancedServoSequences.Add(sequence.Key, sequence.Value);
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }
        public void LoadInterfaceKitSequences()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableInterfaceKitSequences.Clear();

            foreach (string configFile in GetListOfInterfaceKitConfigFiles())
            {
                string jsonString = File.ReadAllText(configFile);

                InterfaceKitSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<InterfaceKitSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                foreach (var sequence in sequenceConfig.InterfaceKitSequences.ToDictionary(k => k.Name, v => v))
                {
                    AvailableInterfaceKitSequences.Add(sequence.Key, sequence.Value);
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadStepperSequences()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableStepperSequences.Clear();

            foreach (string configFile in GetListOfStepperConfigFiles())
            {
                string jsonString = File.ReadAllText(configFile);

                StepperSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<StepperSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                foreach (var sequence in sequenceConfig.StepperSequences.ToDictionary(k => k.Name, v => v))
                {
                    AvailableStepperSequences.Add(sequence.Key, sequence.Value);
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        private IEnumerable<string> GetListOfPerformanceConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"Performances\PerformanceConfig_1.json",
                @"Performances\PerformanceConfig_2.json",
                @"Performances\PerformanceConfig_3.json",

                @"Performances\PerformanceConfig_Skulls.json",
            };

            return files;
        }

        private IEnumerable<string> GetListOfAdvancedServoConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_99415.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_99220_Skulls.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Test B.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Test C.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A+B+C.json",
            };

            return files;
        }

        private IEnumerable<string> GetListOfInterfaceKitConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_46049.json",
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_48284.json",
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_48301.json",
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_124744.json",
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_251831.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfStepperConfigFiles()
        {
            // TODO(crhodes)
            // Read a directory and return files, perhaps with RegEx name match

            List<string> files = new List<string>
            {
                @"localhost\localhost_StepperSequenceConfig_1.json",
            };

            return files;
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return jsonOptions;
        }

        #endregion
    }
}
