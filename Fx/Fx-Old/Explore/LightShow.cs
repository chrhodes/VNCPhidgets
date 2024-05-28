using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using Fx;

namespace Explore
{
    public partial class LightShow : Form
    {
        #region Initialization

        public LightShow()
        {
            InitializeComponent();
        }

        #endregion

        FxShow fxShow;
        //DebugLightGrid.frmLightGrid lightGrid; // = new DebugLightGrid.frmLightGrid();

        #region Event Handlers

        private void btnFindShow_Click(object sender, EventArgs e)
        {
            FindShow();
        }

        private void btnLightsOff_Click(object sender, EventArgs e)
        {
            fxShow.LightsOff(int.Parse(txtOffDelay.Text));
        }

        private void btnLightsOn_Click(object sender, EventArgs e)
        {
            fxShow.LightsOn(int.Parse(txtOnDelay.Text));
        }

        private void btnLoadShow_Click(object sender, EventArgs e)
        {
            LoadShow(String.Format("{0}\\{1}", cbShowLocations.Text, txtShow.Text));
        }

        private void btnLoadShows_Click(object sender, EventArgs e)
        {
            LoadShows();
        }


        private void btnPresentShow_Click(object sender, EventArgs e)
        {
            PresentShow();
        }

        private void Show2_Click(object sender, EventArgs e)
        {
            Show2();
        }
        #endregion

        #region Main Function Routines

        private void FindShow()
        {
            MessageBox.Show("NotImplementedException()");
        }

        private void LoadShow(string showPath)
        {
            DebugWindow.Common.WriteToDebugWindow("LoadShow()");

            try
            {
                XElement showXml = XElement.Load(showPath);
                fxShow = new FxShow();
                fxShow.ShowXml = showXml;

                if (rbUseLightGrid.Checked)
                {
                    FxShow.UseDebugLightGrid = true;
                    //FxShow.DebugLightGrid = lightGrid;
                    //lightGrid.Show();
                }
                else
                {
                    FxShow.UseDebugLightGrid = false;
                    //lightGrid.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load showXml from path");
                MessageBox.Show(ex.ToString());
            }

            try
            {
                fxShow.Prepare();

                txtShowName.Text = fxShow.Name;
                txtShowDescription.Text = fxShow.Description;
                txtSetDelay.Text = fxShow.SetDelay.ToString();
                txtShowDuration.Text = fxShow.Duration;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot Prepare fxShow");
            }
        }

        private void LoadShows()
        {
            DebugWindow.Common.WriteToDebugWindow("LoadShows()");
        }

        private void PresentShow()
        {
            DebugWindow.Common.WriteToDebugWindow("PresentShow()");

            Random rnd = new Random();

            for (int i = 0; i < int.Parse(txtLoops.Text); i++)
            {

                rnd.Next(100, 250);
                int onDelay = int.Parse(txtOnDelay.Text);
                int offDelay = int.Parse(txtOffDelay.Text);

                onDelay += rnd.Next(100, 250);
                offDelay += rnd.Next(100, 250);

                fxShow.LightsOn(onDelay);
                fxShow.LightsOff(offDelay);               
            }

            //fxShow.Present();
        }

        private void Show2()
        {
            DebugWindow.Common.WriteToDebugWindow("LoadShow()");
            Random rnd = new Random();

            for (int i = 0; i < int.Parse(txtLoops.Text); i++)
            {
                int onDelay = int.Parse(txtOnDelay.Text);
                int offDelay = int.Parse(txtOffDelay.Text);
                int colorChoice = rnd.Next(1, 4);

                onDelay += rnd.Next(0, 50);
                offDelay += rnd.Next(0, 250);

                switch (colorChoice)
                {
                    case 1:
                        fxShow.LightsOn(onDelay, "Red");
                        fxShow.LightsOff(0, "Green");
                        fxShow.LightsOff(0, "White");   
                        break;

                    case 2:
                        fxShow.LightsOff(0, "Red");
                        fxShow.LightsOn(onDelay, "Green");
                        fxShow.LightsOff(0, "White");
                        break;

                    case 3:
                        fxShow.LightsOff(0, "Red");
                        fxShow.LightsOff(0, "Green");
                        fxShow.LightsOn(onDelay, "White");
                        break;
                
                }

                System.Threading.Thread.Sleep(offDelay);
            }
        }

        #endregion

        private void btnPresentFxShow_Click(object sender, EventArgs e)
        {
            fxShow.Present();
        }

        //private void LightShow_Load(object sender, EventArgs e)
        //{
        //}

        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    lightGrid = new DebugLightGrid.frmLightGrid();
        //}

        //private void btnLoadLightGrid_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        backgroundWorker1.RunWorkerAsync();
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(string.Format("Exception: {0}.{1}() - {2}",
        //                     System.Reflection.Assembly.GetExecutingAssembly().FullName,
        //                     System.Reflection.MethodInfo.GetCurrentMethod().Name,
        //                     ex.ToString()
        //                     ));  
        //    }
        //}

    }
}
