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

            OpenAllSteppersCommand = new DelegateCommand(OpenAllSteppers, OpenAllSteppersCanExecute);
            CloseAllSteppersCommand = new DelegateCommand(CloseAllSteppers, CloseAllSteppersCanExecute);

            OpenStepperCommand = new DelegateCommand<SerialHubPortChannel?>(OpenStepper, OpenStepperCanExecute);
            CloseStepperCommand = new DelegateCommand<SerialHubPortChannel?>(CloseStepper, CloseStepperCanExecute);

            ZeroCurrentPositionCommand = new DelegateCommand<string>(ZeroCurrentPosition, ZeroCurrentPositionCanExecute);

            LoadUIConfig();
               
            Message = "Stepper1063ViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            StepperPhidgets = Common.PhidgetDeviceLibrary.ManagerAttachedPhidgetDevices
                .Where(x => x.DeviceClass == "Stepper")
                .DistinctBy(x => x.DeviceSerialNumber)
                .Select(x => x.DeviceSerialNumber);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        void LoadPhidgets()
        {
            LoadSteppers();

            // TODO(crhodes)
            // Figure out what else is available on phidget
            // like Digital I/O and Current
        }

        private void LoadSteppers()
        {
            var steppers = Common.PhidgetDeviceLibrary.StepperChannels
                            .Where(kv => kv.Key.SerialNumber == SelectedStepperPhidget);

            // NOTE(crhodes)
            // May be able to go back to Stepper[]

            foreach (var stepper in steppers)
            {
                switch (stepper.Key.Channel)
                {
                    case 0:
                        Stepper0 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
                        break;

                    case 1:
                        Stepper1 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
                        break;

                    case 2:
                        Stepper2 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
                        break;

                    case 3:
                        Stepper3 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
                        break;

                        // TODO(crhodes)
                        // Add more cases if a board supports more channels
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

        #region Stepper Events

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


        private Boolean _logStoppeddEvents = false;
        public Boolean LogStoppedEvents
        {
            get => _logStoppeddEvents;
            set
            {
                if (_logStoppeddEvents == value)
                    return;
                _logStoppeddEvents = value;
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

        #region Stepper

        private IEnumerable<Int32> _SteperPhidgets;
        public IEnumerable<Int32> StepperPhidgets
        {
            get
            {
                return _SteperPhidgets;
            }

            set
            {
                _SteperPhidgets = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedSteperPhidgets = null;
        public Int32? SelectedStepperPhidget
        {
            get => _selectedSteperPhidgets;
            set
            {
                _selectedSteperPhidgets = value;
                OnPropertyChanged();

                LoadPhidgets();

                OpenAllSteppersCommand.RaiseCanExecuteChanged();
                CloseAllSteppersCommand.RaiseCanExecuteChanged();

                SteppersVisibility = Visibility.Visible;
            }
        }

        #region Steppers

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


        #endregion

        #endregion

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands

        #region OpenSteppers Command

        public DelegateCommand OpenAllSteppersCommand { get; set; }
        public string OpenAllSteppersContent { get; set; } = "Open";
        public string OpenAllSteppersToolTip { get; set; } = "OpenSteppers ToolTip";

        // Can get fancy and use Resources
        //public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        //public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        //    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        public async void OpenAllSteppers()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenSteppers) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenAllSteppers";
            PublishStatusMessage(Message);

            var steppers = Common.PhidgetDeviceLibrary.StepperChannels
                .Where(kv => kv.Key.SerialNumber == SelectedStepperPhidget);

            foreach (var stepper in steppers)
            {
                OpenStepper(stepper.Key);
            }

            OpenAllSteppersCommand.RaiseCanExecuteChanged();
            CloseAllSteppersCommand.RaiseCanExecuteChanged();

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

        public Boolean OpenAllSteppersCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedStepperPhidget > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region OpenStepper Command

        public DelegateCommand<SerialHubPortChannel?> OpenStepperCommand { get; set; }
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
        public async void OpenStepper(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenStepper on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            switch (shpc.Channel)
            {
                case 0:
                    await OpenStepper(Stepper0);
                    break;

                case 1:
                    await OpenStepper(Stepper1);
                    break;

                case 2:
                    await OpenStepper(Stepper2);
                    break;

                case 3:
                    await OpenStepper(Stepper0);
                    break;

                    // TODO(crhodes)
                    // Add more cases if a board supports more channels
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();

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

        private async Task OpenStepper(StepperEx stepper)
        {
            ConfigureInitialLogging(stepper);

            if (stepper.IsOpen is false)
            {
                await Task.Run(() => stepper.Open(10000));
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{stepper} already open", Common.LOG_CATEGORY);
            }
        }

        void ConfigureInitialLogging(StepperEx stepper)
        {
            stepper.LogPhidgetEvents = LogPhidgetEvents;
            stepper.LogErrorEvents = LogErrorEvents;
            stepper.LogPropertyChangeEvents = LogPropertyChangeEvents;

            stepper.LogPositionChangeEvents = LogPositionChangeEvents;
            stepper.LogVelocityChangeEvents = LogVelocityChangeEvents;

            stepper.LogStoppedEvents = LogStoppedEvents;

            stepper.LogDeviceChannelSequence = LogDeviceChannelSequence;
            stepper.LogChannelAction = LogChannelAction;
            stepper.LogActionVerification = LogActionVerification;
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean OpenStepperCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            Int32 channel;

            if (SelectedStepperPhidget is null) return false;

            if (serialHubPortChannel is null) return false;

            StepperEx? host;

            if (!Common.PhidgetDeviceLibrary.StepperChannels
                    .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
            { 
                return true; 
            }

            if (host.IsOpen)
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

        public DelegateCommand CloseAllSteppersCommand { get; set; }
        public string CloseAllSteppersContent { get; set; } = "Close";
        public string CloseAllSteppersToolTip { get; set; } = "CloseSteppers ToolTip";

        // Can get fancy and use Resources
        //public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        //public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        //    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

        public async void CloseAllSteppers()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseStepper) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseAllSteppers";
            PublishStatusMessage(Message);

            var steppers = Common.PhidgetDeviceLibrary.StepperChannels
                .Where(kv => kv.Key.SerialNumber == SelectedStepperPhidget);

            foreach (var stepper in steppers)
            {
                CloseStepper(stepper.Key);
            }

            OpenAllSteppersCommand.RaiseCanExecuteChanged();
            CloseAllSteppersCommand.RaiseCanExecuteChanged();

            //InitializeVelocityCommand.RaiseCanExecuteChanged();
            //InitializeAccelerationCommand.RaiseCanExecuteChanged();

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

        public Boolean CloseAllSteppersCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedStepperPhidget > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseStepper Command

        public DelegateCommand<SerialHubPortChannel?> CloseStepperCommand { get; set; }
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
        public async void CloseStepper(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseStepper on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";

            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.StepperChannels[shpc].Close());

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
        public Boolean CloseStepperCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean CloseStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            StepperEx? host;

            if (!Common.PhidgetDeviceLibrary.StepperChannels
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
        public Boolean ZeroCurrentPositionCanExecute(string stepperNumber)
        //public Boolean ZeroCurrentPositionCanExecute()
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

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
