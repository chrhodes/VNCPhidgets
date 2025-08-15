using System;
using System.Linq;
using System.Windows;
//using System.Windows.Controls;

using Prism.Events;

using VNC;
using VNC.Core.Mvvm;
using VNC.Core.Presentation;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;
using VNC.WPF.Presentation.Views;

using VNCPhidget22Explorer.Presentation.ViewModels;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class PerformanceInfoControl : ViewBase//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load

        public PerformanceInfoControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;

            InitializeComponent();

            // Wire up ViewModel if needed

            // If View First with ViewModel in Xaml

            // ViewModel = (IAdvancedServoBoardSequencesControlsViewModel)DataContext;

            // Can create directly

            // ViewModel = AdvancedServoBoardSequencesControlsViewModel();

            // Can use ourselves for everything

            //DataContext = this;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public AdvancedServoBoardSequencesControl(IAdvancedServoBoardSequencesControlsViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountVP++;

        //    InitializeComponent();

        //    ViewModel = viewModel;  // ViewBase sets the DataContext to ViewModel

        //    // For the rare case where the ViewModel needs to know about the View
        //    // ViewModel.View = this;

        //    InitializeView();

        //    if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View
            // Hook eventhandlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();
            ViewDataContextType = this.DataContext?.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            lgPerformanceInfo.IsCollapsed = true;
            //spDeveloperInfo.DataContext = this;
            //lgSequences.IsCollapsed = true;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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

        #region Commands (none)



        #endregion

        #region Public Methods (none)



        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods (none)



        #endregion

        //#region IInstanceCount

        //private static Int32 _instanceCountV;

        //public Int32 InstanceCountV
        //{
        //    get => _instanceCountV;
        //    set => _instanceCountV = value;
        //}

        //private static Int32 _instanceCountVP;

        //public Int32 InstanceCountVP
        //{
        //    get => _instanceCountVP;
        //    set => _instanceCountVP = value;
        //}

        //#endregion

        public static WindowHost _aboutHost = null;

        private void ListBoxEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var v2 = (DevExpress.Xpf.Editors.ListBoxEdit)e.Source;

            Performance emptyPerformance = (Performance)v2.EditValue;
            var v4 = emptyPerformance.Name;

            Performance performance;

            // NOTE(crhodes)
            // Launch a Modal Window after looking up Performance

            if (_aboutHost is null) _aboutHost = new WindowHost(Common.EventAggregator);

            System.Windows.Controls.UserControl userControl = (Views.PerformanceInfoControl)Common.Container.Resolve(typeof(Views.PerformanceInfoControl));

            // TODO(crhodes)
            // Lookup Performance

            if (PerformanceLibrary.AvailablePerformances.ContainsKey(emptyPerformance.Name ?? ""))
            {
                performance = PerformanceLibrary.AvailablePerformances[emptyPerformance.Name];

                userControl.DataContext = performance;

                _aboutHost.DisplayUserControlInHost(
                    "VNCPhidgets22Explorer About",
                        Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                    //(Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
                    //(Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
                    ShowWindowMode.Modal_ShowDialog,
                    userControl
                );
            }
            else
            {
                Log.ERROR($"Cannot find performance:>{emptyPerformance?.Name}<", Common.LOG_CATEGORY);
            }
        }
    }
}
