using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

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

            ConfigureDigitalOutputs(8);

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

        public DigitalInputEx[] DigitalInputs { get; set; }

        DigitalOutputEx[] _digitalOutputs = new DigitalOutputEx[8];
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

        //public VoltageInputEx[] VoltageInputs;
        //public VoltageRatioInputEx[] VoltageRatioInputs;

        //public VoltageOutputEx[] VoltageOutputs;

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

            ConfigurePhidgets();

            for (int i = 0; i < DigitalOutputs.Count(); i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                DigitalOutputs[i].Open();
                //await Task.Run(() => DigitalOutputs[i].Open(500));
                var dOut = DigitalOutputs[i];

                // NOTE(crhodes)
                // Have to set attached here as it is not set
                // until after Attach Event completes
                //DigitalOutputs[i].IsAttached = DigitalOutputs[i].Attached;
            }

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

        private void ConfigurePhidgets()
        {
            // NOTE(crhodes)
            // This ViewModel needs to support all types of InterfaceKits
            // Configure all channels that might exist and wire up event handlers
            // The SelectedInterfaceKit property change event can enable and disable as needed.
            // TODO(crhodes)
            // Figure out what do in UI (if anything) if all channels not present on a device
            // Hide controls maybe

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels;

            DigitalInputs = new DigitalInputEx[deviceChannels.DigitalInputCount];
            //DigitalOutputs = new DigitalOutputEx[deviceChannels.DigitalOutputCount];
            //VoltageInputs = new Phidgets.VoltageInput[deviceChannels.VoltageInputCount];
            //VoltageRatioInputs = new Phidgets.VoltageRatioInput[deviceChannels.VoltageRatioInputCount];
            //VoltageOutputs = new Phidgets.VoltageOutput[deviceChannels.VoltageOutputCount];

            // NOTE(crhodes)
            // Cannot do this get as we don't have a selected Phidget/SerialNumber

            ConfigureDigitalInputs(deviceChannels.DigitalInputCount);
            //ConfigureDigitalOutputs(deviceChannels.DigitalOutputCount);
            ConfigureVoltageInputs(deviceChannels.VoltageInputCount);
            ConfigureVoltageRatioInputs(deviceChannels.VoltageRatioInputCount);
        }



        // TODO(crhodes)
        // Maybe this is where we use ChannelCounts and some type of Configuration Request
        // to only do this for some

        private void ConfigureDigitalInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            for (int i = 0; i < channelCount; i++)
            {
                //DigitalInputs[i] = new DigitalInputEx(SelectedInterfaceKit.SerialNumber, , ;
                //var channel = DigitalInputs[i];

                //channel.DeviceSerialNumber = SerialNumber;
                //channel.Channel = i;
                //channel.IsHubPortDevice = false;
                //channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
                //channel.StateChange += Channel_DigitalInputStateChange;
            }

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[0].Attach += DI0_Attach;
            //    ActiveInterfaceKit.DigitalInputs[0].Detach += DI0_Detach;
            //    ActiveInterfaceKit.DigitalInputs[0].PropertyChange += DI0_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[0].StateChange += DI0_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[0].Error += DI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[1].Attach += DI1_Attach;
            //    ActiveInterfaceKit.DigitalInputs[1].Detach += DI1_Detach;
            //    ActiveInterfaceKit.DigitalInputs[1].PropertyChange += DI1_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[1].StateChange += DI1_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[1].Error += DI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[2].Attach += DI2_Attach;
            //    ActiveInterfaceKit.DigitalInputs[2].Detach += DI2_Detach;
            //    ActiveInterfaceKit.DigitalInputs[2].PropertyChange += DI2_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[2].StateChange += DI2_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[2].Error += DI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[3].Attach += DI3_Attach;
            //    ActiveInterfaceKit.DigitalInputs[3].Detach += DI3_Detach;
            //    ActiveInterfaceKit.DigitalInputs[3].PropertyChange += DI3_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[3].StateChange += DI3_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[3].Error += DI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[4].Attach += DI4_Attach;
            //    ActiveInterfaceKit.DigitalInputs[4].Detach += DI4_Detach;
            //    ActiveInterfaceKit.DigitalInputs[4].PropertyChange += DI4_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[4].StateChange += DI4_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[4].Error += DI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[5].Attach += DI5_Attach;
            //    ActiveInterfaceKit.DigitalInputs[5].Detach += DI5_Detach;
            //    ActiveInterfaceKit.DigitalInputs[5].PropertyChange += DI5_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[5].StateChange += DI5_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[5].Error += DI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[6].Attach += DI6_Attach;
            //    ActiveInterfaceKit.DigitalInputs[6].Detach += DI6_Detach;
            //    ActiveInterfaceKit.DigitalInputs[6].PropertyChange += DI6_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[6].StateChange += DI6_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[6].Error += DI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[7].Attach += DI7_Attach;
            //    ActiveInterfaceKit.DigitalInputs[7].Detach += DI7_Detach;
            //    ActiveInterfaceKit.DigitalInputs[7].PropertyChange += DI7_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[7].StateChange += DI7_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[7].Error += DI7_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[8].Attach += DI8_Attach;
            //    ActiveInterfaceKit.DigitalInputs[8].Detach += DI8_Detach;
            //    ActiveInterfaceKit.DigitalInputs[8].PropertyChange += DI8_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[8].StateChange += DI8_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[8].Error += DI8_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[9].Attach += DI9_Attach;
            //    ActiveInterfaceKit.DigitalInputs[9].Detach += DI9_Detach;
            //    ActiveInterfaceKit.DigitalInputs[9].PropertyChange += DI9_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[9].StateChange += DI9_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[9].Error += DI9_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[10].Attach += DI10_Attach;
            //    ActiveInterfaceKit.DigitalInputs[10].Detach += DI10_Detach;
            //    ActiveInterfaceKit.DigitalInputs[10].PropertyChange += DI10_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[10].StateChange += DI10_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[10].Error += DI10_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[11].Attach += DI11_Attach;
            //    ActiveInterfaceKit.DigitalInputs[11].Detach += DI11_Detach;
            //    ActiveInterfaceKit.DigitalInputs[11].PropertyChange += DI11_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[11].StateChange += DI11_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[11].Error += DI11_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[12].Attach += DI12_Attach;
            //    ActiveInterfaceKit.DigitalInputs[12].Detach += DI12_Detach;
            //    ActiveInterfaceKit.DigitalInputs[12].PropertyChange += DI12_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[12].StateChange += DI12_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[12].Error += DI12_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[13].Attach += DI13_Attach;
            //    ActiveInterfaceKit.DigitalInputs[13].Detach += DI13_Detach;
            //    ActiveInterfaceKit.DigitalInputs[13].PropertyChange += DI13_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[13].StateChange += DI13_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[13].Error += DI13_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[14].Attach += DI14_Attach;
            //    ActiveInterfaceKit.DigitalInputs[14].Detach += DI14_Detach;
            //    ActiveInterfaceKit.DigitalInputs[14].PropertyChange += DI14_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[14].StateChange += DI14_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[14].Error += DI14_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[15].Attach += DI15_Attach;
            //    ActiveInterfaceKit.DigitalInputs[15].Detach += DI15_Detach;
            //    ActiveInterfaceKit.DigitalInputs[15].PropertyChange += DI15_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[15].StateChange += DI15_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[15].Error += DI15_Error;
            //}
        }

        private void ConfigureDigitalOutputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            for (Int16 i = 0; i < channelCount; i++)
            {
                //DigitalOutputs[i] = new DigitalOutputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new DigitalOutputConfiguration() { Channel = i },
                //    EventAggregator);

                DigitalOutputs[i] = new DigitalOutputEx(
                    48284,
                    new DigitalOutputConfiguration() { Channel = i },
                    EventAggregator);

                //var channel = DigitalOutputs[i];

                //channel.DeviceSerialNumber = SerialNumber;
                //channel.Channel = i;
                //channel.IsHubPortDevice = false;
                //channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
            }

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[0].Attach += DO0_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[0].Detach += DO0_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[0].PropertyChange += DO0_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[0].Error += DO0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[1].Attach += DO1_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[1].Detach += DO1_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[1].PropertyChange += DO1_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[1].Error += DO1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[2].Attach += DO2_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[2].Detach += DO2_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[2].PropertyChange += DO2_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[2].Error += DO2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[3].Attach += DO3_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[3].Detach += DO3_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[3].PropertyChange += DO3_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[3].Error += DO3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[4].Attach += DO4_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[4].Detach += DO4_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[4].PropertyChange += DO4_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[4].Error += DO4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[5].Attach += DO5_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[5].Detach += DO5_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[5].PropertyChange += DO5_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[5].Error += DO5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[6].Attach += DO6_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[6].Detach += DO6_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[6].PropertyChange += DO6_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[6].Error += DO6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[7].Attach += DO7_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[7].Detach += DO7_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[7].PropertyChange += DO7_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[7].Error += DO7_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[8].Attach += DO8_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[8].Detach += DO8_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[8].PropertyChange += DO8_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[8].Error += DO8_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[9].Attach += DO9_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[9].Detach += DO9_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[9].PropertyChange += DO9_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[9].Error += DO9_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[10].Attach += DO10_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[10].Detach += DO10_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[10].PropertyChange += DO10_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[10].Error += DO10_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[11].Attach += DO11_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[11].Detach += DO11_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[11].PropertyChange += DO11_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[11].Error += DO11_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[12].Attach += DO12_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[12].Detach += DO12_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[12].PropertyChange += DO12_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[12].Error += DO12_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[13].Attach += DO13_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[13].Detach += DO13_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[13].PropertyChange += DO13_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[13].Error += DO13_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[14].Attach += DO14_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[14].Detach += DO14_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[14].PropertyChange += DO14_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[14].Error += DO14_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[15].Attach += DO15_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[15].Detach += DO15_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[15].PropertyChange += DO15_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[15].Error += DO15_Error;
            //}
        }

        private void ConfigureVoltageInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[0].Attach += AI0_Attach;
            //    ActiveInterfaceKit.VoltageInputs[0].Detach += AI0_Detach;
            //    ActiveInterfaceKit.VoltageInputs[0].PropertyChange += AI0_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[0].SensorChange += AI0_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[0].VoltageChange += AI0_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[0].Error += AI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[1].Attach += AI1_Attach;
            //    ActiveInterfaceKit.VoltageInputs[1].Detach += AI1_Detach;
            //    ActiveInterfaceKit.VoltageInputs[1].PropertyChange += AI1_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[1].SensorChange += AI1_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[1].VoltageChange += AI1_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[1].Error += AI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[2].Attach += AI2_Attach;
            //    ActiveInterfaceKit.VoltageInputs[2].Detach += AI2_Detach;
            //    ActiveInterfaceKit.VoltageInputs[2].PropertyChange += AI2_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[2].SensorChange += AI2_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[2].VoltageChange += AI2_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[2].Error += AI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[3].Attach += AI3_Attach;
            //    ActiveInterfaceKit.VoltageInputs[3].Detach += AI3_Detach;
            //    ActiveInterfaceKit.VoltageInputs[3].PropertyChange += AI3_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[3].SensorChange += AI3_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[3].VoltageChange += AI3_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[3].Error += AI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[4].Attach += AI4_Attach;
            //    ActiveInterfaceKit.VoltageInputs[4].Detach += AI4_Detach;
            //    ActiveInterfaceKit.VoltageInputs[4].PropertyChange += AI4_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[4].SensorChange += AI4_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[4].VoltageChange += AI4_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[4].Error += AI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[5].Attach += AI5_Attach;
            //    ActiveInterfaceKit.VoltageInputs[5].Detach += AI5_Detach;
            //    ActiveInterfaceKit.VoltageInputs[5].PropertyChange += AI5_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[5].SensorChange += AI5_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[5].VoltageChange += AI5_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[5].Error += AI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[6].Attach += AI6_Attach;
            //    ActiveInterfaceKit.VoltageInputs[6].Detach += AI6_Detach;
            //    ActiveInterfaceKit.VoltageInputs[6].PropertyChange += AI6_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[6].SensorChange += AI6_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[6].VoltageChange += AI6_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[6].Error += AI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[7].Attach += AI7_Attach;
            //    ActiveInterfaceKit.VoltageInputs[7].Detach += AI7_Detach;
            //    ActiveInterfaceKit.VoltageInputs[7].PropertyChange += AI7_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[7].SensorChange += AI7_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[7].VoltageChange += AI7_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[7].Error += AI7_Error;
            //}
        }

        private void ConfigureVoltageRatioInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Attach += AI0_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Detach += AI0_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].PropertyChange += AI0_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].SensorChange += AI0_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].VoltageRatioChange += AI0_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Error += AI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Attach += AI1_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Detach += AI1_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].PropertyChange += AI1_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].SensorChange += AI1_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].VoltageRatioChange += AI1_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Error += AI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Attach += AI2_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Detach += AI2_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].PropertyChange += AI2_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].SensorChange += AI2_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].VoltageRatioChange += AI2_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Error += AI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Attach += AI3_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Detach += AI3_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].PropertyChange += AI3_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].SensorChange += AI3_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].VoltageRatioChange += AI3_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Error += AI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Attach += AI4_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Detach += AI4_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].PropertyChange += AI4_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].SensorChange += AI4_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].VoltageRatioChange += AI4_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Error += AI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Attach += AI5_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Detach += AI5_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].PropertyChange += AI5_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].SensorChange += AI5_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].VoltageRatioChange += AI5_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Error += AI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Attach += AI6_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Detach += AI6_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].PropertyChange += AI6_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].SensorChange += AI6_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].VoltageRatioChange += AI6_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Error += AI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Attach += AI7_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Detach += AI7_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].PropertyChange += AI7_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].SensorChange += AI7_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].VoltageRatioChange += AI7_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Error += AI7_Error;
            //}
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

            for (int i = 0; i < DigitalOutputs.Count(); i++)
            {
                var dOut = DigitalOutputs[i];
                DigitalOutputs[i].Close();
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
