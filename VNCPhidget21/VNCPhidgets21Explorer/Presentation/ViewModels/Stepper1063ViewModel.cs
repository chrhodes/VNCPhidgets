﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

using Phidgets;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget;

using VNCPhidgetConfig = VNCPhidget21.Configuration;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class Stepper1063ViewModel : EventViewModelBase, IStepper1063ViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public Stepper1063ViewModel(
            IEventAggregator eventAggregator,
            IDialogService dialogService) : base(eventAggregator, dialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // Turn off logging of PropertyChanged from VNC.Core
            // We display the logging in 
            //LogOnPropertyChanged = false;

            // TODO(crhodes)
            //

            ConfigFileName_DoubleClick_Command = new DelegateCommand(ConfigFileName_DoubleClick);
            OpenStepperCommand = new DelegateCommand(OpenStepper, OpenStepperCanExecute);
            CloseStepperCommand = new DelegateCommand(CloseStepper, CloseStepperCanExecute);

            // TODO(crhodes)
            // For now just hard code this.  Can have UI let us choose later.

            HostConfigFileName = "hostconfig.json";
            LoadUIConfig();

            //SayHelloCommand = new DelegateCommand(
            //    SayHello, SayHelloCanExecute);
                
            Message = "Stepper1063ViewModel says hello";           

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            string jsonString = File.ReadAllText(HostConfigFileName);

            VNCPhidgetConfig.HostConfig? hostConfig = 
                JsonSerializer.Deserialize<VNCPhidgetConfig.HostConfig>
                (jsonString, GetJsonSerializerOptions());

            Hosts = hostConfig.Hosts.ToList();
            //this.Sensors2 = phidgetConfig.Sensors.ToList();

            Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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

        //private VNCPhidgetConfig.HostConfig _hostConfig;
        //public VNCPhidgetConfig.HostConfig HostConfig
        //{
        //    get => _hostConfig;
        //    set
        //    {
        //        if (_hostConfig == value)
        //            return;
        //        _hostConfig = value;
        //        OnPropertyChanged();
        //    }
        //}

        private IEnumerable<VNCPhidgetConfig.Host> _Hosts;
        public IEnumerable<VNCPhidgetConfig.Host> Hosts
        {
            get
            {
                if (null == _Hosts)
                {
                    // TODO(crhodes)
                    // Load this like the sensors.xml for now

                    //_Hosts =
                    //    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Elements("Host")
                    //    select new Host(
                    //        item.Attribute("Name").Value,
                    //        item.Attribute("IPAddress").Value,
                    //        item.Attribute("Port").Value,
                    //        bool.Parse(item.Attribute("Enable").Value)
                    //        );
                }

                return _Hosts;
            }

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
                Steppers = _selectedHost.Steppers.ToList<VNCPhidgetConfig.Stepper>();
                OnPropertyChanged();
            }
        }

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
                    // Will need to declare Phidgets.Phidget?
                    PhidgetDevice = null;
                }

                OnPropertyChanged();
            }
        }

        private Phidgets.Phidget _phidgetDevice;
        public Phidgets.Phidget PhidgetDevice
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

        public ICommand SayHelloCommand { get; private set; }

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

        #region Stepper Properties


         private Double _currentMaxS0;
        public Double CurrentMax_S0
        {
            get => _currentMaxS0;
            set
            {
                if (_currentMaxS0 == value)
                    return;
                _currentMaxS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _currentS0;
        public Double Current_S0
        {
            get => _currentS0;
            set
            {
                if (_currentS0 == value)
                    return;
                _currentS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _currentLimitS0;
        public Double CurrentLimit_S0
        {
            get => _currentLimitS0;
            set
            {
                if (_currentLimitS0 == value)
                    return;
                _currentLimitS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveStepper.Stepper.steppers[0].CurrentLimit != value)
                    {
                        ActiveStepper.Stepper.steppers[0].CurrentLimit = value;
                    }
                }
                catch (Exception ex)
                {
                    ActiveStepper.Stepper.steppers[0].CurrentLimit = value;
                }
            }
        }

        private Double _currentMinS0;
        public Double CurrentMin_S0
        {
            get => _currentMinS0;
            set
            {
                if (_currentMinS0 == value)
                    return;
                _currentMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _positionMax_S0;
        public Double PositionMax_S0
        {
            get => _positionMax_S0;
            set
            {
                if (_positionMax_S0 == value)
                    return;
                _positionMax_S0 = value;
                OnPropertyChanged();
            }
        }


        private Int64 _currentPositionS0;
        public Int64 CurrentPosition_S0
        {
            get => _currentPositionS0;
            set
            {
                if (_currentPositionS0 == value)
                    return;
                _currentPositionS0 = value;
                OnPropertyChanged();

                if (ActiveStepper.Stepper.steppers[0].CurrentPosition != value)
                {
                    ActiveStepper.Stepper.steppers[0].CurrentPosition = value;
                }
            }
        }

        private Double _positionMin_S0;
        public Double PositionMin_S0
        {
            get => _positionMin_S0;
            set
            {
                if (_positionMin_S0 == value)
                    return;
                _positionMin_S0 = value;

                OnPropertyChanged();
            }
        }

        private Int64 _targetPositionS0;
        public Int64 TargetPosition_S0
        {
            get => _targetPositionS0;
            set
            {
                if (_targetPositionS0 == value)
                    return;
                _targetPositionS0 = value;
                OnPropertyChanged();

                if (ActiveStepper.Stepper.steppers[0].TargetPosition != value)
                {
                    ActiveStepper.Stepper.steppers[0].TargetPosition = value;
                }
            }
        }



        private Double _velocityMinS0;
        public Double VelocityMin_S0
        {
            get => _velocityMinS0;
            set
            {
                if (_velocityMinS0 == value)
                    return;
                _velocityMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityS0;
        public Double Velocity_S0
        {
            get => _velocityS0;
            set
            {
                if (_velocityS0 == value)
                    return;
                _velocityS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _velocityLimitS0;
        public Double VelocityLimit_S0
        {
            get => _velocityLimitS0;
            set
            {
                if (_velocityLimitS0 == value)
                    return;
                _velocityLimitS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveStepper.Stepper.steppers[0].VelocityLimit != value)
                    {
                        ActiveStepper.Stepper.steppers[0].VelocityLimit = value;
                    }
                }
                catch (Exception ex)
                {
                    ActiveStepper.Stepper.steppers[0].VelocityLimit = value;
                }
            }
        }

        private Double _velocityMaxS0;
        public Double VelocityMax_S0
        {
            get => _velocityMaxS0;
            set
            {
                if (_velocityMaxS0 == value)
                    return;
                _velocityMaxS0 = value;
                OnPropertyChanged();
            }
        }



        private Double _accelerationMinS0;
        public Double AccelerationMin_S0
        {
            get => _accelerationMinS0;
            set
            {
                if (_accelerationMinS0 == value)
                    return;
                _accelerationMinS0 = value;
                OnPropertyChanged();
            }
        }

        private Double _accelerationS0;
        public Double Acceleration_S0
        {
            get => _accelerationS0;
            set
            {
                if (_accelerationS0 == value)
                    return;
                _accelerationS0 = value;
                OnPropertyChanged();

                try
                {
                    if (ActiveStepper.Stepper.steppers[0].Acceleration != value)
                    {
                        ActiveStepper.Stepper.steppers[0].Acceleration = value;
                    }
                }
                catch (Exception ex)
                {
                    // NOTE(crhodes)
                    // This throws exception  Humm
                    //ActiveStepper.Stepper.steppers[0].Acceleration = value;
                    ActiveStepper.Stepper.steppers[0].Acceleration = AccelerationMax_S0;
                }
            }
        }

        private Double _accelerationMaxS0;
        public Double AccelerationMax_S0
        {
            get => _accelerationMaxS0;
            set
            {
                if (_accelerationMaxS0 == value)
                    return;
                _accelerationMaxS0 = value;
                OnPropertyChanged();
            }
        }






        private bool? _engagedS0;
        public bool? Engaged_S0
        {
            get => _engagedS0;
            set
            {
                if (_engagedS0 == value)
                    return;
                _engagedS0 = value;

                ActiveStepper.Stepper.steppers[0].Engaged = (Boolean)value;
                OnPropertyChanged();
            }
        }


        private bool? _speedRampingS0;
        public bool? SpeedRamping_S0
        {
            get => _speedRampingS0;
            set
            {
                if (_speedRampingS0 == value)
                    return;
                _speedRampingS0 = value;
                OnPropertyChanged();
            }
        }

        private bool? _stoppedS0;
        public bool? Stopped_S0
        {
            get => _stoppedS0;
            set
            {
                if (_stoppedS0 == value)
                    return;
                _stoppedS0 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        #region Command ConfigFileName DoubleClick

        public DelegateCommand ConfigFileName_DoubleClick_Command { get; set; }

        public void ConfigFileName_DoubleClick()
        {
            Message = "ConfigFileName_DoubleClick";
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

        public void OpenStepper()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
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

            ActiveStepper.Open();

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

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void ActiveStepper_VelocityChange(object sender, Phidgets.Events.VelocityChangeEventArgs e)
        {
            Phidgets.Stepper stepper = sender as Phidgets.Stepper;
            var index = e.Index;
            var velocity = e.Velocity;

            switch (e.Index)
            {
                case 0:
                    Velocity_S0 = e.Velocity;
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
                    Log.Trace($"VelocityChange index:{index} value:{velocity}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveStepper_PositionChange(object sender, Phidgets.Events.StepperPositionChangeEventArgs e)
        {
            Phidgets.Stepper stepper = sender as Phidgets.Stepper;
            var index = e.Index;
            var position = e.Position;

            switch (e.Index)
            {
                case 0:
                    CurrentPosition_S0 = e.Position;
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
                    Log.Trace($"PositionChange index:{index} value:{position}", Common.LOG_CATEGORY);
                    break;
            }
        }

        private void ActiveStepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            Phidgets.Stepper stepper = sender as Phidgets.Stepper;
            var index = e.Index;
            var value = e.Value;

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

        private void ActiveStepper_CurrentChange(object sender, Phidgets.Events.CurrentChangeEventArgs e)
        {
            Phidgets.Stepper stepper = sender as Phidgets.Stepper;
            var index = e.Index;
            var current = e.Current;

            switch (e.Index)
            {
                case 0:
                    Current_S0 = e.Current;
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
                    Log.Trace($"CurrentChange index:{index} value:{current}", Common.LOG_CATEGORY);
                    break;
            }
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

        public void CloseStepper()
        {
            Int64 startTicks = Log.EVENT("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseStepper";

            ActiveStepper.Close();
            UpdateStepperProperties();
            ActiveStepper = null;
            ClearDigitalInputsAndOutputs();

            //OpenStepperCommand.RaiseCanExecuteChanged();
            //CloseStepperCommand.RaiseCanExecuteChanged();

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

            Log.EVENT("Exit", Common.LOG_CATEGORY, startTicks);
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

        #endregion

        #region Event Handlers

        //private void PopulateSensorValues(StepperAnalogSensor interfaceKitAnalogSensor)
        //{

        //}

        //private void ActiveStepper_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;

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

        //private void ActiveStepper_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;

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


        //private void ActiveStepper_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        //{
        //    Phidgets.Stepper ifk = (Phidgets.Stepper)sender;
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

        private void ActiveStepper_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveStepper_Attach {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                // TODO(crhodes)
                // This is where properties should be grabbed
                UpdateStepperProperties();

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void UpdateStepperProperties()
        {
            // TODO(crhodes)
            // May not need this anymore.  Consider moving into ActiveStepper_{Attach,Detach}

            if (ActiveStepper.Stepper.Attached)
            {
                DeviceAttached = ActiveStepper.Stepper.Attached;

                StepperStepperCollection steppers = ActiveStepper.Stepper.steppers;

                StepperStepper stepper = null;

                StepperDigitalInputCollection inputs = ActiveStepper.Stepper.inputs;

                for (int i = 0; i < steppers.Count; i++)
                {
                    stepper = steppers[i];

                    switch (i)
                    {
                        case 0:
                            Stopped_S0 = stepper.Stopped;
                            Engaged_S0 = stepper.Engaged;
               

                            CurrentMax_S0 = stepper.CurrentMax;
                            Current_S0 = stepper.Current;
                            //CurrentLimit_S0 = stepper.CurrentLimit; // Thows exception
                            CurrentMin_S0 = stepper.CurrentMin;

 
                            AccelerationMax_S0 = stepper.AccelerationMax;
                            //Acceleration_S0 = stepper.Acceleration; // Throws exception
                            AccelerationMin_S0 = stepper.AccelerationMin;


                            VelocityMax_S0 = stepper.VelocityMax;
                            Velocity_S0 = stepper.Velocity;
                            //VelocityLimit_S0 = stepper.VelocityLimit; // Throws exception
                            VelocityMin_S0 = stepper.VelocityMin;


                            PositionMax_S0 = stepper.PositionMax;
                            CurrentPosition_S0 = stepper.CurrentPosition;
                            TargetPosition_S0 = stepper.TargetPosition;
                            PositionMin_S0 = stepper.PositionMin;



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

                        //case 6:
                        //    break;

                        //case 7:
                        //    break;

                        default:
                            Log.Trace($"UpdateStepperProperties count:{steppers.Count}", Common.LOG_CATEGORY);
                            break;

                    }
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
            }

            OpenStepperCommand.RaiseCanExecuteChanged();
            CloseStepperCommand.RaiseCanExecuteChanged();
        }

        private void ActiveStepper_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                Phidgets.Phidget device = (Phidgets.Phidget)sender;
                Log.Trace($"ActiveStepper_Detach {device.Address},{device.SerialNumber}", Common.LOG_CATEGORY);

                // TODO(crhodes)
                // What kind of cleanup?  Maybe set ActiveStepper to null.  Clear UI
                UpdateStepperProperties();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods

        #region SayHello Command

        private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = $"Hello from {this.GetType()}";

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }
        
        private bool SayHelloCanExecute()
        {
            return true;
        }
        
        #endregion
        
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
