using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;
using System.Windows;
using DevExpress.Xpo.Logger.Transport;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class Stepper1063ViewModel : EventViewModelBase, IStepper1063ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public Stepper1063ViewModel(
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

            OpenSteppersCommand = new DelegateCommand(OpenSteppers, OpenSteppersCanExecute);
            CloseSteppersCommand = new DelegateCommand(CloseSteppers, CloseSteppersCanExecute);

            OpenStepperCommand = new DelegateCommand<string>(OpenStepper, OpenStepperCanExecute);
            CloseStepperCommand = new DelegateCommand<string>(CloseStepper, CloseStepperCanExecute);

            InitializeVelocityCommand = new DelegateCommand<string>(InitializeVelocity, InitializeVelocityCanExecute);
            InitializeAccelerationCommand = new DelegateCommand<string>(InitializeAcceleration, InitializeAccelerationCanExecute);

            //RotateCommand = new DelegateCommand<string>(Rotate, RotateCanExecute);

            // If using CommandParameter, figure out TYPE here and below
            // and remove above declaration
            //RotateCommand = new DelegateCommand<TYPE>(Rotate, RotateCanExecute);


            //ZeroCurrentPositionCommand = new DelegateCommand(ZeroCurrentPosition, ZeroCurrentPositionCanExecute);

            // If using CommandParameter, figure out TYPE here and below
            // and remove above declaration
            ZeroCurrentPositionCommand = new DelegateCommand<string>(ZeroCurrentPosition, ZeroCurrentPositionCanExecute);

            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNCPhidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();
               
            Message = "Stepper1063ViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig = 
                JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();

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
                Steppers = _selectedHost.Steppers?.ToList<VNCPhidgetConfig.Stepper>();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Phidget

        private Phidget22.Phidget _phidgetDevice;
        public Phidget22.Phidget PhidgetDevice
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

        private bool? _stepperAttached;
        public bool? StepperAttached
        {
            get => _stepperAttached;
            set
            {
                _stepperAttached = value;
                OpenStepperCommand.RaiseCanExecuteChanged();
                CloseStepperCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Stepper

        #region Steppers

        private StepperEx _stepper0;
        public StepperEx Stepper0
        {
            get => _stepper0;
            set
            {
                if (_stepper0 == value)
                    return;
                _stepper0 = value;
                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // I think some boards support more than one Stepper
        // Stub out four for now

        private StepperEx _stepper1;
        public StepperEx Stepper1
        {
            get => _stepper1;
            set
            {
                if (_stepper1 == value)
                    return;
                _stepper1 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper2;
        public StepperEx Stepper2
        {
            get => _stepper2;
            set
            {
                if (_stepper2 == value)
                    return;
                _stepper2 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper3;
        public StepperEx Stepper3
        {
            get => _stepper3;
            set
            {
                if (_stepper3 == value)
                    return;
                _stepper3 = value;
                OnPropertyChanged();
            }
        }

        private Visibility _steppersVisibility = Visibility.Collapsed;
        public Visibility SteppersVisibility
        {
            get => _steppersVisibility;
            set
            {
                if (_steppersVisibility == value)
                    return;
                _steppersVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Stepper Events

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

        #endregion

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

                OpenSteppersCommand.RaiseCanExecuteChanged();
                OpenStepperCommand.RaiseCanExecuteChanged();

                // Set to null when host changes
                if (value is not null)
                {
                    DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[value.SerialNumber].DeviceChannels;

                    SteppersVisibility = deviceChannels.StepperCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    SteppersVisibility = Visibility.Collapsed;
                }

                OnPropertyChanged();
            }
        }

        //private StepperEx _activeStepper;
        //public StepperEx ActiveStepper
        //{
        //    get => _activeStepper;
        //    set
        //    {
        //        if (_activeStepper == value)
        //            return;
        //        _activeStepper = value;


        //        //if (_activeStepper is not null)
        //        //{
        //        //    PhidgetDevice = _activeStepper.Stepper;
        //        //}
        //        //else
        //        //{
        //        //    // TODO(crhodes)
        //        //    // PhidgetDevice = null ???
        //        //    // Will need to declare Phidget22.Phidget?
        //        //    PhidgetDevice = null;
        //        //}

        //        OnPropertyChanged();
        //    }
        //}

        //private int? _stepperCount;
        //public int? StepperCount
        //{
        //    get => _stepperCount;
        //    set
        //    {
        //        if (_stepperCount == value)
        //            return;
        //        _stepperCount = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private double _degrees;
        //public double Degrees
        //{
        //    get => _degrees;
        //    set
        //    {
        //        if (_degrees == value)
        //            return;
        //        _degrees = value;
        //        OnPropertyChanged();
        //    }
        //}

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
            PublishStatusMessage(Message);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region InitializeVelocityCommand

        //public DelegateCommand InitializeVelocityCommand { get; set; }
        public DelegateCommand<string> InitializeVelocityCommand { get; set; }
        public string InitializeVelocityContent { get; set; } = "Initilize Velocity";
        public string InitializeVelocityToolTip { get; set; } = "Initialize Velocity using Velocity Scale";

        // Can get fancy and use Resources
        //public string InitializeSlowStepperContent { get; set; } = "ViewName_InitializeSlowStepperContent";
        //public string InitializeSlowStepperToolTip { get; set; } = "ViewName_InitializeSlowStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowStepperContent">InitializeSlowStepper</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowStepperContentToolTip">InitializeSlowStepper ToolTip</system:String>  

        //public void InitializeSlowStepper()

        public void InitializeVelocity(string speed)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeVelocity) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeVelocity";
            PublishStatusMessage(Message);

            if ((Boolean)DeviceAttached)
            {
                //StepperServoCollection servos = ActiveStepper.Stepper.servos;

                try
                {
                    //for (int i = 0; i < servos.Count; i++)
                    //{
                    //    StepperProperties[i].InitializeVelocity(ConvertStringToInitializeMotion(speed));
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish(
            //      new InitializeSlowStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeSlowStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Subscribe(InitializeSlowStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeVelocity) Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowStepperCanExecute()
        public bool InitializeVelocityCanExecute(string speed)
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

        #region InitializeAccelerationCommand

        //public DelegateCommand InitializeVelocityCommand { get; set; }
        public DelegateCommand<string> InitializeAccelerationCommand { get; set; }
        public string InitializeAccelerationContent { get; set; } = "Initilize Acceleration";
        public string InitializeAccelerationToolTip { get; set; } = "Initialize Acceleration using Acceleration Scale";

        // Can get fancy and use Resources
        //public string InitializeSlowStepperContent { get; set; } = "ViewName_InitializeSlowStepperContent";
        //public string InitializeSlowStepperToolTip { get; set; } = "ViewName_InitializeSlowStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowStepperContent">InitializeSlowStepper</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowStepperContentToolTip">InitializeSlowStepper ToolTip</system:String>  

        //public void InitializeSlowStepper()

        public void InitializeAcceleration(string speed)
        {
            //Int64 startTicks = 0;
            //if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeAcceleration) Enter", Common.LOG_CATEGORY);
            //// TODO(crhodes)
            //// Do something amazing.
            //Message = "Cool, you called InitializeAcceleration";
            //PublishStatusMessage(Message);

            //if ((Boolean)DeviceAttached)
            //{
            //    StepperStepperCollection steppers = ActiveStepper.Stepper.steppers;

            //    try
            //    {
            //        for (int i = 0; i < steppers.Count; i++)
            //        {
            //            StepperProperties[i].InitializeAcceleration(ConvertStringToInitializeMotion(speed));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex, Common.LOG_CATEGORY);
            //    }
            //}

            //// Uncomment this if you are telling someone else to handle this

            //// Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish();

            //// May want EventArgs

            ////  EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish(
            ////      new InitializeSlowStepperEventArgs()
            ////      {
            ////            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            ////            Process = _contextMainViewModel.Context.SelectedProcess
            ////      });

            //// Start Cut Three - Put this in PrismEvents

            //// public class InitializeSlowStepperEvent : PubSubEvent { }

            //// End Cut Three

            //// Start Cut Four - Put this in places that listen for event

            ////Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Subscribe(InitializeSlowStepper);

            //// End Cut Four

            //if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeAcceleration) Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowStepperCanExecute()
        public bool InitializeAccelerationCanExecute(string speed)
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

        #region OpenStepper Command

        public DelegateCommand OpenSteppersCommand { get; set; }
        public string OpenSteppersContent { get; set; } = "Open";
        public string OpenSteppersToolTip { get; set; } = "OpenSteppers ToolTip";

        // Can get fancy and use Resources
        //public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        //public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        //    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        public async void OpenSteppers()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenSteppers) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenStepper";
            PublishStatusMessage(Message);

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedStepper.SerialNumber].DeviceChannels;

            Int32 serialNumber = SelectedStepper.SerialNumber;

            for (int channel = 0; channel < deviceChannels.StepperCount; channel++)
            {
                OpenStepper(channel.ToString());
            }

            DeviceAttached = true;  // To enable Close button

            OpenSteppersCommand.RaiseCanExecuteChanged();
            CloseSteppersCommand.RaiseCanExecuteChanged();

            InitializeVelocityCommand.RaiseCanExecuteChanged();
            InitializeAccelerationCommand.RaiseCanExecuteChanged();

            //ActiveStepper = new StepperEx(
            //    SelectedHost.IPAddress,
            //    SelectedHost.Port,
            //    SelectedStepper.SerialNumber,
            //    EventAggregator);

            //ActiveStepper.Stepper.Attach += ActiveStepper_Attach;
            //ActiveStepper.Stepper.Detach += ActiveStepper_Detach;

            //ActiveStepper.Stepper.CurrentChange += ActiveStepper_CurrentChange;
            //ActiveStepper.Stepper.InputChange += ActiveStepper_InputChange;
            //ActiveStepper.Stepper.PositionChange += ActiveStepper_PositionChange;
            //ActiveStepper.Stepper.VelocityChange += ActiveStepper_VelocityChange;

            //// NOTE(crhodes)
            //// Capture Digital Input and Output changes so we can update the UI
            //// The StepperEx attaches to these events also.
            //// Itlogs the changes if xxx is set to true.

            ////ActiveStepper.OutputChange += ActiveStepper_OutputChange;
            ////ActiveStepper.InputChange += ActiveStepper_InputChange;

            ////// NOTE(crhodes)
            ////// Let's do see if we can watch some analog data stream in.

            ////ActiveStepper.SensorChange += ActiveStepper_SensorChange;

            ////await Task.Run(() => ActiveStepper.Open(Common.PhidgetOpenTimeout));

            //await Task.Run(() => ActiveStepper.Open(Common.PhidgetOpenTimeout));

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenStepperEvent>().Publish(
            //      new OpenStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenStepperEvent>().Subscribe(OpenStepper);

            // End Cut Four

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenSteppers) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenSteppersCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedStepper is not null)
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

        #region OpenStepper Command

        public DelegateCommand<string> OpenStepperCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenStepperHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenStepperCommandParameter;

        public string OpenStepperContent { get; set; } = "Open";
        public string OpenStepperToolTip { get; set; } = "Open Stepper";

        // Can get fancy and use Resources
        //public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        //public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        //    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        void ConfigureInitialLogging(StepperEx stepper)
        {
            stepper.LogPhidgetEvents = LogPhidgetEvents;
            stepper.LogErrorEvents = LogErrorEvents;
            stepper.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            stepper.LogPositionChangeEvents = LogPositionChangeEvents;
            stepper.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            stepper.LogPerformanceSequence = LogPerformanceSequence;
            stepper.LogSequenceAction = LogSequenceAction;
            stepper.LogActionVerification = LogActionVerification;
        }

        public async void OpenStepper(string stepperNumber)
        //public void OpenStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called OpenStepper";
            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedStepper.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(stepperNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                StepperEx stepperHost = PhidgetDeviceLibrary.StepperChannels[serialChannel];

                switch (channel)
                {
                    case 0:

                        if (Stepper0 is null)
                        {
                            // NOTE(crhodes)
                            // Connect the UI to the Control so the UI is bound to the information

                            Stepper0 = PhidgetDeviceLibrary.StepperChannels[serialChannel];

                            // NOTE(crhodes)
                            // If this is the first time the channel is open use the global Logging settings
                            // Can turn off what is not need in Channel UI once open before further interacitons

                            ConfigureInitialLogging(Stepper0);
                        }
                        if (Stepper0.IsOpen is false)
                        {
                            await Task.Run(() => Stepper0.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper0 already open", Common.LOG_CATEGORY);
                        }

                        break;

                    case 1:
                        if (Stepper1 is null)
                        {
                            Stepper1 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                            ConfigureInitialLogging(Stepper1);
                        }
                        if (Stepper1.IsOpen is false)
                        {
                            await Task.Run(() => Stepper1.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper1 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 2:
                        if (Stepper2 is null)
                        {
                            Stepper2 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                            ConfigureInitialLogging(Stepper2);
                        }
                        if (Stepper2.IsOpen is false)
                        {
                            await Task.Run(() => Stepper2.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper2 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 3:
                        if (Stepper3 is null)
                        {
                            Stepper3 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                            ConfigureInitialLogging(Stepper3);
                        }
                        if (Stepper3.IsOpen is false)
                        {
                            await Task.Run(() => Stepper3.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper3 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    //case 4:
                    //    if (Stepper4 is null)
                    //    {
                    //        Stepper4 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                    //        ConfigureInitialLogging(Stepper4);
                    //    }
                    //    if (Stepper4.IsOpen is false)
                    //    {
                    //        await Task.Run(() => Stepper4.Open(500));
                    //    }
                    //    else
                    //    {
                    //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper4 already open", Common.LOG_CATEGORY);
                    //    }
                    //    break;

                    //case 5:
                    //    if (Stepper5 is null)
                    //    {
                    //        Stepper5 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                    //        ConfigureInitialLogging(Stepper5);
                    //    }
                    //    if (Stepper5.IsOpen is false)
                    //    {
                    //        await Task.Run(() => Stepper5.Open(500));
                    //    }
                    //    else
                    //    {
                    //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper5 already open", Common.LOG_CATEGORY);
                    //    }
                    //    break;

                    //case 6:
                    //    if (Stepper6 is null)
                    //    {
                    //        Stepper6 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                    //        ConfigureInitialLogging(Stepper6);
                    //    }
                    //    if (Stepper6.IsOpen is false)
                    //    {
                    //        await Task.Run(() => Stepper6.Open(500));
                    //    }
                    //    else
                    //    {
                    //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper6 already open", Common.LOG_CATEGORY);
                    //    }
                    //    break;

                    //case 7:
                    //    if (Stepper7 is null)
                    //    {
                    //        Stepper7 = PhidgetDeviceLibrary.StepperChannels[serialChannel];
                    //        ConfigureInitialLogging(Stepper7);
                    //    }
                    //    if (Stepper7.IsOpen is false)
                    //    {
                    //        await Task.Run(() => Stepper7.Open(500));
                    //    }
                    //    else
                    //    {
                    //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Stepper7 already open", Common.LOG_CATEGORY);
                    //    }
                    //    break;
                }
                //}

                OpenStepperCommand.RaiseCanExecuteChanged();
                CloseStepperCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Message = $"Cannot parse stepperNumber:>{stepperNumber}<";
                Log.Error(Message, Common.LOG_CATEGORY);
            }

            // If launching a UserControl

            // if (_OpenStepperHost is null) _OpenStepperHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<OpenStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenStepperEvent>().Publish(
            //      new OpenStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenStepperEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenStepperEvent>().Subscribe(OpenStepper);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool OpenStepperCanExecute(string channelNumber)
        //public bool OpenStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(channelNumber, out channel)) throw new Exception($"Cannot parse channelNumber:{channelNumber}");

            if (SelectedStepper is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedStepper.SerialNumber, Channel = channel };

            StepperEx? host;

            if (!PhidgetDeviceLibrary.StepperChannels.TryGetValue(serialChannel, out host)) return false;

            if (host.Attached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region CloseSteppers Command

        public DelegateCommand CloseSteppersCommand { get; set; }
        public string CloseSteppersContent { get; set; } = "Close";
        public string CloseSteppersToolTip { get; set; } = "CloseSteppers ToolTip";

        // Can get fancy and use Resources
        //public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        //public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        //    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

        public async void CloseSteppers()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseStepper) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseSteppers";
            PublishStatusMessage(Message);

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedStepper.SerialNumber].DeviceChannels;

            for (int channel = 0; channel < deviceChannels.RCServoCount; channel++)
            {
                CloseStepper(channel.ToString());
            }

            DeviceAttached = false; // To enable Open button

            OpenSteppersCommand.RaiseCanExecuteChanged();
            CloseSteppersCommand.RaiseCanExecuteChanged();

            InitializeVelocityCommand.RaiseCanExecuteChanged();
            InitializeAccelerationCommand.RaiseCanExecuteChanged();

            //await Task.Run(() => ActiveStepper.Close());

            //DeviceAttached = false;
            //UpdateStepperProperties();
            //ActiveStepper = null;
            //ClearDigitalInputsAndOutputs();

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //RefreshStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

            //InitializeAccelerationCommand.RaiseCanExecuteChanged();
            //InitializeVelocityCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseStepperEvent>().Publish(
            //      new CloseStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseStepperEvent>().Subscribe(CloseStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseStepper) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseSteppersCanExecute()
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

        #region CloseStepper Command

        public DelegateCommand<string> CloseStepperCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseStepperHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseStepperCommandParameter;

        public string CloseStepperContent { get; set; } = "Close";
        public string CloseStepperToolTip { get; set; } = "Close Stepper";

        // Can get fancy and use Resources
        //public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        //public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        //    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseStepper(string servoNumber)
        //public void CloseStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called CloseStepper";
            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedStepper.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(servoNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                await Task.Run(() => PhidgetDeviceLibrary.StepperChannels[serialChannel].Close());
            }
            else
            {
                Message = $"Cannot parse servoNumber:>{servoNumber}<";
                Log.Error(Message, Common.LOG_CATEGORY);
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseStepperHost is null) _CloseStepperHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseStepperEvent>().Publish(
            //      new CloseStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseStepperEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseStepperEvent>().Subscribe(CloseStepper);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool CloseStepperCanExecute(string channelNumber)
        //public bool CloseStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(channelNumber, out channel)) throw new Exception($"Cannot parse channelNumber:{channelNumber}");

            if (SelectedStepper is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedStepper.SerialNumber, Channel = channel };

            StepperEx? host;

            if (!PhidgetDeviceLibrary.StepperChannels.TryGetValue(serialChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        //#region Rotate Command

        // TODO(crhodes)
        // I think this belongs in StepperPositionControl
        // like the stuff in RCServoTargetPositionControl


        // Start Cut Three - Put this in Fields and Properties

        //public DelegateCommand<string> RotateCommand { get; set; }
        //// If using CommandParameter, figure out TYPE here and above
        //// and remove above declaration
        ////public DelegateCommand<TYPE> RotateCommand { get; set; }

        //// End Cut Three

        //// If displaying UserControl
        //// public static WindowHost _RotateHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE RotateCommandParameter;

        //public string RotateContent { get; set; } = "Rotate";
        //public string RotateToolTip { get; set; } = "Rotate ToolTip";

        //// Can get fancy and use Resources
        ////public string RotateContent { get; set; } = "ViewName_RotateContent";
        ////public string RotateToolTip { get; set; } = "ViewName_RotateContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_RotateContent">Rotate</system:String>
        ////    <system:String x:Key="ViewName_RotateContentToolTip">Rotate ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here
        ////public void Rotate(TYPE value)
        //public void Rotate(string direction)
        //{
        //    // FIX(crhodes)
        //    // 
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.

        //    Message = "Cool, you called Rotate";
        //PublishStatusMessage(Message);

        //    PublishStatusMessage(Message);

        //    //var sa = StepperProperties[0].StepAngle;

        //    //Double circle = 360;
        //    //var circleSteps = circle / sa;

        //    //Int64 stepsToMove = (Int64)(Degrees / sa);

        //    //stepsToMove = stepsToMove * 16; // 1/16 steps

        //    //switch (direction)
        //    //{
        //    //    case "CW":
        //    //        StepperProperties[0].TargetPosition += stepsToMove;
        //    //        break;

        //    //    case "CCW":
        //    //        StepperProperties[0].TargetPosition -= stepsToMove;
        //    //        break;

        //    //    default:
        //    //        Log.Error($"Unexpected direction:>{direction}", Common.LOG_CATEGORY);
        //    //        break;
        //    //}

        //    // If launching a UserControl

        //    // if (_RotateHost is null) _RotateHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<RotateEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<RotateEvent>().Publish(
        //    //      new RotateEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class RotateEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<RotateEvent>().Subscribe(Rotate);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        ////public bool RotateCanExecute(TYPE value)
        //public bool RotateCanExecute(string direction)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //}

        //#endregion

        #region ZeroCurrentPosition Command

        // TODO(crhodes)
        // I think this belongs in StepperPositionControl
        // like the stuff in RCServoTargetPositionControl

        //public DelegateCommand ZeroCurrentPositionCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        public DelegateCommand<string> ZeroCurrentPositionCommand { get; set; }

        // If displaying UserControl
        // public static WindowHost _ZeroCurrentPositionHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE ZeroCurrentPositionCommandParameter;

        public string ZeroCurrentPositionContent { get; set; } = "ZeroCurrentPosition";
        public string ZeroCurrentPositionToolTip { get; set; } = "ZeroCurrentPosition ToolTip";

        // Can get fancy and use Resources
        //public string ZeroCurrentPositionContent { get; set; } = "ViewName_ZeroCurrentPositionContent";
        //public string ZeroCurrentPositionToolTip { get; set; } = "ViewName_ZeroCurrentPositionContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ZeroCurrentPositionContent">ZeroCurrentPosition</system:String>
        //    <system:String x:Key="ViewName_ZeroCurrentPositionContentToolTip">ZeroCurrentPosition ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public void ZeroCurrentPosition(string stepperNumber)
        //public void ZeroCurrentPosition()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called ZeroCurrentPosition";
            PublishStatusMessage(Message);

            Int32 channel;

            if (!Int32.TryParse(stepperNumber, out channel)) throw new Exception($"Cannot parse stepperNumber:{stepperNumber}");

            switch (channel)
            {
                case 0:
                    Stepper0?.AddPositionOffset(-Stepper0.Position);
                    break;

                case 1:
                    Stepper1.AddPositionOffset(-Stepper1.Position);
                    break;

                case 2:
                    Stepper2.AddPositionOffset(-Stepper2.Position);
                    break;

                case 3:
                    Stepper3.AddPositionOffset(-Stepper3.Position);
                    break;

                //case 4:
                //    Stepper4.Position = 0;
                //    Stepper4.TargetPosition = 0;
                //    break;

                //case 5:
                //    Stepper5.Position = 0;
                //    Stepper5.TargetPosition = 0;
                //    break;

                //case 6:
                //    Stepper6.Position = 0;
                //    Stepper6.TargetPosition = 0;
                //    break;

                //case 7:
                //    Stepper7.Position = 0;
                //    Stepper7.TargetPosition = 0;
                //    break;
            }

            // If launching a UserControl

            // if (_ZeroCurrentPositionHost is null) _ZeroCurrentPositionHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Publish(
            //      new ZeroCurrentPositionEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class ZeroCurrentPositionEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Subscribe(ZeroCurrentPosition);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool ZeroCurrentPositionCanExecute(string stepperNumber)
        //public bool ZeroCurrentPositionCanExecute()
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

        #region Private Methods (none)


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
