using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PhidgetHelper.Sensors
{
    public class DistanceSensor : AnalogSensor
    {

        /// <summary>
        /// Initializes a new instance of the DistanceSensor class.
        /// </summary>
        /// 
        public DistanceSensor(Phidgets.InterfaceKitAnalogSensor sensor, DistanceSensorType sensorType)
        {
            analogSensor = sensor;
            SensorType = sensorType;

            DataRate = PhidgetHelper.Constants.DATA_RATE_DEFAULT;
            Sensitivity = PhidgetHelper.Constants.SENSITIVITY_DEFAULT;

            switch (sensorType)
            {
                case DistanceSensorType.GP2D120X:
                    MinRange = 4;
                    MaxRange = 30;
                    break;

                case DistanceSensorType.GP2Y0A21:
                    MinRange = 10;
                    MaxRange = 80;
                    break;

                case DistanceSensorType.GP2Y0A02:
                    MinRange = 20;
                    MaxRange = 150;
                    break;
            }
        }

        public DistanceSensor(Phidgets.InterfaceKitAnalogSensor sensor, DistanceSensorType sensorType, int dataRate, int sensitivity, int minRange, int maxRange)
        {
            analogSensor = sensor;
            SensorType = sensorType;

            DataRate = dataRate;
            Sensitivity = sensitivity;

            MinRange = minRange;
            MaxRange = maxRange;
        }

        public enum DistanceSensorType
        {
            GP2D120X,   //  4-30 cm
            GP2Y0A21,   // 10-80 cm
            GP2Y0A02    // 20-150 cm
        }

        protected override int OnCoerceMaxRange(int value)
        {
            // TODO: Keep the proposed value within the desired range of the particular sensor.

            switch (SensorType)
            {
                case DistanceSensorType.GP2D120X:
                    if (value > 30)
                    {
                        value = 30;
                    }

                    break;

                case DistanceSensorType.GP2Y0A21:
                    if (value > 80)
                    {
                        value = 80;
                    }

                    break;

                case DistanceSensorType.GP2Y0A02:
                    if (value > 150)
                    {
                        value = 150;
                    }

                    break;
            }

            return value;
        }

        protected override int OnCoerceMinRange(int value)
        {
            // TODO: Keep the proposed value within the desired range.
            // TODO: Keep the proposed value within the desired range of the particular sensor.

            switch (SensorType)
            {
                case DistanceSensorType.GP2D120X:
                    if (value < 4)
                    {
                        value = 4;
                    }

                    break;

                case DistanceSensorType.GP2Y0A21:
                    if (value < 10)
                    {
                        value = 10;
                    }

                    break;

                case DistanceSensorType.GP2Y0A02:
                    if (value < 20)
                    {
                        value = 20;
                    }

                    break;
            }

            return value;
        }

        //public static readonly DependencyProperty SensorTypeProperty = DependencyProperty.Register(
        //    "SensorType", typeof(DistanceSensorType), typeof(DistanceSensorY), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SensorTypeProperty = DependencyProperty.Register(
            "SensorType", typeof(DistanceSensorType), typeof(DistanceSensor), new UIPropertyMetadata(null));

        public DistanceSensorType SensorType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (DistanceSensorType)GetValue(SensorTypeProperty);
            }
            set
            {
                SetValue(SensorTypeProperty, value);
            }
        }

        public override void Update(int sensorRawValue, int sensorValue, int dataRate, int sensitivity)
        {
            //Trace.WriteLine(string.Format("T({0}) - Update in DistanceSensor()",
            //    System.Threading.Thread.CurrentThread.ManagedThreadId));

            this.Dispatcher.Invoke(delegate()
            {
                DataRate = dataRate;
                Sensitivity = sensitivity;
                SensorRawValue = sensorRawValue;
                //SensorValue = sensorValue;

                double calculatedValue = 0;

                switch (SensorType)
                {
                    case DistanceSensorType.GP2D120X:
                        calculatedValue = 2076 / (sensorValue - 11.0);
                        break;

                    case DistanceSensorType.GP2Y0A21:
                        calculatedValue = 4800 / (sensorValue - 20.0);
                        break;

                    case DistanceSensorType.GP2Y0A02:
                        calculatedValue = 9462 / (sensorValue - 16.92);
                        break;
                }

                SensorValue = (int)calculatedValue;

                FireRangeEvents(SensorValue);
            });
        }
    }
}
