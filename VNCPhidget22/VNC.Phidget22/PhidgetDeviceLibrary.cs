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

namespace VNC.Phidget22
{
    // TODO(crhodes)
    // Make this a Singleton

    public class PhidgetDeviceLibrary
    {
        #region Constructors, Initialization, and Load
        public PhidgetDeviceLibrary()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            BuildPhidgetDeviceDictionary();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void BuildPhidgetDeviceDictionary()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            _availablePhidgets = new Dictionary<Int32, PhidgetDevice>();

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


            //if (host.AdvancedServos is not null)
            //{
            //    foreach (VNCPhidgetConfig.AdvancedServo advancedServo in host.AdvancedServos)
            //    {
            //        _availablePhidget22.Add(
            //            advancedServo.SerialNumber,
            //            new PhidgetDevice(
            //                host.IPAddress, host.Port,
            //                DeviceClass.AdvancedServo, 
            //                ChannelClass.RCServo,
            //                advancedServo.SerialNumber));
            //    }
            //}

            //if (host.InterfaceKits is not null)
            //{
            //    foreach (VNCPhidgetConfig.InterfaceKit interfaceKit in host.InterfaceKits)
            //    {
            //        _availablePhidget22.Add(
            //            interfaceKit.SerialNumber,
            //            new PhidgetDevice(
            //                host.IPAddress, host.Port,
            //                DeviceClass.InterfaceKit, 
            //                ChannelClass.None,
            //                interfaceKit.SerialNumber));
            //    }
            //}

            //if (host.Steppers is not null)
            //{
            //    foreach (VNCPhidgetConfig.Stepper stepper in host.Steppers)
            //    {
            //        _availablePhidget22.Add(
            //            stepper.SerialNumber,
            //            new PhidgetDevice(
            //                host.IPAddress, host.Port,
            //                DeviceClass.Stepper,
            //                ChannelClass.None,
            //                stepper.SerialNumber));
            //    }
            //}


            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Fields and Properties

        //public static Dictionary<Int32, PhidgetDevice> AvailablePhidget22 = new Dictionary<Int32, PhidgetDevice>();
        public Dictionary<Int32, PhidgetDevice> _availablePhidgets;
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


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion
    }
}
