using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Runtime.InteropServices;  // DllImport()

//Needed for the InterfaceKit class, phidget base classes, and the PhidgetException class
using Phidgets;
//Needed for the event handling classes 
using Phidgets.Events;  

namespace Explore
{
    public partial class PlaySounds : Form
    {
        //Declare an InterfaceKit object
        static InterfaceKit ifKit;
        Boolean hasPlayed = false;

        Sounds soundPlayer1;
        Sounds soundPlayer2;
        MediaPlayer mediaPlayer1;
        MediaPlayer mediaPlayer2;
        MediaPlayer mediaPlayer3;
        MediaPlayer mediaPlayer4;

        private static Sounds GetSoundPlayer()
        {
            return new Sounds();
        }

        private static MediaPlayer GetMediaPlayer()
        {
            return new MediaPlayer();
        }

        public PlaySounds()
        {
            InitializeComponent();
        }

        #region Initialization

        private void Form1_Load(object sender, EventArgs e)
        {
            //InitializePhidget();
            soundPlayer1 = GetSoundPlayer();
            soundPlayer2 = GetSoundPlayer();
            mediaPlayer1 = GetMediaPlayer();
            mediaPlayer2 = GetMediaPlayer();
            mediaPlayer3 = GetMediaPlayer();
            mediaPlayer4 = GetMediaPlayer();

            mediaPlayer1.MediaEnded += new EventHandler(mediaPlayer1_MediaEnded);
            mediaPlayer2.MediaEnded += new EventHandler(mediaPlayer2_MediaEnded);
            mediaPlayer3.MediaEnded += new EventHandler(mediaPlayer3_MediaEnded);
            mediaPlayer4.MediaEnded += new EventHandler(mediaPlayer4_MediaEnded);

            distanceSensor1.FireInRangeEvent += new EventHandler<DistanceSensorEventArgs>(distanceSensor1_FireInRangeEvent);
            distanceSensor1.FireOutOfRangeEvent += new Sensor1101_GP2Y0A02YK.OutOfRangeEventHandler(distanceSensor1_FireOutOfRangeEvent);

            distanceSensor2.FireInRangeEvent += distanceSensor2_FireInRangeEvent;
            distanceSensor2.FireOutOfRangeEvent += distanceSensor2_FireOutOfRangeEvent;
        }

        private void distanceSensor1_FireInRangeEvent(object sender, DistanceSensorEventArgs e)
        {
            AlterOutputPort(0, true);
            txtSensor1Value.Text = e.Distance.ToString();

            if ( ! ckPlaying1.Checked)
            {
                ckPlaying1.Checked = true;
                PlayMedia(txtFileName1.Text, mediaPlayer1);
            }
        }

        private void distanceSensor1_FireOutOfRangeEvent()
        {
            AlterOutputPort(0, false);
            mediaPlayer1.Stop();
            ckPlaying1.Checked = false;
            txtSensor1Value.Text = "Off";
        }

        private void distanceSensor2_FireInRangeEvent(object sender, DistanceSensorEventArgs e)
        {
            AlterOutputPort(1, true);
            txtSensor2Value.Text = e.Distance.ToString();

            if ( ! ckPlaying2.Checked)
            {
                ckPlaying2.Checked = true;
                PlayMedia(txtFileName2.Text, mediaPlayer2);
            }
        }

        private void distanceSensor2_FireOutOfRangeEvent()
        {
            AlterOutputPort(1, false);
            mediaPlayer2.Stop();
            ckPlaying2.Checked = false;
            txtSensor2Value.Text = "Off";
        }

