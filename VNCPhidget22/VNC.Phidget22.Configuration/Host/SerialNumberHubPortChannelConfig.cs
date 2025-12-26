using System;

namespace VNC.Phidget22.Configuration
{
    public class SerialNumberHubPortChannelConfig
    {
        /// <summary>
        /// Name of file
        /// </summary>
        public string Name { get; set; } = "SerialNumberHubPortChannelConfig NAME";

        /// <summary>
        /// Description of this file
        /// </summary>
        public string Description { get; set; } = "SerialNumberHubPortChannelConfig DESCRIPTION";

        public SerialNumberHubPortChannel[]? SerialNumberHubPortChannels { get; set; }
    }
}
