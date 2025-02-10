using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;
using System.Threading.Channels;
//using Phidget22;

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

            OpenDigitalInputCommand = new DelegateCommand<string>(OpenDigitalInput, OpenDigitalInputCanExecute);
            CloseDigitalInputCommand = new DelegateCommand<string>(CloseDigitalInput, CloseDigitalInputCanExecute);

            OpenDigitalOutputCommand = new DelegateCommand<string>(OpenDigitalOutput, OpenDigitalOutputCanExecute);
            CloseDigitalOutputCommand = new DelegateCommand<string>(CloseDigitalOutput, CloseDigitalOutputCanExecute);

            OpenVoltageInputCommand = new DelegateCommand<string>(OpenVoltageInput, OpenVoltageInputCanExecute);
            CloseVoltageInputCommand = new DelegateCommand<string>(CloseVoltageInput, CloseVoltageInputCanExecute);

            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNC.Phidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            //CreateChannels();

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

        private bool _logPhidgetEvents = true;
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

        private bool _logErrorEvents = true;    // Probably always want to see errors
        public bool LogErrorEvents
        {
            get => _logErrorEvents;
            set
            {
                if (_logErrorEvents == value)
                    return;
                _logErrorEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _logPropertyChangeEvents = false;
        public bool LogPropertyChangeEvents
        {
            get => _logPropertyChangeEvents;
            set
            {
                if (_logPropertyChangeEvents == value)
                    return;
                _logPropertyChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _logStateChangeEvents = false;
        public bool LogStateChangeEvents
        {
            get => _logStateChangeEvents;
            set
            {
                if (_logStateChangeEvents == value)
                    return;
                _logStateChangeEvents = value;
                OnPropertyChanged();
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
            }
        }

        private bool _logVoltageChangeEvents = false;
        public bool LogVoltageChangeEvents
        {
            get => _logVoltageChangeEvents;
            set
            {
                if (_logVoltageChangeEvents == value)
                    return;
                _logVoltageChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _logPerformanceSequence = false;
        public bool LogPerformanceSequence
        {
            get => _logPerformanceSequence;
            set
            {
                if (_logPerformanceSequence == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
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

        private bool _logActionVerification = false;
        public bool LogActionVerification
        {
            get => _logActionVerification;
            set
            {
                if (_logActionVerification == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
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

        #region DigitalInput

        //DigitalInputEx[] _digitalInputs = new DigitalInputEx[16];

        //public DigitalInputEx[] DigitalInputs
        //{
        //    get
        //    {
        //        return _digitalInputs;
        //    }
        //    set
        //    {
        //        _digitalInputs = value;
        //        OnPropertyChanged();
        //    }
        //}


        private DigitalInputEx _digitalInput0;
        public DigitalInputEx DigitalInput0
        {
            get => _digitalInput0;
            set
            {
                if (_digitalInput0 == value)
                    return;
                _digitalInput0 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput1;
        public DigitalInputEx DigitalInput1
        {
            get => _digitalInput1;
            set
            {
                if (_digitalInput1 == value)
                    return;
                _digitalInput1 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput2;
        public DigitalInputEx DigitalInput2
        {
            get => _digitalInput2;
            set
            {
                if (_digitalInput2 == value)
                    return;
                _digitalInput2 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput4;
        public DigitalInputEx DigitalInput4
        {
            get => _digitalInput4;
            set
            {
                if (_digitalInput4 == value)
                    return;
                _digitalInput4 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput5;
        public DigitalInputEx DigitalInput5
        {
            get => _digitalInput5;
            set
            {
                if (_digitalInput5 == value)
                    return;
                _digitalInput5 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput6;
        public DigitalInputEx DigitalInput6
        {
            get => _digitalInput6;
            set
            {
                if (_digitalInput6 == value)
                    return;
                _digitalInput6 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput7;
        public DigitalInputEx DigitalInput7
        {
            get => _digitalInput7;
            set
            {
                if (_digitalInput7 == value)
                    return;
                _digitalInput7 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput8;
        public DigitalInputEx DigitalInput8
        {
            get => _digitalInput8;
            set
            {
                if (_digitalInput8 == value)
                    return;
                _digitalInput8 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput9;
        public DigitalInputEx DigitalInput9
        {
            get => _digitalInput9;
            set
            {
                if (_digitalInput9 == value)
                    return;
                _digitalInput9 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput10;
        public DigitalInputEx DigitalInput10
        {
            get => _digitalInput10;
            set
            {
                if (_digitalInput10 == value)
                    return;
                _digitalInput10 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput11;
        public DigitalInputEx DigitalInput11
        {
            get => _digitalInput11;
            set
            {
                if (_digitalInput11 == value)
                    return;
                _digitalInput11 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput12;
        public DigitalInputEx DigitalInput12
        {
            get => _digitalInput12;
            set
            {
                if (_digitalInput12 == value)
                    return;
                _digitalInput12 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput13;
        public DigitalInputEx DigitalInput13
        {
            get => _digitalInput13;
            set
            {
                if (_digitalInput13 == value)
                    return;
                _digitalInput13 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput14;
        public DigitalInputEx DigitalInput14
        {
            get => _digitalInput14;
            set
            {
                if (_digitalInput14 == value)
                    return;
                _digitalInput14 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput15;
        public DigitalInputEx DigitalInput15
        {
            get => _digitalInput15;
            set
            {
                if (_digitalInput15 == value)
                    return;
                _digitalInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DigitalOutput

        private DigitalOutputEx _digitalOutput0;
        public DigitalOutputEx DigitalOutput0
        {
            get => _digitalOutput0;
            set
            {
                if (_digitalOutput0 == value)
                    return;
                _digitalOutput0 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput1;
        public DigitalOutputEx DigitalOutput1
        {
            get => _digitalOutput1;
            set
            {
                if (_digitalOutput1 == value)
                    return;
                _digitalOutput1 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput2;
        public DigitalOutputEx DigitalOutput2
        {
            get => _digitalOutput2;
            set
            {
                if (_digitalOutput2 == value)
                    return;
                _digitalOutput2 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput3;
        public DigitalOutputEx DigitalOutput3
        {
            get => _digitalOutput3;
            set
            {
                if (_digitalOutput3 == value)
                    return;
                _digitalOutput2 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput4;
        public DigitalOutputEx DigitalOutput4
        {
            get => _digitalOutput4;
            set
            {
                if (_digitalOutput4 == value)
                    return;
                _digitalOutput4 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput5;
        public DigitalOutputEx DigitalOutput5
        {
            get => _digitalOutput5;
            set
            {
                if (_digitalOutput5 == value)
                    return;
                _digitalOutput5 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput6;
        public DigitalOutputEx DigitalOutput6
        {
            get => _digitalOutput6;
            set
            {
                if (_digitalOutput6 == value)
                    return;
                _digitalOutput6 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput7;
        public DigitalOutputEx DigitalOutput7
        {
            get => _digitalOutput7;
            set
            {
                if (_digitalOutput7 == value)
                    return;
                _digitalOutput7 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput8;
        public DigitalOutputEx DigitalOutput8
        {
            get => _digitalOutput8;
            set
            {
                if (_digitalOutput8 == value)
                    return;
                _digitalOutput8 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput9;
        public DigitalOutputEx DigitalOutput9
        {
            get => _digitalOutput9;
            set
            {
                if (_digitalOutput9 == value)
                    return;
                _digitalOutput9 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput10;
        public DigitalOutputEx DigitalOutput10
        {
            get => _digitalOutput10;
            set
            {
                if (_digitalOutput10 == value)
                    return;
                _digitalOutput10 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput11;
        public DigitalOutputEx DigitalOutput11
        {
            get => _digitalOutput11;
            set
            {
                if (_digitalOutput11 == value)
                    return;
                _digitalOutput11 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput12;
        public DigitalOutputEx DigitalOutput12
        {
            get => _digitalOutput12;
            set
            {
                if (_digitalOutput12 == value)
                    return;
                _digitalOutput12 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput13;
        public DigitalOutputEx DigitalOutput13
        {
            get => _digitalOutput13;
            set
            {
                if (_digitalOutput13 == value)
                    return;
                _digitalOutput13 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput14;
        public DigitalOutputEx DigitalOutput14
        {
            get => _digitalOutput14;
            set
            {
                if (_digitalOutput14 == value)
                    return;
                _digitalOutput14 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput15;
        public DigitalOutputEx DigitalOutput15
        {
            get => _digitalOutput15;
            set
            {
                if (_digitalOutput15 == value)
                    return;
                _digitalOutput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region VoltageInput

        private VoltageInputEx _voltageInput0;
        public VoltageInputEx VoltageInput0
        {
            get => _voltageInput0;
            set
            {
                if (_voltageInput0 == value)
                    return;
                _voltageInput0 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput1;
        public VoltageInputEx VoltageInput1
        {
            get => _voltageInput1;
            set
            {
                if (_voltageInput1 == value)
                    return;
                _voltageInput1 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput2;
        public VoltageInputEx VoltageInput2
        {
            get => _voltageInput2;
            set
            {
                if (_voltageInput2 == value)
                    return;
                _voltageInput2 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput4;
        public VoltageInputEx VoltageInput4
        {
            get => _voltageInput4;
            set
            {
                if (_voltageInput4 == value)
                    return;
                _voltageInput4 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput5;
        public VoltageInputEx VoltageInput5
        {
            get => _voltageInput5;
            set
            {
                if (_voltageInput5 == value)
                    return;
                _voltageInput5 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput6;
        public VoltageInputEx VoltageInput6
        {
            get => _voltageInput6;
            set
            {
                if (_voltageInput6 == value)
                    return;
                _voltageInput6 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput7;
        public VoltageInputEx VoltageInput7
        {
            get => _voltageInput7;
            set
            {
                if (_voltageInput7 == value)
                    return;
                _voltageInput7 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput8;
        public VoltageInputEx VoltageInput8
        {
            get => _voltageInput8;
            set
            {
                if (_voltageInput8 == value)
                    return;
                _voltageInput8 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput9;
        public VoltageInputEx VoltageInput9
        {
            get => _voltageInput9;
            set
            {
                if (_voltageInput9 == value)
                    return;
                _voltageInput9 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput10;
        public VoltageInputEx VoltageInput10
        {
            get => _voltageInput10;
            set
            {
                if (_voltageInput10 == value)
                    return;
                _voltageInput10 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput11;
        public VoltageInputEx VoltageInput11
        {
            get => _voltageInput11;
            set
            {
                if (_voltageInput11 == value)
                    return;
                _voltageInput11 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput12;
        public VoltageInputEx VoltageInput12
        {
            get => _voltageInput12;
            set
            {
                if (_voltageInput12 == value)
                    return;
                _voltageInput12 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput13;
        public VoltageInputEx VoltageInput13
        {
            get => _voltageInput13;
            set
            {
                if (_voltageInput13 == value)
                    return;
                _voltageInput13 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput14;
        public VoltageInputEx VoltageInput14
        {
            get => _voltageInput14;
            set
            {
                if (_voltageInput14 == value)
                    return;
                _voltageInput14 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput15;
        public VoltageInputEx VoltageInput15
        {
            get => _voltageInput15;
            set
            {
                if (_voltageInput15 == value)
                    return;
                _voltageInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region VoltageRatioInput

        private VoltageRatioInputEx _voltageRatioInput0;
        public VoltageRatioInputEx VoltageRatioInput0
        {
            get => _voltageRatioInput0;
            set
            {
                if (_voltageRatioInput0 == value)
                    return;
                _voltageRatioInput0 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput1;
        public VoltageRatioInputEx VoltageRatioInput1
        {
            get => _voltageRatioInput1;
            set
            {
                if (_voltageRatioInput1 == value)
                    return;
                _voltageRatioInput1 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput2;
        public VoltageRatioInputEx VoltageRatioInput2
        {
            get => _voltageRatioInput2;
            set
            {
                if (_voltageRatioInput2 == value)
                    return;
                _voltageRatioInput2 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput4;
        public VoltageRatioInputEx VoltageRatioInput4
        {
            get => _voltageRatioInput4;
            set
            {
                if (_voltageRatioInput4 == value)
                    return;
                _voltageRatioInput4 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput5;
        public VoltageRatioInputEx VoltageRatioInput5
        {
            get => _voltageRatioInput5;
            set
            {
                if (_voltageRatioInput5 == value)
                    return;
                _voltageRatioInput5 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput6;
        public VoltageRatioInputEx VoltageRatioInput6
        {
            get => _voltageRatioInput6;
            set
            {
                if (_voltageRatioInput6 == value)
                    return;
                _voltageRatioInput6 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput7;
        public VoltageRatioInputEx VoltageRatioInput7
        {
            get => _voltageRatioInput7;
            set
            {
                if (_voltageRatioInput7 == value)
                    return;
                _voltageRatioInput7 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput8;
        public VoltageRatioInputEx VoltageRatioInput8
        {
            get => _voltageRatioInput8;
            set
            {
                if (_voltageRatioInput8 == value)
                    return;
                _voltageRatioInput8 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput9;
        public VoltageRatioInputEx VoltageRatioInput9
        {
            get => _voltageRatioInput9;
            set
            {
                if (_voltageRatioInput9 == value)
                    return;
                _voltageRatioInput9 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput10;
        public VoltageRatioInputEx VoltageRatioInput10
        {
            get => _voltageRatioInput10;
            set
            {
                if (_voltageRatioInput10 == value)
                    return;
                _voltageRatioInput10 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput11;
        public VoltageRatioInputEx VoltageRatioInput11
        {
            get => _voltageRatioInput11;
            set
            {
                if (_voltageRatioInput11 == value)
                    return;
                _voltageRatioInput11 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput12;
        public VoltageRatioInputEx VoltageRatioInput12
        {
            get => _voltageRatioInput12;
            set
            {
                if (_voltageRatioInput12 == value)
                    return;
                _voltageRatioInput12 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput13;
        public VoltageRatioInputEx VoltageRatioInput13
        {
            get => _voltageRatioInput13;
            set
            {
                if (_voltageRatioInput13 == value)
                    return;
                _voltageRatioInput13 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput14;
        public VoltageRatioInputEx VoltageRatioInput14
        {
            get => _voltageRatioInput14;
            set
            {
                if (_voltageRatioInput14 == value)
                    return;
                _voltageRatioInput14 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput15;
        public VoltageRatioInputEx VoltageRatioInput15
        {
            get => _voltageRatioInput15;
            set
            {
                if (_voltageRatioInput15 == value)
                    return;
                _voltageRatioInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region VoltageOutput

        private VoltageOutputEx _voltageOutput0;
        public VoltageOutputEx VoltageOutput0
        {
            get => _voltageOutput0;
            set
            {
                if (_voltageOutput0 == value)
                    return;
                _voltageOutput0 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput1;
        public VoltageOutputEx VoltageOutput1
        {
            get => _voltageOutput1;
            set
            {
                if (_voltageOutput1 == value)
                    return;
                _voltageOutput1 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput2;
        public VoltageOutputEx VoltageOutput2
        {
            get => _voltageOutput2;
            set
            {
                if (_voltageOutput2 == value)
                    return;
                _voltageOutput2 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput4;
        public VoltageOutputEx VoltageOutput4
        {
            get => _voltageOutput4;
            set
            {
                if (_voltageOutput4 == value)
                    return;
                _voltageOutput4 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput5;
        public VoltageOutputEx VoltageOutput5
        {
            get => _voltageOutput5;
            set
            {
                if (_voltageOutput5 == value)
                    return;
                _voltageOutput5 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput6;
        public VoltageOutputEx VoltageOutput6
        {
            get => _voltageOutput6;
            set
            {
                if (_voltageOutput6 == value)
                    return;
                _voltageOutput6 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput7;
        public VoltageOutputEx VoltageOutput7
        {
            get => _voltageOutput7;
            set
            {
                if (_voltageOutput7 == value)
                    return;
                _voltageOutput7 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput8;
        public VoltageOutputEx VoltageOutput8
        {
            get => _voltageOutput8;
            set
            {
                if (_voltageOutput8 == value)
                    return;
                _voltageOutput8 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput9;
        public VoltageOutputEx VoltageOutput9
        {
            get => _voltageOutput9;
            set
            {
                if (_voltageOutput9 == value)
                    return;
                _voltageOutput9 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput10;
        public VoltageOutputEx VoltageOutput10
        {
            get => _voltageOutput10;
            set
            {
                if (_voltageOutput10 == value)
                    return;
                _voltageOutput10 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput11;
        public VoltageOutputEx VoltageOutput11
        {
            get => _voltageOutput11;
            set
            {
                if (_voltageOutput11 == value)
                    return;
                _voltageOutput11 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput12;
        public VoltageOutputEx VoltageOutput12
        {
            get => _voltageOutput12;
            set
            {
                if (_voltageOutput12 == value)
                    return;
                _voltageOutput12 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput13;
        public VoltageOutputEx VoltageOutput13
        {
            get => _voltageOutput13;
            set
            {
                if (_voltageOutput13 == value)
                    return;
                _voltageOutput13 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput14;
        public VoltageOutputEx VoltageOutput14
        {
            get => _voltageOutput14;
            set
            {
                if (_voltageOutput14 == value)
                    return;
                _voltageOutput14 = value;
                OnPropertyChanged();
            }
        }

        private VoltageOutputEx _voltageOutput15;
        public VoltageOutputEx VoltageOutput15
        {
            get => _voltageOutput15;
            set
            {
                if (_voltageOutput15 == value)
                    return;
                _voltageOutput15 = value;
                OnPropertyChanged();
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

                OpenDigitalInputCommand.RaiseCanExecuteChanged();
                OpenDigitalOutputCommand.RaiseCanExecuteChanged();

                OpenVoltageInputCommand.RaiseCanExecuteChanged();
                //OpenVoltageOutputCommand.RaiseCanExecuteChanged();

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
        // May want to make this just a Phidget22.InterfaceKit
        // to avoid all the ActiveInterfaceKit.InterfaceKit stuff

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

        #region Event Handlers (none)



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
                // If do not specify a timeout, Open() may return
                // before initial state is available in Attach event

                //await Task.Run(() => DigitalInputs[i].Open());
                //await Task.Run(() => DigitalOutputs[i].Open(500));
            }

            ConfigureDigitalOutputs(deviceChannels.DigitalOutputCount, serialNumber);

            for (int i = 0; i < deviceChannels.DigitalOutputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available
                
                //DigitalOutputs[i].Open();
                //await Task.Run(() => DigitalOutputs[i].Open(500));
            }

            ConfigureVoltageInputs(deviceChannels.VoltageInputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageInputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available
                
                //VoltageInputs[i].Open();
                //await Task.Run(() => VoltageOutputs[i].Open(500));
            }

            ConfigureVoltageRatioInputs(deviceChannels.VoltageRatioInputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageRatioInputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                //VoltageRatioInputs[i].Open();
                //await Task.Run(() => VoltageOutputs[i].Open(500));
            }

            ConfigureVoltageOutputs(deviceChannels.VoltageOutputCount, serialNumber);

            for (int i = 0; i < deviceChannels.VoltageOutputCount; i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                //VoltageOutputs[i].Open();
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

            //for (int i = 0; i < DigitalInputs.Count(); i++)
            //{
            //    DigitalInputs[i].Close();
            //}

            //for (int i = 0; i < DigitalOutputs.Count(); i++)
            //{
            //    DigitalOutputs[i].Close();
            //}

            //for (int i = 0; i < VoltageInputs.Count(); i++)
            //{
            //    VoltageInputs[i].Close();
            //}

            //for (int i = 0; i < VoltageRatioInputs.Count(); i++)
            //{
            //    VoltageRatioInputs[i].Close();
            //}

            //for (int i = 0; i < VoltageOutputs.Count(); i++)
            //{
            //    VoltageOutputs[i].Close();
            //}

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

        #region OpenDigitalInput Command

        public DelegateCommand<string> OpenDigitalInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenDigitalInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenDigitalInputCommandParameter;

        public string OpenDigitalInputContent { get; set; } = "Open";
        public string OpenDigitalInputToolTip { get; set; } = "Open DigitalInput";

        // Can get fancy and use Resources
        //public string OpenDigitalInputContent { get; set; } = "ViewName_OpenDigitalInputContent";
        //public string OpenDigitalInputToolTip { get; set; } = "ViewName_OpenDigitalInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenDigitalInputContent">OpenDigitalInput</system:String>
        //    <system:String x:Key="ViewName_OpenDigitalInputContentToolTip">OpenDigitalInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void OpenDigitalInput(string channelNumber)
        //public void OpenDigitalInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called OpenDigitalInput";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 number;

            //if (Int32.TryParse(channelNumber, out number))
            //{
            //    DigitalInputs[number].SerialNumber = serialNumber;

            //    await Task.Run(() => DigitalInputs[number].Open());
            //}
            //else
            //{
            //    Message = $"Cannot parse channelNumber:>{channelNumber}<";
            //    Log.Error(Message, Common.LOG_CATEGORY);
            //}

            // If launching a UserControl

            // if (_OpenDigitalInputHost is null) _OpenDigitalInputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenDigitalInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenDigitalInputEvent>().Publish(
            //      new OpenDigitalInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenDigitalInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenDigitalInputEvent>().Subscribe(OpenDigitalInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool OpenDigitalInputCanExecute(string value)
        //public bool OpenDigitalInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region CloseDigitalInput Command

        public DelegateCommand<string> CloseDigitalInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseDigitalInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseDigitalInputCommandParameter;

        public string CloseDigitalInputContent { get; set; } = "Close";
        public string CloseDigitalInputToolTip { get; set; } = "Close DigitalInput";

        // Can get fancy and use Resources
        //public string CloseDigitalInputContent { get; set; } = "ViewName_CloseDigitalInputContent";
        //public string CloseDigitalInputToolTip { get; set; } = "ViewName_CloseDigitalInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseDigitalInputContent">CloseDigitalInput</system:String>
        //    <system:String x:Key="ViewName_CloseDigitalInputContentToolTip">CloseDigitalInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseDigitalInput(string channelNumber)
        //public void CloseDigitalInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called CloseDigitalInput";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 number;

            //if (Int32.TryParse(channelNumber, out number))
            //{
            //    DigitalInputs[number].SerialNumber = serialNumber;

            //    await Task.Run(() => DigitalInputs[number].Close());
            //}
            //else
            //{
            //    Message = $"Cannot parse channelNumber:>{channelNumber}<";
            //    Log.Error(Message, Common.LOG_CATEGORY);
            //}


            // If launching a UserControl

            // if (_CloseDigitalInputHost is null) _CloseDigitalInputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseDigitalInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseDigitalInputEvent>().Publish(
            //      new CloseDigitalInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseDigitalInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseDigitalInputEvent>().Subscribe(CloseDigitalInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool CloseDigitalInputCanExecute(string value)
        //public bool CloseDigitalInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        void ConfigureInitialLogging(DigitalInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogPerformanceSequence = LogPerformanceSequence;
            phidgetEx.LogSequenceAction = LogSequenceAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        void ConfigureInitialLogging(DigitalOutputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogPerformanceSequence = LogPerformanceSequence;
            phidgetEx.LogSequenceAction = LogSequenceAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        void ConfigureInitialLogging(VoltageInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogPerformanceSequence = LogPerformanceSequence;
            phidgetEx.LogSequenceAction = LogSequenceAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        void ConfigureInitialLogging(VoltageRatioInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogPerformanceSequence = LogPerformanceSequence;
            phidgetEx.LogSequenceAction = LogSequenceAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        void ConfigureInitialLogging(VoltageOutputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogPerformanceSequence = LogPerformanceSequence;
            phidgetEx.LogSequenceAction = LogSequenceAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        #region OpenDigitalOutput Command

        public DelegateCommand<string> OpenDigitalOutputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenDigitalOutputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenDigitalOutputCommandParameter;

        public string OpenDigitalOutputContent { get; set; } = "Open";
        public string OpenDigitalOutputToolTip { get; set; } = "Open DigitalOutput";

        private async Task OpenDigitalOutput(DigitalOutputEx digitalOutput, SerialChannel serialChannel)
        {
            ConfigureInitialLogging(digitalOutput);

            if (digitalOutput.IsOpen is false)
            {
                await Task.Run(() => digitalOutput.Open(500));
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{digitalOutput} already open", Common.LOG_CATEGORY);
            }
        }

        // Can get fancy and use Resources
        //public string OpenDigitalOutputContent { get; set; } = "ViewName_OpenDigitalOutputContent";
        //public string OpenDigitalOutputToolTip { get; set; } = "ViewName_OpenDigitalOutputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenDigitalOutputContent">OpenDigitalOutput</system:String>
        //    <system:String x:Key="ViewName_OpenDigitalOutputContentToolTip">OpenDigitalOutput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void OpenDigitalOutput(string channelNumber)
        //public void OpenDigitalOutput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = $"Cool, you called OpenDigitalOutput on Channel:{channelNumber}";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(channelNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                DigitalOutputEx digitalOutputHost = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];

                switch (channel)
                {
                    case 0:
                        if (DigitalOutput0 is null) DigitalOutput0 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput0, serialChannel);
                        break;

                    case 1:
                        if (DigitalOutput1 is null) DigitalOutput1 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput1, serialChannel);
                        break;

                    case 2:
                        if (DigitalOutput2 is null) DigitalOutput2 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput2, serialChannel);
                        break;

                    case 3:
                        if (DigitalOutput3 is null) DigitalOutput3 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput3, serialChannel);
                        break;

                    case 4:
                        if (DigitalOutput4 is null) DigitalOutput4 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput4, serialChannel);
                        break;

                    case 5:
                        if (DigitalOutput5 is null) DigitalOutput5 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput5, serialChannel);
                        break;

                    case 6:
                        if (DigitalOutput6 is null) DigitalOutput6 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput6, serialChannel);
                        break;

                    case 7:
                        if (DigitalOutput7 is null) DigitalOutput7 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput7, serialChannel);
                        break;

                    case 8:
                        if (DigitalOutput8 is null) DigitalOutput8 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput8, serialChannel);
                        break;

                    case 9:
                        if (DigitalOutput9 is null) DigitalOutput9 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput9, serialChannel);
                        break;

                    case 10:
                        if (DigitalOutput10 is null) DigitalOutput10 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput10, serialChannel);
                        break;

                    case 11:
                        if (DigitalOutput11 is null) DigitalOutput11 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput11, serialChannel);
                        break;

                    case 12:
                        if (DigitalOutput12 is null) DigitalOutput12 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput12, serialChannel);
                        break;

                    case 13:
                        if (DigitalOutput13 is null) DigitalOutput13 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput13, serialChannel);
                        break;

                    case 14:
                        if (DigitalOutput14 is null) DigitalOutput14 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput14, serialChannel);
                        break;

                    case 15:
                        if (DigitalOutput15 is null) DigitalOutput15 = PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel];
                        await OpenDigitalOutput(DigitalOutput15, serialChannel);
                        break;
                }
            }

            // If launching a UserControl

            // if (_OpenDigitalOutputHost is null) _OpenDigitalOutputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenDigitalOutputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenDigitalOutputEvent>().Publish(
            //      new OpenDigitalOutputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenDigitalOutputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenDigitalOutputEvent>().Subscribe(OpenDigitalOutput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool OpenDigitalOutputCanExecute(string channelNumber)
        //public bool OpenDigitalOutputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(channelNumber, out channel)) throw new Exception($"Cannot parse channelNumber:{channelNumber}");

            if (SelectedInterfaceKit is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedInterfaceKit.SerialNumber, Channel = channel };

            DigitalOutputEx? host;

            if (!PhidgetDeviceLibrary.DigitalOutputChannels.TryGetValue(serialChannel, out host)) return false;

            if (host.IsAttached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region CloseDigitalOutput Command

        public DelegateCommand<string> CloseDigitalOutputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseDigitalOutputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseDigitalOutputCommandParameter;

        public string CloseDigitalOutputContent { get; set; } = "Close";
        public string CloseDigitalOutputToolTip { get; set; } = "Close DigitalOutput";

        // Can get fancy and use Resources
        //public string CloseDigitalOutputContent { get; set; } = "ViewName_CloseDigitalOutputContent";
        //public string CloseDigitalOutputToolTip { get; set; } = "ViewName_CloseDigitalOutputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseDigitalOutputContent">CloseDigitalOutput</system:String>
        //    <system:String x:Key="ViewName_CloseDigitalOutputContentToolTip">CloseDigitalOutput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseDigitalOutput(string channelNumber)
        //public void CloseDigitalOutput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called CloseDigitalOutput";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(channelNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                await Task.Run(() => PhidgetDeviceLibrary.DigitalOutputChannels[serialChannel].Close());
            }
            else
            {
                Message = $"Cannot parse channelNumber:>{channelNumber}<";
                Log.Error(Message, Common.LOG_CATEGORY);
            }

            OpenDigitalOutputCommand.RaiseCanExecuteChanged();
            CloseDigitalOutputCommand.RaiseCanExecuteChanged();


            // If launching a UserControl

            // if (_CloseDigitalOutputHost is null) _CloseDigitalOutputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseDigitalOutputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseDigitalOutputEvent>().Publish(
            //      new CloseDigitalOutputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseDigitalOutputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseDigitalOutputEvent>().Subscribe(CloseDigitalOutput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool CloseDigitalOutputCanExecute(string channelNumber)
        //public bool CloseDigitalOutputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(channelNumber, out channel)) throw new Exception($"Cannot parse channelNumber:{channelNumber}");

            if (SelectedInterfaceKit is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedInterfaceKit.SerialNumber, Channel = channel };

            DigitalOutputEx? host;

            if (!PhidgetDeviceLibrary.DigitalOutputChannels.TryGetValue(serialChannel, out host)) return false;

            if (host.IsAttached)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region OpenVoltageInput Command

        public DelegateCommand<string> OpenVoltageInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenVoltageInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenVoltageInputCommandParameter;

        public string OpenVoltageInputContent { get; set; } = "Open";
        public string OpenVoltageInputToolTip { get; set; } = "Open VoltageInput";

        // Can get fancy and use Resources
        //public string OpenVoltageInputContent { get; set; } = "ViewName_OpenVoltageInputContent";
        //public string OpenVoltageInputToolTip { get; set; } = "ViewName_OpenVoltageInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenVoltageInputContent">OpenVoltageInput</system:String>
        //    <system:String x:Key="ViewName_OpenVoltageInputContentToolTip">OpenVoltageInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void OpenVoltageInput(string channelNumber)
        //public void OpenVoltageInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called OpenVoltageInput";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 number;

            //if (Int32.TryParse(channelNumber, out number))
            //{
            //    VoltageInputs[number].SerialNumber = serialNumber;

            //    await Task.Run(() => VoltageInputs[number].Open());
            //}
            //else
            //{
            //    Message = $"Cannot parse channelNumber:>{channelNumber}<";
            //    Log.Error(Message, Common.LOG_CATEGORY);
            //}

            // If launching a UserControl

            // if (_OpenVoltageInputHost is null) _OpenVoltageInputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenVoltageInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenVoltageInputEvent>().Publish(
            //      new OpenVoltageInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenVoltageInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenVoltageInputEvent>().Subscribe(OpenVoltageInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool OpenVoltageInputCanExecute(string value)
        //public bool OpenVoltageInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region CloseVoltageInput Command

        public DelegateCommand<string> CloseVoltageInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseVoltageInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseVoltageInputCommandParameter;

        public string CloseVoltageInputContent { get; set; } = "Close";
        public string CloseVoltageInputToolTip { get; set; } = "Close VoltageInput";

        // Can get fancy and use Resources
        //public string CloseVoltageInputContent { get; set; } = "ViewName_CloseVoltageInputContent";
        //public string CloseVoltageInputToolTip { get; set; } = "ViewName_CloseVoltageInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseVoltageInputContent">CloseVoltageInput</system:String>
        //    <system:String x:Key="ViewName_CloseVoltageInputContentToolTip">CloseVoltageInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseVoltageInput(string channelNumber)
        //public void CloseVoltageInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called CloseVoltageInput";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedInterfaceKit.SerialNumber;
            Int32 number;

            //if (Int32.TryParse(channelNumber, out number))
            //{
            //    VoltageInputs[number].SerialNumber = serialNumber;

            //    await Task.Run(() => VoltageInputs[number].Close());
            //}
            //else
            //{
            //    Message = $"Cannot parse channelNumber:>{channelNumber}<";
            //    Log.Error(Message, Common.LOG_CATEGORY);
            //}

            // If launching a UserControl

            // if (_CloseVoltageInputHost is null) _CloseVoltageInputHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseVoltageInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseVoltageInputEvent>().Publish(
            //      new CloseVoltageInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseVoltageInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseVoltageInputEvent>().Subscribe(CloseVoltageInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool CloseVoltageInputCanExecute(string value)
        //public bool CloseVoltageInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion
        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

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
                //DigitalInputs[i].SerialNumber = serialNumber;
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
                //DigitalOutputs[i].SerialNumber = serialNumber;
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
                //VoltageInputs[i].SerialNumber = serialNumber;
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
                //VoltageRatioInputs[i].SerialNumber = serialNumber;
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
                //VoltageOutputs[i].SerialNumber = serialNumber;
            }
        }

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
