using System;
using System.Windows;

using VNC;
using VNC.Core;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class ShellViewModel : ViewModelBase, IInstanceCountVM
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

            //DeveloperUIMode = Common.DeveloperUIMode;

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

        private string _title = "VNCPhidget22Explorer - Shell";

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

        //private System.Windows.Size _windowSize;
        //public System.Windows.Size WindowSize
        //{
        //    get => _windowSize;
        //    set
        //    {
        //        if (_windowSize == value)
        //            return;
        //        _windowSize = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Visibility _developerUIMode = Visibility.Visible;
        //public Visibility DeveloperUIMode
        //{
        //    get => _developerUIMode;
        //    set
        //    {
        //        if (_developerUIMode == value)
        //            return;
        //        _developerUIMode = value;
        //        OnPropertyChanged();
        //    }
        //}

        public Information InformationApplication { get; set; }
        public Information InformationVNCCore { get; set; }

        // TODO(crhodes)
        // Can we get rid of these?

        public string AssemblyVersionVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Version; }
        public string AssemblyNameVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Name; }
        public string AssemblyFullNameVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.FullName; }
        public string AssemblyTitleVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.AssemblyTitle; }
        //public string AssemblyAssemblyVersionVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.AssemblyVersion; }
        public string AssemblyCompanyVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Company; }
        public string AssemblyConfigurationVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Configuration; }
        public string AssemblyCopyrightVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Copyright; }
        public string AssemblyDescriptionVNCPhidget { get => Common.InformationVNCPhidget.AssemblyInformation.Description; }

        public string FileVersionVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.FileVersion; }
        public string FileDescriptionVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.FileDescription; }
        public string ProductNameVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductName; }
        public string InternalNameVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.InternalName; }
        public string ProductVersionVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductVersion; }
        public string ProductMajorPartVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductMajorPart; }
        public string ProductMinorPartVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductMinorPart; }
        public string ProductBuildPartVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductBuildPart; }
        public string ProductPrivatePartVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.ProductPrivatePart; }
        public string CommentsVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.Comments; }
        public string IsDebugVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.IsDebug.ToString(); }
        public string IsPatchedVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.IsPatched.ToString(); }
        public string IsPreReleaseVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.IsPreRelease.ToString(); }
        public string IsPrivateBuildVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.IsPrivateBuild.ToString(); }
        public string IsSpecialBuildVNCPhidget { get => Common.InformationVNCPhidget.FileInformation.IsSpecialBuild.ToString(); }



        // TODO(crhodes)
        // Convert above to use this (I think this is right)

        public Information InformationVNCPhidget { get; set; }

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

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
