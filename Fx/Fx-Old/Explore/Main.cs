using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Explore
{
    public partial class MainMenu : Form
    {
        const string CONTROL_NAME = "MainMenu";

        public MainMenu()
        {
            InitializeComponent();
        }

        protected void InitializeApplicationEventHandlers()
        {
#if TRACE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            ApplicationEventHandler.Init();
            ApplicationEventHandler.ToggleF11Event += this.ToggleDebugMode;
            ApplicationEventHandler.ToggleF12Event += this.ToggleDeveloperMode;
        }

        /// <summary>
        /// Perform any operations that are needed when DebugMode switches state.
        /// </summary>
        protected void ToggleDebugMode()
        {
#if TRACE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif

            Common.DebugMode = ! Common.DebugMode;
            DebugWindow.Common.DebugMode = ! DebugWindow.Common.DebugMode;

            if(Common.DebugMode)
            {
                // TODO:
            }
            else
            {
                // TODO:
            }
        }

        /// <summary>
        /// Perform any operations that are needed when DeveloperMode switches state.
        /// </summary>
        protected void ToggleDeveloperMode()
        {
#if TRACE
            Trace.WriteLine(string.Format("{0}:{1}()", CONTROL_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif

            Common.DeveloperMode = ! Common.DeveloperMode;
            DebugWindow.Common.DeveloperMode = !DebugWindow.Common.DeveloperMode;

            if(Common.DeveloperMode)
            {
                this.BackColor = Color.LightCoral;
                DebugWindow.Common.DebugWindow.Show();
            }
            else
            {
                this.BackColor = SystemColors.ControlLight;
                DebugWindow.Common.DebugWindow.Hide();
            }
        }

        #region Event Handlers

        private void btnLightShow_Click(object sender, EventArgs e)
        {
            DebugWindow.Common.WriteToDebugWindow("Showing LightShow form");
            LightShow frm = new LightShow();
            frm.Show();
        }

        private void btnSounds_Click(object sender, EventArgs e)
        {
            DebugWindow.Common.WriteToDebugWindow("Showing PlaySounds form");
            PlaySounds frm = new PlaySounds();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DebugWindow.Common.WriteToDebugWindow("Showing PlayLights form");
            PlayLights frm = new PlayLights();
            frm.Show();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            InitializeApplicationEventHandlers();
        }

        #endregion

    }
}
