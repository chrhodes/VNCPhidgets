﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget22Explorer.Presentation.ViewModels;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class AdvancedServo1061 : ViewBase, IAdvancedServo1061, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public AdvancedServo1061()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IAdvancedServo1061ViewModel)DataContext;

            // Can create directly
            // ViewModel = AdvancedServo1061ViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }
        
        public AdvancedServo1061(IAdvancedServo1061ViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

            InstanceCountVP++;
            InitializeComponent();

            ViewModel = viewModel;  // ViewBase sets the DataContext to ViewModel

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
            //Phidget1.DataContext = ViewModel;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)


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