        private void InitializePhidget()
        {
            try
            {
                //Initialize the InterfaceKit object
                ifKit = new InterfaceKit();

                //Hook the basica event handlers
                ifKit.Attach += new AttachEventHandler(ifKit_Attach);
                ifKit.Detach += new DetachEventHandler(ifKit_Detach);
                ifKit.Error += new ErrorEventHandler(ifKit_Error);

                //Hook the phidget spcific event handlers
                //ifKit.InputChange += new InputChangeEventHandler(ifKit_InputChange);
                //ifKit.OutputChange += new OutputChangeEventHandler(ifKit_OutputChange);
                ifKit.SensorChange += new SensorChangeEventHandler(ifKit_SensorChange);

                //Open the object for device connections
                //ifKit.open();

                //Open the Phidget using the command line arguments

                openCmdLine(ifKit);

                //Wait for an InterfaceKit phidget to be attached
                Trace.WriteLine("Waiting for InterfaceKit to be attached...");
                ifKit.waitForAttachment();

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

        #endregion

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
            txtSensorEvents.AppendText(string.Format("Sensor index {0} value {1}{2}", e.Index, e.Value, Environment.NewLine));
            //Trace.WriteLine(string.Format("Sensor index {0} value {1}", e.Index, e.Value));
            //this.txtIndex.Text = e.Index.ToString();

            switch (e.Index)
            {
                case 0:
                    txtValue0.Text = e.Value.ToString();
                    distanceSensor1.changeDisplay(e.Value);
                    break;

                case 1:
                    txtValue1.Text = e.Value.ToString();
                    distanceSensor2.changeDisplay(e.Value);
                    break;

                case 2:
                    txtValue2.Text = e.Value.ToString();                    
                    break;

                case 3:
                    txtValue3.Text = e.Value.ToString();
                    motionSensor1.changeDisplay(e.Value);
                    break;
            }
        }

        #endregion
        #endregion

        #region Event Handlers

        private void btnLoadFile1_Click(object sender, EventArgs e)
        {
            txtFileName1.Text = GetFileName();
        }

        private void btnLoadFile2_Click(object sender, EventArgs e)
        {
            txtFileName2.Text = GetFileName();
        }

        private void btnLoadFile3_Click(object sender, EventArgs e)
        {
            txtFileName3.Text = GetFileName();
        }

        private void btnLoadFile4_Click(object sender, EventArgs e)
        {
            txtFileName4.Text = GetFileName();
        }

        private void btnPlayFile_Click(object sender, EventArgs e)
        {
            PlaySound(txtFileName1.Text, soundPlayer1);
        }

        private void btnPlayFile1_Click(object sender, EventArgs e)
        {
            ckPlaying1.Checked = true;
            ckPlay1Complete.Checked = false;
            PlayMedia(txtFileName1.Text, mediaPlayer1);


            //// this sleep is here just so you can distinguish the two sounds playing simultaneously 
            ////System.Threading.Thread.Sleep(500);
        }

        private void btnPlayFile2_Click(object sender, EventArgs e)
        {
            ckPlaying2.Checked = true;
            ckPlay2Complete.Checked = false;
            PlayMedia(txtFileName2.Text, mediaPlayer2);
        }

        private void btnPlayFile3_Click(object sender, EventArgs e)
        {
            ckPlaying3.Checked = true;
            ckPlay3Complete.Checked = false;
            PlayMedia(txtFileName3.Text, mediaPlayer3);
        }

        private void btnPlayFile4_Click(object sender, EventArgs e)
        {
            ckPlaying4.Checked = true;
            ckPlay4Complete.Checked = false;
            PlayMedia(txtFileName4.Text, mediaPlayer4);
        }

        private void btnStopPlay_Click(object sender, EventArgs e)
        {
            soundPlayer1.StopPlay();
        }

        private void btnStopPlay1_Click(object sender, EventArgs e)
        {
            mediaPlayer1.Stop();
            ckPlay1Complete.Checked = false;
            ckPlaying1.Checked = false;
        }

        private void btnStopPlay2_Click(object sender, EventArgs e)
        {
            mediaPlayer2.Stop();
            ckPlay2Complete.Checked = false;
            ckPlaying2.Checked = false;
        }

        private void btnStopPlay3_Click(object sender, EventArgs e)
        {
            mediaPlayer3.Stop();
            ckPlay3Complete.Checked = false;
            ckPlaying3.Checked = false;
        }

        private void btnStopPlay4_Click(object sender, EventArgs e)
        {
            mediaPlayer4.Stop();
            ckPlay4Complete.Checked = false;
            ckPlaying4.Checked = false;
        }

        private void ckHasPlayed_CheckedChanged(object sender, EventArgs e)
        {
            hasPlayed = ckHasPlayed.Checked;
        }

        private void ckOutput0_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(0, ckOutput0.Checked);
        }

