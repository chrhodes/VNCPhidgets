/*
 * Code by Christophe Gerbier, 2010
 *
 * Rev 1.02
 *  - Added new properties : Name, Serial and Sensitivity
 *    Name and Serial are taken directly from the board. 
 *  - Added new event : Attached, that fires when the board is ready to use
 *  
*/
//#define DEBUG

using System;
using Microsoft.SPOT;
using System.Threading;
using System.Text;
using GHIElectronics.NETMF.System;
using GHIElectronics.NETMF.USBHost;

namespace FEZPhidgets
{
    public class Phidget1018SensorChangeEventArgs
    {
        public Phidget1018SensorChangeEventArgs(int eIndex, int eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public int Value { get; private set; }
    }

    public class Phidget1018InputChangeEventArgs
    {
        public Phidget1018InputChangeEventArgs(int eIndex, bool eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public bool Value { get; private set; }
    }

    public class Phidget1018OutputChangeEventArgs
    {
        public Phidget1018OutputChangeEventArgs(int eIndex, bool eValue) { Index = eIndex; Value = eValue; }
        public int Index { get; private set; }
        public bool Value { get; private set; }
    }

    public class Phidget1018AttachedEventArgs
    {
        public Phidget1018AttachedEventArgs(string eDeviceName, string eSerial) { DeviceName = eDeviceName; SerialNum = eSerial; }
        public string DeviceName { get; private set; }
        public string SerialNum { get; private set; }
    }

    public class Phidget1018
    {
        public bool[] Input { get; private set; }
        public int[] Sensor { get; private set; }
        public int[] RawSensor { get; private set; }
        public string Serial { get; private set; }
        public string Name { get; private set; }
        public int Sensitivity;

        public event SensorChangeEventHandler SensorChange;
        public event InputChangeEventHandler InputChange;
        public event OutputChangeEventHandler OutputChange;
        public event AttachedEventHandler Attached;

        public delegate void SensorChangeEventHandler(object sender, Phidget1018SensorChangeEventArgs e);
        public delegate void InputChangeEventHandler(object sender, Phidget1018InputChangeEventArgs e);
        public delegate void OutputChangeEventHandler(object sender, Phidget1018OutputChangeEventArgs e);
        public delegate void AttachedEventHandler(object sender, Phidget1018AttachedEventArgs e);

        private USBH_RawDevice _IF1018;
        private USBH_RawDevice.Pipe _IF1018_Pipe;
        private byte[] IFKit;
        private byte[] OutputData = new byte[4];
        private bool[] OutputStates = new bool[8];

        // Constructor
        // Init some internals and public properties
        public Phidget1018()
        {
            Input = new bool[8];
            Sensor = new int[8];
            RawSensor = new int[8];
            IFKit = new byte[8];
            Sensitivity = 10;
            USBHostController.DeviceConnectedEvent += DeviceConnectedEvent;
        }

