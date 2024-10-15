using System;
using System.Windows;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using System.Linq;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class ManagePerformanceLibrary : ViewBase, IManagePerformanceLibrary, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public ManagePerformanceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IManagePerformanceLibraryViewModel)DataContext;

            // Can create directly
            // ViewModel = ManagePerformanceLibraryViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        public ManagePerformanceLibrary(IManagePerformanceLibraryViewModel viewModel)
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

            ViewType = this.GetType().ToString().Split('.').Last();

            // NOTE(crhodes)
            // Put things here that initialize the View

            if (Common.VNCLogging.ViewLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)


        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)
         
        #endregion

        #region Public Methods (None)


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


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
