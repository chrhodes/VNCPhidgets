/* - MotorControl full -
 * This example demonstrates thebasic usage of a Phidget MotorControl.  It demonstrates the manipulation of the velocity and
 * acceleration of the motor, as well as displays the data from generated events.
 *
 * Please note that this example was designed to work with only one Phidget MotorControl connected. 
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
using Phidgets; //Needed for the MotorControl class, Phidget base classes, and the PhidgetException class
using Phidgets.Events; //Needed for the Phidget event handling classes
using System.Collections;

namespace MotorControl_full
{
    public partial class Form1 : Form
    {
        private MotorControl motoControl; //Declare a MotorControl object
        private ErrorEventBox errorBox;
        private ArrayList inputArray;

        public Form1()
        {
            InitializeComponent();
            errorBox = new ErrorEventBox();
        }

        //initialize the MotorConrol object and hook the event handlers
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
                ((CheckBox)inputArray[i]).Enabled = false;

            }

            velocityTrk.SetRange(-100, 100); //The minimum velocity for a motor is -100, the maximum is 100

            motoControl = new MotorControl();

            motoControl.Attach += new AttachEventHandler(motoControl_Attach);
            motoControl.Detach += new DetachEventHandler(motoControl_Detach);
            motoControl.Error += new ErrorEventHandler(motoControl_Error);

            motoControl.CurrentChange += new CurrentChangeEventHandler(motoControl_CurrentChange);
            motoControl.InputChange += new InputChangeEventHandler(motoControl_InputChange);
            motoControl.VelocityChange += new VelocityChangeEventHandler(motoControl_VelocityChange);

            openCmdLine(motoControl);
        }

        //MotorControl Attach event handler...populate the fields and controls
        void motoControl_Attach(object sender, AttachEventArgs e)
        {
            MotorControl attached = (MotorControl)sender;
            attachedTxt.Text = attached.Attached.ToString();
            nameTxt.Text = attached.Name;
            serialTxt.Text = attached.SerialNumber.ToString();
            versiontxt.Text = attached.Version.ToString();
            numMotorsTxt.Text = attached.motors.Count.ToString();
            numInputsTxt.Text = attached.inputs.Count.ToString();

            accelTrk.SetRange((int)attached.motors[0].AccelerationMin + 1, (int)attached.motors[0].AccelerationMax); //The minimum acceleration for a motor is 0, the maximum is 100
            accelTrk.Value = accelTrk.Minimum;

            int i;
            for (i = 0; i < attached.inputs.Count; i++)
            {
                ((CheckBox)inputArray[i]).Visible = true;
                ((CheckBox)inputArray[i]).Enabled = true;
            }

            for (i = 0; i < attached.motors.Count; i++)
            {
                motorCmb.Items.Add(i);
            }

            if (attached.Name == "Phidget Low Voltage Motor Controller 2-motor 4-input")
            {
                cChangeTxt.Clear();
                cChangeTxt.Visible = false;
                label12.Visible = false;
            }
            else
            {
                cChangeTxt.Clear();
                cChangeTxt.Visible = true;
                label12.Visible = true;
            }

            motorCmb.SelectedIndex = 0;
        }

        //MotorControl Detach event handler...Clear all the fields and disable all the controls
        void motoControl_Detach(object sender, DetachEventArgs e)
        {
            MotorControl detached = (MotorControl)sender;
            attachedTxt.Text = detached.Attached.ToString();
            nameTxt.Clear();
            serialTxt.Clear();
            versiontxt.Clear();
            numMotorsTxt.Clear();
            numInputsTxt.Clear();

            int i;
            for (i = 0; i < 4; i++)
            {
                ((CheckBox)inputArray[i]).Visible = false;
                ((CheckBox)inputArray[i]).Enabled = false;
            }

            motorCmb.SelectedIndex = 0;
            motorCmb.Items.Clear();
            motorCmb.Enabled = false;

            cChangeTxt.Clear();
            cChangeTxt.Visible = false;
            label12.Visible = false;
        }

        //error handler...display the error description in a messagebox
        void motoControl_Error(object sender, ErrorEventArgs e)
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

        //Current Change listener...display the data from a current change event
        void motoControl_CurrentChange(object sender, CurrentChangeEventArgs e)
        {
            if (e.Index == (int)motorCmb.SelectedItem)
            {
                cChangeTxt.Text = e.Current.ToString();
            }
        }

        //Input change event handler...display the result of a digital input change
        void motoControl_InputChange(object sender, InputChangeEventArgs e)
        {
            ((CheckBox)inputArray[e.Index]).Checked = e.Value;
        }

        //Velocity Change event handler...Display the velocity data after a Velocity change
        void motoControl_VelocityChange(object sender, VelocityChangeEventArgs e)
        {
            if (e.Index == (int)motorCmb.SelectedItem)
            {
                vCurrentTxt.Text = motoControl.motors[e.Index].Velocity.ToString();
                //vCurrentTxt.Text = e.Velocity.ToString();
            }
        }

        //When a motor is selected, display its current velocity and acceleration data as well as the trakbar controls
        private void motorCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                velocityTrk.Value = (int)motoControl.motors[(int)motorCmb.SelectedItem].Velocity;
            }
            catch (PhidgetException)
            {
                velocityTrk.Value = 0;
            }
            try
            {
                accelTrk.Value = (int)motoControl.motors[(int)motorCmb.SelectedItem].Acceleration;
            }
            catch (PhidgetException)
            {
                accelTrk.Value = accelTrk.Minimum;
            }
            vMaxTxt.Text = velocityTrk.Value.ToString();
            vCurrentTxt.Text = velocityTrk.Value.ToString();
            AccelTxt.Text = accelTrk.Value.ToString();
        }

        //When the acceleration trakbar is changed send this data to the motor control to affect the motor
        //If no motor control is attached, a PhidgetException will be thrown, so we will catch it and deal
        //with it accordingly
        private void accelTrk_Scroll(object sender, EventArgs e)
        {
            try
            {
                motoControl.motors[(int)motorCmb.SelectedItem].Acceleration = accelTrk.Value;
                AccelTxt.Text = accelTrk.Value.ToString();
            }
            catch (PhidgetException)
            {
                MessageBox.Show("No PhidgetMotorControl Connected!");
                attachedTxt.Text = "false";
                nameTxt.Clear();
                serialTxt.Clear();
                versiontxt.Clear();
                numMotorsTxt.Clear();
                numInputsTxt.Clear();

                int i;
                for (i = 0; i < 4; i++)
                {
                    ((CheckBox)inputArray[i]).Visible = false;
                    ((CheckBox)inputArray[i]).Enabled = false;
                }

                motorCmb.SelectedIndex = 0;
                motorCmb.Items.Clear();
                motorCmb.Enabled = false;
            }
        }

        //When the velocity trakbar is changed send this data tothe motor control to affect the motor
        //If no motor control is attached, a PhidgetException will be thrown, so we will catch it and deal
        //with it accordingly
        private void velocityTrk_Scroll(object sender, EventArgs e)
        {
            
            try
            {
                vMaxTxt.Text = velocityTrk.Value.ToString();
                motoControl.motors[(int)motorCmb.SelectedItem].Velocity = velocityTrk.Value;
            }
            catch (PhidgetException)
            {
                MessageBox.Show("No PhidgetMotorControl Connected!");
                attachedTxt.Text = "false";
                nameTxt.Clear();
                serialTxt.Clear();
                versiontxt.Clear();
                numMotorsTxt.Clear();
                numInputsTxt.Clear();

                int i;
                for (i = 0; i < 4; i++)
                {
                    ((CheckBox)inputArray[i]).Visible = false;
                    ((CheckBox)inputArray[i]).Enabled = false;
                }

                motorCmb.SelectedIndex = 0;
                motorCmb.Items.Clear();
                motorCmb.Enabled = false;
            }
        }

        //When the form is being close, make sure to stop all the motors and close the Phidget.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            motoControl.Attach -= new AttachEventHandler(motoControl_Attach);
            motoControl.Detach -= new DetachEventHandler(motoControl_Detach);
            motoControl.Error -= new ErrorEventHandler(motoControl_Error);

            motoControl.InputChange -= new InputChangeEventHandler(motoControl_InputChange);
            motoControl.CurrentChange -= new CurrentChangeEventHandler(motoControl_CurrentChange);
            motoControl.VelocityChange -= new VelocityChangeEventHandler(motoControl_VelocityChange);

            if (motoControl.Attached)
            {
                foreach (MotorControlMotor motor in motoControl.motors)
                {
                    motor.Velocity = 0;
                }
            }

            //run any events in the message queue - otherwise close will hang if there are any outstanding events
            Application.DoEvents();

            motoControl.close();

            motoControl = null;
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