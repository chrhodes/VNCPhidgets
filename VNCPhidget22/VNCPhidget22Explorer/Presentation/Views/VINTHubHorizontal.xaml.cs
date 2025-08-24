using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

using Prism.Services.Dialogs;



//using System.Windows.Controls;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget22Explorer.Presentation.ViewModels;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class VINTHubHorizontal : ViewBase, IVINTHub, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public VINTHubHorizontal()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            //ViewModel = (IInterfaceKitViewModel)DataContext;

            // Can create directly
            // ViewModel = InterfaceKitViewModel();

            ViewModel = new VINTHubViewModel(
                Common.EventAggregator,
                (DialogService)Common.Container.Resolve(typeof(DialogService)));

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        public VINTHubHorizontal(IVINTHubViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            InstanceCountVP++;
            InitializeComponent();

            ViewModel = viewModel;   // ViewBase sets the DataContext to ViewModel

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

            // TODO(crhodes)
            // Figure out how to local handle lgInterfaceKits disabled/hidden until host selected
            //this.lgInterfaceKits.IsEnabled = true;
            this.lgPhidget22Status.IsCollapsed = true;

            lgHubPort0.IsCollapsed = false;
            lgHubPort1.IsCollapsed = true;
            lgHubPort2.IsCollapsed = true;
            lgHubPort3.IsCollapsed = true;
            lgHubPort4.IsCollapsed = true;
            lgHubPort5.IsCollapsed = true;

            // Establish any additional DataContext(s), e.g. to things held in this View

            spDeveloperInfo.DataContext = this;

            // TODO(crhodes)
            // Figure out how to get this info from either Manager or Open channel

            //Phidget1.DataContext = ViewModel;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)

        //private System.Windows.Size _windowSize;
        //public System.Windows.Size WindowSize
        //{
        //    get => _windowSize;
        //    set
        //    {
        //        if (_windowSize == value)
        //            return;
        //        _windowSize = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Event Handlers

        private void LayoutGroup_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutGroup lg = (LayoutGroup)sender;
            var mbe = e;

            var leftAltDown = Keyboard.IsKeyDown(Key.LeftAlt);
            var leftCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl);

            var rightAltDown = Keyboard.IsKeyDown(Key.RightAlt);
            var rightCtrlDown = Keyboard.IsKeyDown(Key.RightCtrl);

            var children = lg.Children;

            foreach (var child in children)
            {
                if (child.GetType() == typeof(DevExpress.Xpf.Editors.CheckEdit))
                {
                    if (leftCtrlDown || rightCtrlDown) { ((CheckEdit)child).IsChecked = true; }
                    if (leftAltDown || rightAltDown) { ((CheckEdit)child).IsChecked = false; }

                }
            }
        }

        //        private void thisControl_SizeChanged(object sender, SizeChangedEventArgs e)
        //        {
        //#if LOGGING
        //            Int64 startTicks = 0;
        //            if (Common.VNCCoreLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //#endif
        //            var newSize = e.NewSize;
        //            var previousSize = e.PreviousSize;
        //            WindowSize = newSize;

        //#if LOGGING
        //            if (Common.VNCCoreLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //#endif
        //        }

        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

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

        //private void SensorMode_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;
        //    var stuff = e.Source as string;

        //}
    }
}
