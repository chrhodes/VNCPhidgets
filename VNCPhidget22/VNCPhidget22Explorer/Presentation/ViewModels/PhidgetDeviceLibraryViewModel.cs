using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Xpf.Grid;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;
using VNC.Core.Services;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Players;

using VNCPhidget22Explorer.Core.Events;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class PhidgetDeviceLibraryViewModel : EventViewModelBase, IPhidgetDeviceLibraryViewModel//, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public PhidgetDeviceLibraryViewModel(
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

            SayHelloCommand = new DelegateCommand(
            SayHello, SayHelloCanExecute);

            ExportGridCommand = new DelegateCommand<GridControl>(ExportGrid, ExportGridCanExecute);
            EventAggregator.GetEvent<SelectedCollectionChangedEvent>().Subscribe(CollectionChanged);

            Message = "PhidgetDeviceLibraryViewModel says hello";
            PublishStatusMessage(Message);

            PhidgetDeviceLibrary = Common.PhidgetDeviceLibrary;

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private VNC.Phidget22.PhidgetDeviceLibrary? _phidgetDeviceLibrary;
        
        public VNC.Phidget22.PhidgetDeviceLibrary? PhidgetDeviceLibrary
        {
            get => _phidgetDeviceLibrary;
            set
            {
                if (_phidgetDeviceLibrary == value)
                    return;
                _phidgetDeviceLibrary = value;
                OnPropertyChanged();
            }
        }

        private string? _outputFileNameAndPath;
        public string? OutputFileNameAndPath
        {
            get => _outputFileNameAndPath;
            set
            {
                if (_outputFileNameAndPath == value)
                    return;
                _outputFileNameAndPath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers (none)

        public virtual void CollectionChanged(SelectedCollectionChangedEventArgs args)
        {
            OutputFileNameAndPath = $@"C:\temp\{args.Name}-TYPE";
        }

        #endregion

        #region Public Methods (none)


        #endregion

        #region Commands

        #region SayHello Command

        public ICommand? SayHelloCommand { get; private set; }

        private void SayHello()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "Hello";
            PublishStatusMessage(Message);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Boolean SayHelloCanExecute()
        {
            return true;
        }

        #endregion

        #region ExportGrid Command

        public DelegateCommand<GridControl>? ExportGridCommand { get; set; }

        public string ExportGridContent { get; set; } = "ExportGrid";
        public string ExportGridToolTip { get; set; } = "ExportGrid ToolTip";

        // Can get fancy and use Resources
        //public string ExportGridContent { get; set; } = "ViewName_ExportGridContent";
        //public string ExportGridToolTip { get; set; } = "ViewName_ExportGridContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ExportGridContent">ExportGrid</system:String>
        //    <system:String x:Key="ViewName_ExportGridContentToolTip">ExportGrid ToolTip</system:String>

        public void ExportGrid(GridControl gridControl)
        {
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called ExportGrid";
            PublishStatusMessage(Message);

            var dialogParameters = new DialogParameters();

            dialogParameters.Add("message", $"Message)");
            dialogParameters.Add("title", "Exception");
            dialogParameters.Add("gridcontrol", gridControl);

            // TODO(crhodes)
            // Add some more context to name, e.g. Org, Team Project, ???

            dialogParameters.Add("outputfilenameandpath", OutputFileNameAndPath);

            DialogService.Show("ExportGridDialog", dialogParameters, r =>
            {
            });
        }

        public bool ExportGridCanExecute(GridControl gridControl)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion ExportGrid Command


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


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
