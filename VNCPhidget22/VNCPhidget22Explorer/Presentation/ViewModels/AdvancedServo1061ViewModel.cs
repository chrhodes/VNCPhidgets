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

            OpenAdvancedServoCommand = new DelegateCommand(OpenAdvancedServo, OpenAdvancedServoCanExecute);
            CloseAdvancedServoCommand = new DelegateCommand(CloseAdvancedServo, CloseAdvancedServoCanExecute);

            //InitializeVelocityCommand = new DelegateCommand<string>(InitializeVelocity, InitializeVelocityCanExecute);
            //InitializeAccelerationCommand = new DelegateCommand<string>(InitializeAcceleration, InitializeAccelerationCanExecute);

            //ConfigureServoCommand = new DelegateCommand(ConfigureServo, ConfigureServoCanExecute);

            ConfigureServo2Command = new DelegateCommand<string>(ConfigureServo2, ConfigureServo2CanExecute);

            SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            OpenRCServoCommand = new DelegateCommand<SerialHubPortChannel?>(OpenRCServo, OpenRCServoCanExecute);
            CloseRCServoCommand = new DelegateCommand<SerialHubPortChannel?>(CloseRCServo, CloseRCServoCanExecute);

            LoadUIConfig();

            Message = "AdvancedServo1061ViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            AdvancedServoPhidgets = Common.PhidgetDeviceLibrary.ManagerAttachedPhidgetDevices
                .Where(x => x.DeviceClass == "AdvancedServo")
                .DistinctBy(x => x.DeviceSerialNumber)
                .Select(x => x.DeviceSerialNumber);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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


        #region Logging

        private Boolean _logPhidgetEvents = true;
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

        private Boolean _logDeviceChannelSequence = false;
        public Boolean LogDeviceChannelSequence
        {
            get => _logDeviceChannelSequence;
            set
            {
                if (_logDeviceChannelSequence == value)
                    return;
                _logPhidgetEvents = value;
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
                _logPhidgetEvents = value;
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
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region AdvancedServo

        private IEnumerable<Int32> _AdvancedServoPhidgets;
        public IEnumerable<Int32> AdvancedServoPhidgets
        {
            get
            {
                return _AdvancedServoPhidgets;
            }

            set
            {
                _AdvancedServoPhidgets = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedAdvancedServoPhidget = null;
        public Int32? SelectedAdvancedServoPhidget
        {
            get => _selectedAdvancedServoPhidget;
            set
            {
                _selectedAdvancedServoPhidget = value;
                OnPropertyChanged();

                OpenAdvancedServoCommand.RaiseCanExecuteChanged();
                CloseAdvancedServoCommand.RaiseCanExecuteChanged();

                OpenRCServoCommand.RaiseCanExecuteChanged();

                RCServosVisibility = Visibility.Visible;
            }
        }

        #region RCServos

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


        RCServoEx _rcServo8;
        public RCServoEx RCServo8
        {
            get
            {
                return _rcServo8;
            }
            set
            {
                _rcServo8 = value;
                OnPropertyChanged();
            }
        }


        RCServoEx _rcServo9;
        public RCServoEx RCServo9
        {
            get
            {
                return _rcServo9;
            }
            set
            {
                _rcServo9 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo10;
        public RCServoEx RCServo10
        {
            get
            {
                return _rcServo10;
            }
            set
            {
                _rcServo10 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo11;
        public RCServoEx RCServo11
        {
            get
            {
                return _rcServo11;
            }
            set
            {
                _rcServo11 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo12;
        public RCServoEx RCServo12
        {
            get
            {
                return _rcServo12;
            }
            set
            {
                _rcServo12 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo13;
        public RCServoEx RCServo13
        {
            get
            {
                return _rcServo13;
            }
            set
            {
                _rcServo13 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo14;
        public RCServoEx RCServo14
        {
            get
            {
                return _rcServo14;
            }
            set
            {
                _rcServo14 = value;
                OnPropertyChanged();
            }
        }

        RCServoEx _rcServo15;
        public RCServoEx RCServo15
        {
            get
            {
                return _rcServo15;
            }
            set
            {
                _rcServo15 = value;
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogCurrentChangeEvents = _logCurrentChangeEvents;
                //}
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogPositionChangeEvents = _logPositionChangeEvents;
                //}
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogVelocityChangeEvents = _logVelocityChangeEvents;
                //}
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

                //if (ActiveAdvancedServo is not null)
                //{
                //    ActiveAdvancedServo.LogTargetPositionReachedEvents = _logTargetPositionReachedEvents;
                //}
            }
        }

        #endregion

        //private IEnumerable<VNCPhidgetConfig.AdvancedServo> _AdvancedServos;
        //public IEnumerable<VNCPhidgetConfig.AdvancedServo> AdvancedServos
        //{
        //    get
        //    {
        //        if (null == _AdvancedServos)
        //        {
        //            // TODO(crhodes)
        //            // Load this like the sensors.xml for now

        //            //_InterfaceKits =
        //            //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
        //            //    select new InterfaceKit(
        //            //        item.Attribute("Name").Value,
        //            //        item.Attribute("IPAddress").Value,
        //            //        item.Attribute("Port").Value,
        //            //        Boolean.Parse(item.Attribute("Enable").Value)
        //            //        );
        //        }

        //        return _AdvancedServos;
        //    }

        //    set
        //    {
        //        _AdvancedServos = value;
        //        OnPropertyChanged();
        //    }
        //}

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
        //            //        Boolean.Parse(item.Attribute("Enable").Value)
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

        //private VNCPhidgetConfig.AdvancedServo _selectedAdvancedServo;
        //public VNCPhidgetConfig.AdvancedServo SelectedAdvancedServo
        //{
        //    get => _selectedAdvancedServo;
        //    set
        //    {
        //        if (_selectedAdvancedServo == value)
        //            return;
        //        _selectedAdvancedServo = value;

        //        OpenAdvancedServoCommand.RaiseCanExecuteChanged();
        //        CloseAdvancedServoCommand.RaiseCanExecuteChanged();

        //        OpenRCServoCommand.RaiseCanExecuteChanged();
        //        //PlayPerformanceCommand.RaiseCanExecuteChanged();
        //        //PlaySequenceCommand.RaiseCanExecuteChanged();

        //        RCServosVisibility = Visibility.Visible;
        //        // Set to null when host changes
        //        //if (value is not null)
        //        //{
        //        //    // FIX(crhodes)
        //        //    // 
        //        //    DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.ManagerAttachedPhidgetDevices[value.SerialNumber].DeviceChannels;

        //        //    RCServosVisibility = deviceChannels.RCServoCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        //        //}
        //        //else
        //        //{
        //        //    RCServosVisibility = Visibility.Collapsed;
        //        //}

        //        OnPropertyChanged();
        //    }
        //}

        //private AdvancedServoEx _activeAdvancedServo;
        //public AdvancedServoEx ActiveAdvancedServo
        //{
        //    get => _activeAdvancedServo;
        //    set
        //    {
        //        if (_activeAdvancedServo == value)
        //            return;
        //        _activeAdvancedServo = value;

        //        //if (_activeAdvancedServo is not null)
        //        //{
        //        //    PhidgetDevice = _activeAdvancedServo.AdvancedServo;
        //        //}
        //        //else
        //        //{
        //        //    PhidgetDevice = null;
        //        //}

        //        OnPropertyChanged();
        //    }
        //}

        //private Int32? _servoCount;
        //public Int32? ServoCount
        //{
        //    get => _servoCount;
        //    set
        //    {
        //        if (_servoCount == value)
        //            return;
        //        _servoCount = value;
        //        OnPropertyChanged();
        //    }
        //}

        // TODO(crhodes)
        // Need to drive this off _deviceChannels.RCServoCount

        //private ServoProperties[] _advancedServoProperties = new ServoProperties[8]
        //{
        //    new ServoProperties() { ServoIndex = 0 },
        //    new ServoProperties() { ServoIndex = 1 },
        //    new ServoProperties() { ServoIndex = 2 },
        //    new ServoProperties() { ServoIndex = 3 },
        //    new ServoProperties() { ServoIndex = 4 },
        //    new ServoProperties() { ServoIndex = 5 },
        //    new ServoProperties() { ServoIndex = 6 },
        //    new ServoProperties() { ServoIndex = 7 },
        //};

        //public ServoProperties[] AdvancedServoProperties
        //{
        //    get => _advancedServoProperties;
        //    set
        //    {
        //        if (_advancedServoProperties == value)
        //            return;
        //        _advancedServoProperties = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        #region Event Handlers (none)



        #endregion

        #region Commands

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
            PublishStatusMessage(Message);

            var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
                .Where(kv => kv.Key.SerialNumber == SelectedAdvancedServoPhidget);


            foreach (var rcServo in rcServos)
            {
                if (rcServo.Value.IsHubPortDevice)
                {
                    // TODO(crhodes)
                    // 
                    //OpenRCServoHubPort(rcServo.Key);
                }
                else
                {
                    OpenRCServo(rcServo.Key);
                }                    
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            //InitializeVelocityCommand.RaiseCanExecuteChanged();
            //InitializeAccelerationCommand.RaiseCanExecuteChanged();

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

        public Boolean OpenAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedAdvancedServoPhidget > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        //#region InitializeVelocityCommand

        ////public DelegateCommand InitializeVelocityCommand { get; set; }
        //public DelegateCommand<string> InitializeVelocityCommand { get; set; }
        //public string InitializeVelocityContent { get; set; } = "Initilize Velocity";
        //public string InitializeVelocityToolTip { get; set; } = "Initialize Velocity using Velocity Scale";

        //// Can get fancy and use Resources
        ////public string InitializeSlowAdvancedServoContent { get; set; } = "ViewName_InitializeSlowAdvancedServoContent";
        ////public string InitializeSlowAdvancedServoToolTip { get; set; } = "ViewName_InitializeSlowAdvancedServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContent">InitializeSlowAdvancedServo</system:String>
        ////    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContentToolTip">InitializeSlowAdvancedServo ToolTip</system:String>  

        ////public void InitializeSlowAdvancedServo()

        //public void InitializeVelocity(string speed)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeVelocity) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called InitializeSlowAdvancedServo";
        //    PublishStatusMessage(Message);

        //    if ((Boolean)DeviceAttached)
        //    {
        //        // FIX(crhodes)
        //        // 
        //        //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

        //        //try
        //        //{
        //        //    for (Int32 i = 0; i < servos.Count; i++)
        //        //    {
        //        //        AdvancedServoProperties[i].InitializeVelocity(ConvertStringToInitializeMotion(speed));
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    Log.Error(ex, Common.LOG_CATEGORY);
        //        //}
        //    }

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish(
        //    //      new InitializeSlowAdvancedServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class InitializeSlowAdvancedServoEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Subscribe(InitializeSlowAdvancedServo);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeVelocity) Exit", Common.LOG_CATEGORY, startTicks);
        //}


        ////public Boolean InitializeSlowAdvancedServoCanExecute()
        //public Boolean InitializeVelocityCanExecute(string speed)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    //return true;
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        //#endregion

        //#region InitializeAccelerationCommand

        ////public DelegateCommand InitializeVelocityCommand { get; set; }
        //public DelegateCommand<string> InitializeAccelerationCommand { get; set; }
        //public string InitializeAccelerationContent { get; set; } = "Initilize Acceleration";
        //public string InitializeAccelerationToolTip { get; set; } = "Initialize Acceleration using Acceleration Scale";

        //// Can get fancy and use Resources
        ////public string InitializeSlowAdvancedServoContent { get; set; } = "ViewName_InitializeSlowAdvancedServoContent";
        ////public string InitializeSlowAdvancedServoToolTip { get; set; } = "ViewName_InitializeSlowAdvancedServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContent">InitializeSlowAdvancedServo</system:String>
        ////    <system:String x:Key="ViewName_InitializeSlowAdvancedServoContentToolTip">InitializeSlowAdvancedServo ToolTip</system:String>  

        ////public void InitializeSlowAdvancedServo()

        //public void InitializeAcceleration(string speed)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeAcceleration) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called InitializeAcceleration";
        //    PublishStatusMessage(Message);

        //    if ((Boolean)DeviceAttached)
        //    {
        //        // FIX(crhodes)
        //        // 
        //        //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

        //        //try
        //        //{
        //        //    for (Int32 i = 0; i < servos.Count; i++)
        //        //    {
        //        //        AdvancedServoProperties[i].InitializeAcceleration(ConvertStringToInitializeMotion(speed));
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    Log.Error(ex, Common.LOG_CATEGORY);
        //        //}
        //    }

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Publish(
        //    //      new InitializeSlowAdvancedServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class InitializeSlowAdvancedServoEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<InitializeSlowAdvancedServoEvent>().Subscribe(InitializeSlowAdvancedServo);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeAcceleration) Exit", Common.LOG_CATEGORY, startTicks);
        //}


        ////public Boolean InitializeSlowAdvancedServoCanExecute()
        //public Boolean InitializeAccelerationCanExecute(string speed)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    //return true;
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        //#endregion

        //#region InitializeMediumAdvancedServo Command

        //public DelegateCommand InitializeMediumAdvancedServoCommand { get; set; }
        //public string InitializeMediumAdvancedServoContent { get; set; } = "Medium";
        //public string InitializeMediumAdvancedServoToolTip { get; set; } = "Initialize Medium Speed and Center Position";

        //// Can get fancy and use Resources
        ////public string InitializeMediumAdvancedServoContent { get; set; } = "ViewName_InitializeMediumAdvancedServoContent";
        ////public string InitializeMediumAdvancedServoToolTip { get; set; } = "ViewName_InitializeMediumAdvancedServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_InitializeMediumAdvancedServoContent">InitializeMediumAdvancedServo</system:String>
        ////    <system:String x:Key="ViewName_InitializeMediumAdvancedServoContentToolTip">InitializeMediumAdvancedServo ToolTip</system:String>  

        //public void InitializeMediumAdvancedServo()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeMediumAdvancedServo) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called InitializeMediumAdvancedServo";
        //    PublishStatusMessage(Message);

        //    if ((Boolean)DeviceAttached)
        //    {
        //        // FIX(crhodes)
        //        // 
        //        //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

        //        //try
        //        //{
        //        //    for (Int32 i = 0; i < servos.Count; i++)
        //        //    {
        //        //        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Percent50);
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    Log.Error(ex, Common.LOG_CATEGORY);
        //        //}
        //    }

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Publish(
        //    //      new InitializeMediumAdvancedServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class InitializeMediumAdvancedServoEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<InitializeMediumAdvancedServoEvent>().Subscribe(InitializeMediumAdvancedServo);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeMediumAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
        //}


        //public Boolean InitializeMediumAdvancedServoCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    //return true;
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        //#endregion

        //#region InitializeFastAdvancedServo Command

        //public DelegateCommand InitializeFastAdvancedServoCommand { get; set; }
        //public string InitializeFastAdvancedServoContent { get; set; } = "Fast";
        //public string InitializeFastAdvancedServoToolTip { get; set; } = "Initialize Fast Speed and Center Position";

        //// Can get fancy and use Resources
        ////public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        ////public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        ////    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        //public void InitializeFastAdvancedServo()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeFastAdvancedServo) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you calledInitializeFastAdvancedServo";
        //    PublishStatusMessage(Message);

        //    if ((Boolean)DeviceAttached)
        //    {
        //        // FIX(crhodes)
        //        // 
        //        //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;

        //        //try
        //        //{
        //        //    for (Int32 i = 0; i < servos.Count; i++)
        //        //    {
        //        //        AdvancedServoProperties[i].InitializeVelocity(ServoProperties.MotionScale.Max);
        //        //    }
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    Log.Error(ex, Common.LOG_CATEGORY);
        //        //}
        //    }

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<OpenAdvancedServoEvent>().Publish(
        //    //      new OpenAdvancedServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class OpenAdvancedServoEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<OpenAdvancedServoEvent>().Subscribe(OpenAdvancedServo);

        //    // End Cut Four

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeFastAdvancedServo) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public Boolean InitializeFastAdvancedServoCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    //return true;
        //    if (DeviceAttached is not null)
        //        return (Boolean)DeviceAttached;
        //    else
        //        return false;
        //}

        //#endregion

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
            PublishStatusMessage(Message);

            var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
                .Where(kv => kv.Key.SerialNumber == SelectedAdvancedServoPhidget);

            foreach (var rcServo in rcServos)
            {
                if (rcServo.Value.IsHubPortDevice)
                {
                    // TODO(crhodes)
                    // 
                    //OpenRCServoHubPort(rcServo.Key);
                }
                else
                {
                    CloseRCServo(rcServo.Key);
                }
            }

            OpenAdvancedServoCommand.RaiseCanExecuteChanged();
            CloseAdvancedServoCommand.RaiseCanExecuteChanged();

            //InitializeVelocityCommand.RaiseCanExecuteChanged();
            //InitializeAccelerationCommand.RaiseCanExecuteChanged();

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

        public Boolean CloseAdvancedServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedAdvancedServoPhidget > 0)
            {
                return true;
            }                
            else
            {
                return false;
            }                
        }

        #endregion

        #region OpenRCServo Command

        public DelegateCommand<SerialHubPortChannel?> OpenRCServoCommand { get; set; }
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

        private async Task OpenRCServo(RCServoEx rcServo)
        {
            ConfigureInitialLogging(rcServo);

            if (rcServo.IsOpen is false)
            {
                await Task.Run(() => rcServo.Open(10000));
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{rcServo} already open", Common.LOG_CATEGORY);
            }
        }

        void ConfigureInitialLogging(RCServoEx rcServo)
        {
            rcServo.LogPhidgetEvents = LogPhidgetEvents;
            rcServo.LogErrorEvents = LogErrorEvents;
            rcServo.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            rcServo.LogPositionChangeEvents = LogPositionChangeEvents;
            rcServo.LogVelocityChangeEvents = LogVelocityChangeEvents;

            rcServo.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            rcServo.LogDeviceChannelSequence = LogDeviceChannelSequence;
            rcServo.LogChannelAction = LogChannelAction;
            rcServo.LogActionVerification = LogActionVerification;
        }

        public async void OpenRCServo(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenRCServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenDigitalInput on " +
                 $"serialHubPortChannel:{shpc.SerialNumber}" +
                 $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];

            switch (shpc.Channel)
            {
                case 0:
                    if (RCServo0 is null) RCServo0 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo0);
                    break;

                case 1:
                    if (RCServo1 is null) RCServo1 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo1);
                    break;

                case 2:
                    if (RCServo2 is null) RCServo2 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo2);
                    break;

                case 3:
                    if (RCServo3 is null) RCServo3 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo3);
                    break;

                case 4:
                    if (RCServo4 is null) RCServo4 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo4);
                    break;

                case 5:
                    if (RCServo5 is null) RCServo5 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo5);
                    break;

                case 6:
                    if (RCServo6 is null) RCServo6 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo6);
                    break;

                case 7:
                    if (RCServo7 is null) RCServo7 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo7);
                    break;

                case 8:
                    if (RCServo8 is null) RCServo8 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo8);
                    break;

                case 9:
                    if (RCServo9 is null) RCServo9 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo9);
                    break;

                case 10:
                    if (RCServo10 is null) RCServo10 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo10);
                    break;

                case 11:
                    if (RCServo11 is null) RCServo11 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo11);
                    break;

                case 12:
                    if (RCServo12 is null) RCServo12 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo12);
                    break;

                case 13:
                    if (RCServo13 is null) RCServo13 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo13);
                    break;

                case 14:
                    if (RCServo14 is null) RCServo14 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo14);
                    break;

                case 15:
                    if (RCServo15 is null) RCServo15 = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];
                    await OpenRCServo(RCServo15);
                    break;

            }
            
            OpenRCServoCommand.RaiseCanExecuteChanged();
            CloseRCServoCommand.RaiseCanExecuteChanged();

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
        public Boolean OpenRCServoCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenRCServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedAdvancedServoPhidget is null) return false;

            RCServoEx? host;

            if (!Common.PhidgetDeviceLibrary.RCServoChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

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

        #region CloseRCServo Command

        public DelegateCommand<SerialHubPortChannel?> CloseRCServoCommand { get; set; }
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
        public async void CloseRCServo(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseRCServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseDigitalInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";

            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.RCServoChannels[shpc].Close());
 
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

        public Boolean CloseRCServoCanExecute(SerialHubPortChannel? serialHubPortChannel)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            RCServoEx? host;

            if (!Common.PhidgetDeviceLibrary.RCServoChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

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
            PublishStatusMessage(Message);

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

        public Boolean ConfigureServoCanExecute()
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
            PublishStatusMessage(Message);

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

        public Boolean SetPositionRangeCanExecute(string value)
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
            PublishStatusMessage(Message);

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
        public Boolean ConfigureServo2CanExecute(string value)
        //public Boolean ConfigureServo2CanExecute()
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

        //private void ConfigureCurrentInputs(short channelCount, Int32 serialNumber)
        //{
        //    for (Int16 i = 0; i < channelCount; i++)
        //    {
        //        //RCServos[i].SerialNumber = serialNumber;
        //    }
        //}

        //private ServoProperties.MotionScale ConvertStringToInitializeMotion(string speed)
        //{
        //    ServoProperties.MotionScale result = ServoProperties.MotionScale.Percent05;

        //    switch (speed)
        //    {
        //        case "Min":
        //            result = ServoProperties.MotionScale.Min;
        //            break;

        //        case "05%":
        //            result = ServoProperties.MotionScale.Percent05;
        //            break;

        //        case "10%":
        //            result = ServoProperties.MotionScale.Percent10;
        //            break;

        //        case "15%":
        //            result = ServoProperties.MotionScale.Percent15;
        //            break;

        //        case "20%":
        //            result = ServoProperties.MotionScale.Percent20;
        //            break;

        //        case "25%":
        //            result = ServoProperties.MotionScale.Percent25;
        //            break;

        //        case "35%":
        //            result = ServoProperties.MotionScale.Percent35;
        //            break;

        //        case "50%":
        //            result = ServoProperties.MotionScale.Percent50;
        //            break;

        //        case "75%":
        //            result = ServoProperties.MotionScale.Percent75;
        //            break;

        //        case "Max":
        //            result = ServoProperties.MotionScale.Max;
        //            break;

        //        default:
        //            Log.Error($"Unexpected speed:{speed}", Common.LOG_CATEGORY);
        //            break;
        //    }

        //    return result;
        //}

        #endregion

        #region IInstanceCount

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
