using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.ServiceModel;

namespace DebugLightGridApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ServiceHost serviceHost = new ServiceHost(typeof(frmLightGrid));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //serviceHost.Open();
            frmLightGrid lightGrid = new frmLightGrid();
            ServiceHost serviceHost = new ServiceHost(lightGrid);
            serviceHost.Open();
            lightGrid.DisplayHostInfo(serviceHost);
            Application.Run(lightGrid);
        }
    }
}
