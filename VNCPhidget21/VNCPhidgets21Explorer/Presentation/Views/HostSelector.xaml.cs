﻿using System;
using System.Windows;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class HostSelector : ViewBase, IHostSelector, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public HostSelector()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();
            
			// Expose ViewModel
						
            // If View First with ViewModel in Xaml

            // ViewModel = (IHostSelectorViewModel)DataContext;

            // Can create directly
            // ViewModel = HostSelectorViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        //public HostSelector(IHostSelectorViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountV++;
        //    InitializeComponent();

        //    ViewModel = viewModel;
            
        //    InitializeView();

        //    Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}
        
        private void InitializeView()
        {
            Int64 startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);
            
            // NOTE(crhodes)
            // Put things here that initialize the View
            
            Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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

        #endregion
    }
}
