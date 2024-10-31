using System;
using Phidget22;

namespace VNCPhidgets22Console
{
    internal class Program
    {
 

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Phidget22.Phidget phidget = new Phidget();
            DigitalOutput do0 = new DigitalOutput();
            DigitalOutput do1 = new DigitalOutput();

            phidget.Attach += Phidget_Attach;
            phidget.Detach += Phidget_Detach;
            phidget.PropertyChange += Phidget_PropertyChange;
            phidget.Error += Phidget_Error;


            do0.Attach += Do0_Attach;

            //phidget.DeviceSerialNumber = 624728;

            //phidget.Open();

            do0.DeviceSerialNumber = 124744;
            do0.Channel = 0;
            do0.IsHubPortDevice = false;

            //do0.HubPort = 0;
            do0.IsLocal = false;
            do0.IsRemote = true;

            //Net.EnableServerDiscovery(ServerType.DeviceRemote);

            Net.ServerAdded += Net_ServerAdded;
            Net.ServerRemoved += Net_ServerRemoved;

            Net.AddServer("PSBC41", "192.168.150.41", 5661, "", 0);
            //Net.AddServer("PSBC41", "192.168.150.41", 5001, "", 0);

            do0.Open(10000);
            

            do0.State = true;
        }

        private static void Net_ServerRemoved(Phidget22.Events.NetServerRemovedEventArgs e)
        {
            var server = e.Server;
            //throw new NotImplementedException();
        }

        private static void Net_ServerAdded(Phidget22.Events.NetServerAddedEventArgs e)
        {
            var server = e.Server;
            //throw new NotImplementedException();
        }

        private static void Do0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            var digitalOutput = (Phidget22.DigitalOutput)sender;
            var aea = e;

            //throw new NotImplementedException();
        }

        private static void Phidget_Error(object sender, Phidget22.Events.ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Phidget_PropertyChange(object sender, Phidget22.Events.PropertyChangeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Phidget_Detach(object sender, Phidget22.Events.DetachEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Phidget_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
