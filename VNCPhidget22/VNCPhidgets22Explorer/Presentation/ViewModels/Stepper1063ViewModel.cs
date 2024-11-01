using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

using Phidget22;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

using VNCPhidgetConfig = VNCPhidget22.Configuration;

namespace VNCPhidget2221Explorer.Presentation.ViewModels
{
    public class Stepper1063ViewModel : EventViewModelBase, IStepper1063ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public Stepper1063ViewModel(
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

            // Turn off logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = false;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);

            OpenStepperCommand = new DelegateCommand(OpenStepper, OpenStepperCanExecute);
            RefreshStepperCommand = new DelegateCommand(RefreshStepper, RefreshStepperCanExecute);
            CloseStepperCommand = new DelegateCommand(CloseStepper, CloseStepperCanExecute);

            InitializeVelocityCommand = new DelegateCommand<string>(InitializeVelocity, InitializeVelocityCanExecute);
            InitializeAccelerationCommand = new DelegateCommand<string>(InitializeAcceleration, InitializeAccelerationCanExecute);

            RotateCommand = new DelegateCommand<string>(Rotate, RotateCanExecute);

            // If using CommandParameter, figure out TYPE here and below
            // and remove above declaration
            //RotateCommand = new DelegateCommand<TYPE>(Rotate, RotateCanExecute);


            ZeroCurentPositionCommand = new DelegateCommand(ZeroCurentPosition, ZeroCurentPositionCanExecute);

            // If using CommandParameter, figure out TYPE here and below
            // and remove above declaration
            //ZeroCurentPositionCommand = new DelegateCommand<TYPE>(ZeroCurentPosition, ZeroCurentPositionCanExecute);


            // HACK(crhodes)
            // For now just hard code this.  Can have UI let us choose later.
            // This could also come from PerformanceLibrary.
            // See HackAroundViewModel.InitializeViewModel()
            // Or maybe a method on something else in VNCPhidget22.Configuration

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();
               
            Message = "Stepper1063ViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig = 
                JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();
            //this.Sensors2 = phidgetConfig.Sensors.ToList();

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

        private double _degrees;
        public double Degrees
        {
            get => _degrees;
            set
            {
                if (_degrees == value)
                    return;
                _degrees = value;
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
                Steppers = _selectedHost.Steppers?.ToList<VNCPhidgetConfig.Stepper>();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Phidget

        private Phidget22.Phidget _phidgetDevice;
        public Phidget22.Phidget PhidgetDevice
        {
            get => _phidgetDevice;
            set
            {
                if (_phidgetDevice == value)
                    return;
                _phidgetDevice = value;
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

                if (ActiveStepper is not null)
                {
                    ActiveStepper.LogPhidgetEvents = value;

                    // NOTE(crhodes)
                    // There is some logging in StepperProperties that is handled separate
                    // from the logging in StepperEx and PhidgetEx

                    for (int i = 0; i < ActiveStepper.Stepper.steppers.Count; i++)
                    {
                        StepperProperties[i].LogPhidgetEvents = value;
                    }
                }
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

                if (ActiveStepper is not null)
                {
                    ActiveStepper.LogSequenceAction = value;

                    // NOTE(crhodes)
                    // There is some logging in StepperProperties that is handled separate
                    // from the logging in StepperEx and PhidgetEx

                    for (int i = 0; i < ActiveStepper.Stepper.steppers.Count; i++)
                    {
                        StepperProperties[i].LogSequenceAction = value;
                    }
                }
            }
        }

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

        #region Stepper

        #region Stepper Events

        private bool _logCurrentChangeEvents = false;
        public bool LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value)
                    return;
                _logCurrentChangeEvents = value;
                OnPropertyChanged();

                if (ActiveStepper is not null)
                {
                    ActiveStepper.LogCurrentChangeEvents = _logCurrentChangeEvents;
                }
            }
        }

