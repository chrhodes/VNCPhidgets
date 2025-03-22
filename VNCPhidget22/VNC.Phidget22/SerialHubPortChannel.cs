using System;

namespace VNC.Phidget22
{
    public struct SerialHubPortChannel
    {
        public Int32 SerialNumber;
        public Int32 HubPort;
        public Int32 Channel;

        public override string ToString()
        {
            return $"S#:{SerialNumber} HP:{HubPort} C:{Channel}";
        }
    }

    // NOTE(crhodes)
    // Making this a class caused the PhidgetDevice dicitonary
    // to fail during Key lookups.
    // Need to override Equals

    //public class SerialChannel
    //{
    //    public Int32 SerialNumber { get; set; }
    //    public Int32 Channel { get; set; }
    //}
}

