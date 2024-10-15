using System;
using System.Windows;

using VNC;
using VNC.Core;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class RibbonShellViewModel : ViewModelBase, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public RibbonShellViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the ViewModel
            // Initialize EventHandlers, Commands, etc.

            DeveloperUIMode = Common.DeveloperUIMode;

            InformationApplication = Common.InformationApplication;
            InformationVNCCore = Common.InformationVNCCore;

            if (Common.VNCLogging.ViewLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private string _title = "VNCPhidgets21Explorer - RibbonShell";

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private System.Windows.Size _windowSize;
        public System.Windows.Size WindowSize
        {
            get => _windowSize;
            set
            {
                if (_windowSize == value)
                    return;
                _windowSize = value;
                OnPropertyChanged();
            }
        }

        private Visibility _developerUIMode = Visibility.Visible;
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

        public Information InformationApplication { get; set; }
        public Information InformationVNCCore { get; set; }

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