        private bool _logPositionChangeEvents = false;
        public bool LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value)
                    return;
                _logPositionChangeEvents = value;
                OnPropertyChanged();

                if (ActiveStepper is not null)
                {
                    ActiveStepper.LogPositionChangeEvents = _logPositionChangeEvents;
                }
            }
        }

        private bool _logVelocityChangeEvents = false;
        public bool LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value)
                    return;
                _logVelocityChangeEvents = value;
                OnPropertyChanged();

                if (ActiveStepper is not null)
                {
                    ActiveStepper.LogVelocityChangeEvents = _logVelocityChangeEvents;
                }
            }
        }

        #endregion

        private IEnumerable<VNCPhidgetConfig.Stepper> _Steppers;
        public IEnumerable<VNCPhidgetConfig.Stepper> Steppers
        {
            get
            {
                if (null == _Steppers)
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

                return _Steppers;
            }

            set
            {
                _Steppers = value;
                OnPropertyChanged();
            }
        }

        private VNCPhidgetConfig.Stepper _selectedStepper;
        public VNCPhidgetConfig.Stepper SelectedStepper
        {
            get => _selectedStepper;
            set
            {
                if (_selectedStepper == value)
                    return;
                _selectedStepper = value;

                OpenStepperCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        private StepperEx _activeStepper;
        public StepperEx ActiveStepper
        {
            get => _activeStepper;
            set
            {
                if (_activeStepper == value)
                    return;
                _activeStepper = value;


                if (_activeStepper is not null)
                {
                    PhidgetDevice = _activeStepper.Stepper;
                }
                else
                {
                    // TODO(crhodes)
                    // PhidgetDevice = null ???
                    // Will need to declare Phidget22.Phidget?
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private int? _stepperCount;
        public int? StepperCount
        {
            get => _stepperCount;
            set
            {
                if (_stepperCount == value)
                    return;
                _stepperCount = value;
                OnPropertyChanged();
            }
        }

        // NOTE(crhodes)
        // 1063 only supports one stepper

        private StepperProperties[] _stepperProperties = new StepperProperties[1] // StepperProperties[8]
        {
            new StepperProperties() { StepperIndex = 0 },
            //new StepperProperties() { StepperIndex = 1 },
            //new StepperProperties() { StepperIndex = 2 },
            //new StepperProperties() { StepperIndex = 3 },
            //new StepperProperties() { StepperIndex = 4 },
            //new StepperProperties() { StepperIndex = 5 },
            //new StepperProperties() { StepperIndex = 6 },
            //new StepperProperties() { StepperIndex = 7 },
        };

        public StepperProperties[] StepperProperties
        {
            get => _stepperProperties;
            set
            {
                if (_stepperProperties == value)
                    return;
                _stepperProperties = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        //private void PopulateSensorValues(StepperAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        //private void ActiveStepper_SensorChange(object sender, Phidget22.Events.SensorChangeEventArgs e)
        //{
        //    Phidget22.Stepper ifk = (Phidget22.Stepper)sender;

        //    StepperAnalogSensor sensor = ifk.sensors[0];

        //    //SIRaw0 = sensor.RawValue;
        //    //SIDataRate0 = sensor.DataRate;
        //    //SIDataRateMax0 = sensor.DataRateMax;
        //    //SIDataRateMin0 = sensor.DataRateMin;
        //    //SISensitivity0= sensor.Sensitivity;

        //    //var sValue = sensor0.Value;
        //    //var eValue = e.Value;

        //    // NOTE(crhodes)
        //    // DataRateMin and DataRateMax do not change.
        //    // Populate in Attach event

        //    switch (e.Index)
        //    {
        //        case 0:
        //            sensor = ifk.sensors[0];
        //            AI0 = sensor.Value;
        //            AIRaw0 = sensor.RawValue;
        //            AIDataRate0 = sensor.DataRate;
        //            //AIDataRateMax0 = sensor.DataRateMax;
        //            //AIDataRateMin0 = sensor.DataRateMin;
        //            AISensitivity0 = sensor.Sensitivity;
        //            break;
        //        case 1:
        //            sensor = ifk.sensors[1];
        //            AI1 = sensor.Value;
        //            AIRaw1 = sensor.RawValue;
        //            AIDataRate1 = sensor.DataRate;
        //            //AIDataRateMax1 = sensor.DataRateMax;
        //            //AIDataRateMin1 = sensor.DataRateMin;
        //            AISensitivity1 = sensor.Sensitivity;
        //            break;
        //        case 2:
        //            sensor = ifk.sensors[2];
        //            AI2 = sensor.Value;
        //            AIRaw2 = sensor.RawValue;
        //            AIDataRate2 = sensor.DataRate;
        //            //AIDataRateMax2 = sensor.DataRateMax;
        //            //AIDataRateMin2 = sensor.DataRateMin;
        //            AISensitivity2 = sensor.Sensitivity;
        //            break;
        //        case 3:
        //            sensor = ifk.sensors[3];
        //            AI3 = sensor.Value;
        //            AIRaw3 = sensor.RawValue;
        //            AIDataRate3 = sensor.DataRate;
        //            //AIDataRateMax3 = sensor.DataRateMax;
        //            //AIDataRateMin3 = sensor.DataRateMin;
        //            AISensitivity3 = sensor.Sensitivity;
        //            break;
        //        case 4:
        //            sensor = ifk.sensors[4];
        //            AI4 = sensor.Value;
        //            AIRaw4 = sensor.RawValue;
        //            AIDataRate4 = sensor.DataRate;
        //            //AIDataRateMax4 = sensor.DataRateMax;
        //            //AIDataRateMin4 = sensor.DataRateMin;
        //            AISensitivity4 = sensor.Sensitivity;
        //            break;
        //        case 5:
        //            sensor = ifk.sensors[5];
        //            AI5 = sensor.Value;
        //            AIRaw5 = sensor.RawValue;
        //            AIDataRate5 = sensor.DataRate;
        //            //AIDataRateMax5 = sensor.DataRateMax;
        //            //AIDataRateMin5 = sensor.DataRateMin;
        //            AISensitivity5 = sensor.Sensitivity;
        //            break;
        //        case 6:
        //            sensor = ifk.sensors[6];
        //            AI6 = sensor.Value;
        //            AIRaw6 = sensor.RawValue;
        //            AIDataRate6 = sensor.DataRate;
        //            //AIDataRateMax6 = sensor.DataRateMax;
        //            //AIDataRateMin6 = sensor.DataRateMin;
        //            AISensitivity6 = sensor.Sensitivity;
        //            break;
        //        case 7:
        //            sensor = ifk.sensors[7];
        //            AI7 = sensor.Value;
        //            AIRaw7 = sensor.RawValue;
        //            AIDataRate7 = sensor.DataRate;
        //            //AIDataRateMax7 = sensor.DataRateMax;
        //            //AIDataRateMin7 = sensor.DataRateMin;
        //            AISensitivity7 = sensor.Sensitivity;
        //            break;
        //    }
        //}

        //private void ActiveStepper_InputChange(object sender, Phidget22.Events.InputChangeEventArgs e)
        //{
        //    Phidget22.Stepper ifk = (Phidget22.Stepper)sender;

        //    switch (e.Index)
        //    {
        //        case 0:
        //            DI0 = e.Value;
        //            break;
        //        case 1:
        //            DI1 = e.Value;
        //            break;
        //        case 2:
        //            DI2 = e.Value;
        //            break;
        //        case 3:
        //            DI3 = e.Value;
        //            break;
        //        case 4:
        //            DI4 = e.Value;
        //            break;
        //        case 5:
        //            DI5 = e.Value;
        //            break;
        //        case 6:
        //            DI6 = e.Value;
        //            break;
        //        case 7:
        //            DI7 = e.Value;
        //            break;
        //        case 8:
        //            DI8 = e.Value;
        //            break;
        //        case 9:
        //            DI9 = e.Value;
        //            break;
        //        case 10:
        //            DI10 = e.Value;
        //            break;
        //        case 11:
        //            DI11 = e.Value;
        //            break;
        //        case 12:
        //            DI12 = e.Value;
        //            break;
        //        case 13:
        //            DI13 = e.Value;
        //            break;
        //        case 14:
        //            DI14 = e.Value;
        //            break;
        //        case 15:
        //            DI15 = e.Value;
        //            break;
        //    }
        //}


        //private void ActiveStepper_OutputChange(object sender, Phidget22.Events.OutputChangeEventArgs e)
        //{
        //    Phidget22.Stepper ifk = (Phidget22.Stepper)sender;
        //    var outputs = ifk.outputs;
        //    StepperDigitalOutputCollection doc = outputs;

        //    switch (e.Index)
        //    {
        //        case 0:
        //            DO0 = e.Value;
        //            break;
        //        case 1:
        //            DO1 = e.Value;
        //            break;
        //        case 2:
        //            DO2 = e.Value;
        //            break;
        //        case 3:
        //            DO3 = e.Value;
        //            break;
        //        case 4:
        //            DO4 = e.Value;
        //            break;
        //        case 5:
        //            DO5 = e.Value;
        //            break;
        //        case 6:
        //            DO6 = e.Value;
        //            break;
        //        case 7:
        //            DO7 = e.Value;
        //            break;
        //        case 8:
        //            DO8 = e.Value;
        //            break;
        //        case 9:
        //            DO9 = e.Value;
        //            break;
        //        case 10:
        //            DO10 = e.Value;
        //            break;
        //        case 11:
        //            DO11 = e.Value;
        //            break;
        //        case 12:
        //            DO12 = e.Value;
        //            break;
        //        case 13:
        //            DO13 = e.Value;
        //            break;
        //        case 14:
        //            DO14 = e.Value;
        //            break;
        //        case 15:
        //            DO15 = e.Value;
        //            break;
        //    }

        //}

        private void ActiveStepper_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            try
            {
                Phidget22.Phidget device = (Phidget22.Phidget)sender;
                Log.Trace($"ActiveStepper_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = device.Attached;

                StepperProperties[0].StepperEx = ActiveStepper;
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateStepperProperties();

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            RefreshStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();

            InitializeAccelerationCommand.RaiseCanExecuteChanged();
            InitializeVelocityCommand.RaiseCanExecuteChanged();
        }

        private void ActiveStepper_Detach(object sender, Phidget22.Events.DetachEventArgs e)
        {
            try
            {
                Phidget22.Phidget device = (Phidget22.Phidget)sender;
                Log.Trace($"ActiveStepper_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                DeviceAttached = device.Attached;

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveStepper to null.  Clear UI
                UpdateStepperProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void ActiveStepper_VelocityChange(object sender, Phidget22.Events.VelocityChangeEventArgs e)
        {
            Phidget22.Stepper stepper = sender as Phidget22.Stepper;
            var index = e.Index;
            var velocity = e.Velocity;

            StepperProperties[index].Velocity = velocity;
            StepperProperties[index].Stopped = stepper.steppers[index].Stopped;

            //switch (e.Index)
            //{
            //    case 0:
            //        Velocity_S0 = e.Velocity;
            //        break;

            //    //case 1:

            //    //    break;

            //    //case 2:

            //    //    break;

            //    //case 3:

            //    //    break;

            //    //case 4:

            //    //    break;

            //    //case 5:

            //    //    break;

            //    //case 60:

            //    //    break;

            //    //case 7:

            //    //    break;

            //    default:
            //        Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);
            //        break;
            //}
        }

        private void ActiveStepper_PositionChange(object sender, Phidget22.Events.StepperPositionChangeEventArgs e)
        {
            Phidget22.Stepper stepper = sender as Phidget22.Stepper;
            var index = e.Index;
            var position = e.Position;

            StepperProperties[index].CurrentPosition = position;
            StepperProperties[index].Stopped = stepper.steppers[index].Stopped;


            //switch (e.Index)
            //{
            //    case 0:
            //        CurrentPosition_S0 = e.Position;
            //        break;

            //    //case 1:

            //    //    break;

            //    //case 2:

            //    //    break;

            //    //case 3:

            //    //    break;

            //    //case 4:

            //    //    break;

            //    //case 5:

            //    //    break;

            //    //case 60:

            //    //    break;

            //    //case 7:

            //    //    break;

            //    default:
            //        Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);
            //        break;
            //}
        }

        private void ActiveStepper_InputChange(object sender, Phidget22.Events.InputChangeEventArgs e)
        {
            Phidget22.Stepper stepper = sender as Phidget22.Stepper;
            var index = e.Index;
            var value = e.Value;

            // TODO(crhodes)
            // Get some places on UI to update

            switch (e.Index)
            {
                case 0:
                    value = e.Value;
                    break;

                //case 1:

                //    break;

                //case 2:

                //    break;

                //case 3:

                //    break;

                //case 4:

                //    break;

                //case 5:

                //    break;

                //case 60:

                //    break;

                //case 7:
                //    break;

                default:
                    Log.Trace($"InputChange index:{index} value:{value}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveStepper_CurrentChange(object sender, Phidget22.Events.CurrentChangeEventArgs e)
        {
            Phidget22.Stepper stepper = sender as Phidget22.Stepper;
            var index = e.Index;
            var current = e.Current;

            StepperProperties[index].Current = current;

            //switch (e.Index)
            //{
            //    case 0:
            //        Current_S0 = e.Current;
            //        break;

            //    //case 1:

            //    //    break;

            //    //case 2:

            //    //    break;

            //    //case 3:

            //    //    break;

            //    //case 4:

            //    //    break;

            //    //case 5:

            //    //    break;

            //    //case 60:

            //    //    break;

            //    //case 7:

            //    //    break;

            //    default:
            //        Log.Trace($"CurrentChange index:{index} value:{current}", Common.LOG_CATEGORY);
            //        break;
            //}
        }

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

        #region OpenStepper Command

        public DelegateCommand OpenStepperCommand { get; set; }
        public string OpenStepperContent { get; set; } = "Open";
        public string OpenStepperToolTip { get; set; } = "OpenStepper ToolTip";

        // Can get fancy and use Resources
        //public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        //public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        //    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        public async void OpenStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenStepper) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called OpenStepper";

            ActiveStepper = new StepperEx(
                SelectedHost.IPAddress,
                SelectedHost.Port,
                SelectedStepper.SerialNumber,
                EventAggregator);

            ActiveStepper.Stepper.Attach += ActiveStepper_Attach;
            ActiveStepper.Stepper.Detach += ActiveStepper_Detach;

            ActiveStepper.Stepper.CurrentChange += ActiveStepper_CurrentChange;
            ActiveStepper.Stepper.InputChange += ActiveStepper_InputChange;
            ActiveStepper.Stepper.PositionChange += ActiveStepper_PositionChange;
            ActiveStepper.Stepper.VelocityChange += ActiveStepper_VelocityChange;

            // NOTE(crhodes)
            // Capture Digital Input and Output changes so we can update the UI
            // The StepperEx attaches to these events also.
            // Itlogs the changes if xxx is set to true.

            //ActiveStepper.OutputChange += ActiveStepper_OutputChange;
            //ActiveStepper.InputChange += ActiveStepper_InputChange;

            //// NOTE(crhodes)
            //// Let's do see if we can watch some analog data stream in.

            //ActiveStepper.SensorChange += ActiveStepper_SensorChange;

            //await Task.Run(() => ActiveStepper.Open(Common.PhidgetOpenTimeout));

            await Task.Run(() => ActiveStepper.Open(Common.PhidgetOpenTimeout));

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenStepperEvent>().Publish(
            //      new OpenStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class OpenStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenStepperEvent>().Subscribe(OpenStepper);

            // End Cut Four

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenStepper) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool OpenStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (SelectedStepper is not null)
            {
                if (DeviceAttached is not null)
                    return !(Boolean)DeviceAttached;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region InitializeVelocityCommand

        //public DelegateCommand InitializeVelocityCommand { get; set; }
        public DelegateCommand<string> InitializeVelocityCommand { get; set; }
        public string InitializeVelocityContent { get; set; } = "Initilize Velocity";
        public string InitializeVelocityToolTip { get; set; } = "Initialize Velocity using Velocity Scale";

        // Can get fancy and use Resources
        //public string InitializeSlowStepperContent { get; set; } = "ViewName_InitializeSlowStepperContent";
        //public string InitializeSlowStepperToolTip { get; set; } = "ViewName_InitializeSlowStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowStepperContent">InitializeSlowStepper</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowStepperContentToolTip">InitializeSlowStepper ToolTip</system:String>  

        //public void InitializeSlowStepper()

        public void InitializeVelocity(string speed)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeVelocity) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeVelocity";

            if ((Boolean)DeviceAttached)
            {
                //StepperServoCollection servos = ActiveStepper.Stepper.servos;

                try
                {
                    //for (int i = 0; i < servos.Count; i++)
                    //{
                    //    StepperProperties[i].InitializeVelocity(ConvertStringToInitializeMotion(speed));
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish(
            //      new InitializeSlowStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeSlowStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Subscribe(InitializeSlowStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeVelocity) Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowStepperCanExecute()
        public bool InitializeVelocityCanExecute(string speed)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region InitializeAccelerationCommand

        //public DelegateCommand InitializeVelocityCommand { get; set; }
        public DelegateCommand<string> InitializeAccelerationCommand { get; set; }
        public string InitializeAccelerationContent { get; set; } = "Initilize Acceleration";
        public string InitializeAccelerationToolTip { get; set; } = "Initialize Acceleration using Acceleration Scale";

        // Can get fancy and use Resources
        //public string InitializeSlowStepperContent { get; set; } = "ViewName_InitializeSlowStepperContent";
        //public string InitializeSlowStepperToolTip { get; set; } = "ViewName_InitializeSlowStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_InitializeSlowStepperContent">InitializeSlowStepper</system:String>
        //    <system:String x:Key="ViewName_InitializeSlowStepperContentToolTip">InitializeSlowStepper ToolTip</system:String>  

        //public void InitializeSlowStepper()

        public void InitializeAcceleration(string speed)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(InitializeAcceleration) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called InitializeAcceleration";

            if ((Boolean)DeviceAttached)
            {
                StepperStepperCollection steppers = ActiveStepper.Stepper.steppers;

                try
                {
                    for (int i = 0; i < steppers.Count; i++)
                    {
                        StepperProperties[i].InitializeAcceleration(ConvertStringToInitializeMotion(speed));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<InitializeSlowStepperEvent>().Publish(
            //      new InitializeSlowStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class InitializeSlowStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<InitializeSlowStepperEvent>().Subscribe(InitializeSlowStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(InitializeAcceleration) Exit", Common.LOG_CATEGORY, startTicks);
        }


        //public bool InitializeSlowStepperCanExecute()
        public bool InitializeAccelerationCanExecute(string speed)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region RefreshStepper Command

        public DelegateCommand RefreshStepperCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> RefreshStepperCommand { get; set; }
        //public TYPE RefreshStepperCommandParameter;
        public string RefreshStepperContent { get; set; } = "Refresh";
        public string RefreshStepperToolTip { get; set; } = "Refresh ToolTip";

        // Can get fancy and use Resources
        //public string RefreshStepperContent { get; set; } = "ViewName_RefreshStepperContent";
        //public string RefreshStepperToolTip { get; set; } = "ViewName_RefreshStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RefreshStepperContent">RefreshStepper</system:String>
        //    <system:String x:Key="ViewName_RefreshStepperContentToolTip">RefreshStepper ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE and fix above
        //public void RefreshStepper(TYPE value)
        public async void RefreshStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(RefreshStepper) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called RefreshStepper";

            await Task.Run(() => RefreshStepperUIProperties());
            //RefreshStepperUIProperties();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RefreshStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RefreshStepperEvent>().Publish(
            //      new RefreshStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class RefreshStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RefreshStepperEvent>().Subscribe(RefreshStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(RefreshStepper) Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool RefreshStepperCanExecute(TYPE value)
        public bool RefreshStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region CloseStepper Command

        public DelegateCommand CloseStepperCommand { get; set; }
        public string CloseStepperContent { get; set; } = "Close";
        public string CloseStepperToolTip { get; set; } = "CloseStepper ToolTip";

        // Can get fancy and use Resources
        //public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        //public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        //    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

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

        public async void CloseStepper()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseStepper) Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseStepper";

            await Task.Run(() => ActiveStepper.Close());

            DeviceAttached = false;
            UpdateStepperProperties();
            ActiveStepper = null;
            ClearDigitalInputsAndOutputs();

            OpenStepperCommand.RaiseCanExecuteChanged();
            RefreshStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();

            InitializeAccelerationCommand.RaiseCanExecuteChanged();
            InitializeVelocityCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseStepperEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseStepperEvent>().Publish(
            //      new CloseStepperEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseStepperEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseStepperEvent>().Subscribe(CloseStepper);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseStepper) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool CloseStepperCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //return true;
            if (DeviceAttached is not null)
                return (Boolean)DeviceAttached;
            else
                return false;
        }

        #endregion

        #region Rotate Command

        // Start Cut Three - Put this in Fields and Properties

        public DelegateCommand<string> RotateCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> RotateCommand { get; set; }

        // End Cut Three

        // If displaying UserControl
        // public static WindowHost _RotateHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE RotateCommandParameter;

        public string RotateContent { get; set; } = "Rotate";
        public string RotateToolTip { get; set; } = "Rotate ToolTip";

        // Can get fancy and use Resources
        //public string RotateContent { get; set; } = "ViewName_RotateContent";
        //public string RotateToolTip { get; set; } = "ViewName_RotateContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RotateContent">Rotate</system:String>
        //    <system:String x:Key="ViewName_RotateContentToolTip">Rotate ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        //public void Rotate(TYPE value)
        public void Rotate(string direction)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called Rotate";

            PublishStatusMessage(Message);

            var sa = StepperProperties[0].StepAngle;

            Double circle = 360;
            var circleSteps = circle / sa;

            Int64 stepsToMove = (Int64)(Degrees / sa);

            stepsToMove = stepsToMove * 16; // 1/16 steps

            switch (direction)
            {
                case "CW":
                    StepperProperties[0].TargetPosition += stepsToMove;
                    break;

                case "CCW":
                    StepperProperties[0].TargetPosition -= stepsToMove;
                    break;

                default:
                    Log.Error($"Unexpected direction:>{direction}", Common.LOG_CATEGORY);
                    break;
            }

            // If launching a UserControl

            // if (_RotateHost is null) _RotateHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RotateEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RotateEvent>().Publish(
            //      new RotateEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class RotateEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RotateEvent>().Subscribe(Rotate);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool RotateCanExecute(TYPE value)
        public bool RotateCanExecute(string direction)
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region ZeroCurentPosition Command

        // Start Cut Three - Put this in Fields and Properties

        public DelegateCommand ZeroCurentPositionCommand { get; set; }
        // If using CommandParameter, figure out TYPE here and above
        // and remove above declaration
        //public DelegateCommand<TYPE> ZeroCurentPositionCommand { get; set; }

        // End Cut Three

        // End Cut Two

        // If displaying UserControl
        // public static WindowHost _ZeroCurentPositionHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE ZeroCurentPositionCommandParameter;

        public string ZeroCurentPositionContent { get; set; } = "ZeroCurentPosition";
        public string ZeroCurentPositionToolTip { get; set; } = "ZeroCurentPosition ToolTip";

        // Can get fancy and use Resources
        //public string ZeroCurentPositionContent { get; set; } = "ViewName_ZeroCurentPositionContent";
        //public string ZeroCurentPositionToolTip { get; set; } = "ViewName_ZeroCurentPositionContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_ZeroCurentPositionContent">ZeroCurentPosition</system:String>
        //    <system:String x:Key="ViewName_ZeroCurentPositionContentToolTip">ZeroCurentPosition ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        //public void ZeroCurentPosition(TYPE value)
        public void ZeroCurentPosition()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            Message = "Cool, you called ZeroCurentPosition";

            PublishStatusMessage(Message);

            StepperProperties[0].CurrentPosition = 0;
            StepperProperties[0].TargetPosition = 0;

            // If launching a UserControl

            // if (_ZeroCurentPositionHost is null) _ZeroCurentPositionHost = new WindowHost();
            // var userControl = new USERCONTROL();

            // _loggingConfigurationHost.DisplayUserControlInHost(
            //     "TITLE GOES HERE",
            //     //Common.DEFAULT_WINDOW_WIDTH,
            //     //Common.DEFAULT_WINDOW_HEIGHT,
            //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
            //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
            //     ShowWindowMode.Modeless_Show,
            //     userControl);

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<ZeroCurentPositionEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<ZeroCurentPositionEvent>().Publish(
            //      new ZeroCurentPositionEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class ZeroCurentPositionEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<ZeroCurentPositionEvent>().Subscribe(ZeroCurentPosition);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public bool ZeroCurentPositionCanExecute(TYPE value)
        public bool ZeroCurentPositionCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods

        private void UpdateStepperProperties()
        {
            Int64 startTicks = Log.Trace($"Enter deviceAttached:{DeviceAttached}", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // May not need this anymore.  Consider moving into ActiveStepper_{Attach,Detach}

            if ((Boolean)DeviceAttached)
            {
                StepperStepperCollection steppers = ActiveStepper.Stepper.steppers;
                StepperStepper stepper = null;

                StepperDigitalInputCollection inputs = ActiveStepper.Stepper.inputs;

                for (int i = 0; i < steppers.Count; i++)
                {
                    stepper = steppers[i];

                    ViewModels.StepperProperties stepperProperties = StepperProperties[i];

                    stepperProperties.LogPhidgetEvents = LogPhidgetEvents;

                    try
                    {
                        stepperProperties.Engaged = stepper.Engaged;

                        stepperProperties.Stopped = stepper.Stopped;
                        stepperProperties.Engaged = stepper.Engaged;
                        stepperProperties.Current = stepper.Current;

                        stepperProperties.AccelerationMax = stepper.AccelerationMax;
                        //stepperProperties.Acceleration = stepper.Acceleration; // Reading throws exception
                        stepperProperties.Acceleration = stepper.AccelerationMin;
                        stepperProperties.AccelerationMin = stepper.AccelerationMin;

                        stepperProperties.VelocityMin = stepper.VelocityMin;
                        stepperProperties.Velocity = stepper.Velocity;
                        //stepperProperties.VelocityLimit = stepper.VelocityLimit; // Reading throws exception
                        stepperProperties.VelocityLimit = stepper.VelocityMin + 360;    // Pretty slow
                        stepperProperties.VelocityMax = stepper.VelocityMax;

                        stepperProperties.PositionMin = stepper.PositionMin;
                        stepperProperties.CurrentPosition = stepper.CurrentPosition;
                        stepperProperties.TargetPosition = stepper.TargetPosition;
                        stepperProperties.PositionMax = stepper.PositionMax;
                    }
                    catch (PhidgetException pex)
                    {
                        // NOTE(crhodes)
                        // If the stepper is not engaged all properties throw excptions
                        InitializeStepperUI();
                    }

                    //StepperProperties[i].ServoType = servos[i].Type;

                    //stepper = steppers[i];

                    //switch (i)
                    //{
                    //    case 0:
                    //        Stopped_S0 = stepper.Stopped;
                    //        Engaged_S0 = stepper.Engaged;

                    //        CurrentMax_S0 = stepper.CurrentMax;
                    //        Current_S0 = stepper.Current;
                    //        //CurrentLimit_S0 = stepper.CurrentLimit; // Thows exception
                    //        CurrentMin_S0 = stepper.CurrentMin;


                    //        AccelerationMax_S0 = stepper.AccelerationMax;
                    //        //Acceleration_S0 = stepper.Acceleration; // Throws exception
                    //        AccelerationMin_S0 = stepper.AccelerationMin;


                    //        VelocityMax_S0 = stepper.VelocityMax;
                    //        Velocity_S0 = stepper.Velocity;
                    //        //VelocityLimit_S0 = stepper.VelocityLimit; // Throws exception
                    //        VelocityMin_S0 = stepper.VelocityMin;


                    //        PositionMax_S0 = stepper.PositionMax;
                    //        CurrentPosition_S0 = stepper.CurrentPosition;
                    //        TargetPosition_S0 = stepper.TargetPosition;
                    //        PositionMin_S0 = stepper.PositionMin;


                    //        break;

                    //    //case 1:
                    //    //    break;

                    //    //case 2:
                    //    //    break;

                    //    //case 3:
                    //    //    break;

                    //    //case 4:
                    //    //    break;

                    //    //case 5:
                    //    //    break;

                    //    //case 6:
                    //    //    break;

                    //    //case 7:
                    //    //    break;

                    //    default:
                    //        Log.Trace($"UpdateStepperProperties count:{steppers.Count}", Common.LOG_CATEGORY);
                    //        break;

                    //}
                }

                //StepperAddress = ActiveStepper.Address;
                //StepperAttached = ActiveStepper.Attached;
                //StepperAttachedToServer = ActiveStepper.AttachedToServer;
                //StepperClass = ActiveStepper.Class.ToString();
                //StepperID = Enum.GetName(typeof(Phidget.PhidgetID), ActiveStepper.ID);
                //StepperLabel = ActiveStepper.Label;
                //StepperLibraryVersion = Phidget.LibraryVersion;  // This is a static field
                //StepperName = ActiveStepper.Name;
                //StepperPort = ActiveStepper.Port;
                //StepperSerialNumber = ActiveStepper.SerialNumber; // This throws exception
                ////SServerID = ActiveStepper.ServerID;
                //StepperType = ActiveStepper.Type;
                //StepperVersion = ActiveStepper.Version;

                //var sensors = ActiveStepper.sensors;
                //StepperAnalogSensor sensor = null;

                //// NOTE(crhodes)
                //// The DataRateMin and DataRateMax do not change.
                //// Populate them here instead of SensorChange event

                //// TODO(crhodes)
                //// May want to grab initial values for all fields here.

                //for (int i = 0; i < sensors.Count; i++)
                //{
                //    sensor = sensors[i];

                //    switch (i)
                //    {
                //        case 0:
                //            AIDataRateMax0 = sensor.DataRateMax;
                //            AIDataRateMin0 = sensor.DataRateMin;
                //            break;
                //        case 1:
                //            AIDataRateMax1 = sensor.DataRateMax;
                //            AIDataRateMin1 = sensor.DataRateMin;
                //            break;
                //        case 2:
                //            AIDataRateMax2 = sensor.DataRateMax;
                //            AIDataRateMin2 = sensor.DataRateMin;
                //            break;
                //        case 3:
                //            AIDataRateMax3 = sensor.DataRateMax;
                //            AIDataRateMin3 = sensor.DataRateMin;
                //            break;
                //        case 4:
                //            AIDataRateMax4 = sensor.DataRateMax;
                //            AIDataRateMin4 = sensor.DataRateMin;
                //            break;
                //        case 5:
                //            AIDataRateMax5 = sensor.DataRateMax;
                //            AIDataRateMin5 = sensor.DataRateMin;
                //            break;
                //        case 6:
                //            AIDataRateMax6 = sensor.DataRateMax;
                //            AIDataRateMin6 = sensor.DataRateMin;
                //            break;
                //        case 7:
                //            AIDataRateMax7 = sensor.DataRateMax;
                //            AIDataRateMin7 = sensor.DataRateMin;
                //            break;
                //    }
                //}
            }
            else
            {
                DeviceAttached = null;
                InitializeStepperUI();
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();
        }

        private void RefreshStepperUIProperties()
        {
            Int64 startTicks = Log.Trace($"Enter deviceAttached:{DeviceAttached}", Common.LOG_CATEGORY);

            if ((Boolean)DeviceAttached)
            {
                //StepperServoCollection servos = ActiveStepper.Stepper.servos;
                //Phidget22.StepperServo servo = null;

                //ServoCount = servos.Count;

                //try
                //{
                //    for (int i = 0; i < ServoCount; i++)
                //    {
                //        StepperProperties[i].RefreshPropertiesFromServo();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Log.Error(ex, Common.LOG_CATEGORY);
                //}
            }
            else
            {
                DeviceAttached = null;
                InitializeStepperUI();
            }

            Log.Trace("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeStepperUI()
        {
            //for (int i = 0; i < 8; i++)
            //{
            //    StepperProperties[i].InitializePropertiesToNull();
            //}
        }

        private StepperProperties.MotionScale ConvertStringToInitializeMotion(string speed)
        {
            StepperProperties.MotionScale result = ViewModels.StepperProperties.MotionScale.Percent05;

            switch (speed)
            {
                case "Min":
                    result = ViewModels.StepperProperties.MotionScale.Min;
                    break;

                case "05%":
                    result = ViewModels.StepperProperties.MotionScale.Percent05;
                    break;

                case "10%":
                    result = ViewModels.StepperProperties.MotionScale.Percent10;
                    break;

                case "15%":
                    result = ViewModels.StepperProperties.MotionScale.Percent15;
                    break;

                case "20%":
                    result = ViewModels.StepperProperties.MotionScale.Percent20;
                    break;

                case "25%":
                    result = ViewModels.StepperProperties.MotionScale.Percent25;
                    break;

                case "35%":
                    result = ViewModels.StepperProperties.MotionScale.Percent35;
                    break;

                case "50%":
                    result = ViewModels.StepperProperties.MotionScale.Percent50;
                    break;

                case "75%":
                    result = ViewModels.StepperProperties.MotionScale.Percent75;
                    break;

                case "Max":
                    result = ViewModels.StepperProperties.MotionScale.Max;
                    break;

                default:
                    Log.Error($"Unexpected speed:{speed}", Common.LOG_CATEGORY);
                    break;
            }

            return result;
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
