using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using Phidgets = Phidget22;
using PhidgetsEvents = Phidget22.Events;

using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;
using System.Threading.Channels;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.CodeParser;

namespace VNCPhidget22Explorer.Presentation.ViewModels
{
    public class VINTHubViewModel
        : EventViewModelBase, IVINTHubViewModel, IInstanceCountVM
    {
        #region Constructors, Initialization, and Load

        public VINTHubViewModel(
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

            // NOTE(crhodes)
            // Can only open a HubPort in one mode at a time
            // Bulk Open operations do not make sense

            //OpenVINTHubCommand = new DelegateCommand(OpenVINTHub, OpenVINTHubCanExecute);

            // Bulk close is convenience

            CloseVINTHubCommand = new DelegateCommand(CloseVINTHub, CloseVINTHubCanExecute);

            OpenDigitalInputCommand = new DelegateCommand<SerialHubPortChannel?>(OpenDigitalInput, OpenDigitalInputCanExecute);
            CloseDigitalInputCommand = new DelegateCommand<SerialHubPortChannel?>(CloseDigitalInput, CloseDigitalInputCanExecute);

            OpenDigitalOutputCommand = new DelegateCommand<SerialHubPortChannel?>(OpenDigitalOutput, OpenDigitalOutputCanExecute);
            CloseDigitalOutputCommand = new DelegateCommand<SerialHubPortChannel?>(CloseDigitalOutput, CloseDigitalOutputCanExecute);

            //OpenRCServoCommand = new DelegateCommand<SerialHubPortChannel?>(OpenRCServo, OpenRCServoCanExecute);
            //CloseRCServoCommand = new DelegateCommand<SerialHubPortChannel?>(CloseRCServo, CloseRCServoCanExecute);
            //SetPositionRangeCommand = new DelegateCommand<string>(SetPositionRange, SetPositionRangeCanExecute);

            //OpenStepperCommand = new DelegateCommand<SerialHubPortChannel?>(OpenStepper, OpenStepperCanExecute);
            //CloseStepperCommand = new DelegateCommand<SerialHubPortChannel?>(CloseStepper, CloseStepperCanExecute);
            //ZeroCurrentPositionCommand = new DelegateCommand<string>(ZeroCurrentPosition, ZeroCurrentPositionCanExecute);

            OpenVoltageInputCommand = new DelegateCommand<SerialHubPortChannel?>(OpenVoltageInput, OpenVoltageInputCanExecute);
            RefreshVoltageInputCommand = new DelegateCommand<SerialHubPortChannel?>(RefreshVoltageInput, RefreshVoltageInputCanExecute);
            CloseVoltageInputCommand = new DelegateCommand<SerialHubPortChannel?>(CloseVoltageInput, CloseVoltageInputCanExecute);

            OpenVoltageRatioInputCommand = new DelegateCommand<SerialHubPortChannel?>(OpenVoltageRatioInput, OpenVoltageRatioInputCanExecute);
            RefreshVoltageRatioInputCommand = new DelegateCommand<SerialHubPortChannel?>(RefreshVoltageRatioInput, RefreshVoltageRatioInputCanExecute);
            CloseVoltageRatioInputCommand = new DelegateCommand<SerialHubPortChannel?>(CloseVoltageRatioInput, CloseVoltageRatioInputCanExecute);

            RaisePerformanceEventCommand = new DelegateCommand<SerialHubPortChannel?>(RaisePerformanceEvent, RaisePerformanceEventCanExecute);

            LoadUIConfig();

            Message = "VINTHubViewModel says hello";

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadUIConfig()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);

            VINTHubPhidgets = Common.PhidgetDeviceLibrary.ManagerAttachedPhidgetDevices
                .Where(x => x.DeviceClass == "Hub")
                .DistinctBy(x => x.DeviceSerialNumber)
                .Select(x => x.DeviceSerialNumber);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void LoadPhidgets()
        {
            // NOTE(crhodes)
            // Load everything that can be used from VINTHub that has been detected

            var digitalInputs = Common.PhidgetDeviceLibrary.DigitalInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            LoadDigitalInputs(digitalInputs);

            var digitalOutputs = Common.PhidgetDeviceLibrary.DigitalOutputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            LoadDigitalOutputs(digitalOutputs);

            var voltageInputs = Common.PhidgetDeviceLibrary.VoltageInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            //var rcServos = Common.PhidgetDeviceLibrary.RCServoChannels
            //    .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            //LoadRCServos(rcServos);

            //var steppers = Common.PhidgetDeviceLibrary.StepperChannels
            //    .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            //LoadSteppers(steppers);

            LoadVoltageInputs(voltageInputs);

            var voltageRatioInputs = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            LoadVoltageRatioInputs(voltageRatioInputs);
        }

        void LoadDigitalInputs(IEnumerable<KeyValuePair<SerialHubPortChannel, DigitalInputEx>> digitalInputs)
        {
            foreach (var digitalInput in digitalInputs)
            {
                // NOTE(crhodes)
                // This is a bit tricky.  For VINT that support DI/DO/VI we use the Key.HubPort

                switch (digitalInput.Key.HubPort)
                {
                    // TODO(crhodes)
                    // Don't think we need 16. Just the maximum number of HubPorts.  Think it is 6.

                    case 0:
                        DigitalInput0 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 1:
                        DigitalInput1 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 2:
                        DigitalInput2 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 3:
                        DigitalInput3 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 4:
                        DigitalInput4 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 5:
                        DigitalInput5 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 6:
                        DigitalInput6 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 7:
                        DigitalInput7 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 8:
                        DigitalInput8 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 9:
                        DigitalInput9 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 10:
                        DigitalInput10 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 11:
                        DigitalInput11 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 12:
                        DigitalInput12 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 13:
                        DigitalInput13 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 14:
                        DigitalInput14 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;

                    case 15:
                        DigitalInput15 = Common.PhidgetDeviceLibrary.DigitalInputChannels[digitalInput.Key];
                        break;
                }
            }
        }

        void LoadDigitalOutputs(IEnumerable<KeyValuePair<SerialHubPortChannel, DigitalOutputEx>> digitalOutputs)
        {
            foreach (var digitalOutput in digitalOutputs)
            {
                // NOTE(crhodes)
                // This is a bit tricky.  For VINT that support DI/DO/VI we use the Key.HubPort
                switch (digitalOutput.Key.HubPort)
                {
                    // TODO(crhodes)
                    // Don't think we need 16. Just the maximum number of HubPorts.  Think it is 6.
                   
                    case 0:
                        DigitalOutput0 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 1:
                        DigitalOutput1 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 2:
                        DigitalOutput2 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 3:
                        DigitalOutput3 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 4:
                        DigitalOutput4 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 5:
                        DigitalOutput5 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 6:
                        DigitalOutput6 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 7:
                        DigitalOutput7 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 8:
                        DigitalOutput8 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 9:
                        DigitalOutput9 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 10:
                        DigitalOutput10 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 11:
                        DigitalOutput11 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 12:
                        DigitalOutput12 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 13:
                        DigitalOutput13 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 14:
                        DigitalOutput14 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;

                    case 15:
                        DigitalOutput15 = Common.PhidgetDeviceLibrary.DigitalOutputChannels[digitalOutput.Key];
                        break;
                }
            }
        }

        //void LoadRCServos(IEnumerable<KeyValuePair<SerialHubPortChannel, RCServoEx>> rcServos)
        //{
        //    foreach (var rcServo in rcServos)
        //    {
        //        // NOTE(crhodes)
        //        // This is a bit tricky.
        //        // For VINTDevice that is connected to a VINT Hub, we use the Channel
        //        // and support as many channels as the VINTDevice supports.

        //        switch (rcServo.Key.Channel)
        //        {
        //            //RCC1000 has 16 channels
        //            case 0:
        //                RCServo0 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 1:
        //                RCServo1 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 2:
        //                RCServo2 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 3:
        //                RCServo3 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 4:
        //                RCServo4 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 5:
        //                RCServo5 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 6:
        //                RCServo6 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 7:
        //                RCServo7 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 8:
        //                RCServo8 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 9:
        //                RCServo9 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 10:
        //                RCServo10 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 11:
        //                RCServo11 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 12:
        //                RCServo12 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 13:
        //                RCServo13 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 14:
        //                RCServo14 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;

        //            case 15:
        //                RCServo15 = Common.PhidgetDeviceLibrary.RCServoChannels[rcServo.Key];
        //                break;
        //        }
        //    }
        //}

        //void LoadSteppers(IEnumerable<KeyValuePair<SerialHubPortChannel, StepperEx>> steppers)
        //{
        //    foreach (var stepper in steppers)
        //    {
        //        // NOTE(crhodes)
        //        // This is a bit tricky.
        //        // For VINTDevice that is connected to a VINT Hub, we use the Channel
        //        // and support as many channels as the VINTDevice supports.

        //        switch (stepper.Key.Channel)
        //        {
        //            // STC1001 has one channel
        //            case 0:
        //                Stepper0 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 1:
        //                Stepper1 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 2:
        //                Stepper2 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 3:
        //                Stepper3 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 4:
        //                Stepper4 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 5:
        //                Stepper5 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 6:
        //                Stepper6 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 7:
        //                Stepper7 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 8:
        //                Stepper8 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 9:
        //                Stepper9 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 10:
        //                Stepper10 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 11:
        //                Stepper11 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 12:
        //                Stepper12 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 13:
        //                Stepper13 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 14:
        //                Stepper14 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;

        //            case 15:
        //                Stepper15 = Common.PhidgetDeviceLibrary.StepperChannels[stepper.Key];
        //                break;
        //        }
        //    }
        //}
        
        void LoadVoltageInputs(IEnumerable<KeyValuePair<SerialHubPortChannel, VoltageInputEx>> voltageInputs)
        {
            foreach (var voltageInput in voltageInputs)
            {
                // NOTE(crhodes)
                // This is a bit tricky.  For VINT that support DI/DO/VI we use the Key.HubPort

                switch (voltageInput.Key.HubPort)
                {
                    // TODO(crhodes)
                    // Don't think we need 16. Just the maximum number of HubPorts.  Think it is 6.

                    case 0:
                        VoltageInput0 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 1:
                        VoltageInput1 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 2:
                        VoltageInput2 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 3:
                        VoltageInput3 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 4:
                        VoltageInput4 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 5:
                        VoltageInput5 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 6:
                        VoltageInput6 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 7:
                        VoltageInput7 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 8:
                        VoltageInput8 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 9:
                        VoltageInput9 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 10:
                        VoltageInput10 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 11:
                        VoltageInput11 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 12:
                        VoltageInput12 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 13:
                        VoltageInput13 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 14:
                        VoltageInput14 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;

                    case 15:
                        VoltageInput15 = Common.PhidgetDeviceLibrary.VoltageInputChannels[voltageInput.Key];
                        break;
                }
            }
        }

        void LoadVoltageRatioInputs(IEnumerable<KeyValuePair<SerialHubPortChannel, VoltageRatioInputEx>> voltageRatioInputs)
        {
            foreach (var voltageRatioInput in voltageRatioInputs)
            {
                switch (voltageRatioInput.Key.HubPort)
                {
                    // TODO(crhodes)
                    // Don't think we need 16. Just the maximum number of HubPorts.  Think it is 6.

                    case 0:
                        VoltageRatioInput0 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 1:
                        VoltageRatioInput1 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 2:
                        VoltageRatioInput2 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 3:
                        VoltageRatioInput3 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 4:
                        VoltageRatioInput4 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 5:
                        VoltageRatioInput5 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 6:
                        VoltageRatioInput6 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 7:
                        VoltageRatioInput7 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 8:
                        VoltageRatioInput8 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 9:
                        VoltageRatioInput9 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 10:
                        VoltageRatioInput10 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 11:
                        VoltageRatioInput11 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 12:
                        VoltageRatioInput12 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 13:
                        VoltageRatioInput13 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 14:
                        VoltageRatioInput14 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;

                    case 15:
                        VoltageRatioInput15 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[voltageRatioInput.Key];
                        break;
                }
            }
        }

        #endregion

        #region Enums (none)



        #endregion

        #region Structures (none)



        #endregion

        #region Fields and Properties

        #region Logging

        private Boolean _logPhidgetEvents = true;
        public Boolean LogPhidgetEvents
        {
            get => _logPhidgetEvents;
            set
            {
                if (_logPhidgetEvents == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logErrorEvents = true;    // Probably always want to see errors
        public Boolean LogErrorEvents
        {
            get => _logErrorEvents;
            set
            {
                if (_logErrorEvents == value)
                    return;
                _logErrorEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logPropertyChangeEvents = false;
        public Boolean LogPropertyChangeEvents
        {
            get => _logPropertyChangeEvents;
            set
            {
                if (_logPropertyChangeEvents == value)
                    return;
                _logPropertyChangeEvents = value;
                OnPropertyChanged();
            }
        }

        #region I/O Events

        private Boolean _logStateChangeEvents = false;
        public Boolean LogStateChangeEvents
        {
            get => _logStateChangeEvents;
            set
            {
                if (_logStateChangeEvents == value)
                    return;
                _logStateChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logSensorChangeEvents = false;
        public Boolean LogSensorChangeEvents
        {
            get => _logSensorChangeEvents;
            set
            {
                if (_logSensorChangeEvents == value)
                    return;
                _logSensorChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logVoltageChangeEvents = false;
        public Boolean LogVoltageChangeEvents
        {
            get => _logVoltageChangeEvents;
            set
            {
                if (_logVoltageChangeEvents == value)
                    return;
                _logVoltageChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logVoltageRatioChangeEvents = false;
        public Boolean LogVoltageRatioChangeEvents
        {
            get => _logVoltageRatioChangeEvents;
            set
            {
                if (_logVoltageRatioChangeEvents == value)
                    return;
                _logVoltageRatioChangeEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region RCServo and Stepper Events

        private Boolean _logCurrentChangeEvents = false;
        public Boolean LogCurrentChangeEvents
        {
            get => _logCurrentChangeEvents;
            set
            {
                if (_logCurrentChangeEvents == value)
                    return;
                _logCurrentChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logPositionChangeEvents = false;
        public Boolean LogPositionChangeEvents
        {
            get => _logPositionChangeEvents;
            set
            {
                if (_logPositionChangeEvents == value)
                    return;
                _logPositionChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logVelocityChangeEvents = false;
        public Boolean LogVelocityChangeEvents
        {
            get => _logVelocityChangeEvents;
            set
            {
                if (_logVelocityChangeEvents == value)
                    return;
                _logVelocityChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logStoppeddEvents = false;
        public Boolean LogStoppedEvents
        {
            get => _logStoppeddEvents;
            set
            {
                if (_logStoppeddEvents == value)
                    return;
                _logStoppeddEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logTargetPositionReachedEvents = false;
        public Boolean LogTargetPositionReachedEvents
        {
            get => _logTargetPositionReachedEvents;
            set
            {
                if (_logTargetPositionReachedEvents == value)
                    return;
                _logTargetPositionReachedEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Performance Events

        private Boolean _logDeviceChannelSequence = false;
        public Boolean LogDeviceChannelSequence
        {
            get => _logDeviceChannelSequence;
            set
            {
                if (_logDeviceChannelSequence == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logChannelAction = false;
        public Boolean LogChannelAction
        {
            get => _logChannelAction;
            set
            {
                if (_logChannelAction == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        private Boolean _logActionVerification = false;
        public Boolean LogActionVerification
        {
            get => _logActionVerification;
            set
            {
                if (_logActionVerification == value)
                    return;
                _logPhidgetEvents = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region VINTHub

        private IEnumerable<Int32> _VINTHubPhidgets;
        public IEnumerable<Int32> VINTHubPhidgets
        {
            get
            {
                return _VINTHubPhidgets;
            }

            set
            {
                _VINTHubPhidgets = value;
                OnPropertyChanged();
            }
        }

        private Int32? _selectedVINTHubPhidget = null;
        public Int32? SelectedVINTHubPhidget
        {
            get => _selectedVINTHubPhidget;
            set
            {
                _selectedVINTHubPhidget = value;
                OnPropertyChanged();

                LoadPhidgets();

                //OpenVINTHubCommand.RaiseCanExecuteChanged();
                CloseVINTHubCommand.RaiseCanExecuteChanged();

                OpenDigitalInputCommand.RaiseCanExecuteChanged();
                OpenDigitalOutputCommand.RaiseCanExecuteChanged();

                //OpenRCServoCommand.RaiseCanExecuteChanged();
                //OpenStepperCommand.RaiseCanExecuteChanged();

                OpenVoltageInputCommand.RaiseCanExecuteChanged();
                OpenVoltageRatioInputCommand.RaiseCanExecuteChanged();

                DigitalInputsVisibility = Visibility.Visible;
                DigitalOutputsVisibility = Visibility.Visible;
                RCServosVisibility = Visibility.Visible;
                SteppersVisibility = Visibility.Visible;
                VoltageInputsVisibility = Visibility.Visible;
            }
        }

        #region DigitalInputs

        private Visibility _digitalInputsVisibility = Visibility.Collapsed;
        public Visibility DigitalInputsVisibility
        {
            get => _digitalInputsVisibility;
            set
            {
                if (_digitalInputsVisibility == value)
                    return;
                _digitalInputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput0;
        public DigitalInputEx DigitalInput0
        {
            get => _digitalInput0;
            set
            {
                if (_digitalInput0 == value)
                    return;
                _digitalInput0 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput1;
        public DigitalInputEx DigitalInput1
        {
            get => _digitalInput1;
            set
            {
                if (_digitalInput1 == value)
                    return;
                _digitalInput1 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput2;
        public DigitalInputEx DigitalInput2
        {
            get => _digitalInput2;
            set
            {
                if (_digitalInput2 == value)
                    return;
                _digitalInput2 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput3;
        public DigitalInputEx DigitalInput3
        {
            get => _digitalInput3;
            set
            {
                if (_digitalInput3 == value)
                    return;
                _digitalInput3 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput4;
        public DigitalInputEx DigitalInput4
        {
            get => _digitalInput4;
            set
            {
                if (_digitalInput4 == value)
                    return;
                _digitalInput4 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput5;
        public DigitalInputEx DigitalInput5
        {
            get => _digitalInput5;
            set
            {
                if (_digitalInput5 == value)
                    return;
                _digitalInput5 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput6;
        public DigitalInputEx DigitalInput6
        {
            get => _digitalInput6;
            set
            {
                if (_digitalInput6 == value)
                    return;
                _digitalInput6 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput7;
        public DigitalInputEx DigitalInput7
        {
            get => _digitalInput7;
            set
            {
                if (_digitalInput7 == value)
                    return;
                _digitalInput7 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput8;
        public DigitalInputEx DigitalInput8
        {
            get => _digitalInput8;
            set
            {
                if (_digitalInput8 == value)
                    return;
                _digitalInput8 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput9;
        public DigitalInputEx DigitalInput9
        {
            get => _digitalInput9;
            set
            {
                if (_digitalInput9 == value)
                    return;
                _digitalInput9 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput10;
        public DigitalInputEx DigitalInput10
        {
            get => _digitalInput10;
            set
            {
                if (_digitalInput10 == value)
                    return;
                _digitalInput10 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput11;
        public DigitalInputEx DigitalInput11
        {
            get => _digitalInput11;
            set
            {
                if (_digitalInput11 == value)
                    return;
                _digitalInput11 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput12;
        public DigitalInputEx DigitalInput12
        {
            get => _digitalInput12;
            set
            {
                if (_digitalInput12 == value)
                    return;
                _digitalInput12 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput13;
        public DigitalInputEx DigitalInput13
        {
            get => _digitalInput13;
            set
            {
                if (_digitalInput13 == value)
                    return;
                _digitalInput13 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput14;
        public DigitalInputEx DigitalInput14
        {
            get => _digitalInput14;
            set
            {
                if (_digitalInput14 == value)
                    return;
                _digitalInput14 = value;
                OnPropertyChanged();
            }
        }

        private DigitalInputEx _digitalInput15;
        public DigitalInputEx DigitalInput15
        {
            get => _digitalInput15;
            set
            {
                if (_digitalInput15 == value)
                    return;
                _digitalInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region DigitalOutputs

        private Visibility _digitalOutputsVisibility = Visibility.Collapsed;
        public Visibility DigitalOutputsVisibility
        {
            get => _digitalOutputsVisibility;
            set
            {
                if (_digitalOutputsVisibility == value)
                    return;
                _digitalOutputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput0;
        public DigitalOutputEx DigitalOutput0
        {
            get => _digitalOutput0;
            set
            {
                if (_digitalOutput0 == value)
                    return;
                _digitalOutput0 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput1;
        public DigitalOutputEx DigitalOutput1
        {
            get => _digitalOutput1;
            set
            {
                if (_digitalOutput1 == value)
                    return;
                _digitalOutput1 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput2;
        public DigitalOutputEx DigitalOutput2
        {
            get => _digitalOutput2;
            set
            {
                if (_digitalOutput2 == value)
                    return;
                _digitalOutput2 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput3;
        public DigitalOutputEx DigitalOutput3
        {
            get => _digitalOutput3;
            set
            {
                if (_digitalOutput3 == value)
                    return;
                _digitalOutput3 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput4;
        public DigitalOutputEx DigitalOutput4
        {
            get => _digitalOutput4;
            set
            {
                if (_digitalOutput4 == value)
                    return;
                _digitalOutput4 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput5;
        public DigitalOutputEx DigitalOutput5
        {
            get => _digitalOutput5;
            set
            {
                if (_digitalOutput5 == value)
                    return;
                _digitalOutput5 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput6;
        public DigitalOutputEx DigitalOutput6
        {
            get => _digitalOutput6;
            set
            {
                if (_digitalOutput6 == value)
                    return;
                _digitalOutput6 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput7;
        public DigitalOutputEx DigitalOutput7
        {
            get => _digitalOutput7;
            set
            {
                if (_digitalOutput7 == value)
                    return;
                _digitalOutput7 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput8;
        public DigitalOutputEx DigitalOutput8
        {
            get => _digitalOutput8;
            set
            {
                if (_digitalOutput8 == value)
                    return;
                _digitalOutput8 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput9;
        public DigitalOutputEx DigitalOutput9
        {
            get => _digitalOutput9;
            set
            {
                if (_digitalOutput9 == value)
                    return;
                _digitalOutput9 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput10;
        public DigitalOutputEx DigitalOutput10
        {
            get => _digitalOutput10;
            set
            {
                if (_digitalOutput10 == value)
                    return;
                _digitalOutput10 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput11;
        public DigitalOutputEx DigitalOutput11
        {
            get => _digitalOutput11;
            set
            {
                if (_digitalOutput11 == value)
                    return;
                _digitalOutput11 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput12;
        public DigitalOutputEx DigitalOutput12
        {
            get => _digitalOutput12;
            set
            {
                if (_digitalOutput12 == value)
                    return;
                _digitalOutput12 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput13;
        public DigitalOutputEx DigitalOutput13
        {
            get => _digitalOutput13;
            set
            {
                if (_digitalOutput13 == value)
                    return;
                _digitalOutput13 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput14;
        public DigitalOutputEx DigitalOutput14
        {
            get => _digitalOutput14;
            set
            {
                if (_digitalOutput14 == value)
                    return;
                _digitalOutput14 = value;
                OnPropertyChanged();
            }
        }

        private DigitalOutputEx _digitalOutput15;
        public DigitalOutputEx DigitalOutput15
        {
            get => _digitalOutput15;
            set
            {
                if (_digitalOutput15 == value)
                    return;
                _digitalOutput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region RCServos

        private Visibility _rcServosVisibility = Visibility.Collapsed;
        public Visibility RCServosVisibility
        {
            get => _rcServosVisibility;
            set
            {
                if (_rcServosVisibility == value)
                    return;
                _rcServosVisibility = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo0;
        public RCServoEx RCServo0
        {
            get => _rcServo0;
            set
            {
                if (_rcServo0 == value)
                    return;
                _rcServo0 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo1;
        public RCServoEx RCServo1
        {
            get => _rcServo1;
            set
            {
                if (_rcServo1 == value)
                    return;
                _rcServo1 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo2;
        public RCServoEx RCServo2
        {
            get => _rcServo2;
            set
            {
                if (_rcServo2 == value)
                    return;
                _rcServo2 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo3;
        public RCServoEx RCServo3
        {
            get => _rcServo3;
            set
            {
                if (_rcServo3 == value)
                    return;
                _rcServo3 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo4;
        public RCServoEx RCServo4
        {
            get => _rcServo4;
            set
            {
                if (_rcServo4 == value)
                    return;
                _rcServo4 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo5;
        public RCServoEx RCServo5
        {
            get => _rcServo5;
            set
            {
                if (_rcServo5 == value)
                    return;
                _rcServo5 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo6;
        public RCServoEx RCServo6
        {
            get => _rcServo6;
            set
            {
                if (_rcServo6 == value)
                    return;
                _rcServo6 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo7;
        public RCServoEx RCServo7
        {
            get => _rcServo7;
            set
            {
                if (_rcServo7 == value)
                    return;
                _rcServo7 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo8;
        public RCServoEx RCServo8
        {
            get => _rcServo8;
            set
            {
                if (_rcServo8 == value)
                    return;
                _rcServo8 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo9;
        public RCServoEx RCServo9
        {
            get => _rcServo9;
            set
            {
                if (_rcServo9 == value)
                    return;
                _rcServo9 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo10;
        public RCServoEx RCServo10
        {
            get => _rcServo10;
            set
            {
                if (_rcServo10 == value)
                    return;
                _rcServo10 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo11;
        public RCServoEx RCServo11
        {
            get => _rcServo11;
            set
            {
                if (_rcServo11 == value)
                    return;
                _rcServo11 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo12;
        public RCServoEx RCServo12
        {
            get => _rcServo12;
            set
            {
                if (_rcServo12 == value)
                    return;
                _rcServo12 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo13;
        public RCServoEx RCServo13
        {
            get => _rcServo13;
            set
            {
                if (_rcServo13 == value)
                    return;
                _rcServo13 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo14;
        public RCServoEx RCServo14
        {
            get => _rcServo14;
            set
            {
                if (_rcServo14 == value)
                    return;
                _rcServo14 = value;
                OnPropertyChanged();
            }
        }

        private RCServoEx _rcServo15;
        public RCServoEx RCServo15
        {
            get => _rcServo15;
            set
            {
                if (_rcServo15 == value)
                    return;
                _rcServo15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Steppers

        private Visibility _steppersVisibility = Visibility.Collapsed;
        public Visibility SteppersVisibility
        {
            get => _steppersVisibility;
            set
            {
                if (_steppersVisibility == value)
                    return;
                _steppersVisibility = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper0;
        public StepperEx Stepper0
        {
            get => _stepper0;
            set
            {
                if (_stepper0 == value)
                    return;
                _stepper0 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper1;
        public StepperEx Stepper1
        {
            get => _stepper1;
            set
            {
                if (_stepper1 == value)
                    return;
                _stepper1 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper2;
        public StepperEx Stepper2
        {
            get => _stepper2;
            set
            {
                if (_stepper2 == value)
                    return;
                _stepper2 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper3;
        public StepperEx Stepper3
        {
            get => _stepper3;
            set
            {
                if (_stepper3 == value)
                    return;
                _stepper3 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper4;
        public StepperEx Stepper4
        {
            get => _stepper4;
            set
            {
                if (_stepper4 == value)
                    return;
                _stepper4 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper5;
        public StepperEx Stepper5
        {
            get => _stepper5;
            set
            {
                if (_stepper5 == value)
                    return;
                _stepper5 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper6;
        public StepperEx Stepper6
        {
            get => _stepper6;
            set
            {
                if (_stepper6 == value)
                    return;
                _stepper6 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper7;
        public StepperEx Stepper7
        {
            get => _stepper7;
            set
            {
                if (_stepper7 == value)
                    return;
                _stepper7 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper8;
        public StepperEx Stepper8
        {
            get => _stepper8;
            set
            {
                if (_stepper8 == value)
                    return;
                _stepper8 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper9;
        public StepperEx Stepper9
        {
            get => _stepper9;
            set
            {
                if (_stepper9 == value)
                    return;
                _stepper9 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper10;
        public StepperEx Stepper10
        {
            get => _stepper10;
            set
            {
                if (_stepper10 == value)
                    return;
                _stepper10 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper11;
        public StepperEx Stepper11
        {
            get => _stepper11;
            set
            {
                if (_stepper11 == value)
                    return;
                _stepper11 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper12;
        public StepperEx Stepper12
        {
            get => _stepper12;
            set
            {
                if (_stepper12 == value)
                    return;
                _stepper12 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper13;
        public StepperEx Stepper13
        {
            get => _stepper13;
            set
            {
                if (_stepper13 == value)
                    return;
                _stepper13 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper14;
        public StepperEx Stepper14
        {
            get => _stepper14;
            set
            {
                if (_stepper14 == value)
                    return;
                _stepper14 = value;
                OnPropertyChanged();
            }
        }

        private StepperEx _stepper15;
        public StepperEx Stepper15
        {
            get => _stepper15;
            set
            {
                if (_stepper15 == value)
                    return;
                _stepper15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region VoltageInputs

        private Visibility _voltageInputsVisibility = Visibility.Collapsed;
        public Visibility VoltageInputsVisibility
        {
            get => _voltageInputsVisibility;
            set
            {
                if (_voltageInputsVisibility == value)
                    return;
                _voltageInputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput0;
        public VoltageInputEx VoltageInput0
        {
            get => _voltageInput0;
            set
            {
                if (_voltageInput0 == value)
                    return;
                _voltageInput0 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput1;
        public VoltageInputEx VoltageInput1
        {
            get => _voltageInput1;
            set
            {
                if (_voltageInput1 == value)
                    return;
                _voltageInput1 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput2;
        public VoltageInputEx VoltageInput2
        {
            get => _voltageInput2;
            set
            {
                if (_voltageInput2 == value)
                    return;
                _voltageInput2 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput3;
        public VoltageInputEx VoltageInput3
        {
            get => _voltageInput3;
            set
            {
                if (_voltageInput3 == value)
                    return;
                _voltageInput3 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput4;
        public VoltageInputEx VoltageInput4
        {
            get => _voltageInput4;
            set
            {
                if (_voltageInput4 == value)
                    return;
                _voltageInput4 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput5;
        public VoltageInputEx VoltageInput5
        {
            get => _voltageInput5;
            set
            {
                if (_voltageInput5 == value)
                    return;
                _voltageInput5 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput6;
        public VoltageInputEx VoltageInput6
        {
            get => _voltageInput6;
            set
            {
                if (_voltageInput6 == value)
                    return;
                _voltageInput6 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput7;
        public VoltageInputEx VoltageInput7
        {
            get => _voltageInput7;
            set
            {
                if (_voltageInput7 == value)
                    return;
                _voltageInput7 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput8;
        public VoltageInputEx VoltageInput8
        {
            get => _voltageInput8;
            set
            {
                if (_voltageInput8 == value)
                    return;
                _voltageInput8 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput9;
        public VoltageInputEx VoltageInput9
        {
            get => _voltageInput9;
            set
            {
                if (_voltageInput9 == value)
                    return;
                _voltageInput9 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput10;
        public VoltageInputEx VoltageInput10
        {
            get => _voltageInput10;
            set
            {
                if (_voltageInput10 == value)
                    return;
                _voltageInput10 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput11;
        public VoltageInputEx VoltageInput11
        {
            get => _voltageInput11;
            set
            {
                if (_voltageInput11 == value)
                    return;
                _voltageInput11 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput12;
        public VoltageInputEx VoltageInput12
        {
            get => _voltageInput12;
            set
            {
                if (_voltageInput12 == value)
                    return;
                _voltageInput12 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput13;
        public VoltageInputEx VoltageInput13
        {
            get => _voltageInput13;
            set
            {
                if (_voltageInput13 == value)
                    return;
                _voltageInput13 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput14;
        public VoltageInputEx VoltageInput14
        {
            get => _voltageInput14;
            set
            {
                if (_voltageInput14 == value)
                    return;
                _voltageInput14 = value;
                OnPropertyChanged();
            }
        }

        private VoltageInputEx _voltageInput15;
        public VoltageInputEx VoltageInput15
        {
            get => _voltageInput15;
            set
            {
                if (_voltageInput15 == value)
                    return;
                _voltageInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region VoltageRatioInputs

        private Visibility _voltageOutputsVisibility = Visibility.Collapsed;
        public Visibility VoltageOutputsVisibility
        {
            get => _voltageOutputsVisibility;
            set
            {
                if (_voltageOutputsVisibility == value)
                    return;
                _voltageOutputsVisibility = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput0;
        public VoltageRatioInputEx VoltageRatioInput0
        {
            get => _voltageRatioInput0;
            set
            {
                if (_voltageRatioInput0 == value)
                    return;
                _voltageRatioInput0 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput1;
        public VoltageRatioInputEx VoltageRatioInput1
        {
            get => _voltageRatioInput1;
            set
            {
                if (_voltageRatioInput1 == value)
                    return;
                _voltageRatioInput1 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput2;
        public VoltageRatioInputEx VoltageRatioInput2
        {
            get => _voltageRatioInput2;
            set
            {
                if (_voltageRatioInput2 == value)
                    return;
                _voltageRatioInput2 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput3;
        public VoltageRatioInputEx VoltageRatioInput3
        {
            get => _voltageRatioInput3;
            set
            {
                if (_voltageRatioInput3 == value)
                    return;
                _voltageRatioInput3 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput4;
        public VoltageRatioInputEx VoltageRatioInput4
        {
            get => _voltageRatioInput4;
            set
            {
                if (_voltageRatioInput4 == value)
                    return;
                _voltageRatioInput4 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput5;
        public VoltageRatioInputEx VoltageRatioInput5
        {
            get => _voltageRatioInput5;
            set
            {
                if (_voltageRatioInput5 == value)
                    return;
                _voltageRatioInput5 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput6;
        public VoltageRatioInputEx VoltageRatioInput6
        {
            get => _voltageRatioInput6;
            set
            {
                if (_voltageRatioInput6 == value)
                    return;
                _voltageRatioInput6 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput7;
        public VoltageRatioInputEx VoltageRatioInput7
        {
            get => _voltageRatioInput7;
            set
            {
                if (_voltageRatioInput7 == value)
                    return;
                _voltageRatioInput7 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput8;
        public VoltageRatioInputEx VoltageRatioInput8
        {
            get => _voltageRatioInput8;
            set
            {
                if (_voltageRatioInput8 == value)
                    return;
                _voltageRatioInput8 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput9;
        public VoltageRatioInputEx VoltageRatioInput9
        {
            get => _voltageRatioInput9;
            set
            {
                if (_voltageRatioInput9 == value)
                    return;
                _voltageRatioInput9 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput10;
        public VoltageRatioInputEx VoltageRatioInput10
        {
            get => _voltageRatioInput10;
            set
            {
                if (_voltageRatioInput10 == value)
                    return;
                _voltageRatioInput10 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput11;
        public VoltageRatioInputEx VoltageRatioInput11
        {
            get => _voltageRatioInput11;
            set
            {
                if (_voltageRatioInput11 == value)
                    return;
                _voltageRatioInput11 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput12;
        public VoltageRatioInputEx VoltageRatioInput12
        {
            get => _voltageRatioInput12;
            set
            {
                if (_voltageRatioInput12 == value)
                    return;
                _voltageRatioInput12 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput13;
        public VoltageRatioInputEx VoltageRatioInput13
        {
            get => _voltageRatioInput13;
            set
            {
                if (_voltageRatioInput13 == value)
                    return;
                _voltageRatioInput13 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput14;
        public VoltageRatioInputEx VoltageRatioInput14
        {
            get => _voltageRatioInput14;
            set
            {
                if (_voltageRatioInput14 == value)
                    return;
                _voltageRatioInput14 = value;
                OnPropertyChanged();
            }
        }

        private VoltageRatioInputEx _voltageRatioInput15;
        public VoltageRatioInputEx VoltageRatioInput15
        {
            get => _voltageRatioInput15;
            set
            {
                if (_voltageRatioInput15 == value)
                    return;
                _voltageRatioInput15 = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Event Handlers (none)



        #endregion

        #region Commands

        #region OpenVINTHub Command

        //public DelegateCommand OpenVINTHubCommand { get; set; }
        //public string OpenVINTHubContent { get; set; } = "Open";
        //public string OpenVINTHubToolTip { get; set; } = "OpenVINTHub ToolTip";

        //// Can get fancy and use Resources
        ////public string OpenVINTHubContent { get; set; } = "ViewName_OpenVINTHubContent";
        ////public string OpenVINTHubToolTip { get; set; } = "ViewName_OpenVINTHubContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_OpenVINTHubContent">OpenVINTHub</system:String>
        ////    <system:String x:Key="ViewName_OpenVINTHubContentToolTip">OpenVINTHub ToolTip</system:String>  

        //public async void OpenVINTHub()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(OpenVINTHub) Enter", Common.LOG_CATEGORY);

        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called OpenVINTHub";
        //    PublishStatusMessage(Message);

        //    var digitalInputs = Common.PhidgetDeviceLibrary.DigitalInputChannels
        //        .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

        //    foreach (var digitalInput in digitalInputs)
        //    {
        //        OpenDigitalInput(digitalInput.Key);                  
        //    }

        //    var digitalOutputs = Common.PhidgetDeviceLibrary.DigitalOutputChannels
        //        .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

        //    foreach (var digitalOutput in digitalOutputs)
        //    {
        //        OpenDigitalOutput(digitalOutput.Key);             
        //    }

        //    var voltageInputs = Common.PhidgetDeviceLibrary.VoltageInputChannels
        //        .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

        //    foreach (var voltageInput in voltageInputs)
        //    {
        //        OpenVoltageInput(voltageInput.Key);               
        //    }

        //    var voltageRatioInputs = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels
        //        .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

        //    foreach (var voltageRatioInput in voltageRatioInputs)
        //    {
        //        OpenVoltageRatioInput(voltageRatioInput.Key);              
        //    }

        //    OpenVINTHubCommand.RaiseCanExecuteChanged();
        //    CloseVINTHubCommand.RaiseCanExecuteChanged();

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<OpenVINTHubEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<OpenVINTHubEvent>().Publish(
        //    //      new OpenVINTHubEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class OpenVINTHubEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<OpenVINTHubEvent>().Subscribe(OpenVINTHub);

        //    // End Cut Four

        //    //OpenVINTHubCommand.RaiseCanExecuteChanged();
        //    //CloseVINTHubCommand.RaiseCanExecuteChanged();

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(OpenVINTHub) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public Boolean OpenVINTHubCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.

        //    // NOTE(crhodes)
        //    // Since Open/Close at VINTHub level operates in bulk,
        //    // We really don't care if anything is already Open or Closed
        //    // once InterfactKit is selected

        //    if (SelectedVINTHubPhidget > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        #endregion

        #region CloseVINTHub Command

        public DelegateCommand CloseVINTHubCommand { get; set; }
        public string CloseVINTHubContent { get; set; } = "Close";
        public string CloseVINTHubToolTip { get; set; } = "CloseVINTHub ToolTip";

        // Can get fancy and use Resources
        //public string CloseVINTHubContent { get; set; } = "ViewName_CloseVINTHubContent";
        //public string CloseVINTHubToolTip { get; set; } = "ViewName_CloseVINTHubContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseVINTHubContent">CloseVINTHub</system:String>
        //    <system:String x:Key="ViewName_CloseVINTHubContentToolTip">CloseVINTHub ToolTip</system:String>  

        public void CloseVINTHub()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(CloseVINTHub) Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called CloseVINTHub";
            PublishStatusMessage(Message);

            var digitalInputs = Common.PhidgetDeviceLibrary.DigitalInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            foreach (var digitalInput in digitalInputs)
            {
                CloseDigitalInput(digitalInput.Key);
            }

            var digitalOutputs = Common.PhidgetDeviceLibrary.DigitalOutputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);


            foreach (var digitalOutput in digitalOutputs)
            {
                CloseDigitalOutput(digitalOutput.Key);
            }

            var voltageInputs = Common.PhidgetDeviceLibrary.VoltageInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            foreach (var voltageInput in voltageInputs)
            {
                CloseVoltageInput(voltageInput.Key);
            }

            var voltageRatioInputs = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels
                .Where(kv => kv.Key.SerialNumber == SelectedVINTHubPhidget);

            foreach (var voltageRatioInput in voltageRatioInputs)
            {
                CloseVoltageRatioInput(voltageRatioInput.Key);
            }

            //OpenVINTHubCommand.RaiseCanExecuteChanged();
            CloseVINTHubCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<CloseVINTHubEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseVINTHubEvent>().Publish(
            //      new CloseVINTHubEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Three - Put this in PrismEvents

            // public class CloseVINTHubEvent : PubSubEvent { }

            // End Cut Three

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseVINTHubEvent>().Subscribe(CloseVINTHub);

            // End Cut Four

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(CloseVINTHub) Exit", Common.LOG_CATEGORY, startTicks);
        }

        public Boolean CloseVINTHubCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            // NOTE(crhodes)
            // Since Open/Close at VINTHub level operates in bulk,
            // We really don't care if anything is already Open or Closed
            // once InterfactKit is selected

            if (SelectedVINTHubPhidget > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Digital Inputs

        #region OpenDigitalInput Command

        public DelegateCommand<SerialHubPortChannel?> OpenDigitalInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenDigitalInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenDigitalInputCommandParameter;

        public string OpenDigitalInputContent { get; set; } = "Open";
        public string OpenDigitalInputToolTip { get; set; } = "Open DigitalInput";

        // Can get fancy and use Resources
        //public string OpenDigitalInputContent { get; set; } = "ViewName_OpenDigitalInputContent";
        //public string OpenDigitalInputToolTip { get; set; } = "ViewName_OpenDigitalInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenDigitalInputContent">OpenDigitalInput</system:String>
        //    <system:String x:Key="ViewName_OpenDigitalInputContentToolTip">OpenDigitalInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void OpenDigitalInput(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenDigitalInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenDigitalInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            DigitalInputEx digitalInputHost = Common.PhidgetDeviceLibrary.DigitalInputChannels[shpc];

                switch (shpc.HubPort)
                {
                    case 0:
                        await OpenDigitalInput(DigitalInput0);
                        break;

                    case 1:
                        await OpenDigitalInput(DigitalInput1);
                        break;

                    case 2:
                        await OpenDigitalInput(DigitalInput2);
                        break;

                    case 3:
                        await OpenDigitalInput(DigitalInput3);
                        break;

                    case 4:
                        await OpenDigitalInput(DigitalInput4);
                        break;

                    case 5:
                        await OpenDigitalInput(DigitalInput5);
                        break;
                }

            OpenDigitalInputCommand.RaiseCanExecuteChanged();
            CloseDigitalInputCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenDigitalInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenDigitalInputEvent>().Publish(
            //      new OpenDigitalInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenDigitalInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenDigitalInputEvent>().Subscribe(OpenDigitalInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task OpenDigitalInput(DigitalInputEx digitalInput)
        {
            ConfigureInitialLogging(digitalInput);

            if (digitalInput.IsOpen is false)
            {
                await Task.Run(() => digitalInput.Open(10000));
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{digitalInput} already open", Common.LOG_CATEGORY);
            }
        }

        private void ConfigureInitialLogging(DigitalInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            phidgetEx.LogDeviceChannelSequence = LogDeviceChannelSequence;
            phidgetEx.LogChannelAction = LogChannelAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean OpenDigitalInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenDigitalInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (SelectedVINTHubPhidget is null) return false;

            if (serialHubPortChannel is null) return false;

            DigitalInputEx? host;

            if (!Common.PhidgetDeviceLibrary.DigitalInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
            { 
                return true; 
            }

            if (host.Attached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region CloseDigitalInput Command

        public DelegateCommand<SerialHubPortChannel?> CloseDigitalInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseDigitalInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseDigitalInputCommandParameter;

        public string CloseDigitalInputContent { get; set; } = "Close";
        public string CloseDigitalInputToolTip { get; set; } = "Close DigitalInput";

        // Can get fancy and use Resources
        //public string CloseDigitalInputContent { get; set; } = "ViewName_CloseDigitalInputContent";
        //public string CloseDigitalInputToolTip { get; set; } = "ViewName_CloseDigitalInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseDigitalInputContent">CloseDigitalInput</system:String>
        //    <system:String x:Key="ViewName_CloseDigitalInputContentToolTip">CloseDigitalInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseDigitalInput(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseDigitalInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseDigitalInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.DigitalInputChannels[shpc].Close());

            OpenDigitalInputCommand.RaiseCanExecuteChanged();
            CloseDigitalInputCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseDigitalInputHost is null) _CloseDigitalInputHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseDigitalInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseDigitalInputEvent>().Publish(
            //      new CloseDigitalInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseDigitalInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseDigitalInputEvent>().Subscribe(CloseDigitalInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean CloseDigitalInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean CloseDigitalInputCanExecute()
        {
            //// TODO(crhodes)
            //// Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            DigitalInputEx? host;

            if (!Common.PhidgetDeviceLibrary.DigitalInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Digital Outputs

        #region OpenDigitalOutput Command

        //public DelegateCommand<Int32?> OpenDigitalOutputCommand { get; set; }
        public DelegateCommand<SerialHubPortChannel?> OpenDigitalOutputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenDigitalOutputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenDigitalOutputCommandParameter;

        public string OpenDigitalOutputContent { get; set; } = "Open";
        public string OpenDigitalOutputToolTip { get; set; } = "Open DigitalOutput";


        // Can get fancy and use Resources
        //public string OpenDigitalOutputContent { get; set; } = "ViewName_OpenDigitalOutputContent";
        //public string OpenDigitalOutputToolTip { get; set; } = "ViewName_OpenDigitalOutputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenDigitalOutputContent">OpenDigitalOutput</system:String>
        //    <system:String x:Key="ViewName_OpenDigitalOutputContentToolTip">OpenDigitalOutput ToolTip</system:String>  

  
        // If using CommandParameter, figure out TYPE here
        public async void OpenDigitalOutput(SerialHubPortChannel? serialHubPortChannel)
        //public async void OpenDigitalOutput(Int32? channelNumber)
        //public void OpenDigitalOutput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenDigitalOutput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);
 
            switch (shpc.HubPort)
            {
                case 0:
                    await OpenDigitalOutput(DigitalOutput0);
                    break;

                case 1:
                    await OpenDigitalOutput(DigitalOutput1);
                    break;

                case 2:
                    await OpenDigitalOutput(DigitalOutput2);
                    break;

                case 3:
                    await OpenDigitalOutput(DigitalOutput3);
                    break;

                case 4:
                    await OpenDigitalOutput(DigitalOutput4);
                    break;

                case 5:
                    await OpenDigitalOutput(DigitalOutput5);
                    break;
            }

            OpenDigitalOutputCommand.RaiseCanExecuteChanged();
            CloseDigitalOutputCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_OpenDigitalOutputHost is null) _OpenDigitalOutputHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<OpenDigitalOutputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenDigitalOutputEvent>().Publish(
            //      new OpenDigitalOutputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenDigitalOutputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenDigitalOutputEvent>().Subscribe(OpenDigitalOutput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task OpenDigitalOutput(DigitalOutputEx digitalOutput)
        {
            ConfigureInitialLogging(digitalOutput);

            if (digitalOutput.IsOpen is false)
            {
                await Task.Run(() => digitalOutput.Open(10000));
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{digitalOutput} already open", Common.LOG_CATEGORY);
            }
        }

        void ConfigureInitialLogging(DigitalOutputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogDeviceChannelSequence = LogDeviceChannelSequence;
            phidgetEx.LogChannelAction = LogChannelAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        // If using CommandParameter, figure out TYPE and fix above
        //public Boolean OpenDigitalOutputCanExecute(Int32? channelNumber)
        public Boolean OpenDigitalOutputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenDigitalOutputCanExecute()
        {
            //// TODO(crhodes)
            //// Add any before button is enabled logic.

            if (SelectedVINTHubPhidget is null) return false;

            DigitalOutputEx? host;

            if (!Common.PhidgetDeviceLibrary.DigitalOutputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
            {
                return true;
            }

            if (host.Attached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region CloseDigitalOutput Command

        public DelegateCommand<SerialHubPortChannel?> CloseDigitalOutputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseDigitalOutputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseDigitalOutputCommandParameter;

        public string CloseDigitalOutputContent { get; set; } = "Close";
        public string CloseDigitalOutputToolTip { get; set; } = "Close DigitalOutput";

        // Can get fancy and use Resources
        //public string CloseDigitalOutputContent { get; set; } = "ViewName_CloseDigitalOutputContent";
        //public string CloseDigitalOutputToolTip { get; set; } = "ViewName_CloseDigitalOutputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseDigitalOutputContent">CloseDigitalOutput</system:String>
        //    <system:String x:Key="ViewName_CloseDigitalOutputContentToolTip">CloseDigitalOutput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseDigitalOutput(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseDigitalOutput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseDigitalInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.DigitalOutputChannels[shpc].Close());

            OpenDigitalOutputCommand.RaiseCanExecuteChanged();
            CloseDigitalOutputCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseDigitalOutputHost is null) _CloseDigitalOutputHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseDigitalOutputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseDigitalOutputEvent>().Publish(
            //      new CloseDigitalOutputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseDigitalOutputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseDigitalOutputEvent>().Subscribe(CloseDigitalOutput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean CloseDigitalOutputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean CloseDigitalOutputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            DigitalOutputEx? host;

            if (!Common.PhidgetDeviceLibrary.DigitalOutputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        //#region OpenRCServo Command

        //public DelegateCommand<SerialHubPortChannel?> OpenRCServoCommand { get; set; }
        //// If displaying UserControl
        //// public static WindowHost _OpenRCServoHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE OpenRCServoCommandParameter;

        //public string OpenRCServoContent { get; set; } = "Open";
        //public string OpenRCServoToolTip { get; set; } = "Open RCServo";

        //// Can get fancy and use Resources
        ////public string OpenRCServoContent { get; set; } = "ViewName_OpenRCServoContent";
        ////public string OpenRCServoToolTip { get; set; } = "ViewName_OpenRCServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_OpenRCServoContent">OpenRCServo</system:String>
        ////    <system:String x:Key="ViewName_OpenRCServoContentToolTip">OpenRCServo ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here



        //public async void OpenRCServo(SerialHubPortChannel? serialHubPortChannel)
        ////public void OpenRCServo()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.

        //    SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

        //    Message = $"Cool, you called OpenRCServo on " +
        //         $"serialHubPortChannel:{shpc.SerialNumber}" +
        //         $":{shpc.HubPort}:{shpc.Channel}";
        //    PublishStatusMessage(Message);

        //    RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.RCServoChannels[shpc];

        //    switch (shpc.Channel)
        //    {
        //        case 0:
        //            await OpenRCServo(RCServo0);
        //            break;

        //        case 1:
        //            await OpenRCServo(RCServo1);
        //            break;

        //        case 2:
        //            await OpenRCServo(RCServo2);
        //            break;

        //        case 3:
        //            await OpenRCServo(RCServo3);
        //            break;

        //        case 4:
        //            await OpenRCServo(RCServo4);
        //            break;

        //        case 5:
        //            await OpenRCServo(RCServo5);
        //            break;

        //        case 6:
        //            await OpenRCServo(RCServo6);
        //            break;

        //        case 7:
        //            await OpenRCServo(RCServo7);
        //            break;

        //        case 8:
        //            await OpenRCServo(RCServo8);
        //            break;

        //        case 9:
        //            await OpenRCServo(RCServo9);
        //            break;

        //        case 10:
        //            await OpenRCServo(RCServo10);
        //            break;

        //        case 11:
        //            await OpenRCServo(RCServo11);
        //            break;

        //        case 12:
        //            await OpenRCServo(RCServo12);
        //            break;

        //        case 13:
        //            await OpenRCServo(RCServo13);
        //            break;

        //        case 14:
        //            await OpenRCServo(RCServo14);
        //            break;

        //        case 15:
        //            await OpenRCServo(RCServo15);
        //            break;

        //    }

        //    OpenRCServoCommand.RaiseCanExecuteChanged();
        //    CloseRCServoCommand.RaiseCanExecuteChanged();

        //    // If launching a UserControl

        //    // if (_OpenRCServoHost is null) _OpenRCServoHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<OpenRCServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<OpenRCServoEvent>().Publish(
        //    //      new OpenRCServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class OpenRCServoEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<OpenRCServoEvent>().Subscribe(OpenRCServo);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private async Task OpenRCServo(RCServoEx rcServo)
        //{
        //    ConfigureInitialLogging(rcServo);

        //    if (rcServo.IsOpen is false)
        //    {
        //        await Task.Run(() => rcServo.Open(10000));
        //    }
        //    else
        //    {
        //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{rcServo} already open", Common.LOG_CATEGORY);
        //    }
        //}

        //void ConfigureInitialLogging(RCServoEx rcServo)
        //{
        //    rcServo.LogPhidgetEvents = LogPhidgetEvents;
        //    rcServo.LogErrorEvents = LogErrorEvents;
        //    rcServo.LogPropertyChangeEvents = LogPropertyChangeEvents;

        //    //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //    rcServo.LogPositionChangeEvents = LogPositionChangeEvents;
        //    rcServo.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //    rcServo.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

        //    rcServo.LogDeviceChannelSequence = LogDeviceChannelSequence;
        //    rcServo.LogChannelAction = LogChannelAction;
        //    rcServo.LogActionVerification = LogActionVerification;
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        //public Boolean OpenRCServoCanExecute(SerialHubPortChannel? serialHubPortChannel)
        ////public Boolean OpenRCServoCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.

        //    if (SelectedVINTHubPhidget is null) return false;

        //    if (serialHubPortChannel is null) return false;

        //    RCServoEx? host;

        //    if (!Common.PhidgetDeviceLibrary.RCServoChannels
        //        .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
        //    {
        //        return true;
        //    }

        //    if (host.Attached)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //#endregion

        //#region CloseRCServo Command

        //public DelegateCommand<SerialHubPortChannel?> CloseRCServoCommand { get; set; }
        //// If displaying UserControl
        //// public static WindowHost _CloseRCServoHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE CloseRCServoCommandParameter;

        //public string CloseRCServoContent { get; set; } = "Close";
        //public string CloseRCServoToolTip { get; set; } = "Close RCServo";

        //// Can get fancy and use Resources
        ////public string CloseRCServoContent { get; set; } = "ViewName_CloseRCServoContent";
        ////public string CloseRCServoToolTip { get; set; } = "ViewName_CloseRCServoContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_CloseRCServoContent">CloseRCServo</system:String>
        ////    <system:String x:Key="ViewName_CloseRCServoContentToolTip">CloseRCServo ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here
        //public async void CloseRCServo(SerialHubPortChannel? serialHubPortChannel)
        ////public void CloseRCServo()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.

        //    SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

        //    Message = $"Cool, you called CloseRCServo on " +
        //        $"serialHubPortChannel:{shpc.SerialNumber}" +
        //        $":{shpc.HubPort}:{shpc.Channel}";

        //    PublishStatusMessage(Message);

        //    await Task.Run(() => Common.PhidgetDeviceLibrary.RCServoChannels[shpc].Close());

        //    OpenRCServoCommand.RaiseCanExecuteChanged();
        //    CloseRCServoCommand.RaiseCanExecuteChanged();

        //    // If launching a UserControl

        //    // if (_CloseRCServoHost is null) _CloseRCServoHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<CloseRCServoEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<CloseRCServoEvent>().Publish(
        //    //      new CloseRCServoEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class CloseRCServoEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<CloseRCServoEvent>().Subscribe(CloseRCServo);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public Boolean CloseRCServoCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.

        //    if (serialHubPortChannel is null) return false;

        //    RCServoEx? host;

        //    if (!Common.PhidgetDeviceLibrary.RCServoChannels
        //        .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
        //    {
        //        return false;
        //    }

        //    if (host.IsOpen)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

        //#region SetPositionRange Command

        //public DelegateCommand<string> SetPositionRangeCommand { get; set; }
        ////public TYPE SetPositionRangeCommandParameter;
        //public string SetPositionRangeContent { get; set; } = "SetPositionRange";
        //public string SetPositionRangeToolTip { get; set; } = "SetPositionRange ToolTip";

        //// Can get fancy and use Resources
        ////public string SetPositionRangeContent { get; set; } = "ViewName_SetPositionRangeContent";
        ////public string SetPositionRangeToolTip { get; set; } = "ViewName_SetPositionRangeContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_SetPositionRangeContent">SetPositionRange</system:String>
        ////    <system:String x:Key="ViewName_SetPositionRangeContentToolTip">SetPositionRange ToolTip</system:String>  

        //public void SetPositionRange(string servoID)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("(SetPositionRange) Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.
        //    Message = "Cool, you called SetPositionRange";
        //    PublishStatusMessage(Message);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<SetPositionRangeEvent>().Publish(
        //    //      new SetPositionRangeEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Three - Put this in PrismEvents

        //    // public class SetPositionRangeEvent : PubSubEvent { }

        //    // End Cut Three

        //    // Start Cut Four - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<SetPositionRangeEvent>().Subscribe(SetPositionRange);

        //    // End Cut Four

        //    // FIX(crhodes)
        //    // 
        //    //AdvancedServoServoCollection servos = ActiveAdvancedServo.AdvancedServo.servos;
        //    //Phidget22.AdvancedServoServo servo = null;

        //    //Int32 servoIndex = Int32.Parse(servoID);
        //    //servo = servos[servoIndex];

        //    // NOTE(crhodes)
        //    // Should be safe to get Acceleration, Velocity, and Position here
        //    // Device is Engaged

        //    Double? halfRange;
        //    Double? tenPercent;

        //    try
        //    {
        //        // FIX(crhodes)
        //        // 
        //        //AdvancedServoProperties[servoIndex].PositionMin =
        //        //    AdvancedServoProperties[servoIndex].Position - AdvancedServoProperties[servoIndex].PositionRange;

        //        //AdvancedServoProperties[servoIndex].PositionMax =
        //        //    AdvancedServoProperties[servoIndex].Position + AdvancedServoProperties[servoIndex].PositionRange;

        //        //switch (servoIndex)
        //        //{

        //        //    case 0:
        //        //        // TODO(crhodes)
        //        //        // Make this fancier.  Take the smaller of difference
        //        //        // between Min and Current and Max and Current
        //        //        // and then divide that by 10

        //        //        AdvancedServoProperties[0].PositionMin = AdvancedServoProperties[0].Position - AdvancedServoProperties[0].PositionRange;

        //        //        AdvancedServoProperties[0].PositionMax = AdvancedServoProperties[0].Position + AdvancedServoProperties[0].PositionRange;

        //        //        //PositionMin_S0 = Position_S0 - PositionRange_S0;

        //        //        //PositionMax_S0 = Position_S0 + PositionRange_S0;

        //        //        break;

        //        //    case 1:
        //        //        PositionMin_S1 = Position_S1 - PositionRange_S1;

        //        //        PositionMax_S1 = Position_S1 + PositionRange_S1;

        //        //        break;

        //        //    case 2:
        //        //        PositionMin_S2 = Position_S2 - PositionRange_S2;

        //        //        PositionMax_S2 = Position_S2 + PositionRange_S2;

        //        //        break;

        //        //    case 3:
        //        //        PositionMin_S3 = Position_S3 - PositionRange_S3;

        //        //        PositionMax_S3 = Position_S3 + PositionRange_S3;

        //        //        break;

        //        //    case 4:
        //        //        PositionMin_S4 = Position_S4 - PositionRange_S4;

        //        //        PositionMax_S4 = Position_S4 + PositionRange_S4;

        //        //        break;

        //        //    case 5:
        //        //        PositionMin_S5 = Position_S5 - PositionRange_S5;

        //        //        PositionMax_S5 = Position_S5 + PositionRange_S5;

        //        //        break;

        //        //    case 6:
        //        //        PositionMin_S6 = Position_S6 - PositionRange_S6;

        //        //        PositionMax_S6 = Position_S6 + PositionRange_S6;

        //        //        break;

        //        //    case 7:
        //        //        PositionMin_S7 = Position_S7 - PositionRange_S7;

        //        //        PositionMax_S7 = Position_S7 + PositionRange_S7;

        //        //        break;

        //        //    default:
        //        //        Log.Trace($"UpdateAdvancedServoProperties count:{servos.Count}", Common.LOG_CATEGORY);
        //        //        break;

        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, Common.LOG_CATEGORY);
        //    }

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("(SetPositionRange) Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //public Boolean SetPositionRangeCanExecute(string value)
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //    //if (DeviceAttached is not null)
        //    //    return (Boolean)DeviceAttached;
        //    //else
        //    //    return false;
        //}

        //#endregion

        //#region OpenStepper Command

        //public DelegateCommand<SerialHubPortChannel?> OpenStepperCommand { get; set; }
        //// If displaying UserControl
        //// public static WindowHost _OpenStepperHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE OpenStepperCommandParameter;

        //public string OpenStepperContent { get; set; } = "Open";
        //public string OpenStepperToolTip { get; set; } = "Open Stepper";

        //// Can get fancy and use Resources
        ////public string OpenStepperContent { get; set; } = "ViewName_OpenStepperContent";
        ////public string OpenStepperToolTip { get; set; } = "ViewName_OpenStepperContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_OpenStepperContent">OpenStepper</system:String>
        ////    <system:String x:Key="ViewName_OpenStepperContentToolTip">OpenStepper ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here
        //public async void OpenStepper(SerialHubPortChannel? serialHubPortChannel)
        ////public void OpenStepper()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.

        //    SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

        //    Message = $"Cool, you called OpenStepper on " +
        //        $"serialHubPortChannel:{shpc.SerialNumber}" +
        //        $":{shpc.HubPort}:{shpc.Channel}";
        //    PublishStatusMessage(Message);

        //    switch (shpc.Channel)
        //    {
        //        case 0:
        //            await OpenStepper(Stepper0);
        //            break;

        //        case 1:
        //            await OpenStepper(Stepper1);
        //            break;

        //        case 2:
        //            await OpenStepper(Stepper2);
        //            break;

        //        case 3:
        //            await OpenStepper(Stepper0);
        //            break;

        //            // TODO(crhodes)
        //            // Add more cases if a board supports more channels
        //    }

        //    OpenStepperCommand.RaiseCanExecuteChanged();
        //    CloseStepperCommand.RaiseCanExecuteChanged();

        //    // If launching a UserControl

        //    // if (_OpenStepperHost is null) _OpenStepperHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<OpenStepperEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<OpenStepperEvent>().Publish(
        //    //      new OpenStepperEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class OpenStepperEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<OpenStepperEvent>().Subscribe(OpenStepper);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //private async Task OpenStepper(StepperEx stepper)
        //{
        //    ConfigureInitialLogging(stepper);

        //    if (stepper.IsOpen is false)
        //    {
        //        await Task.Run(() => stepper.Open(10000));
        //    }
        //    else
        //    {
        //        if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{stepper} already open", Common.LOG_CATEGORY);
        //    }
        //}

        //void ConfigureInitialLogging(StepperEx stepper)
        //{
        //    stepper.LogPhidgetEvents = LogPhidgetEvents;
        //    stepper.LogErrorEvents = LogErrorEvents;
        //    stepper.LogPropertyChangeEvents = LogPropertyChangeEvents;

        //    stepper.LogPositionChangeEvents = LogPositionChangeEvents;
        //    stepper.LogVelocityChangeEvents = LogVelocityChangeEvents;

        //    stepper.LogStoppedEvents = LogStoppedEvents;

        //    stepper.LogDeviceChannelSequence = LogDeviceChannelSequence;
        //    stepper.LogChannelAction = LogChannelAction;
        //    stepper.LogActionVerification = LogActionVerification;
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        //public Boolean OpenStepperCanExecute(SerialHubPortChannel? serialHubPortChannel)
        ////public Boolean OpenStepperCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.

        //    if (SelectedVINTHubPhidget is null) return false;

        //    if (serialHubPortChannel is null) return false;

        //    StepperEx? host;

        //    if (!Common.PhidgetDeviceLibrary.StepperChannels
        //            .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host))
        //    {
        //        return true;
        //    }

        //    if (host.IsOpen)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //#endregion

        //#region CloseStepper Command

        //public DelegateCommand<SerialHubPortChannel?> CloseStepperCommand { get; set; }
        //// If displaying UserControl
        //// public static WindowHost _CloseStepperHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE CloseStepperCommandParameter;

        //public string CloseStepperContent { get; set; } = "Close";
        //public string CloseStepperToolTip { get; set; } = "Close Stepper";

        //// Can get fancy and use Resources
        ////public string CloseStepperContent { get; set; } = "ViewName_CloseStepperContent";
        ////public string CloseStepperToolTip { get; set; } = "ViewName_CloseStepperContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_CloseStepperContent">CloseStepper</system:String>
        ////    <system:String x:Key="ViewName_CloseStepperContentToolTip">CloseStepper ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here
        //public async void CloseStepper(SerialHubPortChannel? serialHubPortChannel)
        ////public void CloseStepper()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
        //    // TODO(crhodes)
        //    // Do something amazing.

        //    SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

        //    Message = $"Cool, you called CloseStepper on " +
        //        $"serialHubPortChannel:{shpc.SerialNumber}" +
        //        $":{shpc.HubPort}:{shpc.Channel}";

        //    PublishStatusMessage(Message);

        //    await Task.Run(() => Common.PhidgetDeviceLibrary.StepperChannels[shpc].Close());

        //    OpenStepperCommand.RaiseCanExecuteChanged();
        //    CloseStepperCommand.RaiseCanExecuteChanged();

        //    // If launching a UserControl

        //    // if (_CloseStepperHost is null) _CloseStepperHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<CloseStepperEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<CloseStepperEvent>().Publish(
        //    //      new CloseStepperEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class CloseStepperEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<CloseStepperEvent>().Subscribe(CloseStepper);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        //public Boolean CloseStepperCanExecute(SerialHubPortChannel? serialHubPortChannel)
        ////public Boolean CloseStepperCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.

        //    if (serialHubPortChannel is null) return false;

        //    StepperEx? host;

        //    if (!Common.PhidgetDeviceLibrary.StepperChannels
        //        .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

        //    if (host.IsOpen)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

        //#region ZeroCurrentPosition Command

        //// TODO(crhodes)
        //// I think this belongs in StepperPositionControl
        //// like the stuff in RCServoTargetPositionControl

        ////public DelegateCommand ZeroCurrentPositionCommand { get; set; }
        //// If using CommandParameter, figure out TYPE here and above
        //// and remove above declaration
        //public DelegateCommand<string> ZeroCurrentPositionCommand { get; set; }

        //// If displaying UserControl
        //// public static WindowHost _ZeroCurrentPositionHost = null;

        //// If using CommandParameter, figure out TYPE here
        ////public TYPE ZeroCurrentPositionCommandParameter;

        //public string ZeroCurrentPositionContent { get; set; } = "ZeroCurrentPosition";
        //public string ZeroCurrentPositionToolTip { get; set; } = "ZeroCurrentPosition ToolTip";

        //// Can get fancy and use Resources
        ////public string ZeroCurrentPositionContent { get; set; } = "ViewName_ZeroCurrentPositionContent";
        ////public string ZeroCurrentPositionToolTip { get; set; } = "ViewName_ZeroCurrentPositionContentToolTip";

        //// Put these in Resource File
        ////    <system:String x:Key="ViewName_ZeroCurrentPositionContent">ZeroCurrentPosition</system:String>
        ////    <system:String x:Key="ViewName_ZeroCurrentPositionContentToolTip">ZeroCurrentPosition ToolTip</system:String>  

        //// If using CommandParameter, figure out TYPE here
        //public void ZeroCurrentPosition(string stepperNumber)
        ////public void ZeroCurrentPosition()
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

        //    // TODO(crhodes)
        //    // Do something amazing.

        //    Message = "Cool, you called ZeroCurrentPosition";
        //    PublishStatusMessage(Message);

        //    Int32 channel;

        //    if (!Int32.TryParse(stepperNumber, out channel)) throw new Exception($"Cannot parse stepperNumber:{stepperNumber}");

        //    switch (channel)
        //    {
        //        case 0:
        //            Stepper0?.AddPositionOffset(-Stepper0.Position);
        //            break;

        //        case 1:
        //            Stepper1.AddPositionOffset(-Stepper1.Position);
        //            break;

        //        case 2:
        //            Stepper2.AddPositionOffset(-Stepper2.Position);
        //            break;

        //        case 3:
        //            Stepper3.AddPositionOffset(-Stepper3.Position);
        //            break;

        //            //case 4:
        //            //    Stepper4.Position = 0;
        //            //    Stepper4.TargetPosition = 0;
        //            //    break;

        //            //case 5:
        //            //    Stepper5.Position = 0;
        //            //    Stepper5.TargetPosition = 0;
        //            //    break;

        //            //case 6:
        //            //    Stepper6.Position = 0;
        //            //    Stepper6.TargetPosition = 0;
        //            //    break;

        //            //case 7:
        //            //    Stepper7.Position = 0;
        //            //    Stepper7.TargetPosition = 0;
        //            //    break;
        //    }

        //    // If launching a UserControl

        //    // if (_ZeroCurrentPositionHost is null) _ZeroCurrentPositionHost = new WindowHost();
        //    // var userControl = new USERCONTROL();

        //    // _loggingConfigurationHost.DisplayUserControlInHost(
        //    //     "TITLE GOES HERE",
        //    //     //Common.DEFAULT_WINDOW_WIDTH,
        //    //     //Common.DEFAULT_WINDOW_HEIGHT,
        //    //     (Int32)userControl.Width + Common.WINDOW_HOSTING_USER_CONTROL_WIDTH_PAD,
        //    //     (Int32)userControl.Height + Common.WINDOW_HOSTING_USER_CONTROL_HEIGHT_PAD,
        //    //     ShowWindowMode.Modeless_Show,
        //    //     userControl);

        //    // Uncomment this if you are telling someone else to handle this

        //    // Common.EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Publish();

        //    // May want EventArgs

        //    //  EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Publish(
        //    //      new ZeroCurrentPositionEventArgs()
        //    //      {
        //    //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
        //    //            Process = _contextMainViewModel.Context.SelectedProcess
        //    //      });

        //    // Start Cut Four - Put this in PrismEvents

        //    // public class ZeroCurrentPositionEvent : PubSubEvent { }

        //    // End Cut Four

        //    // Start Cut Five - Put this in places that listen for event

        //    //Common.EventAggregator.GetEvent<ZeroCurrentPositionEvent>().Subscribe(ZeroCurrentPosition);

        //    // End Cut Five

        //    if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        //// If using CommandParameter, figure out TYPE and fix above
        //public Boolean ZeroCurrentPositionCanExecute(string stepperNumber)
        ////public Boolean ZeroCurrentPositionCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //}

        //#endregion

        #region Voltage Inputs

        #region OpenVoltageInput Command

        public DelegateCommand<SerialHubPortChannel?> OpenVoltageInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenVoltageInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenVoltageInputCommandParameter;

        public string OpenVoltageInputContent { get; set; } = "Open";
        public string OpenVoltageInputToolTip { get; set; } = "Open VoltageInput";

        // Can get fancy and use Resources
        //public string OpenVoltageInputContent { get; set; } = "ViewName_OpenVoltageInputContent";
        //public string OpenVoltageInputToolTip { get; set; } = "ViewName_OpenVoltageInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenVoltageInputContent">OpenVoltageInput</system:String>
        //    <system:String x:Key="ViewName_OpenVoltageInputContentToolTip">OpenVoltageInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void OpenVoltageInput(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenVoltageInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenVoltageInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            switch (shpc.HubPort)
            {
                case 0:
                    await OpenVoltageInput(VoltageInput0);
                    break;

                case 1:
                    await OpenVoltageInput(VoltageInput1);
                    break;

                case 2:
                    await OpenVoltageInput(VoltageInput2);
                    break;

                case 3:
                    await OpenVoltageInput(VoltageInput3);
                    break;

                case 4:
                    await OpenVoltageInput(VoltageInput4);
                    break;

                case 5:
                    await OpenVoltageInput(VoltageInput5);
                    break;
            }

            OpenVoltageInputCommand.RaiseCanExecuteChanged();
            RefreshVoltageInputCommand.RaiseCanExecuteChanged();
            RaisePerformanceEventCommand.RaiseCanExecuteChanged();
            CloseVoltageInputCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenVoltageInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenVoltageInputEvent>().Publish(
            //      new OpenVoltageInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenVoltageInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenVoltageInputEvent>().Subscribe(OpenVoltageInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task OpenVoltageInput(VoltageInputEx voltageInput)
        {
            ConfigureInitialLogging(voltageInput);

            if (voltageInput.IsOpen is false)
            {
                //await Task.Run(() => voltageInput.Open(5000));    // Wait 5 seconds to attach
                await Task.Run(() => voltageInput.Open(10000));  // Block until attached
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{voltageInput} already open", Common.LOG_CATEGORY);
            }
        }

        void ConfigureInitialLogging(VoltageInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogDeviceChannelSequence = LogDeviceChannelSequence;
            phidgetEx.LogChannelAction = LogChannelAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean OpenVoltageInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenVoltageInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region RefreshVoltageInput Command

        public DelegateCommand<SerialHubPortChannel?> RefreshVoltageInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _RefreshVoltageInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE RefreshVoltageInputCommandParameter;

        public string RefreshVoltageInputContent { get; set; } = "Refresh";
        public string RefreshVoltageInputToolTip { get; set; } = "Refresh VoltageInput";

        // Can get fancy and use Resources
        //public string RefreshVoltageInputContent { get; set; } = "ViewName_RefreshVoltageInputContent";
        //public string RefreshVoltageInputToolTip { get; set; } = "ViewName_RefreshVoltageInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RefreshVoltageInputContent">RefreshVoltageInput</system:String>
        //    <system:String x:Key="ViewName_RefreshVoltageInputContentToolTip">RefreshVoltageInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void RefreshVoltageInput(SerialHubPortChannel? serialHubPortChannel)
        //public void RefreshVoltageInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called RefreshVoltageInput\r\n on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            switch (shpc.HubPort)
            {
                case 0:
                    await RefreshVoltageInput(VoltageInput0);
                    break;

                case 1:
                    await RefreshVoltageInput(VoltageInput1);
                    break;

                case 2:
                    await RefreshVoltageInput(VoltageInput2);
                    break;

                case 3:
                    await RefreshVoltageInput(VoltageInput3);
                    break;

                case 4:
                    await RefreshVoltageInput(VoltageInput4);
                    break;

                case 5:
                    await RefreshVoltageInput(VoltageInput5);
                    break;

                //case 6:
                //    await RefreshVoltageInput(VoltageInput6);
                //    break;

                //case 7:
                //    await RefreshVoltageInput(VoltageInput7);
                //    break;

                //case 8:
                //    await RefreshVoltageInput(VoltageInput8);
                //    break;

                //case 9:
                //    await RefreshVoltageInput(VoltageInput9);
                //    break;

                //case 10:
                //    await RefreshVoltageInput(VoltageInput10);
                //    break;

                //case 11:
                //    await RefreshVoltageInput(VoltageInput11);
                //    break;

                //case 12:
                //    await RefreshVoltageInput(VoltageInput12);
                //    break;

                //case 13:
                //    await RefreshVoltageInput(VoltageInput13);
                //    break;

                //case 14:
                //    await RefreshVoltageInput(VoltageInput14);
                //    break;

                //case 15:
                //    await RefreshVoltageInput(VoltageInput15);
                //    break;
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RefreshVoltageInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RefreshVoltageInputEvent>().Publish(
            //      new RefreshVoltageInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class RefreshVoltageInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RefreshVoltageInputEvent>().Subscribe(RefreshVoltageInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task RefreshVoltageInput(VoltageInputEx voltageInput)
        {
            if (voltageInput.IsOpen is true)
            {
                await Task.Run(() => voltageInput.RefreshProperties());
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{voltageInput} not open", Common.LOG_CATEGORY);
            }
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean RefreshVoltageInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean RefreshVoltageInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageInputChannels.TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseVoltageInput Command

        public DelegateCommand<SerialHubPortChannel?> CloseVoltageInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseVoltageInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseVoltageInputCommandParameter;

        public string CloseVoltageInputContent { get; set; } = "Close";
        public string CloseVoltageInputToolTip { get; set; } = "Close VoltageInput";

        // Can get fancy and use Resources
        //public string CloseVoltageInputContent { get; set; } = "ViewName_CloseVoltageInputContent";
        //public string CloseVoltageInputToolTip { get; set; } = "ViewName_CloseVoltageInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseVoltageInputContent">CloseVoltageInput</system:String>
        //    <system:String x:Key="ViewName_CloseVoltageInputContentToolTip">CloseVoltageInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseVoltageInput(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseVoltageInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseVoltageInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.VoltageInputChannels[shpc].Close());

            OpenVoltageInputCommand.RaiseCanExecuteChanged();
            RefreshVoltageInputCommand.RaiseCanExecuteChanged();
            RaisePerformanceEventCommand.RaiseCanExecuteChanged();
            CloseVoltageInputCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseVoltageInputHost is null) _CloseVoltageInputHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseVoltageInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseVoltageInputEvent>().Publish(
            //      new CloseVoltageInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseVoltageInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseVoltageInputEvent>().Subscribe(CloseVoltageInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean CloseVoltageInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean CloseVoltageInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region VoltageRatio Inputs

        #region OpenVoltageRatioInput Command

        public DelegateCommand<SerialHubPortChannel?> OpenVoltageRatioInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _OpenVoltageRatioInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE OpenVoltageRatioInputCommandParameter;

        public string OpenVoltageRatioInputContent { get; set; } = "Open";
        public string OpenVoltageRatioInputToolTip { get; set; } = "Open VoltageRatioInput";

        // Can get fancy and use Resources
        //public string OpenVoltageRatioInputContent { get; set; } = "ViewName_OpenVoltageRatioInputContent";
        //public string OpenVoltageRatioInputToolTip { get; set; } = "ViewName_OpenVoltageRatioInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_OpenVoltageRatioInputContent">OpenVoltageRatioInput</system:String>
        //    <system:String x:Key="ViewName_OpenVoltageRatioInputContentToolTip">OpenVoltageRatioInput ToolTip</system:String>  

         // If using CommandParameter, figure out TYPE here
        public async void OpenVoltageRatioInput(SerialHubPortChannel? serialHubPortChannel)
        //public void OpenVoltageRatioInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called OpenVoltageRatioInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);
            //VoltageRatioInputEx voltageRatioInputHost = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[serialHubPortChannel];

            switch (shpc.HubPort)
            {
                case 0:
                    if (VoltageRatioInput0 is null) VoltageRatioInput0 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput0);
                    break;

                case 1:
                    if (VoltageRatioInput1 is null) VoltageRatioInput1 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput1);
                    break;

                case 2:
                    if (VoltageRatioInput2 is null) VoltageRatioInput2 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput2);
                    break;

                case 3:
                    if (VoltageRatioInput3 is null) VoltageRatioInput3 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput3);
                    break;

                case 4:
                    if (VoltageRatioInput4 is null) VoltageRatioInput4 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput4);
                    break;

                case 5:
                    if (VoltageRatioInput5 is null) VoltageRatioInput5 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                    await OpenVoltageRatioInput(VoltageRatioInput5);
                    break;

                //case 6:
                //    if (VoltageRatioInput6 is null) VoltageRatioInput6 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput6);
                //    break;

                //case 7:
                //    if (VoltageRatioInput7 is null) VoltageRatioInput7 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput7);
                //    break;

                //case 8:
                //    if (VoltageRatioInput8 is null) VoltageRatioInput8 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput8);
                //    break;

                //case 9:
                //    if (VoltageRatioInput9 is null) VoltageRatioInput9 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput9);
                //    break;

                //case 10:
                //    if (VoltageRatioInput10 is null) VoltageRatioInput10 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput10);
                //    break;

                //case 11:
                //    if (VoltageRatioInput11 is null) VoltageRatioInput11 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput11);
                //    break;

                //case 12:
                //    if (VoltageRatioInput12 is null) VoltageRatioInput12 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput12);
                //    break;

                //case 13:
                //    if (VoltageRatioInput13 is null) VoltageRatioInput13 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput13);
                //    break;

                //case 14:
                //    if (VoltageRatioInput14 is null) VoltageRatioInput14 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput14);
                //    break;

                //case 15:
                //    if (VoltageRatioInput15 is null) VoltageRatioInput15 = Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc];
                //    await OpenVoltageRatioInput(VoltageRatioInput15);
                //    break;
            }

            OpenVoltageRatioInputCommand.RaiseCanExecuteChanged();
            RefreshVoltageRatioInputCommand.RaiseCanExecuteChanged();
            CloseVoltageRatioInputCommand.RaiseCanExecuteChanged();

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<OpenVoltageRatioInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<OpenVoltageRatioInputEvent>().Publish(
            //      new OpenVoltageRatioInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class OpenVoltageRatioInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<OpenVoltageRatioInputEvent>().Subscribe(OpenVoltageRatioInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task OpenVoltageRatioInput(VoltageRatioInputEx voltageRatioInput)
        {
            ConfigureInitialLogging(voltageRatioInput);

            if (voltageRatioInput.IsOpen is false)
            {
                await Task.Run(() => voltageRatioInput.Open(10000));    // Wait 1 seconds to attach
                //await Task.Run(() => voltageRatioInput.Open());  // Block until attached
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{voltageRatioInput} already open", Common.LOG_CATEGORY);
            }
        }

        void ConfigureInitialLogging(VoltageRatioInputEx phidgetEx)
        {
            phidgetEx.LogPhidgetEvents = LogPhidgetEvents;
            phidgetEx.LogErrorEvents = LogErrorEvents;
            phidgetEx.LogPropertyChangeEvents = LogPropertyChangeEvents;

            //rcServoHot.LogCurrentChangeEvents = LogCurrentChangeEvents;
            //phidgetEx.LogPositionChangeEvents = LogPositionChangeEvents;
            //phidgetEx.LogVelocityChangeEvents = LogVelocityChangeEvents;

            //stepper.LogTargetPositionReachedEvents = LogTargetPositionReachedEvents;

            phidgetEx.LogDeviceChannelSequence = LogDeviceChannelSequence;
            phidgetEx.LogChannelAction = LogChannelAction;
            phidgetEx.LogActionVerification = LogActionVerification;
        }

        // If using CommandParameter, figure out TYPE and fix above        
        public Boolean OpenVoltageRatioInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean OpenVoltageRatioInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageRatioInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageRatioInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region RefreshVoltageRatioInput Command

        public DelegateCommand<SerialHubPortChannel?> RefreshVoltageRatioInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _RefreshVoltageRatioInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE RefreshVoltageRatioInputCommandParameter;

        public string RefreshVoltageRatioInputContent { get; set; } = "Refresh";
        public string RefreshVoltageRatioInputToolTip { get; set; } = "Refresh VoltageRatioInput";

        // Can get fancy and use Resources
        //public string RefreshVoltageRatioInputContent { get; set; } = "ViewName_RefreshVoltageRatioInputContent";
        //public string RefreshVoltageRatioInputToolTip { get; set; } = "ViewName_RefreshVoltageRatioInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RefreshVoltageRatioInputContent">RefreshVoltageRatioInput</system:String>
        //    <system:String x:Key="ViewName_RefreshVoltageRatioInputContentToolTip">RefreshVoltageRatioInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void RefreshVoltageRatioInput(SerialHubPortChannel? serialHubPortChannel)
        //public void RefreshVoltageRatioInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called RefreshVoltageRatioInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            switch (shpc.HubPort)
            {
                case 0:
                    await RefreshVoltageRatioInput(VoltageRatioInput0);
                    break;

                case 1:
                    await RefreshVoltageRatioInput(VoltageRatioInput1);
                    break;

                case 2:
                    await RefreshVoltageRatioInput(VoltageRatioInput2);
                    break;

                case 3:
                    await RefreshVoltageRatioInput(VoltageRatioInput3);
                    break;

                case 4:
                    await RefreshVoltageRatioInput(VoltageRatioInput4);
                    break;

                case 5:
                    await RefreshVoltageRatioInput(VoltageRatioInput5);
                    break;
            }

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RefreshVoltageRatioInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RefreshVoltageRatioInputEvent>().Publish(
            //      new RefreshVoltageRatioInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class RefreshVoltageRatioInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RefreshVoltageRatioInputEvent>().Subscribe(RefreshVoltageRatioInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private async Task RefreshVoltageRatioInput(VoltageRatioInputEx voltageInput)
        {
            if (voltageInput.IsOpen is true)
            {
                await Task.Run(() => voltageInput.RefreshProperties());
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{voltageInput} not open", Common.LOG_CATEGORY);
            }
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean RefreshVoltageRatioInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean RefreshVoltageRatioInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageRatioInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageRatioInputChannels.TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CloseVoltageRatioInput Command

        public DelegateCommand<SerialHubPortChannel?> CloseVoltageRatioInputCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _CloseVoltageRatioInputHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE CloseVoltageRatioInputCommandParameter;
        public string CloseVoltageRatioInputContent { get; set; } = "Close";
        public string CloseVoltageRatioInputToolTip { get; set; } = "Close VoltageRatioInput";

        // Can get fancy and use Resources
        //public string CloseVoltageRatioInputContent { get; set; } = "ViewName_CloseVoltageRatioInputContent";
        //public string CloseVoltageRatioInputToolTip { get; set; } = "ViewName_CloseVoltageRatioInputContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_CloseVoltageRatioInputContent">CloseVoltageRatioInput</system:String>
        //    <system:String x:Key="ViewName_CloseVoltageRatioInputContentToolTip">CloseVoltageRatioInput ToolTip</system:String>  

        // If using CommandParameter, figure out TYPE here
        public async void CloseVoltageRatioInput(SerialHubPortChannel? serialHubPortChannel)
        //public void CloseVoltageRatioInput()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);
            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called CloseVoltageRatioInput on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            await Task.Run(() => Common.PhidgetDeviceLibrary.VoltageRatioInputChannels[shpc].Close());

            OpenVoltageRatioInputCommand.RaiseCanExecuteChanged();
            RefreshVoltageRatioInputCommand.RaiseCanExecuteChanged();
            CloseVoltageRatioInputCommand.RaiseCanExecuteChanged();

            // If launching a UserControl

            // if (_CloseVoltageRatioInputHost is null) _CloseVoltageRatioInputHost = new WindowHost();
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

            // Common.EventAggregator.GetEvent<CloseVoltageRatioInputEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<CloseVoltageRatioInputEvent>().Publish(
            //      new CloseVoltageRatioInputEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class CloseVoltageRatioInputEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<CloseVoltageRatioInputEvent>().Subscribe(CloseVoltageRatioInput);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean CloseVoltageRatioInputCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean CloseVoltageRatioInputCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.

            if (serialHubPortChannel is null) return false;

            VoltageRatioInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageRatioInputChannels
                .TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region RaisePerformanceEvent Command

        public DelegateCommand<SerialHubPortChannel?> RaisePerformanceEventCommand { get; set; }
        // If displaying UserControl
        // public static WindowHost _RaisePerformanceEventHost = null;

        // If using CommandParameter, figure out TYPE here
        //public TYPE RaisePerformanceEventCommandParameter;

        public string RaisePerformanceEventContent { get; set; } = "Raise Event";
        public string RaisePerformanceEventToolTip { get; set; } = "Raise Performance Event";

        // Can get fancy and use Resources
        //public string RaisePerformanceEventContent { get; set; } = "ViewName_RaisePerformanceEventContent";
        //public string RaisePerformanceEventToolTip { get; set; } = "ViewName_RaisePerformanceEventContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_RaisePerformanceEventContent">RaisePerformanceEvent</system:String>
        //    <system:String x:Key="ViewName_RaisePerformanceEventContentToolTip">RaisePerformanceEvent ToolTip</system:String>  

        private async Task RaisePerformanceEvent(VoltageInputEx voltageInput)
        {
            if (voltageInput.IsOpen is true)
            {
                await Task.Run(() => voltageInput.RaisePlayPerformanceEvent());
            }
            else
            {
                if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER($"{voltageInput} not open", Common.LOG_CATEGORY);
            }
        }

        // If using CommandParameter, figure out TYPE here
        public async void RaisePerformanceEvent(SerialHubPortChannel? serialHubPortChannel)
        //public void RaisePerformanceEvent()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.EventHandler) startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Do something amazing.

            SerialHubPortChannel shpc = (SerialHubPortChannel)serialHubPortChannel;

            Message = $"Cool, you called RaisePerformanceEvent\r\n on " +
                $"serialHubPortChannel:{shpc.SerialNumber}" +
                $":{shpc.HubPort}:{shpc.Channel}";
            PublishStatusMessage(Message);

            switch (shpc.HubPort)
            {
                case 0:
                    await RaisePerformanceEvent(VoltageInput0);
                    break;

                case 1:
                    await RaisePerformanceEvent(VoltageInput1);
                    break;

                case 2:
                    await RaisePerformanceEvent(VoltageInput2);
                    break;

                case 3:
                    await RaisePerformanceEvent(VoltageInput3);
                    break;

                case 4:
                    await RaisePerformanceEvent(VoltageInput4);
                    break;

                case 5:
                    await RaisePerformanceEvent(VoltageInput5);
                    break;

                case 6:
                    await RaisePerformanceEvent(VoltageInput6);
                    break;

                case 7:
                    await RaisePerformanceEvent(VoltageInput7);
                    break;

                case 8:
                    await RaisePerformanceEvent(VoltageInput8);
                    break;

                case 9:
                    await RaisePerformanceEvent(VoltageInput9);
                    break;

                case 10:
                    await RaisePerformanceEvent(VoltageInput10);
                    break;

                case 11:
                    await RaisePerformanceEvent(VoltageInput11);
                    break;

                case 12:
                    await RaisePerformanceEvent(VoltageInput12);
                    break;

                case 13:
                    await RaisePerformanceEvent(VoltageInput13);
                    break;

                case 14:
                    await RaisePerformanceEvent(VoltageInput14);
                    break;

                case 15:
                    await RaisePerformanceEvent(VoltageInput15);
                    break;
            }
            //}
            //else
            //{

            //}

            // Uncomment this if you are telling someone else to handle this

            // Common.EventAggregator.GetEvent<RaisePerformanceEventEvent>().Publish();

            // May want EventArgs

            //  EventAggregator.GetEvent<RaisePerformanceEventEvent>().Publish(
            //      new RaisePerformanceEventEventArgs()
            //      {
            //            Organization = _collectionMainViewModel.SelectedCollection.Organization,
            //            Process = _contextMainViewModel.Context.SelectedProcess
            //      });

            // Start Cut Four - Put this in PrismEvents

            // public class RaisePerformanceEventEvent : PubSubEvent { }

            // End Cut Four

            // Start Cut Five - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<RaisePerformanceEventEvent>().Subscribe(RaisePerformanceEvent);

            // End Cut Five

            if (Common.VNCLogging.EventHandler) Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // If using CommandParameter, figure out TYPE and fix above
        public Boolean RaisePerformanceEventCanExecute(SerialHubPortChannel? serialHubPortChannel)
        //public Boolean RaisePerformanceEventCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            //Int32 channel = (Int32)channelNumber;

            ////if (!Int32.TryParse(channelNumber, out channel)) throw new Exception($"Cannot parse channelNumber:{channelNumber}");

            //if (SelectedVINTHubPhidget is null) return false;

            //SerialHubPortChannel serialHubPortChannel = new SerialHubPortChannel() { SerialNumber = (Int32)SelectedVINTHubPhidget, Channel = channel };

            if (serialHubPortChannel is null) return false;

            VoltageInputEx? host;

            if (!Common.PhidgetDeviceLibrary.VoltageInputChannels.TryGetValue((SerialHubPortChannel)serialHubPortChannel, out host)) return false;

            if (host.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Public Methods (none)



        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods (none)



        #endregion

        #region IInstanceCount

        private static Int32 _instanceCountVM;

        public Int32 InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
