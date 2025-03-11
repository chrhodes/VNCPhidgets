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

        public Dictionary<Int32, PhidgetDevice> _availablePhidgets = new Dictionary<Int32, PhidgetDevice>();
        public Dictionary<Int32, PhidgetDevice> AvailablePhidgets
        {
            get
            {
                //if (_availablePhidgets is null)
                //{
                //    BuildPhidgetDeviceDictionary();
                //}
                return _availablePhidgets;
            }
            set
            {

            }
        }


        private Dictionary<SerialChannel, AccelerometerEx> _accelerometerChannels = new Dictionary<SerialChannel, AccelerometerEx>();

        public Dictionary<SerialChannel, AccelerometerEx> AccelerometerChannels
        {
            get => _accelerometerChannels;
            set => _accelerometerChannels = value;
        }

        private Dictionary<SerialChannel, BLDCMotorEx> _bLDCMotorChannels = new Dictionary<SerialChannel, BLDCMotorEx>();

        public Dictionary<SerialChannel, BLDCMotorEx> BLDCMotorChannels
        {
            get => _bLDCMotorChannels;
            set => _bLDCMotorChannels = value;
        }

        private Dictionary<SerialChannel, CapacitiveTouchEx> _capacitiveTouchChannels = new Dictionary<SerialChannel, CapacitiveTouchEx>();

        public Dictionary<SerialChannel, CapacitiveTouchEx> CapacitiveTouchChannels
        {
            get => _capacitiveTouchChannels;
            set => _capacitiveTouchChannels = value;
        }

        private Dictionary<SerialChannel, DCMotorEx> _dCMotorChannels = new Dictionary<SerialChannel, DCMotorEx>();
        public Dictionary<SerialChannel, DCMotorEx> DCMotorChannels
        {
            get => _dCMotorChannels;
            set => _dCMotorChannels = value;
        }

        private Dictionary<SerialChannel, DigitalInputEx> _digitalInputChannels = new Dictionary<SerialChannel, DigitalInputEx>();
        public Dictionary<SerialChannel, DigitalInputEx> DigitalInputChannels
        {
            get => _digitalInputChannels;
            set => _digitalInputChannels = value;
        }

        private Dictionary<SerialChannel, DigitalOutputEx> _digitalOutputChannels = new Dictionary<SerialChannel, DigitalOutputEx>();

        public Dictionary<SerialChannel, DigitalOutputEx> DigitalOutputChannels
        {
            get => _digitalOutputChannels;
            set => _digitalOutputChannels = value;
        }

        private Dictionary<SerialChannel, DistanceSensorEx> _distanceSensorChannels = new Dictionary<SerialChannel, DistanceSensorEx>();

        public Dictionary<SerialChannel, DistanceSensorEx> DistanceSensorChannels
        {
            get => _distanceSensorChannels;
            set => _distanceSensorChannels = value;
        }

        private Dictionary<SerialChannel, EncoderEx> _encoderChannels = new Dictionary<SerialChannel, EncoderEx>();
        public Dictionary<SerialChannel, EncoderEx> EncoderChannels
        {
            get => _encoderChannels;
            set => _encoderChannels = value;
        }

        private Dictionary<SerialChannel, FrequencyCounterEx> _frequencyCounterChannels = new Dictionary<SerialChannel, FrequencyCounterEx>();
        public Dictionary<SerialChannel, FrequencyCounterEx> FrequencyCounterChannels
        {
            get => _frequencyCounterChannels;
            set => _frequencyCounterChannels = value;
        }

        private Dictionary<SerialChannel, GPSEx> _gPSChannels = new Dictionary<SerialChannel, GPSEx>();
        public Dictionary<SerialChannel, GPSEx> GPSChannels
        {
            get => _gPSChannels;
            set => _gPSChannels = value;
        }

        private Dictionary<SerialChannel, GyroscopeEx> _gyroscopeChannels = new Dictionary<SerialChannel, GyroscopeEx>();
        public Dictionary<SerialChannel, GyroscopeEx> GyroscopeChannels
        {
            get => _gyroscopeChannels;
            set => _gyroscopeChannels = value;
        }

        private Dictionary<SerialChannel, HubEx> _hubChannels = new Dictionary<SerialChannel, HubEx>();
        public Dictionary<SerialChannel, HubEx> HubChannels
        {
            get => _hubChannels;
            set => _hubChannels = value;
        }

        private Dictionary<SerialChannel, HumiditySensorEx> _humiditySensorChannels = new Dictionary<SerialChannel, HumiditySensorEx>();
        public Dictionary<SerialChannel, HumiditySensorEx> HumiditySensorChannels
        {
            get => _humiditySensorChannels;
            set => _humiditySensorChannels = value;
        }

        private Dictionary<SerialChannel, IREx> _iRChannels = new Dictionary<SerialChannel, IREx>();
        public Dictionary<SerialChannel, IREx> IRChannels
        {
            get => _iRChannels;
            set => _iRChannels = value;
        }

        private Dictionary<SerialChannel, LCDEx> _lCDChannels = new Dictionary<SerialChannel, LCDEx>();
        public Dictionary<SerialChannel, LCDEx> LCDChannels
        {
            get => _lCDChannels;
            set => _lCDChannels = value;
        }

        private Dictionary<SerialChannel, LightSensorEx> _lightSensorChannels = new Dictionary<SerialChannel, LightSensorEx>();
        public Dictionary<SerialChannel, LightSensorEx> LightSensorChannels
        {
            get => _lightSensorChannels;
            set => _lightSensorChannels = value;
        }

        private Dictionary<SerialChannel, MagnetometerEx> _magnetometerChannels = new Dictionary<SerialChannel, MagnetometerEx>();
        public Dictionary<SerialChannel, MagnetometerEx> MagnetometerChannels
        {
            get => _magnetometerChannels;
            set => _magnetometerChannels = value;
        }

        private Dictionary<SerialChannel, MotorPositionControllerEx> _motorPositionControllerChannels = new Dictionary<SerialChannel, MotorPositionControllerEx>();
        public Dictionary<SerialChannel, MotorPositionControllerEx> MotorPositionControllerChannels
        {
            get => _motorPositionControllerChannels;
            set => _motorPositionControllerChannels = value;
        }

        //public static Dictionary<SerialChannel, PressureSensorEx> PressureSensorChannels = new Dictionary<SerialChannel, PressureSensorEx>();
        //public static Dictionary<SerialChannel, RCServoEx> RCServoChannels = new Dictionary<SerialChannel, RCServoEx>();
        //public static Dictionary<SerialChannel, ResistanceInputEx> ResistanceInputChannels = new Dictionary<SerialChannel, ResistanceInputEx>();


        private Dictionary<SerialChannel, PHSensorEx> _pHSensorChanels = new Dictionary<SerialChannel, PHSensorEx>();
        public Dictionary<SerialChannel, PHSensorEx> PHSensorChanels
        {
            get => _pHSensorChanels;
            set => _pHSensorChanels = value;
        }

        private Dictionary<SerialChannel, PowerGuardEx> _powerGuardChannels = new Dictionary<SerialChannel, PowerGuardEx>();
        public Dictionary<SerialChannel, PowerGuardEx> PowerGuardChannels
        {
            get => _powerGuardChannels;
            set => _powerGuardChannels = value;
        }

        private Dictionary<SerialChannel, RCServoEx> _rCServoChannels = new Dictionary<SerialChannel, RCServoEx>();
        public Dictionary<SerialChannel, RCServoEx> RCServoChannels
        {
            get => _rCServoChannels;
            set => _rCServoChannels = value;
        }

        private Dictionary<SerialChannel, ResistanceInputEx> _resistanceInputChannels;
        public Dictionary<SerialChannel, ResistanceInputEx> ResistanceInputChannels
        {
            get => _resistanceInputChannels;
            set => _resistanceInputChannels = value;
        }

        private Dictionary<SerialChannel, RFIDEx> _rFIDChannels = new Dictionary<SerialChannel, RFIDEx>();
        public Dictionary<SerialChannel, RFIDEx> RFIDChannels
        {
            get => _rFIDChannels;
            set => _rFIDChannels = value;
        }

        private Dictionary<SerialChannel, SoundSensorEx> _soundSensorChannels = new Dictionary<SerialChannel, SoundSensorEx>();
        public Dictionary<SerialChannel, SoundSensorEx> SoundSensorChannels
        {
            get => _soundSensorChannels;
            set => _soundSensorChannels = value;
        }

        private Dictionary<SerialChannel, SpatialEx> _spatialChannels = new Dictionary<SerialChannel, SpatialEx>();
        public Dictionary<SerialChannel, SpatialEx> SpatialChannels
        {
            get => _spatialChannels;
            set => _spatialChannels = value;
        }

        private Dictionary<SerialChannel, StepperEx> _stepperChannels = new Dictionary<SerialChannel, StepperEx>();
        public Dictionary<SerialChannel, StepperEx> StepperChannels
        {
            get => _stepperChannels;
            set => _stepperChannels = value;
        }

        private Dictionary<SerialChannel, TemperatureSensorEx> _temperatureSensorChannels = new Dictionary<SerialChannel, TemperatureSensorEx>();
        public Dictionary<SerialChannel, TemperatureSensorEx> TemperatureSensorChannels
        {
            get => _temperatureSensorChannels;
            set => _temperatureSensorChannels = value;
        }

        private Dictionary<SerialChannel, VoltageInputEx> _voltageInputChannels = new Dictionary<SerialChannel, VoltageInputEx>();
        public Dictionary<SerialChannel, VoltageInputEx> VoltageInputChannels
        {
            get => _voltageInputChannels;
            set => _voltageInputChannels = value;
        }

        private Dictionary<SerialChannel, VoltageRatioInputEx> _voltageRatioInputChannels = new Dictionary<SerialChannel, VoltageRatioInputEx>();
        public Dictionary<SerialChannel, VoltageRatioInputEx> VoltageRatioInputChannels
        {
            get => _voltageRatioInputChannels;
            set => _voltageRatioInputChannels = value;
        }

        private Dictionary<SerialChannel, VoltageOutputEx> _voltageOutputChannels = new Dictionary<SerialChannel, VoltageOutputEx>();
        public Dictionary<SerialChannel, VoltageOutputEx> VoltageOutputChannels
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

        public bool LogPhidgetEvents { get; set; } = true;

        #endregion

        #region Event Handlers

        private void Manager_Attach(object sender, PhidgetEvents.ManagerAttachEventArgs e)
        {
            var maea = e;
            var channel = e.Channel;

            if (LogPhidgetEvents)
            {
                try
                {
                    if (Common.VNCLogging.ApplicationInitialize) Log.EVENT_HANDLER($"Manager_Attach: deviceClass:>{channel.DeviceClass}< channelClass:>{channel.ChannelClass}< channel:>{channel}< deviceSerialNumber:>{channel.DeviceSerialNumber}< deviceID:>{channel.DeviceID}<", Common.LOG_CATEGORY);

                    switch (channel.DeviceClass)
                    {
                        case Phidgets.DeviceClass.Dictionary:
                            if(Common.VNCLogging.ApplicationInitialize)
                            {
                                Log.EVENT_HANDLER($"parent:>{channel.Parent}<", Common.LOG_CATEGORY);

                                PhidgetDevice phidgetDevice = new PhidgetDevice(channel.ServerPeerName, channel.DeviceClass, channel.DeviceSerialNumber);
                                IncrementDeviceChannelCount(phidgetDevice, channel.ChannelClass);
                                AvailablePhidgets.Add(channel.DeviceSerialNumber, phidgetDevice);
                            }
                            break;

                        case Phidgets.DeviceClass.Hub:
                            if (Common.VNCLogging.ApplicationInitialize)
                            {
                                Log.EVENT_HANDLER($"parent:>{channel.Parent}< hubPort:{channel.HubPort}", Common.LOG_CATEGORY);

                                // FIX(crhodes)
                                // Need to figure out how to handle Hub with Ports.  Channel is alwways 0

                                //PhidgetDevice phidgetDevice = new PhidgetDevice(channel.ServerPeerName, channel.DeviceClass, channel.DeviceSerialNumber);
                                //IncrementDeviceChannelCount(phidgetDevice, channel.ChannelClass);
                                //AvailablePhidgets.Add(channel.DeviceSerialNumber, phidgetDevice);
                            }
                            break;

                        case Phidgets.DeviceClass.VINT:
                            if (Common.VNCLogging.ApplicationInitialize)
                            {
                                Log.EVENT_HANDLER($"parent:>{channel.Parent}", Common.LOG_CATEGORY);

                                PhidgetDevice phidgetDevice = new PhidgetDevice(channel.ServerPeerName, channel.DeviceClass, channel.DeviceSerialNumber);
                                IncrementDeviceChannelCount(phidgetDevice, channel.ChannelClass);
                                AvailablePhidgets.Add(channel.DeviceSerialNumber, phidgetDevice);
                            }
                            break;

                        //case DeviceClass.InterfaceKit:
                        //    DisplayChannelInfo(channel);
                        //    break;

                        // NOTE(crhodes)
                        // For everything else assume it is a Physical Phidget
                        default:
                            if (Common.VNCLogging.ApplicationInitialize)
                            {
                                Log.EVENT_HANDLER($"parent:>{channel.Parent}<", Common.LOG_CATEGORY);
                                //Log.EVENT_HANDLER($"Manager_Attach: deviceClass:{channel.DeviceClass}", Common.LOG_CATEGORY);

                                AddPhidgetDevice(channel);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void AddPhidgetDevice(Phidgets.Phidget channel)
        { 
            if (AvailablePhidgets.ContainsKey(channel.DeviceSerialNumber) == false)
            {
                PhidgetDevice phidgetDevice = new PhidgetDevice(channel.ServerPeerName, channel.DeviceClass, channel.DeviceSerialNumber);

                phidgetDevice.ChannelCount = channel.Parent.GetDeviceChannelCount(Phidgets.ChannelClass.None);

                IncrementDeviceChannelCount(phidgetDevice, channel.ChannelClass);

                AvailablePhidgets.Add(channel.DeviceSerialNumber, phidgetDevice);
            }
            else
            {
                IncrementDeviceChannelCount(AvailablePhidgets[channel.DeviceSerialNumber], channel.ChannelClass);               
            }
        }

        void IncrementDeviceChannelCount(PhidgetDevice phidgetDevice, Phidgets.ChannelClass channelClass)
        {
            var deviceChannels = phidgetDevice.DeviceChannels;

            switch (channelClass)
            {
                case Phidgets.ChannelClass.Accelerometer:
                    deviceChannels.AccelerometerCount++;
                    break;

                case Phidgets.ChannelClass.BLDCMotor:
                    deviceChannels.BLDCMotorCount++;
                    break;

                case Phidgets.ChannelClass.CapacitiveTouch:
                    deviceChannels.CapacitiveTouchCount++;
                    break;

                case Phidgets.ChannelClass.CurrentInput:
                    deviceChannels.CurrentInputCount++;
                    break;

                case Phidgets.ChannelClass.DCMotor:
                    deviceChannels.DCMotorCount++;
                    break;

                case Phidgets.ChannelClass.Dictionary:
                    deviceChannels.DictionaryCount++;
                    break;

                case Phidgets.ChannelClass.DigitalInput:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new DigitalInput" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.DigitalInputCount}", Common.LOG_CATEGORY);

                    DigitalInputChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.DigitalInputCount
                        },
                        new DigitalInputEx(phidgetDevice.SerialNumber,
                            new DigitalInputConfiguration()
                            {
                                Channel = deviceChannels.DigitalInputCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.DigitalInputCount++;
                    break;

                case Phidgets.ChannelClass.DigitalOutput:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new DigitalOutput" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.DigitalOutputCount}", Common.LOG_CATEGORY);

                    DigitalOutputChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.DigitalOutputCount
                        },
                        new DigitalOutputEx(phidgetDevice.SerialNumber,
                            new DigitalOutputConfiguration()
                            {
                                Channel = deviceChannels.DigitalOutputCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.DigitalOutputCount++;
                    break;

                case Phidgets.ChannelClass.DistanceSensor:
                    deviceChannels.DistanceSensorCount++;
                    break;

                case Phidgets.ChannelClass.Encoder:
                    deviceChannels.EncoderCount++;
                    break;

                case Phidgets.ChannelClass.FirmwareUpgrade:
                    deviceChannels.FirmwareUpgradeCount++;
                    break;

                case Phidgets.ChannelClass.FrequencyCounter:
                    deviceChannels.FrequencyCounterCount++;
                    break;

                case Phidgets.ChannelClass.Generic:
                    deviceChannels.GenericCount++;
                    break;

                case Phidgets.ChannelClass.GPS:
                    deviceChannels.GPSCount++;
                    break;

                case Phidgets.ChannelClass.Gyroscope:
                    deviceChannels.GyroscopeCount++;
                    break;

                case Phidgets.ChannelClass.Hub:
                    deviceChannels.HubCount++;
                    break;

                case Phidgets.ChannelClass.HumiditySensor:
                    deviceChannels.HumiditySensorCount++;
                    break;

                case Phidgets.ChannelClass.IR:
                    deviceChannels.IRCount++;
                    break;

                case Phidgets.ChannelClass.LCD:
                    deviceChannels.LCDCount++;
                    break;

                case Phidgets.ChannelClass.LightSensor:
                    deviceChannels.LightSensorCount++;
                    break;

                case Phidgets.ChannelClass.Magnetometer:
                    deviceChannels.MagnetometerCount++;
                    break;
                case Phidgets.ChannelClass.None:
                    deviceChannels.None++;
                    break;

                case Phidgets.ChannelClass.PHSensor:
                    deviceChannels.PHSensorCount++;
                    break;

                case Phidgets.ChannelClass.PowerGuard:
                    deviceChannels.PowerGuardCount++;
                    break;

                case Phidgets.ChannelClass.PressureSensor:
                    deviceChannels.PressureSensorCount++;
                    break;

                case Phidgets.ChannelClass.RCServo:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new RCServoChannel" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.RCServoCount}", Common.LOG_CATEGORY);

                    RCServoChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.RCServoCount
                        },
                        new RCServoEx(phidgetDevice.SerialNumber,
                            new RCServoConfiguration()
                            {
                                Channel = deviceChannels.RCServoCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.RCServoCount++;
                    break;

                case Phidgets.ChannelClass.ResistanceInput:
                    deviceChannels.ResistanceInputCount++;
                    break;

                case Phidgets.ChannelClass.RFID:
                    deviceChannels.RFIDCount++;
                    break;

                case Phidgets.ChannelClass.SoundSensor:
                    deviceChannels.SoundSensorCount++;
                    break;

                case Phidgets.ChannelClass.Spatial:
                    deviceChannels.SpatialCount++;
                    break;

                case Phidgets.ChannelClass.Stepper:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new Stepper" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.StepperCount}", Common.LOG_CATEGORY);
                    StepperChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.StepperCount
                        },
                        new StepperEx(phidgetDevice.SerialNumber,
                            new StepperConfiguration()
                            {
                                Channel = deviceChannels.StepperCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.StepperCount++;
                    break;

                case Phidgets.ChannelClass.TemperatureSensor:
                    deviceChannels.TemperatureSensorCount++;
                    break;

                case Phidgets.ChannelClass.VoltageInput:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new VoltageInput" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageInputCount}", Common.LOG_CATEGORY);
                    VoltageInputChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.VoltageInputCount
                        },
                        new VoltageInputEx(phidgetDevice.SerialNumber,
                            new VoltageInputConfiguration()
                            {
                                Channel = deviceChannels.VoltageInputCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.VoltageInputCount++;
                    break;

                case Phidgets.ChannelClass.VoltageRatioInput:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new VoltageRatioInput" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageRatioInputCount}", Common.LOG_CATEGORY);
                    VoltageRatioInputChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.VoltageRatioInputCount
                        },
                        new VoltageRatioInputEx(phidgetDevice.SerialNumber,
                            new VoltageRatioInputConfiguration()
                            {
                                Channel = deviceChannels.VoltageRatioInputCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.VoltageRatioInputCount++;
                    break;

                case Phidgets.ChannelClass.VoltageOutput:
                    if (Common.VNCLogging.ApplicationInitialize) Log.Trace($"Adding new VoltageOutput" +
                        $" SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.VoltageOutputCount}", Common.LOG_CATEGORY);
                    VoltageOutputChannels.Add(
                        new SerialChannel()
                        {
                            SerialNumber = phidgetDevice.SerialNumber,
                            Channel = deviceChannels.VoltageOutputCount
                        },
                        new VoltageOutputEx(phidgetDevice.SerialNumber,
                            new VoltageOutputConfiguration()
                            {
                                Channel = deviceChannels.VoltageOutputCount
                            },
                            _eventAggregator
                        )
                    );
                    deviceChannels.VoltageOutputCount++;
                    break;

                default:
                    Log.Error($"Unexpected ChannelClass:{channelClass}", Common.LOG_CATEGORY);
                    break;
            }

            phidgetDevice.DeviceChannels = deviceChannels;
        }

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
        //public RCServoEx OpenRCServoHost(int serialNumber, int channel, RCServoConfiguration configuration)
        //{
        //    Int64 startTicks = 0;
        //    if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);


        //    //PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

        //    SerialChannel serialChannel = 
        //        new SerialChannel() 
        //            { 
        //                SerialNumber = serialNumber, 
        //                Channel = channel 
        //            };

        //    // TODO(crhodes)
        //    // Is this where we do this?

        //    RCServoEx rcServoHost = RCServoChannels[serialChannel];

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
        //    rcServoHost.LogSequenceAction = configuration.LogSequenceAction;
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
