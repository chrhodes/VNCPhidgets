﻿using System;
using System.Linq;
using System.Windows;

using VNCPhidget22Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class DataIntervalAndRateControl : ViewBase//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load

        public DataIntervalAndRateControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;

            InitializeComponent();

            // Wire up ViewModel if needed

            // If View First with ViewModel in Xaml

            // ViewModel = (IDataIntervalAndRateViewModel)DataContext;

            // Can create directly

            // ViewModel = DataIntervalAndRateViewModel();

            // Can use ourselves for everything

            //DataContext = this;

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public DataIntervalAndRateControl(IDataIntervalAndRateViewModel viewModel)
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

            lgMain.IsCollapsed = true;

            // Establish any additional DataContext(s), e.g. to things held in this View

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
    }
}
