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

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class ManagePerformanceLibraryViewModel : EventViewModelBase, IManagePerformanceLibraryViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public ManagePerformanceLibraryViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //
            
            SayHelloCommand = new DelegateCommand(SayHello, SayHelloCanExecute);

            ReloadPerformanceConfigFilesCommand = new DelegateCommand(ReloadPerformanceConfigFiles);
            ReloadAdvancedServoSequenceConfigFilesCommand = new DelegateCommand(ReloadAdvancedServoSequenceConfigFiles);
            ReloadInterfaceKitSequenceConfigFilesCommand = new DelegateCommand(ReloadInterfaceKitSequenceConfigFiles);
            ReloadStepperSequenceConfigFilesCommand = new DelegateCommand(ReloadStepperSequenceConfigFiles);

            Message = "ManagePerformanceLibraryViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ReloadPerformanceConfigFiles) Enter", Common.LOG_CATEGORY);

            Message = "ReloadPerformanceConfigFiles Clicked";

            //LoadPerformancesConfig();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ReloadSteReloadPerformanceConfigFilespperSequenceConfigFiles) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadAdvancedServoSequenceConfigFiles()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ReloadAdvancedServoSequenceConfigFiles) Enter", Common.LOG_CATEGORY);

            Message = "ReloadAdvancedServoSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadAdvanceServoConfig();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ReloadAdvancedServoSequenceConfigFiles) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadInterfaceKitSequenceConfigFiles()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ReloadInterfaceKitSequenceConfigFiles) Enter", Common.LOG_CATEGORY);

            Message = "ReloadInterfaceKitSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadInterfaceKitConfig();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ReloadInterfaceKitSequenceConfigFiles) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadStepperSequenceConfigFiles()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ReloadStepperSequenceConfigFiles) Enter", Common.LOG_CATEGORY);

            Message = "ReloadStepperSequenceConfigFiles Clicked";

            // TODO(crhodes)
            // Call something in PerformanceSequencePlayer

            //LoadStepperConfig();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ReloadStepperSequenceConfigFiles) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region SayHello Command

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

        #region Event Handlers (none)



        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods



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
