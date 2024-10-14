using System;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidgets21Explorer.Presentation.ViewModels;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class MainDxLayoutControl : ViewBase, IMain, IInstanceCountV
    {
        public MainDxLayoutControlViewModel _viewModel;

        public MainDxLayoutControl(MainDxLayoutControlViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Initialize SignalR", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        //private void SaveLayout_Click(object sender, RoutedEventArgs e)
        //{
        //    lg_Body_dlm.SaveLayoutToXml(Common.cCONFIG_FILE);
        //}

        //private void RestoreLayout_Click(object sender, RoutedEventArgs e)
        //{
        //    lg_Body_dlm.RestoreLayoutFromXml(Common.cCONFIG_FILE);
        //}

        #region IInstanceCount

        private static int _instanceCountV;

        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        private static int _instanceCountVP;

        public int InstanceCountVP
        {
            get => _instanceCountVP;
            set => _instanceCountVP = value;
        }

        #endregion        
    }
}
