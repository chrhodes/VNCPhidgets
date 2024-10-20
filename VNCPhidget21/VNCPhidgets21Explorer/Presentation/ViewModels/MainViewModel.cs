using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Phidgets;

using Prism.Commands;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public MainViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            InitializeViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        const Int32 sbc11SerialNumber = 46049;

        const Int32 sbc21SerialNumber = 48301;
        const Int32 sbc22SerialNumber = 251831;
        const Int32 sbc23SerialNumber = 48284;

        private void InitializeViewModel()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewModelLow) startTicks = Log.VIEWMODEL_LOW("Enter", Common.LOG_CATEGORY);


            Button1Command = new DelegateCommand(Button1Execute);
            Button2Command = new DelegateCommand(Button2Execute);
            Button3Command = new DelegateCommand(Button3Execute);

            if (Common.VNCLogging.ViewModelLow) Log.VIEWMODEL_LOW("Exit", Common.LOG_CATEGORY, startTicks);
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

        private string _title = "VNCPhidgets21Explorer - Main";

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


        private bool _displayInputChangeEvents = false;

        public bool DisplayInputChangeEvents
        {
            get => _displayInputChangeEvents;
            set
            {
                if (_displayInputChangeEvents == value)
                    return;
                _displayInputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _displayOutputChangeEvents = false;

        public bool DisplayOutputChangeEvents
        {
            get => _displayOutputChangeEvents;
            set
            {
                if (_displayOutputChangeEvents == value)
                    return;
                _displayOutputChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private bool _sensorChangeEvents = false;

        public bool DisplaySensorChangeEvents
        {
            get => _sensorChangeEvents;
            set
            {
                if (_sensorChangeEvents == value)
                    return;
                _sensorChangeEvents = value;
                OnPropertyChanged();
            }
        }

        private int _repeats = 1;
        public int Repeats
        {
            get => _repeats;
            set
            {
                if (_repeats == value)
                    return;
                _repeats = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Event Handlers (none)


        #endregion

        #region Commands (none)


        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods

        private void OpenPhidgetManager()
        {
            Manager phidgetManager = new Manager();

            phidgetManager.Attach += PhidgetManager_Attach;
            phidgetManager.Detach += PhidgetManager_Detach;

            phidgetManager.open();

        }

        private async Task OpenSBCInterfaceKit()
        {
            InterfaceKit ifk11 = new InterfaceKit();

            InterfaceKit ifk21 = new InterfaceKit();
            InterfaceKit ifk22 = new InterfaceKit();
            InterfaceKit ifk23 = new InterfaceKit();

            try
            {
                await Task.Run(() =>
                {
                    Parallel.Invoke(
                         () => InterfaceKitParty(ifk11, sbc11SerialNumber, "192.168.150.11", 5001, 500, 5 * Repeats),
                         () => InterfaceKitParty(ifk21, sbc21SerialNumber, "192.168.150.21", 5001, 250, 10 * Repeats),
                         () => InterfaceKitParty(ifk22, sbc22SerialNumber, "192.168.150.22", 5001, 125, 20 * Repeats),
                         () => InterfaceKitParty(ifk23, sbc23SerialNumber, "192.168.150.23", 5001, 333, 8 * Repeats)
                     );
                });
                //InterfaceKitParty(ifk0, sbc21SerialNumber, "192.168.150.21", 5001, 250);
                //InterfaceKitParty(ifk1, sbc22SerialNumber, "192.168.150.22", 5001, 125);
                //InterfaceKitParty(ifk2, sbc23SerialNumber, "192.168.150.23", 5001, 333);
                //InterfaceKitParty(ifk1);
                //InterfaceKitParty(ifk2);

            }
            catch (PhidgetException pe)
            {
                switch (pe.Type)
                {
                    case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
                        //        ifk.SerialNumber));
                        break;

                    default:
                        //System.Diagnostics.Debug.WriteLine(
                        //    string.Format("{0}\nInterface Kit {0}",
                        //pe.ToString(),
                        //        ifk.SerialNumber));
                        break;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private async void InterfaceKitParty(InterfaceKit ifk, Int32 serialNumber, string hostName, Int32 port, Int32 sleep, Int32 loops)
        {
            try
            {
                Log.Debug($"InterfaceKitParty {hostName},{port} {serialNumber} sleep:{sleep} loops:{loops}", Common.LOG_CATEGORY);
                ifk.Attach += Ifk_Attach;
                ifk.Detach += Ifk_Detach;
                ifk.Error += Ifk_Error;
                ifk.InputChange += Ifk_InputChange;
                ifk.OutputChange += Ifk_OutputChange;
                ifk.SensorChange += Ifk_SensorChange;
                ifk.ServerConnect += Ifk_ServerConnect;
                ifk.ServerDisconnect += Ifk_ServerDisconnect;

                ifk.open(serialNumber, hostName, port);
                ifk.waitForAttachment();

                InterfaceKitDigitalOutputCollection ifkdoc = ifk.outputs;

                for (int i = 0; i < loops; i++)
                {
                    ifkdoc[0] = true;
                    Thread.Sleep(sleep);
                    ifkdoc[0] = false;
                    Thread.Sleep(sleep);
                }

                ifk.close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_ServerDisconnect(object sender, Phidgets.Events.ServerDisconnectEventArgs e)
        {
            try
            {
                var a = e;
                var b = e.GetType();
                Log.Trace("Ifk_ServerDisconnect", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_ServerConnect(object sender, Phidgets.Events.ServerConnectEventArgs e)
        {
            try
            {
                Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                //Log.Trace($"Ifk_ServerConnect {device.Address},{device.Port} S#:{device.SerialNumber}", Common.LOG_CATEGORY);
                Log.Trace($"Ifk_ServerConnect {device.Address},{device.Port}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            if (DisplaySensorChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_SensorChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_OutputChange(object sender, Phidgets.Events.OutputChangeEventArgs e)
        {
            if (DisplayOutputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_OutputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_InputChange(object sender, Phidgets.Events.InputChangeEventArgs e)
        {
            if (DisplayInputChangeEvents)
            {
                try
                {
                    InterfaceKit ifk = (InterfaceKit)sender;
                    var a = e;
                    var b = e.GetType();
                    Log.Trace($"Ifk_InputChange {ifk.Address},{ifk.SerialNumber} - Index:{e.Index} Value:{e.Value}", Common.LOG_CATEGORY);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Common.LOG_CATEGORY);
                }
            }
        }

        private void Ifk_Error(object sender, Phidgets.Events.ErrorEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Ifk_Error {ifk.Address},{ifk.Attached} - {e.Type} {e.Code} {e.Description}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                var a = e;
                var b = e.GetType();
                Log.Trace($"Ifk_Detach {ifk.Address},{ifk.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void Ifk_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            try
            {
                InterfaceKit ifk = (InterfaceKit)sender;
                //Phidget device = (Phidget)e.Device;
                //var b = e.GetType();
                Log.Trace($"Ifk_Attach {ifk.Address},{ifk.Port} S#:{ifk.SerialNumber}", Common.LOG_CATEGORY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, Common.LOG_CATEGORY);
            }
        }

        private void PhidgetManager_Detach(object sender, Phidgets.Events.DetachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
            Log.Trace("PhidgetManager_Detach", Common.LOG_CATEGORY);
        }

        private void PhidgetManager_Attach(object sender, Phidgets.Events.AttachEventArgs e)
        {
            var a = e;
            var b = e.GetType();
            Log.Trace("PhidgetManager_Attach", Common.LOG_CATEGORY);
        }

        private void Button1Execute()
        {
            Int64 startTicks = Log.Info("Enter", "WHOISTHIS");

            Message = "Button1 Clicked";

            //OpenPhidgetManager();
            OpenSBCInterfaceKit();
            //OpenPhidget();
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
            //for (int i = 0; i < 10; i++)
            //{
            //    digitalOutput0.DutyCycle = 1;
            //    Thread.Sleep(500);
            //    digitalOutput0.DutyCycle = 0;
            //    Thread.Sleep(500);
            //}
        }

        private void LightAction1B()
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    digitalOutput2.DutyCycle = 1;
            //    Thread.Sleep(100);
            //    digitalOutput2.DutyCycle = 0;
            //    Thread.Sleep(100);
            //}
        }



        private void Button2Execute()
        {
            Int64 startTicks = Log.Debug("Enter", Common.LOG_CATEGORY);

            Message = "Button2 Clicked";

            //for (int i = 0; i < 10; i++)
            //{
            //    digitalOutput0.DutyCycle = 1;
            //    Thread.Sleep(125);
            //    digitalOutput0.DutyCycle = 0;
            //    Thread.Sleep(125);
            //    digitalOutput2.DutyCycle = 1;
            //    Thread.Sleep(250);
            //    digitalOutput2.DutyCycle = 0;
            //    Thread.Sleep(250);
            //}
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
