using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceModel;
using WindowsFormsExtensionMethods;

namespace DebugLightGridApplication
{
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class frmLightGrid : Form, IDisplayLight
    {
        //ServiceHost serviceHost = new ServiceHost(typeof(ucLightGrid));
        //ServiceHost serviceHost = new ServiceHost(typeof(frmLightGrid));

        public frmLightGrid()
        {
            InitializeComponent();
        }

        public void On(int displayLightId)
        {
            txtOutput.AppendTextWithNewLine(string.Format("On {0}", displayLightId));
            ucLightGrid1.On(displayLightId);
        }

        public void Off(int displayLightId)
        {
            txtOutput.AppendTextWithNewLine(string.Format("Off {0}", displayLightId));
            ucLightGrid1.Off(displayLightId);
        }

        private void frmLightGrid_Load(object sender, EventArgs e)
        {
            //using(ServiceHost serviceHost = new ServiceHost(typeof(ucLightGrid)))
            //{
                // Open the host and start listening for messages
                //serviceHost.Open();
                //DisplayHostInfo(serviceHost);
            //}
        }

        public void DisplayHostInfo(ServiceHost host)
        {
            txtOutput.AppendTextWithNewLine(" --- Host Info ---");
            txtOutput.AppendTextWithNewLine(GetHostInfoString(host));
            txtOutput.AppendTextWithNewLine(" --- End Host Info ---");
        }

        private string GetHostInfoString(ServiceHost host)
        {
            StringBuilder info = new StringBuilder();

            foreach(System.ServiceModel.Description.ServiceEndpoint se in host.Description.Endpoints)
            {
                // TODO: There are more interesting properties here.

                info.AppendLine(string.Format(" Name:                   {0}", se.Name));
                info.AppendLine(string.Format(" Address:                {0}", se.Address));
                info.AppendLine(string.Format(" Behaviors:              {0}", se.Behaviors.ToString()));
                info.AppendLine(string.Format(" Binding.Name:           {0}", se.Binding.Name));
                info.AppendLine(string.Format(" Binding.CloseTimeout:   {0}", se.Binding.CloseTimeout.ToString()));
                info.AppendLine(string.Format(" Binding.MessageVersion: {0}", se.Binding.MessageVersion.ToString()));
                info.AppendLine(string.Format(" Binding.Namespace:      {0}", se.Binding.Namespace));
                info.AppendLine(string.Format(" Binding.OpenTimeout:    {0}", se.Binding.OpenTimeout.ToString()));
                info.AppendLine(string.Format(" Binding.ReceiveTimeout: {0}", se.Binding.ReceiveTimeout.ToString()));
                info.AppendLine(string.Format(" Binding.Scheme:         {0}", se.Binding.Scheme.ToString()));
                info.AppendLine(string.Format(" Binding.SendTimeout:    {0}", se.Binding.SendTimeout.ToString()));
                info.AppendLine(string.Format(" Contract:               {0}", se.Contract.Name));
                info.AppendLine(string.Format(" IsSystemEndpoint:       {0}", se.IsSystemEndpoint));
                info.AppendLine(string.Format(" ListenUri:              {0}", se.ListenUri));
                info.AppendLine(string.Format(" ListenUriMode:          {0}", se.ListenUriMode));
                info.AppendLine(string.Format(" ToString():             {0}", se.ToString()));
                info.AppendLine();
            }

            return info.ToString();
        }
    }
}
