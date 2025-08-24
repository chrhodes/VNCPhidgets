using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget22Explorer.Presentation.ViewModels;


namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class MainDxDockLayoutManager : ViewBase, IMain, IInstanceCountV
    {
        //public MainDxDockLayoutManagerViewModel _viewModel;

        public MainDxDockLayoutManager(MainDxDockLayoutManagerViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Initialize SignalR", Common.LOG_CATEGORY);

            InstanceCountVP++;
            InitializeComponent();

            ViewModel = viewModel;  // ViewBase sets the DataContext to ViewModel
            //DataContext = _viewModel;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            ViewType = this.GetType().ToString().Split('.').Last();
            ViewModelType = ViewModel.GetType().ToString().Split('.').Last();
            ViewDataContextType = this.DataContext?.GetType().ToString().Split('.').Last();

            // NOTE(crhodes)
            // Put things here that initialize the View

            // Establish any additional DataContext(s), e.g. to things held in this View

            spDeveloperInfo.DataContext = this;
            Common.MainDockLayoutManagerControl = dlmControl;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void SaveLayout_Click(object sender, RoutedEventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SaveLayout_Click) Enter", Common.LOG_CATEGORY);

            dlmControl.SaveLayoutToXml(Common.cCONFIG_FILE);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SaveLayout_Click) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void RestoreLayout_Click(object sender, RoutedEventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RestoreLayout_Click) Enter", Common.LOG_CATEGORY);

            dlmControl.RestoreLayoutFromXml(Common.cCONFIG_FILE);

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RestoreLayout_Click) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #region IInstanceCountV

        private static Int32 _instanceCountV;

        public Int32 InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        private static Int32 _instanceCountVP;

        public Int32 InstanceCountVP
        {
            get => _instanceCountVP;
            set => _instanceCountVP = value;
        }

        #endregion
    }
}
