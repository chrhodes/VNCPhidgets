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

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public partial class AdvancedServo1061ViewModel 
        : EventViewModelBase, IAdvancedServo1061ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public AdvancedServo1061ViewModel(
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

            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            InitializeVelocityCommand = new DelegateCommand<string>(InitializeVelocity, InitializeVelocityCanExecute);
            InitializeAccelerationCommand = new DelegateCommand<string>(InitializeAcceleration, InitializeAccelerationCanExecute);

            //ConfigureServoCommand = new DelegateCommand(ConfigureServo, ConfigureServoCanExecute);

            ConfigureServo2Command = new DelegateCommand<string>(ConfigureServo2, ConfigureServo2CanExecute);

            SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            OpenRCServoCommand = new DelegateCommand<string>(OpenRCServo, OpenRCServoCanExecute);
            CloseRCServoCommand = new DelegateCommand<string>(CloseRCServo, CloseRCServoCanExecute);


            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNCPhidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            Message = "AdvancedServo1061ViewModel says hello";

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
                AdvancedServos = _selectedHost.AdvancedServos?.ToList<VNCPhidgetConfig.AdvancedServo>();
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

        private bool? _rcServolAttached;
        public bool? RCServoAttached
        {
            get => _rcServolAttached;
            set
            {
                _rcServolAttached = value;
                OpenRCServoCommand.RaiseCanExecuteChanged();
                CloseRCServoCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        #endregion

        #region AdvancedServo

        #region RCServos

        RCServoEx[] _rcServos = new RCServoEx[16];
        public RCServoEx[] RCServos
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

        RCServoEx _rcServo0;
        public RCServoEx RCServo0
        {
            get
            {
                return _rcServo0;
            }
            set
            {
                _rcServo0 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo1;
        public RCServoEx RCServo1
        {
            get
            {
                return _rcServo1;
            }
            set
            {
                _rcServo1 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo2;
        public RCServoEx RCServo2
        {
            get
            {
                return _rcServo2;
            }
            set
            {
                _rcServo2 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo3;
        public RCServoEx RCServo3
        {
            get
            {
                return _rcServo3;
            }
            set
            {
                _rcServo3 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo4;
        public RCServoEx RCServo4
        {
            get
            {
                return _rcServo4;
            }
            set
            {
                _rcServo4 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo5;
        public RCServoEx RCServo5
        {
            get
            {
                return _rcServo5;
            }
            set
            {
                _rcServo5 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo6;
        public RCServoEx RCServo6
        {
            get
            {
                return _rcServo6;
            }
            set
            {
                _rcServo6 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo7;
        public RCServoEx RCServo7
        {
            get
            {
                return _rcServo7;
            }
            set
            {
                _rcServo7 = value;
                OnPropertyChanged();
            }
        }

        private Visibility _rcServosVisibility = Visibility.Collapsed;
        public Visibility RCServosVisibility
        {
            get => _rcServosVisibility;
            set
            {
                if (_rcServosVisibility == value)
                    return;
                _rcServosVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region AdvancedServo Events

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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogCurrentChangeEvents = _logCurrentChangeEvents;
                //}
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogPositionChangeEvents = _logPositionChangeEvents;
                //}
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogVelocityChangeEvents = _logVelocityChangeEvents;
                //}
            }
        }

        private bool _logTargetPositionReachedEvents = false;
        public bool LogTargetPositionReachedEvents
        {
            get => _logTargetPositionReachedEvents;
            set
            {
                if (_logTargetPositionReachedEvents == value)
                    return;
                _logTargetPositionReachedEvents = value;
                OnPropertyChanged();

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogTargetPositionReachedEvents = _logTargetPositionReachedEvents;
                //}
            }
        }

        #endregion

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

        //private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServoTypes;
        //public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServoTypes
        //{
        //    get
        //    {
        //        if (null == _AdvancedServoTypes)
        //        {
        //            // TODO(crhodes)
        //            // Load this like the sensors.xml for now

        //            //_InterfaceKits =
        //            //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
        //            //    select new InterfaceKit(
        //            //        item.Attribute("Name").Value,
        //            //        item.Attribute("IPAddress").Value,
        //            //        item.Attribute("Port").Value,
        //            //        bool.Parse(item.Attribute("Enable").Value)
        //            //        );
        //        }

        //        return _AdvancedServoTypes;
        //    }

        //    set
        //    {
        //        _AdvancedServoTypes = value;
        //        OnPropertyChanged();
        //    }
        //}

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
                OpenRCServoCommand.RaiseCanExecuteChanged();
                //PlayPerformanceCommand.RaiseCanExecuteChanged();
                //PlaySequenceCommand.RaiseCanExecuteChanged();

                // Set to null when host changes
                if (value is not null)
                {
                    DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[value.SerialNumber].DeviceChannels;

                    RCServosVisibility = deviceChannels.RCServoCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    RCServosVisibility = Visibility.Collapsed;
                }

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

                //if (_activeAdvancedServo is not null)
                //{
                //    PhidgetDevice = _activeAdvancedServo.AdvancedServo;
                //}
                //else
                //{
                //    PhidgetDevice = null;
                //}

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

        // TODO(crhodes)
        // Need to drive this off _deviceChannels.RCServoCount

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

        #region RCServos

        #region RCServo0

        #endregion

        #endregion

        #region CurrentInputs

        #region CurrentInput0

        #endregion

        #endregion

        #endregion

        #endregion

        #region Event Handlers

        #region RCServo0

        //void RCServo0_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_PositionChange(object sender, PhidgetsEvents.RCServoPositionChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_PositionChange: sender:{sender} position:{e.Position}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_TargetPositionReached(object sender, PhidgetsEvents.RCServoTargetPositionReachedEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_TargetPositionReached: sender:{sender} position:{e.Position}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void RCServo0_VelocityChange(object sender, PhidgetsEvents.RCServoVelocityChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"RCServo0_VelocityChangee: sender:{sender} velocity:{e.Velocity}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        #endregion

        #endregion

        #region Commands

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }
        //public DelegateCommand PerformanceFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Enter", Common.LOG_CATEGORY);

            Message = "ConfigFileName_DoubleClick";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Exit", Common.LOG_CATEGORY, startTicks);
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

        public async void OpenAdvancedServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAdvancedServo";

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedAdvancedServo.SerialNumber].DeviceChannels;

            Int32 serialNumber = SelectedAdvancedServo.SerialNumber;

            for (int channel = 0; channel < deviceChannels.RCServoCount; channel++)
            {
                OpenRCServo(channel.ToString());
            }

            DeviceAttached = true;  // To enable Close button

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            InitializeVelocityCommand.RaiseCanExecuteChanged();
            InitializeAccelerationCommand.RaiseCanExecuteChanged();

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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeVelocity) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeSlowAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                // FIX(crhodes)
                // 
                //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                //try
                //{
                //    for (int i = 0; i < servos.Count; i++)
                //    {
                //        AdvancedServoProperties[i].InitializeVelocity(ConvertStringToInitializeMotion(speed));
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error(ex, Common.LOG_CATEGORY);
                //}
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeVelocity) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeAcceleration) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeAcceleration";

            if ((Boolean)DeviceAttached)
            {
                // FIX(crhodes)
                // 
                //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                //try
                //{
                //    for (int i = 0; i < servos.Count; i++)
                //    {
                //        AdvancedServoProperties[i].InitializeAcceleration(ConvertStringToInitializeMotion(speed));
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error(ex, Common.LOG_CATEGORY);
                //}
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeAcceleration) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeMediumAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeMediumAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                // FIX(crhodes)
                // 
                //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                //try
                //{
                //    for (int i = 0; i < servos.Count; i++)
                //    {
                //        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Percent50);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error(ex, Common.LOG_CATEGORY);
                //}
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeMediumAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeFastAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you calledInitializeFastAdvancedServo";

            if ((Boolean)DeviceAttached)
            {
                // FIX(crhodes)
                // 
                //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

                //try
                //{
                //    for (int i = 0; i < servos.Count; i++)
                //    {
                //        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Max);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error(ex, Common.LOG_CATEGORY);
                //}
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeFastAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
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

        //#region RefreshAdvancedServo Command

        //public DelegateCommand RefreshAdvancedServoCommand { get; set; }
        //// If using CommandParameter, figure out TYPE here and above
        //// and remove above declaration
        ////public DelegateCommand<TYPE> RefreshAdvancedServoCommand { get; set; }
        ////public TYPE RefreshAdvancedServoCommandParameter;
        //public string RefreshAdvancedServoContent { get; set; } = "Refresh";
        //public string RefreshAdvancedServoToolTip { get; set; } = "Refresh ToolTip";

        //// Can get fancy and use Resources
        ////public string RefreshAdvancedServoContent { get; set; } = "ViewName_RefreshAdvancedServoContent";
        ////public string RefreshAdvancedServoToolTip { get; set; } = "ViewName_RefreshAdvancedServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_RefreshAdvancedServoContent">RefreshAdvancedServo</system:String>
        ////    <system:String x:Key="ViewName_RefreshAdvancedServoContentToolTip">RefreshAdvancedServo ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE and fix above
        ////public void RefreshAdvancedServo(TYPE value)
        //public async void RefreshAdvancedServo()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RefreshAdvancedServo) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called RefreshAdvancedServo";

        //    await Task.Run(() => RefreshAdvancedServoUIProperties());
        //    //RefreshAdvancedServoUIProperties();

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Publish(
        //    //      new RefreshAdvancedServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class RefreshAdvancedServoEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<RefreshAdvancedServoEvent>().Subscribe(RefreshAdvancedServo);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RefreshAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        ////public bool RefreshAdvancedServoCanExecute(TYPE value)
        //public bool RefreshAdvancedServoCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        //#endregion

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
        //    Phidget22.AdvancedServoServo servo = null;

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
        public async void CloseAdvancedServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAdvancedServo";

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedAdvancedServo.SerialNumber].DeviceChannels;

            for (int channel = 0; channel < deviceChannels.RCServoCount; channel++)
            {
                CloseRCServo(channel.ToString());
            }

            DeviceAttached = false; // To enable Open button

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void DisEngageAllServos()
        {
            for (int i = 0; i < ServoCount; i++)
            {
                // TODO(crhodes)
                // If we keep this have to use RCServo0..RCServo7
                //RCServos[i].Engaged = false;
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

        #region OpenRCServo Command

        public DelegateCommand<string> OpenRCServoCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenRCServoHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenRCServoCommandParameter;

        public string OpenRCServoContent { get; set; } = "Open";
        public string OpenRCServoToolTip { get; set; } = "Open RCServo";

        // Can get fancy and use Resources
        //public string OpenRCServoContent { get; set; } = "ViewName_OpenRCServoContent";
        //public string OpenRCServoToolTip { get; set; } = "ViewName_OpenRCServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenRCServoContent">OpenRCServo</system:String>
        //    <system:String x:Key="ViewName_OpenRCServoContentToolTip">OpenRCServo ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        void ConfigureInitialLogging(RCServoEx rcServo)
        {
            rcServo.LogPhidgetEvents = LogPhidgetEvents;
            rcServo.LogErrorEvents = LogErrorEvents;
            rcServo.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            rcServo.LogPositionChangeEvents = LogPositionChangeEvents;
            rcServo.LogVelocityChangeEvents = LogVelocityChangeEvents;

            rcServo.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            rcServo.LogPerformanceSequence = LogPerformanceSequence;
            rcServo.LogSequenceAction = LogSequenceAction;
            rcServo.LogActionVerification = LogActionVerification;
        }

        public async void OpenRCServo(string servoNumber)
        //public void OpenRCServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called OpenRCServo";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedAdvancedServo.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(servoNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                RCServoEx rcServoHost = PhidgetDeviceLibrary.RCServoChannels[serialChannel];

                // NOTE(crhodes)
                // If this is the first time the channel is open use the global Logging settings
                // Can turn off what is not need in Channel UI once open before further interacitons

                //if (rcServoHost.IsOpen is false)
                //{
                //    await Task.Run(() => rcServoHost.Open(500));

                // NOTE(crhodes)
                // Connect the UI to the Control so the UI is bound to the information

                // TODO(crhodes)
                // Come back and try to clean this up.  Made a mess trying to debug the can't open twice issue
                // which was TargetPostion not being set (;
                switch (channel)
                {
                    case 0:
                        //RCServo0 = rcServoHost;
                        if (RCServo0 is null)
                        {
                            RCServo0 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo0);
                        }
                        if (RCServo0.IsOpen is false)
                        {
                            await Task.Run(() => RCServo0.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo0 already open", Common.LOG_CATEGORY);
                        }

                        break;

                    case 1:
                        if (RCServo1 is null)
                        {
                            RCServo1 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo1);
                        }
                        if (RCServo1.IsOpen is false)
                        {
                            await Task.Run(() => RCServo1.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo1 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 2:
                        if (RCServo2 is null)
                        {
                            RCServo2 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo2);
                        }
                        if (RCServo2.IsOpen is false)
                        {
                            await Task.Run(() => RCServo2.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo2 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 3:
                        if (RCServo3 is null)
                        {
                            RCServo3 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo3);
                        }
                        if (RCServo3.IsOpen is false)
                        {
                            await Task.Run(() => RCServo3.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo3 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 4:
                        if (RCServo4 is null)
                        {
                            RCServo4 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo4);
                        }
                        if (RCServo4.IsOpen is false)
                        {
                            await Task.Run(() => RCServo4.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo4 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 5:
                        if (RCServo5 is null)
                        {
                            RCServo5 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo5);
                        }
                        if (RCServo5.IsOpen is false)
                        {
                            await Task.Run(() => RCServo5.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo5 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 6:
                        if (RCServo6 is null)
                        {
                            RCServo6 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo6);
                        }
                        if (RCServo6.IsOpen is false)
                        {
                            await Task.Run(() => RCServo6.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo6 already open", Common.LOG_CATEGORY);
                        }
                        break;

                    case 7:
                        if (RCServo7 is null)
                        {
                            RCServo7 = PhidgetDeviceLibrary.RCServoChannels[serialChannel];
                            ConfigureInitialLogging(RCServo7);
                        }
                        if (RCServo7.IsOpen is false)
                        {
                            await Task.Run(() => RCServo7.Open(500));
                        }
                        else
                        {
                            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("RCServo7 already open", Common.LOG_CATEGORY);
                        }
                        break;
                }
                //}

                OpenRCServoCommand.RaiseCanExecuteChanged();
                CloseRCServoCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Message = $"Cannot parse servoNumber:>{servoNumber}<";
                Log.Error(Message, Common.LOG_CATEGORY);
            }

            // If launching a UserControl

            // if (_OpenRCServoHost is null) _OpenRCServoHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<OpenRCServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenRCServoEvent>().Publish(
            //      new OpenRCServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenRCServoEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenRCServoEvent>().Subscribe(OpenRCServo);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool OpenRCServoCanExecute(string servoNumber)
        //public bool OpenRCServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(servoNumber, out channel)) throw new Exception($"Cannot parse servoNumber:{servoNumber}");

            if (SelectedAdvancedServo is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedAdvancedServo.SerialNumber, Channel = channel };

            RCServoEx? rcServoHost;

            if (!PhidgetDeviceLibrary.RCServoChannels.TryGetValue(serialChannel, out rcServoHost)) return false;

            if (rcServoHost.IsAttached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //private RCServoEx GetRCServoHost(int serialNumber, int channel)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

        //    RCServoConfiguration rcServoConfiguration = new RCServoConfiguration()
        //    {
        //        Channel = (Int16)channel,

        //        LogPhidgetEvents = RCServos[channel].LogPhidgetEvents,
        //        LogErrorEvents = RCServos[channel].LogErrorEvents,
        //        LogPropertyChangeEvents = RCServos[channel].LogPropertyChangeEvents,

        //        //rcServoHost.LogCurrentChangeEvents =  RCServos[channel].LogCurrentChangeEvents,
        //        LogPositionChangeEvents = RCServos[channel].LogPositionChangeEvents,
        //        LogVelocityChangeEvents = RCServos[channel].LogVelocityChangeEvents,

        //        LogTargetPositionReachedEvents = RCServos[channel].LogTargetPositionReachedEvents,

        //        LogPerformanceSequence = RCServos[channel].LogPerformanceSequence,
        //        LogSequenceAction = RCServos[channel].LogSequenceAction,
        //        LogActionVerification = RCServos[channel].LogActionVerification
        //    };

        //    //RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.ConfigureRCServoHost(serialNumber, channel, RCServoConfiguration);

        //    // NOTE(crhodes)
        //    // This is a new method that got added.  Maybe problem is here.
        //    RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.OpenRCServoHost(serialNumber, channel, rcServoConfiguration);

        //    //rcServoHost.LogPhidgetEvents = LogPhidgetEvents;
        //    //rcServoHost.LogErrorEvents = LogErrorEvents;
        //    //rcServoHost.LogPropertyChangeEvents = LogPropertyChangeEvents;

        //    ////rcServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //    //rcServoHost.LogPositionChangeEvents = LogPositionChangeEvents;
        //    //rcServoHost.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //    //rcServoHost.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

        //    //rcServoHost.LogPerformanceSequence = LogPerformanceSequence;
        //    //rcServoHost.LogSequenceAction = LogSequenceAction;
        //    //rcServoHost.LogActionVerification = LogActionVerification;

        //    if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

        //    return rcServoHost;
        //}

        #endregion

        #region CloseRCServo Command

        public DelegateCommand<string> CloseRCServoCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseRCServoHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseRCServoCommandParameter;

        public string CloseRCServoContent { get; set; } = "Close";
        public string CloseRCServoToolTip { get; set; } = "Close RCServo";

        // Can get fancy and use Resources
        //public string CloseRCServoContent { get; set; } = "ViewName_CloseRCServoContent";
        //public string CloseRCServoToolTip { get; set; } = "ViewName_CloseRCServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseRCServoContent">CloseRCServo</system:String>
        //    <system:String x:Key="ViewName_CloseRCServoContentToolTip">CloseRCServo ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseRCServo(string servoNumber)
        //public void CloseRCServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called CloseRCServo";

            PublishStatusMessage(Message);

            Int32 serialNumber = SelectedAdvancedServo.SerialNumber;
            Int32 channel;

            if (Int32.TryParse(servoNumber, out channel))
            {
                SerialChannel serialChannel = new SerialChannel() { SerialNumber = serialNumber, Channel = channel };

                await Task.Run(() => PhidgetDeviceLibrary.RCServoChannels[serialChannel].Close());
            }
            else
            {
                Message = $"Cannot parse servoNumber:>{servoNumber}<";
                Log.Error(Message, Common.LOG_CATEGORY);
            }

            OpenRCServoCommand.RaiseCanExecuteChanged();
            CloseRCServoCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseRCServoHost is null) _CloseRCServoHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseRCServoEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseRCServoEvent>().Publish(
            //      new CloseRCServoEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseRCServoEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseRCServoEvent>().Subscribe(CloseRCServo);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public bool CloseRCServoCanExecute(string servoNumber)
        //public bool CloseRCServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (!Int32.TryParse(servoNumber, out channel)) throw new Exception($"Cannot parse servoNumber:{servoNumber}");
            
            if (SelectedAdvancedServo is null) return false;

            SerialChannel serialChannel = new SerialChannel() { SerialNumber = SelectedAdvancedServo.SerialNumber, Channel = channel };

            RCServoEx? rcServoHost;

            if (!PhidgetDeviceLibrary.RCServoChannels.TryGetValue(serialChannel, out rcServoHost)) return false;

            if (rcServoHost is null) return false;

            if (rcServoHost.IsAttached)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigureServo) Enter", Common.LOG_CATEGORY);
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigureServo) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SetPositionRange) Enter", Common.LOG_CATEGORY);
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

            // FIX(crhodes)
            // 
            //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
            //Phidget22.AdvancedServoServo servo = null;

            //Int32 servoIndex = Int32.Parse(servoID);
            //servo = servos[servoIndex];

            // NOTE(crhodes)
            // Should be safe to get Acceleration, Velocity, and Position here
            // Device is Engaged

            Double? halfRange;
            Double? tenPercent;

            try
            {
                // FIX(crhodes)
                // 
                //AdvancedServoProperties[servoIndex].PositionMin =
                //    AdvancedServoProperties[servoIndex].Position - AdvancedServoProperties[servoIndex].PositionRange;

                //AdvancedServoProperties[servoIndex].PositionMax =
                //    AdvancedServoProperties[servoIndex].Position + AdvancedServoProperties[servoIndex].PositionRange;

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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SetPositionRange) Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigureServo2) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = $"Cool, you called ConfigureServo2 and passed: {value}";

            Int32 servoIndex = Int32.Parse(value);

            // FIX(crhodes)
            // 
            //AdvancedServoServo servo = ActiveAdvancedServo.AdvancedServo.servos[servoIndex];

            try
            {
                // FIX(crhodes)
                // 
                //servo.setServoParameters(
                //    AdvancedServoProperties[0].MinimumPulseWidth,
                //    AdvancedServoProperties[0].MaximumPulseWidth,
                //    AdvancedServoProperties[0].Degrees,
                //    (Double)AdvancedServoProperties[0].VelocityMax);
                
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

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigureServo2) Exit", Common.LOG_CATEGORY, startTicks);
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

        #region SayHello Command

        public ICommand SayHelloCommand { get; private set; }

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

        private void ConfigureCurrentInputs(short channelCount, Int32 serialNumber)
        {
            for (Int16 i = 0; i < channelCount; i++)
            {
                //RCServos[i].SerialNumber = serialNumber;
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
