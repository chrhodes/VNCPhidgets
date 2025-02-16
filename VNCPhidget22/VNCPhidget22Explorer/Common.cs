using System;
using System.Reflection;

using Prism.Events;
using Prism.Ioc;

using VNC.Phidget22;

using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer
{
    public class Common : VNC.Core.Common
    {
        //private static string _fileVersion;
        //private static string _productName;
        //private static string _productVersion;
        //private static string _runtimeVersion;

        public const string APPLICATION_NAME = "VNCPhidget22Explorer";
        public new const string LOG_CATEGORY = "VNCPhidget22Explorer";

        // NOTE(crhodes)
        // Add new VNC.Core.Information InformationXXX
        // for other Assemblies that should provide Info
        // Initialize in App.xaml.cs ApplicationInitialize
        // Extend Views\AppVersionInfo.xaml as needed

        public static VNC.Core.Information? InformationVNCPhidget;

        public const string cCONFIG_FILE = @"C:\temp\VNCPhidget22Explorer_Config.xml";

        public static IContainerProvider Container;
        public static IEventAggregator EventAggregator;

        public static Shell? CurrentShell;
        public static RibbonShell? CurrentRibbonShell;

        //public static int PhidgetOpenTimeout { get; set; } = 60000; // ms
        public static int? PhidgetOpenTimeout { get; set; } = null; // ms

        public static VNC.Phidget22.PhidgetDeviceLibrary PhidgetDeviceLibrary;

        // These values are added to the dimensions of a hosting window if the
        // hosted User_Control specifies values for MinWidth/MinHeight.
        // They have not been thought through but do seem to "work".

        internal const int WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD = 30;
        internal const int WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD = 75;


        public static event EventHandler AutoHideGroupSpeedChanged;

        private static int _AutoHideGroupSpeed = 250;

        public static int AutoHideGroupSpeed
        {
            get { return _AutoHideGroupSpeed; }
            set
            {
                _AutoHideGroupSpeed = value;

                EventHandler evt = AutoHideGroupSpeedChanged;

                if (evt != null)
                {
                    evt(null, EventArgs.Empty); ;
                }
            }
        }

    }
}
