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

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class ManagePerformanceLibraryViewModel : EventViewModelBase, IManagePerformanceLibraryViewModel//, IInstanceCountVM
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

            // NOTE(crhodes)
            // Because we initialize the Commands here and not in constructor,
            // Have to declare them as Nullable.  Seems like the analyzer should be able to figure 
            // out that we call this method :(
            
            SayHelloCommand = new DelegateCommand(SayHello, SayHelloCanExecute);

            ReloadPerformanceConfigFileCommand = new DelegateCommand(ReloadPerformanceConfigFile);

            ReloadDigitalInputSequenceConfigFileCommand = new DelegateCommand(ReloadDigitalInputSequenceConfigFile);
            ReloadDigitalOutputSequenceConfigFileCommand = new DelegateCommand(ReloadDigitalOutputSequenceConfigFile);
            ReloadRCServoSequenceConfigFileCommand = new DelegateCommand(ReloadRCServoSequenceConfigFile);
            ReloadStepperSequenceConfigFileCommand = new DelegateCommand(ReloadStepperSequenceConfigFile);
            ReloadVoltageInputSequenceConfigFileCommand = new DelegateCommand(ReloadVoltageInputSequenceConfigFile);
            ReloadVoltageRatioInputSequenceConfigFileCommand = new DelegateCommand(ReloadVoltageRatioInputSequenceConfigFile);
            ReloadVoltageOutputSequenceConfigFileCommand = new DelegateCommand(ReloadVoltageOutputSequenceConfigFile);

            Message = "ManagePerformanceLibraryViewModel says hello";
            PublishStatusMessage(Message);

            PerformanceConfigFiles = PerformanceLibrary.GetListOfPerformanceConfigFiles();
            Performances = Common.PerformanceLibrary.AvailablePerformances.Values.ToList();

            DigitalInputSequenceConfigFiles = PerformanceLibrary.GetListOfDigitalInputConfigFiles();
            DigitalInputSequences = PerformanceLibrary.AvailableDigitalInputSequences.Values.ToList();

            DigitalOutputSequenceConfigFiles = PerformanceLibrary.GetListOfDigitalOutputConfigFiles();
            DigitalOutputSequences = PerformanceLibrary.AvailableDigitalOutputSequences.Values.ToList();

            RCServoSequenceConfigFiles = PerformanceLibrary.GetListOfRCServoConfigFiles();
            RCServoSequences = PerformanceLibrary.AvailableRCServoSequences.Values.ToList();

            StepperSequenceConfigFiles = PerformanceLibrary.GetListOfStepperConfigFiles();
            StepperSequences = PerformanceLibrary.AvailableStepperSequences.Values.ToList();

            VoltageInputSequenceConfigFiles = PerformanceLibrary.GetListOfVoltageInputConfigFiles();
            VoltageInputSequences = PerformanceLibrary.AvailableVoltageInputSequences.Values.ToList();

            VoltageOutputSequenceConfigFiles = PerformanceLibrary.GetListOfVoltageOutputConfigFiles();
            VoltageOutputSequences = PerformanceLibrary.AvailableVoltageOutputSequences.Values.ToList();


            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        public ICommand? SayHelloCommand { get; private set; }

        private IEnumerable<string>? _performanceConfigFiles;
        public IEnumerable<string>? PerformanceConfigFiles
        {
            get => _performanceConfigFiles;
            set
            {
                _performanceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedPerformanceConfigFile;
        public string? SelectedPerformanceConfigFile
        {
            get => _selectedPerformanceConfigFile;
            set
            {
                if (_selectedPerformanceConfigFile == value)
                    return;
                _selectedPerformanceConfigFile = value;
                OnPropertyChanged();
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

        private IEnumerable<string>? _digitalInputSequenceConfigFiles;
        public IEnumerable<string>? DigitalInputSequenceConfigFiles
        {
            get => _digitalInputSequenceConfigFiles;
            set
            {
                _digitalInputSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedDigitalInputSequenceConfigFile;
        public string? SelectedDigitalInputSequenceConfigFile
        {
            get => _selectedDigitalInputSequenceConfigFile;
            set
            {
                if (_selectedDigitalInputSequenceConfigFile == value)
                    return;
                _selectedDigitalInputSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _digitalInputSequences;
        public IEnumerable<ChannelSequence>? DigitalInputSequences
        {
            get => _digitalInputSequences;
            set
            {
                _digitalInputSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _digitalOutputSequenceConfigFiles;
        public IEnumerable<string>? DigitalOutputSequenceConfigFiles
        {
            get => _digitalOutputSequenceConfigFiles;
            set
            {
                _digitalOutputSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedDigitalOutputSequenceConfigFile;
        public string? SelectedDigitalOutputSequenceConfigFile
        {
            get => _selectedDigitalOutputSequenceConfigFile;
            set
            {
                if (_selectedDigitalOutputSequenceConfigFile == value)
                    return;
                _selectedDigitalOutputSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _digitalOutputSequences;
        public IEnumerable<ChannelSequence>? DigitalOutputSequences
        {
            get => _digitalOutputSequences;
            set
            {
                _digitalOutputSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _rcServoSequenceConfigFiles;
        public IEnumerable<string>? RCServoSequenceConfigFiles
        {
            get => _rcServoSequenceConfigFiles;
            set
            {
                _rcServoSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedRCServoSequenceConfigFile;
        public string? SelectedRCServoSequenceConfigFile
        {
            get => _selectedRCServoSequenceConfigFile;
            set
            {
                if (_selectedRCServoSequenceConfigFile == value)
                    return;
                _selectedRCServoSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _rcServoSequences;
        public IEnumerable<ChannelSequence>? RCServoSequences
        {
            get => _rcServoSequences;
            set
            {
                _rcServoSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _stepperSequenceConfigFiles;
        public IEnumerable<string>? StepperSequenceConfigFiles
        {
            get => _stepperSequenceConfigFiles;
            set
            {
                _stepperSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedStepperSequenceConfigFile;
        public string? SelectedStepperSequenceConfigFile
        {
            get => _selectedStepperSequenceConfigFile;
            set
            {
                if (_selectedStepperSequenceConfigFile == value)
                    return;
                _selectedStepperSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _stepperSequences;
        public IEnumerable<ChannelSequence>? StepperSequences
        {
            get => _stepperSequences;
            set
            {
                _stepperSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _voltageInputSequenceConfigFiles;
        public IEnumerable<string>? VoltageInputSequenceConfigFiles
        {
            get => _voltageInputSequenceConfigFiles;
            set
            {
                _voltageInputSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedVoltageInputSequenceConfigFile;
        public string? SelectedVoltageInputSequenceConfigFile
        {
            get => _selectedVoltageInputSequenceConfigFile;
            set
            {
                if (_selectedVoltageInputSequenceConfigFile == value)
                    return;
                _selectedVoltageInputSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _voltageInputSequences;
        public IEnumerable<ChannelSequence>? VoltageInputSequences
        {
            get => _voltageInputSequences;
            set
            {
                _voltageInputSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _voltageRatioInputSequenceConfigFiles;
        public IEnumerable<string>? VoltageRatioInputSequenceConfigFiles
        {
            get => _voltageRatioInputSequenceConfigFiles;
            set
            {
                _voltageRatioInputSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedVoltageRatioInputSequenceConfigFile;
        public string? SelectedVoltageRatioInputSequenceConfigFile
        {
            get => _selectedVoltageRatioInputSequenceConfigFile;
            set
            {
                if (_selectedVoltageRatioInputSequenceConfigFile == value)
                    return;
                _selectedVoltageRatioInputSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _voltageRatioInputSequences;
        public IEnumerable<ChannelSequence>? VoltageRatioInputSequences
        {
            get => _voltageRatioInputSequences;
            set
            {
                _voltageRatioInputSequences = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string>? _voltageOutputSequenceConfigFiles;
        public IEnumerable<string>? VoltageOutputSequenceConfigFiles
        {
            get => _voltageOutputSequenceConfigFiles;
            set
            {
                _voltageOutputSequenceConfigFiles = value;
                OnPropertyChanged();
            }
        }

        private string? _selectedVoltageOutputSequenceConfigFile;
        public string? SelectedVoltageOutputSequenceConfigFile
        {
            get => _selectedVoltageOutputSequenceConfigFile;
            set
            {
                if (_selectedVoltageOutputSequenceConfigFile == value)
                    return;
                _selectedVoltageOutputSequenceConfigFile = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<ChannelSequence>? _voltageOutputSequences;
        public IEnumerable<ChannelSequence>? VoltageOutputSequences
        {
            get => _voltageOutputSequences;
            set
            {
                _voltageOutputSequences = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #region Reload Config Files

        public ICommand? ReloadPerformanceConfigFileCommand { get; private set; }

        public ICommand? ReloadDigitalInputSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadDigitalOutputSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadRCServoSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadStepperSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadVoltageInputSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadVoltageRatioInputSequenceConfigFileCommand { get; private set; }
        public ICommand? ReloadVoltageOutputSequenceConfigFileCommand { get; private set; }

        // TODO(crhodes)
        // This could be ReloadConfigFile(string configCategory)
        // with a switch.  See ManagePerformanceLibrary.xaml

        private void ReloadPerformanceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"ReloadPerformanceConfigFile Clicked - >{SelectedPerformanceConfigFile}<";
            PublishStatusMessage(Message);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            Common.PerformanceLibrary.LoadPerformancesFromConfigFile(SelectedPerformanceConfigFile, reload: true);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadDigitalInputSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadDigitalInputConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadDigitalInputSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }
        private void ReloadDigitalOutputSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadDigitalOutputSequenceConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadDigitalOutputSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadRCServoSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadRCServoSequenceConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadRCServoSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadStepperSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadStepperSequenceConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadStepperSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadVoltageInputSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadVoltageInputConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadVoltageInputSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadVoltageRatioInputSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadVoltageRatioInputConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadVoltageRatioInputSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ReloadVoltageOutputSequenceConfigFile()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "ReloadVoltageOutputSequenceConfigFile Clicked";
            PublishStatusMessage(Message);

            Common.PerformanceLibrary.LoadVoltageOutputSequences(reload: true);

            // TODO(crhodes)
            // Reload config file and update PerformanceLibrary

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SayHello) Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";
            PublishStatusMessage(Message);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SayHello) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Boolean SayHelloCanExecute()
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
