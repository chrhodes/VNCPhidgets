using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VNCPhidgetConfig = VNCPhidget22.Configuration;

namespace VNC.Phidget
{
    public class PhidgetLibrary
    {

        #region Constructors, Initialization, and Load

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)

        //public static Dictionary<Int32, PhidgetDevice> AvailablePhidgets = new Dictionary<Int32, PhidgetDevice>();
        public static Dictionary<Int32, PhidgetDevice> _availablePhidgets;
        public static Dictionary<Int32, PhidgetDevice> AvailablePhidgets 
        {
            get
            {
                if (_availablePhidgets is null)
                {
                    BuildPhidgetDeviceDictionary();
                }
                return _availablePhidgets;
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

            _availablePhidgets = new Dictionary<int, PhidgetDevice>();

            foreach (VNCPhidgetConfig.Host host in VNCPhidgetConfig.PerformanceLibrary.Hosts)
            {
                if (host.AdvancedServos is not null)
                {
                    foreach (VNCPhidgetConfig.AdvancedServo advancedServo in host.AdvancedServos)
                    {
                        _availablePhidgets.Add(
                            advancedServo.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidgets.Phidget.PhidgetClass.ADVANCEDSERVO, 
                                advancedServo.SerialNumber));
                    }
                }

                if (host.InterfaceKits is not null)
                {
                    foreach (VNCPhidgetConfig.InterfaceKit interfaceKit in host.InterfaceKits)
                    {
                        _availablePhidgets.Add(
                            interfaceKit.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidgets.Phidget.PhidgetClass.INTERFACEKIT, 
                                interfaceKit.SerialNumber));
                    }
                }

                if (host.Steppers is not null)
                {
                    foreach (VNCPhidgetConfig.Stepper stepper in host.Steppers)
                    {
                        _availablePhidgets.Add(
                            stepper.SerialNumber,
                            new PhidgetDevice(
                                host.IPAddress, host.Port,
                                Phidgets.Phidget.PhidgetClass.STEPPER, 
                                stepper.SerialNumber));
                    }
                }
            }

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion


    }
}
