using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidgets21Explorer.Presentation.ViewModels;

namespace VNCPhidgets21Explorer.Presentation.Views
{
    public partial class RibbonShell : Window, IInstanceCountV, INotifyPropertyChanged
    {
        public RibbonShellViewModel _viewModel;

        public RibbonShell(RibbonShellViewModel viewModel)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);


            InstanceCountV++;
            InitializeComponent();
            InitializeView();

            _viewModel = viewModel;
            DataContext = _viewModel;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            Common.CurrentRibbonShell = this;
            DeveloperUIMode = Common.DeveloperUIMode;

            // NOTE(crhodes)
            // Put things here that initialize the View

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Visibility _developerUIMode = Visibility.Collapsed;
        public Visibility DeveloperUIMode
        {
            get => _developerUIMode;
            set
            {
                if (_developerUIMode == value)
                    return;
                _developerUIMode = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        // This is the traditional approach - requires string name to be passed in

        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            long startTicks = 0;
            if (Common.VNCLogging.INPC) startTicks = Log.VIEW_LOW($"Enter ({propertyName})", Common.LOG_CATEGORY);

            // This is the new CompilerServices attribute!

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (Common.VNCLogging.INPC) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

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
