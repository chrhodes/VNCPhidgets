using System;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Unity;

using VNC;

using VNCPhidget22Explorer.Core;
//using VNCPhidget22Explorer.DomainServices;
using VNCPhidget22Explorer.Presentation.ViewModels;
using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer
{
    public class VNCPhidget22ExplorerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        // 01

        public VNCPhidget22ExplorerModule(IRegionManager regionManager)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            _regionManager = regionManager;

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 02

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ModuleInitialize) startTicks = Log.MODULE_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Maybe the Ribbon, Main, StatusBar should be move back to App.xaml.cs
            // and the App Specific stuff left here

            // TODO(crhodes)
            // This is where you pick the style of what gets loaded in the Shell.
            // The initial shell is chosen in App.xaml.cs CreateShell()

            // If you are using the Shell and the RibbonRegion

            containerRegistry.RegisterSingleton<IRibbonViewModel, RibbonViewModel>();
            // If you are using the simple Ribbon
            containerRegistry.RegisterSingleton<IRibbon, Ribbon>();
            // If you are using the full Ribbon
            containerRegistry.RegisterSingleton<IRibbon, FullRibbon>();

            // If you are using the RibbonShell and the RibbonRegion

            containerRegistry.RegisterSingleton<IRibbonViewModel, FullRibbonViewModel>();


            // Pick one of these for the MainRegion

            //containerRegistry.Register<IMain, Main>();
            //containerRegistry.Register<IMain, MainDxLayoutControl>();
            containerRegistry.Register<IMain, MainDxDockLayoutManager>();

            containerRegistry.RegisterSingleton<IStatusBarViewModel, StatusBarViewModel>();
            containerRegistry.RegisterSingleton<StatusBar, StatusBar>();

            containerRegistry.Register<IAdvancedServo1061, AdvancedServo1061>();
            containerRegistry.Register<IAdvancedServo1061ViewModel, AdvancedServo1061ViewModel>();

            containerRegistry.Register<IInterfaceKit, InterfaceKit1018>();
            containerRegistry.Register<IInterfaceKitViewModel, InterfaceKit1018ViewModel>();

            containerRegistry.Register<IStepper1063, Stepper1063>();
            containerRegistry.Register<IStepper1063ViewModel, Stepper1063ViewModel>();

            containerRegistry.Register<IVINTHub, VINTHub>();
            containerRegistry.Register<IVINTHub, VINTHubHorizontal>();
            containerRegistry.Register<IVINTHubViewModel, VINTHubViewModel>();

            // FIX(crhodes)
            // This works because the parameterless constructor does not exist in HackAround
            containerRegistry.Register<HackAround>();
            containerRegistry.Register<HackAroundViewModel>();

            containerRegistry.Register<ManagePerformanceLibrary>();
            // This calls the view's parameterliess constructor
            //containerRegistry.Register<ManagePerformanceLibraryViewModel>();
            // This calls the view's parameterized constructor
            containerRegistry.Register<IManagePerformanceLibraryViewModel, ManagePerformanceLibraryViewModel>();

            containerRegistry.Register<PhidgetDeviceLibrary>();
            // This calls the view's parameterliess constructor
            //containerRegistry.Register<PhidgetDeviceLibraryViewModel>();
            // This calls the view's parameterized constructor
            containerRegistry.Register<IPhidgetDeviceLibraryViewModel, PhidgetDeviceLibraryViewModel>();

            containerRegistry.Register<PhidgetDeviceLibrary>();
            // This calls the view's parameterliess constructor
            //containerRegistry.Register<EventCoordinatorViewModel>();
            // This calls the view's parameterized constructor
            containerRegistry.Register<IEventCoordinatorViewModel, EventCoordinatorViewModel>();

            // containerRegistry.Register<ICombinedMainViewModel, CombinedMainViewModel>();
            // containerRegistry.RegisterSingleton<ICombinedMain, CombinedMain>();

            // containerRegistry.Register<ICombinedNavigationViewModel, CombinedNavigationViewModel>();
            // containerRegistry.RegisterSingleton<ICombinedNavigation, CombinedNavigation>();

            // containerRegistry.Register<IMouseDetailViewModel, MouseDetailViewModel>();
            // containerRegistry.RegisterSingleton<IMouseDetail, MouseDetail>();            

            // containerRegistry.RegisterSingleton<IMouseDataService, MouseDataService>();
            // containerRegistry.RegisterSingleton<IMouseLookupDataService, MouseLookupDataService>();

            // // This shows the AutoWire ViewModel in action. 

            // containerRegistry.Register<ViewCViewModel>();
            // containerRegistry.Register<ViewC>();
            // containerRegistry.Register<ViewA>();
            // containerRegistry.Register<ViewB>();

            // containerRegistry.Register<IViewABCViewModel, ViewABCViewModel>();
            // containerRegistry.Register<IViewABC, ViewABC>();

            // Figure out how to use one Type

            //containerRegistry.Register<IFriendLookupDataService, LookupDataService>();
            //containerRegistry.Register<IProgrammingLanguageLookupDataService, LookupDataService>();
            //containerRegistry.Register<IMeetingLookupDataService, LookupDataService>();

            if (Common.VNCLogging.ModuleInitialize) Log.MODULE_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // 03

        public void OnInitialized(IContainerProvider containerProvider)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ModuleInitialize) startTicks = Log.MODULE_INITIALIZE("Enter", Common.LOG_CATEGORY);

            _regionManager.RegisterViewWithRegion(RegionNames.RibbonRegion, typeof(IRibbon));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(IMain));
            _regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBar));

            _regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region1, typeof(AdvancedServo1061));
            _regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region2, typeof(AdvancedServo1061));
            _regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region3, typeof(AdvancedServo1061));

            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region1, typeof(InterfaceKit1018));
            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region2, typeof(InterfaceKit1018));
            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region3, typeof(InterfaceKit1018));

            _regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region1, typeof(Stepper1063));
            _regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region2, typeof(Stepper1063));
            _regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region3, typeof(Stepper1063));

            _regionManager.RegisterViewWithRegion(RegionNames.VINTHubRegion1, typeof(VINTHub));
            _regionManager.RegisterViewWithRegion(RegionNames.VINTHubRegion2, typeof(VINTHubHorizontal));
            _regionManager.RegisterViewWithRegion(RegionNames.VINTHubRegion3, typeof(VINTHub));

            _regionManager.RegisterViewWithRegion(RegionNames.HackAroundRegion, typeof(HackAround));
            _regionManager.RegisterViewWithRegion(RegionNames.ManagePerformanceLibraryRegion, typeof(ManagePerformanceLibrary));
            _regionManager.RegisterViewWithRegion(RegionNames.EventCoordinatorRegion, typeof(EventCoordinator));
            _regionManager.RegisterViewWithRegion(RegionNames.PhidgetDeviceLibraryRegion, typeof(PhidgetDeviceLibrary));

            _regionManager.RegisterViewWithRegion(RegionNames.VNCLoggingConfigRegion, typeof(VNC.WPF.Presentation.Dx.Views.VNCLoggingConfig));
            _regionManager.RegisterViewWithRegion(RegionNames.VNCCoreLoggingConfigRegion, typeof(VNC.WPF.Presentation.Dx.Views.VNCCoreLoggingConfig));


            // //This loads CombinedMain into the Shell loaded in App.Xaml.cs

            // _regionManager.RegisterViewWithRegion(RegionNames.CombinedMainRegion, typeof(ICombinedMain));

            // // These load into CombinedMain.xaml

            // _regionManager.RegisterViewWithRegion(RegionNames.CombinedNavigationRegion, typeof(ICombinedNavigation));

            // // This is for Main and AutoWireViewModel demo

            // _regionManager.RegisterViewWithRegion(RegionNames.ViewABCRegion, typeof(IViewABC));
            // _regionManager.RegisterViewWithRegion(RegionNames.ViewARegion, typeof(ViewA));
            // _regionManager.RegisterViewWithRegion(RegionNames.ViewBRegion, typeof(ViewB));
            // _regionManager.RegisterViewWithRegion(RegionNames.ViewCRegion, typeof(ViewC));           

            if (Common.VNCLogging.ModuleInitialize) Log.MODULE_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }
    }
}
