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
        : EventViewModelBase, IAdvancedServo1061ViewModel//, IInstanceCountVM
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

            OpenAllRCServosCommand = new DelegateCommand(OpenAllRCServos, OpenAllRCServosCanExecute);
            CloseAllRCServosCommand = new DelegateCommand(CloseAllRCServos, CloseAllRCServosCanExecute);

            // TODO(crhodes)
            // What is this for?
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

            // NOTE(crhodes)
            // We use ChannelClass here because same UI can be used for
            // AdvancedServo boards and 16xRCServo boards through VINT Hub

            RCServoPhidgets = Common.PhidgetDeviceLibrary.ManagerAttachedPhidgetDevices
                .Where(x => x.ChannelClass == "RCServo")
                .DistinctBy(x => x.DeviceSerialNumber)
                .Select(x => x.DeviceSerialNumber);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadPhidgets()
        {
            var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
                .Where(kv => kv.Key.SerialNumber == SelectedRCServoPhidget);

            LoadRCServos(rcServos);

            // TODO(crhodes)
            // Figure out what else is available on phidget
            // like Digital I/O and Current
        }

        private void LoadRCServos(IEnumerable<KeyValuePair<SerialHubPortChannel, RCServoEx?>> rcServos)
        {
            // NOTE(crhodes)
            // May be able to go back to RCServo[]
            // Check if INPC get's messed up

            foreach (var rcServo in rcServos)
            {
                switch (rcServo.Key.Channel)
                {
                    case 0:
                        RCServo0 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 1:
                        RCServo1 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 2:
                        RCServo2 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 3:
                        RCServo3 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 4:
                        RCServo4 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 5:
                        RCServo5 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 6:
                        RCServo6 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 7:
                        RCServo7 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 8:
                        RCServo8 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 9:
                        RCServo9 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 10:
                        RCServo10 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 11:
                        RCServo11 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 12:
                        RCServo12 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 13:
                        RCServo13 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 14:
                        RCServo14 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                    case 15:
                        RCServo15 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
                        break;

                }
            }
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

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

        #region Performance Events

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

        #endregion

        #region AdvancedServo

        private IEnumerable<Int32> _rcServoPhidgets;
        public IEnumerable<Int32> RCServoPhidgets
        {
            get
            {
                return _rcServoPhidgets;
            }

            set
            {
                _rcServoPhidgets = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedRCServoPhidget = null;
        public Int32? SelectedRCServoPhidget
        {
            get => _selectedRCServoPhidget;
            set
            {
                _selectedRCServoPhidget = value;
                OnPropertyChanged();

                LoadPhidgets();

                OpenAllRCServosCommand?.RaiseCanExecuteChanged();
                CloseAllRCServosCommand?.RaiseCanExecuteChanged();

                OpenRCServoCommand?.RaiseCanExecuteChanged();

                RCServosVisibility = Visibility.Visible;
            }
        }

        #region RCServos

        // TODO(crhodes)
        // I think we can go back to RCServoEX[16]
        // Besure doesn't break UI with INPC

        RCServoEx? _rcServo0;
        public RCServoEx? RCServo0
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

        RCServoEx? _rcServo1;
        public RCServoEx? RCServo1
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

        RCServoEx? _rcServo2;
        public RCServoEx? RCServo2
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

        RCServoEx? _rcServo3;
        public RCServoEx? RCServo3
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

        RCServoEx? _rcServo4;
        public RCServoEx? RCServo4
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

        RCServoEx? _rcServo5;
        public RCServoEx? RCServo5
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

        RCServoEx? _rcServo6;
        public RCServoEx? RCServo6
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

        RCServoEx? _rcServo7;
        public RCServoEx? RCServo7
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

        RCServoEx? _rcServo8;
        public RCServoEx? RCServo8
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

        RCServoEx? _rcServo9;
        public RCServoEx? RCServo9
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

        RCServoEx? _rcServo10;
        public RCServoEx? RCServo10
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

        RCServoEx? _rcServo11;
        public RCServoEx? RCServo11
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

        RCServoEx? _rcServo12;
        public RCServoEx? RCServo12
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

        RCServoEx? _rcServo13;
        public RCServoEx? RCServo13
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

        RCServoEx? _rcServo14;
        public RCServoEx? RCServo14
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

        RCServoEx? _rcServo15;
        public RCServoEx? RCServo15
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

        #endregion


        #region CurrentInputs

        #region CurrentInput0

        #endregion

        #endregion

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands

        #region OpenAllRCServos Command

        public DelegateCommand? OpenAllRCServosCommand { get; set; }
        public string OpenAllRCServosContent { get; set; } = "Open All RCServos";
        public string OpenAllRCServosToolTip { get; set; } = "OpenAllRCServos ToolTip";

        // Can get fancy and use Resources
        //public string OpenAdvancedServoContent { get; set; } = "ViewName_OpenAdvancedServoContent";
        //public string OpenAdvancedServoToolTip { get; set; } = "ViewName_OpenAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenAdvancedServoContent">OpenAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_OpenAdvancedServoContentToolTip">OpenAdvancedServo ToolTip</system:String>  

        public void OpenAllRCServos()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAllRCServos";
            PublishStatusMessage(Message);

            var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
                .Where(kv => kv.Key.SerialNumber == SelectedRCServoPhidget);

            foreach (var rcServo in rcServos)
            {
                OpenRCServo(rcServo.Key);        
            }

            OpenAllRCServosCommand?.RaiseCanExecuteChanged();
            CloseAllRCServosCommand?.RaiseCanExecuteChanged();

            //InitializeVelocityCommand?.RaiseCanExecuteChanged();
            //InitializeAccelerationCommand?.RaiseCanExecuteChanged();

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

        public Boolean OpenAllRCServosCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedRCServoPhidget > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseAllRCServos Command

        public DelegateCommand? CloseAllRCServosCommand { get; set; }
        public string CloseAllRCServosContent { get; set; } = "Close All RCServos";
        public string CloseAllRCServosToolTip { get; set; } = "CloseAllRCServos ToolTip";

        // Can get fancy and use Resources
        //public string CloseAdvancedServoContent { get; set; } = "ViewName_CloseAdvancedServoContent";
        //public string CloseAdvancedServoToolTip { get; set; } = "ViewName_CloseAdvancedServoContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseAdvancedServoContent">CloseAdvancedServo</system:String>
        //    <system:String x:Key="ViewName_CloseAdvancedServoContentToolTip">CloseAdvancedServo ToolTip</system:String>
        public async void CloseAllRCServos()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseAdvancedServo) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAllRCServos";
            PublishStatusMessage(Message);

            var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
                .Where(kv => kv.Key.SerialNumber == SelectedRCServoPhidget);

            foreach (var rcServo in rcServos)
            {
                CloseRCServo(rcServo.Key);
            }

            OpenAllRCServosCommand?.RaiseCanExecuteChanged();
            CloseAllRCServosCommand?.RaiseCanExecuteChanged();

            SetPositionRangeCommand?.RaiseCanExecuteChanged();

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

        public Boolean CloseAllRCServosCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedRCServoPhidget > 0)
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

        public DelegateCommand<SerialHubPortChannel?>? OpenRCServoCommand { get; set; }
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

        public async void OpenRCServo(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenRCServo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"Cool, you called OpenRCServo on " +
                 $"serialHubPortChannel:{serialHubPortChannel?.SerialNumber}" +
                 $":{serialHubPortChannel?.HubPort}:{serialHubPortChannel?.Channel}";
            PublishStatusMessage(Message);

            RCServoEx? rcServoHost = Common.PhidgetDeviceLibrary.RCServoChannels[(SerialHubPortChannel)serialHubPortChannel];

            switch (serialHubPortChannel?.Channel)
            {
                case 0:
                    await OpenRCServo(RCServo0);
                    break;

                case 1:
                    await OpenRCServo(RCServo1);
                    break;

                case 2:
                    await OpenRCServo(RCServo2);
                    break;

                case 3:
                    await OpenRCServo(RCServo3);
                    break;

                case 4:
                    await OpenRCServo(RCServo4);
                    break;

                case 5:
                    await OpenRCServo(RCServo5);
                    break;

                case 6:
                    await OpenRCServo(RCServo6);
                    break;

                case 7:
                    await OpenRCServo(RCServo7);
                    break;

                case 8:
                    await OpenRCServo(RCServo8);
                    break;

                case 9:
                    await OpenRCServo(RCServo9);
                    break;

                case 10:
                    await OpenRCServo(RCServo10);
                    break;

                case 11:
                    await OpenRCServo(RCServo11);
                    break;

                case 12:
                    await OpenRCServo(RCServo12);
                    break;

                case 13:
                    await OpenRCServo(RCServo13);
                    break;

                case 14:
                    await OpenRCServo(RCServo14);
                    break;

                case 15:
                    await OpenRCServo(RCServo15);
                    break;

            }
            
            OpenRCServoCommand?.RaiseCanExecuteChanged();
            CloseRCServoCommand?.RaiseCanExecuteChanged();

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

        private async Task OpenRCServo(RCServoEx? rcServo)
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

        void ConfigureInitialLogging(RCServoEx? rcServo)
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

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean OpenRCServoCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenRCServoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedRCServoPhidget is null) return false;

            if (serialHubPortChannel is null) return false;

            RCServoEx? host;

            if (!Common.PhidgetDeviceLibrary.RCServoChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
            {
                return true;
            }

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

        public DelegateCommand<SerialHubPortChannel?>? CloseRCServoCommand { get; set; }
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

            Message = $"Cool, you called CloseRCServo on " +
                $"serialHubPortChannel:{serialHubPortChannel?.SerialNumber}" +
                $":{serialHubPortChannel?.HubPort}:{serialHubPortChannel?.Channel}";

            PublishStatusMessage(Message);

            if (serialHubPortChannel is not null)
            {
                await Task.Run(() => Common.PhidgetDeviceLibrary.RCServoChannels[(SerialHubPortChannel)serialHubPortChannel].Close());
            }            
 
            OpenRCServoCommand?.RaiseCanExecuteChanged();
            CloseRCServoCommand?.RaiseCanExecuteChanged();

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
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
            { 
                return false; 
            }

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

        #region SetPositionRange Command

        public DelegateCommand<string>? SetPositionRangeCommand { get; set; }
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
                //        Log.TRACE($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
                //        break;

                //}
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, Common.LOG_CATEGORY);
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
        public DelegateCommand<string>? ConfigureServo2Command { get; set; }
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
                Log.ERROR(ex, Common.LOG_CATEGORY);
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



        #endregion

        //#region IInstanceCount

        //private static Int32 _instanceCountVM;

        //public Int32 InstanceCountVM
        //{
        //    get => _instanceCountVM;
        //    set => _instanceCountVM = value;
        //}

        //#endregion
    }
}
