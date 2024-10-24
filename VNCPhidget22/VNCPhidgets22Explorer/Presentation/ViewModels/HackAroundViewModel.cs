using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Phidgets;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;
using VNC.Phidget.Events;
using VNC.Phidget.Players;

using VNCPhidget22.Configuration;

using VNCPhidgetConfig = VNCPhidget22.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
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
            Button4Command = new DelegateCommand(Button4Execute);
            Button5Command = new DelegateCommand(Button5Execute);

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            //ReloadPerformanceConfigFilesCommand = new DelegateCommand(ReloadPerformanceConfigFiles);
            //ReloadAdvancedServoSequenceConfigFilesCommand = new DelegateCommand(ReloadAdvancedServoSequenceConfigFiles);
            //ReloadInterfaceKitSequenceConfigFilesCommand = new DelegateCommand(ReloadInterfaceKitSequenceConfigFiles);
            //ReloadStepperSequenceConfigFilesCommand = new DelegateCommand(ReloadStepperSequenceConfigFiles);

            PlayPerformanceCommand = new DelegateCommand (PlayPerformance, PlayPerformanceCanExecute);
            PlayAdvancedServoSequenceCommand = new DelegateCommand(PlayAdvancedServoSequence, PlayAdvancedServoSequenceCanExecute);

            //InitializeServosCommand = new DelegateCommand(InitializeServos, InitializeServosCanExecute);
            //EngageAndCenterCommand = new DelegateCommand(EngageAndCenter, EngageAndCenterCanExecute);
            //ResetLimitsCommand = new DelegateCommand(ResetLimits);

            SetMotionParametersCommand = new DelegateCommand<string>(SetMotionParameters);
            RelativeAccelerationCommand = new DelegateCommand<Int32?>(RelativeAcceleration);
            RelativeVelocityLimitCommand = new DelegateCommand<Int32?>(RelativeVelocityLimit);

            PlayInterfaceKitSequenceCommand = new DelegateCommand(PlayInterfaceKitSequence, PlayInterfaceKitSequenceCanExecute);

            PlayStepperSequenceCommand = new DelegateCommand(PlayStepperSequence, PlayStepperSequenceCanExecute);

            // TODO(crhodes)
            // Fill out PlayStepperSequenceCommand

            //ActivePerformancePlayer = GetPerformancePlayer();

            Hosts = PerformanceLibrary.Hosts.ToList();

            Performances = PerformanceLibrary.AvailablePerformances.Values.ToList();

            AdvancedServoSequences = PerformanceLibrary.AvailableAdvancedServoSequences.Values.ToList();
            InterfaceKitSequences = PerformanceLibrary.AvailableInterfaceKitSequences.Values.ToList();
            StepperSequences = PerformanceLibrary.AvailableStepperSequences.Values.ToList();

            // Turn on logging of PropertyChanged from VNC.Core
            //LogOnPropertyChanged = false;

            Message = "HackAroundViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private string _message = "Initial Message";

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

        private int _repeats = 1;
        public int Repeats
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
            }
        }

        private bool _logPerformance = false;
        public bool LogPerformance
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

        private bool _logPerformanceSequence = false;
        public bool LogPerformanceSequence
        {
            get => _logPerformanceSequence;
            set
            {
                if (_logPerformanceSequence == value)
                    return;
                _logPerformanceSequence = value;
                OnPropertyChanged();
            }
        }

        private bool _logPerformanceAction = false;
        public bool LogSequenceAction
        {
            get => _logPerformanceAction;
            set
            {
                if (_logPerformanceAction == value)
                    return;
                _logPerformanceAction = value;
                OnPropertyChanged();
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
                _logActionVerification = value;
                OnPropertyChanged();
            }
        }

        #region Hosts

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
                OnPropertyChanged();

                AdvancedServos = _selectedHost.AdvancedServos?.ToList<VNCPhidgetConfig.AdvancedServo>();
                InterfaceKits = _selectedHost.InterfaceKits?.ToList<VNCPhidgetConfig.InterfaceKit>();
                Steppers = _selectedHost.Steppers?.ToList<VNCPhidgetConfig.Stepper>();
            }
        }

        #endregion

        #region Performances

        public string PerformanceFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private PerformancePlayer ActivePerformancePlayer { get; set; }
        private PerformanceLibrary PerformanceLibrary { get; set; } = new PerformanceLibrary();

        private PerformanceSequencePlayer ActivePerformanceSequencePlayer { get; set; }

        private IEnumerable<VNCPhidgetConfig.Performance> _performances;
        public IEnumerable<VNCPhidgetConfig.Performance> Performances
        {
            get => _performances;
            set
            {
                _performances = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Performance? _selectedPerformance;
        public VNCPhidgetConfig.Performance? SelectedPerformance
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

                PlayPerformanceCommand.RaiseCanExecuteChanged();
                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                //EngageAndCenterCommand.RaiseCanExecuteChanged();
                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private List<VNCPhidgetConfig.Performance> _selectedPerformances;
        public List<VNCPhidgetConfig.Performance> SelectedPerformances
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

                //PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                //EngageAndCenterCommand.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region AdvancedServo

        private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServos;
        public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServos
        {
            get
            {
                if (null == _AdvancedServos)
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

                return _AdvancedServos;
            }

            set
            {
                _AdvancedServos = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.AdvancedServo _selectedAdvancedServo;
        public VNCPhidgetConfig.AdvancedServo SelectedAdvancedServo
        {
            get => _selectedAdvancedServo;
            set
            {
                if (_selectedAdvancedServo == value)
                    return;
                _selectedAdvancedServo = value;

                //OpenAdvancedServoCommand.RaiseCanExecuteChanged();
                //PlayPerformanceCommand.RaiseCanExecuteChanged();
                //PlaySequenceCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private bool _logCurrentChangeEvents = false;
        public bool LogCurrentChangeEvents
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

        private bool _logPositionChangeEvents = false;
        public bool LogPositionChangeEvents
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

        private bool _logVelocityChangeEvents = false;
        public bool LogVelocityChangeEvents
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

        public List<Int32> RelativeAccelerationAdjustment { get; } = new List<Int32>
        {                
            -5000,
            -1000,
            -500,
            -100,
            -50,
            50,
            100,
            500,
            1000,
            5000
        };

        public List<Int32> RelativeVelocityLimitAdjustment { get; } = new List<Int32>
        {   
            -1000,
            -500,
            -100,
            -50,
            -10,
            10,
            50,
            100,
            500,
            1000
        };

        private IEnumerable<VNCPhidgetConfig.AdvancedServoSequence> _advancedServoSequences;
        public IEnumerable<VNCPhidgetConfig.AdvancedServoSequence> AdvancedServoSequences
        {
            get => _advancedServoSequences;
            set
            {
                _advancedServoSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.AdvancedServoSequence? _selectedAdvancedServoSequence;
        public VNCPhidgetConfig.AdvancedServoSequence? SelectedAdvancedServoSequence
        {
            get => _selectedAdvancedServoSequence;
            set
            {
                if (_selectedAdvancedServoSequence == value) return;

                _selectedAdvancedServoSequence = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private List<VNCPhidgetConfig.AdvancedServoSequence> _selectedAdvancedServoSequences;
        public List<VNCPhidgetConfig.AdvancedServoSequence> SelectedAdvancedServoSequences
        {
            get => _selectedAdvancedServoSequences;
            set
            {
                if (_selectedAdvancedServoSequences == value)
                {
                    return;
                }

                _selectedAdvancedServoSequences = value;
                OnPropertyChanged();

                PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region InterfaceKit

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

                //OpenInterfaceKitCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private bool _displayInputChangeEvents = false;

        public bool LogInputChangeEvents
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

        private bool _displayOutputChangeEvents = false;

        public bool LogOutputChangeEvents
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

        private bool _sensorChangeEvents = false;

        public bool LogSensorChangeEvents
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

        private IEnumerable<VNCPhidgetConfig.InterfaceKitSequence> _interfaceKitSequences;
        public IEnumerable<VNCPhidgetConfig.InterfaceKitSequence> InterfaceKitSequences
        {
            get => _interfaceKitSequences;
            set
            {
                _interfaceKitSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.InterfaceKitSequence? _selectedInterfaceKitSequence;
        public VNCPhidgetConfig.InterfaceKitSequence? SelectedInterfaceKitSequence
        {
            get => _selectedInterfaceKitSequence;
            set
            {
                if (_selectedInterfaceKitSequence == value) return;

                _selectedInterfaceKitSequence = value;
                OnPropertyChanged();

                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private List<VNCPhidgetConfig.InterfaceKitSequence> _selectedInterfaceKitSequences;
        public List<VNCPhidgetConfig.InterfaceKitSequence> SelectedInterfaceKitSequences
        {
            get => _selectedInterfaceKitSequences;
            set
            {
                if (_selectedInterfaceKitSequences == value)
                {
                    return;
                }

                _selectedInterfaceKitSequences = value;
                OnPropertyChanged();

                PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Stepper

        private IEnumerable<VNCPhidgetConfig.Stepper> _Steppers;
        public IEnumerable<VNCPhidgetConfig.Stepper> Steppers
        {
            get
            {
                if (null == _Steppers)
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

                return _Steppers;
            }

            set
            {
                _Steppers = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Stepper _selectedStepper;
        public VNCPhidgetConfig.Stepper SelectedStepper
        {
            get => _selectedStepper;
            set
            {
                if (_selectedStepper == value)
                    return;
                _selectedStepper = value;

                //OpenStepperCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private IEnumerable<VNCPhidgetConfig.StepperSequence> _stepperSequences;
        public IEnumerable<VNCPhidgetConfig.StepperSequence> StepperSequences
        {
            get => _stepperSequences;
            set
            {
                _stepperSequences = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.StepperSequence? _selectedStepperSequence;
        public VNCPhidgetConfig.StepperSequence? SelectedStepperSequence
        {
            get => _selectedStepperSequence;
            set
            {
                if (_selectedStepperSequence == value) return;

                _selectedStepperSequence = value;
                OnPropertyChanged();

                PlayStepperSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        private Dictionary<string, VNCPhidgetConfig.StepperSequence> _availableStepperSequences;
        public Dictionary<string, VNCPhidgetConfig.StepperSequence> AvailableStepperSequences
        {
            get => _availableStepperSequences;
            set
            {
                _availableStepperSequences = value;
                OnPropertyChanged();
            }
        }

        private List<VNCPhidgetConfig.StepperSequence> _selectedStepperSequences;
        public List<VNCPhidgetConfig.StepperSequence> SelectedStepperSequences
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

                PlayStepperSequenceCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

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

        public ICommand Button1Command { get; private set; }
        public ICommand Button2Command { get; private set; }
        public ICommand Button3Command { get; private set; }
        public ICommand Button4Command { get; private set; }
        public ICommand Button5Command { get; private set; }

        private void Button1Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button1Execute) Enter", Common.LOG_CATEGORY);

            Message = "Button1 Clicked";

            PlayParty();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(Button1Execute) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async void Button2Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button2Execute) Enter", Common.LOG_CATEGORY);

            Message = "Button2 Clicked - Opening PhidgetManager";

            Phidgets.Manager phidgetManager = new Phidgets.Manager();

            phidgetManager.Attach += PhidgetManager_Attach;
            phidgetManager.Detach += PhidgetManager_Detach;
            phidgetManager.ServerConnect += PhidgetManager_ServerConnect;
            phidgetManager.ServerDisconnect += PhidgetManager_ServerDisconnect;
            phidgetManager.Error += PhidgetManager_Error;

            phidgetManager.open();
            //phidgetManager.open("192.168.150.21", 5001);

            phidgetManager.Attach -= PhidgetManager_Attach;
            phidgetManager.Detach -= PhidgetManager_Detach;
            phidgetManager.ServerConnect -= PhidgetManager_ServerConnect;
            phidgetManager.ServerDisconnect -= PhidgetManager_ServerDisconnect;
            phidgetManager.Error -= PhidgetManager_Error;

            phidgetManager.close();

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

            //    for (int i = 0; i < 5; i++)
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

        private void Button4Execute()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(Button4Execute) Enter", Common.LOG_CATEGORY);

            Message = "Button4 Clicked";

            SequenceEventArgs sequenceEventArgs = new SequenceEventArgs();

            sequenceEventArgs.AdvancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            {
                //SerialNumber = 99415,
                Name = "psbc21_SequenceServo0",
                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, Acceleration = 5000, VelocityLimit = 200, Engaged = true },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 110 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 100 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, TargetPosition = 90 },
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, Engaged = false },
                }
            };

            EventAggregator.GetEvent<VNC.Phidget.Events.AdvancedServoSequenceEvent>().Publish(sequenceEventArgs);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(Button4Execute) Exit", Common.LOG_CATEGORY, startTicks);
        }

        //#region Reload Config Files

        //public ICommand ReloadPerformanceConfigFilesCommand { get; private set; }
        //public ICommand ReloadAdvancedServoSequenceConfigFilesCommand { get; private set; }
        //public ICommand ReloadInterfaceKitSequenceConfigFilesCommand { get; private set; }
        //public ICommand ReloadStepperSequenceConfigFilesCommand { get; private set; }

        //private void ReloadPerformanceConfigFiles()
        //{
        //    Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

        //    Message = "ReloadPerformanceConfigFiles Clicked";

        //    //LoadPerformancesConfig();

        //    Log.Info("End", Common.LOG_CATEGORY, startTicks);
        //}

        //private void ReloadAdvancedServoSequenceConfigFiles()
        //{
        //    Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

        //    Message = "ReloadAdvancedServoSequenceConfigFiles Clicked";

        //    // TODO(crhodes)
        //    // Call something in PerformanceSequencePlayer

        //    //LoadAdvanceServoConfig();

        //    Log.Info("End", Common.LOG_CATEGORY, startTicks);
        //}

        //private void ReloadInterfaceKitSequenceConfigFiles()
        //{
        //    Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

        //    Message = "ReloadInterfaceKitSequenceConfigFiles Clicked";

        //    // TODO(crhodes)
        //    // Call something in PerformanceSequencePlayer

        //    //LoadInterfaceKitConfig();

        //    Log.Info("End", Common.LOG_CATEGORY, startTicks);
        //}

        //private void ReloadStepperSequenceConfigFiles()
        //{
        //    Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

        //    Message = "ReloadStepperSequenceConfigFiles Clicked";

        //    // TODO(crhodes)
        //    // Call something in PerformanceSequencePlayer

        //    //LoadStepperConfig();

        //    Log.Info("End", Common.LOG_CATEGORY, startTicks);
        //}

        //#endregion

        #region PerformanceFileName DoubleClick

        public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        //private void PerformanceFileName_DoubleClick()
        //{
        //    Message = "PerformanceFileName_DoubleClick";

        //    LoadPerformancesConfig();
        //}

        #endregion

        #region PlayPerformance Command

        public DelegateCommand PlayPerformanceCommand { get; set; }
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
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayPerformance) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformance";

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformanceSequencePlayer.
            // Think through whether this make sense.

            PerformancePlayer performancePlayer = GetPerformancePlayer();
            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            foreach (VNCPhidgetConfig.Performance performance in SelectedPerformances)
            {
                if (LogPerformance)
                {
                    Log.Trace($"Playing performance:{performance.Name} description:{performance.Description}" +
                        $" loops:{performance.PerformanceLoops} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                        $" beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                        $" performanceSequences:{performance.PerformanceSequences?.Count()}" +
                        $" afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                        $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
                }

                VNCPhidgetConfig.Performance? nextPerformance = performance;

                // NOTE(crhodes)
                // Why would we need to check given UI brought us here.
                // Might be useful generally

                //if (AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
                //{ 

                //}

                await performancePlayer.RunPerformanceLoops(nextPerformance);

                nextPerformance = nextPerformance?.NextPerformance;

                while (nextPerformance is not null)
                {
                    if (LogPerformance)
                    {
                        Log.Trace($"Playing performance:{performance.Name} description:{performance.Description}" +
                            $" loops:{performance.PerformanceLoops} playSequencesInParallel:{performance.PlaySequencesInParallel}" +
                            $" beforePerformanceLoopPerformances:{performance.BeforePerformanceLoopPerformances?.Count()}" +
                            $" performanceSequences:{performance.PerformanceSequences?.Count()}" +
                            $" afterPerformanceLoopPerformances:{performance.AfterPerformanceLoopPerformances?.Count()}" +
                            $" nextPerformance:{performance.NextPerformance}", Common.LOG_CATEGORY);
                    }

                    if (PerformanceLibrary.AvailablePerformances.ContainsKey(nextPerformance.Name ?? ""))
                    {
                        nextPerformance = PerformanceLibrary.AvailablePerformances[nextPerformance.Name];

                        await performancePlayer.RunPerformanceLoops(nextPerformance);

                        nextPerformance = nextPerformance?.NextPerformance;
                    }
                    else
                    {
                        Log.Error($"Cannot find performance:>{nextPerformance.Name}<", Common.LOG_CATEGORY);
                        nextPerformance = null;
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

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayPerformance) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool PlayPerformanceCanExecute()
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

        #region PlayAdvancedServoSequence Command

        public DelegateCommand PlayAdvancedServoSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayAdvancedServoSequenceContent { get; set; } = "Play Sequence";
        public string PlayAdvancedServoSequenceToolTip { get; set; } = "PlayAdvancedServoSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayAdvancedServoSequence()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayAdvancedServoSequence) Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayAdvancedServoSequence";

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformancePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            foreach (VNCPhidgetConfig.AdvancedServoSequence sequence in SelectedAdvancedServoSequences)
            {
                if (LogPerformanceSequence) Log.Trace($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            SerialNumber = SelectedAdvancedServo.SerialNumber,
                            Name = sequence.Name,
                            SequenceType = "AS",
                            SequenceLoops = sequence.SequenceLoops
                        };

                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                            {
                                if (LogPerformanceSequence) Log.Trace($"Executing sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
                                //nextPerformanceSequence = await performanceSequencePlayer.ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                                await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(nextPerformanceSequence);
                            });
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayAdvancedServoSequence) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayAdvancedServoSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedAdvancedServoSequences?.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region AdvancedServo Individual Commands

        #region InitializeServos Command

        //public DelegateCommand InitializeServosCommand { get; set; }
        //// If using CommandParameter, figure out TYPE here and above
        //// and remove above declaration
        ////public DelegateCommand<TYPE> InitializeServosCommand { get; set; }
        ////public TYPE InitializeServosCommandParameter;
        //public string InitializeServosContent { get; set; } = "InitializeServos";
        //public string InitializeServosToolTip { get; set; } = "InitializeServos ToolTip";

        //// Can get fancy and use Resources
        ////public string InitializeServosContent { get; set; } = "ViewName_InitializeServosContent";
        ////public string InitializeServosToolTip { get; set; } = "ViewName_InitializeServosContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_InitializeServosContent">InitializeServos</system:String>
        ////    <system:String x:Key="ViewName_InitializeServosContentToolTip">InitializeServos ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE and fix above
        ////public void InitializeServos(TYPE value)
        //public async void InitializeServos()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeServos) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called InitializeServos";

        //    // TODO(crhodes)
        //    // This has sideffect of setting ActivePerformancePlayer.
        //    // Think through whether this make sense.
        //    // Also, unless we have multiple call here it only does one AdvancedServo
        //    // We need a generic routine like "Engage and Center Servos
        //    // that calls each of the appropriate phidgets.

        //    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        //    VNCPhidgetConfig.PerformanceSequence? advancedServoSequence =
        //        new VNCPhidgetConfig.PerformanceSequence
        //        {
        //            Name = "Initialize Servos",
        //            SequenceType = "AS"
        //        };

        //    await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<InitializeServosEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<InitializeServosEvent>().Publish(
        //    //      new InitializeServosEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class InitializeServosEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<InitializeServosEvent>().Subscribe(InitializeServos);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeServos) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        ////public bool InitializeServosCanExecute(TYPE value)
        //public bool InitializeServosCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;

        //    //if (AdvancedServoSequenceConfigFileName is not null)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        #endregion

        #region EngageAndCenter Command

        //public DelegateCommand EngageAndCenterCommand { get; set; }
        //// If using CommandParameter, figure out TYPE here and above
        //// and remove above declaration
        ////public DelegateCommand<TYPE> EngageAndCenterCommand { get; set; }
        ////public TYPE EngageAndCenterCommandParameter;
        //public string EngageAndCenterContent { get; set; } = "EngageAndCenter";
        //public string EngageAndCenterToolTip { get; set; } = "EngageAndCenter ToolTip";

        //// Can get fancy and use Resources
        ////public string EngageAndCenterContent { get; set; } = "ViewName_EngageAndCenterContent";
        ////public string EngageAndCenterToolTip { get; set; } = "ViewName_EngageAndCenterContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_EngageAndCenterContent">EngageAndCenter</system:String>
        ////    <system:String x:Key="ViewName_EngageAndCenterContentToolTip">EngageAndCenter ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE and fix above
        ////public void EngageAndCenter(TYPE value)
        //public async void EngageAndCenter()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(EngageAndCenter) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called EngageAndCenter";

        //    // TODO(crhodes)
        //    // This has sideffect of setting ActivePerformancePlayer.
        //    // Think through whether this make sense.
        //    // Also, unless we have multiple call here it only does one AdvancedServo
        //    // We need a generic routine like "Engage and Center Servos
        //    // that calls each of the appropriate phidgets.

        //    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        //    VNCPhidgetConfig.PerformanceSequence? advancedServoSequence = 
        //        new VNCPhidgetConfig.PerformanceSequence
        //        {
        //            Name = "Engage and Center Servos",
        //            SequenceType = "AS"
        //        };

        //    await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<EngageAndCenterEvent>().Publish(
        //    //      new EngageAndCenterEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class EngageAndCenterEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<EngageAndCenterEvent>().Subscribe(EngageAndCenter);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(EngageAndCenter) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        ////public bool EngageAndCenterCanExecute(TYPE value)
        //public bool EngageAndCenterCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;

        //    //if (AdvancedServoSequenceConfigFileName is not null)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        #endregion

        #region SetMotionParameters Command

        public DelegateCommand<string> SetMotionParametersCommand { get; set; }

        public async void SetMotionParameters(string speed)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SetMotionParameters) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called SetMotionParameters";

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformanceSequencePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.PerformanceSequence? advancedServoSequence = null;

            switch (speed)
            {
                case "Fast":

                    advancedServoSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            SerialNumber = SelectedAdvancedServo.SerialNumber,
                            Name = "Acceleration(5000) VelocityLimit(1000)",
                            SequenceType = "AS"
                        };
                    break;

                case "Slow":

                    advancedServoSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            SerialNumber = SelectedAdvancedServo.SerialNumber,
                            Name = "Acceleration(500) VelocityLimit(100)",
                            SequenceType = "AS"
                        };
                    break;
            }

            await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SetMotionParameters) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region ResetLimits Command

        //public DelegateCommand ResetLimitsCommand { get; set; }

        //public async void ResetLimits()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ResetLimits) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = $"Cool, you called ResetLimits";

        //    // TODO(crhodes)
        //    // This has sideffect of setting ActivePerformancePlayer.
        //    // Think through whether this make sense.

        //    PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

        //    VNCPhidgetConfig.PerformanceSequence? advancedServoSequence =
        //        new VNCPhidgetConfig.PerformanceSequence
        //        {
        //            Name = "Reset Position Limits (RPL)",
        //            SequenceType = "AS"
        //        };

        //    await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(advancedServoSequence);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
        //    //      new SetMotionParametersEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(AfterCollectResetLimitsonSaved) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        #endregion

        #region RelativeAccelerationCommand Command

        public DelegateCommand<Int32?> RelativeAccelerationCommand { get; set; }

        public async void RelativeAcceleration(Int32? relativeAcceleration)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RelativeAcceleration) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called RelativeAcceleration {relativeAcceleration}";

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformancePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.AdvancedServoSequence advancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            {
                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 1, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 2, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 3, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 4, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 5, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 6, RelativeAcceleration = relativeAcceleration},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 7, RelativeAcceleration = relativeAcceleration}
                }
            };

            await ActivePerformanceSequencePlayer.ActiveAdvancedServoHost.RunActionLoops(advancedServoSequence);

            //VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence = 
            //     new PerformanceSequence 
            //     { 
            //         Name = $"Acceleraion {relativeAcceleration}", 
            //         SequenceType = "AS"
            //     };

            // await ExecutePerformanceSequence(nextPerformanceSequence);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetMotionParametersEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetMotionParametersEvent>().Publish(
            //      new SetMotionParametersEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            //Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RelativeAcceleration) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region RelativeVelocityLimitCommand Command

        public DelegateCommand<Int32?> RelativeVelocityLimitCommand { get; set; }

        public async void RelativeVelocityLimit(Int32? relativeVelocityLimit)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RelativeVelocityLimit) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called RelativeVelocityLimit {relativeVelocityLimit}";

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformancePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            VNCPhidgetConfig.AdvancedServoSequence advancedServoSequence = new VNCPhidgetConfig.AdvancedServoSequence
            {
                Actions = new[]
                {
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 0, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 1, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 2, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 3, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 4, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 5, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 6, RelativeVelocityLimit = relativeVelocityLimit},
                    new VNCPhidgetConfig.AdvancedServoServoAction { ServoIndex = 7, RelativeVelocityLimit = relativeVelocityLimit}
                }                
            };

            await ActivePerformanceSequencePlayer.ActiveAdvancedServoHost.RunActionLoops(advancedServoSequence);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RelativeVelocityLimit) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion
        
        #region PlayInterfaceKitSequence Command

        public DelegateCommand PlayInterfaceKitSequenceCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlaySequenceCommand { get; set; }
        //public TYPE PlaySequenceCommandParameter;
        public string PlayInterfaceKitSequenceContent { get; set; } = "Play Sequence";
        public string PlayInterfaceKitSequenceToolTip { get; set; } = "PlayInterfaceKitSequence ToolTip";

        // Can get fancy and use Resources
        //public string PlaySequenceContent { get; set; } = "ViewName_PlaySequenceContent";
        //public string PlaySequenceToolTip { get; set; } = "ViewName_PlaySequenceContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_PlaySequenceContent">PlaySequence</system:String>
        //    <system:String x:Key="ViewName_PlaySequenceContentToolTip">PlaySequence ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlaySequence(TYPE value)
        public async void PlayInterfaceKitSequence()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayInterfaceKitSequence) Enter", Common.LOG_CATEGORY);

            Message = "Cool, you called PlayInterfaceKitSequence";

            // TODO(crhodes)
            // This has sideaffect of setting ActivePerformancePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            foreach (InterfaceKitSequence sequence in SelectedInterfaceKitSequences)
            {
                if (LogPerformanceSequence) Log.Trace($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    PerformanceSequence? nextPerformanceSequence = 
                        new PerformanceSequence
                        {
                            SerialNumber = SelectedInterfaceKit.SerialNumber,
                            Name = sequence.Name,
                            SequenceType = "IK",
                            SequenceLoops = sequence.SequenceLoops
                        };

                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                    {
                        if (LogPerformanceSequence) Log.Trace($"Executing sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);

                        await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(nextPerformanceSequence);
                    });
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayInterfaceKitSequence) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayInterfaceKitSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedInterfaceKitSequences?.Count > 0)
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

        public DelegateCommand PlayStepperSequenceCommand { get; set; }
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

            // TODO(crhodes)
            // This has sideffect of setting ActivePerformancePlayer.
            // Think through whether this make sense.

            PerformanceSequencePlayer performanceSequencePlayer = GetPerformanceSequencePlayer();

            foreach (VNCPhidgetConfig.StepperSequence sequence in SelectedStepperSequences)
            {
                if (LogPerformanceSequence) Log.Trace($"Playing sequence:{sequence.Name}", Common.LOG_CATEGORY);

                try
                {
                    VNCPhidgetConfig.PerformanceSequence? nextPerformanceSequence =
                        new VNCPhidgetConfig.PerformanceSequence
                        {
                            SerialNumber = SelectedStepper.SerialNumber,
                            Name = sequence.Name,
                            SequenceType = "ST",
                            SequenceLoops = sequence.SequenceLoops
                        };

                    // NOTE(crhodes)
                    // Run on another thread to keep UI active
                    await Task.Run(async () =>
                    {
                        if (LogPerformanceSequence) Log.Trace($"Executing sequence:{nextPerformanceSequence.Name}", Common.LOG_CATEGORY);
                        //nextPerformanceSequence = await performanceSequencePlayer.ExecutePerformanceSequenceLoops(nextPerformanceSequence);
                        await ActivePerformanceSequencePlayer.ExecutePerformanceSequence(nextPerformanceSequence);
                    });
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
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
        //public bool PlayPerformanceCanExecute(TYPE value)
        public bool PlayStepperSequenceCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (SelectedStepperSequences?.Count > 0)
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

        private PerformancePlayer GetPerformancePlayer()
        {
            if (ActivePerformancePlayer == null)
            {
                ActivePerformancePlayer = new PerformancePlayer(EventAggregator);
            }

            // HACK(crhodes)
            // Need a cleaner way of handing logging.  Maybe a LoggingConfiguration class that gets passed around.

            ActivePerformancePlayer.LogPerformance = LogPerformance;

            ActivePerformancePlayer.LogPerformanceSequence = LogPerformanceSequence;

            ActivePerformancePlayer.LogSequenceAction = LogSequenceAction;
            ActivePerformancePlayer.LogActionVerification = LogActionVerification;

            ActivePerformancePlayer.LogPhidgetEvents = LogPhidgetEvents;

            return ActivePerformancePlayer;
        }

        private PerformanceSequencePlayer GetPerformanceSequencePlayer()
        {
            if (ActivePerformanceSequencePlayer == null)
            {
                ActivePerformanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);
            }

            ActivePerformanceSequencePlayer.LogPerformanceSequence = LogPerformanceSequence;
            ActivePerformanceSequencePlayer.LogSequenceAction = LogSequenceAction;
            ActivePerformanceSequencePlayer.LogActionVerification = LogActionVerification;

            ActivePerformanceSequencePlayer.LogCurrentChangeEvents = LogCurrentChangeEvents;
            ActivePerformanceSequencePlayer.LogPositionChangeEvents = LogPositionChangeEvents;
            ActivePerformanceSequencePlayer.LogVelocityChangeEvents = LogVelocityChangeEvents;

            ActivePerformanceSequencePlayer.LogInputChangeEvents = LogInputChangeEvents;
            ActivePerformanceSequencePlayer.LogOutputChangeEvents = LogOutputChangeEvents;
            ActivePerformanceSequencePlayer.LogSensorChangeEvents = LogSensorChangeEvents;

            ActivePerformanceSequencePlayer.LogPhidgetEvents = LogPhidgetEvents;

            return ActivePerformanceSequencePlayer;
        }

        private async Task PlayParty()
        {
            const Int32 sbc11SerialNumber = 46049;

            const Int32 sbc21SerialNumber = 48301;
            const Int32 sbc22SerialNumber = 251831;
            const Int32 sbc23SerialNumber = 48284;

            //InterfaceKitEx ifkEx11 = new InterfaceKitEx("192.168.150.11", 5001, sbc11SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx21 = new InterfaceKitEx("192.168.150.21", 5001, sbc21SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx22 = new InterfaceKitEx("192.168.150.22", 5001, sbc22SerialNumber, embedded: true, EventAggregator);
            InterfaceKitEx ifkEx23 = new InterfaceKitEx("192.168.150.23", 5001, sbc23SerialNumber, embedded: true, EventAggregator);

            try
            {
                //ifkEx11.Open(Common.PhidgetOpenTimeout);
                ifkEx21.Open(Common.PhidgetOpenTimeout);
                ifkEx22.Open(Common.PhidgetOpenTimeout);
                ifkEx23.Open(Common.PhidgetOpenTimeout);

                await Task.Run(() =>
                {
                    Parallel.Invoke(
                         () => InterfaceKitParty2(ifkEx21, 500, 5 * Repeats),
                         () => InterfaceKitParty2(ifkEx22, 250, 10 * Repeats),
                         () => InterfaceKitParty2(ifkEx23, 125, 20 * Repeats)
                     //() => InterfaceKitParty2(ifkEx11, 333, 8 * Repeats)
                     //Parallel.Invoke(
                     //     () => InterfaceKitParty2(ifkEx21, 10, 5 * Repeats),
                     //     () => InterfaceKitParty2(ifkEx22, 10, 10 * Repeats),
                     //     () => InterfaceKitParty2(ifkEx23, 10, 20 * Repeats)
                     //     () => InterfaceKitParty2(ifkEx11, 10, 8 * Repeats)
                     );
                });

                //ifkEx11.Close();
                ifkEx21.Close();
                ifkEx22.Close();
                ifkEx23.Close();
            }
            catch (PhidgetException pe)
            {
                switch (pe.Type)
                {
                    case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
                        //        ifk.SerialNumber));
                        break;

                    default:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("{0}\nInterface Kit {0}",
                        //pe.ToString(),
                        //        ifk.SerialNumber));
                        break;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void InterfaceKitParty2(VNC.Phidget.InterfaceKitEx ifkEx, Int32 sleep, Int32 loops)
        {
            try
            {
                Log.Debug($"InterfaceKitParty2 {ifkEx.Host.IPAddress},{ifkEx.Host.Port} {ifkEx.SerialNumber} " +
                    $"sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);

                InterfaceKitDigitalOutputCollection ifkDigitalOut = ifkEx.InterfaceKit.outputs;

                for (int i = 0; i < loops; i++)
                {
                    ifkDigitalOut[0] = true;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[1] = true;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[2] = true;
                    Thread.Sleep(sleep);

                    ifkDigitalOut[0] = false;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[1] = false;
                    Thread.Sleep(sleep);
                    ifkDigitalOut[2] = false;
                    Thread.Sleep(sleep);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void PhidgetManager_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            Log.Trace($"Error {e.Type} {e.Code}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            Log.Trace($"ServerDisconnect {e.Device}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_ServerConnect(object sender, Phidgets.Events.ServerConnectEventArgs e)
        {
            Log.Trace($"ServerConnect {e.Device}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            Log.Trace($"Detach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            Log.Trace($"Attach {e.Device.Name} {e.Device.Address} {e.Device.ID} {e.Device.SerialNumber}", Common.LOG_CATEGORY);
        }

        private void Button5Execute()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "Button5 Clicked";

            EventAggregator.GetEvent<VNC.Phidget.Events.InterfaceKitSequenceEvent>().Publish(new VNC.Phidget.Events.SequenceEventArgs());

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
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
