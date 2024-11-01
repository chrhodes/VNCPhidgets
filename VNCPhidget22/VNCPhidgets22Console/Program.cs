using System;
using System.ComponentModel;
using System.Data;
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

            ExploreNetManager();


            //OpenDigitalOutputs();
        }

        private static void ExploreNetManager()
        {
            //Net.ServerAdded += Net_ServerAdded;
            //Net.ServerRemoved += Net_ServerRemoved;

            // NOTE(crhodes)
            // This finds a private 172... network ID ???
            // Try on Laptop

            Console.WriteLine("EnableServerDiscovery - DeviceRemote");

            Net.EnableServerDiscovery(ServerType.DeviceRemote);

            // NOTE(crhodes)
            // This doesn't seem to find anything.
            // Try on Laptop

            Console.WriteLine("EnableServerDiscovery - SBC");

            Net.EnableServerDiscovery(ServerType.SBC);

            // NOTE(crhodes)
            // These seem to reliably work.  Have not tried adding if not connected.
            // No ServerAdded ServerRemoved events fired.

            //Net.AddServer("PSBC21", "192.168.150.21", 5661, "", 0);
            //Net.AddServer("PSBC22", "192.168.150.22", 5661, "", 0);
            //Net.AddServer("PSBC23", "192.168.150.23", 5661, "", 0);

            Phidget22.Manager manager = new Phidget22.Manager();

            manager.Attach += Manager_Attach;
            manager.Detach += Manager_Detach;

            Console.WriteLine("Opening Manager");

            manager.Open();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Sleeping 1 second ...");
                Thread.Sleep(1000);
            }
        }

        private static void ExplorePhidget()
        {
            //Phidget22.Phidget phidget = new Phidget();

            ////phidget.Attach += Phidget_Attach;
            //phidget.Detach += Phidget_Detach;
            ////phidget.PropertyChange += Phidget_PropertyChange;
            //phidget.Error += Phidget_Error;

            //try
            //{
            //    phidget.Channel = 0;
            //    phidget.DeviceSerialNumber = 124744;
            //}
            //catch (Exception ex)
            //{

            //}

            //phidget.IsLocal = false;
            //phidget.IsRemote = true;

            //phidget.Open();
        }

        private static void OpenDigitalOutputs()
        {
            DigitalOutput DO0 = new DigitalOutput();
            DigitalOutput DO2 = new DigitalOutput();

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

            DO0.Open();
            DO2.Open();
            Thread.Sleep(400);

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

        private static void Net_ServerAdded(Phidget22.Events.NetServerAddedEventArgs e)
        {
            var server = e.Server;
            Console.WriteLine($"Net_ServerAdded: {server} hostname:{server.Hostname,-20} address:{server.Address,-15} port:{server.Port,-4} type:{server.Type,-15} typeName:{server.TypeName}\n");
            //Console.WriteLine($"Net_ServerAdded: {server}\n" +
            //    $" serverName:{server.Name} hostname:{server.Hostname} address:{server.Address} port:{server.Port} type:{server.Type} typeName:{server.TypeName}\n");
        }

        private static void Net_ServerRemoved(Phidget22.Events.NetServerRemovedEventArgs e)
        {
            var server = e.Server;
            Console.WriteLine($"Net_ServerRemoved: {server.Name},{server.Address},{server.Port}");
        }

        private static void Manager_Attach(object sender, Phidget22.Events.ManagerAttachEventArgs e)
        {
            var maea = e;
            var channel = e.Channel;

            switch (channel.DeviceClass)
            {
                case DeviceClass.Dictionary:
                    //DisplayChannelInfo(channel);
                    break;

                case DeviceClass.Hub:
                    //DisplayChannelInfo(channel);
                    break;

                case DeviceClass.VINT:
                    //DisplayChannelInfo(channel);
                    break;

                case DeviceClass.InterfaceKit:
                    DisplayChannelInfo(channel);
                    break;

                default:
                    Console.WriteLine($">{channel.DeviceClass}<");
                    break;
            }
        }

        private static void DisplayChannelInfo(Phidget channel)
        {
            // NOTE(crhodes)
            // Display parent Info

            Console.WriteLine($"Manager_Attach:");

            var parent = channel.Parent;
            var grandParent = parent?.Parent;
            var greatGrandParent = grandParent?.Parent;

            if (greatGrandParent is not null)
            {
                Console.WriteLine($"{greatGrandParent}  deviceClass:{greatGrandParent.DeviceClass,-15}");
                Console.WriteLine($"  {grandParent}  deviceClass:{grandParent.DeviceClass,-15}");
                Console.WriteLine($"    {parent}  deviceClass:{parent.DeviceClass,-15}");
            }
            else if (grandParent is not null)
            {
                Console.WriteLine($"{grandParent}  deviceClass:{grandParent.DeviceClass,-15}");
                Console.WriteLine($"  {parent}  deviceClass:{parent.DeviceClass,-15}");
            }
            else if (parent is not null)
            {
                Console.WriteLine($"{parent}  deviceClass:{parent.DeviceClass,-15}");
            }

            //do
            //{
            //    Console.WriteLine($"parent:{parent}  deviceClass:{parent.DeviceClass,-15}");
            //    parent = parent.Parent;
            //} while (parent is not null);

            // Display what got attached

            Console.WriteLine($"    - {channel,-60} deviceClass:{channel.DeviceClass,-15} serverPeerName:{channel.ServerPeerName,-20} isChannel:{channel.IsChannel} isHubPortDevice:{channel.IsHubPortDevice,-5} isLocal:{channel.IsLocal} isRemote:{channel.IsRemote}");
            Console.WriteLine();

            //Console.WriteLine($"  attached:{channel.Attached}" +
            //    $", channelClass:{channel.ChannelClass} channelClassName:{channel.ChannelClassName} channelName:{channel.ChannelName} channelSubclass:{channel.ChannelSubclass}\n" +
            //    $", deviceClass:{channel.DeviceClass} deviceClassName:{channel.DeviceClassName} deviceID:{channel.DeviceID} deviceLabel:{channel.DeviceLabel} deviceName:{channel.DeviceName} deviceSerialNumber:{channel.DeviceSerialNumber} deviceSKU:{channel.DeviceSKU} deviceVersion:{channel.DeviceVersion} deviceVINTID:{channel.DeviceVINTID}\n" +
            //    $", serverHostName:{channel.ServerHostname} serverName:{channel.ServerName} serverPeerName:{channel.ServerPeerName} serverUniqueName:{channel.ServerUniqueName}\n" +
            //    $", isChannel:{channel.IsChannel} isHubPortDevice:{channel.IsHubPortDevice} isLocal:{channel.IsLocal} isRemote:{channel.IsRemote} isOpen:{channel.IsOpen}\n" +
            //    $", parent:{channel.Parent}");
        }

        private static void Manager_Detach(object sender, Phidget22.Events.ManagerDetachEventArgs e)
        {
            var mdea = e;
            Console.WriteLine($"Manager_Detach: {e.Channel}");
        }

        private static void Do0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            var digitalOutput = (Phidget22.DigitalOutput)sender;
            var aea = e;
            Console.WriteLine($"Do0_Attach: {e.ToString()}");
        }

        private static void Phidget_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            var aea = e;
            Console.WriteLine($"Phidget_Attach: {e.ToString()}");
        }

        private static void Phidget_Detach(object sender, Phidget22.Events.DetachEventArgs e)
        {
            var dea = e;
            Console.WriteLine($"Phidget_Detach: {e.ToString()}");
        }

        private static void Phidget_PropertyChange(object sender, Phidget22.Events.PropertyChangeEventArgs e)
        {
            var pcea = e;
            Console.WriteLine($"Phidget_PropertyChange: {e.ToString()}");
        }

        private static void Phidget_Error(object sender, Phidget22.Events.ErrorEventArgs e)
        {
            var eea = e;
            Console.WriteLine($"Phidget_Error: {e.ToString()}");
        }


    }
}
