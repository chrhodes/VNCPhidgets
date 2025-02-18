using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using VNC;

namespace VNC.Phidget22.Configuration
{
    /// <summary>
    /// Maintains Library of Hosts, Performances,
    /// and {AdvancedServo,InterfaceKit,Stepper}Sequences
    /// which are loaded from json config files
    /// </summary>
    public class PerformanceLibrary
    {
        #region Constructors, Initialization, and Load

        // TODO(crhodes)
        // Turn this into a singleton

        public PerformanceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            LoadHostsConfig();

            LoadPerformances();

            LoadAdvancedServoSequences();
            //LoadDigitalInputSequences();
            //LoadDigitalOutputSequences();
            LoadInterfaceKitSequences();
            //LoadRCServoSequences();

            LoadStepperSequences();

            //LoadVoltageInputSequences();
            //LoadVoltageOutputSequences();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
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

        public static Dictionary<string, AdvancedServoSequence> AvailableAdvancedServoSequences { get; set; } =
            new Dictionary<string, AdvancedServoSequence>();

        public static Dictionary<string, DigitalInputSequence> AvailableDigitalInputSequences { get; set; } =
             new Dictionary<string, DigitalInputSequence>();

        public static Dictionary<string, DigitalOutputSequence> AvailableDigitalOutputSequences { get; set; } =
            new Dictionary<string, DigitalOutputSequence>();

        public static Dictionary<string, InterfaceKitSequence> AvailableInterfaceKitSequences { get; set; } =
            new Dictionary<string, InterfaceKitSequence>();

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
            Int64 startTicks = 0;

            string configFile = "hostconfig.json";

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
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

        public void LoadPerformances()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailablePerformances.Clear();

            foreach (string configFile in GetListOfPerformanceConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadAdvancedServoSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableAdvancedServoSequences.Clear();

            foreach (string configFile in GetListOfAdvancedServoConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadDigitalInputSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableDigitalInputSequences.Clear();

            foreach (string configFile in GetListOfDigitalInputConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    DigitalInputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<DigitalInputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.DigitalInputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        AvailableDigitalInputSequences.Add(sequence.Key, sequence.Value);
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadDigitalOutputSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableDigitalOutputSequences.Clear();

            foreach (string configFile in GetListOfDigitalOutputConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    DigitalOutputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<DigitalOutputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.DigitalOutputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        AvailableDigitalOutputSequences.Add(sequence.Key, sequence.Value);
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }
        public void LoadInterfaceKitSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableInterfaceKitSequences.Clear();

            foreach (string configFile in GetListOfInterfaceKitConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
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
                catch (FileNotFoundException fnfex)
                {
                    Log.Error($"Cannot find config file >{configFile}<  Check GetListOfInterfaceKitConfigFiles()", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                    Log.Error($"{ex}", Common.LOG_CATEGORY);
                }
            }

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadRCServoSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableRCServoSequences.Clear();

            foreach (string configFile in GetListOfRCServoConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    RCServoSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<RCServoSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.RCServoSequences.ToDictionary(k => k.Name, v => v))
                    {
                        
                        AvailableRCServoSequences.Add(sequence.Key, sequence.Value);
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadStepperSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableStepperSequences.Clear();

            foreach (string configFile in GetListOfStepperConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadVoltageInputSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableVoltageInputSequences.Clear();

            foreach (string configFile in GetListOfVoltageInputConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    VoltageInputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<VoltageInputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.VoltageInputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        AvailableVoltageInputSequences.Add(sequence.Key, sequence.Value);
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public void LoadVoltageOutputSequences()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableVoltageOutputSequences.Clear();

            foreach (string configFile in GetListOfVoltageOutputConfigFiles())
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

                try
                {
                    string jsonString = File.ReadAllText(configFile);

                    VoltageOutputSequenceConfig? sequenceConfig
                        = JsonSerializer.Deserialize<VoltageOutputSequenceConfig>
                        (jsonString, GetJsonSerializerOptions());

                    foreach (var sequence in sequenceConfig.VoltageOutputSequences.ToDictionary(k => k.Name, v => v))
                    {
                        AvailableVoltageOutputSequences.Add(sequence.Key, sequence.Value);
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

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private IEnumerable<string> GetListOfPerformanceConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"Performances\PerformanceConfig_InitializationAndFinalization.json",
                @"Performances\PerformanceConfig_Test.json",

                @"Performances\PerformanceConfig_Skulls_1.json",
                @"Performances\PerformanceConfig_Skulls_2.json",
                @"Performances\PerformanceConfig_Skulls_3.json",

                //@"Performances\PerformanceConfig_2.json",
                //@"Performances\PerformanceConfig_3.json",
            };

            return files;
        }

        private IEnumerable<string> GetListOfAdvancedServoConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Initialization.json",
                @"AdvancedServoSequences\AdvancedServoSequenceConfig_Skulls.json",

                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_99415.json",
                //// These may go away after stuff moves to Initialization
                ////@"AdvancedServoSequences\AdvancedServoSequenceConfig_99220_Skulls.json",
                ////@"AdvancedServoSequences\AdvancedServoSequenceConfig_169501_Skulls.json",

                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test B.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test C.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A+B+C.json",
            };

            return files;
        }

        private IEnumerable<string> GetListOfDigitalInputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"DigitalInputSequences\DigitalInputSequenceConfig_124744.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfDigitalOutputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"DigitalOutputSequences\DigitalOutputSequenceConfig_124744.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfInterfaceKitConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"InterfaceKitSequences\InterfaceKitSequenceConfig_1.json",
                //@"InterfaceKitSequences\InterfaceKitSequenceConfig_48284.json",
                //@"InterfaceKitSequences\InterfaceKitSequenceConfig_48301.json",
                //@"InterfaceKitSequences\InterfaceKitSequenceConfig_124744.json",
                //@"InterfaceKitSequences\InterfaceKitSequenceConfig_251831.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfRCServoConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"RCServoSequences\RCServoSequenceConfig_99415.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfStepperConfigFiles()
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

        private IEnumerable<string> GetListOfVoltageInputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"VoltageInputSequences\VoltageInputSequenceConfig_124744.json"
            };

            return files;
        }

        private IEnumerable<string> GetListOfVoltageOutputConfigFiles()
        {
            // HACK(crhodes)
            // Read a directory and return files, perhaps with RegEx name match
            // for now just hard code
            // Would be nice to control order

            List<string> files = new List<string>
            {
                @"VoltageOutputSequences\VoltageOutputSequenceConfig_XXXX.json"
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
