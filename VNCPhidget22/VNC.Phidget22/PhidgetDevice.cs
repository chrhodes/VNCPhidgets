using Phidget22;

namespace VNC.Phidget22
{

    public class PhidgetDevice
    {
        //public PhidgetDevice()
        //{
        //}

        public PhidgetDevice(string ipAddress, int port, DeviceClass deviceClass, ChannelClass channelClass, int serialNumber)
        {
            IPAddress = ipAddress;
            Port = port;
            ChannelClass = channelClass;
            DeviceClass = deviceClass;
            SerialNumber = serialNumber;
        }

        // TODO(crhodes)
        // How is this used?
        public SerialHost SerialHostKey { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 5661;

        public ChannelClass ChannelClass { get; set; } = ChannelClass.None;

        public DeviceClass DeviceClass { get; set; } = DeviceClass.None;

        public int SerialNumber { get; set; } = 0;

        public PhidgetEx PhidgetEx {  get; set; }
    }
}

