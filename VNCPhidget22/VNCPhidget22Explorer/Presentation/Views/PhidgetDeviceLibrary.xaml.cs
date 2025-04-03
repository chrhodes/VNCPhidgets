using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget22Explorer.Presentation.ViewModels;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class PhidgetDeviceLibrary : ViewBase, IPhidgetDeviceLibrary, IInstanceCountV
    {
        #region Constructors, Initialization, and Load

        public PhidgetDeviceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;

            InitializeComponent();

            // Wire up ViewModel if needed

            // If View First with ViewModel in Xaml

            // ViewModel = (IPhidgetDeviceLibraryViewModel)DataContext;

            // Can create directly

            // ViewModel = Common.PhidgetDeviceLibraryViewModel();

            // Can use ourselves for everything

            //DataContext = this;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public PhidgetDeviceLibrary(IPhidgetDeviceLibraryViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()})", Common.LOG_CATEGORY);

            InstanceCountVP++;

            InitializeComponent();

            ViewModel = viewModel;  // ViewBase sets the DataContext to ViewModel

            // For the rare case where the ViewModel needs to know about the View
            // ViewModel.View = this;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View
            // Hook eventhandlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();
            ViewModelType = ViewModel.GetType().ToString().Split('.').Last();
            ViewDataContextType = this.DataContext?.GetType().ToString().Split('.').Last();

            lgChannels.IsCollapsed = true;

            // TODO(crhodes)
            // Add to this list as new ChannelClass are supported

            lgHubs.IsCollapsed = true;
            lgDigitalInputs.IsCollapsed = true;
            lgDigitalOutputs.IsCollapsed = true;
            lgRCServos.IsCollapsed = true;
            lgSteppers.IsCollapsed = true;
            lgVoltageInputs.IsCollapsed = true;
            lgVoltageRatioInputs.IsCollapsed = true;
            lgVoltageOutputs.IsCollapsed = true;

            // Establish any additional DataContext(s), e.g. to things held in this View

            spDeveloperInfo.DataContext = this;

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

        #region IInstanceCount

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
