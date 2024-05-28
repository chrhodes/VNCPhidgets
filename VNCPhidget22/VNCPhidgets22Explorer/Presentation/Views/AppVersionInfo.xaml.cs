using System;
using System.Windows;

using VNCPhidgets22Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets22Explorer.Presentation.Views
{
    public partial class AppVersionInfo
    {
        #region Constructors, Initialization, and Load
        
        public AppVersionInfo()
        {
            //Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IAppVersionInfoViewModel)DataContext;

            // Can create directly
            // ViewModel = AppVersionInfoViewModel();

            // InitializeView();

            //Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
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

        //private void InitializeView()
        //{
        //    Int64 startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);
            
        //    // NOTE(crhodes)
        //    // Put things here that initialize the View
            
        //    Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        //}
        
        #endregion

    }
}
