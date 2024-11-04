using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Phidget22;

using VNCPhidget22.Configuration;

using VNCPhidgetConfig = VNCPhidget22.Configuration;

namespace VNC.Phidget22
{
    public partial class PhidgetLibrary
    {

#region Structures (None)
        #endregion

        #region Fields and Properties (None)

        //public static Dictionary<Int32, PhidgetDevice> AvailablePhidget22 = new Dictionary<Int32, PhidgetDevice>();
        public static Dictionary<Int32, PhidgetDevice> _availablePhidget22;
        public static Dictionary<Int32, PhidgetDevice> AvailablePhidget22 
        {
            get
            {
                if (_availablePhidget22 is null)
                {
                    BuildPhidgetDeviceDictionary();
                }
                return _availablePhidget22;
            }
            set 
            { 

            } 
        }

        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods (None)


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        private static void BuildPhidgetDeviceDictionary()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            _availablePhidget22 = new Dictionary<int, PhidgetDevice>();

            foreach (VNCPhidgetConfig.Host host in VNCPhidgetConfig.PerformanceLibrary.Hosts)
            {
                if (host.AdvancedServos is not null)
                {
                    foreach (VNCPhidgetConfig.AdvancedServo advancedServo in host.AdvancedServos)
                    {
                        _availablePhidget22.Add(
                            advancedServo.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                DeviceClass.AdvancedServo, 
                                ChannelClass.RCServo,
                                advancedServo.SerialNumber));
                    }
                }

                if (host.InterfaceKits is not null)
                {
                    foreach (VNCPhidgetConfig.InterfaceKit interfaceKit in host.InterfaceKits)
                    {
                        _availablePhidget22.Add(
                            interfaceKit.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                DeviceClass.InterfaceKit, 
                                ChannelClass.None,
                                interfaceKit.SerialNumber));
                    }
                }

                if (host.Steppers is not null)
                {
                    foreach (VNCPhidgetConfig.Stepper stepper in host.Steppers)
                    {
                        _availablePhidget22.Add(
                            stepper.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                DeviceClass.Stepper,
                                ChannelClass.None,
                                stepper.SerialNumber));
                    }
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion
    }
}
