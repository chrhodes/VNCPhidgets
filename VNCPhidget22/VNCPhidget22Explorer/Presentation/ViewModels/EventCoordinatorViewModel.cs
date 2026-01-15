using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;
using VNC.Core.Services;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;
using VNC.Phidget22.Events;
using VNC.Phidget22.Players;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class EventCoordinatorViewModel : EventViewModelBase, IEventCoordinatorViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public EventCoordinatorViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR($"Exit VM:{InstanceCountVM}", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the ViewModel
            // Initialize EventHandlers, Commands, etc.

            EventAggregator.GetEvent<ExecutePerformanceEvent>().Subscribe(PlayPerformance);

            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);

            PlayPerformanceEventCommand = new DelegateCommand(PlayPerformanceEvent, PlayPerformanceEventCanExecute);
            PlayDeviceSettingEventCommand = new DelegateCommand(PlayDeviceSettingEvent, PlayDeviceSettingEventCanExecute);
            PlayTestEventCommand = new DelegateCommand(PlayTestEvent, PlayTestEventCanExecute);

            DeviceSettings = PerformanceLibrary.AvailableDeviceSettings.Values.ToList();
            Performances = PerformanceLibrary.AvailablePerformances.Values.ToList();
            Tests = PerformanceLibrary.AvailableTests.Values.ToList();

            Message = "EventCoordinatorViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        private IEnumerable<Performance>? _deviceSettings;
        public IEnumerable<Performance>? DeviceSettings
        {
            get => _deviceSettings;
            set
            {
                _deviceSettings = value;
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

                PlayDeviceSettingEventCommand?.RaiseCanExecuteChanged();
                //PlayAdvancedServoSequenceCommand?.RaiseCanExecuteChanged();
                //EngageAndCenterCommand?.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

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

                PlayPerformanceEventCommand?.RaiseCanExecuteChanged();
                //PlayAdvancedServoSequenceCommand?.RaiseCanExecuteChanged();
                //EngageAndCenterCommand?.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private IEnumerable<Performance>? _tests;
        public IEnumerable<Performance>? Tests
        {
            get => _tests;
            set
            {
                _tests = value;
                OnPropertyChanged();
            }
        }

        private Performance? _selectedTest;
        public Performance? SelectedTest
        {
            get => _selectedTest;
            set
            {
                if (_selectedTest == value)
                {
                    return;
                }

                _selectedTest = value;
                OnPropertyChanged();

                PlayTestEventCommand?.RaiseCanExecuteChanged();
                //PlayAdvancedServoSequenceCommand?.RaiseCanExecuteChanged();
                //EngageAndCenterCommand?.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand?.RaiseCanExecuteChanged();
            }
        }

        private Performance? _eventPerformance;
        public Performance? EventPerformance
        {
            get => _eventPerformance;
            set
            {
                if (_eventPerformance == value)
                {
                    return;
                }

                _eventPerformance = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.DeviceChannelSequence? _eventPerformanceSequence;
        public VNCPhidgetConfig.DeviceChannelSequence? EventPerformanceSequence
        {
            get => _eventPerformanceSequence;
            set
            {
                if (_eventPerformanceSequence == value)
                {
                    return;
                }

                _eventPerformanceSequence = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        async void PlayPerformance(PerformanceEventArgs eventArgs)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.INFO($"Enter", Common.LOG_CATEGORY);

            EventPerformance = eventArgs.Performance;

            PerformancePlayer performancePlayer = new PerformancePlayer(EventAggregator, Common.PerformanceLibrary);

            await performancePlayer.ExecutePerformance(EventPerformance);

            if (Common.VNCLogging.EventHandler) Log.INFO($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        void PlayPerformanceSequence(SequenceEventArgs eventArgs)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.INFO($"Enter", Common.LOG_CATEGORY);

            //EventPerformanceSequence = eventArgs.DigitalOutputSequence;

            //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

            //performanceSequencePlayer.ExecutePerformanceSequence(eventArgs.DigitalOutputSequence);

            if (Common.VNCLogging.EventHandler) Log.INFO($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Public Methods (none)



        #endregion

        #region Commands

        #region PlayDeviceSettingEvent Command

        public DelegateCommand? PlayDeviceSettingEventCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayDeviceSettingEventCommand { get; set; }
        //public TYPE PlayDeviceSettingEventCommandParameter;
        public string PlayDeviceSettingEventContent { get; set; } = "RaisePlayDeviceSettingEventEvent";
        public string PlayDeviceSettingEventToolTip { get; set; } = "RaisePlayDeviceSettingEventEvent ToolTip";

        // Can get fancy and use Resources
        //public string RaisePlayDeviceSettingEventContent { get; set; } = "ViewName_RaisePlayDeviceSettingEventContent";
        //public string RaisePlayDeviceSettingEventToolTip { get; set; } = "ViewName_RaisePlayDeviceSettingEventContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RaisePlayDeviceSettingEventContent">RaisePlayDeviceSettingEvent</system:String>
        //    <system:String x:Key="ViewName_RaisePlayDeviceSettingEventContentToolTip">RaisePlayDeviceSettingEvent ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayDeviceSettingEvent(TYPE value)

        public void PlayDeviceSettingEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayDeviceSettingEvent) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayDeviceSettingEvent";
            PublishStatusMessage(Message);

            EventAggregator.GetEvent<ExecutePerformanceEvent>().Publish(
                new PerformanceEventArgs()
                {
                    Performance = SelectedDeviceSetting
                });

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayDeviceSettingEvent) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean PlayDeviceSettingEventCanExecute()
        {
            if (SelectedDeviceSetting is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayPerformanceEvent Command

        public DelegateCommand? PlayPerformanceEventCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayPerformanceEventCommand { get; set; }
        //public TYPE PlayPerformanceEventCommandParameter;
        public string PlayPerformanceEventContent { get; set; } = "RaisePlayPerformanceEventEvent";
        public string PlayPerformanceEventToolTip { get; set; } = "RaisePlayPerformanceEventEvent ToolTip";

        // Can get fancy and use Resources
        //public string RaisePlayPerformanceEventContent { get; set; } = "ViewName_RaisePlayPerformanceEventContent";
        //public string RaisePlayPerformanceEventToolTip { get; set; } = "ViewName_RaisePlayPerformanceEventContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RaisePlayPerformanceEventContent">RaisePlayPerformanceEvent</system:String>
        //    <system:String x:Key="ViewName_RaisePlayPerformanceEventContentToolTip">RaisePlayPerformanceEvent ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayPerformanceEvent(TYPE value)

        public void PlayPerformanceEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayPerformanceEvent) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayPerformanceEvent";
            PublishStatusMessage(Message);

            EventAggregator.GetEvent<ExecutePerformanceEvent>().Publish(
                new PerformanceEventArgs()
                {
                    Performance = SelectedPerformance
                });

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayPerformanceEvent) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean PlayPerformanceEventCanExecute()
        {
            if (SelectedPerformance is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region PlayTestEvent Command

        public DelegateCommand? PlayTestEventCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> PlayTestEventCommand { get; set; }
        //public TYPE PlayTestEventCommandParameter;
        public string PlayTestEventContent { get; set; } = "RaisePlayTestEventEvent";
        public string PlayTestEventToolTip { get; set; } = "RaisePlayTestEventEvent ToolTip";

        // Can get fancy and use Resources
        //public string RaisePlayTestEventContent { get; set; } = "ViewName_RaisePlayTestEventContent";
        //public string RaisePlayTestEventToolTip { get; set; } = "ViewName_RaisePlayTestEventContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RaisePlayTestEventContent">RaisePlayTestEvent</system:String>
        //    <system:String x:Key="ViewName_RaisePlayTestEventContentToolTip">RaisePlayTestEvent ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void PlayTestEvent(TYPE value)

        public void PlayTestEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(PlayTestEvent) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called PlayTestEvent";
            PublishStatusMessage(Message);

            EventAggregator.GetEvent<ExecutePerformanceEvent>().Publish(
                new PerformanceEventArgs()
                {
                    Performance = SelectedTest
                });

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(PlayTestEvent) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean PlayTestEventCanExecute()
        {
            if (SelectedTest is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region SayHello Command

        public ICommand? SayHelloCommand { get; private set; }

        private void SayHello()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "Hello";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Boolean SayHelloCanExecute()
        {
            return true;
        }

        #endregion

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods (none)



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
