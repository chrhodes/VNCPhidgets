using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;
using VNCPhidgetConfig = VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class InterfaceKit1018ViewModel 
        : EventViewModelBase, IInterfaceKitViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public InterfaceKit1018ViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            OpenInterfaceKitCommand = new DelegateCommand(OpenInterfaceKit, OpenInterfaceKitCanExecute);
            CloseInterfaceKitCommand = new DelegateCommand(CloseInterfaceKit, CloseInterfaceKitCanExecute);

            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNC.Phidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            ConfigureDigitalOutputs(8);

            Message = "InterfaceKitViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //private void InterfaceKit1018ViewModel_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig ? hostConfig = 
                JsonSerializer.Deserialize< VNCPhidgetConfig.HostConfig >
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();
            
            //Sensors2 = hostConfig.Sensors.ToList();

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return jsonOptions;
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #region Host

        private string _hostConfigFileName;

        public string HostConfigFileName
        {
            get => _hostConfigFileName;
            set
            {
                if (_hostConfigFileName == value) return;
                _hostConfigFileName = value;
                OnPropertyChanged();
            }
        }

        public string HostConfigFileNameToolTip { get; set; } = "DoubleClick to select new file";

        private IEnumerable<VNCPhidgetConfig.Host> _Hosts;
        public IEnumerable<VNCPhidgetConfig.Host> Hosts
        {
            get => _Hosts;
            set
            {
                _Hosts = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Host _selectedHost;
        public VNCPhidgetConfig.Host SelectedHost
        {
            get => _selectedHost;
            set
            {
                if (_selectedHost == value)
                    return;
                _selectedHost = value;
                InterfaceKits = _selectedHost.InterfaceKits.ToList<VNCPhidgetConfig.InterfaceKit>();
                OnPropertyChanged();
            }
        }

        //private IEnumerable<VNCPhidgetConfig.Sensor> _Sensors2;
        //public IEnumerable<VNCPhidgetConfig.Sensor> Sensors2
        //{
        //    get
        //    {
        //        if (null == _Sensors2)
        //        {
        //            // TODO(crhodes)
        //            // Load this like the sensors.xml for now

        //            //_Sensors =
        //            //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Sensors").Elements("Sensor")
        //            //    select new Sensor(
        //            //        item.Attribute("Name").Value,
        //            //        item.Attribute("IPAddress").Value,
        //            //        item.Attribute("Port").Value,
        //            //        bool.Parse(item.Attribute("Enable").Value)
        //            //        );
        //        }

        //        return _Sensors2;
        //    }

        //    set
        //    {
        //        _Sensors2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Phidget

        private Phidgets.Phidget _phidget22Device;
        public Phidgets.Phidget Phidget22Device
        {
            get => _phidget22Device;
            set
            {
                if (_phidget22Device == value)
                    return;
                _phidget22Device = value;
                OnPropertyChanged();
            }
        }

        private bool _logPhidgetEvents = false;
        public bool LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();

                if (ActiveInterfaceKit is not null) ActiveInterfaceKit.LogPhidgetEvents = value;
            }
        }

        private bool _logSequenceAction = false;
        public bool LogSequenceAction
        {
            get => _logSequenceAction;
            set
            {
                if (_logSequenceAction == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();

                if (ActiveInterfaceKit is not null)
                {
                    ActiveInterfaceKit.LogSequenceAction = value;
                }
            }
        }

        // TODO(crhodes)
        // Since channels are now the focus, do we need this?

        private bool? _deviceAttached;
        public bool? DeviceAttached
        {
            get => _deviceAttached;
            set
            {
                if (_deviceAttached == value)
                    return;
                _deviceAttached = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region InterfaceKit

        public DigitalInputEx[] DigitalInputs { get; set; }

        DigitalOutputEx[] _digitalOutputs = new DigitalOutputEx[8];
        public DigitalOutputEx[] DigitalOutputs 
        {
            get
            {
                return _digitalOutputs;
            }
            set
            {
                _digitalOutputs = value;
                OnPropertyChanged();
            }
        }

        //public VoltageInputEx[] VoltageInputs;
        //public VoltageRatioInputEx[] VoltageRatioInputs;

        //public VoltageOutputEx[] VoltageOutputs;

        #region InterfaceKit Events

        private bool _logInputChangeEvents = false;
        public bool LogInputChangeEvents
        {
            get => _logInputChangeEvents;
            set
            {
                if (_logInputChangeEvents == value)
                    return;
                _logInputChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogSensorChangeEvents = _logInputChangeEvents;
                //}
            }
        }

        private bool _logOutputChangeEvents = false;
        public bool LogOutputChangeEvents
        {
            get => _logOutputChangeEvents;
            set
            {
                if (_logOutputChangeEvents == value)
                    return;
                _logOutputChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogOutputChangeEvents = _logOutputChangeEvents;
                //}
            }
        }

        private bool _logSensorChangeEvents = false;
        public bool LogSensorChangeEvents
        {
            get => _logSensorChangeEvents;
            set
            {
                if (_logSensorChangeEvents == value)
                    return;
                _logSensorChangeEvents = value;
                OnPropertyChanged();

                //if (ActiveInterfaceKit is not null)
                //{
                //    ActiveInterfaceKit.LogSensorChangeEvents = _logSensorChangeEvents;
                //}
            }
        }

        #endregion

        private IEnumerable<VNCPhidgetConfig.InterfaceKit> _InterfaceKits;
        public IEnumerable<VNCPhidgetConfig.InterfaceKit> InterfaceKits
        {
            get
            {
                if (null == _InterfaceKits)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_InterfaceKits =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("InterfaceKits").Elements("InterfaceKit")
                    //    select new InterfaceKit(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _InterfaceKits;
            }

            set
            {
                _InterfaceKits = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.InterfaceKit _selectedInterfaceKit;
        public VNCPhidgetConfig.InterfaceKit SelectedInterfaceKit
        {
            get => _selectedInterfaceKit;
            set
            {
                if (_selectedInterfaceKit == value)
                    return;
                _selectedInterfaceKit = value;

                OpenInterfaceKitCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        // TODO(crhodes)
        // May want to make this just a Phidget22.InterfaceKit to avoid all the ActiveInterfaceKit.InterfaceKit stuff

        private DigitalOutputEx _activeInterfaceKit;
        public DigitalOutputEx ActiveInterfaceKit
        {
            get => _activeInterfaceKit;
            set
            {
                if (_activeInterfaceKit == value)
                    return;
                _activeInterfaceKit = value;

                //if (_activeInterfaceKit is not null)
                //{
                //    // FIX(crhodes)
                //    // 
                //    //PhidgetDevice = _activeInterfaceKit.InterfaceKit;
                //    // Not sure we need to do this.  See code in Open.
                //    Phidget22Device = _activeInterfaceKit.PhysicalPhidget;
                //}
                //else
                //{
                //    // TODO(crhodes)
                //    // PhidgetDevice = null ???
                //    // Will need to declare Phidget22.Phidget?
                //    Phidget22Device = null;
                //}

                OnPropertyChanged();
            }
        }

        #region InterfaceKit Phidget Properties

        //#region Digital Inputs

        //#region DI0

        //private bool? _dI0;
        //public bool? DI0
        //{
        //    get => _dI0;
        //    set
        //    {
        //        if (_dI0 == value)
        //            return;
        //        _dI0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI0_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI0_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI0 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI0_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI0_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI0_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI0_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI0_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI0 = e.State;
        //}

        //private void DI0_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI0_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI1

        //private bool? _dI1;
        //public bool? DI1
        //{
        //    get => _dI1;
        //    set
        //    {
        //        if (_dI1 == value)
        //            return;
        //        _dI1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI1_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI1_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI1 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI1_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI1_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI1_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI1_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI1_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI1 = e.State;
        //}

        //private void DI1_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI1_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI2

        //private bool? _dI2;
        //public bool? DI2
        //{
        //    get => _dI2;
        //    set
        //    {
        //        if (_dI2 == value)
        //            return;
        //        _dI2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI2_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI2_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI2 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI2_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI2_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI2_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI2_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI2_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI2 = e.State;
        //}

        //private void DI2_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI2_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI3

        //private bool? _dI3;
        //public bool? DI3
        //{
        //    get => _dI3;
        //    set
        //    {
        //        if (_dI3 == value)
        //            return;
        //        _dI3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI3_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI3_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI3 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI3_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI3_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI3_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI3_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI3_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI3 = e.State;
        //}

        //private void DI3_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI3_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI4

        //private bool? _dI4;
        //public bool? DI4
        //{
        //    get => _dI4;
        //    set
        //    {
        //        if (_dI4 == value)
        //            return;
        //        _dI4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI4_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI4_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI4 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI4_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI4_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI4_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI4_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI4_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI4 = e.State;
        //}

        //private void DI4_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI4_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI5

        //private bool? _dI5;
        //public bool? DI5
        //{
        //    get => _dI5;
        //    set
        //    {
        //        if (_dI5 == value)
        //            return;
        //        _dI5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI5_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI5_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI5 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI5_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI5_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI5_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI5_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI5_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI5 = e.State;
        //}

        //private void DI5_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI5_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI6

        //private bool? _dI6;
        //public bool? DI6
        //{
        //    get => _dI6;
        //    set
        //    {
        //        if (_dI6 == value)
        //            return;
        //        _dI6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI6_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI6_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI6 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI6_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI6_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI6_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI6_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI6_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI6 = e.State;
        //}

        //private void DI6_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI6_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI7

        //private bool? _dI7;
        //public bool? DI7
        //{
        //    get => _dI7;
        //    set
        //    {
        //        if (_dI7 == value)
        //            return;
        //        _dI7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI7_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI7_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI7 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI7_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI7_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI7_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI7_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI7_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI7 = e.State;
        //}

        //private void DI7_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI7_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI8

        //private bool? _dI8;
        //public bool? DI8
        //{
        //    get => _dI8;
        //    set
        //    {
        //        if (_dI8 == value)
        //            return;
        //        _dI0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI8_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI8_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI8 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI8_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI8_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI8_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI8_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI8_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI8 = e.State;
        //}

        //private void DI8_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI8_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI9

        //private bool? _dI9;
        //public bool? DI9
        //{
        //    get => _dI9;
        //    set
        //    {
        //        if (_dI9 == value)
        //            return;
        //        _dI9 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI9_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI9_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI9 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI9_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI9_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI9_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI9_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI9_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI9 = e.State;
        //}

        //private void DI9_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI9_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI10

        //private bool? _dI10;
        //public bool? DI10
        //{
        //    get => _dI10;
        //    set
        //    {
        //        if (_dI10 == value)
        //            return;
        //        _dI10 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI10_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI10_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI10 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI10_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI10_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI10_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI10_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI10_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI10 = e.State;
        //}

        //private void DI10_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI10_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI11

        //private bool? _dI11;
        //public bool? DI11
        //{
        //    get => _dI11;
        //    set
        //    {
        //        if (_dI11 == value)
        //            return;
        //        _dI11 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI11_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI11_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI11 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI11_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI11_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI11_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI11_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI11_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI11 = e.State;
        //}

        //private void DI11_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI11_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI12

        //private bool? _dI12;
        //public bool? DI12
        //{
        //    get => _dI12;
        //    set
        //    {
        //        if (_dI12 == value)
        //            return;
        //        _dI12 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI12_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI12_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI12 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI12_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI12_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI12_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI12_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI12_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI12 = e.State;
        //}

        //private void DI12_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI12_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI13

        //private bool? _dI13;
        //public bool? DI13
        //{
        //    get => _dI13;
        //    set
        //    {
        //        if (_dI13 == value)
        //            return;
        //        _dI13 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI13_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI13_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI13 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI13_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI13_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI13_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI13_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI13_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI13 = e.State;
        //}

        //private void DI13_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI13_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI14

        //private bool? _dI14;
        //public bool? DI14
        //{
        //    get => _dI14;
        //    set
        //    {
        //        if (_dI14 == value)
        //            return;
        //        _dI14 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI14_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI14_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI14 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI14_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI14_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI14_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI14_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI14_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI14 = e.State;
        //}

        //private void DI14_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI14_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DI15

        //private bool? _dI15;
        //public bool? DI15
        //{
        //    get => _dI15;
        //    set
        //    {
        //        if (_dI15 == value)
        //            return;
        //        _dI15 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DI15_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI15_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DI15 = ((Phidgets.DigitalInput)sender).State;
        //}

        //private void DI15_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI15_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI15_Detach(object sender, PhidgetsEvents.DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI15_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DI15_StateChange(object sender, PhidgetsEvents.DigitalInputStateChangeEventArgs e)
        //{
        //    DI15 = e.State;
        //}

        //private void DI15_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DI15_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#endregion

        //#region Digital Outputs

        //#region DO0

        //private bool? _dO0;
        //public bool? DO0
        //{
        //    get => _dO0;
        //    set
        //    {
        //        if (_dO0 == value)
        //            return;
        //        _dO0 = value;

        //        // ActiveInterfaceKit_OutputChange may have called us
        //        // No need to update if same state.

        //        // FIX(crhodes)
        //        // 
        //        //if (ActiveInterfaceKit is not null
        //        //    && value != ActiveInterfaceKit.InterfaceKit.outputs[0])
        //        //{
        //        //    ActiveInterfaceKit.InterfaceKit.outputs[0] = (Boolean)value;
        //        //}

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[0].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[0].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO0_Attach(object sender, PhidgetsEvents.AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO0_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO0 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO0_PropertyChange(object sender, PhidgetsEvents.PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO0_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO0_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO0_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO0_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO0_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO1

        //private bool? _dO1;
        //public bool? DO1
        //{
        //    get => _dO1;
        //    set
        //    {
        //        if (_dO1 == value)
        //            return;
        //        _dO1 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[1].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[1].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO1_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO1_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO1 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO1_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO1_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO1_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO1_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO1_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO1_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO2

        //private bool? _dO2;
        //public bool? DO2
        //{
        //    get => _dO2;
        //    set
        //    {
        //        if (_dO2 == value)
        //            return;
        //        _dO2 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[2].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[2].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO2_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO2_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO2 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO2_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO2_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO2_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO2_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO2_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO2_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO3

        //private bool? _dO3;
        //public bool? DO3
        //{
        //    get => _dO3;
        //    set
        //    {
        //        if (_dO3 == value)
        //            return;
        //        _dO3 = value;
        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[3].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[3].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO3_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO3_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO3 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO3_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO3_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO3_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO3_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO3_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO3_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO4

        //private bool? _dO4;
        //public bool? DO4
        //{
        //    get => _dO4;
        //    set
        //    {
        //        if (_dO4 == value)
        //            return;
        //        _dO4 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[4].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[4].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO4_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO4_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO4 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO4_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO4_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO4_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO4_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO4_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO4_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO5

        //private bool? _dO5;
        //public bool? DO5
        //{
        //    get => _dO5;
        //    set
        //    {
        //        if (_dO5 == value)
        //            return;
        //        _dO5 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[5].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[5].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO5_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO5_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO5 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO5_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO5_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO5_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO5_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO5_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO5_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO6

        //private bool? _dO6;
        //public bool? DO6
        //{
        //    get => _dO6;
        //    set
        //    {
        //        if (_dO6 == value)
        //            return;
        //        _dO6 = value;
        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[6].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[6].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO6_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO6_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO6 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO6_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO6_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO6_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO6_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO6_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO6_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO7

        //private bool? _dO7;
        //public bool? DO7
        //{
        //    get => _dO7;
        //    set
        //    {
        //        if (_dO7 == value)
        //            return;
        //        _dO7 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[7].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[7].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO7_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO7_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO7 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO7_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO7_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO7_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO7_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO7_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO7_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO8

        //private bool? _dO8;
        //public bool? DO8
        //{
        //    get => _dO8;
        //    set
        //    {
        //        if (_dO8 == value)
        //            return;
        //        _dO8 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[8].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[8].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO8_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO8_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO8 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO8_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO8_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO8_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO8_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO8_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO8_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO9

        //private bool? _dO9;
        //public bool? DO9
        //{
        //    get => _dO9;
        //    set
        //    {
        //        if (_dO9 == value)
        //            return;
        //        _dO9 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[9].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[9].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO9_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO9_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO9 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO9_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO9_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO9_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO9_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO9_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO9_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO10

        //private bool? _dO10;
        //public bool? DO10
        //{
        //    get => _dO10;
        //    set
        //    {
        //        if (_dO10 == value)
        //            return;
        //        _dO10 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[10].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[10].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO10_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO10_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO10 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO10_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO10_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO10_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO10_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO10_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO10_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO11

        //private bool? _dO11;
        //public bool? DO11
        //{
        //    get => _dO11;
        //    set
        //    {
        //        if (_dO11 == value)
        //            return;
        //        _dO11 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[11].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[11].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO11_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO11_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO11 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO11_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO11_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO11_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO11_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO11_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO11_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO12

        //private bool? _dO12;
        //public bool? DO12
        //{
        //    get => _dO12;
        //    set
        //    {
        //        if (_dO12 == value)
        //            return;
        //        _dO12 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[12].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[12].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO12_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO12_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO12 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO12_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO12_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO12_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO12_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO12_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO12_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO13

        //private bool? _dO13;
        //public bool? DO13
        //{
        //    get => _dO13;
        //    set
        //    {
        //        if (_dO13 == value)
        //            return;
        //        _dO13 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[13].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[13].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO13_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO13_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO13 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO13_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO13_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO13_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO13_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO13_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO13_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO14

        //private bool? _dO14;
        //public bool? DO14
        //{
        //    get => _dO14;
        //    set
        //    {
        //        if (_dO14 == value)
        //            return;
        //        _dO14 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[14].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[14].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO14_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO14_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO14 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO14_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO14_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO14_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO14_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO14_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO14_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#region DO15

        //private bool? _dO15;
        //public bool? DO15
        //{
        //    get => _dO15;
        //    set
        //    {
        //        if (_dO15 == value)
        //            return;
        //        _dO15 = value;

        //        if (ActiveInterfaceKit is not null
        //            && value != ActiveInterfaceKit.DigitalOutputs[15].State)
        //        {
        //            ActiveInterfaceKit.DigitalOutputs[15].State = (Boolean)value;
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private void DO15_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO15_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    DO15 = ((Phidgets.DigitalOutput)sender).State;
        //}

        //private void DO15_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO15_PropertyChange: sender:{sender} {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO15_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO15_Detach: sender:{sender}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void DO15_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"DO15_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //#endregion

        //#endregion

        //#region Sensor Input

        //#region Analog Sensors

        //#region Sensor A0

        //private Phidgets.VoltageSensorType _aISensorType0;
        //public Phidgets.VoltageSensorType AISensorType0
        //{
        //    get => _aISensorType0;
        //    set
        //    {
        //        if (_aISensorType0 == value)
        //            return;
        //        _aISensorType0 = value;
        //        OnPropertyChanged();

        //        ActiveInterfaceKit.VoltageInputs[0].SensorType = value;
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType0;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType0
        //{
        //    get => _aIRatioSensorType0;
        //    set
        //    {
        //        if (_aIRatioSensorType0 == value)
        //            return;
        //        _aIRatioSensorType0 = value;
        //        OnPropertyChanged();

        //        ActiveInterfaceKit.VoltageRatioInputs[0].SensorType = value;
        //    }
        //}

        //private Double? _aIRatioMode0;
        //public Double? AIRatioMode0
        //{
        //    get => _aIRatioMode0;
        //    set
        //    {
        //        if (_aIRatioMode0 == value)
        //            return;
        //        _aIRatioMode0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage0;
        //public Double? AIVoltage0
        //{
        //    get => _aIVoltage0;
        //    set
        //    {
        //        if (_aIVoltage0 == value)
        //            return;
        //        _aIVoltage0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit0;
        //public Phidgets.Unit AIUnit0
        //{
        //    get => _aIUnit0;
        //    set
        //    {
        //        if (_aIUnit0 == value)
        //            return;
        //        _aIUnit0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI0_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit0 = Phidgets.Unit.Volt;
        //            AIDataRateMin0 = sensor.MinDataRate;
        //            AIDataRate0 = sensor.DataRate;
        //            AIDataRateMax0 = sensor.MaxDataRate;
        //            AIDataIntervalMin0 = sensor.MinDataInterval;
        //            AIDataInterval0 = sensor.DataInterval;
        //            AIDataIntervalMax0 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit0 = Phidgets.Unit.Volt;
        //            AIDataRateMin0 = ratioSensor.MinDataRate;
        //            AIDataRate0 = ratioSensor.DataRate;
        //            AIDataRateMax0 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin0 = ratioSensor.MinDataInterval;
        //            AIDataInterval0 = ratioSensor.DataInterval;
        //            AIDataIntervalMax0 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI0_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI0_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI0_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI0_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage0 = e.Voltage;
        //}

        //void AI0_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
    
        //    AIVoltage0 = e.VoltageRatio;
        //}

        //private void AI0_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI0_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_Error: sender:{sender} code:{e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    if (e.Code == Phidgets.ErrorEventCode.OutOfRange)
        //    {
        //        switch (sender.GetType().Name)
        //        {
        //            case nameof(Phidgets.VoltageInput):
        //                var sensor = (Phidgets.VoltageInput)sender;
        //                AIVoltage0 = sensor.Voltage;

        //                break;

        //            case nameof(Phidgets.VoltageRatioInput):
        //                var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //                AIVoltage0 = ratioSensor.VoltageRatio;
        //                break;
        //        }
        //    }
        //}

        //private Int32? _aIRaw0;
        //public Int32? AIRaw0
        //{
        //    get => _aIRaw0;
        //    set
        //    {
        //        if (_aIRaw0 == value)
        //            return;
        //        _aIRaw0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate0;
        //public Double AIDataRate0
        //{
        //    get => _aIDataRate0;
        //    set
        //    {
        //        if (_aIDataRate0 == value)
        //            return;
        //        _aIDataRate0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax0;
        //public Double AIDataRateMax0
        //{
        //    get => _aIDataRateMax0;
        //    set
        //    {
        //        if (_aIDataRateMax0 == value)
        //            return;
        //        _aIDataRateMax0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin0;
        //public Double AIDataRateMin0
        //{
        //    get => _aIDataRateMin0;
        //    set
        //    {
        //        if (_aIDataRateMin0 == value)
        //            return;
        //        _aIDataRateMin0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin0;
        //public Double AIDataIntervalMin0
        //{
        //    get => _aIDataIntervalMin0;
        //    set
        //    {
        //        if (_aIDataIntervalMin0 == value)
        //            return;
        //        _aIDataIntervalMin0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval0;
        //public Int32 AIDataInterval0
        //{
        //    get => _aIDataInterval0;
        //    set
        //    {
        //        if (_aIDataInterval0 == value)
        //            return;
        //        _aIDataInterval0 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[0].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[0].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax0;
        //public Double AIDataIntervalMax0
        //{
        //    get => _aIDataIntervalMax0;
        //    set
        //    {
        //        if (_aIDataIntervalMax0 == value)
        //            return;
        //        _aIDataIntervalMax0 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A1

        //private Phidgets.VoltageSensorType _aISensorType1;
        //public Phidgets.VoltageSensorType AISensorType1
        //{
        //    get => _aISensorType1;
        //    set
        //    {
        //        if (_aISensorType1 == value)
        //            return;
        //        _aISensorType1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType1;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType1
        //{
        //    get => _aIRatioSensorType1;
        //    set
        //    {
        //        if (_aIRatioSensorType1 == value)
        //            return;
        //        _aIRatioSensorType1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage1;
        //public Double? AIVoltage1
        //{
        //    get => _aIVoltage1;
        //    set
        //    {
        //        if (_aIVoltage1 == value)
        //            return;
        //        _aIVoltage1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit1;
        //public Phidgets.Unit AIUnit1
        //{
        //    get => _aIUnit1;
        //    set
        //    {
        //        if (_aIUnit1 == value)
        //            return;
        //        _aIUnit1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI1_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit1 = Phidgets.Unit.Volt;
        //            AIDataRateMin1 = sensor.MinDataRate;
        //            AIDataRate1 = sensor.DataRate;
        //            AIDataRateMax1 = sensor.MaxDataRate;
        //            AIDataIntervalMin1 = sensor.MinDataInterval;
        //            AIDataInterval1 = sensor.DataInterval;
        //            AIDataIntervalMax1 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit1 = Phidgets.Unit.Volt;
        //            AIDataRateMin1 = ratioSensor.MinDataRate;
        //            AIDataRate1 = ratioSensor.DataRate;
        //            AIDataRateMax1 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin1 = ratioSensor.MinDataInterval;
        //            AIDataInterval1 = ratioSensor.DataInterval;
        //            AIDataIntervalMax1 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI1_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI1_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI1_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI1_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage1 = e.Voltage;
        //}

        //void AI1_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage1 = e.VoltageRatio;
        //}

        //private void AI1_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI1_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI1_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw1;
        //public Int32? AIRaw1
        //{
        //    get => _aIRaw1;
        //    set
        //    {
        //        if (_aIRaw1 == value)
        //            return;
        //        _aIRaw1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate1;
        //public Double AIDataRate1
        //{
        //    get => _aIDataRate1;
        //    set
        //    {
        //        if (_aIDataRate1 == value)
        //            return;
        //        _aIDataRate1 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax1;
        //public Double AIDataRateMax1
        //{
        //    get => _aIDataRateMax1;
        //    set
        //    {
        //        if (_aIDataRateMax1 == value)
        //            return;
        //        _aIDataRateMax1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin1;
        //public Double AIDataRateMin1
        //{
        //    get => _aIDataRateMin1;
        //    set
        //    {
        //        if (_aIDataRateMin1 == value)
        //            return;
        //        _aIDataRateMin1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin1;
        //public Double AIDataIntervalMin1
        //{
        //    get => _aIDataIntervalMin1;
        //    set
        //    {
        //        if (_aIDataIntervalMin1 == value)
        //            return;
        //        _aIDataIntervalMin1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval1;
        //public Int32 AIDataInterval1
        //{
        //    get => _aIDataInterval1;
        //    set
        //    {
        //        if (_aIDataInterval1 == value)
        //            return;
        //        _aIDataInterval1 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[1].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[1].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax1;
        //public Double AIDataIntervalMax1
        //{
        //    get => _aIDataIntervalMax1;
        //    set
        //    {
        //        if (_aIDataIntervalMax1 == value)
        //            return;
        //        _aIDataIntervalMax1 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A2

        //private Phidgets.VoltageSensorType _aISensorType2;
        //public Phidgets.VoltageSensorType AISensorType2
        //{
        //    get => _aISensorType2;
        //    set
        //    {
        //        if (_aISensorType2 == value)
        //            return;
        //        _aISensorType2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType2;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType2
        //{
        //    get => _aIRatioSensorType2;
        //    set
        //    {
        //        if (_aIRatioSensorType2 == value)
        //            return;
        //        _aIRatioSensorType2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage2;
        //public Double? AIVoltage2
        //{
        //    get => _aIVoltage2;
        //    set
        //    {
        //        if (_aIVoltage2 == value)
        //            return;
        //        _aIVoltage2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit2;
        //public Phidgets.Unit AIUnit2
        //{
        //    get => _aIUnit2;
        //    set
        //    {
        //        if (_aIUnit2 == value)
        //            return;
        //        _aIUnit2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI2_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit2 = Phidgets.Unit.Volt;
        //            AIDataRateMin2 = sensor.MinDataRate;
        //            AIDataRate2 = sensor.DataRate;
        //            AIDataRateMax2 = sensor.MaxDataRate;
        //            AIDataIntervalMin2 = sensor.MinDataInterval;
        //            AIDataInterval2 = sensor.DataInterval;
        //            AIDataIntervalMax2 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit2 = Phidgets.Unit.Volt;
        //            AIDataRateMin2 = ratioSensor.MinDataRate;
        //            AIDataRate2 = ratioSensor.DataRate;
        //            AIDataRateMax2 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin2 = ratioSensor.MinDataInterval;
        //            AIDataInterval2 = ratioSensor.DataInterval;
        //            AIDataIntervalMax2 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI2_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI2_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI2_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI2_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage2 = e.Voltage;
        //}

        //void AI2_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage2 = e.VoltageRatio;
        //}

        //private void AI2_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI2_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI2_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw2;
        //public Int32? AIRaw2
        //{
        //    get => _aIRaw2;
        //    set
        //    {
        //        if (_aIRaw2 == value)
        //            return;
        //        _aIRaw2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate2;
        //public Double AIDataRate2
        //{
        //    get => _aIDataRate2;
        //    set
        //    {
        //        if (_aIDataRate2 == value)
        //            return;
        //        _aIDataRate2 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax2;
        //public Double AIDataRateMax2
        //{
        //    get => _aIDataRateMax2;
        //    set
        //    {
        //        if (_aIDataRateMax2 == value)
        //            return;
        //        _aIDataRateMax2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin2;
        //public Double AIDataRateMin2
        //{
        //    get => _aIDataRateMin2;
        //    set
        //    {
        //        if (_aIDataRateMin2 == value)
        //            return;
        //        _aIDataRateMin2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin2;
        //public Double AIDataIntervalMin2
        //{
        //    get => _aIDataIntervalMin2;
        //    set
        //    {
        //        if (_aIDataIntervalMin2 == value)
        //            return;
        //        _aIDataIntervalMin2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval2;
        //public Int32 AIDataInterval2
        //{
        //    get => _aIDataInterval2;
        //    set
        //    {
        //        if (_aIDataInterval2 == value)
        //            return;
        //        _aIDataInterval2 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[2].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[2].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax2;
        //public Double AIDataIntervalMax2
        //{
        //    get => _aIDataIntervalMax2;
        //    set
        //    {
        //        if (_aIDataIntervalMax2 == value)
        //            return;
        //        _aIDataIntervalMax2 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A3

        //private Phidgets.VoltageSensorType _aISensorType3;
        //public Phidgets.VoltageSensorType AISensorType3
        //{
        //    get => _aISensorType3;
        //    set
        //    {
        //        if (_aISensorType3 == value)
        //            return;
        //        _aISensorType3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType3;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType3
        //{
        //    get => _aIRatioSensorType3;
        //    set
        //    {
        //        if (_aIRatioSensorType3 == value)
        //            return;
        //        _aIRatioSensorType3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage3;
        //public Double? AIVoltage3
        //{
        //    get => _aIVoltage3;
        //    set
        //    {
        //        if (_aIVoltage3 == value)
        //            return;
        //        _aIVoltage3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit3;
        //public Phidgets.Unit AIUnit3
        //{
        //    get => _aIUnit3;
        //    set
        //    {
        //        if (_aIUnit3 == value)
        //            return;
        //        _aIUnit3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI3_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit3 = Phidgets.Unit.Volt;
        //            AIDataRateMin3 = sensor.MinDataRate;
        //            AIDataRate3 = sensor.DataRate;
        //            AIDataRateMax3 = sensor.MaxDataRate;
        //            AIDataIntervalMin3 = sensor.MinDataInterval;
        //            AIDataInterval3 = sensor.DataInterval;
        //            AIDataIntervalMax3 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit3 = Phidgets.Unit.Volt;
        //            AIDataRateMin3 = ratioSensor.MinDataRate;
        //            AIDataRate3 = ratioSensor.DataRate;
        //            AIDataRateMax3 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin3 = ratioSensor.MinDataInterval;
        //            AIDataInterval3 = ratioSensor.DataInterval;
        //            AIDataIntervalMax3 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI3_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI3_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI3_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI3_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage3 = e.Voltage;
        //}

        //void AI3_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage3 = e.VoltageRatio;
        //}

        //private void AI3_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI3_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI3_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw3;
        //public Int32? AIRaw3
        //{
        //    get => _aIRaw3;
        //    set
        //    {
        //        if (_aIRaw3 == value)
        //            return;
        //        _aIRaw3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate3;
        //public Double AIDataRate3
        //{
        //    get => _aIDataRate3;
        //    set
        //    {
        //        if (_aIDataRate3 == value)
        //            return;
        //        _aIDataRate3 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax3;
        //public Double AIDataRateMax3
        //{
        //    get => _aIDataRateMax3;
        //    set
        //    {
        //        if (_aIDataRateMax3 == value)
        //            return;
        //        _aIDataRateMax3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin3;
        //public Double AIDataRateMin3
        //{
        //    get => _aIDataRateMin3;
        //    set
        //    {
        //        if (_aIDataRateMin3 == value)
        //            return;
        //        _aIDataRateMin3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin3;
        //public Double AIDataIntervalMin3
        //{
        //    get => _aIDataIntervalMin3;
        //    set
        //    {
        //        if (_aIDataIntervalMin3 == value)
        //            return;
        //        _aIDataIntervalMin3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval3;
        //public Int32 AIDataInterval3
        //{
        //    get => _aIDataInterval3;
        //    set
        //    {
        //        if (_aIDataInterval3 == value)
        //            return;
        //        _aIDataInterval3 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[3].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[3].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax3;
        //public Double AIDataIntervalMax3
        //{
        //    get => _aIDataIntervalMax3;
        //    set
        //    {
        //        if (_aIDataIntervalMax3 == value)
        //            return;
        //        _aIDataIntervalMax3 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A4

        //private Phidgets.VoltageSensorType _aISensorType4;
        //public Phidgets.VoltageSensorType AISensorType4
        //{
        //    get => _aISensorType4;
        //    set
        //    {
        //        if (_aISensorType4 == value)
        //            return;
        //        _aISensorType4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType4;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType4
        //{
        //    get => _aIRatioSensorType4;
        //    set
        //    {
        //        if (_aIRatioSensorType4 == value)
        //            return;
        //        _aIRatioSensorType4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage4;
        //public Double? AIVoltage4
        //{
        //    get => _aIVoltage4;
        //    set
        //    {
        //        if (_aIVoltage4 == value)
        //            return;
        //        _aIVoltage4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit4;
        //public Phidgets.Unit AIUnit4
        //{
        //    get => _aIUnit4;
        //    set
        //    {
        //        if (_aIUnit4 == value)
        //            return;
        //        _aIUnit4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI4_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit4 = Phidgets.Unit.Volt;
        //            AIDataRateMin4 = sensor.MinDataRate;
        //            AIDataRate4 = sensor.DataRate;
        //            AIDataRateMax4 = sensor.MaxDataRate;
        //            AIDataIntervalMin4 = sensor.MinDataInterval;
        //            AIDataInterval4 = sensor.DataInterval;
        //            AIDataIntervalMax4 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit4 = Phidgets.Unit.Volt;
        //            AIDataRateMin4 = ratioSensor.MinDataRate;
        //            AIDataRate4 = ratioSensor.DataRate;
        //            AIDataRateMax4 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin4 = ratioSensor.MinDataInterval;
        //            AIDataInterval4 = ratioSensor.DataInterval;
        //            AIDataIntervalMax4 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI4_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI4_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_VoltageInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI4_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI4_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage4 = e.Voltage;
        //}

        //void AI4_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage4 = e.VoltageRatio;
        //}

        //private void AI4_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI4_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI4_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw4;
        //public Int32? AIRaw4
        //{
        //    get => _aIRaw4;
        //    set
        //    {
        //        if (_aIRaw4 == value)
        //            return;
        //        _aIRaw4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate4;
        //public Double AIDataRate4
        //{
        //    get => _aIDataRate4;
        //    set
        //    {
        //        if (_aIDataRate4 == value)
        //            return;
        //        _aIDataRate4 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax4;
        //public Double AIDataRateMax4
        //{
        //    get => _aIDataRateMax4;
        //    set
        //    {
        //        if (_aIDataRateMax4 == value)
        //            return;
        //        _aIDataRateMax4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin4;
        //public Double AIDataRateMin4
        //{
        //    get => _aIDataRateMin4;
        //    set
        //    {
        //        if (_aIDataRateMin4 == value)
        //            return;
        //        _aIDataRateMin4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin4;
        //public Double AIDataIntervalMin4
        //{
        //    get => _aIDataIntervalMin4;
        //    set
        //    {
        //        if (_aIDataIntervalMin4 == value)
        //            return;
        //        _aIDataIntervalMin4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval4;
        //public Int32 AIDataInterval4
        //{
        //    get => _aIDataInterval4;
        //    set
        //    {
        //        if (_aIDataInterval4 == value)
        //            return;
        //        _aIDataInterval4 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[4].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[4].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax4;
        //public Double AIDataIntervalMax4
        //{
        //    get => _aIDataIntervalMax4;
        //    set
        //    {
        //        if (_aIDataIntervalMax4 == value)
        //            return;
        //        _aIDataIntervalMax4 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A5

        //private Phidgets.VoltageSensorType _aISensorType5;
        //public Phidgets.VoltageSensorType AISensorType5
        //{
        //    get => _aISensorType5;
        //    set
        //    {
        //        if (_aISensorType5 == value)
        //            return;
        //        _aISensorType5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType5;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType5
        //{
        //    get => _aIRatioSensorType5;
        //    set
        //    {
        //        if (_aIRatioSensorType5 == value)
        //            return;
        //        _aIRatioSensorType5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage5;
        //public Double? AIVoltage5
        //{
        //    get => _aIVoltage5;
        //    set
        //    {
        //        if (_aIVoltage5 == value)
        //            return;
        //        _aIVoltage5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit5;
        //public Phidgets.Unit AIUnit5
        //{
        //    get => _aIUnit5;
        //    set
        //    {
        //        if (_aIUnit5 == value)
        //            return;
        //        _aIUnit5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI5_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit5 = Phidgets.Unit.Volt;
        //            AIDataRateMin5 = sensor.MinDataRate;
        //            AIDataRate5 = sensor.DataRate;
        //            AIDataRateMax5 = sensor.MaxDataRate;
        //            AIDataIntervalMin5 = sensor.MinDataInterval;
        //            AIDataInterval5 = sensor.DataInterval;
        //            AIDataIntervalMax5 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit5 = Phidgets.Unit.Volt;
        //            AIDataRateMin5 = ratioSensor.MinDataRate;
        //            AIDataRate5 = ratioSensor.DataRate;
        //            AIDataRateMax5 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin5 = ratioSensor.MinDataInterval;
        //            AIDataInterval5 = ratioSensor.DataInterval;
        //            AIDataIntervalMax5 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI5_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI5_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_SensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI5_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI5_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage5 = e.Voltage;
        //}

        //void AI5_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_VoltageRatioChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage5 = e.VoltageRatio;
        //}

        //private void AI5_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI5_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI5_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw5;
        //public Int32? AIRaw5
        //{
        //    get => _aIRaw5;
        //    set
        //    {
        //        if (_aIRaw5 == value)
        //            return;
        //        _aIRaw5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate5;
        //public Double AIDataRate5
        //{
        //    get => _aIDataRate5;
        //    set
        //    {
        //        if (_aIDataRate5 == value)
        //            return;
        //        _aIDataRate5 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax5;
        //public Double AIDataRateMax5
        //{
        //    get => _aIDataRateMax5;
        //    set
        //    {
        //        if (_aIDataRateMax5 == value)
        //            return;
        //        _aIDataRateMax5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin5;
        //public Double AIDataRateMin5
        //{
        //    get => _aIDataRateMin5;
        //    set
        //    {
        //        if (_aIDataRateMin5 == value)
        //            return;
        //        _aIDataRateMin5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin5;
        //public Double AIDataIntervalMin5
        //{
        //    get => _aIDataIntervalMin5;
        //    set
        //    {
        //        if (_aIDataIntervalMin5 == value)
        //            return;
        //        _aIDataIntervalMin5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval5;
        //public Int32 AIDataInterval5
        //{
        //    get => _aIDataInterval5;
        //    set
        //    {
        //        if (_aIDataInterval5 == value)
        //            return;
        //        _aIDataInterval5 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[5].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[5].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax5;
        //public Double AIDataIntervalMax5
        //{
        //    get => _aIDataIntervalMax5;
        //    set
        //    {
        //        if (_aIDataIntervalMax5 == value)
        //            return;
        //        _aIDataIntervalMax5 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A6

        //private Phidgets.VoltageSensorType _aISensorType6;
        //public Phidgets.VoltageSensorType AISensorType6
        //{
        //    get => _aISensorType6;
        //    set
        //    {
        //        if (_aISensorType6 == value)
        //            return;
        //        _aISensorType6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType6;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType6
        //{
        //    get => _aIRatioSensorType6;
        //    set
        //    {
        //        if (_aIRatioSensorType6 == value)
        //            return;
        //        _aIRatioSensorType6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage6;
        //public Double? AIVoltage6
        //{
        //    get => _aIVoltage6;
        //    set
        //    {
        //        if (_aIVoltage6 == value)
        //            return;
        //        _aIVoltage6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit6;
        //public Phidgets.Unit AIUnit6
        //{
        //    get => _aIUnit6;
        //    set
        //    {
        //        if (_aIUnit6 == value)
        //            return;
        //        _aIUnit6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI6_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit6 = Phidgets.Unit.Volt;
        //            AIDataRateMin6 = sensor.MinDataRate;
        //            AIDataRate6 = sensor.DataRate;
        //            AIDataRateMax6 = sensor.MaxDataRate;
        //            AIDataIntervalMin6 = sensor.MinDataInterval;
        //            AIDataInterval6 = sensor.DataInterval;
        //            AIDataIntervalMax6 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit6 = Phidgets.Unit.Volt;
        //            AIDataRateMin6 = ratioSensor.MinDataRate;
        //            AIDataRate6 = ratioSensor.DataRate;
        //            AIDataRateMax6 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin6 = ratioSensor.MinDataInterval;
        //            AIDataInterval6 = ratioSensor.DataInterval;
        //            AIDataIntervalMax6 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI6_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI6_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_SensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI6_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI6_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage6 = e.Voltage;
        //}

        //void AI6_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_VoltageChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage6 = e.VoltageRatio;
        //}

        //private void AI6_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI6_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI6_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw6;
        //public Int32? AIRaw6
        //{
        //    get => _aIRaw6;
        //    set
        //    {
        //        if (_aIRaw6 == value)
        //            return;
        //        _aIRaw6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate6;
        //public Double AIDataRate6
        //{
        //    get => _aIDataRate6;
        //    set
        //    {
        //        if (_aIDataRate6 == value)
        //            return;
        //        _aIDataRate6 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax6;
        //public Double AIDataRateMax6
        //{
        //    get => _aIDataRateMax6;
        //    set
        //    {
        //        if (_aIDataRateMax6 == value)
        //            return;
        //        _aIDataRateMax6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin6;
        //public Double AIDataRateMin6
        //{
        //    get => _aIDataRateMin6;
        //    set
        //    {
        //        if (_aIDataRateMin6 == value)
        //            return;
        //        _aIDataRateMin6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin6;
        //public Double AIDataIntervalMin6
        //{
        //    get => _aIDataIntervalMin6;
        //    set
        //    {
        //        if (_aIDataIntervalMin6 == value)
        //            return;
        //        _aIDataIntervalMin6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval6;
        //public Int32 AIDataInterval6
        //{
        //    get => _aIDataInterval6;
        //    set
        //    {
        //        if (_aIDataInterval6 == value)
        //            return;
        //        _aIDataInterval6 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[6].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[6].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax6;
        //public Double AIDataIntervalMax6
        //{
        //    get => _aIDataIntervalMax6;
        //    set
        //    {
        //        if (_aIDataIntervalMax6 == value)
        //            return;
        //        _aIDataIntervalMax6 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#region Sensor A7

        //private Phidgets.VoltageSensorType _aISensorType7;
        //public Phidgets.VoltageSensorType AISensorType7
        //{
        //    get => _aISensorType7;
        //    set
        //    {
        //        if (_aISensorType7 == value)
        //            return;
        //        _aISensorType7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.VoltageRatioSensorType _aIRatioSensorType7;
        //public Phidgets.VoltageRatioSensorType AIRatioSensorType7
        //{
        //    get => _aIRatioSensorType7;
        //    set
        //    {
        //        if (_aIRatioSensorType7 == value)
        //            return;
        //        _aIRatioSensorType7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double? _aIVoltage7;
        //public Double? AIVoltage7
        //{
        //    get => _aIVoltage7;
        //    set
        //    {
        //        if (_aIVoltage7 == value)
        //            return;
        //        _aIVoltage7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Phidgets.Unit _aIUnit7;
        //public Phidgets.Unit AIUnit7
        //{
        //    get => _aIUnit7;
        //    set
        //    {
        //        if (_aIUnit7 == value)
        //            return;
        //        _aIUnit7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //void AI7_Attach(object sender, AttachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_Attach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    switch (sender.GetType().Name)
        //    {
        //        case nameof(Phidgets.VoltageInput):
        //            var sensor = (Phidgets.VoltageInput)sender;
        //            AIUnit7 = Phidgets.Unit.Volt;
        //            AIDataRateMin7 = sensor.MinDataRate;
        //            AIDataRate7 = sensor.DataRate;
        //            AIDataRateMax7 = sensor.MaxDataRate;
        //            AIDataIntervalMin7 = sensor.MinDataInterval;
        //            AIDataInterval7 = sensor.DataInterval;
        //            AIDataIntervalMax7 = sensor.MaxDataInterval;

        //            break;

        //        case nameof(Phidgets.VoltageRatioInput):
        //            var ratioSensor = (Phidgets.VoltageRatioInput)sender;
        //            AIUnit7 = Phidgets.Unit.Volt;
        //            AIDataRateMin7 = ratioSensor.MinDataRate;
        //            AIDataRate7 = ratioSensor.DataRate;
        //            AIDataRateMax7 = ratioSensor.MaxDataRate;
        //            AIDataIntervalMin7 = ratioSensor.MinDataInterval;
        //            AIDataInterval7 = ratioSensor.DataInterval;
        //            AIDataIntervalMax7 = ratioSensor.MaxDataInterval;
        //            break;
        //    }
        //}

        //void AI7_PropertyChange(object sender, PropertyChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_PropertyChange: {e.PropertyName}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI7_VoltageInputSensorChange(object sender, VoltageInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_SensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private void AI7_VoltageRatioInputSensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_VoltageRatioInputSensorChange: {e.SensorValue} {e.SensorUnit}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI7_VoltageChange(object sender, VoltageInputVoltageChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_VoltageChange: {e.Voltage}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage7 = e.Voltage;
        //}

        //void AI7_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI0_VoltageChange: {e.VoltageRatio}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }

        //    AIVoltage7 = e.VoltageRatio;
        //}
        //private void AI7_Detach(object sender, DetachEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_Detach:", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //void AI7_Error(object sender, PhidgetsEvents.ErrorEventArgs e)
        //{
        //    if (LogPhidgetEvents)
        //    {
        //        try
        //        {
        //            Log.EVENT_HANDLER($"AI7_Error: sender:{sender} {e.Code} - {e.Description}", Common.LOG_CATEGORY);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, Common.LOG_CATEGORY);
        //        }
        //    }
        //}

        //private Int32? _aIRaw7;
        //public Int32? AIRaw7
        //{
        //    get => _aIRaw7;
        //    set
        //    {
        //        if (_aIRaw7 == value)
        //            return;
        //        _aIRaw7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRate7;
        //public Double AIDataRate7
        //{
        //    get => _aIDataRate7;
        //    set
        //    {
        //        if (_aIDataRate7 == value)
        //            return;
        //        _aIDataRate7 = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMax7;
        //public Double AIDataRateMax7
        //{
        //    get => _aIDataRateMax7;
        //    set
        //    {
        //        if (_aIDataRateMax7 == value)
        //            return;
        //        _aIDataRateMax7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataRateMin7;
        //public Double AIDataRateMin7
        //{
        //    get => _aIDataRateMin7;
        //    set
        //    {
        //        if (_aIDataRateMin7 == value)
        //            return;
        //        _aIDataRateMin7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMin7;
        //public Double AIDataIntervalMin7
        //{
        //    get => _aIDataIntervalMin7;
        //    set
        //    {
        //        if (_aIDataIntervalMin7 == value)
        //            return;
        //        _aIDataIntervalMin7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int32 _aIDataInterval7;
        //public Int32 AIDataInterval7
        //{
        //    get => _aIDataInterval7;
        //    set
        //    {
        //        if (_aIDataInterval7 == value)
        //            return;
        //        _aIDataInterval7 = value;

        //        // TODO(crhodes)
        //        // Maybe switch statement based on which type of input being used
        //        ActiveInterfaceKit.VoltageInputs[7].DataInterval = value;
        //        //ActiveInterfaceKit.VoltageRatioInputs[7].DataInterval = value;

        //        OnPropertyChanged();
        //    }
        //}

        //private Double _aIDataIntervalMax7;
        //public Double AIDataIntervalMax7
        //{
        //    get => _aIDataIntervalMax7;
        //    set
        //    {
        //        if (_aIDataIntervalMax7 == value)
        //            return;
        //        _aIDataIntervalMax7 = value;
        //        OnPropertyChanged();
        //    }
        //}

        //#endregion

        //#endregion

        //#endregion

        #endregion

        #endregion

        #endregion

        #region Event Handlers

        //private void ActiveInterfaceKit_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.Phidget device = (Phidget22.Phidget)sender;
        //        Log.Trace($"ActiveInterfaceKit_Attach {device.DeviceName},{device.ServerHostname},{device.ServerPeerName} S#:{device.DeviceSerialNumber}", Common.LOG_CATEGORY);

        //        DeviceAttached = device.Attached;

        //        // TODO(crhodes)
        //        // This is where properties should be grabbed
        //        UpdateInterfaceKitProperties();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}

        //private void ActiveInterfaceKit_Detach(object sender, Phidget22.Events.DetachEventArgs e)
        //{
        //    try
        //    {
        //        Phidget22.Phidget device = (Phidget22.Phidget)sender;
        //        Log.Trace($"ActiveInterfaceKit_Detach {device.DeviceName},{device.ServerHostname},{device.ServerPeerName} S#:{device.DeviceSerialNumber}", Common.LOG_CATEGORY);

        //        DeviceAttached = device.Attached;

        //        // TODO(crhodes)
        //        // What kind of cleanup?  Maybe set ActiveInterfaceKit to null.  Clear UI
        //        UpdateInterfaceKitProperties();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }
        //}



        #endregion

        #region Commands

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Enter", Common.LOG_CATEGORY);

            Message = "ConfigFileName_DoubleClick";

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(ConfigFileName_DoubleClick) Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region OpenInterfaceKit Command

        public DelegateCommand OpenInterfaceKitCommand { get; set; }
        public string OpenInterfaceKitContent { get; set; } = "Open";
        public string OpenInterfaceKitToolTip { get; set; } = "OpenInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string OpenInterfaceKitContent { get; set; } = "ViewName_OpenInterfaceKitContent";
        //public string OpenInterfaceKitToolTip { get; set; } = "ViewName_OpenInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenInterfaceKitContent">OpenInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_OpenInterfaceKitContentToolTip">OpenInterfaceKit ToolTip</system:String>  

        public async void OpenInterfaceKit()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenInterfaceKit) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenInterfaceKit";
            
            //ActiveInterfaceKit = new InterfaceKitEx(
            //    SelectedHost.IPAddress,
            //    SelectedHost.Port,
            //    SelectedInterfaceKit.SerialNumber,
            //    EventAggregator);

            //ActiveInterfaceKit = new InterfaceKitEx(
            //    SelectedInterfaceKit.SerialNumber,
            //    Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels,
            //    EventAggregator);

            // FIX(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.Attach += ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.InterfaceKit.Detach += ActiveInterfaceKit_Detach;


            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The InterfaceKitEx attaches to these events also.
            // It logs the changes if Log{Input,Output,Sensor}ChangeEvents are set to true.

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.OutputChange += ActiveInterfaceKit_OutputChange;
            //ActiveInterfaceKit.InterfaceKit.InputChange += ActiveInterfaceKit_InputChange;

            // NOTE(crhodes)
            // Let's do see if we can watch some analog data stream in.

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.SensorChange += ActiveInterfaceKit_SensorChange;

            ConfigurePhidgets();

            for (int i = 0; i < DigitalOutputs.Count(); i++)
            {
                // NOTE(crhodes)
                // If do not specify a timeout, Open() returns
                // before initial state is available

                DigitalOutputs[i].Open();
                //await Task.Run(() => DigitalOutputs[i].Open(500));
                var dOut = DigitalOutputs[i];

                // NOTE(crhodes)
                // Have to set attached here as it is not set
                // until after Attach Event completes
                //DigitalOutputs[i].IsAttached = DigitalOutputs[i].Attached;
            }

            //ActiveInterfaceKit.LogPhidgetEvents = LogPhidgetEvents;

            ////ActiveInterfaceKit.LogInputChangeEvents = LogInputChangeEvents;
            ////ActiveInterfaceKit.LogOutputChangeEvents = LogOutputChangeEvents;
            ////ActiveInterfaceKit.LogSensorChangeEvents = LogSensorChangeEvents;

            ////ActiveInterfaceKit.PhidgetDeviceAttached += ActiveInterfaceKit_PhidgetDeviceAttached;

            //var pdBefore = Phidget22Device;

            //await Task.Run(() => ActiveInterfaceKit.Open());

            //Phidget22Device = ActiveInterfaceKit.PhysicalPhidget;

            // NOTE(crhodes)
            // This won't work as Phidget22Device won't be set until Phidget_Attach event fires

            //DeviceAttached = Phidget22Device.Attached;

            // NOTE(crhodes)
            // I don't think these are gonna fire given how we populate PhysicalPhidget

            //ActiveInterfaceKit.PhysicalPhidget.Attach += ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.PhysicalPhidget.Detach += ActiveInterfaceKit_Detach;


            //ActiveInterfaceKit.LogPhidgetEvents = LogPhidgetEvents;

            //ActiveInterfaceKit.LogInputChangeEvents = LogInputChangeEvents;
            //ActiveInterfaceKit.LogOutputChangeEvents = LogOutputChangeEvents;
            //ActiveInterfaceKit.LogSensorChangeEvents = LogSensorChangeEvents;

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenInterfaceKitEvent>().Publish(
            //      new OpenInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenInterfaceKitEvent>().Subscribe(OpenInterfaceKit);

            // End Cut Four

            //OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            //CloseInterfaceKitCommand.RaiseCanExecuteChanged();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenInterfaceKit) Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ConfigurePhidgets()
        {
            // NOTE(crhodes)
            // This ViewModel needs to support all types of InterfaceKits
            // Configure all channels that might exist and wire up event handlers
            // The SelectedInterfaceKit property change event can enable and disable as needed.
            // TODO(crhodes)
            // Figure out what do in UI (if anything) if all channels not present on a device
            // Hide controls maybe

            DeviceChannels deviceChannels = Common.PhidgetDeviceLibrary.AvailablePhidgets[SelectedInterfaceKit.SerialNumber].DeviceChannels;

            DigitalInputs = new DigitalInputEx[deviceChannels.DigitalInputCount];
            //DigitalOutputs = new DigitalOutputEx[deviceChannels.DigitalOutputCount];
            //VoltageInputs = new Phidgets.VoltageInput[deviceChannels.VoltageInputCount];
            //VoltageRatioInputs = new Phidgets.VoltageRatioInput[deviceChannels.VoltageRatioInputCount];
            //VoltageOutputs = new Phidgets.VoltageOutput[deviceChannels.VoltageOutputCount];

            // NOTE(crhodes)
            // Cannot do this get as we don't have a selected Phidget/SerialNumber

            ConfigureDigitalInputs(deviceChannels.DigitalInputCount);
            //ConfigureDigitalOutputs(deviceChannels.DigitalOutputCount);
            ConfigureVoltageInputs(deviceChannels.VoltageInputCount);
            ConfigureVoltageRatioInputs(deviceChannels.VoltageRatioInputCount);
        }



        // TODO(crhodes)
        // Maybe this is where we use ChannelCounts and some type of Configuration Request
        // to only do this for some

        private void ConfigureDigitalInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            for (int i = 0; i < channelCount; i++)
            {
                //DigitalInputs[i] = new DigitalInputEx(SelectedInterfaceKit.SerialNumber, , ;
                //var channel = DigitalInputs[i];

                //channel.DeviceSerialNumber = SerialNumber;
                //channel.Channel = i;
                //channel.IsHubPortDevice = false;
                //channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
                //channel.StateChange += Channel_DigitalInputStateChange;
            }

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[0].Attach += DI0_Attach;
            //    ActiveInterfaceKit.DigitalInputs[0].Detach += DI0_Detach;
            //    ActiveInterfaceKit.DigitalInputs[0].PropertyChange += DI0_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[0].StateChange += DI0_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[0].Error += DI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[1].Attach += DI1_Attach;
            //    ActiveInterfaceKit.DigitalInputs[1].Detach += DI1_Detach;
            //    ActiveInterfaceKit.DigitalInputs[1].PropertyChange += DI1_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[1].StateChange += DI1_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[1].Error += DI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[2].Attach += DI2_Attach;
            //    ActiveInterfaceKit.DigitalInputs[2].Detach += DI2_Detach;
            //    ActiveInterfaceKit.DigitalInputs[2].PropertyChange += DI2_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[2].StateChange += DI2_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[2].Error += DI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[3].Attach += DI3_Attach;
            //    ActiveInterfaceKit.DigitalInputs[3].Detach += DI3_Detach;
            //    ActiveInterfaceKit.DigitalInputs[3].PropertyChange += DI3_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[3].StateChange += DI3_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[3].Error += DI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[4].Attach += DI4_Attach;
            //    ActiveInterfaceKit.DigitalInputs[4].Detach += DI4_Detach;
            //    ActiveInterfaceKit.DigitalInputs[4].PropertyChange += DI4_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[4].StateChange += DI4_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[4].Error += DI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[5].Attach += DI5_Attach;
            //    ActiveInterfaceKit.DigitalInputs[5].Detach += DI5_Detach;
            //    ActiveInterfaceKit.DigitalInputs[5].PropertyChange += DI5_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[5].StateChange += DI5_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[5].Error += DI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[6].Attach += DI6_Attach;
            //    ActiveInterfaceKit.DigitalInputs[6].Detach += DI6_Detach;
            //    ActiveInterfaceKit.DigitalInputs[6].PropertyChange += DI6_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[6].StateChange += DI6_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[6].Error += DI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[7].Attach += DI7_Attach;
            //    ActiveInterfaceKit.DigitalInputs[7].Detach += DI7_Detach;
            //    ActiveInterfaceKit.DigitalInputs[7].PropertyChange += DI7_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[7].StateChange += DI7_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[7].Error += DI7_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[8].Attach += DI8_Attach;
            //    ActiveInterfaceKit.DigitalInputs[8].Detach += DI8_Detach;
            //    ActiveInterfaceKit.DigitalInputs[8].PropertyChange += DI8_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[8].StateChange += DI8_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[8].Error += DI8_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[9].Attach += DI9_Attach;
            //    ActiveInterfaceKit.DigitalInputs[9].Detach += DI9_Detach;
            //    ActiveInterfaceKit.DigitalInputs[9].PropertyChange += DI9_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[9].StateChange += DI9_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[9].Error += DI9_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[10].Attach += DI10_Attach;
            //    ActiveInterfaceKit.DigitalInputs[10].Detach += DI10_Detach;
            //    ActiveInterfaceKit.DigitalInputs[10].PropertyChange += DI10_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[10].StateChange += DI10_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[10].Error += DI10_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[11].Attach += DI11_Attach;
            //    ActiveInterfaceKit.DigitalInputs[11].Detach += DI11_Detach;
            //    ActiveInterfaceKit.DigitalInputs[11].PropertyChange += DI11_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[11].StateChange += DI11_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[11].Error += DI11_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[12].Attach += DI12_Attach;
            //    ActiveInterfaceKit.DigitalInputs[12].Detach += DI12_Detach;
            //    ActiveInterfaceKit.DigitalInputs[12].PropertyChange += DI12_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[12].StateChange += DI12_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[12].Error += DI12_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[13].Attach += DI13_Attach;
            //    ActiveInterfaceKit.DigitalInputs[13].Detach += DI13_Detach;
            //    ActiveInterfaceKit.DigitalInputs[13].PropertyChange += DI13_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[13].StateChange += DI13_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[13].Error += DI13_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[14].Attach += DI14_Attach;
            //    ActiveInterfaceKit.DigitalInputs[14].Detach += DI14_Detach;
            //    ActiveInterfaceKit.DigitalInputs[14].PropertyChange += DI14_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[14].StateChange += DI14_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[14].Error += DI14_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalInputs[15].Attach += DI15_Attach;
            //    ActiveInterfaceKit.DigitalInputs[15].Detach += DI15_Detach;
            //    ActiveInterfaceKit.DigitalInputs[15].PropertyChange += DI15_PropertyChange;
            //    ActiveInterfaceKit.DigitalInputs[15].StateChange += DI15_StateChange;
            //    ActiveInterfaceKit.DigitalInputs[15].Error += DI15_Error;
            //}
        }

        private void ConfigureDigitalOutputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            for (Int16 i = 0; i < channelCount; i++)
            {
                //DigitalOutputs[i] = new DigitalOutputEx(
                //    SelectedInterfaceKit.SerialNumber,
                //    new DigitalOutputConfiguration() { Channel = i },
                //    EventAggregator);

                DigitalOutputs[i] = new DigitalOutputEx(
                    48284,
                    new DigitalOutputConfiguration() { Channel = i },
                    EventAggregator);

                //var channel = DigitalOutputs[i];

                //channel.DeviceSerialNumber = SerialNumber;
                //channel.Channel = i;
                //channel.IsHubPortDevice = false;
                //channel.IsRemote = true;

                //channel.Attach += Phidget_Attach;
                //channel.Detach += Phidget_Detach;
                //channel.Error += Phidget_Error;
                //channel.PropertyChange += Channel_PropertyChange;
            }

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[0].Attach += DO0_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[0].Detach += DO0_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[0].PropertyChange += DO0_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[0].Error += DO0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[1].Attach += DO1_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[1].Detach += DO1_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[1].PropertyChange += DO1_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[1].Error += DO1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[2].Attach += DO2_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[2].Detach += DO2_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[2].PropertyChange += DO2_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[2].Error += DO2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[3].Attach += DO3_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[3].Detach += DO3_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[3].PropertyChange += DO3_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[3].Error += DO3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[4].Attach += DO4_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[4].Detach += DO4_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[4].PropertyChange += DO4_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[4].Error += DO4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[5].Attach += DO5_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[5].Detach += DO5_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[5].PropertyChange += DO5_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[5].Error += DO5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[6].Attach += DO6_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[6].Detach += DO6_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[6].PropertyChange += DO6_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[6].Error += DO6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[7].Attach += DO7_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[7].Detach += DO7_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[7].PropertyChange += DO7_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[7].Error += DO7_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[8].Attach += DO8_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[8].Detach += DO8_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[8].PropertyChange += DO8_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[8].Error += DO8_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[9].Attach += DO9_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[9].Detach += DO9_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[9].PropertyChange += DO9_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[9].Error += DO9_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[10].Attach += DO10_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[10].Detach += DO10_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[10].PropertyChange += DO10_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[10].Error += DO10_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[11].Attach += DO11_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[11].Detach += DO11_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[11].PropertyChange += DO11_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[11].Error += DO11_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[12].Attach += DO12_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[12].Detach += DO12_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[12].PropertyChange += DO12_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[12].Error += DO12_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[13].Attach += DO13_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[13].Detach += DO13_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[13].PropertyChange += DO13_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[13].Error += DO13_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[14].Attach += DO14_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[14].Detach += DO14_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[14].PropertyChange += DO14_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[14].Error += DO14_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.DigitalOutputs[15].Attach += DO15_Attach;
            //    ActiveInterfaceKit.DigitalOutputs[15].Detach += DO15_Detach;
            //    ActiveInterfaceKit.DigitalOutputs[15].PropertyChange += DO15_PropertyChange;
            //    ActiveInterfaceKit.DigitalOutputs[15].Error += DO15_Error;
            //}
        }

        private void ConfigureVoltageInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[0].Attach += AI0_Attach;
            //    ActiveInterfaceKit.VoltageInputs[0].Detach += AI0_Detach;
            //    ActiveInterfaceKit.VoltageInputs[0].PropertyChange += AI0_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[0].SensorChange += AI0_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[0].VoltageChange += AI0_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[0].Error += AI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[1].Attach += AI1_Attach;
            //    ActiveInterfaceKit.VoltageInputs[1].Detach += AI1_Detach;
            //    ActiveInterfaceKit.VoltageInputs[1].PropertyChange += AI1_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[1].SensorChange += AI1_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[1].VoltageChange += AI1_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[1].Error += AI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[2].Attach += AI2_Attach;
            //    ActiveInterfaceKit.VoltageInputs[2].Detach += AI2_Detach;
            //    ActiveInterfaceKit.VoltageInputs[2].PropertyChange += AI2_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[2].SensorChange += AI2_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[2].VoltageChange += AI2_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[2].Error += AI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[3].Attach += AI3_Attach;
            //    ActiveInterfaceKit.VoltageInputs[3].Detach += AI3_Detach;
            //    ActiveInterfaceKit.VoltageInputs[3].PropertyChange += AI3_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[3].SensorChange += AI3_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[3].VoltageChange += AI3_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[3].Error += AI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[4].Attach += AI4_Attach;
            //    ActiveInterfaceKit.VoltageInputs[4].Detach += AI4_Detach;
            //    ActiveInterfaceKit.VoltageInputs[4].PropertyChange += AI4_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[4].SensorChange += AI4_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[4].VoltageChange += AI4_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[4].Error += AI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[5].Attach += AI5_Attach;
            //    ActiveInterfaceKit.VoltageInputs[5].Detach += AI5_Detach;
            //    ActiveInterfaceKit.VoltageInputs[5].PropertyChange += AI5_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[5].SensorChange += AI5_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[5].VoltageChange += AI5_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[5].Error += AI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[6].Attach += AI6_Attach;
            //    ActiveInterfaceKit.VoltageInputs[6].Detach += AI6_Detach;
            //    ActiveInterfaceKit.VoltageInputs[6].PropertyChange += AI6_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[6].SensorChange += AI6_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[6].VoltageChange += AI6_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[6].Error += AI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageInputs[7].Attach += AI7_Attach;
            //    ActiveInterfaceKit.VoltageInputs[7].Detach += AI7_Detach;
            //    ActiveInterfaceKit.VoltageInputs[7].PropertyChange += AI7_PropertyChange;
            //    ActiveInterfaceKit.VoltageInputs[7].SensorChange += AI7_VoltageInputSensorChange;
            //    ActiveInterfaceKit.VoltageInputs[7].VoltageChange += AI7_VoltageChange;
            //    ActiveInterfaceKit.VoltageInputs[7].Error += AI7_Error;
            //}
        }

        private void ConfigureVoltageRatioInputs(Int16 channelCount)
        {
            Int16 configuredChannels = 0;

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Attach += AI0_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Detach += AI0_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].PropertyChange += AI0_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].SensorChange += AI0_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].VoltageRatioChange += AI0_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[0].Error += AI0_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Attach += AI1_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Detach += AI1_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].PropertyChange += AI1_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].SensorChange += AI1_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].VoltageRatioChange += AI1_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[1].Error += AI1_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Attach += AI2_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Detach += AI2_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].PropertyChange += AI2_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].SensorChange += AI2_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].VoltageRatioChange += AI2_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[2].Error += AI2_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Attach += AI3_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Detach += AI3_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].PropertyChange += AI3_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].SensorChange += AI3_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].VoltageRatioChange += AI3_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[3].Error += AI3_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Attach += AI4_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Detach += AI4_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].PropertyChange += AI4_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].SensorChange += AI4_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].VoltageRatioChange += AI4_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[4].Error += AI4_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Attach += AI5_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Detach += AI5_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].PropertyChange += AI5_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].SensorChange += AI5_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].VoltageRatioChange += AI5_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[5].Error += AI5_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Attach += AI6_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Detach += AI6_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].PropertyChange += AI6_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].SensorChange += AI6_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].VoltageRatioChange += AI6_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[6].Error += AI6_Error;
            //}

            //if (channelCount > configuredChannels++)
            //{
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Attach += AI7_Attach;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Detach += AI7_Detach;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].PropertyChange += AI7_PropertyChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].SensorChange += AI7_VoltageRatioInputSensorChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].VoltageRatioChange += AI7_VoltageRatioChange;
            //    ActiveInterfaceKit.VoltageRatioInputs[7].Error += AI7_Error;
            //}
        }


        //private void ActiveInterfaceKit_PhidgetDeviceAttached(object? sender, EventArgs e)
        //{
        //    Phidget22Device = ActiveInterfaceKit.PhysicalPhidget;

        //    DeviceAttached = Phidget22Device.Attached;

        //    // NOTE(crhodes)
        //    // This won't work as Phidget22Device won't be set until Phidget_Attach event fires

        //    //DeviceAttached = Phidget22Device.Attached;

        //    // NOTE(crhodes)
        //    // I don't think these are gonna fire given how we populate PhysicalPhidget

        //    //ActiveInterfaceKit.PhysicalPhidget.Attach += ActiveInterfaceKit_Attach;
        //    //ActiveInterfaceKit.PhysicalPhidget.Detach += ActiveInterfaceKit_Detach;

        //    // FIX(crhodes)
        //    // This is a problem.  We have to wait until all DI, DO, VI, VO devices that were openned
        //    // attach.  Looks like we are going to have to go to separate event handlers for each channel. Ugh
        //    UpdateInterfaceKitProperties();
        //}

        public bool OpenInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedInterfaceKit is not null)
            {
                //if (DeviceAttached is not null)
                //    return !(Boolean)DeviceAttached;
                //else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseInterfaceKit Command

        public DelegateCommand CloseInterfaceKitCommand { get; set; }
        public string CloseInterfaceKitContent { get; set; } = "Close";
        public string CloseInterfaceKitToolTip { get; set; } = "CloseInterfaceKit ToolTip";

        // Can get fancy and use Resources
        //public string CloseInterfaceKitContent { get; set; } = "ViewName_CloseInterfaceKitContent";
        //public string CloseInterfaceKitToolTip { get; set; } = "ViewName_CloseInterfaceKitContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseInterfaceKitContent">CloseInterfaceKit</system:String>
        //    <system:String x:Key="ViewName_CloseInterfaceKitContentToolTip">CloseInterfaceKit ToolTip</system:String>  

        public void CloseInterfaceKit()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseInterfaceKit) Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseInterfaceKit";

            // TODO(crhodes)
            // 
            //ActiveInterfaceKit.InterfaceKit.Attach -= ActiveInterfaceKit_Attach;
            //ActiveInterfaceKit.InterfaceKit.Detach -= ActiveInterfaceKit_Detach;

            //ActiveInterfaceKit.Close();

            for (int i = 0; i < DigitalOutputs.Count(); i++)
            {
                var dOut = DigitalOutputs[i];
                DigitalOutputs[i].Close();
            }

            UpdateInterfaceKitProperties();
            ActiveInterfaceKit = null;
            ClearDigitalInputsAndOutputs();

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();

            CloseInterfaceKitCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseInterfaceKitEvent>().Publish(
            //      new CloseInterfaceKitEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseInterfaceKitEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseInterfaceKitEvent>().Subscribe(CloseInterfaceKit);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseInterfaceKit) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseInterfaceKitCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
            //if (DeviceAttached is not null)
            //    return (Boolean)DeviceAttached;
            //else
            //    return false;
        }

        #endregion

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private void ClearDigitalInputsAndOutputs()
        {
            //DI0 = DO0 = null;
            //DI1 = DO1 = null;
            //DI2 = DO2 = null;
            //DI3 = DO3 = null;
            //DI4 = DO4 = null;
            //DI5 = DO5 = null;
            //DI6 = DO6 = null;
            //DI7 = DO7 = null;
            //DI8 = DO8 = null;
            //DI9 = DO9 = null;
            //DI10 = DO10 = null;
            //DI11 = DO11 = null;
            //DI12 = DO12 = null;
            //DI13 = DO13 = null;
            //DI14 = DO14 = null;
            //DI15 = DO15 = null;
        }

        // TODO(crhodes)
        // 
        //private void PopulateSensorValues(InterfaceKitAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        private void UpdateInterfaceKitProperties()
        {
            //var interfaceKitEx = ActiveInterfaceKit;
            //var phidget = interfaceKitEx.PhysicalPhidget;

            //DO1 = interfaceKitEx.DigitalOutputs[1].State;

            //Phidgets.DigitalInput[] DigitalInputs = 
            //for (int i = 0; i < interfaceKitEx.DigitalInputs.Count(); i++)
            //{
            //    DigitalInputs[i].Open();
            //}

            //for (int i = 0; i < digitalOutputCount; i++)
            //{
            //    DigitalOutputs[i].Open();
            //}

            //for (int i = 0; i < voltageInputCount; i++)
            //{
            //    VoltageInputs[i].Open();
            //}

            //for (int i = 0; i < voltageRatioInputCount; i++)
            //{
            //    VoltageRatioInputs[i].Open();
            //}

            //for (int i = 0; i < voltageOutputCount; i++)
            //{
            //    VoltageOutputs[i].Open();

            //}

            // FIX(crhodes)
            // 
            //if (ActiveInterfaceKit.InterfaceKit.Attached)
            //{
            //    //IkAddress = ActiveInterfaceKit.InterfaceKit.Address;
            //    //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    DeviceAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    //IkAttachedToServer = ActiveInterfaceKit.InterfaceKit.AttachedToServer;
            //    //IkClass = ActiveInterfaceKit.InterfaceKit.Class.ToString();
            //    //IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.InterfaceKit.ID);
            //    //IkLabel = ActiveInterfaceKit.InterfaceKit.Label;
            //    //IkLibraryVersion = Phidget.LibraryVersion;  // This is a static field
            //    //IkName = ActiveInterfaceKit.InterfaceKit.Name;
            //    //IkPort = ActiveInterfaceKit.InterfaceKit.Port;
            //    //IkSerialNumber = ActiveInterfaceKit.InterfaceKit.SerialNumber; // This throws exception
            //    ////IkServerID = ActiveInterfaceKit.ServerID;
            //    //IkType = ActiveInterfaceKit.InterfaceKit.Type;
            //    //IkVersion = ActiveInterfaceKit.InterfaceKit.Version;

            //    var sensors = ActiveInterfaceKit.InterfaceKit.sensors;
            //    InterfaceKitAnalogSensor sensor = null;

            //    // NOTE(crhodes)
            //    // The DataRateMin and DataRateMax do not change.
            //    // Populate them here instead of SensorChange event

            //    // TODO(crhodes)
            //    // May want to grab initial values for all fields here.

            //    for (int i = 0; i < sensors.Count; i++)
            //    {
            //        sensor = sensors[i];

            //        switch (i)
            //        {
            //            case 0:
            //                AIDataRateMax0 = sensor.DataRateMax;
            //                AIDataRateMin0 = sensor.DataRateMin;
            //                break;
            //            case 1:
            //                AIDataRateMax1 = sensor.DataRateMax;
            //                AIDataRateMin1 = sensor.DataRateMin;
            //                break;
            //            case 2:
            //                AIDataRateMax2 = sensor.DataRateMax;
            //                AIDataRateMin2 = sensor.DataRateMin;
            //                break;
            //            case 3:
            //                AIDataRateMax3 = sensor.DataRateMax;
            //                AIDataRateMin3 = sensor.DataRateMin;
            //                break;
            //            case 4:
            //                AIDataRateMax4 = sensor.DataRateMax;
            //                AIDataRateMin4 = sensor.DataRateMin;
            //                break;
            //            case 5:
            //                AIDataRateMax5 = sensor.DataRateMax;
            //                AIDataRateMin5 = sensor.DataRateMin;
            //                break;
            //            case 6:
            //                AIDataRateMax6 = sensor.DataRateMax;
            //                AIDataRateMin6 = sensor.DataRateMin;
            //                break;
            //            case 7:
            //                AIDataRateMax7 = sensor.DataRateMax;
            //                AIDataRateMin7 = sensor.DataRateMin;
            //                break;
            //        }
            //    }
            //}
            //else
            //{
            //    DeviceAttached = null;
            //    // NOTE(crhodes)
            //    // Commented out properties throw exceptions when Phidget not attached
            //    // Just clear field

            //    //IkAddress = ActiveInterfaceKit.Address;
            //    //IkAddress = "";
            //    //IkAttached = ActiveInterfaceKit.InterfaceKit.Attached;
            //    ////IkAttachedToServer = ActiveInterfaceKit.AttachedToServer;
            //    //IkAttachedToServer = false;
            //    //// This doesn't throw exception but let's clear anyway
            //    ////IkClass = ActiveInterfaceKit.Class.ToString();
            //    //IkClass = "";
            //    ////IkID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveInterfaceKit.ID);
            //    //IkID = "";
            //    ////IkLabel = ActiveInterfaceKit.Label;
            //    //IkLabel = "";
            //    ////IkLibraryVersion = ActiveInterfaceKit.LibraryVersion;
            //    //IkLibraryVersion = Phidget.LibraryVersion;
            //    ////IkName = ActiveInterfaceKit.Name;
            //    //IkName = "";
            //    ////IkSerialNumber = ActiveInterfaceKit.SerialNumber;
            //    //IkSerialNumber = null;
            //    ////IkServerID = ActiveInterfaceKit.ServerID;
            //    //IkServerID = "";
            //    ////IkType = ActiveInterfaceKit.Type;
            //    //IkType = "";
            //    ////IkVersion = ActiveInterfaceKit.Version;
            //    //IkVersion = null;
            //}

            OpenInterfaceKitCommand.RaiseCanExecuteChanged();
            CloseInterfaceKitCommand.RaiseCanExecuteChanged();
        }

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
