using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;
using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;
using VNCPhidgetConfig = VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class InterfaceKit1018ViewModel 
        : EventViewModelBase, IInterfaceKitViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public InterfaceKit1018ViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            OpenInterfaceKitCommand = new DelegateCommand(OpenInterfaceKit, OpenInterfaceKitCanExecute);
            CloseInterfaceKitCommand = new DelegateCommand(CloseInterfaceKit, CloseInterfaceKitCanExecute);

            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNC.Phidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            CreateChannels();

            Message = "InterfaceKitViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //private void InterfaceKit1018ViewModel_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig ? hostConfig = 
                JsonSerializer.Deserialize< VNCPhidgetConfig.HostConfig >
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();
            
            //Sensors2 = hostConfig.Sensors.ToList();

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return jsonOptions;
        }
        private void CreateChannels()
        {
            // NOTE(crhodes)
            // This ViewModel needs to support all types of InterfaceKits
            // Configure all channels that might exist and wire up event handlers
            // The SelectedInterfaceKit property change event can enable and disable as needed.
            // TODO(crhodes)
            // Figure out what do in UI (if anything) if all channels not present on a device
            // Hide controls maybe


            //DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels;

            //DigitalInputs = new DigitalInputEx[deviceChannels.DigitalInputCount];
            //DigitalOutputs = new DigitalOutputEx[deviceChannels.DigitalOutputCount];
            //VoltageInputs = new VoltageInputEx[deviceChannels.VoltageInputCount];
            //VoltageRatioInputs = new VoltageRatioInputEx[deviceChannels.VoltageRatioInputCount];
            //VoltageOutputs = new VoltageOutputEx[deviceChannels.VoltageOutputCount];

            // NOTE(crhodes)
            // Need to create early so bindings work.

            // TODO(crhodes)
            // 
            // Handle the most an InterfaceKit might have
            // Maybe initialize some defaults, and channel

            for (int i = 0; i < 16; i++)
            {
                DigitalInputs[i] = new DigitalInputEx(0, new DigitalInputConfiguration() { Channel = (Int16)i }, EventAggregator);
            }
            for (int i = 0; i < 16; i++)
            {
                DigitalOutputs[i] = new DigitalOutputEx(0, new DigitalOutputConfiguration() { Channel = (Int16)i }, EventAggregator);
            }
            for (int i = 0; i < 8; i++)
            {
                VoltageInputs[i] = new VoltageInputEx(0, new RCServoConfiguration() { Channel = (Int16)i }, EventAggregator);
            }
            for (int i = 0; i < 8; i++)
            {
                VoltageRatioInputs[i] = new VoltageRatioInputEx(0, new VoltageRatioInputConfiguration() { Channel = (Int16)i }, EventAggregator);
            }
            for (int i = 0; i < 8; i++)
            {
                VoltageOutputs[i] = new VoltageOutputEx(0, new VoltageOutputConfiguration() { Channel = (Int16)i }, EventAggregator);
            }
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #region Host

        private string _hostConfigFileName;

        public string HostConfigFileName
        {
            get => _hostConfigFileName;
            set
            {
                if (_hostConfigFileName == value) return;
                _hostConfigFileName = value;
                OnPropertyChanged();
            }
        }

        public string HostConfigFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private IEnumerable<VNCPhidgetConfig.Host> _Hosts;
        public IEnumerable<VNCPhidgetConfig.Host> Hosts
        {
            get => _Hosts;
            set
            {
                _Hosts = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Host _selectedHost;
        public VNCPhidgetConfig.Host SelectedHost
        {
            get => _selectedHost;
            set
            {
                if (_selectedHost == value)
                    return;
                _selectedHost = value;
                InterfaceKits = _selectedHost.InterfaceKits.ToList<VNCPhidgetConfig.InterfaceKit>();
                OnPropertyChanged();
            }
        }

        //private IEnumerable<VNCPhidgetConfig.Sensor> _Sensors2;
        //public IEnumerable<VNCPhidgetConfig.Sensor> Sensors2
        //{
        //    get
        //    {
        //        if (null == _Sensors2)
        //        {
        //            // TODO(crhodes)
        //            // Load this like the sensors.xml for now

        //            //_Sensors =
        //            //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Sensors").Elements("Sensor")
        //            //    select new Sensor(
        //            //        item.Attribute("Name").Value,
        //            //        item.Attribute("IPAddress").Value,
        //            //        item.Attribute("Port").Value,
        //            //        bool.Parse(item.Attribute("Enable").Value)
        //            //        );
        //        }

        //        return _Sensors2;
        //    }

        //    set
        //    {
        //        _Sensors2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Phidget

        private Phidgets.Phidget _phidget22Device;
        public Phidgets.Phidget Phidget22Device
        {
            get => _phidget22Device;
            set
            {
                if (_phidget22Device == value)
                    return;
                _phidget22Device = value;
                OnPropertyChanged();
            }
        }

        private bool _logPhidgetEvents = false;
        public bool LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();

                if (ActiveInterfaceKit is not null) ActiveInterfaceKit.LogPhidgetEvents = value;
            }
        }

        private bool _logSequenceAction = false;
        public bool LogSequenceAction
        {
            get => _logSequenceAction;
            set
            {
                if (_logSequenceAction == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();

                if (ActiveInterfaceKit is not null)
                {
                    ActiveInterfaceKit.LogSequenceAction = value;
                }
            }
        }

        // TODO(crhodes)
        // Since channels are now the focus, do we need this?

        private bool? _deviceAttached;
        public bool? DeviceAttached
        {
            get => _deviceAttached;
            set
            {
                if (_deviceAttached == value)
                    return;
                _deviceAttached = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region InterfaceKit

        DigitalInputEx[] _digitalInputs = new DigitalInputEx[16];
        public DigitalInputEx[] DigitalInputs
        {
            get
            {
                return _digitalInputs;
            }
            set
            {
                _digitalInputs = value;
                OnPropertyChanged();
            }
        }

        DigitalOutputEx[] _digitalOutputs = new DigitalOutputEx[16];
        public DigitalOutputEx[] DigitalOutputs
        {
            get
            {
                return _digitalOutputs;
            }
            set
            {
                _digitalOutputs = value;
                OnPropertyChanged();
            }
        }

        VoltageInputEx[] _voltageInputs = new VoltageInputEx[8];
        public VoltageInputEx[] VoltageInputs
        {
            get
            {
                return _voltageInputs;
            }
            set
            {
                _voltageInputs = value;
                OnPropertyChanged();
            }
        }

        VoltageRatioInputEx[] _voltageRatioInputs = new VoltageRatioInputEx[8];
        public VoltageRatioInputEx[] VoltageRatioInputs
        {
            get
            {
                return _voltageRatioInputs;
            }
            set
            {
                _voltageRatioInputs = value;
                OnPropertyChanged();
            }
        }

        VoltageOutputEx[] _voltageOutputs = new VoltageOutputEx[8];
        public VoltageOutputEx[] VoltageOutputs
        {
            get
            {
                return _voltageOutputs;
            }
            set
            {
                _voltageOutputs = value;
                OnPropertyChanged();
            }
        }

        #region InterfaceKit Events

        private bool _logInputChangeEvents = false;
        public bool LogInputChangeEvents
        {
            get => _logInputChangeEvents;
            set
            {
                if (_logInputChangeEvents == value)
                    return;
                _logInputChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogSensorChangeEvents = _logInputChangeEvents;
                //}
            }
        }

        private bool _logOutputChangeEvents = false;
        public bool LogOutputChangeEvents
        {
            get => _logOutputChangeEvents;
            set
            {
                if (_logOutputChangeEvents == value)
                    return;
                _logOutputChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogOutputChangeEvents = _logOutputChangeEvents;
                //}
            }
        }

        private bool _logSensorChangeEvents = false;
        public bool LogSensorChangeEvents
        {
            get => _logSensorChangeEvents;
            set
            {
                if (_logSensorChangeEvents == value)
                    return;
                _logSensorChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogSensorChangeEvents = _logSensorChangeEvents;
                //}
            }
        }

        #endregion

        private IEnumerable<VNCPhidgetConfig.InterfaceKit> _InterfaceKits;
        public IEnumerable<VNCPhidgetConfig.InterfaceKit> InterfaceKits
        {
            get
            {
                if (null == _InterfaceKits)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_InterfaceKits =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
                    //    select new InterfaceKit(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _InterfaceKits;
            }

            set
            {
                _InterfaceKits = value;
                OnPropertyChanged();
            }
        }

        private Visibility _digitalInputsVisibility = Visibility.Collapsed;
        public Visibility DigitalInputsVisibility
        {
            get => _digitalInputsVisibility;
            set
            {
                if (_digitalInputsVisibility == value)
                    return;
                _digitalInputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _digitalOutputsVisibility = Visibility.Collapsed;
        public Visibility DigitalOutputsVisibility
        {
            get => _digitalOutputsVisibility;
            set
            {
                if (_digitalOutputsVisibility == value)
                    return;
                _digitalOutputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _voltageInputsVisibility = Visibility.Collapsed;
        public Visibility VoltageInputsVisibility
        {
            get => _voltageInputsVisibility;
            set
            {
                if (_voltageInputsVisibility == value)
                    return;
                _voltageInputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _voltageOutputsVisibility = Visibility.Collapsed;
        public Visibility VoltageOutputsVisibility
        {
            get => _voltageOutputsVisibility;
            set
            {
                if (_voltageOutputsVisibility == value)
                    return;
                _voltageOutputsVisibility = value;
                OnPropertyChanged();
            }
        }
        
        private VNCPhidgetConfig.InterfaceKit _selectedInterfaceKit;
        public VNCPhidgetConfig.InterfaceKit SelectedInterfaceKit
        {
            get => _selectedInterfaceKit;
            set
            {
                if (_selectedInterfaceKit == value)
                    return;
                _selectedInterfaceKit = value;

                OpenInterfaceKitCommand.RaiseCanExecuteChanged();

                // Set to null when host changes
                if (value is not null)
                {
                    DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[value.SerialNumber].DeviceChannels;

                    DigitalInputsVisibility = deviceChannels.DigitalInputCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                    DigitalOutputsVisibility = deviceChannels.DigitalOutputCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                    VoltageInputsVisibility = deviceChannels.VoltageInputCount > 0 || deviceChannels.VoltageRatioInputCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                    VoltageOutputsVisibility = deviceChannels.VoltageOutputCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    DigitalInputsVisibility = Visibility.Collapsed;
                    DigitalOutputsVisibility = Visibility.Collapsed;
                    VoltageInputsVisibility = Visibility.Collapsed;
                    VoltageOutputsVisibility = Visibility.Collapsed;
                }

                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // May want to make this just a Phidget22.InterfaceKit to avoid all the ActiveInterfaceKit.InterfaceKit stuff

        private DigitalOutputEx _activeInterfaceKit;
        public DigitalOutputEx ActiveInterfaceKit
        {
            get => _activeInterfaceKit;
            set
            {
                if (_activeInterfaceKit == value)
                    return;
                _activeInterfaceKit = value;

                //if (_activeInterfaceKit is not null)
                //{
                //    // FIX(crhodes)
                //    // 
                //    //PhidgetDevice = _activeInterfaceKit.InterfaceKit;
                //    // Not sure we need to do this.  See code in Open.
                //    Phidget22Device = _activeInterfaceKit.PhysicalPhidget;
                //}
                //else
                //{
                //    // TODO(crhodes)
                //    // PhidgetDevice = null ???
                //    // Will need to declare Phidget22.Phidget?
                //    Phidget22Device = null;
                //}

                OnPropertyChanged();
            }
        }

        #region InterfaceKit Phidget Properties

        #endregion

        #endregion

        #endregion

        #region Event Handler


        #endregion

        #region Commands

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Enter", Common.LOG_CATEGORY);

            Message = "ConfigFileName_DoubleClick";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region OpenInterfaceKit Command

        public DelegateCommand OpenInterfaceKitCommand { get; set; }
        public string OpenInterfaceKitContent { get; set; } = "Open";
        public string OpenInterfaceKitToolTip { get; set; } = "OpenInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string OpenInterfaceKitContent { get; set; } = "ViewName_OpenInterfaceKitContent";
        //public string OpenInterfaceKitToolTip { get; set; } = "ViewName_OpenInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenInterfaceKitContent">OpenInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_OpenInterfaceKitContentToolTip">OpenInterfaceKit ToolTip</system:String>  

        public async void OpenInterfaceKit()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenInterfaceKit) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenInterfaceKit";

            //ActiveInterfaceKit = new InterfaceKitEx(
            //    SelectedHost.IPAddress,
            //    SelectedHost.Port,
            //    SelectedInterfaceKit.SerialNumber,
            //    EventAggregator);

            //ActiveInterfaceKit = new InterfaceKitEx(
            //    SelectedInterfaceKit.SerialNumber,
            //    Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels,
            //    EventAggregator);

            // FIX(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.Attach += ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.InterfaceKit.Detach += ActiveInterfaceKit_Detach;


            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The InterfaceKitEx attaches to these events also.
            // It logs the changes if Log{Input,Output,Sensor}ChangeEvents are set to true.

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.OutputChange += ActiveInterfaceKit_OutputChange;
            //ActiveInterfaceKit.InterfaceKit.InputChange += ActiveInterfaceKit_InputChange;

            // NOTE(crhodes)
            // Let's do see if we can watch some analog data stream in.

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.SensorChange += ActiveInterfaceKit_SensorChange;

            //ConfigurePhidgets();

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels;

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;

            ConfigureDigitalInputs(deviceChannels.DigitalInputCount, serialNumber);

            for (int i = 0; i < deviceChannels.DigitalInputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                DigitalInputs[i].Open();
                //await Task.Run(() => DigitalOutputs[i].Open(500));
            }

            ConfigureDigitalOutputs(deviceChannels.DigitalOutputCount, serialNumber);

            for (int i = 0; i < deviceChannels.DigitalOutputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available
                
                DigitalOutputs[i].Open();
                //await Task.Run(() => DigitalOutputs[i].Open(500));
            }

            ConfigureVoltageInputs(deviceChannels.VoltageInputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageInputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available
                
                VoltageInputs[i].Open();
                //await Task.Run(() => VoltageOutputs[i].Open(500));
            }

            ConfigureVoltageRatioInputs(deviceChannels.VoltageRatioInputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageRatioInputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                VoltageRatioInputs[i].Open();
                //await Task.Run(() => VoltageOutputs[i].Open(500));
            }

            ConfigureVoltageOutputs(deviceChannels.VoltageOutputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageOutputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                VoltageOutputs[i].Open();
                //await Task.Run(() => VoltageOutputs[i].Open(500));
            }

            //for (int i = 0; i < VoltageInputs.Count(); i++)
            //{
            //    // NOTE(crhodes)
            //    // If do not specify a timeout, Open() returns
            //    // before initial state is available

            //    DigitalInputs[i].Open();
            //    //await Task.Run(() => DigitalOutputs[i].Open(500));
            //}

            //for (int i = 0; i < DigitalInputs.Count(); i++)
            //{
            //    // NOTE(crhodes)
            //    // If do not specify a timeout, Open() returns
            //    // before initial state is available

            //    DigitalInputs[i].Open();
            //    //await Task.Run(() => DigitalOutputs[i].Open(500));
            //}

            //ActiveInterfaceKit.LogPhidgetEvents = LogPhidgetEvents;

            ////ActiveInterfaceKit.LogInputChangeEvents = LogInputChangeEvents;
            ////ActiveInterfaceKit.LogOutputChangeEvents = LogOutputChangeEvents;
            ////ActiveInterfaceKit.LogSensorChangeEvents = LogSensorChangeEvents;

            ////ActiveInterfaceKit.PhidgetDeviceAttached += ActiveInterfaceKit_PhidgetDeviceAttached;

            //var pdBefore = Phidget22Device;

            //await Task.Run(() => ActiveInterfaceKit.Open());

            //Phidget22Device = ActiveInterfaceKit.PhysicalPhidget;

            // NOTE(crhodes)
            // This won't work as Phidget22Device won't be set until Phidget_Attach event fires

            //DeviceAttached = Phidget22Device.Attached;

            // NOTE(crhodes)
            // I don't think these are gonna fire given how we populate PhysicalPhidget

            //ActiveInterfaceKit.PhysicalPhidget.Attach += ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.PhysicalPhidget.Detach += ActiveInterfaceKit_Detach;


            //ActiveInterfaceKit.LogPhidgetEvents = LogPhidgetEvents;

            //ActiveInterfaceKit.LogInputChangeEvents = LogInputChangeEvents;
            //ActiveInterfaceKit.LogOutputChangeEvents = LogOutputChangeEvents;
            //ActiveInterfaceKit.LogSensorChangeEvents = LogSensorChangeEvents;

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish(
            //      new OpenInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Subscribe(OpenInterfaceKit);

            // End Cut Four

            //OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            //CloseInterfaceKitCommand.RaiseCanExecuteChanged();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenInterfaceKit) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // TODO(crhodes)
        // Maybe this is where we use ChannelCounts and some type of Configuration Request
        // to only do this for some.  This is called in Open to set the SerialNumber

        private void ConfigureDigitalInputs(Int16 channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //DigitalInputs[i] = new DigitalOutputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new DigitalOutputConfiguration() { Channel = i },
                //    EventAggregator);

                // FIX(crhodes)
                // Need to figure out a way to set the SerialNumber before we open
                // How now just hard code, ugh.
                //DigitalInputs[i] = new DigitalInputEx(
                //    serialNumber,
                //    new DigitalInputConfiguration() { Channel = i },
                //    EventAggregator);
                DigitalInputs[i].SerialNumber = serialNumber;
            }
        }

        private void ConfigureDigitalOutputs(Int16 channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //DigitalOutputs[i] = new DigitalOutputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new DigitalOutputConfiguration() { Channel = i },
                //    EventAggregator);

                // FIX(crhodes)
                // Need to figure out a way to set the SerialNumber before we open
                // How now just hard code, ugh.
                //DigitalOutputs[i] = new DigitalOutputEx(
                //    serialNumber,
                //    new DigitalOutputConfiguration() { Channel = i },
                //    EventAggregator);
                DigitalOutputs[i].SerialNumber = serialNumber;
            }
        }

        private void ConfigureVoltageInputs(Int16 channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //VoltageInputs[i] = new VoltageInputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new VoltageInputConfiguration() { Channel = i },
                //    EventAggregator);

                // FIX(crhodes)
                // Need to figure out a way to set the SerialNumber before we open
                // How now just hard code, ugh.
                //VoltageInputs[i] = new VoltageInputEx(
                //    serialNumber,
                //    new VoltageInputConfiguration() { Channel = i },
                //    EventAggregator);
                VoltageInputs[i].SerialNumber = serialNumber;
            }
        }

        private void ConfigureVoltageRatioInputs(Int16 channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //VoltageRatioInputs[i] = new VoltageRatioInputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new VoltageRatioInputConfiguration() { Channel = i },
                //    EventAggregator);

                // FIX(crhodes)
                // Need to figure out a way to set the SerialNumber before we open
                // How now just hard code, ugh.
                //VoltageRatioInputs[i] = new VoltageRatioInputEx(
                //    serialNumber,
                //    new VoltageRatioInputConfiguration() { Channel = i },
                //    EventAggregator);
                VoltageRatioInputs[i].SerialNumber = serialNumber;
            }
        }

        private void ConfigureVoltageOutputs(Int16 channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //VoltageOutputs[i] = new VoltageOutputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new VoltageOutputConfiguration() { Channel = i },
                //    EventAggregator);

                // FIX(crhodes)
                // Need to figure out a way to set the SerialNumber before we open
                // How now just hard code, ugh.
                //VoltageOutputs[i] = new VoltageOutputEx(
                //    serialNumber,
                //    new VoltageOutputConfiguration() { Channel = i },
                //    EventAggregator);
                VoltageOutputs[i].SerialNumber = serialNumber;
            }
        }


        //private void ActiveInterfaceKit_PhidgetDeviceAttached(object? sender, EventArgs e)
        //{
        //    Phidget22Device = ActiveInterfaceKit.PhysicalPhidget;

        //    DeviceAttached = Phidget22Device.Attached;

        //    // NOTE(crhodes)
        //    // This won't work as Phidget22Device won't be set until Phidget_Attach event fires

        //    //DeviceAttached = Phidget22Device.Attached;

        //    // NOTE(crhodes)
        //    // I don't think these are gonna fire given how we populate PhysicalPhidget

        //    //ActiveInterfaceKit.PhysicalPhidget.Attach += ActiveInterfaceKit_Attach;
        //    //ActiveInterfaceKit.PhysicalPhidget.Detach += ActiveInterfaceKit_Detach;

        //    // FIX(crhodes)
        //    // This is a problem.  We have to wait until all DI, DO, VI, VO devices that were openned
        //    // attach.  Looks like we are going to have to go to separate event handlers for each channel. Ugh
        //    UpdateInterfaceKitProperties();
        //}

        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedInterfaceKit is not null)
            {
                //if (DeviceAttached is not null)
                //    return !(Boolean)DeviceAttached;
                //else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseInterfaceKit Command

        public DelegateCommand CloseInterfaceKitCommand { get; set; }
        public string CloseInterfaceKitContent { get; set; } = "Close";
        public string CloseInterfaceKitToolTip { get; set; } = "CloseInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string CloseInterfaceKitContent { get; set; } = "ViewName_CloseInterfaceKitContent";
        //public string CloseInterfaceKitToolTip { get; set; } = "ViewName_CloseInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseInterfaceKitContent">CloseInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_CloseInterfaceKitContentToolTip">CloseInterfaceKit ToolTip</system:String>  

        public void CloseInterfaceKit()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseInterfaceKit) Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseInterfaceKit";

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.Attach -= ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.InterfaceKit.Detach -= ActiveInterfaceKit_Detach;

            //ActiveInterfaceKit.Close();

            for (int i = 0; i < DigitalInputs.Count(); i++)
            {
                DigitalInputs[i].Close();
            }

            for (int i = 0; i < DigitalOutputs.Count(); i++)
            {
                DigitalOutputs[i].Close();
            }

            for (int i = 0; i < VoltageInputs.Count(); i++)
            {
                VoltageInputs[i].Close();
            }

            for (int i = 0; i < VoltageRatioInputs.Count(); i++)
            {
                VoltageRatioInputs[i].Close();
            }

            for (int i = 0; i < VoltageOutputs.Count(); i++)
            {
                VoltageOutputs[i].Close();
            }

            UpdateInterfaceKitProperties();
            ActiveInterfaceKit = null;
            ClearDigitalInputsAndOutputs();

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();

            CloseInterfaceKitCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish(
            //      new CloseInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Subscribe(CloseInterfaceKit);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseInterfaceKit) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
            //if (DeviceAttached is not null)
            //    return (Boolean)DeviceAttached;
            //else
            //    return false;
        }

        #endregion

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private void ClearDigitalInputsAndOutputs()
        {
            //DI0 = DO0 = null;
            //DI1 = DO1 = null;
            //DI2 = DO2 = null;
            //DI3 = DO3 = null;
            //DI4 = DO4 = null;
            //DI5 = DO5 = null;
            //DI6 = DO6 = null;
            //DI7 = DO7 = null;
            //DI8 = DO8 = null;
            //DI9 = DO9 = null;
            //DI10 = DO10 = null;
            //DI11 = DO11 = null;
            //DI12 = DO12 = null;
            //DI13 = DO13 = null;
            //DI14 = DO14 = null;
            //DI15 = DO15 = null;
        }

        // TODO(crhodes)
        // 
        //private void PopulateSensorValues(InterfaceKitAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        private void UpdateInterfaceKitProperties()
        {
            //var interfaceKitEx = ActiveInterfaceKit;
            //var phidget = interfaceKitEx.PhysicalPhidget;

            //DO1 = interfaceKitEx.DigitalOutputs[1].State;

            //Phidgets.DigitalInput[] DigitalInputs = 
            //for (int i = 0; i < interfaceKitEx.DigitalInputs.Count(); i++)
            //{
            //    DigitalInputs[i].Open();
            //}

            //for (int i = 0; i < digitalOutputCount; i++)
            //{
            //    DigitalOutputs[i].Open();
            //}

            //for (int i = 0; i < voltageInputCount; i++)
            //{
            //    VoltageInputs[i].Open();
            //}

            //for (int i = 0; i < voltageRatioInputCount; i++)
            //{
            //    VoltageRatioInputs[i].Open();
            //}

            //for (int i = 0; i < voltageOutputCount; i++)
            //{
            //    VoltageOutputs[i].Open();

            //}

            // FIX(crhodes)
            // 
            //if (ActiveInterfaceKit.InterfaceKit.Attached)
            //{
            //    //IkAddress = ActiveInterfaceKit.InterfaceKit.Address;
            //    //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    DeviceAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    //IkAttachedToServer = ActiveInterfaceKit.InterfaceKit.AttachedToServer;
            //    //IkClass = ActiveInterfaceKit.InterfaceKit.Class.ToString();
            //    //IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.InterfaceKit.ID);
            //    //IkLabel = ActiveInterfaceKit.InterfaceKit.Label;
            //    //IkLibraryVersion = Phidget.LibraryVersion;  // This is a static field
            //    //IkName = ActiveInterfaceKit.InterfaceKit.Name;
            //    //IkPort = ActiveInterfaceKit.InterfaceKit.Port;
            //    //IkSerialNumber = ActiveInterfaceKit.InterfaceKit.SerialNumber; // This throws exception
            //    ////IkServerID = ActiveInterfaceKit.ServerID;
            //    //IkType = ActiveInterfaceKit.InterfaceKit.Type;
            //    //IkVersion = ActiveInterfaceKit.InterfaceKit.Version;

            //    var sensors = ActiveInterfaceKit.InterfaceKit.sensors;
            //    InterfaceKitAnalogSensor sensor = null;

            //    // NOTE(crhodes)
            //    // The DataRateMin and DataRateMax do not change.
            //    // Populate them here instead of SensorChange event

            //    // TODO(crhodes)
            //    // May want to grab initial values for all fields here.

            //    for (int i = 0; i < sensors.Count; i++)
            //    {
            //        sensor = sensors[i];

            //        switch (i)
            //        {
            //            case 0:
            //                AIDataRateMax0 = sensor.DataRateMax;
            //                AIDataRateMin0 = sensor.DataRateMin;
            //                break;
            //            case 1:
            //                AIDataRateMax1 = sensor.DataRateMax;
            //                AIDataRateMin1 = sensor.DataRateMin;
            //                break;
            //            case 2:
            //                AIDataRateMax2 = sensor.DataRateMax;
            //                AIDataRateMin2 = sensor.DataRateMin;
            //                break;
            //            case 3:
            //                AIDataRateMax3 = sensor.DataRateMax;
            //                AIDataRateMin3 = sensor.DataRateMin;
            //                break;
            //            case 4:
            //                AIDataRateMax4 = sensor.DataRateMax;
            //                AIDataRateMin4 = sensor.DataRateMin;
            //                break;
            //            case 5:
            //                AIDataRateMax5 = sensor.DataRateMax;
            //                AIDataRateMin5 = sensor.DataRateMin;
            //                break;
            //            case 6:
            //                AIDataRateMax6 = sensor.DataRateMax;
            //                AIDataRateMin6 = sensor.DataRateMin;
            //                break;
            //            case 7:
            //                AIDataRateMax7 = sensor.DataRateMax;
            //                AIDataRateMin7 = sensor.DataRateMin;
            //                break;
            //        }
            //    }
            //}
            //else
            //{
            //    DeviceAttached = null;
            //    // NOTE(crhodes)
            //    // Commented out properties throw exceptions when Phidget not attached
            //    // Just clear field

            //    //IkAddress = ActiveInterfaceKit.Address;
            //    //IkAddress = "";
            //    //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    ////IkAttachedToServer = ActiveInterfaceKit.AttachedToServer;
            //    //IkAttachedToServer = false;
            //    //// This doesn't throw exception but let's clear anyway
            //    ////IkClass = ActiveInterfaceKit.Class.ToString();
            //    //IkClass = "";
            //    ////IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.ID);
            //    //IkID = "";
            //    ////IkLabel = ActiveInterfaceKit.Label;
            //    //IkLabel = "";
            //    ////IkLibraryVersion = ActiveInterfaceKit.LibraryVersion;
            //    //IkLibraryVersion = Phidget.LibraryVersion;
            //    ////IkName = ActiveInterfaceKit.Name;
            //    //IkName = "";
            //    ////IkSerialNumber = ActiveInterfaceKit.SerialNumber;
            //    //IkSerialNumber = null;
            //    ////IkServerID = ActiveInterfaceKit.ServerID;
            //    //IkServerID = "";
            //    ////IkType = ActiveInterfaceKit.Type;
            //    //IkType = "";
            //    ////IkVersion = ActiveInterfaceKit.Version;
            //    //IkVersion = null;
            //}

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            CloseInterfaceKitCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
