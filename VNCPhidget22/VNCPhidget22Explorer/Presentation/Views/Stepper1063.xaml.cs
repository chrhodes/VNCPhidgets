using System;
using System.Linq;
using System.Windows;

using DevExpress.XtraRichEdit.Layout.Engine;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget2221Explorer.Presentation.ViewModels;

namespace VNCPhidget2221Explorer.Presentation.Views
{
    public partial class Stepper1063 : ViewBase, IStepper1063, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public Stepper1063()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IStepper1063ViewModel)DataContext;

            // Can create directly
            // ViewModel = Stepper1063ViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        public Stepper1063(IStepper1063ViewModel viewModel)
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

            // NOTE(crhodes)
            // Put things here that initialize the View

            this.lgPhidget22tatus.IsCollapsed = true;

            // Establish any additional DataContext(s), e.g. to things held in this View

            spDeveloperInfo.DataContext = this;
            Phidget1.DataContext = ViewModel;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (none)


        #endregion

        #region Event Handlers (none)


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
