using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;

using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

using VNC;
using VNC.Core.Presentation.ViewModels;
using VNC.Core.Presentation.Views;
using VNC.Phidget22.Configuration;

using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer
{
    public partial class App : PrismApplication
    {
        #region Constructors, Initialization, and Load

        public App()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Initialize SignalR", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // If don't delay a bit here, the SignalR logging infrastructure does not initialize quickly enough
            // and the first few log messages are missed.
            // NB.  All are properly recored in the log file.

            Thread.Sleep(250);

            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR(String.Format("Enter"), Common.LOG_CATEGORY, startTicks);
#if DEBUG
            Common.InitializeLogging(debugConfig: true);
            VNC.Phidget22.Common.InitializeLogging(debugConfig: true);
#else
            Common.InitializeLogging();
            VNC.Phidget22.Common.InitializeLogging();
#endif

            // NOTE(crhodes)
            // Application Initialization is done in Startup EventHandler
            // and GetAndSetInformation(), infra.

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR(String.Format("Exit"), Common.LOG_CATEGORY, startTicks);
        }

        // 01

        protected override void ConfigureViewModelLocator()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.ConfigureViewModelLocator();

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 02

        protected override IContainerExtension CreateContainerExtension()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

            return base.CreateContainerExtension();
        }

        // 03 - Create the catalog of Modules

        protected override IModuleCatalog CreateModuleCatalog()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

            return base.CreateModuleCatalog();
        }

        // 04

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);


            base.RegisterRequiredTypes(containerRegistry);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 05

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);


            //containerRegistry.RegisterSingleton<ICustomerDataService, CustomerDataServiceMock>();
            //containerRegistry.RegisterSingleton<IMaterialDataService, MaterialDataServiceMock>();

            // TODO(crhodes)
            // Think this is where we switch to using the Generic Repository.
            // But how to avoid pulling knowledge of EF Context in.  Maybe Service hides details
            // of
            // containerRegistry.RegisterSingleton<IAddressDataService, AddressDataService>();
            // AddressDataService2 has a constructor that takes a CustomPoolAndSpaDbContext.

            //containerRegistry.RegisterSingleton<ICatLookupDataService, CatLookupDataService>();

            // Common Dialogs used by most applications.

            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>("NotificationDialog");
            containerRegistry.RegisterDialog<OkCancelDialog, OkCancelDialogViewModel>("OkCancelDialog");
            containerRegistry.RegisterDialog<ExportGridDialog, ExportGridDialogViewModel>("ExportGridDialog");

            // Add the new UI elements

            // NOTE(crhodes)
            // Most of what would typically appear here is in VNCPhidgetsExploerModule
            //
            // Maybe the Ribbon, Main, StatusBar should be moved back here
            // and the App Specific stuff left in VNCPhidgetsExplorerModule

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //NOTE(crhodes)
        // This has been removed in Prism 8.0

        // 06

        // protected override void ConfigureServiceLocator()
        // {
        // Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

        // base.ConfigureServiceLocator();

        // Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        // }

        // 07 - Configure the catalog of modules
        // Modules are loaded at Startup and must be a project reference

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //NOTE(crhodes)
            // Order matters here.  Application depends on types in Cat
#if VNCTYPES
            moduleCatalog.AddModule(typeof(CatModule));
