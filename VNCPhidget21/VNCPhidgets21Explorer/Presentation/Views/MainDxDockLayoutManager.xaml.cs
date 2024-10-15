using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidgets21Explorer.Presentation.ViewModels;


namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class MainDxDockLayoutManager : ViewBase, IMain, IInstanceCountV
    {
        public MainDxDockLayoutManagerViewModel _viewModel;

        public MainDxDockLayoutManager(MainDxDockLayoutManagerViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Initialize SignalR", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            ViewType = this.GetType().ToString().Split('.').Last();

            // NOTE(crhodes)
            // Put things here that initialize the View

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void SaveLayout_Click(object sender, RoutedEventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SaveLayout_Click) Enter", Common.LOG_CATEGORY);

            dlm.SaveLayoutToXml(Common.cCONFIG_FILE);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SaveLayout_Click) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void RestoreLayout_Click(object sender, RoutedEventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RestoreLayout_Click) Enter", Common.LOG_CATEGORY);

            dlm.RestoreLayoutFromXml(Common.cCONFIG_FILE);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RestoreLayout_Click) Exit", Common.LOG_CATEGORY, startTicks);
        }

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
