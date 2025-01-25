using System;
using Phidget22;

namespace ConsoleApplication
{
	class Program
	{

		//Declare any event handlers here. These will be called every time the associated event occurs.

		private static void VoltageRatioInput0_VoltageRatioChange(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
		{
			Console.WriteLine("VoltageRatio: " + e.VoltageRatio);
		}

		private static void VoltageRatioInput0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
		{
			Console.WriteLine("Attach!");
		}

		private static void VoltageRatioInput0_Detach(object sender, Phidget22.Events.DetachEventArgs e)
		{
			Console.WriteLine("Detach!");
		}

		private static void VoltageRatioInput0_Error(object sender, Phidget22.Events.ErrorEventArgs e)
		{
			Console.WriteLine("Code: " + e.Code);
			Console.WriteLine("Description: " + e.Description);
			Console.WriteLine("----------");
		}

		private static void DigitalInput0_StateChange(object sender, Phidget22.Events.DigitalInputStateChangeEventArgs e)
		{
			Console.WriteLine("State: " + e.State);
		}

		private static void DigitalInput0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
		{
			Console.WriteLine("Attach!");
		}

		private static void DigitalInput0_Detach(object sender, Phidget22.Events.DetachEventArgs e)
		{
			Console.WriteLine("Detach!");
		}

		private static void DigitalInput0_Error(object sender, Phidget22.Events.ErrorEventArgs e)
		{
			Console.WriteLine("Code: " + e.Code);
			Console.WriteLine("Description: " + e.Description);
			Console.WriteLine("----------");
		}

		private static void DigitalOutput0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
		{
			Console.WriteLine("Attach!");
		}

		private static void DigitalOutput0_Detach(object sender, Phidget22.Events.DetachEventArgs e)
		{
			Console.WriteLine("Detach!");
		}

		private static void DigitalOutput0_Error(object sender, Phidget22.Events.ErrorEventArgs e)
		{
			Console.WriteLine("Code: " + e.Code);
			Console.WriteLine("Description: " + e.Description);
			Console.WriteLine("----------");
		}

		static void Main(string[] args)
		{
			//Enable server discovery to allow your program to find other Phidgets on the local network.
			Net.EnableServerDiscovery(Phidget22.ServerType.DeviceRemote);

			//Create your Phidget channels
			VoltageRatioInput voltageRatioInput0 = new VoltageRatioInput();
			DigitalInput digitalInput0 = new DigitalInput();
			DigitalOutput digitalOutput0 = new DigitalOutput();

			//Set addressing parameters to specify which channel to open (if any)
			voltageRatioInput0.IsRemote = true;
			digitalInput0.IsRemote = true;
			digitalOutput0.IsRemote = true;

			//Assign any event handlers you need before calling open so that no events are missed.
			voltageRatioInput0.VoltageRatioChange += VoltageRatioInput0_VoltageRatioChange;
			voltageRatioInput0.Attach += VoltageRatioInput0_Attach;
			voltageRatioInput0.Detach += VoltageRatioInput0_Detach;
			voltageRatioInput0.Error += VoltageRatioInput0_Error;
			digitalInput0.StateChange += DigitalInput0_StateChange;
			digitalInput0.Attach += DigitalInput0_Attach;
			digitalInput0.Detach += DigitalInput0_Detach;
			digitalInput0.Error += DigitalInput0_Error;
			digitalOutput0.Attach += DigitalOutput0_Attach;
			digitalOutput0.Detach += DigitalOutput0_Detach;
			digitalOutput0.Error += DigitalOutput0_Error;

			try
			{
				//Open your Phidgets and wait for attachment
				voltageRatioInput0.Open(20000);
				digitalInput0.Open(20000);
				digitalOutput0.Open(20000);

				//Do stuff with your Phidgets here or in your event handlers.
				digitalOutput0.DutyCycle = 1;

				//Wait until Enter has been pressed before exiting
				Console.ReadLine();

				//Close your Phidgets once the program is done.
				voltageRatioInput0.Close();
				digitalInput0.Close();
				digitalOutput0.Close();
			}
			catch (PhidgetException ex)
			{
				//We will catch Phidget Exceptions here, and print the error informaiton.
				Console.WriteLine(ex.ToString());
				Console.WriteLine("");
				Console.WriteLine("PhidgetException " + ex.ErrorCode + " (" + ex.Description + "): " + ex.Detail);
			}
		}
	}
}
