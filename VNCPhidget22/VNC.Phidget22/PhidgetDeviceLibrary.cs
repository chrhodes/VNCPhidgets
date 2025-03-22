using System;
using System.Collections.Generic;
using System.Threading;

using Prism.Events;

using VNC.Phidget22.Configuration;
using VNCPhidgetConfig = VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;

using PhidgetEvents = Phidget22.Events;
using Phidgets = Phidget22;
using VNC.Phidget22.Configuration.Performance;
using Prism.Regions.Behaviors;
using Phidget22;
using System.Diagnostics.CodeAnalysis;

namespace VNC.Phidget22
{
    // TODO(crhodes)
    // Make this a Singleton

    /// <summary>
    /// Builds a PhidgetDevice Dictionary using the Phidgets.Manager class
    /// Manually adds Servers(Hosts) based on entries in Hosts config file
    /// </summary>
    public class PhidgetDeviceLibrary
    {
        #region Constructors, Initialization, and Load

        private readonly IEventAggregator _eventAggregator;

        public PhidgetDeviceLibrary(IEventAggregator eventAggregator)
        //public PhidgetDeviceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            _eventAggregator = eventAggregator;
            BuildPhidgetDeviceDictionary();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void BuildPhidgetDeviceDictionary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ApplicationInitialize) startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // This is for Excel use.  It needs to match what is in Manager_Attach()
            // See Manager_Attach for column choice comments
            if (Common.VNCLogging.ApplicationInitialize) Log.EVENT_HANDLER($"|ServerPeerName" +
                //$"|ServerHostName" +
                $"|DeviceSerialNumber" +
                $"|IsLocal|IsRemote|GrandParent|Parent" +
                $"|IsHubPortDevice|HubPort" +
                //$"|HubPortCount" +
                //$"|DeviceClassName" +
                $"|DeviceClass|DeviceName" +
                //$"|DeviceSKU" +
                $"|DeviceID|DeviceVINTID|DeviceVersion" +
                $"|IsChannel|Channel|ChannelClass" +
                //$"|ChannelClassName" +
                $"|ChannelName|ChannelSubClass", Common.LOG_CATEGORY);
                //$"|DeviceFirmwareUpgradeString"


            //_availablePhidgets = new Dictionary<Int32, PhidgetDevice>();

            // NOTE(crhodes)
            // EnableServerDiscovery does not work consistently
            // Not sure why

            //Net.EnableServerDiscovery(ServerType.DeviceRemote);
            //Net.EnableServerDiscovery(ServerType.SBC);

            // So, for now, depend on Host file listing the Computers (typically SBC) hosting Phidgets

            foreach (VNCPhidgetConfig.Host host in PerformanceLibrary.Hosts)
            {
                Phidgets.Net.AddServer(host.Name, host.IPAddress, host.Port, "", 0);
            }

            Phidgets.Manager manager = new Phidgets.Manager();

            manager.Attach += Manager_Attach;
            manager.Detach += Manager_Detach;

            manager.Open();

            Thread.Sleep(1000);

            Common.PhidgetDeviceLibrary = this;

            if (Common.VNCLogging.ApplicationInitialize) Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Fields and Properties

        //public static Dictionary<Int32, PhidgetDevice> AvailablePhidget22 = new Dictionary<Int32, PhidgetDevice>();

        //public Dictionary<SerialHubChannel, PhidgetDevice> _managerAttachedPhidgetDevices = new Dictionary<SerialHubChannel, PhidgetDevice>();
        //public Dictionary<SerialHubChannel, PhidgetDevice> ManagerAttachedPhidgetDevices
        //{
        //    get => _managerAttachedPhidgetDevices;
        //}

        public List<PhidgetDevice> _managerAttachedPhidgetDevices = new List<PhidgetDevice>();
        public List<PhidgetDevice> ManagerAttachedPhidgetDevices
        {
            get => _managerAttachedPhidgetDevices;
        }

        private Dictionary<SerialHubPortChannel, AccelerometerEx> _accelerometerChannels = new Dictionary<SerialHubPortChannel, AccelerometerEx>();

        public Dictionary<SerialHubPortChannel, AccelerometerEx> AccelerometerChannels
        {
            get => _accelerometerChannels;
            set => _accelerometerChannels = value;
        }

