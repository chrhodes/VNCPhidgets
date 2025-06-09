using System;
using System.Linq;
using System.Windows.Forms;

using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Editors;
using DevExpress.XtraRichEdit.Import.OpenXml;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22.Configuration;

using VNCPhidget22Explorer.Presentation.ViewModels;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class PerformanceSelectorControl : ViewBase//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public PerformanceSelectorControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IHostSelectorViewModel)DataContext;

            // Can create directly
            // ViewModel = HostSelectorViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }        
        
        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            ViewType = this.GetType().ToString().Split('.').Last();
            ViewDataContextType = this.DataContext?.GetType().ToString().Split('.').Last();

            // NOTE(crhodes)
            // Put things here that initialize the View

            lgPerformances.IsCollapsed = true;
            lgChananelClassSequences.IsCollapsed = true;

            //lgDigitalOutputSequences.IsCollapsed = true;

            //lgRCServoSequences.IsCollapsed = true;

            //lgStepperSequences.IsCollapsed = true;

            // Establish any additional DataContext(s), e.g. to things held in this View
            spDeveloperInfo.DataContext = this;

            // HACK(crhodes)
            // Trying stuff
            //cbePerformances.ItemsSource = new BindingSource(Common.PerformanceLibrary.AvailablePerformances, null);

            //Common.PerformanceLibrary.AvailablePerformances.CollectionChanged += AvailablePerformances_CollectionChanged;
            //cbePerformances.ItemsSource = Common.PerformanceLibrary.AvailablePerformances;

            //cbePerformances.ItemsSource = Common.PerformanceLibrary.AvailablePerformances.Values.ToList();

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void AvailablePerformances_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Log.Trace("PerformanceSelectorControl notified AvailablePerformances_CollectionChanged", Common.LOG_CATEGORY);
            cbePerformances.ItemsSource = new BindingSource(PerformanceLibrary.AvailablePerformances, null);
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

        private void TextEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var viewModel = (HackAroundViewModel)DataContext;

            // TODO(crhodes)
            // Figure out how to 

            viewModel.SerialNumber = null;
        }
    }
}
