/*
 * Code by Christophe Gerbier, 2010
 *
*/
using System;
using Microsoft.SPOT;
using System.Threading;
using GHIElectronics.NETMF.System;
using GHIElectronics.NETMF.USBHost;

namespace FEZPhidgets
{
    public class Phidget1052PositionChangeEventArgs
    {
        public Phidget1052PositionChangeEventArgs(int ePosition, bool eDirection, sbyte eRelativeMove) { Position = ePosition; Direction = eDirection; RelativeMove = eRelativeMove; }
        public int Position { get; private set; }
        public bool Direction { get; private set; }
        public int RelativeMove { get; private set; }
    }

    public class Phidget1052InputChangeEventArgs
    {
        public Phidget1052InputChangeEventArgs(bool eValue) { Value = eValue; }
        public bool Value { get; private set; }
    }

    public class Phidget1052
    {
        public bool Input { get; private set; }
        public int Position { get; private set; }

        public event PositionChangeEventHandler PositionChange;
        public event InputChangeEventHandler InputChange;

        public delegate void PositionChangeEventHandler(object sender, Phidget1052PositionChangeEventArgs e);
        public delegate void InputChangeEventHandler(object sender, Phidget1052InputChangeEventArgs e);


        private USBH_RawDevice _IF1052;
        private USBH_RawDevice.Pipe _IF1052_Pipe;
        private Thread _IF1052_Thread;
        private byte[] EncoderData = new byte[8];
        private int Button;
        

        // Constructor
        // Init some internals and public properties
        public Phidget1052()
        {
            Position = 0;
            USBHostController.DeviceConnectedEvent += DeviceConnectedEvent;
        }

        // Code taken from the (very useful) XBOX_Controller driver
        private void DeviceConnectedEvent(USBH_Device device)
        {
            if (device.TYPE == USBH_DeviceType.HID && device.VENDOR_ID == 0x06C2 && device.PRODUCT_ID == 0x004B)  // Detect only Phidgets' Encoder 1052
            {
                _IF1052 = new USBH_RawDevice(device);
                USB_Descriptors.ConfigurationDescriptor cd = _IF1052.GetConfigurationDescriptors(0);   // Get descriptors
                USB_Descriptors.EndpointDescriptor _IF1052_EP = null; // communication endpoint
                for (int i = 0; i < cd.interfaces.Length; i++)   // look for HID class
                {
                    if (cd.interfaces[i].bInterfaceClass == 0x03)  // found
                    {
                        Debug.Print("Phidget Digital Encoder 1052 detected");
                        for (int ep = 0; ep < cd.interfaces[i].endpoints.Length; ep++)  // look for input interrupt Endpoint
                        {
                            if (cd.interfaces[i].endpoints[ep].bmAttributes == 0x03)  // is it interrupt Endpoint?
                            {
                                _IF1052.SendSetupTransfer(0x00, 0x09, cd.bConfigurationValue, 0x00);  // set configuration
                                _IF1052_EP = cd.interfaces[i].endpoints[ep];       // get endpoint
                                _IF1052_Pipe = _IF1052.OpenPipe(_IF1052_EP);    // open pipe
                                _IF1052_Pipe.TransferTimeout = 0;                  // recommended for interrupt transfers
                                while (USBHostController.GetLastError() != 0) { }
                                _IF1052_Thread = new Thread(_1052Thread);         // create the polling thread
                                _IF1052_Thread.Priority = ThreadPriority.Normal;
                                _IF1052_Thread.Start();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }


        private void _1052Thread()
        {
            int count;
            while (true)
            {
                Thread.Sleep(_IF1052_Pipe.PipeEndpoint.bInterval);  // Read every bInterval
                count = 0;
                lock (EncoderData)
                {
                    try
                    {
                        count = _IF1052_Pipe.TransferData(EncoderData, 0, EncoderData.Length);
                        if (count > 3 && USBHostController.GetLastError() == 0)   // Valid data received
                        {
                            if ((EncoderData[1] & 0x01) != Button)   // Input state change
                            {
                                Input = (EncoderData[1] & 0x01) == 0 ? true : false;  // Input state
                                Button = EncoderData[1] & 0x01;     // Save state for next change event
                                if (InputChange != null) { InputChange(this, new Phidget1052InputChangeEventArgs(Input)); }   // Raise event if available
                            }

                            if (EncoderData[4] != 0)   // Position change
                            {
                                Position += (sbyte)EncoderData[4];    // Relative move value
                                if (PositionChange != null) { PositionChange(this, new Phidget1052PositionChangeEventArgs(Position, (sbyte)EncoderData[4] > 0, (sbyte)EncoderData[4])); }
                            }
                        }
                    }
                    catch
                    {
                        count = 0;
                    }
                }
            }
        }
    }
}
