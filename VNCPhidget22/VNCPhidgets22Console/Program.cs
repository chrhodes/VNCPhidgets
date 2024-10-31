using System;
using System.Threading;

using Phidget22;

namespace VNCPhidgets22Console
{
    internal class Program
    {

        // TODO(crhodes)
        // Add logging.

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Phidget22.Phidget phidget = new Phidget();
            DigitalOutput DO0 = new DigitalOutput();
            DigitalOutput DO2 = new DigitalOutput();

            phidget.Attach += Phidget_Attach;
            phidget.Detach += Phidget_Detach;
            phidget.PropertyChange += Phidget_PropertyChange;
            phidget.Error += Phidget_Error;

            DO0.Attach += Do0_Attach;

            //phidget.DeviceSerialNumber = 624728;

            //phidget.Open();

            DO0.DeviceSerialNumber = 124744;
            DO0.Channel = 0;
            DO0.IsHubPortDevice = false;

            //do0.HubPort = 0;
            DO0.IsLocal = false;
            DO0.IsRemote = true;

            DO2.DeviceSerialNumber = 124744;
            DO2.Channel = 2;
            DO2.IsHubPortDevice = false;

            //do0.HubPort = 0;
            DO2.IsLocal = false;
            DO2.IsRemote = true;

            Net.ServerAdded += Net_ServerAdded;
            Net.ServerRemoved += Net_ServerRemoved;

            // NOTE(crhodes)
            // This finds a private 172... network ID ???
            // Try on Laptop

            //Net.EnableServerDiscovery(ServerType.DeviceRemote);

            // NOTE(crhodes)
            // This doesn't seem to find anything.
            // Try on Laptop

            //Net.EnableServerDiscovery(ServerType.SBC);

            // NOTE(crhodes)
            // These seem to reliably work.  Have not tried adding if not connected.
            // No ServerAdded ServerRemoved events fired.

            //Net.AddServer("PSBC21", "192.168.150.21", 5661, "", 0);
            //Net.AddServer("PSBC22", "192.168.150.22", 5661, "", 0);
            //Net.AddServer("PSBC23", "192.168.150.23", 5661, "", 0);

            //Net.AddServer("PSBC41", "192.168.150.41", 5661, "", 0);

            DO0.Open();
            DO2.Open();
            Thread.Sleep(200);

            var isAttached0 = DO0.Attached;
            var isAttached1 = DO2.Attached;

            var doChannelCount = DO0.GetDeviceChannelCount(ChannelClass.DigitalOutput);
            var diChannelCount = DO0.GetDeviceChannelCount(ChannelClass.DigitalInput);
            var viChannelCount = DO0.GetDeviceChannelCount(ChannelClass.VoltageInput);
            var voChannelCount = DO0.GetDeviceChannelCount(ChannelClass.VoltageOutput);
            var vriChannelCount = DO0.GetDeviceChannelCount(ChannelClass.VoltageRatioInput);

            var parentPhidget = DO0.Parent;

            var parentdoChannelCount = parentPhidget.GetDeviceChannelCount(ChannelClass.DigitalOutput);
            var parentdiChannelCount = parentPhidget.GetDeviceChannelCount(ChannelClass.DigitalInput);
            var parentviChannelCount = parentPhidget.GetDeviceChannelCount(ChannelClass.VoltageInput);
            var parentvoChannelCount = parentPhidget.GetDeviceChannelCount(ChannelClass.VoltageOutput);
            var parentvriChannelCount = parentPhidget.GetDeviceChannelCount(ChannelClass.VoltageRatioInput);

            for (int i = 0; i < 10; i++)
            {
                DO0.State = true;
                DO2.State = false;
                Thread.Sleep(500);
                DO0.State = false;
                DO2.State = true;
                Thread.Sleep(500);
            }
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
