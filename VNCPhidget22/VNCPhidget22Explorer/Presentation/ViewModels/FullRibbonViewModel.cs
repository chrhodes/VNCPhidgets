using System;
using System.Windows.Controls;

using DevExpress.Xpf.Docking;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Core.Presentation;
using VNC.WPF.Presentation.Views;

using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class FullRibbonViewModel : EventViewModelBase, IRibbonViewModel//, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public FullRibbonViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

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

            AboutCommand = new DelegateCommand(About, AboutCanExecute);
            AddPhidgetDeviceViewCommand = new DelegateCommand<string>(AddPhidgetDeviceView, AddPhidgetDeviceViewCanExecute);

            // If using CommandParameter, figure out TYPE here and below
            // and remove above declaration
            //About = new DelegateCommand<TYPE>(About, AboutCanExecute);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)



        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands

        #region About Command

        public static WindowHost _aboutHost = null;

        public DelegateCommand AboutCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> About { get; set; }

        // If using CommandParameter, figure out TYPE here
        //public TYPE AboutParameter;
        public string AboutContent { get; set; } = "About";
        public string AboutToolTip { get; set; } = "Display About Information";

        // Can get fancy and use Resources
        //public string AboutContent { get; set; } = "ViewName_AboutContent";
        //public string AboutToolTip { get; set; } = "ViewName_AboutContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_AboutContent">About</system:String>
        //    <system:String x:Key="ViewName_AboutContentToolTip">About ToolTip</system:String>

        // If using CommandParameter, figure out TYPE here
        //public void About(TYPE value)
        public void About()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called About";
            PublishStatusMessage(Message);

            if (_aboutHost is null) _aboutHost = new WindowHost(EventAggregator);

            // NOTE(crhodes)
            // About has About() and About(ViewModel) constructors.
            // If No DI Registrations - About() is called - does not wire View to ViewModel
            // If About DI Registrations - About() is called - does not wire View to ViewModel
            // If AboutViewModel DI Registrations - About(viewModel) is called - does wire View to ViewModel
            // NB.  AutoWireViewModel=false

            // NB. If AutoWireViewModel=true, the About() is called but then magically it is wired to ViewModel!

            UserControl userControl = (Views.About)Common.Container.Resolve(typeof(Views.About));

            _aboutHost.DisplayUserControlInHost(
                "VNCPhidgets22Explorer About",
                    Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                //(Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
                //(Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
                ShowWindowMode.Modeless_Show,
                userControl
            );

            // TODO(crhodes)
            // See what we can learn about rendered/actual size of control so we can resize window once loaded.

            //var userControl = new About();

            // NOTE(crhodes)
            // Wire things up ourselves - ViewModel First - with a little help from DI.

            //DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VAVM1st_Host,
            //    "MVVM ViewAViewModel First (ViewModel is passed new ViewA) - By Hand",
            //    Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //    ShowWindowMode.Modeless_Show,
            //    new ViewAViewModel(
            //        new ViewA(),
            //        (IEventAggregator)Common.Container.Resolve(typeof(EventAggregator)),
            //        (DialogService)Common.Container.Resolve(typeof(DialogService))
            //    )
            //);

            //_aboutHost.DisplayUserControlInHost(
            //    "VNC Logging Configuration",
            //    Common.DEFAULT_WINDOW_WIDTH,
            //    Common.DEFAULT_WINDOW_HEIGHT,
            //    (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //    (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //    ShowWindowMode.Modeless_Show,
            //    userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<AboutEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<AboutEvent>().Publish(
            //      new AboutEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class AboutEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<AboutEvent>().Subscribe(About);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean AboutCanExecute(TYPE value)
        public Boolean AboutCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region AddPhidgetDeviceView Command

        //public static WindowHost _aboutHost = null;

        public DelegateCommand<string> AddPhidgetDeviceViewCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> AddPhidgetDeviceView { get; set; }

        // If using CommandParameter, figure out TYPE here
        //public TYPE AddPhidgetDeviceViewParameter;
        public string AddPhidgetDeviceViewContent { get; set; } = "AddPhidgetDeviceView";
        public string AddPhidgetDeviceViewToolTip { get; set; } = "Display AddPhidgetDeviceView Information";

        // Can get fancy and use Resources
        //public string AddPhidgetDeviceViewContent { get; set; } = "ViewName_AddPhidgetDeviceViewContent";
        //public string AddPhidgetDeviceViewToolTip { get; set; } = "ViewName_AddPhidgetDeviceViewContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_AddPhidgetDeviceViewContent">AddPhidgetDeviceView</system:String>
        //    <system:String x:Key="ViewName_AddPhidgetDeviceViewContentToolTip">AddPhidgetDeviceView ToolTip</system:String>

        // If using CommandParameter, figure out TYPE here
        //public void AddPhidgetDeviceView(TYPE value)
        public void AddPhidgetDeviceView(string phidgetDeviceView)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = $"Cool, you called AddPhidgetDeviceView {phidgetDeviceView}";
            PublishStatusMessage(Message);

            LayoutPanel? layoutPanel;

            switch (phidgetDeviceView)
            {
                // NOTE(crhodes)
                // These allow multiple

                case "AdvancedServo1061":
                    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                        new AdvancedServo1061_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    break;

                case "InterfaceKit1018":
                    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                        new InterfaceKit1018_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });   
                    break;

                case "Stepper1063":
                    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                        new Stepper1063_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });    
                    break;

                case "VINTHub":
                    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                            new VINTHub_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    break;

                case "VINTHubHorizontal":
                    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                            new VINTHubHorizontal_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    break;

                // NOTE(crhodes)
                // These allow only one

                case "HackAround":
                    layoutPanel = Common.MainDockLayoutManagerControl.GetItem(phidgetDeviceView) as LayoutPanel;

                    if (layoutPanel is null)
                    {
                        Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                            new HackAround_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    }
                    else
                    {
                        Common.MainDockLayoutManagerControl.Activate(layoutPanel);
                    }
                    break;

                case "ManagePerformanceLibrary":
                    layoutPanel = Common.MainDockLayoutManagerControl.GetItem(phidgetDeviceView) as LayoutPanel;

                    if (layoutPanel is null)
                    {
                        Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                            new ManagePerformanceLibrary_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    }
                    else
                    {
                        Common.MainDockLayoutManagerControl.Activate(layoutPanel);
                    }
                    break;

                case "PhidgetDeviceLibrary":
                    layoutPanel = Common.MainDockLayoutManagerControl.GetItem(phidgetDeviceView) as LayoutPanel;

                    if (layoutPanel is null)
                    {
                        Common.MainDockLayoutManagerControl.LayoutRoot.Add(
                            new PhidgetDeviceLibrary_LayoutPanel() { Name = phidgetDeviceView, Caption = phidgetDeviceView });
                    }
                            else
                    {
                        Common.MainDockLayoutManagerControl.Activate(layoutPanel);
                    }
                    break;

                default:
                    Log.ERROR($"Unsupported phidgetDeviceView:>{phidgetDeviceView}<", Common.LOG_CATEGORY);
                    break;
            }

            //LayoutPanel lp = Common.MainDockLayoutManagerControl.GetItem("AdvancedServo1061") as LayoutPanel;

            //if (lp is null)
            //{
            //    Common.MainDockLayoutManagerControl.LayoutRoot.Add(
            //        new AdvancedServo1061_1() { Name = "AdvancedServo1061", Caption="AdvancedServo1061" } );
            //}
            //else
            //{
            //    Common.MainDockLayoutManagerControl.Activate(lp);
            //}

            //if (_aboutHost is null) _aboutHost = new WindowHost(EventAggregator);

            // NOTE(crhodes)
            // AddPhidgetDeviceView has AddPhidgetDeviceView() and AddPhidgetDeviceView(ViewModel) constructors.
            // If No DI Registrations - AddPhidgetDeviceView() is called - does not wire View to ViewModel
            // If AddPhidgetDeviceView DI Registrations - AddPhidgetDeviceView() is called - does not wire View to ViewModel
            // If AddPhidgetDeviceViewViewModel DI Registrations - AddPhidgetDeviceView(viewModel) is called - does wire View to ViewModel
            // NB.  AutoWireViewModel=false

            // NB. If AutoWireViewModel=true, the AddPhidgetDeviceView() is called but then magically it is wired to ViewModel!

            //UserControl userControl = (Views.AddPhidgetDeviceView)Common.Container.Resolve(typeof(Views.AddPhidgetDeviceView));

            //_aboutHost.DisplayUserControlInHost(
            //    "VNCPhidgets22Explorer AddPhidgetDeviceView",
            //        Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //    //(Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //    //(Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //    ShowWindowMode.Modeless_Show,
            //    userControl
            //);

            // TODO(crhodes)
            // See what we can learn about rendered/actual size of control so we can resize window once loaded.

            //var userControl = new AddPhidgetDeviceView();

            // NOTE(crhodes)
            // Wire things up ourselves - ViewModel First - with a little help from DI.

            //DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VAVM1st_Host,
            //    "MVVM ViewAViewModel First (ViewModel is passed new ViewA) - By Hand",
            //    Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //    ShowWindowMode.Modeless_Show,
            //    new ViewAViewModel(
            //        new ViewA(),
            //        (IEventAggregator)Common.Container.Resolve(typeof(EventAggregator)),
            //        (DialogService)Common.Container.Resolve(typeof(DialogService))
            //    )
            //);

            //_aboutHost.DisplayUserControlInHost(
            //    "VNC Logging Configuration",
            //    Common.DEFAULT_WINDOW_WIDTH,
            //    Common.DEFAULT_WINDOW_HEIGHT,
            //    (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //    (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //    ShowWindowMode.Modeless_Show,
            //    userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<AddPhidgetDeviceViewEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<AddPhidgetDeviceViewEvent>().Publish(
            //      new AddPhidgetDeviceViewEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class AddPhidgetDeviceViewEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<AddPhidgetDeviceViewEvent>().Subscribe(AddPhidgetDeviceView);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean AddPhidgetDeviceViewCanExecute(TYPE value)
        public Boolean AddPhidgetDeviceViewCanExecute(string value)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        //#region IInstanceCount

        //private static Int32 _instanceCountVM;

        //public Int32 InstanceCountVM
        //{
        //    get => _instanceCountVM;
        //    set => _instanceCountVM = value;
        //}

        //#endregion
    }
}
