using System;
using System.Windows.Input;

using ph22 = Phidget22;
using ph22E = Phidget22.Events;

using Prism.Commands;

using VNC;
using VNC.Core.Mvvm;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;

namespace VNCPhidgets22Explorer.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public MainViewModel()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        ph22.DigitalOutput digitalOutput0;
        ph22.DigitalOutput digitalOutput2;

        const Int32 sbc11SerialNumber = 46049;
        const Int32 sbc21SerialNumber = 48284;
        const Int32 sbc22SerialNumber = 48301;
        const Int32 sbc23SerialNumber = 251831;

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            Button1Command = new DelegateCommand(Button1Execute);
            Button2Command = new DelegateCommand(Button2Execute);
            Button3Command = new DelegateCommand(Button3Execute);

            //try
            //{
            //    digitalOutput0 = new ph22.DigitalOutput();
            //    digitalOutput0.Channel = 0;
            //    digitalOutput0.DeviceSerialNumber = sbc22SerialNumber;
            //    digitalOutput0.Open(5000);

            //    digitalOutput2 = new ph22.DigitalOutput();
            //    digitalOutput2.Channel = 2;
            //    digitalOutput0.DeviceSerialNumber = sbc22SerialNumber;
            //    digitalOutput2.Open(5000);
            //}
            //catch (Exception ex)
            //{

            //}

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties


        public ICommand Button1Command { get; private set; }
        public ICommand Button2Command { get; private set; }
        public ICommand Button3Command { get; private set; }

        private string _title = "VNCPhidgets22Explorer - Main - " + Common.ProductVersion;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _message = "Initial Message";

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        private int _numerator = 10;
        public int Numerator
        {
            get => _numerator;
            set
            {
                if (_numerator == value)
                    return;
                _numerator = value;
                OnPropertyChanged();
            }
        }

        private int _denominator = 2;
        public int Denominator
        {
            get => _denominator;
            set
            {
                if (_denominator == value)
                    return;
                _denominator = value;
                OnPropertyChanged();
            }
        }

        private string _answer = "???";

        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer == value)
                    return;
                _answer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private void OpenPhidget()
        {
            ph22.Net.ServerAdded += Net_ServerAdded;
            ph22.Net.ServerRemoved += Net_ServerRemoved;

            //ph22.Net.EnableServerDiscovery(ph22.ServerType.SBC);
            ph22.Net.AddServer("phsbc11", "192.168.150.11", 5001, "", 0);
            ph22.Net.AddServer("phsbc21", "192.168.150.21", 5001, "", 0);
            ph22.Net.AddServer("phsbc22", "192.168.150.22", 5001, "", 0);
            ph22.Net.AddServer("phsbc23", "192.168.150.23", 5001, "", 0);

            // NOTE(crhodes)
            // Passing null throws exception

            //ph22.Net.AddServer("phsbc11", "192.168.150.11", 5001, null, 0);
            //ph22.Net.AddServer("phsbc21", "192.168.150.21", 5001, null, 0);
            //ph22.Net.AddServer("phsbc22", "192.168.150.22", 5001, null, 0);
            //ph22.Net.AddServer("phsbc23", "192.168.150.23", 5001, null, 0);

            ph22.Phidget phidget = new ph22.Phidget();

            phidget.Attach += Phidget_Attach;
            phidget.Detach += Phidget_Detach;

            ph22.DigitalOutput digitalOutput;

            digitalOutput = new ph22.DigitalOutput();

            digitalOutput.Attach += DigitalOutput_Attach;
            digitalOutput.Detach += DigitalOutput_Detach;

            digitalOutput.Channel = 0;
            digitalOutput.IsRemote = true;
            digitalOutput.DeviceSerialNumber = sbc22SerialNumber;
            digitalOutput.Open(5000);

            digitalOutput.DutyCycle = 1;
            digitalOutput.DutyCycle = 0;

            //phidget.IsHubPortDevice = true;

            //phidget.Channel = 0;
            //phidget.DeviceSerialNumber = sbc21SerialNumber;

            //phidget.Open();
        }

        private void DigitalOutput_Detach(object sender, ph22E.DetachEventArgs e)
        {
            var a = e;
            var b = e.GetType();

        }

        private void DigitalOutput_Attach(object sender, ph22E.AttachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void Net_ServerRemoved(ph22E.NetServerRemovedEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void Net_ServerAdded(ph22E.NetServerAddedEventArgs e)
        {
            var a = e;
            var server = e.Server;
            var b = e.GetType();
        }

        private void Phidget_Detach(object sender, ph22E.DetachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void Phidget_Attach(object sender, ph22E.AttachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void OpenPhidgetManager()
        {
            ph22.Manager phidgetManager = new ph22.Manager();

            phidgetManager.Attach += PhidgetManager_Attach;
            phidgetManager.Detach += PhidgetManager_Detach;

            phidgetManager.Open();

        }

        private void PhidgetManager_Detach(object sender, ph22E.ManagerDetachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void PhidgetManager_Attach(object sender, ph22E.ManagerAttachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
        }

        private void Button1Execute()
        {
            Int64 startTicks = Log.Info("Enter", "WHOISTHIS");

            Message = "Button1 Clicked";

            //OpenPhidgetManager();
            OpenPhidget();
            //LightAction1();

            //ph22.DigitalOutput digitalOutput = new ph22.DigitalOutput();
            //digitalOutput.Open(5000);
            //digitalOutput.DutyCycle = 0;
            //Console.ReadLine();
            //digitalOutput.Close();
            //digitalOutput.Dispose();

            Log.Info("End", "WHOISTHIS", startTicks);
        }

        private void LightAction1()
        {
            //for (int i = 0; i < 10; i++)
            //{
                Parallel.Invoke(() => LightAction1A(), () => LightAction1B());
                //await LightAction1A();
                //await LightAction1B();
            //}
        }

        private void LightAction1A()
        {
            for (int i = 0; i < 10; i++)
            {
                digitalOutput0.DutyCycle = 1;
                Thread.Sleep(500);
                digitalOutput0.DutyCycle = 0;
                Thread.Sleep(500);
            }
        }

        private void LightAction1B()
        {
            for (int i = 0; i < 50; i++)
            {
                digitalOutput2.DutyCycle = 1;
                Thread.Sleep(100);
                digitalOutput2.DutyCycle = 0;
                Thread.Sleep(100);
            }
        }



        private void Button2Execute()
        {
            Int64 startTicks = Log.Debug("Enter", Common.LOG_CATEGORY);

            Message = "Button2 Clicked";

            for (int i = 0; i < 10; i++)
            {
                digitalOutput0.DutyCycle = 1;
                Thread.Sleep(125);
                digitalOutput0.DutyCycle = 0;
                Thread.Sleep(125);
                digitalOutput2.DutyCycle = 1;
                Thread.Sleep(250);
                digitalOutput2.DutyCycle = 0;
                Thread.Sleep(250);
            }
            //ph22.DigitalOutput digitalOutput = new ph22.DigitalOutput();
            //digitalOutput.Open(5000);
            //digitalOutput.DutyCycle = 1;
            //Console.ReadLine();
            //digitalOutput.Close();
            //digitalOutput.Dispose();

            Log.Debug("End", Common.LOG_CATEGORY, startTicks);
        }

        private void Button3Execute()
        {
            Int64 startTicks = Log.Trace("Enter", Common.LOG_CATEGORY);

            Message = "Button3 Clicked";

            try
            {
                method1();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
                Log.Error(ex, "BOOM");
            }

            Log.Trace("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method1()
        {
            Int64 startTicks = Log.Trace1("Enter", Common.LOG_CATEGORY);

            method2();

            Log.Trace1("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method2()
        {
            Int64 startTicks = Log.Trace2("Enter", Common.LOG_CATEGORY);

            method3();

            Log.Trace2("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method3()
        {
            Int64 startTicks = Log.Trace3("Enter", Common.LOG_CATEGORY);

            method4();

            Log.Trace3("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method4()
        {
            Int64 startTicks = Log.Trace4("Enter", Common.LOG_CATEGORY);

            method5();

            Log.Trace4("End", Common.LOG_CATEGORY, startTicks);
        }

        private void method5()
        {
            Int64 startTicks = Log.Trace5("Enter", Common.LOG_CATEGORY);


            int answer = Numerator / Denominator;

            Answer = answer.ToString();

            Log.Trace5("End", Common.LOG_CATEGORY, startTicks);
        }


        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
