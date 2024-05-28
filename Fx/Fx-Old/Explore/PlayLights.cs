using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Runtime.InteropServices;  // DllImport()

//Needed for the InterfaceKit class, phidget base classes, and the PhidgetException class
using Phidgets;
//Needed for the event handling classes 
using Phidgets.Events;  

namespace Explore
{
    public partial class PlayLights : Form
    {
        static InterfaceKit ifKitA;
        static InterfaceKit ifKitB;
        static InterfaceKit ifKitC;

        public PlayLights()
        {
            InitializeComponent();
        }

        private void PlayLights_Load(object sender, EventArgs e)
        {
            //InitializePhidget();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            InitializePhidget();
        }

        private void InitializePhidget()
        {
            try
            {
                //Initialize the InterfaceKit object
                ifKitA = new InterfaceKit();
                ifKitB = new InterfaceKit();
                ifKitC = new InterfaceKit();
                
                //Hook the basica event handlers
                ifKitA.Attach += new AttachEventHandler(ifKit_Attach);
                ifKitA.Detach += new DetachEventHandler(ifKit_Detach);
                ifKitA.Error += new ErrorEventHandler(ifKit_Error);

                //Hook the phidget spcific event handlers
                //ifKit.InputChange += new InputChangeEventHandler(ifKit_InputChange);
                //ifKit.OutputChange += new OutputChangeEventHandler(ifKit_OutputChange);
                //ifKit.SensorChange += new SensorChangeEventHandler(ifKit_SensorChange);

                //Open the object for device connections
                //ifKit.open();

                //Open the Phidget using the command line arguments

                ifKitA.open(Int32.Parse(txtInterfaceKitA.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));
                ifKitB.open(Int32.Parse(txtInterfaceKitB.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));
                ifKitC.open(Int32.Parse(txtInterfaceKitC.Text), cbHostIP.Text, Int32.Parse(txtPort.Text));

                //openCmdLine(ifKit);

                //Wait for an InterfaceKit phidget to be attached
                Trace.WriteLine("Waiting for InterfaceKit to be attached...");

                ifKitA.waitForAttachment();
                ifKitB.waitForAttachment();
                ifKitC.waitForAttachment();

                ////Wait for user input so that we can wait and watch for some event data 
                ////from the phidget
                //Console.WriteLine("Press any key to end...");
                //Console.Read();

                ////User input was rad so we'll terminate the program, so close the object
                //ifKit.close();

                ////set the object to null to get it out of memory
                //ifKit = null;

                ////If no expcetions where thrown at this point it is safe to terminate 
                ////the program
                //Console.WriteLine("ok");
            }
            catch (PhidgetException ex)
            {
                Trace.WriteLine(ex.Description);
            }
        }


        #region Phidgets


        //Parses command line arguments and calls the appropriate open
        #region Command line open functions

        private void openCmdLine(Phidget p)
        {
            openCmdLine(p, null);
        }

        private void openCmdLine(Phidget p, String pass)
        {
            int serial = -1;
            int port = 5001;
            String host = null;
            bool remote = false, remoteIP = false;
            string[] args = Environment.GetCommandLineArgs();
            String appName = args[0];

            try
            { //Parse the flags
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-"))
                        switch (args[i].Remove(0, 1).ToLower())
                        {
                            case "n":
                                serial = int.Parse(args[++i]);
                                break;
                            case "r":
                                remote = true;
                                break;
                            case "s":
                                remote = true;
                                host = args[++i];
                                break;
                            case "p":
                                pass = args[++i];
                                break;
                            case "i":
                                remoteIP = true;
                                host = args[++i];
                                if (host.Contains(":"))
                                {
                                    port = int.Parse(host.Split(':')[1]);
                                    host = host.Split(':')[0];
                                }
                                break;
                            default:
                                goto usage;
                        }
                    else
                        goto usage;
                }

                if (remoteIP)
                    p.open(serial, host, port, pass);
                else if (remote)
                    p.open(serial, host, pass);
                else
                    p.open(serial);
                return; //success
            }
            catch
            {
            }
        usage:
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Invalid Command line arguments." + Environment.NewLine);
            sb.AppendLine("Usage: " + appName + " [Flags...]");
            sb.AppendLine("Flags:\t-n   serialNumber\tSerial Number, omit for any serial");
            sb.AppendLine("\t-r\t\tOpen remotely");
            sb.AppendLine("\t-s   serverID\tServer ID, omit for any server");
            sb.AppendLine("\t-i   ipAddress:port\tIp Address and Port. Port is optional, defaults to 5001");
            sb.AppendLine("\t-p   password\tPassword, omit for no password" + Environment.NewLine);
            sb.AppendLine("Examples: ");
            sb.AppendLine(appName + " -n 50098");
            sb.AppendLine(appName + " -r");
            sb.AppendLine(appName + " -s myphidgetserver");
            sb.AppendLine(appName + " -n 45670 -i 127.0.0.1:5001 -p paswrd");
            MessageBox.Show(sb.ToString(), "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Application.Exit();
        }

