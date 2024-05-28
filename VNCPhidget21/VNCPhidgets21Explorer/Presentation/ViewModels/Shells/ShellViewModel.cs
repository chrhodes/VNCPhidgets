﻿using System;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class ShellViewModel : ViewModelBase, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public ShellViewModel()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)

        private string _title = "VNCPhidgets21Explorer - Shell";

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

        //public string RuntimeVersion { get => Common.RuntimeVersion; }
        //public string FileVersion { get => Common.FileVersion; }
        //public string ProductVersion { get => Common.ProductVersion; }
        //public string ProductName { get => Common.ProductName; }

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
