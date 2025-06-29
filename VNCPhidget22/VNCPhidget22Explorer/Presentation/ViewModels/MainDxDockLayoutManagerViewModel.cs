﻿using System;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class MainDxDockLayoutManagerViewModel : EventViewModelBase//, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public MainDxDockLayoutManagerViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);


            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            DeveloperModeCommand = new DelegateCommand(DeveloperMode, DeveloperModeCanExecute);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)



        #endregion

        #region Event Handlers (none)


        #endregion

        #region Commands

        #region DeveloperMode Command

        public DelegateCommand? DeveloperModeCommand { get; set; }
        //public TYPE DeveloperModeCommandParameter;
        public string DeveloperModeContent { get; set; } = "DeveloperMode";
        public string? DeveloperModeToolTip { get; set; } = "DeveloperMode ToolTip";

        // Can get fancy and use Resources
        //public string DeveloperModeContent { get; set; } = "ViewName_DeveloperModeContent";
        //public string DeveloperModeToolTip { get; set; } = "ViewName_DeveloperModeContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_DeveloperModeContent">DeveloperMode</system:String>
        //    <system:String x:Key="ViewName_DeveloperModeContentToolTip">DeveloperMode ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void DeveloperMode(TYPE value)
        public void DeveloperMode()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(DeveloperMode) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called DeveloperMode";
            PublishStatusMessage(Message);

            EventAggregator.GetEvent<StatusMessageEvent>().Publish(Message);

            if (Common.DeveloperMode)
            {
                if (Common.CurrentShell is not null) Common.CurrentShell.DeveloperUIMode = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (Common.CurrentShell is not null) Common.CurrentShell.DeveloperUIMode = System.Windows.Visibility.Visible;
            }

            Common.DeveloperMode = !Common.DeveloperMode;

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<DeveloperModeEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<DeveloperModeEvent>().Publish(
            //      new DeveloperModeEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class DeveloperModeEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<DeveloperModeEvent>().Subscribe(DeveloperMode);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(DeveloperMode) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean DeveloperModeCanExecute(TYPE value)
        public Boolean DeveloperModeCanExecute()
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
