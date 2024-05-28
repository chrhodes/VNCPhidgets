using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

using DevExpress.Xpf.Core;
using dxe = DevExpress.Xpf.Editors;

//Needed for the event handling classes 
using PE = Phidgets.Events;
using System.Collections.ObjectModel;

namespace FxShow.User_Interface.Windows
{
    /// <summary>
    /// Interaction logic for wndDX_MainWindow.xaml
    /// </summary>
    public partial class wndDX_MainWindow : DXWindow
    {
                
        #region Constants

        private static int CLASS_BASE_ERRORNUMBER = ErrorNumbers.FXSHOW;
        private const string LOG_APPNAME = Common.APP_NAME;

        #endregion

        #region Initialization

        public wndDX_MainWindow()
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("wndDX_MainWindow()");
#endif
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("OnWindowLoaded");
#endif
            //cbe_ShowNames.Clear();
            //cbe_ShowNames.Items.BeginUpdate();
            //cbe_ShowNames.Items.Add("Show1.xml");
            //cbe_ShowNames.Items.Add("Show2.xml");
            //cbe_ShowNames.Items.Add("Show3.xml");
            //cbe_ShowNames.Items.EndUpdate();

        }

        #endregion

        #region Fields and Properties

        private const string cXMLRootElement = "FxShow";
        private const string cListElements = "Hosts";
        //private const string cElement = "Environment";

        private string _RawXML = "";

        #region AdvancedServos

        // NOTE(crhodes)
        // Not sure what/where MotorControlers are.
        // For now just commenting out so can get this to compile

        //public PhidgetHelper.MotorControllers.AdvancedServo _AS;
        //public PhidgetHelper.MotorControllers.AdvancedServo AS
        //{
        //    get { return _AS; }
        //    set { _AS = value; }
        //}

        //private Collection<string> _AdvancedServosA;
        //public Collection<string> AdvancedServosA
        //{
        //    get
        //    {
        //        if (null == _AdvancedServosA)
        //        {
        //            _AdvancedServosA = new Collection<string>();
        //        }
        //        return _AdvancedServosA;
        //    }
        //    set
        //    {
        //        _AdvancedServosA = value;
        //    }
        //}

        //private Dictionary<string, PhidgetHelper.MotorControllers.AdvancedServo> _AdvancedServosD;
        //public Dictionary<string, PhidgetHelper.MotorControllers.AdvancedServo> AdvancedServosD
        //{
        //    get
        //    {
        //        if (_AdvancedServosD == null)
        //        {
        //            _AdvancedServosD = new Dictionary<string, PhidgetHelper.MotorControllers.AdvancedServo>();
        //        }
        //        return _AdvancedServosD;
        //    }
        //    set
        //    {
        //        _AdvancedServosD = value;
        //    }
        //}

        //private IEnumerable<PhidgetHelper.MotorControllers.AdvancedServo> _AdvancedServos;
        //public IEnumerable<PhidgetHelper.MotorControllers.AdvancedServo> AdvancedServos
        //{
        //    get
        //    {
        //        if (null == _AdvancedServos)
        //        {
        //            try
        //            {
        //                _AdvancedServos =
        //                    from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Descendants("Host").Elements("AdvancedServo")
        //                    select new PhidgetHelper.MotorControllers.AdvancedServo(
        //                        item.Parent.Attribute("IPAddress").Value,
        //                        int.Parse(item.Parent.Attribute("Port").Value),
        //                        int.Parse(item.Attribute("SerialNumber").Value),
        //                        bool.Parse(item.Attribute("Enable").Value)
        //                        );
        //            }
        //            catch (Exception ex)
        //            {
        //                Trace.WriteLine("Unable to Find AdvancedServos in XML");
        //                // TODO(crhodes): Check for various things, null _RawXML, etc.
        //            }
        //        }

        //        return _AdvancedServos;
        //    }

        //    set
        //    {
        //        _AdvancedServos = value;
        //    }
        //}

        #endregion

        #region Hosts

        private PhidgetHelper.SBCs.Host _HOST;
        public PhidgetHelper.SBCs.Host HOST
        {
            get { return _HOST; }
            set { _HOST = value; }
        }

        private Dictionary<string, PhidgetHelper.SBCs.Host> _HostD;
        public Dictionary<string, PhidgetHelper.SBCs.Host> HostD
        {
            get
            {
                if (_HostD == null)
                {
                    _HostD = new Dictionary<string, PhidgetHelper.SBCs.Host>();
                }
                return _HostD;
            }
            set
            {
                _HostD = value;
            }
        }


