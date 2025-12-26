using System;

namespace VNC.Phidget22.Configuration
{
    // TODO(crhodes)
    // Don't think these need to be nullable.

    public class SerialNumberHubPortChannel
    {
        public string Name { get; set; } = "SerialNumberHubPortChannel NAME";

        public int? SerialNumber { get; set; } = null;
        public int? HubPort { get; set; } = null;
        public int? Channel { get; set; } = null;
    }
}