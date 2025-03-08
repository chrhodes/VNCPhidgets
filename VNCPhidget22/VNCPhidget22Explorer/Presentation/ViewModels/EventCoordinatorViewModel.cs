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

            EventAggregator.GetEvent<PlayPerformanceEvent>().Subscribe(PlayPerformance);

            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);

            RaisePlayPerformanceEventCommand = new DelegateCommand(RaisePlayPerformanceEvent, RaisePlayPerformanceEventCanExecute);

            Performances = PerformanceLibrary.AvailablePerformances.Values.ToList();

            Message = "EventCoordinatorViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        private IEnumerable<Performance> _performances;
        public IEnumerable<Performance> Performances
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

                RaisePlayPerformanceEventCommand.RaiseCanExecuteChanged();
                //PlayAdvancedServoSequenceCommand.RaiseCanExecuteChanged();
                //EngageAndCenterCommand.RaiseCanExecuteChanged();
                //PlayInterfaceKitSequenceCommand.RaiseCanExecuteChanged();
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

        private VNCPhidgetConfig.DeviceClassSequence? _eventPerformanceSequence;
        public VNCPhidgetConfig.DeviceClassSequence? EventPerformanceSequence
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

        void PlayPerformance(PerformanceEventArgs eventArgs)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.Info($"Enter", Common.LOG_CATEGORY);

            EventPerformance = eventArgs.Performance;

            PerformancePlayer performancePlayer = new PerformancePlayer(EventAggregator);

            performancePlayer.PlayPerformance(EventPerformance);

            if (Common.VNCLogging.EventHandler) Log.Info($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        void PlayPerformanceSequence(SequenceEventArgs eventArgs)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.Info($"Enter", Common.LOG_CATEGORY);

            //EventPerformanceSequence = eventArgs.DigitalOutputSequence;

            //PerformanceSequencePlayer performanceSequencePlayer = new PerformanceSequencePlayer(EventAggregator);

            //performanceSequencePlayer.ExecutePerformanceSequence(eventArgs.DigitalOutputSequence);

            if (Common.VNCLogging.EventHandler) Log.Info($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Public Methods (none)



        #endregion

        #region Commands

        #region RaisePlayPerformanceEvent Command

        public DelegateCommand RaisePlayPerformanceEventCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> RaisePlayPerformanceEventCommand { get; set; }
        //public TYPE RaisePlayPerformanceEventCommandParameter;
        public string RaisePlayPerformanceEventContent { get; set; } = "RaiseRaisePlayPerformanceEventEvent";
        public string RaisePlayPerformanceEventToolTip { get; set; } = "RaiseRaisePlayPerformanceEventEvent ToolTip";

        // Can get fancy and use Resources
        //public string RaisePlayPerformanceEventContent { get; set; } = "ViewName_RaisePlayPerformanceEventContent";
        //public string RaisePlayPerformanceEventToolTip { get; set; } = "ViewName_RaisePlayPerformanceEventContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RaisePlayPerformanceEventContent">RaisePlayPerformanceEvent</system:String>
        //    <system:String x:Key="ViewName_RaisePlayPerformanceEventContentToolTip">RaisePlayPerformanceEvent ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void RaisePlayPerformanceEvent(TYPE value)

        public async void RaisePlayPerformanceEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RaisePlayPerformanceEvent) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called RaisePlayPerformanceEvent";
            PublishStatusMessage(Message);

            EventAggregator.GetEvent<PlayPerformanceEvent>().Publish(
                new PerformanceEventArgs()
                {
                    Performance = SelectedPerformance
                });

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RaisePlayPerformanceEvent) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool RaisePlayPerformanceEventCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
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

        #region SayHello Command

        public ICommand SayHelloCommand { get; private set; }

        private void SayHello()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "Hello";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private bool SayHelloCanExecute()
        {
            return true;
        }

        #endregion

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
