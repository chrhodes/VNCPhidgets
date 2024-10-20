using System;
using System.Linq;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class AppVersionInfo : ViewBase
    {
        #region Constructors, Initialization, and Load
        
        public AppVersionInfo()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IAppVersionInfoViewModel)DataContext;

            // Can create directly
            // ViewModel = AppVersionInfoViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        //public AppVersionInfo(IAppVersionInfoViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountV++;
        //    InitializeComponent();

        //    ViewModel = viewModel;

        //    InitializeView();

        //    Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            ViewType = this.GetType().ToString().Split('.').Last();

            // NOTE(crhodes)
            // Put things here that initialize the View

            spMain.DataContext = Common.InformationApplication;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion
    }
}
