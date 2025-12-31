using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using DevExpress.Mvvm.Native;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;
using VNC.Phidget22.Players;

using VNCPhidget22Explorer.Core.Events;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class HackAroundViewModel 
        : EventViewModelBase, IMainViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public HackAroundViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            Button1Command = new DelegateCommand(Button1Execute);
            Button2Command = new DelegateCommand(Button2Execute);
            Button3Command = new DelegateCommand(Button3Execute);
            //Button4Command = new DelegateCommand(Button4Execute);
            Button5Command = new DelegateCommand(Button5Execute);

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            PlayDeviceSettingsCommand = new DelegateCommand(PlayDeviceSettings, PlayDeviceSettingsCanExecute);

            PlayPerformanceCommand = new DelegateCommand(PlayPerformance, PlayPerformanceCanExecute);

            PlayDigitalOutputSequenceCommand = new DelegateCommand(PlayDigitalOutputSequence, PlayDigitalOutputSequenceCanExecute);

            PlayRCServoSequenceCommand = new DelegateCommand(PlayRCServoSequence, PlayRCServoSequenceCanExecute);

            PlayStepperSequenceCommand = new DelegateCommand(PlayStepperSequence, PlayStepperSequenceCanExecute);

            LoadPerformances();

            LoadChannelSequences();

            EventAggregator.GetEvent<SelectedCollectionChangedEvent>().Subscribe(CollectionChanged);

            // Turn on logging of PropertyChanged from VNC.Core
            //LogOnPropertyChanged = false;

            Message = "HackAroundViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }


        private void LoadPerformances()
        {
            DeviceSettings = PerformanceLibrary.AvailableDeviceSettings.Values.ToList();
            Performances = PerformanceLibrary.AvailablePerformances.Values.ToList();

            // TODO(crhodes)
            // Might be better to do this with an Event so gets called fewer times.
            // Currently called for each Performance in file.  Ideally just once per file.

            //Common.PerformanceLibrary.AvailablePerformances.CollectionChanged += AvailablePerformances_CollectionChanged;
        }

        private void AvailablePerformances_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Log.TRACE("HackAroundViewModel notified AvailablePerformances_CollectionChanged", Common.LOG_CATEGORY);

            //Performances = Common.PerformanceLibrary.AvailablePerformances.Values.ToObservableCollection<Performance>();
            //Performances = Common.PerformanceLibrary.AvailablePerformances.Values.ToList();
        }

        private void CollectionChanged(SelectedCollectionChangedEventArgs args)
        {
            Log.TRACE($"HackAroundViewModel notified >{args.Name}< CollectionChanged", Common.LOG_CATEGORY);

            switch (args.Name)
            {
                case "DeviceSettings":
                    DeviceSettings = PerformanceLibrary.AvailableDeviceSettings.Values.ToList();
                    break;

                case "Performances":
                    Performances = PerformanceLibrary.AvailablePerformances.Values.ToList();
                    break;

                case "DigitalInputSequences":
                    DigitalInputSequences = PerformanceLibrary.AvailableDigitalInputSequences.Values.ToList();
                    break;

                case "DigitalOutputSequences":
                    DigitalOutputSequences = PerformanceLibrary.AvailableDigitalOutputSequences.Values.ToList();
                    break;

                case "RCServoSequences":
                    RCServoSequences = PerformanceLibrary.AvailableRCServoSequences.Values.ToList();
                    break;

                case "StepperSequences":
                    StepperSequences = PerformanceLibrary.AvailableStepperSequences.Values.ToList();
                    break;

                case "VoltageInputSequences":
                    VoltageInputSequences = PerformanceLibrary.AvailableVoltageInputSequences.Values.ToList();
                    break;

                case "VoltageOutputSequences":
                    VoltageOutputSequences = PerformanceLibrary.AvailableVoltageOutputSequences.Values.ToList();
                    break;

                default:
                    Log.ERROR($"Unexpected Collection Name: >{args.Name}<", Common.LOG_CATEGORY);
                    break;
            }
        }

        void LoadChannelSequences()
        {
            // TODO(crhodes)
            // Add a method for each new supported ChannelClass

            LoadDigitalInputSequences();
            LoadDigitalOutputSequences();
            LoadRCServoSequences();
            LoadStepperSequences();
            LoadVolatageInputSequences();
            LoadVolatageOutputSequences();
        }

        private void LoadDigitalInputSequences()
        {
            DigitalInputSequences = PerformanceLibrary.AvailableDigitalInputSequences.Values.ToList();

            DigitalInputs = Common.PhidgetDeviceLibrary.DigitalInputChannels
                .Keys
                .DistinctBy(x => x.SerialNumber)
                .Select(x => x.SerialNumber)
                .ToList();
        }
        private void LoadDigitalOutputSequences()
        {
            DigitalOutputSequences = PerformanceLibrary.AvailableDigitalOutputSequences.Values.ToList();

            DigitalOutputs = Common.PhidgetDeviceLibrary.DigitalOutputChannels
                  .Keys
                  .DistinctBy(x => x.SerialNumber)
                  .Select(x => x.SerialNumber)
                  .ToList();
        }

        private void LoadRCServoSequences()
        {
            RCServoSequences = PerformanceLibrary.AvailableRCServoSequences.Values.ToList();

            RCServos = Common.PhidgetDeviceLibrary.RCServoChannels
                 .Keys
                 .DistinctBy(x => x.SerialNumber)
                 .Select(x => x.SerialNumber)
                 .ToList();
        }

        private void LoadStepperSequences()
        {
            StepperSequences = PerformanceLibrary.AvailableStepperSequences.Values.ToList();

            Steppers = Common.PhidgetDeviceLibrary.StepperChannels
                 .Keys
                 .DistinctBy(x => x.SerialNumber)
                 .Select(x => x.SerialNumber)
                 .ToList();
        }

        private void LoadVolatageInputSequences()
        {
            VoltageInputSequences = PerformanceLibrary.AvailableVoltageInputSequences.Values.ToList();

            VoltageInputs = Common.PhidgetDeviceLibrary.VoltageInputChannels
                 .Keys
                 .DistinctBy(x => x.SerialNumber)
                 .Select(x => x.SerialNumber)
                 .ToList();
        }

        private void LoadVolatageOutputSequences()
        {
            VoltageOutputSequences = PerformanceLibrary.AvailableVoltageOutputSequences.Values.ToList();

            VoltageOutputs = Common.PhidgetDeviceLibrary.VoltageOutputChannels
                 .Keys
                 .DistinctBy(x => x.SerialNumber)
                 .Select(x => x.SerialNumber)
                 .ToList();
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private Int32 _repeats = 1;
        public Int32 Repeats
        {
            get => _repeats;
            set
            {
                if (_repeats == value)
                    return;
                _repeats = value;
                OnPropertyChanged();
            }
        }

        private string? _hostConfigFileName;
        public string? HostConfigFileName
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

        #region Logging

        private Boolean _logPhidgetEvents = false;

        public Boolean LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logErrorEvents = true;    // Probably always want to see errors
        public Boolean LogErrorEvents
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

        private Boolean _logPropertyChangeEvents = false;
        public Boolean LogPropertyChangeEvents
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

        #region AdvancedServo

        private Boolean _logCurrentChangeEvents = false;
        public Boolean LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value)
                    return;
                _logCurrentChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logPositionChangeEvents = false;
        public Boolean LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value)
                    return;
                _logPositionChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logVelocityChangeEvents = false;
        public Boolean LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value)
                    return;
                _logVelocityChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logTargetPositionReachedEvents = false;
        public Boolean LogTargetPositionReachedEvents
        {
            get => _logTargetPositionReachedEvents;
            set
            {
                if (_logTargetPositionReachedEvents == value)
                    return;
                _logTargetPositionReachedEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region InterfaceKit

        private Boolean _displayInputChangeEvents = false;

        public Boolean LogInputChangeEvents
        {
            get => _displayInputChangeEvents;
            set
            {
                if (_displayInputChangeEvents == value)
                    return;
                _displayInputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _displayOutputChangeEvents = false;

        public Boolean LogOutputChangeEvents
        {
            get => _displayOutputChangeEvents;
            set
            {
                if (_displayOutputChangeEvents == value)
                    return;
                _displayOutputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _sensorChangeEvents = false;

        public Boolean LogSensorChangeEvents
        {
            get => _sensorChangeEvents;
            set
            {
                if (_sensorChangeEvents == value)
                    return;
                _sensorChangeEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private Boolean _logPerformance = false;
        public Boolean LogPerformance
        {
            get => _logPerformance;
            set
            {
                if (_logPerformance == value)
                    return;
                _logPerformance = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logDeviceChannelSequence = false;
        public Boolean LogDeviceChannelSequence
        {
            get => _logDeviceChannelSequence;
            set
            {
                if (_logDeviceChannelSequence == value)
                    return;
                _logDeviceChannelSequence = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logChannelAction = false;
        public Boolean LogChannelAction
        {
            get => _logChannelAction;
            set
            {
                if (_logChannelAction == value)
                    return;
                _logChannelAction = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logActionVerification = false;
        public Boolean LogActionVerification
        {
            get => _logActionVerification;
            set
            {
                if (_logActionVerification == value)
                    return;
                _logActionVerification = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DeviceSettings

        public string DeviceSettingsFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private IEnumerable<Performance>? _performanceConfigs;
        public IEnumerable<Performance>? DeviceSettings
        {
            get => _performanceConfigs;
            set
            {
                _performanceConfigs = value;
                OnPropertyChanged();
            }
        }

        private Performance? _selectedDeviceSetting;
        public Performance? SelectedDeviceSetting
        {
            get => _selectedDeviceSetting;
            set
            {
                if (_selectedDeviceSetting == value)
                {
                    return;
                }

                _selectedDeviceSetting = value;
                OnPropertyChanged();

                PlayDeviceSettingsCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<Performance>? _selectedDeviceSettings;
        public List<Performance>? SelectedDeviceSettings
        {
            get => _selectedDeviceSettings;
            set
            {
                if (_selectedDeviceSettings == value)
                {
                    return;
                }

                _selectedDeviceSettings = value;
                OnPropertyChanged();

                PlayDeviceSettingsCommand?.RaiseCanExecuteChanged();
            }
        }

        private Int32? _serialNumber2;
        public Int32? SerialNumber2
        {
            get => _serialNumber2;
            set
            {
                _serialNumber2 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Performances

        public string PerformanceFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private IEnumerable<Performance>? _performances;
        public IEnumerable<Performance>? Performances
        {
            get => _performances;
            set
            {
                _performances = value;
                OnPropertyChanged();
            }
        }

        private Performance? _selectedPerformance;
        public Performance? SelectedPerformance
        {
            get => _selectedPerformance;
            set
            {
                if (_selectedPerformance == value)
                {
                    return;
                }

                _selectedPerformance = value;
                OnPropertyChanged();

                PlayPerformanceCommand?.RaiseCanExecuteChanged();
                PlayDigitalOutputSequenceCommand?.RaiseCanExecuteChanged();
                PlayRCServoSequenceCommand?.RaiseCanExecuteChanged();
                PlayStepperSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<Performance>? _selectedPerformances;
        public List<Performance>? SelectedPerformances
        {
            get => _selectedPerformances;
            set
            {
                if (_selectedPerformances == value)
                {
                    return;
                }

                _selectedPerformances = value;
                OnPropertyChanged();

                PlayPerformanceCommand?.RaiseCanExecuteChanged();
                PlayDigitalOutputSequenceCommand?.RaiseCanExecuteChanged();
                PlayRCServoSequenceCommand?.RaiseCanExecuteChanged();
                PlayStepperSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private Int32? _serialNumber;
        public Int32? SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ChannelClass Sequences

        private List<Int32> _hubPorts = new List<Int32>()
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public List<Int32> HubPorts
        {
            get => _hubPorts;
            set
            {
                if (_hubPorts == value)
                    return;
                _hubPorts = value;
                OnPropertyChanged();
            }
        }
        
        private Int32? _selectedHubPort;
        public Int32? SelectedHubPort
        {
            get => _selectedHubPort;
            set
            {
                _selectedHubPort = value;
                OnPropertyChanged();
            }
        }

        private List<Int32> _channels = new List<Int32>()
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public List<Int32> Channels
        {
            get => _channels;
            set
            {
                if (_hubPorts == value)
                    return;
                _channels = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedChannel;
        public Int32? SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                _selectedChannel = value;
                OnPropertyChanged();
            }
        }

        #region DigitalInput

        private IEnumerable<Int32>? _DigitalInputs;
        public IEnumerable<Int32>? DigitalInputs
        {
            get
            {
                return _DigitalInputs;
            }

            set
            {
                _DigitalInputs = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedDigitalInputPhidget;
        public Int32? SelectedDigitalInputPhidget
        {
            get => _selectedDigitalInputPhidget;
            set
            {
                _selectedDigitalInputPhidget = value;
                OnPropertyChanged();

                //PlayDigitalInputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _digitalInputSequences;
        public IEnumerable<ChannelSequence>? DigitalInputSequences
        {
            get => _digitalInputSequences;
            set
            {
                _digitalInputSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedDigitalInputSequence;
        public ChannelSequence? SelectedDigitalInputSequence
        {
            get => _selectedDigitalInputSequence;
            set
            {
                if (_selectedDigitalInputSequence == value) return;

                _selectedDigitalInputSequence = value;
                OnPropertyChanged();

                //PlayDigitalInputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedDigitalInputSequences;
        public List<ChannelSequence>? SelectedDigitalInputSequences
        {
            get => _selectedDigitalInputSequences;
            set
            {
                if (_selectedDigitalInputSequences == value)
                {
                    return;
                }

                _selectedDigitalInputSequences = value;
                OnPropertyChanged();

                //PlayDigitalInputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region DigitalOutput

        private IEnumerable<Int32>? _DigitalOutputs;
        public IEnumerable<Int32>? DigitalOutputs
        {
            get
            {
                return _DigitalOutputs;
            }

            set
            {
                _DigitalOutputs = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedDigitalOutputPhidget;
        public Int32? SelectedDigitalOutputPhidget
        {
            get => _selectedDigitalOutputPhidget;
            set
            {
                _selectedDigitalOutputPhidget = value;
                OnPropertyChanged();

                PlayDigitalOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _digitalOutputSequences;
        public IEnumerable<ChannelSequence>? DigitalOutputSequences
        {
            get => _digitalOutputSequences;
            set
            {
                _digitalOutputSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedDigitalOutputSequence;
        public ChannelSequence? SelectedDigitalOutputSequence
        {
            get => _selectedDigitalOutputSequence;
            set
            {
                if (_selectedDigitalOutputSequence == value) return;

                _selectedDigitalOutputSequence = value;
                OnPropertyChanged();

                PlayDigitalOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedDigitalOutputSequences;
        public List<ChannelSequence>? SelectedDigitalOutputSequences
        {
            get => _selectedDigitalOutputSequences;
            set
            {
                if (_selectedDigitalOutputSequences == value)
                {
                    return;
                }

                _selectedDigitalOutputSequences = value;
                OnPropertyChanged();

                PlayDigitalOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region RCServo

        private IEnumerable<Int32>? _rcServos;
        public IEnumerable<Int32>? RCServos
        {
            get
            {
                return _rcServos;
            }

            set
            {
                _rcServos = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedRCServoPhidget;
        public Int32? SelectedRCServoPhidget
        {
            get => _selectedRCServoPhidget;
            set
            {
                _selectedRCServoPhidget = value;
                OnPropertyChanged();

                PlayRCServoSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _rcServoSequences;
        public IEnumerable<ChannelSequence>? RCServoSequences
        {
            get => _rcServoSequences;
            set
            {
                _rcServoSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedRCServoSequence;
        public ChannelSequence? SelectedRCServoSequence
        {
            get => _selectedRCServoSequence;
            set
            {
                if (_selectedRCServoSequence == value) return;

                _selectedRCServoSequence = value;
                OnPropertyChanged();

                PlayRCServoSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedRCServoSequences;
        public List<ChannelSequence>? SelectedRCServoSequences
        {
            get => _selectedRCServoSequences;
            set
            {
                if (_selectedRCServoSequences == value)
                {
                    return;
                }

                _selectedRCServoSequences = value;
                OnPropertyChanged();

                PlayRCServoSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Stepper

        private IEnumerable<Int32>? _Steppers;
        public IEnumerable<Int32>? Steppers
        {
            get
            {
                return _Steppers;
            }

            set
            {
                _Steppers = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedStepperPhidget;
        public Int32? SelectedStepperPhidget
        {
            get => _selectedStepperPhidget;
            set
            {
                _selectedStepperPhidget = value;
                OnPropertyChanged();

                PlayStepperSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _stepperSequences;
        public IEnumerable<ChannelSequence>? StepperSequences
        {
            get => _stepperSequences;
            set
            {
                _stepperSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedStepperSequence;
        public ChannelSequence? SelectedStepperSequence
        {
            get => _selectedStepperSequence;
            set
            {
                if (_selectedStepperSequence == value) return;

                _selectedStepperSequence = value;
                OnPropertyChanged();

                PlayStepperSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedStepperSequences;
        public List<ChannelSequence>? SelectedStepperSequences
        {
            get => _selectedStepperSequences;
            set
            {
                if (_selectedStepperSequences == value)
                {
                    return;
                }

                _selectedStepperSequences = value;
                OnPropertyChanged();

                PlayStepperSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region VoltageInput

        private IEnumerable<Int32>? _VoltageInputs;
        public IEnumerable<Int32>? VoltageInputs
        {
            get
            {
                return _VoltageInputs;
            }

            set
            {
                _VoltageInputs = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedVoltageInputPhidget;
        public Int32? SelectedVoltageInputPhidget
        {
            get => _selectedVoltageInputPhidget;
            set
            {
                _selectedVoltageInputPhidget = value;
                OnPropertyChanged();

                //PlayVoltageInputCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _voltageInputSequences;
        public IEnumerable<ChannelSequence>? VoltageInputSequences
        {
            get => _voltageInputSequences;
            set
            {
                _voltageInputSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedVoltageInputSequence;
        public ChannelSequence? SelectedVoltageInputSequence
        {
            get => _selectedVoltageInputSequence;
            set
            {
                if (_selectedVoltageInputSequence == value) return;

                _selectedVoltageInputSequence = value;
                OnPropertyChanged();

                //PlayVoltageInputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedVoltageInputSequences;
        public List<ChannelSequence>? SelectedVoltageInputSequences
        {
            get => _selectedVoltageInputSequences;
            set
            {
                if (_selectedVoltageInputSequences == value)
                {
                    return;
                }

                _selectedVoltageInputSequences = value;
                OnPropertyChanged();

                //PlayVoltageInputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region VoltageOutput

        private IEnumerable<Int32>? _VoltageOutputs;
        public IEnumerable<Int32>? VoltageOutputs
        {
            get
            {
                return _VoltageOutputs;
            }

            set
            {
                _VoltageOutputs = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedVoltageOutputPhidget;
        public Int32? SelectedVoltageOutputPhidget
        {
            get => _selectedVoltageOutputPhidget;
            set
            {
                _selectedVoltageOutputPhidget = value;
                OnPropertyChanged();

                //PlayVoltageOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _voltageOutputSequences;
        public IEnumerable<ChannelSequence>? VoltageOutputSequences
        {
            get => _voltageOutputSequences;
            set
            {
                _voltageOutputSequences = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // This is for displaying info on the first sequence

        private ChannelSequence? _selectedVoltageOutputSequence;
        public ChannelSequence? SelectedVoltageOutputSequence
        {
            get => _selectedVoltageOutputSequence;
            set
            {
                if (_selectedVoltageOutputSequence == value) return;

                _selectedVoltageOutputSequence = value;
                OnPropertyChanged();

                //PlayVoltageOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private List<ChannelSequence>? _selectedVoltageOutputSequences;
        public List<ChannelSequence>? SelectedVoltageOutputSequences
        {
            get => _selectedVoltageOutputSequences;
            set
            {
                if (_selectedVoltageOutputSequences == value)
                {
                    return;
                }

                _selectedVoltageOutputSequences = value;
                OnPropertyChanged();

                //PlayVoltageOutputSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Commands

        #region Command ConfigFileName DoubleClick

        public DelegateCommand? ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Enter", Common.LOG_CATEGORY);

            Message = "ConfigFileName_DoubleClick";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        public ICommand? Button1Command { get; private set; }
        public ICommand? Button2Command { get; private set; }
        public ICommand? Button3Command { get; private set; }
        public ICommand? Button4Command { get; private set; }
        public ICommand? Button5Command { get; private set; }

        private void Button1Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button1Execute) Enter", Common.LOG_CATEGORY);

            Message = "Button1 Clicked";

            //PlayParty();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(Button1Execute) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Button2Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button2Execute) Enter", Common.LOG_CATEGORY);

            //Message = "Button2 Clicked - Opening PhidgetManager";

            //Phidget22.Manager phidgetManager = new Phidget22.Manager();

            //phidgetManager.Attach += PhidgetManager_Attach;
            //phidgetManager.Detach += PhidgetManager_Detach;
            //phidgetManager.ServerConnect += PhidgetManager_ServerConnect;
            //phidgetManager.ServerDisconnect += PhidgetManager_ServerDisconnect;
            //phidgetManager.Error += PhidgetManager_Error;

            //phidgetManager.open();
            ////phidgetManager.open("192.168.150.21", 5001);

            //phidgetManager.Attach -= PhidgetManager_Attach;
            //phidgetManager.Detach -= PhidgetManager_Detach;
            //phidgetManager.ServerConnect -= PhidgetManager_ServerConnect;
            //phidgetManager.ServerDisconnect -= PhidgetManager_ServerDisconnect;
            //phidgetManager.Error -= PhidgetManager_Error;

            //phidgetManager.close();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(Button2Execute) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Button3Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button3Execute) Enter", Common.LOG_CATEGORY);

            Message = "Button3 Clicked - Loading PhidgetDevices";

            //InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);

            //ifkEx21.Open();

            ////ifkEx21.InterfaceKit.OutputChange += Ifk_OutputChange;

            ////InterfaceKitDigitalOutputCollection ifkdoc = ifkEx21.InterfaceKit.outputs;
            ////InterfaceKitDigitalOutputCollection ifkdoc = ifkEx.outputs;

            //Task.Run(() =>
            //{
            //    InterfaceKitDigitalOutputCollection ifkdoc = ifkEx21.InterfaceKit.outputs;

            //    for (Int32 i = 0; i < 5; i++)
            //    {
            //        ifkdoc[0] = true;
            //        Thread.Sleep(500);
            //        ifkdoc[0] = false;
            //        Thread.Sleep(500);
            //    }
            //    //Parallel.Invoke(
            //    //    () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats)
            //    //);
            //});

            //ifkEx21.Close();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(Button3Execute) Exit", Common.LOG_CATEGORY, startTicks);
        }

           #region PerformanceFileName DoubleClick

        public DelegateCommand? PerformanceFileName_DoubleClick_Command { get; set; }

        //private void PerformanceFileName_DoubleClick()
        //{
        //    Message = "PerformanceFileName_DoubleClick";

        //    LoadPerformancesConfig();
        //}

        #endregion

        #region PlayPerformance Command

        public DelegateCommand? PlayDeviceSettingsCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayPerformanceCommand { get; set; }
        //public TYPE PlayPerformanceCommandParameter;
        public string PlayDeviceSettingsContent { get; set; } = "Play DeviceSettings";
        public string PlayDeviceSettingsToolTip { get; set; } = "Play DeviceSettings ToolTip";

        // Can get fancy and use Resources
        //public string PlayPerformanceContent { get; set; } = "ViewName_PlayPerformanceContent";
        //public string PlayPerformanceToolTip { get; set; } = "ViewName_PlayPerformanceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlayPerformanceContent">PlayPerformance</system:String>
        //    <system:String x:Key="ViewName_PlayPerformanceContentToolTip">PlayPerformance ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayPerformance(TYPE value)

        public async void PlayDeviceSettings()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayDeviceSettings";

            PerformancePlayer performancePlayer = GetNewPerformancePlayer();

            // TODO(crhodes)
            // Maybe this should be a do / while loop

            if (LogPerformance) Log.TRACE($"Selected Performances:{SelectedDeviceSettings?.Count} serialNumber:{SerialNumber}", Common.LOG_CATEGORY);

            if (SelectedDeviceSettings is not null)
            {
                foreach (Performance performance in SelectedDeviceSettings)
                {
                    Performance? selectedPerformance = performance;

                    if (SerialNumber is not null)
                    {
                        if (LogPerformance) Log.TRACE($"Setting serialNumber:{SerialNumber} on nextPerformance:{selectedPerformance.Name}", Common.LOG_CATEGORY);
                        selectedPerformance.SerialNumber = SerialNumber;
                    }

                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                    {
                        await performancePlayer.ExecutePerformance(selectedPerformance);
                    });
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean PlayDeviceSettingsCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedDeviceSettings?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayPerformance Command

        public DelegateCommand? PlayPerformanceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayPerformanceCommand { get; set; }
        //public TYPE PlayPerformanceCommandParameter;
        public string PlayPerformanceContent { get; set; } = "PlayPerformance";
        public string PlayPerformanceToolTip { get; set; } = "PlayPerformance ToolTip";

        // Can get fancy and use Resources
        //public string PlayPerformanceContent { get; set; } = "ViewName_PlayPerformanceContent";
        //public string PlayPerformanceToolTip { get; set; } = "ViewName_PlayPerformanceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlayPerformanceContent">PlayPerformance</system:String>
        //    <system:String x:Key="ViewName_PlayPerformanceContentToolTip">PlayPerformance ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayPerformance(TYPE value)

        public async void PlayPerformance()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformance";

            PerformancePlayer performancePlayer = GetNewPerformancePlayer();

            // TODO(crhodes)
            // Maybe this should be a do / while loop

            if (LogPerformance) Log.TRACE($"Selected Performances:{SelectedPerformances?.Count} serialNumber:{SerialNumber}", Common.LOG_CATEGORY);

            if (SelectedPerformances is not null)
            {
                foreach (Performance performance in SelectedPerformances)
                {
                    Performance? selectedPerformance = performance;

                    if (SerialNumber is not null)
                    {
                        if (LogPerformance) Log.TRACE($"Setting serialNumber:{SerialNumber} on nextPerformance:{selectedPerformance.Name}", Common.LOG_CATEGORY);
                        selectedPerformance.SerialNumber = SerialNumber;
                    }

                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                    {
                        await performancePlayer.ExecutePerformance(selectedPerformance);
                    });
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean PlayPerformanceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedPerformances?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayRCServoSequence Command

        public DelegateCommand? PlayRCServoSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayRCServoSequenceContent { get; set; } = "Play Sequence";
        public string PlayRCServoSequenceToolTip { get; set; } = "PlayRCServoSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayRCServoSequence()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayRCServoSequence) Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayRCServoSequence";
            PublishStatusMessage(Message);

            // NOTE(crhodes)
            // This has side effect of using current Logging Settings

            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();
            player.SerialNumber = SelectedRCServoPhidget;

            if (SelectedRCServoSequences is not null)
            {
                foreach (RCServoSequence sequence in SelectedRCServoSequences)
                {
                    if (LogDeviceChannelSequence) Log.TRACE($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                    try
                    {
                        DeviceChannelSequence? nextPhidgetDeviceSequence =
                            new DeviceChannelSequence
                            {
                                Name = sequence.Name,
                                SerialNumber = SelectedRCServoPhidget,
                                ChannelClass = "RCServo",
                                SequenceLoops = sequence.SequenceLoops
                            };

                        // NOTE(crhodes)
                        // Apply ChannelSequence overrides if provided

                        if (nextPhidgetDeviceSequence.HubPort is null)
                        {
                            nextPhidgetDeviceSequence.HubPort = SelectedHubPort;
                        }

                        if (nextPhidgetDeviceSequence.Channel is null)
                        {
                            nextPhidgetDeviceSequence.Channel = SelectedChannel;
                        }

                        // NOTE(crhodes)
                        // Run on another thread to keep UI active
                        await Task.Run(async () =>
                        {
                            if (LogDeviceChannelSequence) Log.TRACE($"Executing sequence:{nextPhidgetDeviceSequence.Name}", Common.LOG_CATEGORY);

                            await player.ExecuteDeviceChannelSequence(nextPhidgetDeviceSequence);
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.ERROR(ex, Common.LOG_CATEGORY);
                    }
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayRCServoSequence) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean PlayPerformanceCanExecute(TYPE value)
        public Boolean PlayRCServoSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedRCServoPhidget > 0 && SelectedRCServoSequences?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        //#region AdvancedServo Individual Commands

        //#region InitializeServos Command

        ////public DelegateCommand InitializeServosCommand { get; set; }
        ////// If using CommandParameter, figure out TYPE here and above
        ////// and remove above declaration
        //////public DelegateCommand<TYPE> InitializeServosCommand { get; set; }
        //////public TYPE InitializeServosCommandParameter;
        ////public string InitializeServosContent { get; set; } = "InitializeServos";
        ////public string InitializeServosToolTip { get; set; } = "InitializeServos ToolTip";

        ////// Can get fancy and use Resources
        //////public string InitializeServosContent { get; set; } = "ViewName_InitializeServosContent";
        //////public string InitializeServosToolTip { get; set; } = "ViewName_InitializeServosContentToolTip";

        ////// Put these in Resource File
        //////    <system:String x:Key="ViewName_InitializeServosContent">InitializeServos</system:String>
        //////    <system:String x:Key="ViewName_InitializeServosContentToolTip">InitializeServos ToolTip</system:String>  

        ////// If using CommandParameter, figure out TYPE and fix above
        //////public void InitializeServos(TYPE value)
        ////public async void InitializeServos()
        ////{
        ////    Int64 startTicks = 0;
        ////    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeServos) Enter", Common.LOG_CATEGORY);
        ////    // TODO(crhodes)
        ////    // Do something amazing.
        ////    Message = "Cool, you called InitializeServos";

        ////    // TODO(crhodes)
        ////    // This has sideffect of setting ActivePerformancePlayer.
        ////    // Think through whether this make sense.
        ////    // Also, unless we have multiple call here it only does one AdvancedServo
        ////    // We need a generic routine like "Engage and Center Servos
        ////    // that calls each of the appropriate Phidget22.

        ////    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        ////    VNCPhidgetConfig.PerformanceSequence? advancedServoSequence =
        ////        new VNCPhidgetConfig.PerformanceSequence
        ////        {
        ////            Name = "Initialize Servos",
        ////            SequenceType = "AS"
        ////        };

        ////    await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

        ////    // Uncomment this if you are telling someone else to handle this

        ////    // Common.EventAggregator.GetEvent<InitializeServosEvent>().Publish();

        ////    // May want EventArgs

        ////    //  EventAggregator.GetEvent<InitializeServosEvent>().Publish(
        ////    //      new InitializeServosEventArgs()
        ////    //      {
        ////    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        ////    //            Process = _contextMainViewModel.Context.SelectedProcess
        ////    //      });

        ////    // Start Cut Three - Put this in PrismEvents

        ////    // public class InitializeServosEvent : PubSubEvent { }

        ////    // End Cut Three

        ////    // Start Cut Four - Put this in places that listen for event

        ////    //Common.EventAggregator.GetEvent<InitializeServosEvent>().Subscribe(InitializeServos);

        ////    // End Cut Four

        ////    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeServos) Exit", Common.LOG_CATEGORY, startTicks);
        ////}

        ////// If using CommandParameter, figure out TYPE and fix above
        //////public Boolean InitializeServosCanExecute(TYPE value)
        ////public Boolean InitializeServosCanExecute()
        ////{
        ////    // TODO(crhodes)
        ////    // Add any before button is enabled logic.
        ////    return true;

        ////    //if (AdvancedServoSequenceConfigFileName is not null)
        ////    //{
        ////    //    return true;
        ////    //}
        ////    //else
        ////    //{
        ////    //    return false;
        ////    //}
        ////}

        //#endregion

        //#region EngageAndCenter Command

        ////public DelegateCommand EngageAndCenterCommand { get; set; }
        ////// If using CommandParameter, figure out TYPE here and above
        ////// and remove above declaration
        //////public DelegateCommand<TYPE> EngageAndCenterCommand { get; set; }
        //////public TYPE EngageAndCenterCommandParameter;
        ////public string EngageAndCenterContent { get; set; } = "EngageAndCenter";
        ////public string EngageAndCenterToolTip { get; set; } = "EngageAndCenter ToolTip";

        ////// Can get fancy and use Resources
        //////public string EngageAndCenterContent { get; set; } = "ViewName_EngageAndCenterContent";
        //////public string EngageAndCenterToolTip { get; set; } = "ViewName_EngageAndCenterContentToolTip";

        ////// Put these in Resource File
        //////    <system:String x:Key="ViewName_EngageAndCenterContent">EngageAndCenter</system:String>
        //////    <system:String x:Key="ViewName_EngageAndCenterContentToolTip">EngageAndCenter ToolTip</system:String>  

        ////// If using CommandParameter, figure out TYPE and fix above
        //////public void EngageAndCenter(TYPE value)
        ////public async void EngageAndCenter()
        ////{
        ////    Int64 startTicks = 0;
        ////    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(EngageAndCenter) Enter", Common.LOG_CATEGORY);
        ////    // TODO(crhodes)
        ////    // Do something amazing.
        ////    Message = "Cool, you called EngageAndCenter";

        ////    // TODO(crhodes)
        ////    // This has sideffect of setting ActivePerformancePlayer.
        ////    // Think through whether this make sense.
        ////    // Also, unless we have multiple call here it only does one AdvancedServo
        ////    // We need a generic routine like "Engage and Center Servos
        ////    // that calls each of the appropriate Phidget22.

        ////    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        ////    VNCPhidgetConfig.PerformanceSequence? advancedServoSequence = 
        ////        new VNCPhidgetConfig.PerformanceSequence
        ////        {
        ////            Name = "Engage and Center Servos",
        ////            SequenceType = "AS"
        ////        };

        ////    await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

        ////    // Uncomment this if you are telling someone else to handle this

        ////    // Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Publish();

        ////    // May want EventArgs

        ////    //  EventAggregator.GetEvent<EngageAndCenterEvent>().Publish(
        ////    //      new EngageAndCenterEventArgs()
        ////    //      {
        ////    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        ////    //            Process = _contextMainViewModel.Context.SelectedProcess
        ////    //      });

        ////    // Start Cut Three - Put this in PrismEvents

        ////    // public class EngageAndCenterEvent : PubSubEvent { }

        ////    // End Cut Three

        ////    // Start Cut Four - Put this in places that listen for event

        ////    //Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Subscribe(EngageAndCenter);

        ////    // End Cut Four

        ////    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(EngageAndCenter) Exit", Common.LOG_CATEGORY, startTicks);
        ////}

        ////// If using CommandParameter, figure out TYPE and fix above
        //////public Boolean EngageAndCenterCanExecute(TYPE value)
        ////public Boolean EngageAndCenterCanExecute()
        ////{
        ////    // TODO(crhodes)
        ////    // Add any before button is enabled logic.
        ////    return true;

        ////    //if (AdvancedServoSequenceConfigFileName is not null)
        ////    //{
        ////    //    return true;
        ////    //}
        ////    //else
        ////    //{
        ////    //    return false;
        ////    //}
        ////}

        //#endregion

        //#endregion

        #region PlayDigitalOutputSequence Command

        public DelegateCommand? PlayDigitalOutputSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayDigitalOutputSequenceContent { get; set; } = "Play Sequence";
        public string PlayDigitalOutputSequenceToolTip { get; set; } = "PlayDigitalOutputSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayDigitalOutputSequence()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayDigitalOutputSequence) Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayDigitalOutputSequence";
            PublishStatusMessage(Message);

            // NOTE(crhodes)
            // This has side effect of using current Logging Settings

            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();
            player.SerialNumber = SelectedDigitalOutputPhidget;

            if (SelectedDigitalOutputSequences is not null)
            {
                foreach (DigitalOutputSequence sequence in SelectedDigitalOutputSequences)
                {
                    if (LogDeviceChannelSequence) Log.TRACE($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                    try
                    {
                        DeviceChannelSequence? nextPhidgetDeviceSequence =
                            new DeviceChannelSequence
                            {
                                SerialNumber = SelectedDigitalOutputPhidget,
                                Name = sequence.Name,
                                ChannelClass = "DigitalOutput",
                                SequenceLoops = sequence.SequenceLoops
                            };

                        // NOTE(crhodes)
                        // Apply ChannelSequence overrides if provided

                        if (nextPhidgetDeviceSequence.HubPort is null)
                        {
                            nextPhidgetDeviceSequence.HubPort = SelectedHubPort;
                        }

                        if (nextPhidgetDeviceSequence.Channel is null)
                        {
                            nextPhidgetDeviceSequence.Channel = SelectedChannel;
                        }

                        // NOTE(crhodes)
                        // Run on another thread to keep UI active
                        await Task.Run(async () =>
                        {
                            if (LogDeviceChannelSequence) Log.TRACE($"Executing sequence:{nextPhidgetDeviceSequence.Name}", Common.LOG_CATEGORY);

                            await player.ExecuteDeviceChannelSequence(nextPhidgetDeviceSequence);
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.ERROR(ex, Common.LOG_CATEGORY);
                    }
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayDigitalOutputSequence) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean PlayPerformanceCanExecute(TYPE value)
        public Boolean PlayDigitalOutputSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedDigitalOutputSequences?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayStepperSequence Command

        public DelegateCommand? PlayStepperSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayStepperSequenceContent { get; set; } = "Play Sequence";
        public string PlayStepperSequenceToolTip { get; set; } = "PlayStepperSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayStepperSequence()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayStepperSequence) Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayStepperSequence";
            PublishStatusMessage(Message);

            // NOTE(crhodes)
            // This has side effect of using current Logging Settings

            DeviceChannelSequencePlayer player = GetNewDeviceChannelSequencePlayer();
            player.SerialNumber = SelectedStepperPhidget;

            if (SelectedStepperSequences is not null)
            {
                foreach (StepperSequence sequence in SelectedStepperSequences)
                {
                    if (LogDeviceChannelSequence) Log.TRACE($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                    try
                    {
                        DeviceChannelSequence? nextPhidgetDeviceSequence =
                            new DeviceChannelSequence
                            {
                                SerialNumber = SelectedStepperPhidget,
                                Name = sequence.Name,
                                ChannelClass = "Stepper",
                                SequenceLoops = sequence.SequenceLoops
                            };

                        // NOTE(crhodes)
                        // Apply ChannelSequence overrides if provided

                        if (nextPhidgetDeviceSequence.HubPort is null)
                        {
                            nextPhidgetDeviceSequence.HubPort = SelectedHubPort;
                        }

                        if (nextPhidgetDeviceSequence.Channel is null)
                        {
                            nextPhidgetDeviceSequence.Channel = SelectedChannel;
                        }

                        // NOTE(crhodes)
                        // Run on another thread to keep UI active
                        await Task.Run(async () =>
                        {
                            if (LogDeviceChannelSequence) Log.TRACE($"Executing sequence:{nextPhidgetDeviceSequence.Name}", Common.LOG_CATEGORY);

                            await player.ExecuteDeviceChannelSequence(nextPhidgetDeviceSequence);
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.ERROR(ex, Common.LOG_CATEGORY);
                    }
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
            //      new PlayPerformanceEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class PlayPerformanceEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayStepperSequence) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean PlayPerformanceCanExecute(TYPE value)
        public Boolean PlayStepperSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedStepperPhidget > 0 && SelectedStepperSequences?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Public Methods (none)



        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods

        private PerformancePlayer GetNewPerformancePlayer()
        {
            PerformancePlayer player = new PerformancePlayer(EventAggregator, Common.PerformanceLibrary);

            // HACK(crhodes)
            // Need a cleaner way of handing logging.  Maybe a LoggingConfiguration class that gets passed around.

            player.LogPerformance = LogPerformance;

            player.LogDeviceChannelSequence = LogDeviceChannelSequence;
            player.LogChannelAction = LogChannelAction;
            player.LogActionVerification = LogActionVerification;

            player.LogPhidgetEvents = LogPhidgetEvents;

            return player;
        }

        private DeviceChannelSequencePlayer GetNewDeviceChannelSequencePlayer()
        {
            DeviceChannelSequencePlayer deviceChannelSequencePlayer = new DeviceChannelSequencePlayer(EventAggregator);

            deviceChannelSequencePlayer.LogDeviceChannelSequence = LogDeviceChannelSequence;
            deviceChannelSequencePlayer.LogChannelAction = LogChannelAction;
            deviceChannelSequencePlayer.LogActionVerification = LogActionVerification;

            deviceChannelSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
            deviceChannelSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
            deviceChannelSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;
            deviceChannelSequencePlayer.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            deviceChannelSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
            deviceChannelSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;
            deviceChannelSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

            deviceChannelSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;

            return deviceChannelSequencePlayer;
        }

        //private async Task PlayParty()
        //{
        //    const Int32 sbc11SerialNumber = 46049;

        //    const Int32 sbc21SerialNumber = 48301;
        //    const Int32 sbc22SerialNumber = 251831;
        //    const Int32 sbc23SerialNumber = 48284;

        //    //InterfaceKitEx ifkEx11 = new InterfaceKitEx("192.168.150.11", 5001, sbc11SerialNumber, embedded: true, EventAggregator);
        //    InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);
        //    InterfaceKitEx ifkEx22 = new InterfaceKitEx("192.168.150.22", 5001, sbc22SerialNumber, embedded: true, EventAggregator);
        //    InterfaceKitEx ifkEx23 = new InterfaceKitEx("192.168.150.23", 5001, sbc23SerialNumber, embedded: true, EventAggregator);

        //    try
        //    {
        //        //ifkEx11.Open(Common.PhidgetOpenTimeout);
        //        ifkEx21.Open(Common.PhidgetOpenTimeout);
        //        ifkEx22.Open(Common.PhidgetOpenTimeout);
        //        ifkEx23.Open(Common.PhidgetOpenTimeout);

        //        await Task.Run(() =>
        //        {
        //            Parallel.Invoke(
        //                 () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats),
        //                 () => InterfaceKitParty2(ifkEx22, 250, 10 * Repeats),
        //                 () => InterfaceKitParty2(ifkEx23, 125, 20 * Repeats)
        //             //() => InterfaceKitParty2(ifkEx11, 333, 8 * Repeats)
        //             //Parallel.Invoke(
        //             //     () => InterfaceKitParty2(ifkEx21, 10, 5 * Repeats),
        //             //     () => InterfaceKitParty2(ifkEx22, 10, 10 * Repeats),
        //             //     () => InterfaceKitParty2(ifkEx23, 10, 20 * Repeats)
        //             //     () => InterfaceKitParty2(ifkEx11, 10, 8 * Repeats)
        //             );
        //        });

        //        //ifkEx11.Close();
        //        ifkEx21.Close();
        //        ifkEx22.Close();
        //        ifkEx23.Close();
        //    }
        //    catch (PhidgetException pe)
        //    {
        //        switch (pe.Type)
        //        {
        //            case Phidget22.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
        //                //System.Diagnostics.Debug.WriteLine(
        //                //    string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
        //                //        ifk.SerialNumber));
        //                break;

        //            default:
        //                //System.Diagnostics.Debug.WriteLine(
        //                //    string.Format("{0}\nInterface Kit {0}",
        //                //pe.ToString(),
        //                //        ifk.SerialNumber));
        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ERROR(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void InterfaceKitParty2(VNC.Phidget22.InterfaceKitEx ifkEx, Int32 sleep, Int32 loops)
        //{
        //    try
        //    {
        //        //Log.Debug($"InterfaceKitParty2 {ifkEx.Host.IPAddress},{ifkEx.Host.Port} {ifkEx.SerialNumber} " +
        //        //    $"sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

        //        //InterfaceKitDigitalOutputCollection ifkDigitalOut = ifkEx.InterfaceKit.outputs;

        //        //for (Int32 i = 0; i < loops; i++)
        //        //{
        //        //    ifkDigitalOut[0] = true;
        //        //    Thread.Sleep(sleep);
        //        //    ifkDigitalOut[1] = true;
        //        //    Thread.Sleep(sleep);
        //        //    ifkDigitalOut[2] = true;
        //        //    Thread.Sleep(sleep);

        //        //    ifkDigitalOut[0] = false;
        //        //    Thread.Sleep(sleep);
        //        //    ifkDigitalOut[1] = false;
        //        //    Thread.Sleep(sleep);
        //        //    ifkDigitalOut[2] = false;
        //        //    Thread.Sleep(sleep);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ERROR(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void PhidgetManager_Error(object sender, Phidget22.Events.ErrorEventArgs e)
        //{
        //    Log.TRACE($"Error {e.Type} {e.Code}", Common.LOG_CATEGORY);
        //}

        //private void PhidgetManager_ServerDisconnect(object sender, Phidget22.Events.ServerDisconnectEventArgs e)
        //{
        //    Log.TRACE($"ServerDisconnect {e.Device}", Common.LOG_CATEGORY);
        //}

        //private void PhidgetManager_ServerConnect(object sender, Phidget22.Events.ServerConnectEventArgs e)
        //{
        //    Log.TRACE($"ServerConnect {e.Device}", Common.LOG_CATEGORY);
        //}

        //private void PhidgetManager_Detach(object sender, Phidget22.Events.DetachEventArgs e)
        //{
        //    Log.TRACE($"Detach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        //}

        //private void PhidgetManager_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        //{
        //    Log.TRACE($"Attach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        //}

        private void Button5Execute()
        {
            Int64 startTicks = Log.INFO("Enter", Common.LOG_CATEGORY);

            Message = "Button5 Clicked";

            EventAggregator.GetEvent<VNC.Phidget22.Events.InterfaceKitSequenceEvent>().Publish(new VNC.Phidget22.Events.SequenceEventArgs());

            Log.INFO("End", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region IInstanceCountVM

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