        //Attach event handler...Display the serial number of the attached InterfaceKit 
        //to the console      
        static void ifKit_Attach(object sender, AttachEventArgs e)
        {
            Trace.WriteLine(string.Format("InterfaceKit {0} attached!",
                                e.Device.SerialNumber.ToString()));
        }

        //Detach event handler...Display the serial number of the detached InterfaceKit 
        //to the console
        static void ifKit_Detach(object sender, DetachEventArgs e)
        {
            Trace.WriteLine(string.Format("InterfaceKit {0} detached!",
                                e.Device.SerialNumber.ToString()));
        }

        //Error event handler...Display the error description to the console
        static void ifKit_Error(object sender, ErrorEventArgs e)
        {
            Trace.WriteLine(e.Description);
        }

        ////Input Change event handler...Display the input index and the new value to the 
        ////console
        //static void ifKit_InputChange(object sender, InputChangeEventArgs e)
        //{
        //    Trace.WriteLine(string.Format("Input index {0} value (1)", e.Index, e.Value.ToString()));
        //}

        ////Output change event handler...Display the output index and the new value to 
        ////the console
        //static void ifKit_OutputChange(object sender, OutputChangeEventArgs e)
        //{
        //    Trace.WriteLine(string.Format("Output index {0} value {0}", e.Index, e.Value.ToString()));
        //}

        //Sensor Change event handler...Display the sensor index and it's new value to 
        //the console

        void ifKit_SensorChange(object sender, SensorChangeEventArgs e)
        {
            //txtSensorEvents.AppendText(string.Format("Sensor index {0} value {1}{2}", e.Index, e.Value, Environment.NewLine));
            ////Trace.WriteLine(string.Format("Sensor index {0} value {1}", e.Index, e.Value));
            ////this.txtIndex.Text = e.Index.ToString();

            //switch (e.Index)
            //{
            //    case 0:
            //        txtValue0.Text = e.Value.ToString();
            //        distanceSensor1.changeDisplay(e.Value);
            //        break;

            //    case 1:
            //        txtValue1.Text = e.Value.ToString();
            //        distanceSensor2.changeDisplay(e.Value);
            //        break;

            //    case 2:
            //        txtValue2.Text = e.Value.ToString();
            //        break;

            //    case 3:
            //        txtValue3.Text = e.Value.ToString();
            //        motionSensor1.changeDisplay(e.Value);
            //        break;
            //}
        }

        #endregion

