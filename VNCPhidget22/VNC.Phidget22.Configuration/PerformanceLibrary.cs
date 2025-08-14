using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

using Prism.Events;
using Prism.Regions.Behaviors;

using Unity.Interception.Utilities;

using VNC.Core.Collections;
using VNC.Phidget22.Configuration.Performance;

using VNCPhidget22Explorer.Core.Events;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VNC.Phidget22.Configuration
{
    /// <summary>
    /// Maintains Library of Hosts, Performances,
    /// and {AdvancedServo,InterfaceKit,Stepper}Sequences
    /// which are loaded from json config files
    /// All properties marked static.  Almost a singleton
    /// </summary>
    public class PerformanceLibrary : INotifyPropertyChanged, INotifyCollectionChanged
    {
        IEventAggregator _eventAggregator;
        #region Constructors, Initialization, and Load

        // TODO(crhodes)
        // Turn this into a singleton

        public PerformanceLibrary(IEventAggregator eventAggregator)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;
            //LoadConfigFiles();

            //AvailablePerformances.CollectionChanged += AvailablePerformances_CollectionChanged;

            if (Core.Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void AvailablePerformances_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Log.Trace("Collection Changed", Common.LOG_CATEGORY);
        }

        public void LoadConfigFiles()
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Enter", Common.LOG_CATEGORY);

            try
            {
                LoadHostsConfig();

                LoadPerformances();

                LoadDigitalInputSequences();
                LoadDigitalOutputSequences();

                LoadRCServoSequences();

                LoadStepperSequences();

                LoadVoltageInputSequences();
                LoadVoltageRatioInputSequences();
                LoadVoltageOutputSequences();

                Common.PerformanceLibrary = this;
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public static List<Host> Hosts { get; private set; } = new List<Host>();

        //private ObservableDictionary<string, Performance.Performance> _availablePerformances =
        //    new ObservableDictionary<string, Performance.Performance>();
        //public ObservableDictionary<string, Performance.Performance> AvailablePerformances
        //{
        //    get 
        //    { 
        //        return _availablePerformances; 
        //    }
        //    set
        //    {
        //        _availablePerformances = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public event NotifyCollectionChangedEventHandler? CollectionChanged
        //{
        //    add
        //    {
        //        ((INotifyCollectionChanged)AvailablePerformances).CollectionChanged += value;
        //    }

        //    remove
        //    {
        //        ((INotifyCollectionChanged)AvailablePerformances).CollectionChanged -= value;
        //    }
        //}

        //private Dictionary<string, Performance> _availablePerformances =
        //    new Dictionary<string, Performance>();
        //public Dictionary<string, Performance> AvailablePerformances
        //{
        //    get { return _availablePerformances; }
        //    set
        //    {
        //        _availablePerformances = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, Performance.Performance> AvailablePerformances { get; set; } =
            new Dictionary<string, Performance.Performance>();

        //private ObservableDictionary<string, DigitalInputSequence> _availableDigitalInputSequences =
        //    new ObservableDictionary<string, DigitalInputSequence>();
        //public ObservableDictionary<string, DigitalInputSequence> AvailableDigitalInputSequences
        //{
        //    get
        //    {
        //        return _availableDigitalInputSequences;
        //    }
        //    set
        //    {
        //        _availableDigitalInputSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, DigitalInputSequence> AvailableDigitalInputSequences { get; set; } =
             new Dictionary<string, DigitalInputSequence>();

        //private ObservableDictionary<string, DigitalOutputSequence> _availableDigitalOutputSequences =
        //    new ObservableDictionary<string, DigitalOutputSequence>();
        //public ObservableDictionary<string, DigitalOutputSequence> AvailableDigitalOutputSequences
        //{
        //    get
        //    {
        //        return _availableDigitalOutputSequences;
        //    }
        //    set
        //    {
        //        _availableDigitalOutputSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, DigitalOutputSequence> AvailableDigitalOutputSequences { get; set; } =
            new Dictionary<string, DigitalOutputSequence>();

        //private ObservableDictionary<string, RCServoSequence> _availableRCServoSequences =
        //    new ObservableDictionary<string, RCServoSequence>();
        //public ObservableDictionary<string, RCServoSequence> AvailableRCServoSequences
        //{
        //    get
        //    {
        //        return _availableRCServoSequences;
        //    }
        //    set
        //    {
        //        _availableRCServoSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, RCServoSequence> AvailableRCServoSequences { get; set; } =
            new Dictionary<string, RCServoSequence>();

        //private ObservableDictionary<string, StepperSequence> _availableStepperSequences =
        //    new ObservableDictionary<string, StepperSequence>();
        //public ObservableDictionary<string, StepperSequence> AvailableStepperSequences
        //{
        //    get
        //    {
        //        return _availableStepperSequences;
        //    }
        //    set
        //    {
        //        _availableStepperSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, StepperSequence> AvailableStepperSequences { get; set; } =
            new Dictionary<string, StepperSequence>();

        //private ObservableDictionary<string, VoltageInputSequence> _availableVoltageInputSequences =
        //    new ObservableDictionary<string, VoltageInputSequence>();
        //public ObservableDictionary<string, VoltageInputSequence> AvailableVoltageInputSequences
        //{
        //    get
        //    {
        //        return _availableVoltageInputSequences;
        //    }
        //    set
        //    {
        //        _availableVoltageInputSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static Dictionary<string, VoltageInputSequence> AvailableVoltageInputSequences { get; set; } =
             new Dictionary<string, VoltageInputSequence>();

        //private ObservableDictionary<string, VoltageOutputSequence> _availableVoltageOutputSequences =
        //    new ObservableDictionary<string, VoltageOutputSequence>();
        //public ObservableDictionary<string, VoltageOutputSequence> AvailableVoltageOutputSequences
        //{
        //    get
        //    {
        //        return _availableVoltageOutputSequences;
        //    }
        //    set
        //    {
        //        _availableVoltageOutputSequences = value;
        //        OnPropertyChanged();
        //    }
        //}

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

            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                HostConfig? hostConfig
                    = JsonSerializer.Deserialize<HostConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (hostConfig is not null) Hosts = hostConfig.Hosts.ToList();

                //LoadNetworkHosts(Hosts);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // TODO(crhodes)
        // Clean this area up
        // Looks like identical code
        // Method that gets a list of files,
        // Add to AvailableTYPE

        public void LoadPerformances(bool reload = false)
        {
            Int64 startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks 
                    = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            if (reload)
            {
                //AvailablePerformances.Clear();
                //AvailablePerformances = new ObservableDictionary<string, Performance>();
            }            

            foreach (string configFile in GetListOfPerformanceConfigFiles())
            {
                LoadPerformancesFromConfigFile(configFile);
            }

            //AvailablePerformances.CollectionChanged += AvailablePerformances_CollectionChanged;

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // NOTE(crhodes)
        // This allows easier reloading of a single file
        // TODO(crhodes)
        // Need to create for other ConfigFiles

        public void LoadPerformancesFromConfigFile(string configFile, bool reload = false)
        {
            Int64 startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitializeLow) startTicks = 
                    Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                PerformanceConfig? performanceConfig
                    = JsonSerializer.Deserialize<PerformanceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (performanceConfig != null && performanceConfig.Performances is not null)
                {
                    foreach (var performance in performanceConfig.Performances)
                    {
                        try
                        {
                            if (performance.Name is not null)
                            {
                                if (reload)
                                { 
                                    AvailablePerformances.Remove(performance.Name);
                                }

                                AvailablePerformances.Add(performance.Name, performance);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{performance.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        { 
                            Name = "Performances" 
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfPerformanceConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadDigitalInputSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableDigitalInputSequences.Clear();

            foreach (string configFile in GetListOfDigitalInputConfigFiles())
            {
                LoadDigitalInputSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadDigitalInputSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                DigitalInputSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<DigitalInputSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.DigitalInputSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.DigitalInputSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableDigitalInputSequences.Remove(sequence.Name);

                                AvailableDigitalInputSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "DigitalInputSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfDigitalInputConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadDigitalOutputSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableDigitalOutputSequences.Clear();

            foreach (string configFile in GetListOfDigitalOutputConfigFiles())
            {
                LoadDigitalOutputSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadDigitalOutputSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                DigitalOutputSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<DigitalOutputSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig is not null && sequenceConfig.DigitalOutputSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.DigitalOutputSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableDigitalOutputSequences.Remove(sequence.Name);

                                AvailableDigitalOutputSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "DigitalOutputSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfDigitalOutputConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadRCServoSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableRCServoSequences.Clear();

            foreach (string configFile in GetListOfRCServoConfigFiles())
            {
                LoadRCServoSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadRCServoSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                RCServoSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<RCServoSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.RCServoSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.RCServoSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableRCServoSequences.Remove(sequence.Name);

                                AvailableRCServoSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "RCServoSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfAdvancedServoConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadStepperSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableStepperSequences.Clear();

            foreach (string configFile in GetListOfStepperConfigFiles())
            {
                LoadStepperSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadStepperSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                StepperSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<StepperSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.StepperSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.StepperSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableStepperSequences.Remove(sequence.Name);

                                AvailableStepperSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "StepperSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfStepperConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadVoltageInputSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableVoltageInputSequences.Clear();

            foreach (string configFile in GetListOfVoltageInputConfigFiles())
            {
                LoadVoltageInputSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadVoltageInputSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                VoltageInputSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<VoltageInputSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.VoltageInputSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.VoltageInputSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableVoltageInputSequences.Remove(sequence.Name);

                                AvailableVoltageInputSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "VoltageInputSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfVoltageInputConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadVoltageRatioInputSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //AvailableVoltageInputSequences.Clear();

            foreach (string configFile in GetListOfVoltageRatioInputConfigFiles())
            {
                LoadVoltageRatioInputSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadVoltageRatioInputSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                VoltageInputSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<VoltageInputSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.VoltageInputSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.VoltageInputSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableVoltageInputSequences.Remove(sequence.Name);

                                AvailableVoltageInputSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfVoltageRatioInputConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
        }

        public void LoadVoltageOutputSequences(bool reload = false)
        {
            long startTicks = 0;
            if (Core.Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            AvailableVoltageOutputSequences.Clear();

            foreach (string configFile in GetListOfVoltageOutputConfigFiles())
            {
                LoadVoltageOutputSequencesFromConfigFile(reload, configFile);
            }

            if (Core.Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadVoltageOutputSequencesFromConfigFile(bool reload, string configFile)
        {
            if (Core.Common.VNCLogging.ApplicationInitializeLow) Log.APPLICATION_INITIALIZE_LOW($"Loading config file >{configFile}<", Common.LOG_CATEGORY);

            try
            {
                string jsonString = File.ReadAllText(configFile);

                VoltageOutputSequenceConfig? sequenceConfig
                    = JsonSerializer.Deserialize<VoltageOutputSequenceConfig>
                    (jsonString, GetJsonSerializerOptions());

                if (sequenceConfig != null && sequenceConfig.VoltageOutputSequences is not null)
                {
                    foreach (var sequence in sequenceConfig.VoltageOutputSequences)
                    {
                        try
                        {
                            if (sequence.Name is not null)
                            {
                                if (reload) AvailableVoltageOutputSequences.Remove(sequence.Name);

                                AvailableVoltageOutputSequences.Add(sequence.Name, sequence);
                            }
                        }
                        catch (ArgumentException ax)
                        {
                            Log.Error($"Duplicate Key >{sequence.Name}<", Common.LOG_CATEGORY);
                            Log.Error($"{ax}", Common.LOG_CATEGORY);
                        }
                    }

                    _eventAggregator.GetEvent<SelectedCollectionChangedEvent>().Publish(
                        new SelectedCollectionChangedEventArgs()
                        {
                            Name = "VoltageOutputSequences"
                        });
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Log.Error($"Cannot find config file >{configFile}<  Check GetListOfVoltageOutputConfigFiles()", Common.LOG_CATEGORY);
                Log.Error($"{fnfex}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error($"Error processing config file >{configFile}<", Common.LOG_CATEGORY);
                Log.Error($"{ex}", Common.LOG_CATEGORY);
            }
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
            // Maybe just put in ConfigFiles.json

            List<string> files = new List<string>
            {
                // Top Level Routines
                // These go first to get on top of dropdown

                @"Performances\PerformanceConfig_Test.json",

                @"Performances\PerformanceConfig_Test AS Replacement.json",

                @"Performances\Skulls\PerformanceConfig_Skulls_Routines.json",
                @"Performances\Skulls\PerformanceConfig_Skulls_Consulting.json",
                @"Performances\Skulls\PerformanceConfig_Skulls_MoveAll.json",
                @"Performances\Skulls\PerformanceConfig_Skulls_MovementCharacteristics_All.json",             

                @"Performances\PerformanceConfig_Movement Studies.json",

                // Performances\Skulls\         

                @"Performances\Skulls\PerformanceConfig_Skulls_Initialize.json",
  
                @"Performances\Skulls\PerformanceConfig_Skulls_MoveAllAxes.json",
                @"Performances\Skulls\PerformanceConfig_Skulls_MovementCharacteristics.json",
                @"Performances\Skulls\PerformanceConfig_Skulls_MoveAngles.json",

                @"Performances\Skulls\Device\PerformanceConfig_99220_Skull_012.json",
                @"Performances\Skulls\Device\PerformanceConfig_99220_Skull_456.json",
                @"Performances\Skulls\Device\PerformanceConfig_169501_Skull_012.json",

                @"Performances\Skulls\Skull\PerformanceConfig_Skull_012_LeftRight.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_012_TiltLeftRight.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_012_UpDown.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_012_YesNoMaybe.json",

                @"Performances\Skulls\Skull\PerformanceConfig_Skull_456_LeftRight.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_456_TiltLeftRight.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_456_UpDown.json",
                @"Performances\Skulls\Skull\PerformanceConfig_Skull_456_YesNoMaybe.json",

                // Performances\DigitalOutput\

                @"Performances\DigitalOutput\PerformanceConfig_DigitalOutputs.json",

                // Performances\RCServo\

                @"Performances\RCServo\PerformanceConfig_RCServos.json",

                @"Performances\RCServo\PerformanceConfig_RCServo_OpenClose.json",
                @"Performances\RCServo\PerformanceConfig_RCServo_MovementCharacteristics.json",
                @"Performances\RCServo\PerformanceConfig_RCServo_PP1.json",
                @"Performances\RCServo\PerformanceConfig_RCServo_PP2.json",
                @"Performances\RCServo\PerformanceConfig_RCServo_PP3.json",
                @"Performances\RCServo\PerformanceConfig_RCServo_PP4.json",

                // Performances\Stepper\

                @"Performances\Stepper\PerformanceConfig_Steppers.json",

                @"Performances\Stepper\PerformanceConfig_Stepper_OpenClose.json",
                @"Performances\Stepper\PerformanceConfig_Stepper_MovementCharacteristics.json",
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
                @"DigitalInputSequences\DigitalInputSequenceConfig_1.json",
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
                @"DigitalOutputSequences\DigitalOutputSequenceConfig_OpenClose.json",
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

                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Skulls.json",
                //@"AdvancedServoSequences\AdvancedServoSequenceConfig_Test A+B+C.json",
                
                @"RCServoSequences\RCServoSequenceConfig_99415.json",

                //@"RCServoSequences\RCServoSequenceConfig_Initialize.json",

                @"RCServoSequences\RCServoSequenceConfig_Move.json",
                @"RCServoSequences\RCServoSequenceConfig_MovementCharacteristics.json",
                @"RCServoSequences\RCServoSequenceConfig_OpenClose.json",

                // RCServoSequences\Skulls\

                @"RCServoSequences\Skulls\RCServoSequenceConfig_Skull 1 Initialization.json",
                @"RCServoSequences\Skulls\RCServoSequenceConfig_Skull 2 Initialization.json",
                @"RCServoSequences\Skulls\RCServoSequenceConfig_Skull 3 Initialization.json",
                @"RCServoSequences\Skulls\RCServoSequenceConfig_Skull Move.json",
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
                @"StepperSequences\StepperSequenceConfig_OpenClose.json",
                @"StepperSequences\StepperSequenceConfig_MovementCharacteristics.json",
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
                // TODO(crhodes)
                // Implement
                //@"VoltageRatioInputSequences\VoltageRatioInputSequenceConfig_1.json"
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

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        // This is the traditional approach - requires string name to be passed in

        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {

#if LOGGING
            long startTicks = 0;
            if (Common.VNCCoreLogging.INPC) startTicks = Log.VIEW_LOW($"Enter ({propertyName})", Common.LOG_CATEGORY);
#endif
            // This is the new CompilerServices attribute!

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#if LOGGING
            if (Common.VNCCoreLogging.INPC) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
#endif
        }

        #endregion

    }
}