#endif
            moduleCatalog.AddModule(typeof(VNCPhidget22ExplorerModule));

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 08

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 09

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.ConfigureDefaultRegionBehaviors(regionBehaviors);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 10

        protected override void RegisterFrameworkExceptionTypes()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.RegisterFrameworkExceptionTypes();

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 11

        protected override Window CreateShell()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Figure out how early we can save Container
            // Put it in Common so everyone from everywhere can access

            Common.Container = Container;

            // TODO(crhodes)
            // Pick the shell to start with.

            Shell shell = Container.Resolve<Shell>();
            //Shell shell = Container.Resolve<RibbonShell>();

            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();

            Common.EventAggregator = eventAggregator;

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

            return shell;

            // NOTE(crhodes)
            // The type of view to load into the shell is handled in VNCPhidget22ExplorerModule.cs
        }

        // 12

        protected override void InitializeShell(Window shell)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.InitializeShell(shell);

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 13

        protected override void InitializeModules()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.InitializeModules();

            // NOTE(crhodes)
            // These must complete before UI starts loading to ensure data available for binding.

            InitializePerformanceLibrary();
            InitializePhidgetDeviceLibrary();

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePerformanceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // This loads all the json config files

            PerformanceLibrary performanceLibrary = new PerformanceLibrary(Common.EventAggregator);

            performanceLibrary.LoadConfigFiles();

            Common.PerformanceLibrary = performanceLibrary;

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializePhidgetDeviceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // This will read Hosts.json to know what servers we have
            // This uses a Phidget Manager to determine what Phidgets are attached.

            VNC.Phidget22.PhidgetDeviceLibrary phidgetDeviceLibrary = new VNC.Phidget22.PhidgetDeviceLibrary(Common.EventAggregator);

            phidgetDeviceLibrary.BuildPhidgetDeviceDictionary();

            Common.PhidgetDeviceLibrary = phidgetDeviceLibrary;

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)


        #endregion

        #region Event Handlers

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Int64 startTicks = 0;
            //if (Common.VNCLogging.EventHandler) startTicks = Log.APPLICATION_START("Enter", Common.LOG_CATEGORY);
            if (Common.VNCLogging.ApplicationStart) startTicks = Log.APPLICATION_START("Enter", Common.LOG_CATEGORY);

            try
            {
                GetAndSetInformation();
                VerifyApplicationPrerequisites();
                InitializeApplication();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.InnerException.ToString());
            }

            if (Common.VNCLogging.ApplicationStart) Log.APPLICATION_START("Exit", Common.LOG_CATEGORY, startTicks);
            //if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void GetAndSetInformation()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // Get Information about VNC.Core

            Common.SetVersionInfoVNCCore();

            var appFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            // Get Information about ourselves

            Common.SetVersionInfoApplication(Assembly.GetExecutingAssembly(), appFileVersionInfo);

            // Add Information about the other assemblies in our application

            var vncVNCPhidget22ExplorerCoreAssembly = Assembly.GetAssembly(typeof(VNCPhidget22Explorer.Core.RegionNames));

            if (vncVNCPhidget22ExplorerCoreAssembly is not null)
            {
                var vncVNCPhidget22ExplorerCoreFileVersionInfo = System.Diagnostics.FileVersionInfo
                    .GetVersionInfo(vncVNCPhidget22ExplorerCoreAssembly.Location);

                Common.InformationApplicationCore = Common.GetInformation(
                    vncVNCPhidget22ExplorerCoreAssembly,
                    vncVNCPhidget22ExplorerCoreFileVersionInfo);
            }         

            var vncPhidgetAssembly = Assembly.GetAssembly(typeof(VNC.Phidget22.Common));

            if (vncPhidgetAssembly is not null)
            {
                var vncPhidgetFileVersionInfo = System.Diagnostics.FileVersionInfo
                    .GetVersionInfo(vncPhidgetAssembly.Location);

                Common.InformationVNCPhidget = Common.GetInformation(
                    vncPhidgetAssembly, 
                    vncPhidgetFileVersionInfo);
            }

            var vncPhidgetConfigurationAssembly = Assembly.GetAssembly(typeof(VNC.Phidget22.Configuration.Common));

            if (vncPhidgetConfigurationAssembly is not null)
            {
                var vncPhidgetConfigurationFileVersionInfo = System.Diagnostics.FileVersionInfo
                    .GetVersionInfo(vncPhidgetConfigurationAssembly.Location);

                Common.InformationVNCPhidgetConfiguration = Common.GetInformation(
                    vncPhidgetConfigurationAssembly, 
                    vncPhidgetConfigurationFileVersionInfo);
            }

            // Get Information about Phidget assembly

            var phidget22Assembly = Assembly.GetAssembly(typeof(Phidget22.Phidget));

            if (phidget22Assembly is not null)
            {
                var phidget22FileVersionInfo = System.Diagnostics.FileVersionInfo
                    .GetVersionInfo(phidget22Assembly.Location);

                Common.InformationPhidget22 = Common.GetInformation(
                    phidget22Assembly, 
                    phidget22FileVersionInfo);
            }

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void VerifyApplicationPrerequisites()
        {
            //TODO(crhodes)
            // Add any necessary checks for config files, etc
            // That are required by application
        }

        private void InitializeApplication()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            //TODO(crhodes)
            // Perform any required Initialization.

            // TODO(crhodes)
            // Would be nice to have this loaded from config file
            // Maybe add option to change so can select another folder at runtime.

            Directory.SetCurrentDirectory("./Resources/json");

#if DEBUG
            //Common.DeveloperMode = true;
            Common.DeveloperMode = false;
            //Common.DeveloperUIMode = Visibility.Visible;
            //Common.DeveloperUIMode = Visibility.Hidden;
            Common.DeveloperUIMode = Visibility.Collapsed;
#else
            Common.DeveloperMode = false;
            Common.DeveloperUIMode = Visibility.Collapsed;  // No space reserved
#endif

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);


            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Application_Deactivated(object sender, EventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.APPLICATION_END("Enter", Common.LOG_CATEGORY);


            if (Common.VNCLogging.EventHandler) Log.APPLICATION_END("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            if (Common.VNCLogging.ApplicationEnd) Log.APPLICATION_END("Enter", Common.LOG_CATEGORY, startTicks);

            // TODO(crhodes)
            // Need to fire an event that will be handled by ViewModels
            // Indicating any open devices need to be closed
            // Start by looking at CloseAdvancedServo
            // This is to handle the case where things end with Open devices
            // Maybe look for null in Active{AdvancedServo,InterfaceKit,Stepper}

            if (Common.VNCLogging.ApplicationEnd) Log.APPLICATION_END("Exit", Common.LOG_CATEGORY, startTicks);
            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            long startTicks = Log.APPLICATION_END($"Enter: ReasonSessionEnding:({e.ReasonSessionEnding})", Common.LOG_CATEGORY);


            Log.APPLICATION_END("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void Application_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.ERROR("Unexpected error occurred. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, Common.LOG_CATEGORY);

            MessageBox.Show("Unexpected error occurred. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");

            e.Handled = true;
        }

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion
    }
}