        private void ckOutput0A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 0, ckOutput0A.Checked);
        }

        private void ckOutput1A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 1, ckOutput1A.Checked);
        }

        private void ckOutput2A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 2, ckOutput2A.Checked);
        }

        private void ckOutput3A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 3, ckOutput3A.Checked);
        }

        private void ckOutput4A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 4, ckOutput4A.Checked);
        }

        private void ckOutput5A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 5, ckOutput5A.Checked);
        }

        private void ckOutput6A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 6, ckOutput6A.Checked);
        }

        private void ckOutput7A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 7, ckOutput7A.Checked);
        }

        private void ckOutput8A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 8, ckOutput8A.Checked);
        }

        private void ckOutput9A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 9, ckOutput9A.Checked);
        }

        private void ckOutput10A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 10, ckOutput10A.Checked);
        }

        private void ckOutput11A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 11, ckOutput11A.Checked);
        }

        private void ckOutput12A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 12, ckOutput12A.Checked);
        }

        private void ckOutput13A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 13, ckOutput13A.Checked);
        }

        private void ckOutput14A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 14, ckOutput14A.Checked);
        }

        private void ckOutput15A_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitA, 15, ckOutput15A.Checked);
        }


        private void ckOutput0B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 0, ckOutput0B.Checked);
        }

        private void ckOutput1B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 1, ckOutput1B.Checked);
        }

        private void ckOutput2B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 2, ckOutput2B.Checked);
        }

        private void ckOutput3B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 3, ckOutput3B.Checked);
        }

        private void ckOutput4B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 4, ckOutput4B.Checked);
        }

        private void ckOutput5B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 5, ckOutput5B.Checked);
        }

        private void ckOutput6B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 6, ckOutput6B.Checked);
        }

        private void ckOutput7B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 7, ckOutput7B.Checked);
        }

        private void ckOutput8B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 8, ckOutput8B.Checked);
        }

        private void ckOutput9B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 9, ckOutput9B.Checked);
        }

        private void ckOutput10B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 10, ckOutput10B.Checked);
        }

        private void ckOutput11B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 11, ckOutput11B.Checked);
        }

        private void ckOutput12B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 12, ckOutput12B.Checked);
        }

        private void ckOutput13B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 13, ckOutput13B.Checked);
        }

        private void ckOutput14B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 14, ckOutput14B.Checked);
        }

        private void ckOutput15B_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitB, 15, ckOutput15B.Checked);
        }

        private void ckOutput0C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 0, ckOutput0C.Checked);
        }

        private void ckOutput1C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 1, ckOutput1C.Checked);
        }

        private void ckOutput2C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 2, ckOutput2C.Checked);
        }

        private void ckOutput3C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 3, ckOutput3C.Checked);
        }

        private void ckOutput4C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 4, ckOutput4C.Checked);
        }

        private void ckOutput5C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 5, ckOutput5C.Checked);
        }

        private void ckOutput6C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 6, ckOutput6C.Checked);
        }

        private void ckOutput7C_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(ifKitC, 7, ckOutput7C.Checked);
        }

        private void AlterOutputPort(InterfaceKit ifKit, int portNumber, bool value)
        {
            ifKit.outputs[portNumber] = value;
        }

        private void RedLightsOn(int duration)
        {
            AlterOutputPort(ifKitA, 0, true);
            AlterOutputPort(ifKitA, 3, true);
            System.Threading.Thread.Sleep(duration);
            AlterOutputPort(ifKitA, 0, false);
            AlterOutputPort(ifKitA, 3, false);
        }

        private void GreeLightsOn(int duration)
        {
            AlterOutputPort(ifKitA, 1, true);
            AlterOutputPort(ifKitA, 4, true);
            System.Threading.Thread.Sleep(duration);
            AlterOutputPort(ifKitA, 1, false);
            AlterOutputPort(ifKitA, 4, false);
        }

        private void WhiteLightsOn(int duration)
        {
            AlterOutputPort(ifKitA, 2, true);
            AlterOutputPort(ifKitA, 5, true);
            System.Threading.Thread.Sleep(duration);
            AlterOutputPort(ifKitA, 2, false);
            AlterOutputPort(ifKitA, 5, false);
        }

        private void btnLights_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                System.Threading.Thread.Sleep((int)nudRedDelay.Value);

                RedLightsOn((int)nudRedDuration.Value);

                System.Threading.Thread.Sleep((int)nudGreenDelay.Value);

                GreeLightsOn((int)nudGreenDuration.Value);

                System.Threading.Thread.Sleep((int)nudWhiteDelay.Value);

                WhiteLightsOn((int)nudWhiteDuration.Value);
            }
        }

        private void nudDelayIncrement_ValueChanged(object sender, EventArgs e)
        {
            nudRedDelay.Increment =   (int)nudDelayIncrement.Value;
            nudGreenDelay.Increment = (int)nudDelayIncrement.Value;
            nudWhiteDelay.Increment = (int)nudDelayIncrement.Value;
        }

        private void nudDurationIncrement_ValueChanged(object sender, EventArgs e)
        {
            nudRedDuration.Increment =   (int)nudDurationIncrement.Value;
            nudGreenDuration.Increment = (int)nudDurationIncrement.Value;
            nudWhiteDuration.Increment = (int)nudDurationIncrement.Value;
        }

        private void btnLights2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random(Int32.Parse(txtSeed.Text));


            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                int redDuration = rnd.Next(10) * (int)nudDurationIncrement.Value;
                int greenDuration = rnd.Next(10) * (int)nudDurationIncrement.Value;
                int whiteDuration = rnd.Next(10) * (int)nudDurationIncrement.Value;

                int redDelay = rnd.Next(10) * (int)nudDelayIncrement.Value;
                int greenDelay = rnd.Next(10) * (int)nudDelayIncrement.Value;
                int whiteDelay = rnd.Next(10) * (int)nudDelayIncrement.Value;   
            
                System.Threading.Thread.Sleep(redDelay);

                RedLightsOn(redDuration);

                System.Threading.Thread.Sleep(greenDelay);

                GreeLightsOn(greenDuration);

                System.Threading.Thread.Sleep(whiteDelay);

                WhiteLightsOn(whiteDuration);
            }
        }

        private void btnLights3_Click(object sender, EventArgs e)
        {
            int redDelay = (int)nudRedDelay.Value;
            int greenDelay = (int)nudGreenDelay.Value;
            int whiteDelay = (int)nudWhiteDelay.Value;

            int redDuration = (int)nudRedDuration.Value;
            int greenDuration = (int)nudGreenDuration.Value;
            int whiteDuration = (int)nudWhiteDuration.Value;

            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                System.Threading.Thread.Sleep(redDelay);

                RedLightsOn(redDuration);

                System.Threading.Thread.Sleep(greenDelay);

                GreeLightsOn(greenDuration);

                System.Threading.Thread.Sleep(whiteDelay);

                WhiteLightsOn(whiteDuration);

                redDelay += (int)nudDelayIncrement.Value;
                greenDelay += (int)nudDelayIncrement.Value;
                whiteDelay += (int)nudDelayIncrement.Value;

                redDuration -= (int)nudDurationIncrement.Value;
                greenDuration -= (int)nudDurationIncrement.Value;
                whiteDuration -= (int)nudDurationIncrement.Value;
            }
        }

        private void btnLights4_Click(object sender, EventArgs e)
        {
            int redDelay = (int)nudRedDelay.Value;
            int greenDelay = (int)nudGreenDelay.Value;
            int whiteDelay = (int)nudWhiteDelay.Value;

            int redDuration = (int)nudRedDuration.Value;
            int greenDuration = (int)nudGreenDuration.Value;
            int whiteDuration = (int)nudWhiteDuration.Value;

            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                System.Threading.Thread.Sleep(redDelay);

                RedLightsOn(redDuration);

                System.Threading.Thread.Sleep(greenDelay);

                GreeLightsOn(greenDuration);

                System.Threading.Thread.Sleep(whiteDelay);

                WhiteLightsOn(whiteDuration);

                redDelay -= (int)nudDelayIncrement.Value;
                greenDelay -= (int)nudDelayIncrement.Value;
                whiteDelay -= (int)nudDelayIncrement.Value;

                redDuration += (int)nudDurationIncrement.Value;
                greenDuration += (int)nudDurationIncrement.Value;
                whiteDuration += (int)nudDurationIncrement.Value;
            }
        }

        private void BlinkLight(int j)
        {
            AlterOutputPort(ifKitA, j, true);
            System.Threading.Thread.Sleep((int)nudDurationIncrement.Value);
            AlterOutputPort(ifKitA, j, false);
            System.Threading.Thread.Sleep((int)nudDelayIncrement.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    switch (j)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                            BlinkLight(j);                           
                            break;

                        default:
                            break;

                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                BlinkLight(6);
                BlinkLight(1);
                BlinkLight(4);
                BlinkLight(13);
                BlinkLight(5);
                BlinkLight(11);

                BlinkLight(12);
                BlinkLight(10);
                BlinkLight(2);
                BlinkLight(3);
                BlinkLight(9);
                BlinkLight(14);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)nudLoops.Value; i++)
            {
                BlinkLight(6);
                BlinkLight(14);
                BlinkLight(9);
                BlinkLight(1);
                BlinkLight(4);
                BlinkLight(3);
                BlinkLight(2);
                BlinkLight(13);
                BlinkLight(5);
                BlinkLight(10);
                BlinkLight(12);
                BlinkLight(11);
            }
        }

        private void PlayLights_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ifKitA != null)
            {
            	ifKitA.close();
            }
            
            if (ifKitB != null)
            {
                ifKitB.close();
            }

            if (ifKitC != null)
            {
                ifKitC.close();
            }
        }

    }
}
        #endregion
