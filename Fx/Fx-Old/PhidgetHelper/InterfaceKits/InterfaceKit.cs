using System.Diagnostics;

using PE = Phidgets.Events;

namespace PhidgetHelper.InterfaceKits
{
    public class InterfaceKit : Phidgets.InterfaceKit
    {
        // Fields...
        private int _FxSerialNumber;

        private bool _Embedded;
        private bool _Enable;

        private int _Port;
        private string _IPAddress;

        /// <summary>
        /// Initializes a new instance of the InterfaceKit class.
        /// </summary>
        /// <param name="embedded"></param>
        /// <param name="enabled"></param>
        public InterfaceKit(string ipAddress, int port, int fXSerialNumber, bool enable, bool embedded)
        {
            Trace.WriteLine(string.Format("T({0}) - InterfaceKit()",
                System.Threading.Thread.CurrentThread.ManagedThreadId));

            _IPAddress = ipAddress;
            _Port = port;
            _FxSerialNumber = fXSerialNumber;
            _Embedded = embedded;
            _Enable = enable;
        }

        public  PhidgetHelper.Sensors.AnalogSensor[] Sensors = new PhidgetHelper.Sensors.AnalogSensor[8];

        public bool Embedded
        {
            get { return _Embedded; }
            set
            {
                _Embedded = value;
            }
        }

        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;
            }
        }

        public int FxSerialNumber
        {
            get { return _FxSerialNumber; }
            set
            {
                _FxSerialNumber = value;
            }
        }

        public string IPAddress
        {
            get { return _IPAddress; }
            set
            {
                _IPAddress = value;
            }
        }

        public int Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
            }
        }

        //Attach event handler...Display the serial number of the attached InterfaceKit 
        //to the console      
        public void ifKit_Attach(object sender, PE.AttachEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - InterfaceKit {1} attached in InterfaceKits class!",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                e.Device.SerialNumber.ToString()));

            Phidgets.InterfaceKit ik = (Phidgets.InterfaceKit)e.Device;

            Trace.WriteLine(ik.SerialNumber.ToString());

            // TODO(crhodes): Can't create here because we are likely on a different thread, sigh.

        //    sensor0 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor1 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor2 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor3 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor4 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor5 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor6 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        //    sensor7 = new PhidgetHelper.Sensors.DistanceSensor(PhidgetHelper.Sensors.DistanceSensor.DistanceSensorType.GP2D120X, 4, 30);
        }

        //Detach event handler...Display the serial number of the detached InterfaceKit 
        //to the console
        public void ifKit_Detach(object sender, PE.DetachEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - InterfaceKit {1} detached!",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                e.Device.SerialNumber.ToString()));
        }

        //Error event handler...Display the error description to the console
        public void ifKit_Error(object sender, PE.ErrorEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - ifKit_Error(): Code:{1}\nDescription:{2}\nType:{3}",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                e.Code, e.Description, e.Type));
        }

        public void ifKit_InputChange(object sender, PE.InputChangeEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - InterfaceKit {1} InputChanged()",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                ((Phidgets.InterfaceKit)sender).SerialNumber.ToString()));
        }

        public void ifKit_OutputChange(object sender, PE.OutputChangeEventArgs e)
        {
            Trace.WriteLine(string.Format("T({0}) - InterfaceKit {1} OutputChanged()",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                                ((Phidgets.InterfaceKit)sender).SerialNumber.ToString()));
        }

        public void ifKit_SensorChange(object sender, PE.SensorChangeEventArgs e)
        {
            Phidgets.InterfaceKitAnalogSensor sensor = ((Phidgets.InterfaceKit)sender).sensors[e.Index];

            int value = sensor.Value;
            int rawValue = sensor.RawValue;
            int dataRateMax = sensor.DataRateMax;
            int dataRateMin = sensor.DataRateMin;

            //Trace.WriteLine(string.Format("T({0}) - IK[{1}] s[{2}] Changed(RawValue:{3} Value:{4})  DataRateMax:{5}  DataRateMin:{6} DataRate:{7} Sensitivity:{8}",
            Trace.WriteLine(string.Format("T({0}) - IK[{1,6}] s[{2}] Changed(RawValue:{3,4} Value:{4,4})  DataRate:{5,4} Sensitivity:{6,4}",
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                ((Phidgets.InterfaceKit)sender).SerialNumber.ToString(),
                e.Index,
                sensor.RawValue,
                sensor.Value,
                //sensor.DataRateMax,
                //sensor.DataRateMin,
                sensor.DataRate,
                sensor.Sensitivity
                ));

            //if (e.Index == 0)
            //{
            //    this.Dispatcher.Invoke(delegate()
            //    {
                    Sensors[e.Index].Update(sensor.RawValue, sensor.Value, sensor.DataRate, sensor.Sensitivity);
                    //sensor0.Update(sensor.RawValue, sensor.Value, sensor.DataRate, sensor.Sensitivity);
                    //sensor0.Update(sensor.RawValue, sensor.Value, sensor.DataRate, sensor.Sensitivity);
            //    });
            //}

        }

    }
}
