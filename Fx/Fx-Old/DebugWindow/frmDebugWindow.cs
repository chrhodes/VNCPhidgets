using System;
using System.Windows.Forms;

namespace DebugWindow
{
    public partial class frmDebugWindow : Form
    {
        public frmDebugWindow()
        {
            InitializeComponent();
        }

        private void btnClearOutput_Click(System.Object sender, System.EventArgs e)
        {
            this.txtOutput.Clear();
        }

        private void chkDebugSQL_CheckedChanged(object sender, EventArgs e)
        {
            Common.DebugSQL = ((System.Windows.Forms.CheckBox)sender).Checked;
        }

        private void frmDebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Common.DeveloperMode = false;
            e.Cancel = true;
        }

        private void chkDebugValidation_CheckedChanged(object sender, EventArgs e)
        {
            Common.DebugValidation = ((System.Windows.Forms.CheckBox)sender).Checked;
        }

        private void chkDebugSubmission_CheckedChanged(object sender, EventArgs e)
        {
            Common.DebugSubmission = ((System.Windows.Forms.CheckBox)sender).Checked;
        }

        private void chkDebugEAI_CheckedChanged(object sender, EventArgs e)
        {
            Common.DebugEAI = ((System.Windows.Forms.CheckBox)sender).Checked;
        }

        //private void btnGetAllConfigInfo_Click(object sender, EventArgs e)
        //{
        //    Common.WriteToDebugWindow("MCR.Config Data");
        //    Common.WriteToDebugWindow(MCR.Config.GetAllConfigInfo());
        //    Common.WriteToDebugWindow("MCR_EAC.ConfigData Data");
        //    Common.WriteToDebugWindow(ConfigData.GetAllConfigInfo());
        //}

    }
}