        // Code parts taken from the (very useful) XBOX_Controller driver
        private void DeviceConnectedEvent(USBH_Device device)
        {
            Thread _IF1018_Thread;

            if (device.TYPE == USBH_DeviceType.HID && device.VENDOR_ID == 0x06C2 && device.PRODUCT_ID == 0x0045)  // Detect only Phidgets' InterfaceKit
            {
                _IF1018 = new USBH_RawDevice(device);
                USB_Descriptors.ConfigurationDescriptor cd = _IF1018.GetConfigurationDescriptors(0);   // Get descriptors
                USB_Descriptors.EndpointDescriptor _IF1018_EP = null; // communication endpoint
                USBH_ERROR e = 0;
                //_IF1018_EP = null; // communication endpoint
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
                                _IF1018.SendSetupTransfer(0x80, 0x06, 0x0303, 0x0409, toto, 0, 0x0C);
                                for (int j = 2; j <= 0x0A; j += 2) Serial += (char)toto[j];
                                #endregion

                                #region Board name
                                // Get the board name
                                byte[] tutu = new byte[0x28];
                                _IF1018.SendSetupTransfer(0x80, 0x06, 0x0302, 0x0409, tutu, 0, 0x28);
                                for (int j = 2; j <= 0x26; j += 2) Name += (char)tutu[j];
                                #endregion

                                //Debug.Print(Name + " detected, serial : "+Serial);

                                _IF1018.SendSetupTransfer(0x00, 0x09, cd.bConfigurationValue, 0x00);  // set configuration
                                _IF1018_EP = cd.interfaces[i].endpoints[ep];       // get endpoint
                                _IF1018_Pipe = _IF1018.OpenPipe(_IF1018_EP);    // open pipe
                                _IF1018_Pipe.TransferTimeout = 0;                  // recommended for interrupt transfers
                                
                                // This loop is in fact waiting for the 1018 to be ready, that is : not throwing toggle errors
                                do
                                {
                                    Thread.Sleep(_IF1018_Pipe.PipeEndpoint.bInterval);  
                                    try
                                    {
                                        _IF1018_Pipe.TransferData(IFKit, 0, IFKit.Length);
                                    }
                                    catch
                                    {
                                        e = USBHostController.GetLastError();
                                    }
                                } while (e != 0);

                                _IF1018_Thread = new Thread(_1018Thread);         // Create the polling thread
                                _IF1018_Thread.Priority = ThreadPriority.Highest;
                                _IF1018_Thread.Start();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }


        private void _1018Thread()
        {
            int count;
            int[] S = new int[8];

            if (Attached != null) // We send the Attached event to inform the user that he can use the 1018
            {
                Attached(this, new Phidget1018AttachedEventArgs(Name,Serial));
            }

            while (true)
            {
                Thread.Sleep(_IF1018_Pipe.PipeEndpoint.bInterval);  // Read every bInterval
                count = 0;
                try
                {
                    count = _IF1018_Pipe.TransferData(IFKit, 0, IFKit.Length);
                }
                catch
                {
                    //USBH_ERROR e = USBHostController.GetLastError();
                    //Debug.Print("CHG ERROR " + e.ToString());
                    //Array.Clear(IFKit, 0, IFKit.Length);
                    count = 0;
                }
                if (count == 8)   // Valid data received
                {
                    #region GetDigitalIO
                    for (int i = 0, j = 0x01; i < 8; i++, j <<= 1)   // Scan through the 8 (0..7) digital inputs
                    {
                        if ((IFKit[1] & j) == 0 != Input[i]) // Input change
                        {
                            //Debug.Print("Index: " + i.ToString() + " : "+ ((IFKit[1] & j) == 0 ? "true" : "false")); 
                            // If available, raise user's event for each input state change
                            if (InputChange != null) { Input[i] = (IFKit[1] & j) == 0; InputChange(this, new Phidget1018InputChangeEventArgs(i, (IFKit[1] & j) == 0)); }
                        }
                        Input[i] = (IFKit[1] & j) == 0;  // Fill the input array
                        //Debug.Print("Input value " + i.ToString() + " : " + (IFKit[1] & j).ToString());
                    }
                    #endregion
                    #region GetAnalogSensors
                    // Analog inputs' data and digital output states are sent in two times : first 4 (0..3) then last 4 (4..7)
                    if ((IFKit[0] & 0x01) == 0)   // First set of 4
                    {
                        for (int i = 0, j = 0x10; i < 4; i++, j <<= 1) { OutputStates[i] = (IFKit[0] & j) != 0; }

                        for (int i = 0; i < 4; i++) { S[i] = RawSensor[i]; }   // Memorize values for sensitivity check

                        // Get raw values from sensors. Raw values range is 0..4095
                        RawSensor[0] = (IFKit[2] + (IFKit[3] & 0x0f) * 256);
                        RawSensor[1] = (IFKit[4] + (IFKit[3] & 0xf0) * 16);
                        RawSensor[2] = (IFKit[5] + (IFKit[6] & 0x0f) * 256);
                        RawSensor[3] = (IFKit[7] + (IFKit[6] & 0xf0) * 16);

                        for (int i = 0; i < 4; i++)
                        {
                            if (System.Math.Abs(RawSensor[i] - S[i]) > Sensitivity)   // Generate event (if available) only if change is greater than sensitivity
                            {
                                //Debug.Print("Sensor " + i.ToString() + " : " + RawSensor[i].ToString() + ", Abs = " + System.Math.Abs(RawSensor[i] - S[i]).ToString());
                                if (SensorChange != null)
                                {
                                    Sensor[i] = (int)MathEx.Round(RawSensor[i] / 4.095);   // Fill in "normal" sensors' values. Range is 0..1000
                                    SensorChange(this, new Phidget1018SensorChangeEventArgs(i, (int)MathEx.Round(RawSensor[i] / 4.095)));
                                }
                            }
                            Sensor[i] = (int)MathEx.Round(RawSensor[i] / 4.095);
                        }
                    }
                    else   // Same procedure for the next 4 analog inputs and digital outputs
                    {
                        for (int i = 4, j = 0x10; i < 8; i++, j <<= 1) { OutputStates[i] = (IFKit[0] & j) != 0; }

                        for (int i = 4; i < 8; i++) { S[i] = RawSensor[i]; }

                        RawSensor[4] = (IFKit[2] + (IFKit[3] & 0x0f) * 256);
                        RawSensor[5] = (IFKit[4] + (IFKit[3] & 0xf0) * 16);
                        RawSensor[6] = (IFKit[5] + (IFKit[6] & 0x0f) * 256);
                        RawSensor[7] = (IFKit[7] + (IFKit[6] & 0xf0) * 16);

                        for (int i = 4; i < 8; i++)
                        {
                            if (System.Math.Abs(RawSensor[i] - S[i]) > Sensitivity)
                            {
                                //Debug.Print("Sensor " + i.ToString() + " : " + RawSensor[i].ToString() + ", Abs = " + System.Math.Abs(RawSensor[i] - S[i]).ToString());
                                if (SensorChange != null)
                                {
                                    Sensor[i] = (int)MathEx.Round(RawSensor[i] / 4.095);
                                    SensorChange(this, new Phidget1018SensorChangeEventArgs(i, (int)MathEx.Round(RawSensor[i] / 4.095)));
                                }
                            }
                            Sensor[i] = (int)MathEx.Round(RawSensor[i] / 4.095);
                        }
                    }
                    #endregion
                }
            }
        }

        public bool Output(int Index)
        {
            return OutputStates[Index];
        }

        public void Output(int Index, bool Value)
        {
            OutputStates[Index] = Value;
            OutputData[0] = 0x00;
            OutputData[1] = 0x00;
            OutputData[2] = 0x00;
            OutputData[3] = 0x01;
            for (byte k = 0, j = 1; k < 8; k++, j <<= 1) { OutputData[0] += OutputStates[k] ? j : (byte)0; }
            //Debug.Print("Sending output : "+OutputData[0].ToString());
            _IF1018.SendSetupTransfer(0x21, 0x09, 0x02, 0x00, OutputData, 0, 4);
            if (OutputChange != null) { OutputChange(this, new Phidget1018OutputChangeEventArgs(Index, Value)); }
        }
    }
}
