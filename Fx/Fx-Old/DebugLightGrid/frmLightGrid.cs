using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugLightGrid
{
    public partial class frmLightGrid : Form
    {
        public frmLightGrid()
        {
            InitializeComponent();
        }

        public void On(int displayLightId)
        {
            ucLightGrid1.On(displayLightId);
        }

        public void Off(int displayLightId)
        {
            ucLightGrid1.Off(displayLightId);            
        }
    }
}
