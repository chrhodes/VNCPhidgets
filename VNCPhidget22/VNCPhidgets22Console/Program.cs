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

            //phidget.Open();
            do0.Open();

            do0.State = true;
        }

        private static void Do0_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            
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
