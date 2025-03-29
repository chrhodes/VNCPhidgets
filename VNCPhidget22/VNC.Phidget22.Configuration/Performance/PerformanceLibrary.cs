using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

using Prism.Regions.Behaviors;

using Unity.Interception.Utilities;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VNC.Phidget22.Configuration.Performance
{
    /// <summary>
    /// Maintains Library of Hosts, Performances,
    /// and {AdvancedServo,InterfaceKit,Stepper}Sequences
    /// which are loaded from json config files
    /// All properties marked static.  Almost a singleton
    /// </summary>
    public class PerformanceLibrary
    {
        #region Constructors, Initialization, and Load

        // TODO(crhodes)
        // Turn this into a singleton

        public PerformanceLibrary()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            try
            {
                LoadHostsConfig();

                LoadPerformances();

                LoadDigitalInputSequences();
                LoadDigitalOutputSequences();

                LoadRCServoSequences();

                LoadStepperSequences();

                LoadVoltageInputSequences();
                LoadVoltageOutputSequences();
            }
            catch (Exception ex)
            {
                
            }

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public static List<Host> Hosts {  get; private set; } = new List<Host>();

        public static Dictionary<string, Performance> AvailablePerformances { get; set; } =
            new Dictionary<string, Performance>();

        //public static Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; } =
        //    new Dictionary<string, AdvancedServoSequence>();

        public static Dictionary<string, DigitalInputSequence> AvailableDigitalInputSequences { get; set; } =
             new Dictionary<string, DigitalInputSequence>();

        public static Dictionary<string, DigitalOutputSequence> AvailableDigitalOutputSequences { get; set; } =
            new Dictionary<string, DigitalOutputSequence>();

        //public static Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; } =
        //    new Dictionary<string, InterfaceKitSequence>();

        public static Dictionary<string, RCServoSequence> AvailableRCServoSequences { get; set; } =
            new Dictionary<string, RCServoSequence>();

        public static Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; } =
            new Dictionary<string, StepperSequence>();

        public static Dictionary<string, VoltageInputSequence> AvailableVoltageInputSequences { get; set; } =
             new Dictionary<string, VoltageInputSequence>();

        public static Dictionary<string, VoltageOutputSequence> AvailableVoltageOutputSequences { get; set; } =
            new Dictionary<string, VoltageOutputSequence>();

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands (none)



        #endregion

        #region Public Methods

        public void LoadHostsConfig()
        {
            long startTicks = 0;

            string configFile = "hostconfig.json";

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                HostConfig? hostConfig
                    = JsonSerializer.Deserialize<HostConfig>
                    (jsonString, GetJsonSerializerOptions());

                Hosts = hostConfig.Hosts.ToList();

                //LoadNetworkHosts(Hosts);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //void LoadNetworkHosts(List<Host> hosts)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

        //    foreach (Host host in hosts)
        //    {
        //        try
        //        {
        //            Phidgets.Net.AddServer(host.Name, host.IPAddress, host.Port, "", 0);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
        //            Log.Error($"{ex}", Common.LOG_CATEGORY);
        //        }                
        //    }

        //    if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        // TODO(crhodes)
        // Clean this area up
        // Looks like identical code
        // Method that gets a list of files,
        // Add to AvailableTYPE

        public void LoadPerformances()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailablePerformances.Clear();

            foreach (string configFile in GetListOfPerformanceConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    PerformanceConfig? performanceConfig
                        = JsonSerializer.Deserialize<PerformanceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var performance in performanceConfig.Performances.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailablePerformances.Add(performance.Key, performance.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{performance.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfPerformanceConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadDigitalInputSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableDigitalInputSequences.Clear();

            foreach (string configFile in GetListOfDigitalInputConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    DigitalInputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<DigitalInputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.DigitalInputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailableDigitalInputSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfDigitalInputConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadDigitalOutputSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableDigitalOutputSequences.Clear();

            foreach (string configFile in GetListOfDigitalOutputConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    DigitalOutputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<DigitalOutputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.DigitalOutputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailableDigitalOutputSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfDigitalOutputConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadRCServoSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableRCServoSequences.Clear();

            foreach (string configFile in GetListOfRCServoConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    RCServoSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<RCServoSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.RCServoSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailableRCServoSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfAdvancedServoConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadStepperSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableStepperSequences.Clear();

            foreach (string configFile in GetListOfStepperConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    StepperSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<StepperSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.StepperSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailableStepperSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfStepperConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadVoltageInputSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableVoltageInputSequences.Clear();

            foreach (string configFile in GetListOfVoltageInputConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    VoltageInputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<VoltageInputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.VoltageInputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try
                        {
                            AvailableVoltageInputSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }                        
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfVoltageInputConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadVoltageOutputSequences()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableVoltageOutputSequences.Clear();

            foreach (string configFile in GetListOfVoltageOutputConfigFiles())
            {
                if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    VoltageOutputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<VoltageOutputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.VoltageOutputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        try 
                        {
                            AvailableVoltageOutputSequences.Add(sequence.Key, sequence.Value);
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Key}<", Common.LOG_CATEGORY);
                        }
                    }
                }
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfVoltageOutputConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        public static IEnumerable<string> GetListOfPerformanceConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"Performances\PerformanceConfig_InitializationAndFinalization.json",

                @"Performances\PerformanceConfig_Movement Studies.json",

                @"Performances\PerformanceConfig_Skulls_1.json",
                @"Performances\PerformanceConfig_Skulls_2.json",
                @"Performances\PerformanceConfig_Skulls_3.json",

                @"Performances\PerformanceConfig_DigitalOutputs.json",
                @"Performances\PerformanceConfig_RCServos.json",
                @"Performances\PerformanceConfig_Steppers.json",

                @"Performances\PerformanceConfig_Test.json",
                @"Performances\PerformanceConfig_Test AS Replacement.json",

                //@"Performances\PerformanceConfig_2.json",
                //@"Performances\PerformanceConfig_3.json",
            };

            return files;
        }

        public static IEnumerable<string> GetListOfDigitalInputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"DigitalInputSequences\DigitalInputSequenceConfig_1.json"
            };

            return files;
        }

        public static IEnumerable<string> GetListOfDigitalOutputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"DigitalOutputSequences\DigitalOutputSequenceConfig_1.json",
            };

            return files;
        }

        public static IEnumerable<string> GetListOfRCServoConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                // TODO(crhodes)
                // Review and fix these files.  Should be RCServoSequences
                // The ServoIndex is wrong, should be Channel
                // Probably more

                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Initialization.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Skulls.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A+B+C.json",
                
                @"RCServoSequences\RCServoSequenceConfig_99415.json",

                @"RCServoSequences\RCServoSequenceConfig_Initialize.json",

                @"RCServoSequences\RCServoSequenceConfig_Move.json",
                @"RCServoSequences\RCServoSequenceConfig_MovementCharacteristics.json",
                @"RCServoSequences\RCServoSequenceConfig_OpenClose.json",
            };

            return files;
        }

        public static IEnumerable<string> GetListOfStepperConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code

            List<string> files = new List<string>
            {
                @"StepperSequences\StepperSequenceConfig_1.json",
            };
            return files;
        }

        public static IEnumerable<string> GetListOfVoltageInputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"VoltageInputSequences\VoltageInputSequenceConfig_1.json"
            };

            return files;
        }

        public static IEnumerable<string> GetListOfVoltageRatioInputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"VoltageRatioInputSequences\VoltageRatioInputSequenceConfig_1.json"
            };

            return files;
        }

        public static IEnumerable<string> GetListOfVoltageOutputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"VoltageOutputSequences\VoltageOutputSequenceConfig_1.json"
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