        private IEnumerable<PhidgetHelper.SBCs.Host> _Hosts;
        public IEnumerable<PhidgetHelper.SBCs.Host> Hosts
        {
            get
            {
                if (null == _Hosts)
                {
                    _Hosts =
                        from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Elements("Host")
                        select new PhidgetHelper.SBCs.Host(
                            item.Attribute("Name").Value,
                            item.Attribute("IPAddress").Value,
                            item.Attribute("Port").Value,
                            bool.Parse(item.Attribute("Enable").Value)
                            );
                }

                return _Hosts;
            }

            set
            {
                _Hosts = value;
            }
        }

        #endregion

        #region InterfaceKits

        private PhidgetHelper.InterfaceKits.InterfaceKit _IK;
        public PhidgetHelper.InterfaceKits.InterfaceKit IK
        {
            get { return _IK; }
            set { _IK = value; }
        }

        private Dictionary<string, PhidgetHelper.InterfaceKits.InterfaceKit> _InterfaceKitsD;
        public Dictionary<string, PhidgetHelper.InterfaceKits.InterfaceKit> InterfaceKitsD
        {
            get
            {
                if (null == _InterfaceKitsD)
                {
                    _InterfaceKitsD = new Dictionary<string, PhidgetHelper.InterfaceKits.InterfaceKit>();
                }
                return _InterfaceKitsD;
            }
            set
            {
                _InterfaceKitsD = value;
            }
        }

        private Collection<string> _InterfaceKits;
        public Collection<string> InterfaceKits
        {
            get
            {
                if (null == _InterfaceKits)
                {
                    _InterfaceKits = new Collection<string>();
                }
                return _InterfaceKits;
            }
            set
            {
                _InterfaceKits = value;
            }
        }
        #endregion

        #endregion
        
        #region Event Handlers

        private void btn_LoadShow_Click(object sender, RoutedEventArgs e)
        {
            // TODO(crhodes): Parse XML file and build Dictionary of Hosts and Interface Kits
            string filePath = ((dxe.ComboBoxEditItem)cbe_ShowLocations.SelectedItem).Content.ToString();

            string filePath2 = cbe_ShowLocations.SelectedItem.ToString();
            //string fileName = cbe_ShowNames.SelectedItem.ToString();

            //LoadShowFromFile(string.Format("{0}\\{1}", filePath, fileName));
            LoadShowFromFile(filePath);
        }

