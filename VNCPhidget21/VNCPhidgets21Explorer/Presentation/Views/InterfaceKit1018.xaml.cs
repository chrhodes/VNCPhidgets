using System;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidgets21Explorer.Presentation.ViewModels;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class InterfaceKit1018 : ViewBase, IInterfaceKit, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public InterfaceKit1018()
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

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        public InterfaceKit1018(IInterfaceKitViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            ViewModel = viewModel;
            
            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View

            this.lgPhidgetStatus.IsCollapsed = true;

            if (Common.VNCLogging.ViewLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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