        private Dictionary<SerialHubPortChannel, BLDCMotorEx> _bLDCMotorChannels = new Dictionary<SerialHubPortChannel, BLDCMotorEx>();

        public Dictionary<SerialHubPortChannel, BLDCMotorEx> BLDCMotorChannels
        {
            get => _bLDCMotorChannels;
            set => _bLDCMotorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, CapacitiveTouchEx> _capacitiveTouchChannels = new Dictionary<SerialHubPortChannel, CapacitiveTouchEx>();

        public Dictionary<SerialHubPortChannel, CapacitiveTouchEx> CapacitiveTouchChannels
        {
            get => _capacitiveTouchChannels;
            set => _capacitiveTouchChannels = value;
        }

        private Dictionary<SerialHubPortChannel, CurrentInputEx> _currentInputChannels = new Dictionary<SerialHubPortChannel, CurrentInputEx>();

        public Dictionary<SerialHubPortChannel, CurrentInputEx> CurrentInputChannels
        {
            get => _currentInputChannels;
            set => _currentInputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, DCMotorEx> _dCMotorChannels = new Dictionary<SerialHubPortChannel, DCMotorEx>();
        public Dictionary<SerialHubPortChannel, DCMotorEx> DCMotorChannels
        {
            get => _dCMotorChannels;
            set => _dCMotorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, DigitalInputEx> _digitalInputChannels = new Dictionary<SerialHubPortChannel, DigitalInputEx>();
        public Dictionary<SerialHubPortChannel, DigitalInputEx> DigitalInputChannels
        {
            get => _digitalInputChannels;
            set => _digitalInputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, DigitalOutputEx> _digitalOutputChannels = new Dictionary<SerialHubPortChannel, DigitalOutputEx>();

        public Dictionary<SerialHubPortChannel, DigitalOutputEx> DigitalOutputChannels
        {
            get => _digitalOutputChannels;
            set => _digitalOutputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, DistanceSensorEx> _distanceSensorChannels = new Dictionary<SerialHubPortChannel, DistanceSensorEx>();

        public Dictionary<SerialHubPortChannel, DistanceSensorEx> DistanceSensorChannels
        {
            get => _distanceSensorChannels;
            set => _distanceSensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, EncoderEx> _encoderChannels = new Dictionary<SerialHubPortChannel, EncoderEx>();
        public Dictionary<SerialHubPortChannel, EncoderEx> EncoderChannels
        {
            get => _encoderChannels;
            set => _encoderChannels = value;
        }

        private Dictionary<SerialHubPortChannel, FrequencyCounterEx> _frequencyCounterChannels = new Dictionary<SerialHubPortChannel, FrequencyCounterEx>();
        public Dictionary<SerialHubPortChannel, FrequencyCounterEx> FrequencyCounterChannels
        {
            get => _frequencyCounterChannels;
            set => _frequencyCounterChannels = value;
        }

        private Dictionary<SerialHubPortChannel, GPSEx> _gPSChannels = new Dictionary<SerialHubPortChannel, GPSEx>();
        public Dictionary<SerialHubPortChannel, GPSEx> GPSChannels
        {
            get => _gPSChannels;
            set => _gPSChannels = value;
        }

        private Dictionary<SerialHubPortChannel, GyroscopeEx> _gyroscopeChannels = new Dictionary<SerialHubPortChannel, GyroscopeEx>();
        public Dictionary<SerialHubPortChannel, GyroscopeEx> GyroscopeChannels
        {
            get => _gyroscopeChannels;
            set => _gyroscopeChannels = value;
        }

        private Dictionary<SerialHubPortChannel, HubEx> _hubChannels = new Dictionary<SerialHubPortChannel, HubEx>();
        public Dictionary<SerialHubPortChannel, HubEx> HubChannels
        {
            get => _hubChannels;
            set => _hubChannels = value;
        }

        private Dictionary<SerialHubPortChannel, HumiditySensorEx> _humiditySensorChannels = new Dictionary<SerialHubPortChannel, HumiditySensorEx>();
        public Dictionary<SerialHubPortChannel, HumiditySensorEx> HumiditySensorChannels
        {
            get => _humiditySensorChannels;
            set => _humiditySensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, IREx> _iRChannels = new Dictionary<SerialHubPortChannel, IREx>();
        public Dictionary<SerialHubPortChannel, IREx> IRChannels
        {
            get => _iRChannels;
            set => _iRChannels = value;
        }

        private Dictionary<SerialHubPortChannel, LCDEx> _lCDChannels = new Dictionary<SerialHubPortChannel, LCDEx>();
        public Dictionary<SerialHubPortChannel, LCDEx> LCDChannels
        {
            get => _lCDChannels;
            set => _lCDChannels = value;
        }

        private Dictionary<SerialHubPortChannel, LightSensorEx> _lightSensorChannels = new Dictionary<SerialHubPortChannel, LightSensorEx>();
        public Dictionary<SerialHubPortChannel, LightSensorEx> LightSensorChannels
        {
            get => _lightSensorChannels;
            set => _lightSensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, MagnetometerEx> _magnetometerChannels = new Dictionary<SerialHubPortChannel, MagnetometerEx>();
        public Dictionary<SerialHubPortChannel, MagnetometerEx> MagnetometerChannels
        {
            get => _magnetometerChannels;
            set => _magnetometerChannels = value;
        }

        private Dictionary<SerialHubPortChannel, MotorPositionControllerEx> _motorPositionControllerChannels = new Dictionary<SerialHubPortChannel, MotorPositionControllerEx>();
        public Dictionary<SerialHubPortChannel, MotorPositionControllerEx> MotorPositionControllerChannels
        {
            get => _motorPositionControllerChannels;
            set => _motorPositionControllerChannels = value;
        }

        private Dictionary<SerialHubPortChannel, PHSensorEx> _pHSensorChanels = new Dictionary<SerialHubPortChannel, PHSensorEx>();
        public Dictionary<SerialHubPortChannel, PHSensorEx> PHSensorChanels
        {
            get => _pHSensorChanels;
            set => _pHSensorChanels = value;
        }

        private Dictionary<SerialHubPortChannel, PowerGuardEx> _powerGuardChannels = new Dictionary<SerialHubPortChannel, PowerGuardEx>();
        public Dictionary<SerialHubPortChannel, PowerGuardEx> PowerGuardChannels
        {
            get => _powerGuardChannels;
            set => _powerGuardChannels = value;
        }

        private Dictionary<SerialHubPortChannel, PressureSensorEx> _pressureSensorChannels = new Dictionary<SerialHubPortChannel, PressureSensorEx>();
        public Dictionary<SerialHubPortChannel, PressureSensorEx> PressureSensorChannels
        {
            get => _pressureSensorChannels;
            set => _pressureSensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, RCServoEx> _rCServoChannels = new Dictionary<SerialHubPortChannel, RCServoEx>();
        public Dictionary<SerialHubPortChannel, RCServoEx> RCServoChannels
        {
            get => _rCServoChannels;
            set => _rCServoChannels = value;
        }

        private Dictionary<SerialHubPortChannel, ResistanceInputEx> _resistanceInputChannels = new Dictionary<SerialHubPortChannel, ResistanceInputEx>();
        public Dictionary<SerialHubPortChannel, ResistanceInputEx> ResistanceInputChannels
        {
            get => _resistanceInputChannels;
            set => _resistanceInputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, RFIDEx> _rFIDChannels = new Dictionary<SerialHubPortChannel, RFIDEx>();
        public Dictionary<SerialHubPortChannel, RFIDEx> RFIDChannels
        {
            get => _rFIDChannels;
            set => _rFIDChannels = value;
        }

        private Dictionary<SerialHubPortChannel, SoundSensorEx> _soundSensorChannels = new Dictionary<SerialHubPortChannel, SoundSensorEx>();
        public Dictionary<SerialHubPortChannel, SoundSensorEx> SoundSensorChannels
        {
            get => _soundSensorChannels;
            set => _soundSensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, SpatialEx> _spatialChannels = new Dictionary<SerialHubPortChannel, SpatialEx>();
        public Dictionary<SerialHubPortChannel, SpatialEx> SpatialChannels
        {
            get => _spatialChannels;
            set => _spatialChannels = value;
        }

        private Dictionary<SerialHubPortChannel, StepperEx> _stepperChannels = new Dictionary<SerialHubPortChannel, StepperEx>();
        public Dictionary<SerialHubPortChannel, StepperEx> StepperChannels
        {
            get => _stepperChannels;
            set => _stepperChannels = value;
        }

        private Dictionary<SerialHubPortChannel, TemperatureSensorEx> _temperatureSensorChannels = new Dictionary<SerialHubPortChannel, TemperatureSensorEx>();
        public Dictionary<SerialHubPortChannel, TemperatureSensorEx> TemperatureSensorChannels
        {
            get => _temperatureSensorChannels;
            set => _temperatureSensorChannels = value;
        }

        private Dictionary<SerialHubPortChannel, VoltageInputEx> _voltageInputChannels = new Dictionary<SerialHubPortChannel, VoltageInputEx>();
        public Dictionary<SerialHubPortChannel, VoltageInputEx> VoltageInputChannels
        {
            get => _voltageInputChannels;
            set => _voltageInputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, VoltageRatioInputEx> _voltageRatioInputChannels = new Dictionary<SerialHubPortChannel, VoltageRatioInputEx>();
        public Dictionary<SerialHubPortChannel, VoltageRatioInputEx> VoltageRatioInputChannels
        {
            get => _voltageRatioInputChannels;
            set => _voltageRatioInputChannels = value;
        }

        private Dictionary<SerialHubPortChannel, VoltageOutputEx> _voltageOutputChannels = new Dictionary<SerialHubPortChannel, VoltageOutputEx>();
        public Dictionary<SerialHubPortChannel, VoltageOutputEx> VoltageOutputChannels
        {
            get => _voltageOutputChannels;
            set => _voltageOutputChannels = value;
        }
        
        // TODO(crhodes)
        // Populate this from ConfigFile
        // Also used info that came from Phidgets.  It is in OneNote

        public static Dictionary<RCServoType, RCServoPulseWidths> RCServoTypes = new Dictionary<RCServoType, RCServoPulseWidths>()
        {
            [VNC.Phidget22.Configuration.RCServoType.DEFAULT] = new RCServoPulseWidths
            {
                RCServoType = VNC.Phidget22.Configuration.RCServoType.DEFAULT,
                MinPulseWidth = 245,
                MaxPulseWidth = 2592
            },
            [VNC.Phidget22.Configuration.RCServoType.HITEC_HS422] = new RCServoPulseWidths
            {
                RCServoType = VNC.Phidget22.Configuration.RCServoType.HITEC_HS422,
                MinPulseWidth = 650,
                MaxPulseWidth = 2450
            },
            [VNC.Phidget22.Configuration.RCServoType.SG90] = new RCServoPulseWidths
            {
                RCServoType = VNC.Phidget22.Configuration.RCServoType.SG90,
                MinPulseWidth = 650,
                MaxPulseWidth = 2450
            },
            [VNC.Phidget22.Configuration.RCServoType.USER_DEFINED] = new RCServoPulseWidths
            {
                RCServoType = VNC.Phidget22.Configuration.RCServoType.USER_DEFINED,
                MinPulseWidth = 1000,
                MaxPulseWidth = 1001
            }
        };

        public Boolean LogPhidgetEvents { get; set; } = true;

        #endregion

        #region Event Handlers

        private void Manager_Attach(object sender, PhidgetEvents.ManagerAttachEventArgs e)
        {
            var maea = e;
            var phidget = e.Channel;

            try
            {
                if (Common.VNCLogging.ApplicationInitialize) Log.EVENT_HANDLER($"|{phidget.ServerPeerName}" + // IPAddress,Port
                    //$"|{phidget.ServerHostname}" + // IPAddress
                    $"|{phidget.DeviceSerialNumber}" +
                    $"|{phidget.IsLocal}|{phidget.IsRemote}|{phidget.Parent?.Parent}|{phidget.Parent}" +
                    $"|{phidget.IsHubPortDevice}|{phidget.HubPort}" +
                    //$"|{phidget.HubPortCount}" +      // Throws exception if not Hub
                    $"|{phidget.DeviceClass}|{phidget.DeviceName}" +//
                    //$"|{phidget.DeviceClassName}" + // Just adds "Phidget" to DeviceClass, e.g. InterfaceKit -> PhidgetInterfaceKit
                    //$"|{phidget.DeviceSKU}" +  // Similar to DeviceID
                    $"|{phidget.DeviceID}|{phidget.DeviceVINTID}|{phidget.DeviceVersion}" +
                    $"|{phidget.IsChannel}|{phidget.Channel}|{phidget.ChannelClass}" +
                    //$"|{phidget.ChannelClassName}" +  // Just adds "Phidget" to ChannelClass, e.g. RCServo -> PhidgetRCServo
                    $"|{phidget.ChannelName}|{phidget.ChannelSubclass}", Common.LOG_CATEGORY);
                    //$"|{phidget.DeviceFirmwareUpgradeString}" // Looks like DeviceSKU

                switch (phidget.DeviceClass)
                {
                    case Phidgets.DeviceClass.Dictionary:
                        if (Common.VNCLogging.ApplicationInitialize)
                        {
                        }
                        // FIX(crhodes)
                        // Looks like each SBC has a Dictionary.
                        // If want to use/save, need to use ServerName or ServerPeerName as Key
                        // Currently AvailablePhidgets expects an Int32 as the key

                        //PhidgetDevice phidgetDevice = new PhidgetDevice(phidget.ServerPeerName, phidget.DeviceClass, phidget.DeviceSerialNumber);
                        //IncrementDeviceChannelCount(phidgetDevice, phidget.ChannelClass);
                        //AvailablePhidgets.Add(phidget.DeviceSerialNumber, phidgetDevice);
                        break;

                    case Phidgets.DeviceClass.Hub:
                        // NOTE(crhodes)
                        // 
                        // Looks like each (new) SBC has a built in Hub, DeviceID:PN_HUB0004
                        // In addition, independent Hubs have a DeviceSerialNumber like a Phidget Board
                        // and are attached to a SBC

                        // Treat as a Phidget

                        AddPhidgetDevice(phidget);                            
                        break;

                    case Phidgets.DeviceClass.VINT:
                        // NOTE(crhodes)
                        // 
                        // VINT's are just versital Phidgets that behave like PhidgetBoards
                        AddPhidgetDevice(phidget);
                        break;

                    default:
                        // NOTE(crhodes)
                        // For everything else assume it is a Physical Phidget with a SerialNumber

                        AddPhidgetDevice(phidget);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void AddPhidgetDevice(Phidgets.Phidget phidget)
        { 
            PhidgetDevice phidgetDevice = new PhidgetDevice(phidget.ServerPeerName, phidget.DeviceSerialNumber);

            phidgetDevice.IsLocal = phidget.IsLocal;
            phidgetDevice.IsRemote = phidget.IsRemote;
            phidgetDevice.GrandParent = phidget.Parent?.Parent?.ToString();
            phidgetDevice.Parent = phidget.Parent.ToString();

            phidgetDevice.IsHubPortDevice = phidget.IsHubPortDevice;
            phidgetDevice.HubPort = phidget.HubPort;

            phidgetDevice.DeviceClass = phidget.DeviceClass.ToString();
            phidgetDevice.DeviceName = phidget.DeviceName;
            phidgetDevice.DeviceID = phidget.DeviceID.ToString();
            phidgetDevice.DeviceVINTID = phidget.DeviceVINTID.ToString();
            phidgetDevice.DeviceVersion = phidget.DeviceVersion.ToString();

            phidgetDevice.IsChannel = phidget.IsChannel;
            phidgetDevice.Channel = phidget.Channel;
            phidgetDevice.ChannelClass = phidget.ChannelClass.ToString();
            phidgetDevice.ChannelName = phidget.ChannelName.ToString();
            phidgetDevice.ChannelSubclass = phidget.ChannelSubclass.ToString();

            // NOTE(crhodes)
            // Switch from Dictionary to List
            // Same Host can have different DeviceClasses at same SerialNumber, HubPort, Channel

            //ManagerAttachedPhidgetDevices.Add(serialHubChannel, phidgetDevice);

            if (Common.VNCLogging.ApplicationInitializeLow) Log.EVENT_HANDLER($"Adding AttachedPhidget {phidget.ServerPeerName}|{phidget.DeviceClass}|{phidget.DeviceSerialNumber}", Common.LOG_CATEGORY);

            ManagerAttachedPhidgetDevices.Add(phidgetDevice);


            // NOTE(crhodes)
            // Now add the Phidget to ChannelClass specific Dictionaries
            // that are used to find the right Phidget to use

            AddDeviceChannel(phidget, phidgetDevice);
        }
        void AddDeviceChannel(Phidgets.Phidget phidget, PhidgetDevice phidgetDevice)
        {
            SerialHubPortChannel serialHubChannel = new SerialHubPortChannel()
            {
                SerialNumber = phidget.DeviceSerialNumber,
                HubPort = phidget.HubPort,
                Channel = phidget.Channel
            };

            var channelClass = phidget.ChannelClass;

            switch (phidget.ChannelClass)
            {
                case Phidgets.ChannelClass.Accelerometer:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.BLDCMotor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.CapacitiveTouch:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.CurrentInput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    
                    CurrentInputChannels.Add(
                        serialHubChannel,
                        new CurrentInputEx(
                            new CurrentInputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    phidgetDevice.DeviceChannels.CurrentInputCount++;

                    break;

                case Phidgets.ChannelClass.DCMotor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Dictionary:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.DigitalInput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);

                    DigitalInputChannels.Add(
                        serialHubChannel,
                        new DigitalInputEx(
                            new DigitalInputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.DigitalOutput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);

                    DigitalOutputChannels.Add(
                        serialHubChannel,
                        new DigitalOutputEx(
                             new DigitalOutputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.DistanceSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Encoder:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.FirmwareUpgrade:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.FrequencyCounter:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Generic:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                       $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.GPS:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Gyroscope:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Hub:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);

                    HubChannels.Add(
                        serialHubChannel,
                        new HubEx(
                            new HubConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.HumiditySensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.IR:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.LCD:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.LightSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Magnetometer:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                      $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.None:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.PHSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.PowerGuard:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.PressureSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.RCServo:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);

                    RCServoChannels.Add(
                        serialHubChannel,
                        new RCServoEx(
                            //phidget.DeviceSerialNumber,
                            new RCServoConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.ResistanceInput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.RFID:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.SoundSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Spatial:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.Stepper:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    
                    StepperChannels.Add(
                        serialHubChannel,
                        new StepperEx(
                            phidget.DeviceSerialNumber,
                            new StepperConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    phidgetDevice.DeviceChannels.StepperCount++;

                    break;

                case Phidgets.ChannelClass.TemperatureSensor:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    break;

                case Phidgets.ChannelClass.VoltageInput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    
                    VoltageInputChannels.Add(
                        serialHubChannel,
                        new VoltageInputEx(
                            new VoltageInputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.VoltageRatioInput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    
                    VoltageRatioInputChannels.Add(
                        serialHubChannel,
                        new VoltageRatioInputEx(
                            new VoltageRatioInputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                case Phidgets.ChannelClass.VoltageOutput:
                    if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new {channelClass}" +
                        $" SerialNumber:{serialHubChannel.SerialNumber} HubPort:{serialHubChannel.HubPort} Channel:{serialHubChannel.Channel}", Common.LOG_CATEGORY);
                    
                    VoltageOutputChannels.Add(
                        serialHubChannel,
                        new VoltageOutputEx(
                            new VoltageOutputConfiguration()
                            {
                                DeviceSerialNumber = phidget.DeviceSerialNumber,
                                HubPortDevice = phidget.IsHubPortDevice,
                                HubPort = phidget.HubPort,
                                Channel = phidget.Channel
                            },
                            _eventAggregator
                        )
                    );

                    break;

                default:
                    Log.Error($"Unexpected ChannelClass:>{channelClass}<", Common.LOG_CATEGORY);
                    break;
            }
        }

        //void IncrementDeviceChannelCount(PhidgetDevice phidgetDevice, Phidgets.ChannelClass channelClass)
        //{
        //    var deviceChannels = phidgetDevice.DeviceChannels;

        //    switch (channelClass)
        //    {
        //        case Phidgets.ChannelClass.Accelerometer:
        //            deviceChannels.AccelerometerCount++;
        //            break;

        //        case Phidgets.ChannelClass.BLDCMotor:
        //            deviceChannels.BLDCMotorCount++;
        //            break;

        //        case Phidgets.ChannelClass.CapacitiveTouch:
        //            deviceChannels.CapacitiveTouchCount++;
        //            break;

        //        case Phidgets.ChannelClass.CurrentInput:
        //            deviceChannels.CurrentInputCount++;
        //            break;

        //        case Phidgets.ChannelClass.DCMotor:
        //            deviceChannels.DCMotorCount++;
        //            break;

        //        case Phidgets.ChannelClass.Dictionary:
        //            deviceChannels.DictionaryCount++;
        //            break;

        //        case Phidgets.ChannelClass.DigitalInput:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new DigitalInput" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.DigitalInputCount}", Common.LOG_CATEGORY);

        //            DigitalInputChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.DigitalInputCount
        //                },
        //                new DigitalInputEx(phidgetDevice.SerialNumber,
        //                    new DigitalInputConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.DigitalInputCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.DigitalInputCount++;
        //            break;

        //        case Phidgets.ChannelClass.DigitalOutput:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new DigitalOutput" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.DigitalOutputCount}", Common.LOG_CATEGORY);

        //            DigitalOutputChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.DigitalOutputCount
        //                },
        //                new DigitalOutputEx(phidgetDevice.SerialNumber,
        //                    new DigitalOutputConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.DigitalOutputCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.DigitalOutputCount++;
        //            break;

        //        case Phidgets.ChannelClass.DistanceSensor:
        //            deviceChannels.DistanceSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.Encoder:
        //            deviceChannels.EncoderCount++;
        //            break;

        //        case Phidgets.ChannelClass.FirmwareUpgrade:
        //            deviceChannels.FirmwareUpgradeCount++;
        //            break;

        //        case Phidgets.ChannelClass.FrequencyCounter:
        //            deviceChannels.FrequencyCounterCount++;
        //            break;

        //        case Phidgets.ChannelClass.Generic:
        //            deviceChannels.GenericCount++;
        //            break;

        //        case Phidgets.ChannelClass.GPS:
        //            deviceChannels.GPSCount++;
        //            break;

        //        case Phidgets.ChannelClass.Gyroscope:
        //            deviceChannels.GyroscopeCount++;
        //            break;

        //        case Phidgets.ChannelClass.Hub:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new Hub" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.HubCount}", Common.LOG_CATEGORY);

        //            HubChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.HubCount
        //                },
        //                new HubEx(phidgetDevice.SerialNumber,
        //                    new HubConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.HubCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );

        //            deviceChannels.HubCount++;
        //            break;

        //        case Phidgets.ChannelClass.HumiditySensor:
        //            deviceChannels.HumiditySensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.IR:
        //            deviceChannels.IRCount++;
        //            break;

        //        case Phidgets.ChannelClass.LCD:
        //            deviceChannels.LCDCount++;
        //            break;

        //        case Phidgets.ChannelClass.LightSensor:
        //            deviceChannels.LightSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.Magnetometer:
        //            deviceChannels.MagnetometerCount++;
        //            break;
        //        case Phidgets.ChannelClass.None:
        //            deviceChannels.None++;
        //            break;

        //        case Phidgets.ChannelClass.PHSensor:
        //            deviceChannels.PHSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.PowerGuard:
        //            deviceChannels.PowerGuardCount++;
        //            break;

        //        case Phidgets.ChannelClass.PressureSensor:
        //            deviceChannels.PressureSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.RCServo:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new RCServoChannel" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.RCServoCount}", Common.LOG_CATEGORY);

        //            RCServoChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.RCServoCount
        //                },
        //                new RCServoEx(phidgetDevice.SerialNumber,
        //                    new RCServoConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.RCServoCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.RCServoCount++;
        //            break;

        //        case Phidgets.ChannelClass.ResistanceInput:
        //            deviceChannels.ResistanceInputCount++;
        //            break;

        //        case Phidgets.ChannelClass.RFID:
        //            deviceChannels.RFIDCount++;
        //            break;

        //        case Phidgets.ChannelClass.SoundSensor:
        //            deviceChannels.SoundSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.Spatial:
        //            deviceChannels.SpatialCount++;
        //            break;

        //        case Phidgets.ChannelClass.Stepper:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new Stepper" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.StepperCount}", Common.LOG_CATEGORY);
        //            StepperChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.StepperCount
        //                },
        //                new StepperEx(phidgetDevice.SerialNumber,
        //                    new StepperConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.StepperCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.StepperCount++;
        //            break;

        //        case Phidgets.ChannelClass.TemperatureSensor:
        //            deviceChannels.TemperatureSensorCount++;
        //            break;

        //        case Phidgets.ChannelClass.VoltageInput:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new VoltageInput" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageInputCount}", Common.LOG_CATEGORY);
        //            VoltageInputChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.VoltageInputCount
        //                },
        //                new VoltageInputEx(phidgetDevice.SerialNumber,
        //                    new VoltageInputConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.VoltageInputCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.VoltageInputCount++;
        //            break;

        //        case Phidgets.ChannelClass.VoltageRatioInput:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new VoltageRatioInput" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageRatioInputCount}", Common.LOG_CATEGORY);
        //            VoltageRatioInputChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.VoltageRatioInputCount
        //                },
        //                new VoltageRatioInputEx(phidgetDevice.SerialNumber,
        //                    new VoltageRatioInputConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.VoltageRatioInputCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.VoltageRatioInputCount++;
        //            break;

        //        case Phidgets.ChannelClass.VoltageOutput:
        //            if (Common.VNCLogging.ApplicationInitializeLow) Log.Trace($"Adding new VoltageOutput" +
        //                $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageOutputCount}", Common.LOG_CATEGORY);
        //            VoltageOutputChannels.Add(
        //                new SerialHubChannel()
        //                {
        //                    SerialNumber = phidgetDevice.SerialNumber,
        //                    Channel = deviceChannels.VoltageOutputCount
        //                },
        //                new VoltageOutputEx(phidgetDevice.SerialNumber,
        //                    new VoltageOutputConfiguration()
        //                    {
        //                        HostComputer = phidgetDevice.HostComputer,
        //                        Channel = deviceChannels.VoltageOutputCount
        //                    },
        //                    _eventAggregator
        //                )
        //            );
        //            deviceChannels.VoltageOutputCount++;
        //            break;

        //        default:
        //            Log.Error($"Unexpected ChannelClass:{channelClass}", Common.LOG_CATEGORY);
        //            break;
        //    }

        //    phidgetDevice.DeviceChannels = deviceChannels;
        //}

        private void Manager_Detach(object sender, PhidgetEvents.ManagerDetachEventArgs e)
        {
            if (LogPhidgetEvents)
            {
                try
                {
                    Log.EVENT_HANDLER($"Manager_Detach: {e.Channel}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        #endregion

        #region Commands (none)



        #endregion

        #region Public Methods (none)

        /// <summary>
        /// Returns Open RCServoEx channel hosted by serialNumber
        /// configured based on configuration
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="channel"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public RCServoEx OpenRCServoHost(Int32 serialNumber, Int32 channel, RCServoConfiguration configuration)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);


        //    //PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

        //    SerialChannel serialHubPortChannel = 
        //        new SerialChannel() 
        //            { 
        //                SerialNumber = serialNumber, 
        //                Channel = channel 
        //            };

        //    // TODO(crhodes)
        //    // Is this where we do this?

        //    RCServoEx rcServoHost = RCServoChannels[serialHubPortChannel];

        //    //RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.OpenRCServoHost(serialNumber, channel, rcServoConfiguration);

        //    if (rcServoHost is null) throw new Exception($"Cannot find RCServoChannel for serialNumber:{serialNumber} channel:{channel}");

        //    // TODO(crhodes)
        //    // Maybe RCServoEx has a RCServoConfiguration property that we just set

        //    rcServoHost.LogPhidgetEvents = configuration.LogPhidgetEvents;
        //    rcServoHost.LogErrorEvents = configuration.LogErrorEvents;
        //    rcServoHost.LogPropertyChangeEvents = configuration.LogPropertyChangeEvents;

        //    //rcServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
        //    rcServoHost.LogPositionChangeEvents = configuration.LogPositionChangeEvents;
        //    rcServoHost.LogVelocityChangeEvents = configuration.LogVelocityChangeEvents;

        //    rcServoHost.LogTargetPositionReachedEvents = configuration.LogTargetPositionReachedEvents;

        //    rcServoHost.LogDeviceChannelSequence = configuration.LogDeviceChannelSequence;
        //    rcServoHost.LogChannelAction = configuration.LogChannelAction;
        //    rcServoHost.LogActionVerification = configuration.LogActionVerification;

        //    if (rcServoHost.Attached is false)
        //    {
        //        rcServoHost.Open();
        //    }

        //    if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

        //    return rcServoHost;
        //}

        #endregion

        #region Protected Methods (none)



        #endregion

        #region Private Methods (none)



        #endregion
    }
}
