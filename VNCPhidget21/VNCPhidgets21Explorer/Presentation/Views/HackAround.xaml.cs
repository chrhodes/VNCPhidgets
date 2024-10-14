using System;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidgets21Explorer.Presentation.ViewModels;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class HackAround : ViewBase, IInstanceCountV
    {
        public HackAroundViewModel _viewModel;

        public HackAround(HackAroundViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()})", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

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
