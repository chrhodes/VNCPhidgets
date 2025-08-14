using System;
using System.Windows;

using VNC;
using VNC.Core;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class ShellViewModel : ViewModelBase//, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public ShellViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR($"Exit VM:{InstanceCountVM}", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the ViewModel
            // Initialize EventHandlers, Commands, etc.

            Title = "VNCPhidget22Explorer - Shell";

            //DeveloperUIMode = Common.DeveloperUIMode;

            InformationApplication = Common.InformationApplication;
            InformationApplicationCore = Common.InformationApplicationCore;
            InformationVNCPhidget = Common.InformationVNCPhidget;
            InformationVNCPhidgetConfiguration = Common.InformationVNCPhidgetConfiguration;

            InformationPhidget22 = Common.InformationPhidget22;

            InformationVNCCore = Common.InformationVNCCore;

            if (Common.VNCLogging.ViewLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        public Information? InformationApplication { get; set; }
        public Information? InformationApplicationCore { get; set; }
        public Information? InformationVNCCore { get; set; }
        public Information? InformationVNCPhidget { get; set; }
        public Information? InformationVNCPhidgetConfiguration { get; set; }
        public Information? InformationPhidget22 { get; set; }

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion
    }
}
