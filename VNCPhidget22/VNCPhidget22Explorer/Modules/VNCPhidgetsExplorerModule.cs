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
            // This is where you pick the style of what gets loaded in the Shell.

            // If you are using the Ribbon Shell and the RibbonRegion

            containerRegistry.RegisterSingleton<IRibbonViewModel, RibbonViewModel>();
            containerRegistry.RegisterSingleton<IRibbon, Ribbon>();

            // If you are using the Shell and the RibbonRegion

            //containerRegistry.RegisterSingleton<IRibbonViewModel, ShellRibbonViewModel>();
            //containerRegistry.RegisterSingleton<IRibbon, ShellRibbon>();

            // Pick one of these for the MainRegion
  
            //containerRegistry.Register<IMain, Main>();
            //containerRegistry.Register<IMain, MainDxLayoutControl>();
            containerRegistry.Register<IMain, MainDxDockLayoutManager>();

            containerRegistry.RegisterSingleton<IStatusBarViewModel, StatusBarViewModel>();
            containerRegistry.RegisterSingleton<StatusBar, StatusBar>();

            containerRegistry.Register<IInterfaceKit, InterfaceKit1018>();
            containerRegistry.Register<IInterfaceKitViewModel, InterfaceKit1018ViewModel>();

            containerRegistry.Register<IAdvancedServo1061, AdvancedServo1061>();
            containerRegistry.Register<IAdvancedServo1061ViewModel, AdvancedServo1061ViewModel>();

            //containerRegistry.Register<IStepper1063, Stepper1063>();
            //containerRegistry.Register<IStepper1063ViewModel, Stepper1063ViewModel>();

            // FIX(crhodes)
            // 
            //containerRegistry.Register<HackAround>();
            //containerRegistry.Register<HackAroundViewModel>();

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

            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region1, typeof(InterfaceKit1018));
            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region2, typeof(InterfaceKit1018));
            _regionManager.RegisterViewWithRegion(RegionNames.InterfaceKit1018Region3, typeof(InterfaceKit1018));

            _regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region1, typeof(AdvancedServo1061));
            //_regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region2, typeof(AdvancedServo1061));
            //_regionManager.RegisterViewWithRegion(RegionNames.AdvancedServo1061Region3, typeof(AdvancedServo1061));

            // FIX(crhodes)
            // 
            //_regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region1, typeof(Stepper1063));
            //_regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region2, typeof(Stepper1063));
            //_regionManager.RegisterViewWithRegion(RegionNames.Stepper1063Region3, typeof(Stepper1063));

            //_regionManager.RegisterViewWithRegion(RegionNames.HackAroundRegion, typeof(HackAround));

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
