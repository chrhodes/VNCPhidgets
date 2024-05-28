using System;
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

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class ManagePerformanceLibraryViewModel : EventViewModelBase, IManagePerformanceLibraryViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public ManagePerformanceLibraryViewModel(
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

            // TODO(crhodes)
            //
            
            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);


            ReloadPerformanceConfigFilesCommand = new DelegateCommand(ReloadPerformanceConfigFiles);
            ReloadAdvancedServoSequenceConfigFilesCommand = new DelegateCommand(ReloadAdvancedServoSequenceConfigFiles);
            ReloadInterfaceKitSequenceConfigFilesCommand = new DelegateCommand(ReloadInterfaceKitSequenceConfigFiles);
            ReloadStepperSequenceConfigFilesCommand = new DelegateCommand(ReloadStepperSequenceConfigFiles);

            Message = "ManagePerformanceLibraryViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        public ICommand SayHelloCommand { get; private set; }
        
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

        #endregion

        #region Commands

        #region Reload Config Files

        public ICommand ReloadPerformanceConfigFilesCommand { get; private set; }
        public ICommand ReloadAdvancedServoSequenceConfigFilesCommand { get; private set; }
        public ICommand ReloadInterfaceKitSequenceConfigFilesCommand { get; private set; }
        public ICommand ReloadStepperSequenceConfigFilesCommand { get; private set; }

        private void ReloadPerformanceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadPerformanceConfigFiles Clicked";

            //LoadPerformancesConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadAdvancedServoSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadAdvancedServoSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadAdvanceServoConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadInterfaceKitSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadInterfaceKitSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadInterfaceKitConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadStepperSequenceConfigFiles()
        {
            Int64 startTicks = Log.Info("Enter", Common.LOG_CATEGORY);

            Message = "ReloadStepperSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadStepperConfig();

            Log.Info("End", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion


        #region Event Handlers



        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


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
