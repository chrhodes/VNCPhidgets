using Microsoft.SPOT;
using System.Threading;
using GHIElectronics.NETMF.System;
using GHIElectronics.NETMF.USBHost;
using FEZPhidgets.Events;
using System;

namespace FEZPhidgets
{
    public class PhidgetRFID
    {
        public string SerialNumber { get; private set; }
        public string Name { get; private set; }
        public bool TagPresent { get; private set; }
        public string LastTag { get; private set; }
        public bool Attached { get; private set; }

        public event AttachEventHandler Attach;
        public event TagEventHandler Tag;
        public event TagEventHandler TagLost;
        public event OutputChangeEventHandler OutputChange;

        private USBH_RawDevice _RFID;
        private USBH_RawDevice.Pipe _RFID_Pipe;
        private byte[] RFID_Data = new byte[7];
        private byte[] OutputData = new byte[4];
        private bool _Antenna, _Led;
        private bool[] OutputStates = new bool[2];
        private Thread _RFID_Thread;

        // Constructor
        // Init some internals and public properties
        public PhidgetRFID()
        {
            LastTag = string.Empty;
            TagPresent = false;
            Attached = false;

            USBHostController.DeviceConnectedEvent += DeviceConnectedEvent;
        }

        public bool Antenna 
        {
            get { return _Antenna; }
            set 
            { 
                _Antenna = value;
                SetIO();
            }
        }

        public bool LED
        {
            get { return _Led; }
            set
            {
                _Led = value;
                SetIO();
            }
        }

        private void SetIO()
        {
            int toto = (OutputStates[0] ? 0x01 : 00) + (OutputStates[1] ? 0x02 : 00) + (_Led ? 0x04 : 0x00) + (_Antenna ? 0x08 : 0x00);
            OutputData[0] = (byte)toto;
            OutputData[1] = 0x00;
            OutputData[2] = 0x00;
            OutputData[3] = 0x00;
            _RFID.SendSetupTransfer(0x21, 0x09, 0x02, 0x00, OutputData, 0, 4);
        }

        private void DeviceConnectedEvent(USBH_Device device)
        {
            

            if (device.TYPE == USBH_DeviceType.HID && device.VENDOR_ID == 0x06C2 && device.PRODUCT_ID == 0x0031)  // Detect only Phidgets RFID
            {
                _RFID = new USBH_RawDevice(device);
                USB_Descriptors.ConfigurationDescriptor cd = _RFID.GetConfigurationDescriptors(0);   // Get descriptors
                USB_Descriptors.EndpointDescriptor _RFID_EP = null; // communication endpoint
                USBH_ERROR e = 0;
                for (int i = 0; i < cd.interfaces.Length; i++)   // look for HID class
                {
                    if (cd.interfaces[i].bInterfaceClass == 0x03)  // found
                    {
                        for (int ep = 0; ep < cd.interfaces[i].endpoints.Length; ep++)  // look for input interrupt Endpoint
                        {
                            if (cd.interfaces[i].endpoints[ep].bmAttributes == 0x03)  // is it interrupt Endpoint?
                            {
                                #region Board serial number
                                // Get the board's serial number
                                byte[] toto = new byte[0x0C];
                                _RFID.SendSetupTransfer(0x80, 0x06, 0x0303, 0x0409, toto, 0, 0x0C);
                                for (int j = 2; j <= 0x0A; j += 2) SerialNumber += (char)toto[j];
                                #endregion

                                #region Board name
                                // Get the board name
                                byte[] tutu = new byte[0x18];
                                _RFID.SendSetupTransfer(0x80, 0x06, 0x0302, 0x0409, tutu, 0, 0x18);
                                for (int j = 2; j <= 0x16; j += 2) Name += (char)tutu[j];
                                #endregion

                                //Debug.Print(Name + " detected, serial : "+Serial);

                                _RFID.SendSetupTransfer(0x00, 0x09, cd.bConfigurationValue, 0x00);  // set configuration
                                _RFID_EP = cd.interfaces[i].endpoints[ep];       // get endpoint
                                _RFID_Pipe = _RFID.OpenPipe(_RFID_EP);    // open pipe
                                _RFID_Pipe.TransferTimeout = 0;                  // recommended for interrupt transfers

                                // This loop is in fact waiting for the RFID board to be ready, that is : not throwing toggle errors
                                do
                                {
                                    Thread.Sleep(_RFID_Pipe.PipeEndpoint.bInterval);
                                    try
                                    {
                                        _RFID_Pipe.TransferData(RFID_Data, 0, RFID_Data.Length);
                                    }
                                    catch
                                    {
                                        e = USBHostController.GetLastError();
                                    }
                                } while (e != 0);

                                _RFID_Thread = new Thread(RFIDThread);         // Create the polling thread
                                _RFID_Thread.Priority = ThreadPriority.Highest;
                                _RFID_Thread.Start();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void RFIDThread()
        {
            int count, CountLost=0,j;
            byte[] Tmp = new byte[5];
            string TagTmp = string.Empty;

            if (Attach != null) // We send the Attached event to inform the user that he can use the RFID reader
            {
                Attached = true;
                Attach(this, new PhidgetAttachEventArgs(Name, SerialNumber));
            }
            Thread.Sleep(_RFID_Pipe.PipeEndpoint.bInterval);  // Read every bInterval
            while (true)
            {
                count = 0;
                try
                {
                    count = _RFID_Pipe.TransferData(RFID_Data, 0, RFID_Data.Length);
                }
                catch
                {
                    //USBH_ERROR e = USBHostController.GetLastError();
                    //Debug.Print("CHG ERROR " + e.ToString());
                    //Array.Clear(IFKit, 0, IFKit.Length);
                    count = 0;
                }
                if (count == 7)   // Valid data received
                {
                    TagPresent = (RFID_Data[0] == 0);
                    if (TagPresent)
                    {
                        CountLost = 0;
                        
                        for (j=1; j<=5; j++) Tmp[j-1] = RFID_Data[j];
                        TagTmp = new string(ToHexString(Tmp)).ToLower();

                        if (TagTmp != LastTag)
                        {
                            LastTag = TagTmp;
                            if (Tag != null) { Tag(this, new TagEventArgs(LastTag)); }
                        }
                    }
                    else   // Outputs status or tag loss
                    {
                        OutputStates[0] = (RFID_Data[1] & 1) != 0;
                        OutputStates[1] = (RFID_Data[1] & 2) != 0;
                        CountLost++;
                        if (CountLost > 1)   // Effective tag loss
                        {
                            if (LastTag != string.Empty)
                            {
                                if (TagLost != null) { TagLost(this, new TagEventArgs(LastTag)); }
                                LastTag = string.Empty;
                                CountLost = 0;
                            }
                        }
                    }
                    
                }
                Thread.Sleep(_RFID_Pipe.PipeEndpoint.bInterval);  // Read every bInterval
            }
        }

        static char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7','8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        //convert a hex number to a hex string
        private static char[] ToHexString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return chars;
        }


        public bool Output(int Index)
        {
            return OutputStates[Index];
        }

        public void Output(int Index, bool Value)
        {
            if (Index > 1) { return; }  // Only 2 outputs on RFID board
            OutputStates[Index] = Value;
            //Debug.Print("Sending output : "+OutputData[0].ToString());
            SetIO();
            if (OutputChange != null) { OutputChange(this, new PhidgetOutputChangeEventArgs(Index, Value)); }
        }
    }
}