        private void cbe_ShowLocations_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // TODO(crhodes): Populate cbe_ShowNames with files matching pattern
            //cbe_ShowNames.Clear();
            //cbe_ShowNames.Items.BeginUpdate();
            //cbe_ShowNames.Items.Add("Show1.xml");
            //cbe_ShowNames.Items.Add("Show2.xml");
            //cbe_ShowNames.Items.Add("Show3.xml");
            //cbe_ShowNames.Items.EndUpdate();
        }

        private void cbe_ShowName_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // TODO(crhodes): Not sure we need this
        }

        private void cbeAdvancedServos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            string serialNumber = ((dxe.ComboBoxEdit)sender).SelectedItem.ToString();

            //AS = AdvancedServosD[serialNumber];
            //teNbrServos.EditValue = AS.servos.Count;
            //lgAdvancedServos.DataContext = AS;
            cbeAdvancedServos.SelectedIndex = 0;
        }

        private void cbeInterfaceKits_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - cbeInterfaceKits_SelectedIndexChanged",
                System.Threading.Thread.CurrentThread.ManagedThreadId));

            string serialNumber = ((dxe.ComboBoxEdit)sender).SelectedItem.ToString();

            IK = InterfaceKitsD[serialNumber];
            lgInterfaceKits.DataContext = IK;
            lgAnalogInputs_A0.DataContext = IK.Sensors[0];
            lgAnalogInputs_A1.DataContext = IK.Sensors[1];
            lgAnalogInputs_A2.DataContext = IK.Sensors[2];
            lgAnalogInputs_A3.DataContext = IK.Sensors[3];
            lgAnalogInputs_A4.DataContext = IK.Sensors[4];
            lgAnalogInputs_A5.DataContext = IK.Sensors[5];
            lgAnalogInputs_A6.DataContext = IK.Sensors[6];
            lgAnalogInputs_A7.DataContext = IK.Sensors[7];
        }

        private void cbeServos_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //InitializeServos(AS);
        }

        private void cbeServoTypes_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ceD0A_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());

            foreach (var item in InterfaceKits)
            {
                string ikName = item.ToString();
                InterfaceKitsD[ikName].outputs[index] = true;
            }
        }

        private void ceD0A_UnChecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());

            foreach (var item in InterfaceKits)
            {
                string ikName = item.ToString();
                InterfaceKitsD[ikName].outputs[index] = false;                
            }
        }

        private void ceDI_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ceDI_UnChecked(object sender, RoutedEventArgs e)
        {

        }

        private void ceDO_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            InterfaceKitsD[cbeInterfaceKits.Text].outputs[index] = true;
        }

        private void ceDO_UnChecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            InterfaceKitsD[cbeInterfaceKits.Text].outputs[index] = false;
        }

        private void ceEnableCurrentEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.CurrentChange += advancedServo_CurrentChange;            	
            }
            else
            {
                //AS.CurrentChange -= advancedServo_CurrentChange
            }
        }

        private void ceEnablePositionEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.PositionChange += advancedServo_PositionChange;
            }
            else
            {
                //AS.PositionChange -= advancedServo_PositionChange;
            }
        }

        private void ceEnableVelocityEvents_Changed(object sender, RoutedEventArgs e)
        {
            if (bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString()))
            {
                //AS.VelocityChange += advancedServo_VelocityChange;
            }
            else
            {
                //AS.VelocityChange -= advancedServo_VelocityChange;
            }
        }

        private void ceEngaged_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Engaging: " + index.ToString());

            //AS.servos[index].Engaged = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());

            //foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //{
            //    int index = int.Parse(item.Content.ToString());
            //    Trace.WriteLine("Engaging:" + index.ToString());
            //    AS.servos[index].Engaged = (bool)ceEngaged.IsChecked;
            //}
        }

        private void ceEngaged_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("DisEngaging: " + index.ToString());

            //AS.servos[index].Engaged = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());

        }

        private void ceSpeedRamping_Checked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Enable SpeedRamping: " + index.ToString());

            //AS.servos[index].SpeedRamping = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());
        }

        private void ceSpeedRamping_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((dxe.CheckEdit)sender).Tag.ToString());
            Trace.WriteLine("Disable SpeedRamping: " + index.ToString());

            //AS.servos[index].SpeedRamping = bool.Parse(((dxe.CheckEdit)sender).IsChecked.ToString());
        }

        private void DXWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
#if TRACE
            System.Diagnostics.Debug.WriteLine("DXWindow_Closing_1");
