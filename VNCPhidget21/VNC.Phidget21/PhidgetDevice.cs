namespace VNC.Phidget
{

    public class PhidgetDevice
    {
        //public PhidgetDevice()
        //{
        //}

        public PhidgetDevice(string ipAddress, int port, Phidgets.Phidget.PhidgetClass phidgetClass, int serialNumber)
        {
            IPAddress = ipAddress;
            Port = port;
            PhidgetClass = phidgetClass;
            SerialNumber = serialNumber;
        }

        public SerialHost SerialHostKey { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 5001;

        public Phidgets.Phidget.PhidgetClass PhidgetClass { get; set; } = Phidgets.Phidget.PhidgetClass.NOTHING;

        public int SerialNumber { get; set; } = 0;

        public PhidgetEx PhidgetEx {  get; set; }
    }
}

