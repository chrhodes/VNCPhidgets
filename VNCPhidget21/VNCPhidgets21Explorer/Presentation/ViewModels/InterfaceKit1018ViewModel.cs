using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Phidgets;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgetConfig = VNCPhidget21.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
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
            // Or maybe a method on something else in VNCPhidget21.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            Message = "InterfaceKitViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig ? hostConfig = 
                JsonSerializer.Deserialize< VNCPhidgetConfig.HostConfig >
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();
            Sensors2 = hostConfig.Sensors.ToList();

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

        #endregion

        #region Phidget

        private Phidgets.Phidget _phidgetDevice;
        public Phidgets.Phidget PhidgetDevice
        {
            get => _phidgetDevice;
            set
            {
                if (_phidgetDevice == value)
                    return;
                _phidgetDevice = value;
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
        // May want to make this just a Phidgets.InterfaceKit to avoid all the ActiveInterfaceKit.InterfaceKit stuff

        private InterfaceKitEx _activeInterfaceKit;
        public InterfaceKitEx ActiveInterfaceKit
        {
            get => _activeInterfaceKit;
            set
            {
                if (_activeInterfaceKit == value)
                    return;
                _activeInterfaceKit = value;

                if (_activeInterfaceKit is not null)
                {
                    PhidgetDevice = _activeInterfaceKit.InterfaceKit;
                }
                else
                {
                    // TODO(crhodes)
                    // PhidgetDevice = null ???
                    // Will need to declare Phidgets.Phidget?
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

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

                if (ActiveInterfaceKit is not null)
                {
                    ActiveInterfaceKit.LogSensorChangeEvents = _logInputChangeEvents;
                }
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

                if (ActiveInterfaceKit is not null)
                {
                    ActiveInterfaceKit.LogOutputChangeEvents = _logOutputChangeEvents;
                }
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

                if (ActiveInterfaceKit is not null)
                {
                    ActiveInterfaceKit.LogSensorChangeEvents = _logSensorChangeEvents;
                }
            }
        }

        #region InterfaceKit Phidget Properties

        #region Sensor Input

        #region Analog Sensors

        #region Sensor A0

        private Int32? _aI0;
        public Int32? AI0
        {
            get => _aI0;
            set
            {
                if (_aI0 == value)
                    return;
                _aI0 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw0;
        public Int32? AIRaw0
        {
            get => _aIRaw0;
            set
            {
                if (_aIRaw0 == value)
                    return;
                _aIRaw0 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate0;
        public Int32 AIDataRate0
        {
            get => _aIDataRate0;
            set
            {
                if (_aIDataRate0 == value)
                    return;
                _aIDataRate0 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax0;
        public Int32 AIDataRateMax0
        {
            get => _aIDataRateMax0;
            set
            {
                if (_aIDataRateMax0 == value)
                    return;
                _aIDataRateMax0 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin0;
        public Int32 AIDataRateMin0
        {
            get => _aIDataRateMin0;
            set
            {
                if (_aIDataRateMin0 == value)
                    return;
                _aIDataRateMin0 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity0;
        public Int32 AISensitivity0
        {
            get => _aISensitivity0;
            set
            {             
                if (_aISensitivity0 == value)
                    return;
                _aISensitivity0 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[0].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[0].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A1

        private Int32? _aI1;
        public Int32? AI1
        {
            get => _aI1;
            set
            {
                if (_aI1 == value)
                    return;
                _aI1 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw1;
        public Int32? AIRaw1
        {
            get => _aIRaw1;
            set
            {
                if (_aIRaw1 == value)
                    return;
                _aIRaw1 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate1;
        public Int32 AIDataRate1
        {
            get => _aIDataRate1;
            set
            {
                if (_aIDataRate1 == value)
                    return;
                _aIDataRate1 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax1;
        public Int32 AIDataRateMax1
        {
            get => _aIDataRateMax1;
            set
            {
                if (_aIDataRateMax1 == value)
                    return;
                _aIDataRateMax1 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin1;
        public Int32 AIDataRateMin1
        {
            get => _aIDataRateMin1;
            set
            {
                if (_aIDataRateMin1 == value)
                    return;
                _aIDataRateMin1 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity1;
        public Int32 AISensitivity1
        {
            get => _aISensitivity1;
            set
            {
                if (_aISensitivity1 == value)
                    return;
                _aISensitivity1 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[1].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[1].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A2

        private Int32? _aI2;
        public Int32? AI2
        {
            get => _aI2;
            set
            {
                if (_aI2 == value)
                    return;
                _aI2 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw2;
        public Int32? AIRaw2
        {
            get => _aIRaw2;
            set
            {
                if (_aIRaw2 == value)
                    return;
                _aIRaw2 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate2;
        public Int32 AIDataRate2
        {
            get => _aIDataRate2;
            set
            {
                if (_aIDataRate2 == value)
                    return;
                _aIDataRate2 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax2;
        public Int32 AIDataRateMax2
        {
            get => _aIDataRateMax2;
            set
            {
                if (_aIDataRateMax2 == value)
                    return;
                _aIDataRateMax2 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin2;
        public Int32 AIDataRateMin2
        {
            get => _aIDataRateMin2;
            set
            {
                if (_aIDataRateMin2 == value)
                    return;
                _aIDataRateMin2 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity2;
        public Int32 AISensitivity2
        {
            get => _aISensitivity2;
            set
            {
                if (_aISensitivity2 == value)
                    return;
                _aISensitivity2 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[2].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[2].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A3

        private Int32? _aI3;
        public Int32? AI3
        {
            get => _aI3;
            set
            {
                if (_aI3 == value)
                    return;
                _aI3 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw3;
        public Int32? AIRaw3
        {
            get => _aIRaw3;
            set
            {
                if (_aIRaw3 == value)
                    return;
                _aIRaw3 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate3;
        public Int32 AIDataRate3
        {
            get => _aIDataRate3;
            set
            {
                if (_aIDataRate3 == value)
                    return;
                _aIDataRate3 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax3;
        public Int32 AIDataRateMax3
        {
            get => _aIDataRateMax3;
            set
            {
                if (_aIDataRateMax3 == value)
                    return;
                _aIDataRateMax3 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin3;
        public Int32 AIDataRateMin3
        {
            get => _aIDataRateMin3;
            set
            {
                if (_aIDataRateMin3 == value)
                    return;
                _aIDataRateMin3 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity3;
        public Int32 AISensitivity3
        {
            get => _aISensitivity3;
            set
            {
                if (_aISensitivity3 == value)
                    return;
                _aISensitivity3 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[3].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[3].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A4

        private Int32? _aI4;
        public Int32? AI4
        {
            get => _aI4;
            set
            {
                if (_aI4 == value)
                    return;
                _aI4 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw4;
        public Int32? AIRaw4
        {
            get => _aIRaw4;
            set
            {
                if (_aIRaw4 == value)
                    return;
                _aIRaw4 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate4;
        public Int32 AIDataRate4
        {
            get => _aIDataRate4;
            set
            {
                if (_aIDataRate4 == value)
                    return;
                _aIDataRate4 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax4;
        public Int32 AIDataRateMax4
        {
            get => _aIDataRateMax4;
            set
            {
                if (_aIDataRateMax4 == value)
                    return;
                _aIDataRateMax4 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin4;
        public Int32 AIDataRateMin4
        {
            get => _aIDataRateMin4;
            set
            {
                if (_aIDataRateMin4 == value)
                    return;
                _aIDataRateMin4 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity4;
        public Int32 AISensitivity4
        {
            get => _aISensitivity4;
            set
            {
                if (_aISensitivity4 == value)
                    return;
                _aISensitivity4 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[4].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[4].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A5

        private Int32? _aI5;
        public Int32? AI5
        {
            get => _aI5;
            set
            {
                if (_aI5 == value)
                    return;
                _aI5 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw5;
        public Int32? AIRaw5
        {
            get => _aIRaw5;
            set
            {
                if (_aIRaw5 == value)
                    return;
                _aIRaw5 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate5;
        public Int32 AIDataRate5
        {
            get => _aIDataRate5;
            set
            {
                if (_aIDataRate5 == value)
                    return;
                _aIDataRate5 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax5;
        public Int32 AIDataRateMax5
        {
            get => _aIDataRateMax5;
            set
            {
                if (_aIDataRateMax5 == value)
                    return;
                _aIDataRateMax5 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin5;
        public Int32 AIDataRateMin5
        {
            get => _aIDataRateMin5;
            set
            {
                if (_aIDataRateMin5 == value)
                    return;
                _aIDataRateMin5 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity5;
        public Int32 AISensitivity5
        {
            get => _aISensitivity5;
            set
            {
                if (_aISensitivity5 == value)
                    return;
                _aISensitivity5 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[5].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[5].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A6

        private Int32? _aI6;
        public Int32? AI6
        {
            get => _aI6;
            set
            {
                if (_aI6 == value)
                    return;
                _aI6 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw6;
        public Int32? AIRaw6
        {
            get => _aIRaw6;
            set
            {
                if (_aIRaw6 == value)
                    return;
                _aIRaw6 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate6;
        public Int32 AIDataRate6
        {
            get => _aIDataRate6;
            set
            {
                if (_aIDataRate6 == value)
                    return;
                _aIDataRate6 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax6;
        public Int32 AIDataRateMax6
        {
            get => _aIDataRateMax6;
            set
            {
                if (_aIDataRateMax6 == value)
                    return;
                _aIDataRateMax6 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin6;
        public Int32 AIDataRateMin6
        {
            get => _aIDataRateMin6;
            set
            {
                if (_aIDataRateMin6 == value)
                    return;
                _aIDataRateMin6 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity6;
        public Int32 AISensitivity6
        {
            get => _aISensitivity6;
            set
            {
                if (_aISensitivity6 == value)
                    return;
                _aISensitivity6 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[6].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[6].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Sensor A7

        private Int32? _aI7;
        public Int32? AI7
        {
            get => _aI7;
            set
            {
                if (_aI7 == value)
                    return;
                _aI7 = value;
                OnPropertyChanged();
            }
        }

        private Int32? _aIRaw7;
        public Int32? AIRaw7
        {
            get => _aIRaw7;
            set
            {
                if (_aIRaw7 == value)
                    return;
                _aIRaw7 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRate7;
        public Int32 AIDataRate7
        {
            get => _aIDataRate7;
            set
            {
                if (_aIDataRate7 == value)
                    return;
                _aIDataRate7 = value;

                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMax7;
        public Int32 AIDataRateMax7
        {
            get => _aIDataRateMax7;
            set
            {
                if (_aIDataRateMax7 == value)
                    return;
                _aIDataRateMax7 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aIDataRateMin7;
        public Int32 AIDataRateMin7
        {
            get => _aIDataRateMin7;
            set
            {
                if (_aIDataRateMin7 == value)
                    return;
                _aIDataRateMin7 = value;
                OnPropertyChanged();
            }
        }

        private Int32 _aISensitivity7;
        public Int32 AISensitivity7
        {
            get => _aISensitivity7;
            set
            {
                if (_aISensitivity7 == value)
                    return;
                _aISensitivity7 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.sensors[7].Sensitivity)
                {
                    ActiveInterfaceKit.InterfaceKit.sensors[7].Sensitivity = (Int32)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Digital Inputs

        private bool? _dI0;
        public bool? DI0
        {
            get => _dI0;
            set
            {
                if (_dI0 == value)
                    return;
                _dI0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI1;
        public bool? DI1
        {
            get => _dI1;
            set
            {
                if (_dI1 == value)
                    return;
                _dI1 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI2;
        public bool? DI2
        {
            get => _dI2;
            set
            {
                if (_dI2 == value)
                    return;
                _dI2 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI3;
        public bool? DI3
        {
            get => _dI3;
            set
            {
                if (_dI3 == value)
                    return;
                _dI3 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI4;
        public bool? DI4
        {
            get => _dI4;
            set
            {
                if (_dI4 == value)
                    return;
                _dI4 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI5;
        public bool? DI5
        {
            get => _dI5;
            set
            {
                if (_dI5 == value)
                    return;
                _dI5 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI6;
        public bool? DI6
        {
            get => _dI6;
            set
            {
                if (_dI6 == value)
                    return;
                _dI6 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI7;
        public bool? DI7
        {
            get => _dI7;
            set
            {
                if (_dI7 == value)
                    return;
                _dI7 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI8;
        public bool? DI8
        {
            get => _dI8;
            set
            {
                if (_dI8 == value)
                    return;
                _dI0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI9;
        public bool? DI9
        {
            get => _dI9;
            set
            {
                if (_dI9 == value)
                    return;
                _dI9 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI10;
        public bool? DI10
        {
            get => _dI10;
            set
            {
                if (_dI10 == value)
                    return;
                _dI10 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI11;
        public bool? DI11
        {
            get => _dI11;
            set
            {
                if (_dI11 == value)
                    return;
                _dI11 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI12;
        public bool? DI12
        {
            get => _dI12;
            set
            {
                if (_dI12 == value)
                    return;
                _dI12 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI13;
        public bool? DI13
        {
            get => _dI13;
            set
            {
                if (_dI13 == value)
                    return;
                _dI13 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI14;
        public bool? DI14
        {
            get => _dI14;
            set
            {
                if (_dI14 == value)
                    return;
                _dI14 = value;
                OnPropertyChanged();
            }
        }

        private bool? _dI15;
        public bool? DI15
        {
            get => _dI15;
            set
            {
                if (_dI15 == value)
                    return;
                _dI15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Digital Outputs

        private bool? _dO0;
        public bool? DO0
        {
            get => _dO0;
            set
            {
                if (_dO0 == value)
                    return;
                _dO0 = value;

                // ActiveInterfaceKit_OutputChange may have called us
                // No need to update if same state.

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[0])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[0] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO1;
        public bool? DO1
        {
            get => _dO1;
            set
            {
                if (_dO1 == value)
                    return;
                _dO1 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[1])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[1] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO2;
        public bool? DO2
        {
            get => _dO2;
            set
            {
                if (_dO2 == value)
                    return;
                _dO2 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[2])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[2] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO3;
        public bool? DO3
        {
            get => _dO3;
            set
            {
                if (_dO3 == value)
                    return;
                _dO3 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[3])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[3] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO4;
        public bool? DO4
        {
            get => _dO4;
            set
            {
                if (_dO4 == value)
                    return;
                _dO4 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[4])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[4] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO5;
        public bool? DO5
        {
            get => _dO5;
            set
            {
                if (_dO5 == value)
                    return;
                _dO5 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[5])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[5] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO6;
        public bool? DO6
        {
            get => _dO6;
            set
            {
                if (_dO6 == value)
                    return;
                _dO6 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[6])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[6] = (Boolean)value;
                }
                OnPropertyChanged();
            }
        }

        private bool? _dO7;
        public bool? DO7
        {
            get => _dO7;
            set
            {
                if (_dO7 == value)
                    return;
                _dO7 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[7])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[7] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO8;
        public bool? DO8
        {
            get => _dO8;
            set
            {
                if (_dO8 == value)
                    return;
                _dO0 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[8])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[8] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO9;
        public bool? DO9
        {
            get => _dO9;
            set
            {
                if (_dO9 == value)
                    return;
                _dO9 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[9])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[9] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO10;
        public bool? DO10
        {
            get => _dO10;
            set
            {
                if (_dO10 == value)
                    return;
                _dO10 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[10])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[10] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO11;
        public bool? DO11
        {
            get => _dO11;
            set
            {
                if (_dO11 == value)
                    return;
                _dO11 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[11])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[11] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO12;
        public bool? DO12
        {
            get => _dO12;
            set
            {
                if (_dO12 == value)
                    return;
                _dO12 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[12])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[12] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO13;
        public bool? DO13
        {
            get => _dO13;
            set
            {
                if (_dO13 == value)
                    return;
                _dO13 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[13])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[13] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO14;
        public bool? DO14
        {
            get => _dO14;
            set
            {
                if (_dO14 == value)
                    return;
                _dO14 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[14])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[14] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        private bool? _dO15;
        public bool? DO15
        {
            get => _dO15;
            set
            {
                if (_dO15 == value)
                    return;
                _dO15 = value;

                if (ActiveInterfaceKit is not null
                    && value != ActiveInterfaceKit.InterfaceKit.outputs[15])
                {
                    ActiveInterfaceKit.InterfaceKit.outputs[15] = (Boolean)value;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        private IEnumerable<VNCPhidgetConfig.Sensor> _Sensors2;
        public IEnumerable<VNCPhidgetConfig.Sensor> Sensors2
        {
            get
            {
                if (null == _Sensors2)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Sensors =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Sensors").Elements("Sensor")
                    //    select new Sensor(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Sensors2;
            }

            set
            {
                _Sensors2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        private void ActiveInterfaceKit_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveInterfaceKit_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = device.Attached;

                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateInterfaceKitProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void ActiveInterfaceKit_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveInterfaceKit_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = device.Attached;

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveInterfaceKit to null.  Clear UI
                UpdateInterfaceKitProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void ActiveInterfaceKit_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;

            InterfaceKitAnalogSensor sensor = ifk.sensors[0];

            //SIRaw0 = sensor.RawValue;
            //SIDataRate0 = sensor.DataRate;
            //SIDataRateMax0 = sensor.DataRateMax;
            //SIDataRateMin0 = sensor.DataRateMin;
            //SISensitivity0= sensor.Sensitivity;

            //var sValue = sensor0.Value;
            //var eValue = e.Value;

            // NOTE(crhodes)
            // DataRateMin and DataRateMax do not change.
            // Populate in Attach event

            switch (e.Index)
            {
                case 0:
                    sensor = ifk.sensors[0];
                    AI0 = sensor.Value;
                    AIRaw0 = sensor.RawValue;
                    AIDataRate0 = sensor.DataRate;
                    //AIDataRateMax0 = sensor.DataRateMax;
                    //AIDataRateMin0 = sensor.DataRateMin;
                    AISensitivity0 = sensor.Sensitivity;
                    break;
                case 1:
                    sensor = ifk.sensors[1];
                    AI1 = sensor.Value;
                    AIRaw1 = sensor.RawValue;
                    AIDataRate1 = sensor.DataRate;
                    //AIDataRateMax1 = sensor.DataRateMax;
                    //AIDataRateMin1 = sensor.DataRateMin;
                    AISensitivity1 = sensor.Sensitivity;
                    break;
                case 2:
                    sensor = ifk.sensors[2];
                    AI2 = sensor.Value;
                    AIRaw2 = sensor.RawValue;
                    AIDataRate2 = sensor.DataRate;
                    //AIDataRateMax2 = sensor.DataRateMax;
                    //AIDataRateMin2 = sensor.DataRateMin;
                    AISensitivity2 = sensor.Sensitivity;
                    break;
                case 3:
                    sensor = ifk.sensors[3];
                    AI3 = sensor.Value;
                    AIRaw3 = sensor.RawValue;
                    AIDataRate3 = sensor.DataRate;
                    //AIDataRateMax3 = sensor.DataRateMax;
                    //AIDataRateMin3 = sensor.DataRateMin;
                    AISensitivity3 = sensor.Sensitivity;
                    break;
                case 4:
                    sensor = ifk.sensors[4];
                    AI4 = sensor.Value;
                    AIRaw4 = sensor.RawValue;
                    AIDataRate4 = sensor.DataRate;
                    //AIDataRateMax4 = sensor.DataRateMax;
                    //AIDataRateMin4 = sensor.DataRateMin;
                    AISensitivity4 = sensor.Sensitivity;
                    break;
                case 5:
                    sensor = ifk.sensors[5];
                    AI5 = sensor.Value;
                    AIRaw5 = sensor.RawValue;
                    AIDataRate5 = sensor.DataRate;
                    //AIDataRateMax5 = sensor.DataRateMax;
                    //AIDataRateMin5 = sensor.DataRateMin;
                    AISensitivity5 = sensor.Sensitivity;
                    break;
                case 6:
                    sensor = ifk.sensors[6];
                    AI6 = sensor.Value;
                    AIRaw6 = sensor.RawValue;
                    AIDataRate6 = sensor.DataRate;
                    //AIDataRateMax6 = sensor.DataRateMax;
                    //AIDataRateMin6 = sensor.DataRateMin;
                    AISensitivity6 = sensor.Sensitivity;
                    break;
                case 7:
                    sensor = ifk.sensors[7];
                    AI7 = sensor.Value;
                    AIRaw7 = sensor.RawValue;
                    AIDataRate7 = sensor.DataRate;
                    //AIDataRateMax7 = sensor.DataRateMax;
                    //AIDataRateMin7 = sensor.DataRateMin;
                    AISensitivity7 = sensor.Sensitivity;
                    break;
            }
        }

        private void ActiveInterfaceKit_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;

            switch (e.Index)
            {
                case 0:
                    DI0 = e.Value;
                    break;
                case 1:
                    DI1 = e.Value;
                    break;
                case 2:
                    DI2 = e.Value;
                    break;
                case 3:
                    DI3 = e.Value;
                    break;
                case 4:
                    DI4 = e.Value;
                    break;
                case 5:
                    DI5 = e.Value;
                    break;
                case 6:
                    DI6 = e.Value;
                    break;
                case 7:
                    DI7 = e.Value;
                    break;
                case 8:
                    DI8 = e.Value;
                    break;
                case 9:
                    DI9 = e.Value;
                    break;
                case 10:
                    DI10 = e.Value;
                    break;
                case 11:
                    DI11 = e.Value;
                    break;
                case 12:
                    DI12 = e.Value;
                    break;
                case 13:
                    DI13 = e.Value;
                    break;
                case 14:
                    DI14 = e.Value;
                    break;
                case 15:
                    DI15 = e.Value;
                    break;
            }
        }

        private void ActiveInterfaceKit_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            Phidgets.InterfaceKit ifk = (Phidgets.InterfaceKit)sender;
            var outputs = ifk.outputs;
            InterfaceKitDigitalOutputCollection doc = outputs;

            switch (e.Index)
            {
                case 0:
                    DO0 = e.Value;
                    break;
                case 1:
                    DO1 = e.Value;
                    break;
                case 2:
                    DO2 = e.Value;
                    break;
                case 3:
                    DO3 = e.Value;
                    break;
                case 4:
                    DO4 = e.Value;
                    break;
                case 5:
                    DO5 = e.Value;
                    break;
                case 6:
                    DO6 = e.Value;
                    break;
                case 7:
                    DO7 = e.Value;
                    break;
                case 8:
                    DO8 = e.Value;
                    break;
                case 9:
                    DO9 = e.Value;
                    break;
                case 10:
                    DO10 = e.Value;
                    break;
                case 11:
                    DO11 = e.Value;
                    break;
                case 12:
                    DO12 = e.Value;
                    break;
                case 13:
                    DO13 = e.Value;
                    break;
                case 14:
                    DO14 = e.Value;
                    break;
                case 15:
                    DO15 = e.Value;
                    break;
            }

        }



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

            
            ActiveInterfaceKit = new InterfaceKitEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedInterfaceKit.SerialNumber,
                SelectedInterfaceKit.Embedded, 
                EventAggregator);

            ActiveInterfaceKit.InterfaceKit.Attach += ActiveInterfaceKit_Attach;
            ActiveInterfaceKit.InterfaceKit.Detach += ActiveInterfaceKit_Detach;

            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The InterfaceKitEx attaches to these events also.
            // It logs the changes if Log{Input,Output,Sensor}ChangeEvents are set to true.

            ActiveInterfaceKit.InterfaceKit.OutputChange += ActiveInterfaceKit_OutputChange;
            ActiveInterfaceKit.InterfaceKit.InputChange += ActiveInterfaceKit_InputChange;

            // NOTE(crhodes)
            // Let's do see if we can watch some analog data stream in.

            ActiveInterfaceKit.InterfaceKit.SensorChange += ActiveInterfaceKit_SensorChange;

            ActiveInterfaceKit.LogPhidgetEvents = LogPhidgetEvents;

            ActiveInterfaceKit.LogInputChangeEvents = LogInputChangeEvents;
            ActiveInterfaceKit.LogOutputChangeEvents = LogOutputChangeEvents;
            ActiveInterfaceKit.LogSensorChangeEvents = LogSensorChangeEvents;

            await Task.Run(() => ActiveInterfaceKit.Open());

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

        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedInterfaceKit is not null)
            {
                if (DeviceAttached is not null)
                    return !(Boolean)DeviceAttached;
                else
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

            ActiveInterfaceKit.InterfaceKit.Attach -= ActiveInterfaceKit_Attach;
            ActiveInterfaceKit.InterfaceKit.Detach -= ActiveInterfaceKit_Detach;

            ActiveInterfaceKit.Close();
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
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SayHello) Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SayHello) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private bool SayHelloCanExecute()
        {
            return true;
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
            DI0 = DO0 = null;
            DI1 = DO1 = null;
            DI2 = DO2 = null;
            DI3 = DO3 = null;
            DI4 = DO4 = null;
            DI5 = DO5 = null;
            DI6 = DO6 = null;
            DI7 = DO7 = null;
            DI8 = DO8 = null;
            DI9 = DO9 = null;
            DI10 = DO10 = null;
            DI11 = DO11 = null;
            DI12 = DO12 = null;
            DI13 = DO13 = null;
            DI14 = DO14 = null;
            DI15 = DO15 = null;
        }

        private void PopulateSensorValues(InterfaceKitAnalogSensor interfaceKitAnalogSensor)
        {

        }

        private void UpdateInterfaceKitProperties()
        {

            if (ActiveInterfaceKit.InterfaceKit.Attached)
            {
                //IkAddress = ActiveInterfaceKit.InterfaceKit.Address;
                //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
                DeviceAttached = ActiveInterfaceKit.InterfaceKit.Attached;
                //IkAttachedToServer = ActiveInterfaceKit.InterfaceKit.AttachedToServer;
                //IkClass = ActiveInterfaceKit.InterfaceKit.Class.ToString();
                //IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.InterfaceKit.ID);
                //IkLabel = ActiveInterfaceKit.InterfaceKit.Label;
                //IkLibraryVersion = Phidget.LibraryVersion;  // This is a static field
                //IkName = ActiveInterfaceKit.InterfaceKit.Name;
                //IkPort = ActiveInterfaceKit.InterfaceKit.Port;
                //IkSerialNumber = ActiveInterfaceKit.InterfaceKit.SerialNumber; // This throws exception
                ////IkServerID = ActiveInterfaceKit.ServerID;
                //IkType = ActiveInterfaceKit.InterfaceKit.Type;
                //IkVersion = ActiveInterfaceKit.InterfaceKit.Version;

                var sensors = ActiveInterfaceKit.InterfaceKit.sensors;
                InterfaceKitAnalogSensor sensor = null;

                // NOTE(crhodes)
                // The DataRateMin and DataRateMax do not change.
                // Populate them here instead of SensorChange event

                // TODO(crhodes)
                // May want to grab initial values for all fields here.

                for (int i = 0; i < sensors.Count; i++)
                {
                    sensor = sensors[i];

                    switch (i)
                    {
                        case 0:
                            AIDataRateMax0 = sensor.DataRateMax;
                            AIDataRateMin0 = sensor.DataRateMin;
                            break;
                        case 1:
                            AIDataRateMax1 = sensor.DataRateMax;
                            AIDataRateMin1 = sensor.DataRateMin;
                            break;
                        case 2:
                            AIDataRateMax2 = sensor.DataRateMax;
                            AIDataRateMin2 = sensor.DataRateMin;
                            break;
                        case 3:
                            AIDataRateMax3 = sensor.DataRateMax;
                            AIDataRateMin3 = sensor.DataRateMin;
                            break;
                        case 4:
                            AIDataRateMax4 = sensor.DataRateMax;
                            AIDataRateMin4 = sensor.DataRateMin;
                            break;
                        case 5:
                            AIDataRateMax5 = sensor.DataRateMax;
                            AIDataRateMin5 = sensor.DataRateMin;
                            break;
                        case 6:
                            AIDataRateMax6 = sensor.DataRateMax;
                            AIDataRateMin6 = sensor.DataRateMin;
                            break;
                        case 7:
                            AIDataRateMax7 = sensor.DataRateMax;
                            AIDataRateMin7 = sensor.DataRateMin;
                            break;
                    }
                }
            }
            else
            {
                DeviceAttached = null;
                // NOTE(crhodes)
                // Commented out properties throw exceptions when Phidget not attached
                // Just clear field

                //IkAddress = ActiveInterfaceKit.Address;
                //IkAddress = "";
                //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
                ////IkAttachedToServer = ActiveInterfaceKit.AttachedToServer;
                //IkAttachedToServer = false;
                //// This doesn't throw exception but let's clear anyway
                ////IkClass = ActiveInterfaceKit.Class.ToString();
                //IkClass = "";
                ////IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.ID);
                //IkID = "";
                ////IkLabel = ActiveInterfaceKit.Label;
                //IkLabel = "";
                ////IkLibraryVersion = ActiveInterfaceKit.LibraryVersion;
                //IkLibraryVersion = Phidget.LibraryVersion;
                ////IkName = ActiveInterfaceKit.Name;
                //IkName = "";
                ////IkSerialNumber = ActiveInterfaceKit.SerialNumber;
                //IkSerialNumber = null;
                ////IkServerID = ActiveInterfaceKit.ServerID;
                //IkServerID = "";
                ////IkType = ActiveInterfaceKit.Type;
                //IkType = "";
                ////IkVersion = ActiveInterfaceKit.Version;
                //IkVersion = null;
            }

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
