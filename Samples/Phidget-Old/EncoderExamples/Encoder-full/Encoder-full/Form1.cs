/* - Encoder full -
 * This example simply displays the attached Phidget Encoder's details and when the encoder is manipulated, will
 * display the resulting data to the specified fields.
 *
 * Please note that this example was designed to work with only one Phidget Encoder connected. 
 * For an example showing how to use two Phidgets of the same time concurrently, please see the
 * Servo-multi example in the Servo Examples.
 *
 * Copyright 2007 Phidgets Inc.  
 * This work is licensed under the Creative Commons Attribution 2.5 Canada License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.5/ca/
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phidgets; //Needed for the Encoder class and the PhidgetException class
using Phidgets.Events; //Needed for the event handling classes
using System.Collections;

namespace Encoder_full
{
    public partial class Form1 : Form
    {
        Phidgets.Encoder encoder; //Declare an encoder object
        private ArrayList inputArray;
        private ErrorEventBox errorBox;

        public Form1()
        {
            InitializeComponent();
            errorBox = new ErrorEventBox();
        }

        //We'll create our encoder object and initialize the event handlers and open the Phidget Encoder
        //A small note:  since we are doing this in the form load method, we don't have to worry about 
        //waiting for attach on the phidget as this would prevent the form from loading until you have an 
        //encoder attached.  I have programmd so that the form can fully load without consequence and 
        //everything will work once an Encoder is attached.
        private void Form1_Load(object sender, EventArgs e)
        {
            inputArray = new ArrayList();
            inputArray.Add(input0Chk);
            inputArray.Add(input1Chk);
            inputArray.Add(input2Chk);
            inputArray.Add(input3Chk);

            for (int i = 0; i < 4; i++)
            {
                ((CheckBox)inputArray[i]).Visible = false;
                ((CheckBox)inputArray[i]).AutoCheck = false;
                ((CheckBox)inputArray[i]).Enabled = true;
            }

            encoder = new Phidgets.Encoder();

            encoder.Attach += new AttachEventHandler(encoder_Attach);
            encoder.Detach += new DetachEventHandler(encoder_Detach);
            encoder.Error += new ErrorEventHandler(encoder_Error);
            
            encoder.InputChange += new InputChangeEventHandler(encoder_InputChange);
            encoder.PositionChange += new EncoderPositionChangeEventHandler(encoder_PositionChange);

            openCmdLine(encoder);
        }

        //When the application is being terminated, close the Phidget.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Unhook the event handlers
            encoder.Attach -= new AttachEventHandler(encoder_Attach);
            encoder.Detach -= new DetachEventHandler(encoder_Detach);
            encoder.Error -= new ErrorEventHandler(encoder_Error);
            encoder.PositionChange -= new EncoderPositionChangeEventHandler(encoder_PositionChange);
            encoder.InputChange -= new InputChangeEventHandler(encoder_InputChange);

            //run any events in the message queue - otherwise close will hang if there are any outstanding events
            Application.DoEvents();

            encoder.close();
        }

        //Attach event handler...populate our encoder info fields and enable our editable fields
        void encoder_Attach(object sender, AttachEventArgs e)
        {
            Phidgets.Encoder attached = (Phidgets.Encoder)sender;
            attachedTxt.Text = attached.Attached.ToString();
            nameTxt.Text = attached.Name;
            serialTxt.Text = attached.SerialNumber.ToString();
            versionTxt.Text = attached.Version.ToString();
            numEncodersTxt.Text = attached.encoders.Count.ToString();
            numInputsTxt.Text = attached.inputs.Count.ToString();

            for (int i = 0; i < attached.inputs.Count; i++)
            {
                ((CheckBox)inputArray[i]).Visible = true;
            }

            for (int i = 0; i < attached.encoders.Count; i++)
                encoderCmb.Items.Add(i);

            encoderCmb.SelectedIndex = 0;
            encoderCmb.Enabled = true;

            switch (attached.ID)
            {
                case Phidget.PhidgetID.ENCODER_HS_4ENCODER:
                    enabledChk.Enabled = true;
                    timeChangeLabel.Text = "Time since last change (us):";
                    break;
                case Phidget.PhidgetID.ENCODER_HS_1ENCODER:
                case Phidget.PhidgetID.ENCODER_1ENCODER_1INPUT:
                    enabledChk.Enabled = false;
                    timeChangeLabel.Text = "Time since last change (ms):";
                    indexPositionTxt.Text = "Unsupported";
                    break;
                default:
                    break;
            }
        }

        //Detach event code...We'll clear our display fields and disable our editable fields while device is not attached
        //as trying to communicate with the device while not attached will generate a PhidgetException.  In this example,
        //I have coded so that this should not occur, but best practice would be to catch it and handle it accordingly
        void encoder_Detach(object sender, DetachEventArgs e)
        {
            Phidgets.Encoder detached = (Phidgets.Encoder)sender;
            attachedTxt.Text = detached.Attached.ToString();
            nameTxt.Text = "";
            serialTxt.Text = "";
            versionTxt.Text = "";
            numEncodersTxt.Text = "";
            numInputsTxt.Text = "";
            positionTxt.Text = "";
            encoderPositionTxt.Text = "";
            timeTxt.Text = "";
            velocityTextBox.Text = "";

            for (int i = 0; i < 4; i++)
            {
                ((CheckBox)inputArray[i]).Visible = false;
            }
            enabledChk.Enabled = false;
            enabledChk.Checked = false;
            indexPositionTxt.Clear();
            encoderCmb.Enabled = false;
            encoderCmb.Items.Clear();
        }

        //Error event handler...we'll just send the description text to a popup message box for this example
        void encoder_Error(object sender, ErrorEventArgs e)
        {
            Phidget phid = (Phidget)sender;
            DialogResult result;
            switch (e.Type)
            {
                case PhidgetException.ErrorType.PHIDGET_ERREVENT_BADPASSWORD:
                    phid.close();
                    TextInputBox dialog = new TextInputBox("Error Event",
                        "Authentication error: This server requires a password.", "Please enter the password, or cancel.");
                    result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                        openCmdLine(phid, dialog.password);
                    else
                        Environment.Exit(0);
                    break;
                default:
                    if (!errorBox.Visible)
                        errorBox.Show();
                    break;
            }
            errorBox.addMessage(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + e.Description);
        }

        //Input Change event handler....on the current Phidget Encoders, this means the pushbutton in the knob.
        //Event arguements contain the input index (on current Phidget Encoders will only be 1) and the value, a bool
        //to represent clicked or unlcicked state
        void encoder_InputChange(object sender, InputChangeEventArgs e)
        {
            ((CheckBox)inputArray[e.Index]).Checked = e.Value;
        }

        //Encoder Position Change event handler...the event arguements will provide the encoder index, value, and 
        //the elapsed time since the last event.  These value, including the current position value stored in the
        //corresponding element in the encoder objects encoder collection could be used to calculate velocity...
        const int velQueueSize = 50;
        Queue velData = new Queue(velQueueSize);
        void encoder_PositionChange(object sender, EncoderPositionChangeEventArgs e)
        {
            int index = (int)encoderCmb.SelectedItem;
            if (index == e.Index)
            {
                positionTxt.Text = e.PositionChange.ToString();

                try
                {
                    timeTxt.Text = e.Time.ToString();
                }
                catch
                {
                    timeTxt.Text = "Unknown";
                }
                encoderPositionTxt.Text = encoder.encoders[e.Index].ToString();

                // Velocity calculated in counts per second - averaged over 50 samples
                double veloc = 0;
                try
                {
                    veloc = (((double)e.PositionChange * 1000000) / ((double)e.Time * 4));
                }
                catch
                {

                }
                if (velData.Count == velQueueSize)
                    velData.Dequeue();
                velData.Enqueue(veloc);
                double averageVel = 0;
                int velCount = 0;
                foreach (double d in velData)
                {
                    averageVel += d;
                    velCount++;
                }
                averageVel /= velCount;
                velocityTextBox.Text = averageVel.ToString("0.#");

                switch (encoder.ID)
                {
                    case Phidget.PhidgetID.ENCODER_HS_4ENCODER:
                        try
                        {
                            indexPositionTxt.Text = encoder.encodersWithEnable[e.Index].IndexPosition.ToString();
                        }
                        catch (PhidgetException)
                        {
                            indexPositionTxt.Text = "Unknown";
                        }
                        break;
                }
            }
        }

        private void encoderCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = (int)encoderCmb.SelectedItem;

            positionTxt.Text = "";
            timeTxt.Text = "";
            velocityTextBox.Text = "";
            velData.Clear();

            enabledChk.Checked = encoder.encodersWithEnable[index].Enabled;
            try
            {
                encoderPositionTxt.Text = encoder.encodersWithEnable[index].Position.ToString();
            }
            catch (PhidgetException)
            {
                encoderPositionTxt.Text = "Unknown";
            }
            switch (encoder.ID)
            {
                case Phidget.PhidgetID.ENCODER_HS_4ENCODER:
                    try
                    {
                        indexPositionTxt.Text = encoder.encodersWithEnable[index].IndexPosition.ToString();
                    }
                    catch (PhidgetException)
                    {
                        indexPositionTxt.Text = "Unknown";
                    }
                    break;
            }
        }

        private void enabledChk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int index = (int)encoderCmb.SelectedItem;
                encoder.encodersWithEnable[index].Enabled = enabledChk.Checked;
            }
            catch { }
        }

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
            catch { }
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
        #endregion

    }
}