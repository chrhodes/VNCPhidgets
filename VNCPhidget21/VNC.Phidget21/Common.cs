namespace VNC.Phidget
{
    public class Common : VNC.Core.Common
    {
        public const string APPLICATION_NAME = "VNCPhidget";
        public new const string LOG_CATEGORY = "VNCPhidget";

        //public static int PhidgetOpenTimeout { get; set; } = 60000; // ms
        public static int? PhidgetOpenTimeout { get; set; } = null; // ms
    }
}
