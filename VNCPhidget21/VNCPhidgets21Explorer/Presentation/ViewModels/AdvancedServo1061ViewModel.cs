using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

using Phidgets;
using Phidgets.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgetConfig = VNCPhidget21.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{

    public partial class AdvancedServo1061ViewModel 
        : EventViewModelBase, IAdvancedServo1061ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public AdvancedServo1061ViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }


        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // Turn on logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = true;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);
            //PerformanceFileName_DoubleClick_Command = new DelegateCommand(PerformanceFileName_DoubleClick);

            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            RefreshAdvancedServoCommand = new DelegateCommand(RefreshAdvancedServo, RefreshAdvancedServoCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            InitializeVelocityCommand = new DelegateCommand<string>(InitializeVelocity, InitializeVelocityCanExecute);
            InitializeAccelerationCommand = new DelegateCommand<string>(InitializeAcceleration, InitializeAccelerationCanExecute);
            //InitializeMediumAdvancedServoCommand = new DelegateCommand(InitializeMediumAdvancedServo, InitializeMediumAdvancedServoCanExecute);
            //InitializeFastAdvancedServoCommand = new DelegateCommand(InitializeFastAdvancedServo, InitializeFastAdvancedServoCanExecute);


            //ConfigureServoCommand = new DelegateCommand(ConfigureServo, ConfigureServoCanExecute);

            ConfigureServo2Command = new DelegateCommand<string>(ConfigureServo2, ConfigureServo2CanExecute);

            SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            //PlayPerformanceCommand = new DelegateCommand<string>(PlayPerformance, PlayPerformanceCanExecute);
            //PlaySequenceCommand = new DelegateCommand<string>(PlaySequence, PlaySequenceCanExecute);

            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNCPhidget21.Configuration

            HostConfigFileName = "hostconfig.json";
            //PerformanceConfigFileName = "advancedservoperformancesconfig.json";

            LoadUIConfig();
            //LoadPerformancesConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);

            Message = "AdvancedServo1061ViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig = 
                JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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

        private string _performanceConfigFileName;
        public string PerformanceConfigFileName
        {
            get => _performanceConfigFileName;
            set
            {
                if (_performanceConfigFileName == value) return;
                _performanceConfigFileName = value;
                OnPropertyChanged();
            }
        }

        public string PerformanceFileNameToolTip { get; set; } = "DoubleClick to select new file";

        //private VNCPhidgetConfig.HostConfig _hostConfig;
        //public VNCPhidgetConfig.HostConfig HostConfig
        //{
        //    get => _hostConfig;
        //    set
        //    {
        //        if (_hostConfig == value)
        //            return;
        //        _hostConfig = value;
        //        OnPropertyChanged();
        //    }
        //}

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
                AdvancedServos = _selectedHost.AdvancedServos.ToList<VNCPhidgetConfig.AdvancedServo>();
                OnPropertyChanged();
            }
        }

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

                if (ActiveAdvancedServo is not null)
                {                   
                    ActiveAdvancedServo.LogPhidgetEvents = value;

                    // NOTE(crhodes)
                    // There is some logging in ServoProperties that is handled separate
                    // from the logging in AdvancedServoEx and PhidgetEx

                    for (int i = 0; i < 8; i++)
                    {
                        AdvancedServoProperties[i].LogPhidgetEvents = value;
                    }
                }
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

                if (ActiveAdvancedServo is not null)
                {
                    ActiveAdvancedServo.LogCurrentChangeEvents = _logCurrentChangeEvents;
                }
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

                if (ActiveAdvancedServo is not null)
                {
                    ActiveAdvancedServo.LogPositionChangeEvents = _logPositionChangeEvents;
                }
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

                if (ActiveAdvancedServo is not null)
                {
                    ActiveAdvancedServo.LogVelocityChangeEvents = _logVelocityChangeEvents;
                }
            }
        }

        //private bool _logPerformanceStep = false;
        //public bool LogPerformanceStep
        //{
        //    get => _logPerformanceStep;
        //    set
        //    {
        //        if (_logPerformanceStep == value)
        //            return;
        //        _logPerformanceStep = value;
        //        OnPropertyChanged();
        //    }
        //}

        #region AdvancedServo Properties

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

        //public Phidgets.ServoServo.ServoType ServoTypeEnum;

        private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServoTypes;
        public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServoTypes
        {
            get
            {
                if (null == _AdvancedServoTypes)
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

                return _AdvancedServoTypes;
            }

            set
            {
                _AdvancedServoTypes = value;
                OnPropertyChanged();
            }
        }

        //private ServoServo.ServoType _servoTypeEnum;
        //public ServoServo.ServoType ServoTypeEnum
        //{
        //    get => _servoTypeEnum;
        //    set
        //    {
        //        if (_servoTypeEnum == value)
        //        {
        //            return;
        //        }

        //        _servoTypeEnum = value;
        //        OnPropertyChanged();
        //    }
        //}

        private VNCPhidgetConfig.AdvancedServo _selectedAdvancedServo;
        public VNCPhidgetConfig.AdvancedServo SelectedAdvancedServo
        {
            get => _selectedAdvancedServo;
            set
            {
                if (_selectedAdvancedServo == value)
                    return;
                _selectedAdvancedServo = value;

                OpenAdvancedServoCommand.RaiseCanExecuteChanged();
                //PlayPerformanceCommand.RaiseCanExecuteChanged();
                //PlaySequenceCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private AdvancedServoEx _activeAdvancedServo;
        public AdvancedServoEx ActiveAdvancedServo
        {
            get => _activeAdvancedServo;
            set
            {
                if (_activeAdvancedServo == value)
                    return;
                _activeAdvancedServo = value;

                if (_activeAdvancedServo is not null)
                {
                    PhidgetDevice = _activeAdvancedServo.AdvancedServo;
                }
                else
                {
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private int? _servoCount;
        public int? ServoCount
        {
            get => _servoCount;
            set
            {
                if (_servoCount == value)
                    return;
                _servoCount = value;
                OnPropertyChanged();
            }
        }

        private ServoProperties[] _advancedServoProperties = new ServoProperties[8]
        {
            new ServoProperties() { ServoIndex = 0 },
            new ServoProperties() { ServoIndex = 1 },
            new ServoProperties() { ServoIndex = 2 },
            new ServoProperties() { ServoIndex = 3 },
            new ServoProperties() { ServoIndex = 4 },
            new ServoProperties() { ServoIndex = 5 },
            new ServoProperties() { ServoIndex = 6 },
            new ServoProperties() { ServoIndex = 7 },
        };

        public ServoProperties[] AdvancedServoProperties
        {
            get => _advancedServoProperties;
            set
            {
                if (_advancedServoProperties == value)
                    return;
                _advancedServoProperties = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand SayHelloCommand { get; private set; }

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }
        //public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
        }

        //private void PerformanceFileName_DoubleClick()
        //{
        //    Message = "PerformanceFileName_DoubleClick";

        //    LoadPerformancesConfig();
        //}

        #endregion

        #region OpenAdvancedServo Command

        public DelegateCommand OpenAdvancedServoCommand { get; set; }
        public string OpenAdvancedServoContent { get; set; } = "Open";
        public string OpenAdvancedServoToolTip { get; set; } = "OpenAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        //public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        public void OpenAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAdvancedServo";

            ActiveAdvancedServo = new AdvancedServoEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedAdvancedServo.SerialNumber,
                EventAggregator);

            ActiveAdvancedServo.AdvancedServo.Attach += ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach += ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.AdvancedServo.CurrentChange += ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange += ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange += ActiveAdvancedServo_VelocityChange;

            ActiveAdvancedServo.LogPhidgetEvents = LogPhidgetEvents;

            ActiveAdvancedServo.Open(Common.PhidgetOpenTimeout);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish(
            //      new OpenAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Subscribe(OpenAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveAdvancedServo_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            var senderType = sender.GetType();

            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var velocity = e.Velocity;

            //if (LogVelocityChangeEvents)
            //{
            //    Log.Trace($"VelocityChange index:{index} velocity:{velocity} position:{servo.servos[index].Position}", Common.LOG_CATEGORY);
            //}

            AdvancedServoProperties[e.Index].Velocity = e.Velocity;
        }

        private void ActiveAdvancedServo_PositionChange(object sender, PositionChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var position = e.Position;

            //if (LogPositionChangeEvents)
            //{
            //    Log.Trace($"PositionChange index:{index} position:{position} velocity:{servo.servos[index].Velocity}", Common.LOG_CATEGORY);
            //}

            AdvancedServoProperties[e.Index].Position = e.Position;
            AdvancedServoProperties[e.Index].Stopped = servo.servos[e.Index].Stopped;
        }

        private void ActiveAdvancedServo_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            Phidgets.AdvancedServo servo = sender as Phidgets.AdvancedServo;
            var index = e.Index;
            var current = e.Current;

            AdvancedServoProperties[e.Index].Current = e.Current;
        }

        public bool OpenAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedAdvancedServo is not null)
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

        #region InitializeVelocityCommand

        //public DelegateCommand InitializeVelocityCommand { get; set; }
        public DelegateCommand<string> InitializeVelocityCommand { get; set; }
        public string InitializeVelocityContent { get; set; } = "Initilize Velocity";
        public string InitializeVelocityToolTip { get; set; } = "Initialize Velocity using Velocity Scale";

        // Can get fancy and use Resources
        //public string InitializeSlowAdvancedServoContent { get; set; } = "ViewName_InitializeSlowAdvancedServoContent";
        //public string InitializeSlowAdvancedServoToolTip { get; set; } = "ViewName_InitializeSlowAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContent">InitializeSlowAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContentToolTip">InitializeSlowAdvancedServo ToolTip</system:String>  

        //public void InitializeSlowAdvancedServo()

        public void InitializeVelocity(string speed)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeSlowAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                try
                {
                    for (int i = 0; i < servos.Count; i++)
                    {
                        AdvancedServoProperties[i].InitializeVelocity(ConvertStringToInitializeMotion(speed));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish(
            //      new InitializeSlowAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeSlowAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Subscribe(InitializeSlowAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowAdvancedServoCanExecute()
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
        //public string InitializeSlowAdvancedServoContent { get; set; } = "ViewName_InitializeSlowAdvancedServoContent";
        //public string InitializeSlowAdvancedServoToolTip { get; set; } = "ViewName_InitializeSlowAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContent">InitializeSlowAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContentToolTip">InitializeSlowAdvancedServo ToolTip</system:String>  

        //public void InitializeSlowAdvancedServo()

        public void InitializeAcceleration(string speed)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeAcceleration";

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                try
                {
                    for (int i = 0; i < servos.Count; i++)
                    {
                        AdvancedServoProperties[i].InitializeAcceleration(ConvertStringToInitializeMotion(speed));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish(
            //      new InitializeSlowAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeSlowAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Subscribe(InitializeSlowAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowAdvancedServoCanExecute()
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

        #region InitializeMediumAdvancedServo Command

        public DelegateCommand InitializeMediumAdvancedServoCommand { get; set; }
        public string InitializeMediumAdvancedServoContent { get; set; } = "Medium";
        public string InitializeMediumAdvancedServoToolTip { get; set; } = "Initialize Medium Speed and Center Position";

        // Can get fancy and use Resources
        //public string InitializeMediumAdvancedServoContent { get; set; } = "ViewName_InitializeMediumAdvancedServoContent";
        //public string InitializeMediumAdvancedServoToolTip { get; set; } = "ViewName_InitializeMediumAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeMediumAdvancedServoContent">InitializeMediumAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_InitializeMediumAdvancedServoContentToolTip">InitializeMediumAdvancedServo ToolTip</system:String>  

        public void InitializeMediumAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeMediumAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                try
                {
                    for (int i = 0; i < servos.Count; i++)
                    {
                        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Percent50);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Publish(
            //      new InitializeMediumAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeMediumAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Subscribe(InitializeMediumAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }


        public bool InitializeMediumAdvancedServoCanExecute()
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

        #region InitializeFastAdvancedServo Command

        public DelegateCommand InitializeFastAdvancedServoCommand { get; set; }
        public string InitializeFastAdvancedServoContent { get; set; } = "Fast";
        public string InitializeFastAdvancedServoToolTip { get; set; } = "Initialize Fast Speed and Center Position";

        // Can get fancy and use Resources
        //public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        //public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        public void InitializeFastAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you calledInitializeFastAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                try
                {
                    for (int i = 0; i < servos.Count; i++)
                    {
                        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Max);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish(
            //      new OpenAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Subscribe(OpenAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool InitializeFastAdvancedServoCanExecute()
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

        #region RefreshAdvancedServo Command

        public DelegateCommand RefreshAdvancedServoCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> RefreshAdvancedServoCommand { get; set; }
        //public TYPE RefreshAdvancedServoCommandParameter;
        public string RefreshAdvancedServoContent { get; set; } = "Refresh";
        public string RefreshAdvancedServoToolTip { get; set; } = "Refresh ToolTip";

        // Can get fancy and use Resources
        //public string RefreshAdvancedServoContent { get; set; } = "ViewName_RefreshAdvancedServoContent";
        //public string RefreshAdvancedServoToolTip { get; set; } = "ViewName_RefreshAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RefreshAdvancedServoContent">RefreshAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_RefreshAdvancedServoContentToolTip">RefreshAdvancedServo ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void RefreshAdvancedServo(TYPE value)
        public void RefreshAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called RefreshAdvancedServo";

            RefreshAdvancedServoUIProperties();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish(
            //      new RefreshAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class RefreshAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Subscribe(RefreshAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool RefreshAdvancedServoCanExecute(TYPE value)
        public bool RefreshAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region SetAdvancedServoDefaults Command

        //public DelegateCommand<string> SetAdvancedServoDefaultsCommand { get; set; }
        ////public string SetAdvancedServoDefaultsCommandParameter;
        //public string SetAdvancedServoDefaultsContent { get; set; } = "SetAdvancedServoDefaults";
        //public string SetAdvancedServoDefaultsToolTip { get; set; } = "SetAdvancedServoDefaults ToolTip";

        //// Can get fancy and use Resources
        ////public string SetAdvancedServoDefaultsContent { get; set; } = "ViewName_SetAdvancedServoDefaultsContent";
        ////public string SetAdvancedServoDefaultsToolTip { get; set; } = "ViewName_SetAdvancedServoDefaultsContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_SetAdvancedServoDefaultsContent">SetAdvancedServoDefaults</system:String>
        ////    <system:String x:Key="ViewName_SetAdvancedServoDefaultsContentToolTip">SetAdvancedServoDefaults ToolTip</system:String>  

        //public void SetAdvancedServoDefaults(string servoID)
        //{
        //    Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = $"Cool, you called SetAdvancedServoDefaults from servo {servoID}";

        //    AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
        //    Phidgets.AdvancedServoServo servo = null;

        //    Int32 servoIndex = Int32.Parse(servoID);
        //    servo = servos[servoIndex];

        //    // NOTE(crhodes)
        //    // Should be safe to get Acceleration, Velocity, and Position here
        //    // Device is Engaged

        //    Double? halfRange;
        //    Double? percent = .20;
        //    Double? midPoint;

        //    try
        //    {
        //        for (int i = 0; i < ActiveAdvancedServo.AdvancedServo.servos.Count; i++)
        //        {
        //            AdvancedServoProperties[i].Acceleration = AdvancedServoProperties[i].AccelerationMin;
        //            AdvancedServoProperties[i].VelocityLimit = AdvancedServoProperties[i].VelocityMin == 0
        //                ? 10 : 
        //                AdvancedServoProperties[i].VelocityMin;

        //            midPoint = (AdvancedServoProperties[i].DevicePositionMax - AdvancedServoProperties[i].DevicePositionMin) / 2;
        //            halfRange = midPoint * percent;
        //            AdvancedServoProperties[i].PositionMin = midPoint - halfRange;
        //            AdvancedServoProperties[i].PositionMax = midPoint + halfRange;
        //            AdvancedServoProperties[i].Position = midPoint;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Publish(
        //    //      new SetAdvancedServoDefaultsEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class SetAdvancedServoDefaultsEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<SetAdvancedServoDefaultsEvent>().Subscribe(SetAdvancedServoDefaults);

        //    // End Cut Four

        //    Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public bool SetAdvancedServoDefaultsCanExecute(string value)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        #endregion

        #region CloseAdvancedServo Command

        public DelegateCommand CloseAdvancedServoCommand { get; set; }
        public string CloseAdvancedServoContent { get; set; } = "Close";
        public string CloseAdvancedServoToolTip { get; set; } = "CloseAdvancedServo ToolTip";

        // Can get fancy and use Resources
        //public string CloseAdvancedServoContent { get; set; } = "ViewName_CloseAdvancedServoContent";
        //public string CloseAdvancedServoToolTip { get; set; } = "ViewName_CloseAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseAdvancedServoContent">CloseAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_CloseAdvancedServoContentToolTip">CloseAdvancedServo ToolTip</system:String>  

        public void CloseAdvancedServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAdvancedServo";

            DisEngageAllServos();

            // NOTE(crhodes)
            // May need to give device chance to respond

            ActiveAdvancedServo.AdvancedServo.Attach -= ActiveAdvancedServo_Attach;
            ActiveAdvancedServo.AdvancedServo.Detach -= ActiveAdvancedServo_Detach;

            ActiveAdvancedServo.AdvancedServo.CurrentChange -= ActiveAdvancedServo_CurrentChange;
            ActiveAdvancedServo.AdvancedServo.PositionChange -= ActiveAdvancedServo_PositionChange;
            ActiveAdvancedServo.AdvancedServo.VelocityChange -= ActiveAdvancedServo_VelocityChange;
 
            ActiveAdvancedServo.Close();

            DeviceAttached = false;
            UpdateAdvancedServoProperties();

            ActiveAdvancedServo = null;
            //ClearDigitalInputsAndOutputs();

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            RefreshAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            InitializeVelocityCommand.RaiseCanExecuteChanged();
            InitializeAccelerationCommand.RaiseCanExecuteChanged();
            //InitializeMediumAdvancedServoCommand.RaiseCanExecuteChanged();
            //InitializeFastAdvancedServoCommand.RaiseCanExecuteChanged();

            //SetAdvancedServoDefaultsCommand.RaiseCanExecuteChanged();
            SetPositionRangeCommand.RaiseCanExecuteChanged();

            //PlayPerformanceCommand.RaiseCanExecuteChanged();
            //PlaySequenceCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseAdvancedServoEvent>().Publish(
            //      new CloseAdvancedServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseAdvancedServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseAdvancedServoEvent>().Subscribe(CloseAdvancedServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void DisEngageAllServos()
        {
            for (int i = 0; i < ServoCount; i++)
            {
                var advancedServo = ActiveAdvancedServo.AdvancedServo;
                advancedServo.servos[i].Engaged = false;
            }
        }

        public bool CloseAdvancedServoCanExecute()
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

        #region ConfigureServo Command

        public DelegateCommand ConfigureServoCommand { get; set; }
        public string ConfigureServoContent { get; set; } = "ConfigureServo";
        public string ConfigureServoToolTip { get; set; } = "ConfigureServo ToolTip";

        // Can get fancy and use Resources
        //public string ConfigureServoContent { get; set; } = "ViewName_ConfigureServoContent";
        //public string ConfigureServoToolTip { get; set; } = "ViewName_ConfigureServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ConfigureServoContent">ConfigureServo</system:String>
        //    <system:String x:Key="ViewName_ConfigureServoContentToolTip">ConfigureServo ToolTip</system:String>  

        public void ConfigureServo()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called ConfigureServo";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<ConfigureServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ConfigureServoEvent>().Publish(
            //      new ConfigureServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class ConfigureServoEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ConfigureServoEvent>().Subscribe(ConfigureServo);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool ConfigureServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region SetPositionRange Command

        public DelegateCommand<string> SetPositionRangeCommand { get; set; }
        //public TYPE SetPositionRangeCommandParameter;
        public string SetPositionRangeContent { get; set; } = "SetPositionRange";
        public string SetPositionRangeToolTip { get; set; } = "SetPositionRange ToolTip";

        // Can get fancy and use Resources
        //public string SetPositionRangeContent { get; set; } = "ViewName_SetPositionRangeContent";
        //public string SetPositionRangeToolTip { get; set; } = "ViewName_SetPositionRangeContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_SetPositionRangeContent">SetPositionRange</system:String>
        //    <system:String x:Key="ViewName_SetPositionRangeContentToolTip">SetPositionRange ToolTip</system:String>  

        public void SetPositionRange(string servoID)
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called SetPositionRange";

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<SetPositionRangeEvent>().Publish(
            //      new SetPositionRangeEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class SetPositionRangeEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Subscribe(SetPositionRange);

            // End Cut Four


            AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
            Phidgets.AdvancedServoServo servo = null;

            Int32 servoIndex = Int32.Parse(servoID);
            servo = servos[servoIndex];

            // NOTE(crhodes)
            // Should be safe to get Acceleration, Velocity, and Position here
            // Device is Engaged

            Double? halfRange;
            Double? tenPercent;

            try
            {
                AdvancedServoProperties[servoIndex].PositionMin = 
                    AdvancedServoProperties[servoIndex].Position - AdvancedServoProperties[servoIndex].PositionRange;

                AdvancedServoProperties[servoIndex].PositionMax =
                    AdvancedServoProperties[servoIndex].Position + AdvancedServoProperties[servoIndex].PositionRange;

                //switch (servoIndex)
                //{

                //    case 0:
                //        // TODO(crhodes)
                //        // Make this fancier.  Take the smaller of difference
                //        // between Min and Current and Max and Current
                //        // and then divide that by 10

                //        AdvancedServoProperties[0].PositionMin = AdvancedServoProperties[0].Position - AdvancedServoProperties[0].PositionRange;

                //        AdvancedServoProperties[0].PositionMax = AdvancedServoProperties[0].Position + AdvancedServoProperties[0].PositionRange;

                //        //PositionMin_S0 = Position_S0 - PositionRange_S0;

                //        //PositionMax_S0 = Position_S0 + PositionRange_S0;

                //        break;

                //    case 1:
                //        PositionMin_S1 = Position_S1 - PositionRange_S1;

                //        PositionMax_S1 = Position_S1 + PositionRange_S1;

                //        break;

                //    case 2:
                //        PositionMin_S2 = Position_S2 - PositionRange_S2;

                //        PositionMax_S2 = Position_S2 + PositionRange_S2;

                //        break;

                //    case 3:
                //        PositionMin_S3 = Position_S3 - PositionRange_S3;

                //        PositionMax_S3 = Position_S3 + PositionRange_S3;

                //        break;

                //    case 4:
                //        PositionMin_S4 = Position_S4 - PositionRange_S4;

                //        PositionMax_S4 = Position_S4 + PositionRange_S4;

                //        break;

                //    case 5:
                //        PositionMin_S5 = Position_S5 - PositionRange_S5;

                //        PositionMax_S5 = Position_S5 + PositionRange_S5;

                //        break;

                //    case 6:
                //        PositionMin_S6 = Position_S6 - PositionRange_S6;

                //        PositionMax_S6 = Position_S6 + PositionRange_S6;

                //        break;

                //    case 7:
                //        PositionMin_S7 = Position_S7 - PositionRange_S7;

                //        PositionMax_S7 = Position_S7 + PositionRange_S7;

                //        break;

                //    default:
                //        Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                //        break;

                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool SetPositionRangeCanExecute(string value)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region ConfigureServo2 Command

        //public DelegateCommand ConfigureServo2Command { get; set; }
        public DelegateCommand<string> ConfigureServo2Command { get; set; }
        // If using CommandParameter, figure out TYPE and use second above
        //public DelegateCommand<TYPE> ConfigureServo2CommandParameter;
        public string ConfigureServo2Content { get; set; } = "ConfigureServo2";
        public string ConfigureServo2ToolTip { get; set; } = "ConfigureServo2 ToolTip";

        // Can get fancy and use Resources
        //public string ConfigureServo2Content { get; set; } = "ViewName_ConfigureServo2Content";
        //public string ConfigureServo2ToolTip { get; set; } = "ViewName_ConfigureServo2ContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ConfigureServo2Content">ConfigureServo2</system:String>
        //    <system:String x:Key="ViewName_ConfigureServo2ContentToolTip">ConfigureServo2 ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        public void ConfigureServo2(string value)
        //public void ConfigureServo2()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called ConfigureServo2 and passed: {value}";

            Int32 servoIndex = Int32.Parse(value);

            AdvancedServoServo servo = ActiveAdvancedServo.AdvancedServo.servos[servoIndex];

            try
            {
                servo.setServoParameters(
                    AdvancedServoProperties[0].MinimumPulseWidth,
                    AdvancedServoProperties[0].MaximumPulseWidth, 
                    AdvancedServoProperties[0].Degrees, 
                    (Double)AdvancedServoProperties[0].VelocityMax);
                //servo.setServoParameters(MinimumPulseWidth_S0, MaximumPulseWidth_S0, Degrees_S0, (Double)VelocityMax_S0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<ConfigureServo2Event>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ConfigureServo2Event>().Publish(
            //      new ConfigureServo2EventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class ConfigureServo2Event : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ConfigureServo2Event>().Subscribe(ConfigureServo2);

            // End Cut Four

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool ConfigureServo2CanExecute(string value)
        //public bool ConfigureServo2CanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion

        #region Event Handlers

        private void ActiveAdvancedServo_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                //Log.Trace($"ActiveAdvancedServo_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                
                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;

                AdvancedServoProperties[0].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[1].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[2].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[3].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[4].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[5].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[6].AdvancedServoEx = ActiveAdvancedServo;
                AdvancedServoProperties[7].AdvancedServoEx = ActiveAdvancedServo;

                UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            RefreshAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            InitializeVelocityCommand.RaiseCanExecuteChanged();
            InitializeAccelerationCommand.RaiseCanExecuteChanged();

            //SetAdvancedServoDefaultsCommand.RaiseCanExecuteChanged();
            SetPositionRangeCommand.RaiseCanExecuteChanged();
        }

        private void UpdateAdvancedServoProperties()
        {
            Int64 startTicks = Log.Trace($"Enter deviceAttached:{DeviceAttached}", Common.LOG_CATEGORY);

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
                Phidgets.AdvancedServoServo servo = null;

                ServoCount = servos.Count;

                try
                {
                    for (int i = 0; i < ServoCount; i++)
                    {
                        // NOTE(crhodes)
                        // All the work is now done in Type.UpdateProperties()
                        AdvancedServoProperties[i].LogPhidgetEvents = LogPhidgetEvents;
                        AdvancedServoProperties[i].ServoType = servos[i].Type;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
            else
            {
                DeviceAttached = null;
                InitializAdvancedServoUI();
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void RefreshAdvancedServoUIProperties()
        {
            Int64 startTicks = Log.Trace($"Enter deviceAttached:{DeviceAttached}", Common.LOG_CATEGORY);

            if ((Boolean)DeviceAttached)
            {
                AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
                Phidgets.AdvancedServoServo servo = null;

                ServoCount = servos.Count;

                try
                {
                    for (int i = 0; i < ServoCount; i++)
                    {
                        AdvancedServoProperties[i].RefreshPropertiesFromServo();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
            else
            {
                DeviceAttached = null;
                InitializAdvancedServoUI();
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveAdvancedServo_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;

                Log.Trace($"ActiveAdvancedServo_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = ActiveAdvancedServo.AdvancedServo.Attached;

                for (int i = 0; i < ServoCount; i++)
                {
                    AdvancedServoProperties[i].InitializeProperties();
                    AdvancedServoProperties[i].AdvancedServoEx = null;
                }

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveAdvancedServo to null.  Clear UI
                //UpdateAdvancedServoProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        private bool SayHelloCanExecute()
        {
            return true;
        }

        private void InitializAdvancedServoUI()
        {
            for (int i = 0; i < 8; i++)
            {
                AdvancedServoProperties[i].InitializeProperties();
            }         
        }

        private ServoProperties.MotionScale ConvertStringToInitializeMotion(string speed)
        {
            ServoProperties.MotionScale result = ServoProperties.MotionScale.Percent05;

            switch (speed)
            {
                case "Min":
                    result = ServoProperties.MotionScale.Min;
                    break;

                case "05%":
                    result = ServoProperties.MotionScale.Percent05;
                    break;

                case "10%":
                    result = ServoProperties.MotionScale.Percent10;
                    break;

                case "15%":
                    result = ServoProperties.MotionScale.Percent15;
                    break;

                case "20%":
                    result = ServoProperties.MotionScale.Percent20;
                    break;

                case "25%":
                    result = ServoProperties.MotionScale.Percent25;
                    break;

                case "35%":
                    result = ServoProperties.MotionScale.Percent35;
                    break;

                case "50%":
                    result = ServoProperties.MotionScale.Percent50;
                    break;

                case "75%":
                    result = ServoProperties.MotionScale.Percent75;
                    break;

                case "Max":
                    result = ServoProperties.MotionScale.Max;
                    break;

                default:
                    Log.Error($"Unexpected speed:{speed}", Common.LOG_CATEGORY);
                    break;
            }

            return result;
        }

        #endregion

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
