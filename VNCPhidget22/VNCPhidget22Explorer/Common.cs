using System;
using System.Reflection;

using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

//using VNC.Phidget22.Configuration;


////using VNC.Phidget22;
////using VNC.Phidget22.Configuration.Performance;
////using VNC.WPF.Presentation.Dx.Views;

using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer
{
    public class Common : VNC.WPF.Presentation.Common //VNC.Core.Common
    {
        //private static string _fileVersion;
        //private static string _productName;
        //private static string _productVersion;
        //private static string _runtimeVersion;

        public const string APPLICATION_NAME = "VNCPhidget22Explorer";
        public new const string LOG_CATEGORY = "VNCPhidget22Explorer";

        public const string cCONFIG_FILE = @"C:\temp\VNCPhidget22Explorer_Config.xml";

        // NOTE(crhodes)
        // Add new VNC.Core.Information InformationXXX
        // for other Assemblies that should provide Info
        // Initialize in App.xaml.cs GetAndSetInformation()
        //
        // Extend Views\AppVersionInfo.xaml as needed
        //
        // Update ShellViewModel
        //  Add new properties
        //  Update InitializeViewModel()

        //public static VNC.Core.Information? InformationApplicationCore;
        public static VNC.Core.Information? InformationVNCPhidget;
        public static VNC.Core.Information? InformationVNCPhidgetConfiguration;
        public static VNC.Core.Information? InformationPhidget22;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        // HACK(crhodes)
        // Decide if want to keep this.
        // Put here to try to get in View and ViewModel that don't have it passed in.
        // Can also ask for in constructor, which is what is mostly done

        public static IContainerProvider Container;

        public static IEventAggregator EventAggregator;
        public static IRegionManager DefaultRegionManager;

        public static Shell CurrentShell;
        public static DevExpress.Xpf.Docking.DockLayoutManager MainDockLayoutManagerControl;

        internal static VNC.Phidget22.PhidgetDeviceLibrary PhidgetDeviceLibrary;
        internal static VNC.Phidget22.Configuration.PerformanceLibrary PerformanceLibrary;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        //public static Int32 PhidgetOpenTimeout { get; set; } = 60000; // ms
        public static Int32? PhidgetOpenTimeout { get; set; } = null; // ms

        public static event EventHandler? AutoHideGroupSpeedChanged;

        private static Int32 _AutoHideGroupSpeed = 250;        

        public static Int32 AutoHideGroupSpeed
        {
            get { return _AutoHideGroupSpeed; }
            set
            {
                _AutoHideGroupSpeed = value;

                EventHandler? evt = AutoHideGroupSpeedChanged;

                if (evt != null)
                {
                    evt(null, EventArgs.Empty); ;
                }
            }
        }
    }
}
