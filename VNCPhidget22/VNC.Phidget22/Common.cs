using System;

namespace VNC.Phidget22
{
    public class Common : VNC.Core.Common
    {
        public const string APPLICATION_NAME = "VNCPhidget";
        public new const string LOG_CATEGORY = "VNCPhidget";

        public static PhidgetDeviceLibrary PhidgetDeviceLibrary;

        //public static Int32 PhidgetOpenTimeout { get; set; } = 60000; // ms
        public static Int32? PhidgetOpenTimeout { get; set; } = null; // ms
    }
}
