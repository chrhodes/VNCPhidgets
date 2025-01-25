using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Phidget22;
using Phidget22.Events;

namespace LoadCellCalibrator
{
    public partial class Form1 : Form
    {
        VoltageRatioInput ratio = null; //declare our voltageratio object that will be used to connect to the bridge device
        double x1, x2, y1, y2, m, b; //our data points as well as the calculated slope and intercept

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(String[] commandLine)
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ratio = new VoltageRatioInput(); //create an instance of the object

            //register the pertinent event handlers
            ratio.Attach += ratio_attach; 
            ratio.Detach += ratio_detach;
            ratio.VoltageRatioChange += ratio_change;

            try
            {
                ratio.Open(); //open the device
            }
            catch (PhidgetException ex)
            {//let user know if there's an error while opening
                warningIcon.Text = "Error connecting to device: " + ex.Description;
                warningIcon.Visible = true;
                warningTimer.Start();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ratio.Close(); //clean up to prevent issues down the line
        }


        void ratio_attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            VoltageRatioInput attached = (VoltageRatioInput)sender;
            waitingLbl.Visible = false; //device is attached so we can hide the waiting notification
            box1.Enabled = true; //allow for user to interact with the first step
            serialNumber.Text = "Attached: " + attached.DeviceSerialNumber.ToString(); //populate serial field
            channelAttached.Text = "Channel: " + attached.Channel.ToString(); //populate the channel field
            channelSelect.Enabled = true; //allow user to change which channel is open
            channelSelect.DropDownItems.Clear();
            if(attached.DeviceID == DeviceID.PN_1046)//1046 has 4 channels, populate menu appropriately
            {
                for(int i = 0; i < 4; i++)
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem(i.ToString());
                    newItem.Click += channelSelect_Click;
                    channelSelect.DropDownItems.Add(newItem);
                }
            }
            else if(attached.DeviceID == DeviceID.PN_DAQ1500)//daq1500 has 2 channels
            {
                for (int i = 0; i < 2; i++)
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem(i.ToString());
                    newItem.Click += channelSelect_Click;
                    channelSelect.DropDownItems.Add(newItem);
                }
            }
        }

        private void warningTimer_Tick(object sender, EventArgs e)
        { //only show warning messages for a short time then remove them
            warningIcon.Visible = false;
            warningTimer.Stop();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e) //provide an option to reset the form if something goes wrong midway through or you want to generate a new calibration without having to close and reopen the device
         {
            box1.Enabled = true;
            box2.Enabled = false;
            outputBox.Enabled = false;
            foreach (Control gb in this.Controls)
            {
                foreach (var text in gb.Controls)
                {
                    if (text is TextBox)
                        ((TextBox)text).Text = string.Empty;
                }
            }
            load1.Text = "0";
        }

        void ratio_detach(object sender, Phidget22.Events.DetachEventArgs e)
        {//reset form when device detaches
            serialNumber.Text = "Detached";
            channelAttached.Text = "";
            box1.Enabled = false;
            waitingLbl.Visible = true;
            ratio.Channel = 0;
        }

        void ratio_change(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            if (outputBox.Enabled)
                correctedLoad.Text = ((m * e.VoltageRatio) + b).ToString("f3"); //if we have input the necessary information we can output a load value as data comes in
        }

        void point1_Click(object sender, EventArgs e) //in step one we are taking one data point
        {
            try
            {
                y1 = double.Parse(load1.Text); //our first y value is whatever the user has input as the force being applied (should be 0 for the first step almost every time)
            }
            catch (Exception) { load1.Text = "Invalid Input"; return; }
            try
            {
                x1 = ratio.VoltageRatio; //first x value is whatever the device is measuring as the output voltage/supply voltage.
            }
            catch(PhidgetException ex) {
                warningIcon.Text = "Error taking measurement: " + ex.Description;
                warningIcon.Visible = true;
                warningTimer.Start();
                return;
            }

            measurement1.Text = x1.ToString("e4"); //show the measured value for user reference (not actually necessary)
            box2.Enabled = true; //make the second measurement available
            box1.Enabled = false; //disable this one so there is only 1 step active at a given time for ease of use
        }

        void point2_Click(object sender, EventArgs e)
        {
            try
            {
                y2 = double.Parse(load2.Text); //our second y value is also whatever the user inputs as the force applied.  this should be non-zero in most cases and ideally as close to the maximum range of the load cell as you can find a known weight for.
            }
            catch (Exception) { load2.Text = "Invalid Input"; return; }
            try
            {
                x2 = ratio.VoltageRatio;  //take another measurement from the device and use that for our second x value
            }
            catch (PhidgetException ex)
            {
                warningIcon.Text = "Error taking measurement: " + ex.Description;
                warningIcon.Visible = true;
                warningTimer.Start();
                return;
            }

            measurement2.Text = x2.ToString("e4");//again, we are showing the value for user reference only, this isn't actually necessary
            box2.Enabled = false; //disable this box and enable the output box
            outputBox.Enabled = true;

            m = (y2 - y1) / (x2 - x1); //our slope is y delta over x delta
            b = y1 - (m * x1); //for intercept, just plug in a test case (our first data point for example) and solve y=mx+b for b.  
            bTxt.Text = b.ToString("f3"); //display parameters so user can make note of them.
            mTxt.Text = m.ToString("f3");
        }

        void channelSelect_Click(object sender, EventArgs e) //menu lets the user change the channel of the device they are opening so that they can calibrate multiple load cells connected to a single bridge unit
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (int.Parse(item.Text) != ratio.Channel) //don't change if the channel selected is already open
            {
                ratio.Close(); //close current channel and reset form
                serialNumber.Text = "Detached";
                channelAttached.Text = "";
                box1.Enabled = false;
                box2.Enabled = false;
                outputBox.Enabled = false;
                foreach (Control gb in this.Controls)
                {
                    foreach (var text in gb.Controls)
                    {
                        if (text is TextBox)
                            ((TextBox)text).Text = string.Empty;
                    }
                }
                load1.Text = "0";
                waitingLbl.Visible = true;
                ratio.Channel = int.Parse(item.Text); //grab new channel to open
                ratio.Open(); //open it
            }
            else
            {
                warningIcon.Text = "Channel already opened!";
                warningIcon.Visible = true;
                warningTimer.Start();
            }
        }
    }
}