        private void ckOutput1_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(1, ckOutput1.Checked);
        }

        private void ckOutput2_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(2, ckOutput2.Checked);
        }

        private void ckOutput3_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(3, ckOutput3.Checked);
        }

        private void ckOutput4_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(43, ckOutput4.Checked);
        }

        private void ckOutput5_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(5, ckOutput5.Checked);
        }

        private void ckOutput6_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(6, ckOutput6.Checked);
        }

        private void ckOutput7_CheckedChanged(object sender, EventArgs e)
        {
            AlterOutputPort(7, ckOutput7.Checked);
        }

        private void mediaPlayer1_MediaEnded(object sender, EventArgs e)
        {
            ckPlaying1.Checked = false;
            ckPlay1Complete.Checked = true;
        }

        private void mediaPlayer2_MediaEnded(object sender, EventArgs e)
        {
            ckPlaying2.Checked = false;
            ckPlay2Complete.Checked = true;
        }

        private void mediaPlayer3_MediaEnded(object sender, EventArgs e)
        {
            ckPlaying3.Checked = false;
            ckPlay3Complete.Checked = true;
        }

        private void mediaPlayer4_MediaEnded(object sender, EventArgs e)
        {
            ckPlaying4.Checked = false;
            ckPlay4Complete.Checked = true;
        }

        private void nudSensor1Max_ValueChanged(object sender, EventArgs e)
        {
            distanceSensor1.MaxRange = (double)nudSensor1Max.Value;
        }

        private void nudSensor1Min_ValueChanged(object sender, EventArgs e)
        {
            distanceSensor1.MinRange = (double)nudSensor1Min.Value;
        }

        private void nudSensor2Max_ValueChanged(object sender, EventArgs e)
        {
            distanceSensor2.MaxRange = (double)nudSensor1Max.Value;
        }

        private void nudSensor2Min_ValueChanged(object sender, EventArgs e)
        {
            distanceSensor2.MinRange = (double)nudSensor1Min.Value;
        }

        //Modify the sensitivity of the analog inputs. In other words, 
        // the amount that the inputs must change before triggering sensorchange events.
        private void trkSensitivity_Scroll(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < ifKit.sensors.Count; i++)
                {
                    ifKit.sensors[i].Sensitivity = trkSensitivity.Value;
                }

                txtSenstivity.Text = trkSensitivity.Value.ToString();
            }
            catch (PhidgetException ex)
            {
                MessageBox.Show(ex.Description);
            }
        }




        #endregion

        #region Main Function Routines

        private void AlterOutputPort(int portNumber, bool value)
        {
            ifKit.outputs[portNumber] = value;
        }

        private string GetFileName()
        {
            openFileDialog1.Title = "Select a Wave Sound File";
            openFileDialog1.Filter = "Wav Files(*.wav)|*.wav";
            openFileDialog1.InitialDirectory = cbInitialDirectory.Text;

            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        private void PlaySound(string soundFileName, Sounds sound)
        {
            sound.Play(soundFileName, sound.SND_FILENAME | sound.SND_ASYNC);
        }

        private void PlayMedia(string soundFileName, MediaPlayer mediaPlayer)
        {
            mediaPlayer.Open(new System.Uri(soundFileName));
            mediaPlayer.Play();            
        }

        #endregion

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            InitializePhidget();
        }
    }
}