#endif
            UnInitializeAdvancedServos();

            // Close all Phidgets we might have opened

            foreach (var item in InterfaceKitsD)
            {
                ClosePhidget(item.Value);
            }
        }

        private void SpinEdit_DataRate_Spin(object sender, dxe.SpinEventArgs e)
        {

            int currentValue = (int)((dxe.SpinEdit)sender).Value;

            Trace.WriteLine(string.Format("SpinUp:{0}-Value:{1}", e.IsSpinUp.ToString(), currentValue));

            if (e.IsSpinUp)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue * 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue + 8;
                }
            }
            else
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue / 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue - 8;
                }
            }
        }

        private void SpinEdit_Sensitivity_Spin(object sender, dxe.SpinEventArgs e)
        {
            int currentValue = (int)((dxe.SpinEdit)sender).Value;

            Trace.WriteLine(string.Format("SpinUp:{0}-Value:{1}", e.IsSpinUp.ToString(), currentValue));

            if (e.IsSpinUp)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue * 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue + 10;
                }
            }
            else
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    ((dxe.SpinEdit)sender).Value = currentValue / 2;
                }
                else
                {
                    ((dxe.SpinEdit)sender).Value = currentValue - 10;
                }
            }
        }

        private void tbeSetAcceleration_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Setting Acceleration:" + index.ToString());
            //        double acceleration = double.Parse(e.NewValue.ToString());
            //        AS.servos[index].Acceleration = acceleration;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }

        private void tbeSetVelocity_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Setting Velocity:" + index.ToString());
            //        double velocity = double.Parse(e.NewValue.ToString());
            //        AS.servos[index].VelocityLimit = velocity;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }

        private void tbeTargetPosition_EditValueChanged(object sender, dxe.EditValueChangedEventArgs e)
        {
            //// TODO(crhodes): Decide if want to disenage briefly then reengage.
            //try
            //{
            //    foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            //    {
            //        int index = int.Parse(item.Content.ToString());
            //        Trace.WriteLine("Positioning:" + index.ToString());
            //        double position = double.Parse(e.NewValue.ToString());

            //        switch (index)
            //        {
            //            case 0:
            //                position += (double)sePositionAdjustmentS0.Value;
            //                break;

            //            case 1:
            //                position += (double)sePositionAdjustmentS1.Value;
            //                break;

            //            case 2:
            //                position += (double)sePositionAdjustmentS2.Value;
            //                break;

            //            case 3:
            //                position += (double)sePositionAdjustmentS3.Value;
            //                break;

            //            case 4:
            //                position += (double)sePositionAdjustmentS4.Value;
            //                break;

            //            case 5:
            //                position += (double)sePositionAdjustmentS5.Value;
            //                break;

            //            case 6:
            //                position += (double)sePositionAdjustmentS6.Value;
            //                break;

            //            case 7:
            //                position += (double)sePositionAdjustmentS7.Value;
            //                break;
            //        }

            //        AS.servos[index].Position = position;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(ex.ToString());
            //}
        }

        #endregion

        #region Phidget Event Handlers

        #region AdvancedServo

        private void advancedServo_Attach(object sender, PE.AttachEventArgs e)
        {
            //Trace.WriteLine(string.Format("AdvancedServo {0} attached!",
            //                    e.Device.SerialNumber.ToString()));

            //Phidgets.AdvancedServo aSrv = (Phidgets.AdvancedServo)e.Device;

            //AdvancedServosA.Add(aSrv.SerialNumber.ToString());

            //// Set the default servo type to the one Phidgets sells.
            //foreach (Phidgets.AdvancedServoServo motor in aSrv.servos)
            //{
            //    motor.Engaged = false;
            //    motor.Type = Phidgets.ServoServo.ServoType.HITEC_HS322HD;
            //    motor.Acceleration = motor.AccelerationMin;
            //    //motor.VelocityLimit = motor.VelocityMax;
            //    motor.VelocityLimit = motor.VelocityMin;
            //    motor.SpeedRamping = true;
            //    motor.Position = (motor.PositionMax - motor.PositionMin) / 2;
            //}

            //// may want to add all the other types from the ServoServo.ServoType enumeration

            //foreach (string servoType in System.Enum.GetNames(typeof(Phidgets.ServoServo.ServoType)))
            //{
            //    if (servoType.Equals(Phidgets.ServoServo.ServoType.USER_DEFINED.ToString()))
            //    {
            //    	break;
            //    }
            //    // TODO(crhodes):
            //    //cbeServoTypes.Items.Add(servoType);
            //}

            ////cbeServos.EditValue = 0;
            ////cbeServos.SelectedIndex = 0;
        }

        private void advancedServo_CurrentChange(object sender, PE.CurrentChangeEventArgs e)
        {
            int index = e.Index;
            string current = e.Current.ToString("F3");

            //Trace.WriteLine("CurrentChange: " + index);

            this.Dispatcher.Invoke(new Action(delegate()
            {
                try
                {
                    switch (index)
                    {
                        case 0:
                            teCurrentS0.Text = current;
                            break;

                        case 1:
                            teCurrentS1.Text = current;
                            break;

                        case 2:
                            teCurrentS2.Text = current;
                            break;

                        case 3:
                            teCurrentS3.Text = current;
                            break;

                        case 4:
                            teCurrentS4.Text = current;
                            break;

                        case 5:
                            teCurrentS5.Text = current;
                            break;

                        case 6:
                            teCurrentS6.Text = current;
                            break;

                        case 7:
                            teCurrentS7.Text = current;
                            break;
                    }                     
                }
                catch (Exception ex)
                {
                    //Trace.WriteLine(ex.ToString());
                }
            }
            ));
        }

        private void advancedServo_Detach(object sender, PE.DetachEventArgs e)
        {
            Trace.WriteLine(string.Format("AdvancedServo {0} detached!",
                                e.Device.SerialNumber.ToString()));
        }

        private void advancedServo_Error(object sender, PE.ErrorEventArgs e)
        {
            Trace.WriteLine(string.Format("advancedServo_Error(): Code:{0}\nDescription:{1}\nType:{2}",
                e.Code, e.Description, e.Type));
        }

        private void advancedServo_PositionChange(object sender, Phidgets.Events.PositionChangeEventArgs e)
        {
            //int index = e.Index;
            //bool isStopped = AS.servos[index].Stopped;
            //string position = int.Parse(e.Position.Round().ToString()).ToString();

            ////Trace.WriteLine("PositionChange: " + index);

            //this.Dispatcher.Invoke(new Action(delegate()
            //{
            //    try
            //    {
            //        switch (index)
            //        {
            //            case 0:
            //                tePositionS0.Text = position;
            //                ceStoppedS0.IsChecked = isStopped;
            //                break;

            //            case 1:
            //                tePositionS1.Text = position;
            //                ceStoppedS1.IsChecked = isStopped;
            //                break;

            //            case 2:
            //                tePositionS2.Text = position;
            //                ceStoppedS2.IsChecked = isStopped;
            //                break;

            //            case 3:
            //                tePositionS3.Text = position;
            //                ceStoppedS3.IsChecked = isStopped;
            //                break;

            //            case 4:
            //                tePositionS4.Text = position;
            //                ceStoppedS4.IsChecked = isStopped;
            //                break;

            //            case 5:
            //                tePositionS5.Text = position;
            //                ceStoppedS5.IsChecked = isStopped;
            //                break;

            //            case 6:
            //                tePositionS6.Text = position;
            //                ceStoppedS6.IsChecked = isStopped;
            //                break;

            //            case 7:
            //                tePositionS7.Text = position;
            //                ceStoppedS7.IsChecked = isStopped;
            //                break;
            //        }   
            //    }
            //    catch (Exception ex)
            //    {
            //        //Trace.WriteLine(ex.ToString());
            //    }
            //}
            //));
        }

        private void advancedServo_VelocityChange(object sender, PE.VelocityChangeEventArgs e)
        {
            //int index = e.Index;
            //string velocity = int.Parse(e.Velocity.Round().ToString()).ToString();

            //Trace.WriteLine("VelocityChange: " + index);

            //this.Dispatcher.Invoke(new Action(delegate()
            //{
            //    try
            //    {
            //        switch (index)
            //        {
            //            case 0:
            //                teVelocityS0.Text = velocity;
            //                break;

            //            case 1:
            //                teVelocityS1.Text = velocity;
            //                break;

            //            case 2:
            //                teVelocityS2.Text = velocity;
            //                break;

            //            case 3:
            //                teVelocityS3.Text = velocity;
            //                break;

            //            case 4:
            //                teVelocityS4.Text = velocity;
            //                break;

            //            case 5:
            //                teVelocityS5.Text = velocity;
            //                break;

            //            case 6:
            //                teVelocityS6.Text = velocity;
            //                break;

            //            case 7:
            //                teVelocityS7.Text = velocity;
            //                break;
            //        } 
            //    }
            //    catch (Exception ex)
            //    {
            //        //Trace.WriteLine(ex.ToString());
            //    }

            //}
            //));
        }
        #endregion

        #region InterfaceKit
    
        void ifKit_Attach(object sender, PE.AttachEventArgs e)
        {
            string serialNumber = e.Device.SerialNumber.ToString();

            Trace.WriteLine(string.Format("T({0}) - InterfaceKit {1} attached in wndDX class!",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                serialNumber));

            Phidgets.InterfaceKit ik = (Phidgets.InterfaceKit)e.Device;

            PhidgetHelper.InterfaceKits.InterfaceKit iK = InterfaceKitsD[serialNumber];

            var sensors =
                from item in XDocument.Parse(_RawXML)
                    .Descendants("FxShow")
                    .Descendants("Hosts")
                    .Descendants("Host")
                    .Elements("InterfaceKit")
                    .Where(item => ((string)item.Attribute("SerialNumber")) == serialNumber).Descendants()
                select item;

            this.Dispatcher.Invoke(delegate()
            {
                try
                {
                    foreach (var sensor in sensors)
                    {
                        Trace.WriteLine(sensor.ToString());

                        int analogInput = int.Parse(sensor.Attribute("AnalogInput").Value);
                        int dataRate = int.Parse(sensor.Attribute("DataRate").Value);
                        int sensitivity = int.Parse(sensor.Attribute("Sensitivity").Value);
                        int minRange = int.Parse(sensor.Attribute("MinRange").Value);
                        int maxRange = int.Parse(sensor.Attribute("MaxRange").Value);

                        switch (sensor.Name.LocalName)
                        {
                            case "DistanceSensor":
                                PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType sensorType;

                                switch (sensor.Attribute("Type").Value)
                                {
                                    case "GP2D120X":
                                        sensorType = PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X;
                                        break;

                                    case "GP2Y0A02":
                                        sensorType = PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2Y0A02;
                                        break;

                                    case "GP2Y0A21":
                                        sensorType = PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2Y0A21;
                                        break;

                                    default:
                                        throw new Exception();
                                }

                                iK.Sensors[analogInput] = new PhidgetHelper.Sensors.DistanceSensor(
                                    ik.sensors[analogInput], sensorType, dataRate, sensitivity, minRange, maxRange);

                                break;

                            case "AnalogSensor":
                                iK.Sensors[analogInput] = new PhidgetHelper.Sensors.AnalogSensor(
                                    ik.sensors[analogInput], dataRate, sensitivity, minRange, maxRange);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });

            InterfaceKits.Add(ik.SerialNumber.ToString());
        }

        #endregion

        #endregion

        #region Main Function Routines

        private void ClosePhidget(Phidgets.Phidget phidget)
        {
            try
            {
#if TRACE
                System.Diagnostics.Debug.WriteLine(String.Format("Closing Phidget: {0}-{1}-{2}",
                    phidget.Type, phidget.SerialNumber, phidget.Name));
#endif

                phidget.close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                //throw;
            }
        }

        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame(true);

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate(object arg)
            {
                DispatcherFrame fr = arg as DispatcherFrame;
                fr.Continue = false;
            }, frame);

            Dispatcher.PushFrame(frame);            
        }

        private void InitializeAdvancedServos()
        {
//            foreach (var item in AdvancedServos)
//            {
//#if TRACE
//                System.Diagnostics.Debug.WriteLine("Procesing AdvancedServo Phidget: " + item.FxSerialNumber);
//#endif

//                try
//                {
//                    if (item.Enable)
//                    {
//                        item.Attach += advancedServo_Attach;
//                        item.Detach += advancedServo_Detach;
//                        item.Error += advancedServo_Error;

//                        // TODO(crhodes): Doesn't make sense to do current change unless have array of display items
//                        //item.CurrentChange += advancedServo_CurrentChange;
//                        //item.PositionChange += advancedServo_PositionChange;
//                        //item.VelocityChange += advancedServo_VelocityChange;

//                        item.open(item.FxSerialNumber, item.IPAddress, item.Port);

//                        AdvancedServosD.Add(item.FxSerialNumber.ToString(), item);
//                    }
//                }
//                catch (Phidgets.PhidgetException pe)
//                {
//                    switch (pe.Type)
//                    {
//                        case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
//                            System.Diagnostics.Debug.WriteLine(
//                                string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
//                                    item.FxSerialNumber));
//                            break;

//                        default:
//                            System.Diagnostics.Debug.WriteLine(
//                                string.Format("{0}\nInterface Kit {0}",
//                                    pe.ToString(),
//                                    item.FxSerialNumber));
//                            break;
//                    }

//                }
//                catch (Exception ex)
//                {
//                    throw;
//                }
//            }

//            cbeAdvancedServos.ItemsSource = AdvancedServosA;            
        }

        private void InitializeHosts()
        {
            foreach (var item in Hosts)
            {
                HostD.Add(item.Name, item);

                PhidgetHelper.SBCs.Host host = HostD[item.Name];

                System.Diagnostics.Debug.WriteLine(
                    string.Format("Host: {0} - {1} - {2} - {3}",
                        host.Name,
                        host.IPAddress,
                        host.Port,
                        host.Enabled.ToString()));
            }

            cbeHosts.ItemsSource = HostD;
        }

        private void InitializeInterfaceKits()
        {
            var interfaceKits =
                from item in XDocument.Parse(_RawXML).Descendants("FxShow").Descendants("Hosts").Descendants("Host").Elements("InterfaceKit")
                select new PhidgetHelper.InterfaceKits.InterfaceKit(
                    item.Parent.Attribute("IPAddress").Value,
                    int.Parse(item.Parent.Attribute("Port").Value),
                    int.Parse(item.Attribute("SerialNumber").Value),
                    bool.Parse(item.Attribute("Enable").Value),
                    bool.Parse(item.Attribute("Embedded").Value)
                    );

            foreach (var item in interfaceKits)
            {
#if TRACE
                System.Diagnostics.Debug.WriteLine(String.Format("T({0}) - Procesing Phidget: {1}",
                    System.Threading.Thread.CurrentThread.ManagedThreadId,
                    item.FxSerialNumber));
#endif

                try
                {
                    if (item.Enable)
                    {
                        item.Attach += item.ifKit_Attach;
                        item.Detach += item.ifKit_Detach;
                        item.Error += item.ifKit_Error;

                        item.Attach += ifKit_Attach;    // Add to ComboBox
                        //item.Detach += ifKit_Detach;
                        //item.Error += ifKit_Error;

                        item.InputChange += item.ifKit_InputChange;
                        item.OutputChange += item.ifKit_OutputChange;
                        item.SensorChange += item.ifKit_SensorChange;

                        //item.InputChange += ifKit_InputChange;
                        //item.OutputChange += ifKit_OutputChange;
                        //item.SensorChange += ifKit_SensorChange;

                        item.open(item.FxSerialNumber, item.IPAddress, item.Port);

                        InterfaceKitsD.Add(item.FxSerialNumber.ToString(), item);
                    }
                }
                catch (Phidgets.PhidgetException pe)
                {
                    switch (pe.Type)
                    {
                        case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
                            System.Diagnostics.Debug.WriteLine(
                                string.Format("TimeOut Error.  InterfaceKit {0} not attached.  Disable in ConfigFile or attach",
                                    item.FxSerialNumber));
                            break;

                        default:
                            System.Diagnostics.Debug.WriteLine(
                                string.Format("{0}\nInterface Kit {0}",
                                    pe.ToString(),
                                    item.FxSerialNumber));
                            break;
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            cbeInterfaceKits.ItemsSource = InterfaceKits;

        }

        private void InitializeServo(Phidgets.AdvancedServoServo motor)
        {

            ////Use a try-catch block around code where you are getting and displaying the servo data
            ////if the current position state has yet to be set, it will throw a PhidgetException for value not set
            ////you can use this to test this and to display that the value is unknown
            //try
            //{
            //    ceEngaged.IsChecked = motor.servos[index].Engaged;
            //    ceSpeedRamping.IsChecked = motor.servos[index].SpeedRamping;

            //    //maxPosnTrackBar.SetRange((int)advServo.servos[index].PositionMin, (int)advServo.servos[index].PositionMax);
            //    //maxPosnTrackBar.Value = (int)advServo.servos[index].PositionMax;
            //    //maxPosnTextBox.Text = ((int)advServo.servos[index].PositionMax).ToString();

            //    //minPosnTrackBar.SetRange((int)advServo.servos[index].PositionMin, (int)advServo.servos[index].PositionMax);
            //    //minPosnTrackBar.Value = (int)advServo.servos[index].PositionMin;
            //    //minPosnTextBox.Text = ((int)advServo.servos[index].PositionMin).ToString();

            //    //positionTrk.SetRange((int)(advServo.servos[index].PositionMin * 100), (int)(advServo.servos[index].PositionMax * 100));

            //    if (motor.servos[index].Engaged)
            //    {
            //        try
            //        {
            //            //positionTrk.Value = (int)(advServo.servos[index].Position * 100);
            //        }
            //        catch { }
            //        teActualPosition.Text = motor.servos[index].Position.ToString();
            //        //target_positionTxt.Text = advServo.servos[index].Position.ToString();
            //    }
            //    else
            //    {
            //        //positionTrk.Value = (int)(minPosnTrackBar.Value * 100);
            //        teActualVelocity.Text = motor.servos[index].Velocity.ToString();
            //        teActualPosition.Text = "Unknown";
            //        //target_positionTxt.Text = positionTrk.Value.ToString();
            //    }

            //    teCurrent.Text = motor.servos[index].Current.ToString();
            //    teActualVelocity.Text = motor.servos[index].Velocity.ToString();

            //    //velocityTrk.SetRange((int)advServo.servos[index].VelocityMin, (int)Math.Round(advServo.servos[index].VelocityMax));
            //    //velocityTrk.Value = (int)advServo.servos[index].VelocityLimit;
            //    //target_velocityTxt.Text = advServo.servos[index].VelocityLimit.ToString();

            //    //accelTrk.SetRange((int)advServo.servos[index].AccelerationMin, (int)advServo.servos[index].AccelerationMax);

            //    try
            //    {
            //        //accelTrk.Value = (int)advServo.servos[index].Acceleration;
            //        //accelTxt.Text = advServo.servos[index].Acceleration.ToString();
            //    }
            //    catch
            //    {
            //        //accelTxt.Text = "Unknown";
            //    }
            //    //maxPosnTextBox.Text = ((int)advServo.servos[index].PositionMax).ToString();
            //    //minPosnTextBox.Text = ((int)advServo.servos[index].PositionMin).ToString();

            //    ceStopped.IsChecked = motor.servos[index].Stopped;

            //    //servoTypeCmb.SelectedItem = advServo.servos[index].Type.ToString();
            //}
            //catch (Phidgets.PhidgetException)
            //{

            //    //teActualPosition.Text = "Unknown";
            //    //teActualVelocity.Text = "Unknown";
            //    //teCurrent.Text = "Unknown";
            //    //motor.servos[index].Engaged = (bool)ceEngaged.IsChecked;

            //    //positionTrk.Maximum = (int)(advServo.servos[0].PositionMax * 100);
            //    //positionTrk.Minimum = (int)(advServo.servos[0].PositionMin * 100);
            //    //positionTrk.Value = (int)(advServo.servos[0].PositionMin * 100);
            //    //velocityTrk.Value = (int)advServo.servos[0].VelocityMin;
            //    //accelTrk.Value = (int)advServo.servos[0].AccelerationMin;

            //    //target_positionTxt.Text = positionTrk.Value.ToString();
            //    //target_velocityTxt.Text = velocityTrk.Value.ToString();
            //    //accelTxt.Text = accelTrk.Value.ToString();
            //}
        }

        private void InitializeServos(Phidgets.AdvancedServo advServo)
        {
            foreach (dxe.ComboBoxEditItem item in cbeServos.SelectedItems)
            {
                int index = int.Parse(item.Content.ToString());
                Trace.WriteLine("InitializingServo:" + index.ToString());
                //InitializeServo(AS.servos[index]);  
            }
        }

        public void LoadShow(string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    //cbEnvironments.Items.Clear();
                    //Environments = null;

                    _RawXML = streamReader.ReadToEnd();

                    //foreach (ListTypeInfo fileType in Environments)
                    //{
                    //    cbEnvironments.Items.Add(fileType);
                    //}
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Windows.MessageBox.Show(string.Format("PopulateListFromFile({0}) Config file not found.", fileName));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("PopulateListFromFile({0}) : {1}", fileName, ex));
            }

            InitializeHosts();

            InitializeInterfaceKits();

            InitializeAdvancedServos();
            

        }
        
        private void LoadShowFromFile(string filePath)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            //openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = filePath;

            if (System.Windows.Forms.DialogResult.OK == openFileDialog1.ShowDialog())
            {
                string fileName = openFileDialog1.FileName;

                LoadShow(fileName);
            }
        }

        private void UnInitializeAdvancedServos()
        {
//            if (null == AdvancedServos)
//            {
//            	return;
//            }
//            foreach (var item in AdvancedServos)
//            {
//#if TRACE
//                System.Diagnostics.Debug.WriteLine("UnInitialize AdvancedServo Phidget: " + item.FxSerialNumber);
//#endif

//                try
//                {
//                    if (item.Enable)
//                    {
//                        item.Attach -= advancedServo_Attach;
//                        item.Detach -= advancedServo_Detach;
//                        item.Error -= advancedServo_Error;

//                        item.CurrentChange -= advancedServo_CurrentChange;
//                        item.PositionChange -= advancedServo_PositionChange;
//                        item.VelocityChange -= advancedServo_VelocityChange;

//                        // Cannot close Phidget if outstanding commands.

//                        DoEvents();

//                        ClosePhidget(item);
//                    }
//                }
//                catch (Phidgets.PhidgetException pe)
//                {
//                    switch (pe.Type)
//                    {
//                        case Phidgets.PhidgetException.ErrorType.PHIDGET_ERR_TIMEOUT:
//                            System.Diagnostics.Debug.WriteLine(
//                                string.Format("TimeOut Error.  AdvancedServos {0} not attached.  Disable in ConfigFile or attach",
//                                    item.FxSerialNumber));
//                            break;

//                        default:
//                            System.Diagnostics.Debug.WriteLine(
//                                string.Format("{0}\nAdvancedServos {0}",
//                                    pe.ToString(),
//                                    item.FxSerialNumber));
//                            break;
//                    }

//                }
//                catch (Exception ex)
//                {
//                    throw;
//                }
//            }
        }

        #endregion
    }
}
