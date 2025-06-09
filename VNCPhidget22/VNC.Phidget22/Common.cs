using System;

using VNC.Phidget22.Configuration;

namespace VNC.Phidget22
{
    public class Common : VNC.Core.Common
    {
        public const string APPLICATION_NAME = "VNCPhidget";
        public new const string LOG_CATEGORY = "VNCPhidget";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static PhidgetDeviceLibrary PhidgetDeviceLibrary;
        public static PerformanceLibrary PerformanceLibrary;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        //public static Int32 PhidgetOpenTimeout { get; set; } = 60000; // ms
        public static Int32? PhidgetOpenTimeout { get; set; } = null; // ms
    }
}
