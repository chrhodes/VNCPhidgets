using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidgets = Phidget22;
using PhidgetEvents = Phidget22.Events;

using VNCPhidgetConfig = VNC.Phidget22.Configuration;

using System.Net;
using System.Threading.Channels;
using System.Threading;
using VNC.Phidget22.Configuration;
using VNC.Phidget22.Ex;
using Prism.Events;

namespace VNC.Phidget22
{
    // TODO(crhodes)
    // Make this a Singleton

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

            foreach (VNCPhidgetConfig.Host host in VNCPhidgetConfig.PerformanceLibrary.Hosts)
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

        public static Dictionary<SerialChannel, RCServoEx> RCServoChannels = new Dictionary<SerialChannel, RCServoEx>();

        // TODO(crhodes)
        // Populate this from ConfigFile

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
                    Log.EVENT_HANDLER($"Manager_Attach: {channel} {channel.DeviceClass} {channel.DeviceID}", Common.LOG_CATEGORY);

                    switch (channel.DeviceClass)
                    {
                        case Phidgets.DeviceClass.Dictionary:
                            Log.EVENT_HANDLER($"Manager_Attach: {e} {e.Channel}", Common.LOG_CATEGORY);
                            Log.EVENT_HANDLER($"{channel.DeviceClass} parent:{channel.Parent}", Common.LOG_CATEGORY);
                            break;

                        case Phidgets.DeviceClass.Hub:
                            Log.EVENT_HANDLER($"Manager_Attach: {e} {e.Channel}", Common.LOG_CATEGORY);
                            Log.EVENT_HANDLER($"{channel.DeviceClass} parent:{channel.Parent}", Common.LOG_CATEGORY);
                            break;

                        case Phidgets.DeviceClass.VINT:
                            Log.EVENT_HANDLER($"Manager_Attach: {e} {e.Channel}", Common.LOG_CATEGORY);
                            Log.EVENT_HANDLER($"{channel.DeviceClass} parent:{channel.Parent}", Common.LOG_CATEGORY);
                            break;

                        //case DeviceClass.InterfaceKit:
                        //    DisplayChannelInfo(channel);
                        //    break;

                        // NOTE(crhodes)
                        // For everything else assume it is a Physical Phidget
                        default:

                            AddPhidgetDevice(channel);
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
                    deviceChannels.DigitalInputCount++;
                    break;

                case Phidgets.ChannelClass.DigitalOutput:
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
                    Log.Trace($"Adding new RCServoChannel SerialNumber:{phidgetDevice.SerialNumber} Channel:{deviceChannels.RCServoCount}", Common.LOG_CATEGORY);
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
                    deviceChannels.StepperCount++;
                    break;

                case Phidgets.ChannelClass.TemperatureSensor:
                    deviceChannels.TemperatureSensorCount++;
                    break;

                case Phidgets.ChannelClass.VoltageInput:
                    deviceChannels.VoltageInputCount++;
                    break;

                case Phidgets.ChannelClass.VoltageOutput:
                    deviceChannels.VoltageOutputCount++;
                    break;

                case Phidgets.ChannelClass.VoltageRatioInput:
                    deviceChannels.VoltageRatioInputCount++;
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
        public RCServoEx OpenRCServoHost(int serialNumber, int channel, RCServoConfiguration configuration)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Trace00) startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);


            //PhidgetDevice phidgetDevice = AvailablePhidgets[serialNumber];

            SerialChannel serialChannel = 
                new SerialChannel() 
                    { 
                        SerialNumber = serialNumber, 
                        Channel = channel 
                    };

            // TODO(crhodes)
            // Is this where we do this?

            RCServoEx rcServoHost = RCServoChannels[serialChannel];

            //RCServoEx rcServoHost = Common.PhidgetDeviceLibrary.OpenRCServoHost(serialNumber, channel, rcServoConfiguration);

            if (rcServoHost is null) throw new Exception($"Cannot find RCServoChannel for serialNumber:{serialNumber} channel:{channel}");

            // TODO(crhodes)
            // Maybe RCServoEx has a RCServoConfiguration property that we just set

            rcServoHost.LogPhidgetEvents = configuration.LogPhidgetEvents;
            rcServoHost.LogErrorEvents = configuration.LogErrorEvents;
            rcServoHost.LogPropertyChangeEvents = configuration.LogPropertyChangeEvents;

            //rcServoHost.LogCurrentChangeEvents = LogCurrentChangeEvents;
            rcServoHost.LogPositionChangeEvents = configuration.LogPositionChangeEvents;
            rcServoHost.LogVelocityChangeEvents = configuration.LogVelocityChangeEvents;

            rcServoHost.LogTargetPositionReachedEvents = configuration.LogTargetPositionReachedEvents;

            rcServoHost.LogPerformanceSequence = configuration.LogPerformanceSequence;
            rcServoHost.LogSequenceAction = configuration.LogSequenceAction;
            rcServoHost.LogActionVerification = configuration.LogActionVerification;

            if (rcServoHost.Attached is false)
            {
                rcServoHost.Open();
            }

            if (Common.VNCLogging.Trace00) Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

            return rcServoHost;
        }

        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion
    }
}
