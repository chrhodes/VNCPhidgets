using System;
using System.Threading;
using System.Windows;

namespace PhidgetHelper.Sensors
{
    public class AnalogSensor : DependencyObject
    {
        public Phidgets.InterfaceKitAnalogSensor analogSensor;

        public event EventHandler<DistanceSensorEventArgs> InRange;
        public event EventHandler OutOfRange;

        // Keep track of previous state to slow the rate of repeat events.
        // Start both at false.

        internal bool _IsInRange = false;
        internal bool _IsOutOfRange = false;

        /// <summary>
        /// Initializes a new instance of the AnalogSensor class.
        /// </summary>
        public AnalogSensor()
        {
            DataRate = PhidgetHelper.Constants.DATA_RATE_DEFAULT;
            Sensitivity = PhidgetHelper.Constants.SENSITIVITY_DEFAULT;
        }

        public AnalogSensor(Phidgets.InterfaceKitAnalogSensor sensor, int dataRate, int sensitivity, int minRange, int maxRange)
        {
            analogSensor = sensor;

            DataRate = dataRate;
            Sensitivity = sensitivity;

            MinRange = minRange;
            MaxRange = maxRange;
        }

        public static readonly DependencyProperty MinRangeProperty = DependencyProperty.Register(
            "MinRange", typeof(int), typeof(AnalogSensor), 
            new UIPropertyMetadata(Constants.ANALOG_MIN, new PropertyChangedCallback(OnMinRangeChanged), new CoerceValueCallback(OnCoerceMinRange)));

        private static object OnCoerceMinRange(DependencyObject o, object value)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                return analogSensor.OnCoerceMinRange((int)value);
            else
                return value;
        }

        private static void OnMinRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                analogSensor.OnMinRangeChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceMinRange(int value)
        {
            // TODO: Keep the proposed value within the desired range.
            if (value < Constants.ANALOG_MIN)
            {
                return Constants.ANALOG_MIN;
            }
            else
            {
                return value;                
            }
        }

        protected virtual void OnMinRangeChanged(int oldValue, int newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        public int MinRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(MinRangeProperty);
            }
            set
            {
                SetValue(MinRangeProperty, value);
            }
        }

        public static readonly DependencyProperty MaxRangeProperty = DependencyProperty.Register(
            "MaxRange", typeof(int), typeof(AnalogSensor), 
            new UIPropertyMetadata(Constants.ANALOG_MAX, new PropertyChangedCallback(OnMaxRangeChanged), new CoerceValueCallback(OnCoerceMaxRange)));

        private static object OnCoerceMaxRange(DependencyObject o, object value)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                return analogSensor.OnCoerceMaxRange((int)value);
            else
                return value;
        }

        private static void OnMaxRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                analogSensor.OnMaxRangeChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceMaxRange(int value)
        {
            // TODO: Keep the proposed value within the desired range.
            if (value > Constants.ANALOG_MAX)
            {
                return Constants.ANALOG_MAX;
            }
            else
            {
                return value;
            }
        }

        protected virtual void OnMaxRangeChanged(int oldValue, int newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        public int MaxRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(MaxRangeProperty);
            }
            set
            {
                SetValue(MaxRangeProperty, value);
            }
        }

        public static readonly DependencyProperty SensorRawValueProperty = DependencyProperty.Register(
            "SensorRawValue", typeof(int), typeof(AnalogSensor), new UIPropertyMetadata(0));

        public int SensorRawValue
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(SensorRawValueProperty);
            }
            set
            {
                SetValue(SensorRawValueProperty, value);
            }
        }
        
        public static readonly DependencyProperty SensorValueProperty = DependencyProperty.Register(
            "SensorValue", typeof(int), typeof(AnalogSensor), new UIPropertyMetadata(0));

        public int SensorValue
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(SensorValueProperty);
            }
            set
            {
                SetValue(SensorValueProperty, value);
            }
        }

        public static readonly DependencyProperty DataRateProperty = DependencyProperty.Register(
            "DataRate", typeof(int), typeof(AnalogSensor), 
            new UIPropertyMetadata(Constants.DATA_RATE_MIN, new PropertyChangedCallback(OnDataRateChanged), new CoerceValueCallback(OnCoerceDataRate)));

        private static object OnCoerceDataRate(DependencyObject o, object value)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                return analogSensor.OnCoerceDataRate((int)value);
            else
                return value;
        }

        private static void OnDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                analogSensor.OnDataRateChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceDataRate(int value)
        {
            // TODO: Keep the proposed value within the desired range.
            // NB. This does not allow getting down to low values for directly connected phidgets.

            // Multiples of 8

            value = (value / 8) * 8;

            // NB. Lower values are high data rates.
            if (value < PhidgetHelper.Constants.DATA_RATE_MAX)
            {
            	value = PhidgetHelper.Constants.DATA_RATE_MAX;
            }

            // NB. Higher values are low data rates.
            if (value > PhidgetHelper.Constants.DATA_RATE_MIN)
            {
            	value = PhidgetHelper.Constants.DATA_RATE_MIN;
            }

            return value;
        }

        protected virtual void OnDataRateChanged(int oldValue, int newValue)
        {
            if (analogSensor != null)
            {
            	analogSensor.DataRate = newValue;
            }
            
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        public int DataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(DataRateProperty);
            }
            set
            {
                SetValue(DataRateProperty, value);
            }
        }
        
        public static readonly DependencyProperty SensorChangeTriggerProperty = DependencyProperty.Register(
            "SensorChangeTrigger", typeof(int), typeof(AnalogSensor), 
            new UIPropertyMetadata(Constants.SENSOR_CHANGE_TRIGGER, new PropertyChangedCallback(OnSensorChangeTriggerChanged), new CoerceValueCallback(OnCoerceSensorChangeTrigger)));

        private static object OnCoerceSensorChangeTrigger(DependencyObject o, object value)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                return analogSensor.OnCoerceSensorChangeTrigger((int)value);
            else
                return value;
        }

        private static void OnSensorChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                analogSensor.OnSensorChangeTriggerChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceSensorChangeTrigger(int value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorChangeTriggerChanged(int oldValue, int newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        public int SensorChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(SensorChangeTriggerProperty);
            }
            set
            {
                SetValue(SensorChangeTriggerProperty, value);
            }
        }

        public static readonly DependencyProperty RatiometricProperty = DependencyProperty.Register(
            "Ratiometric", typeof(bool), typeof(AnalogSensor), new UIPropertyMetadata(false));

        public bool Ratiometric
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(RatiometricProperty);
            }
            set
            {
                SetValue(RatiometricProperty, value);
            }
        }

        public static readonly DependencyProperty SensitivityProperty = DependencyProperty.Register(
            "Sensitivity", typeof(int), typeof(AnalogSensor), 
            new UIPropertyMetadata(Constants.SENSITIVITY_DEFAULT, new PropertyChangedCallback(OnSensitivityChanged), new CoerceValueCallback(OnCoerceSensitivity)));

        private static object OnCoerceSensitivity(DependencyObject o, object value)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                return analogSensor.OnCoerceSensitivity((int)value);
            else
                return value;
        }

        private static void OnSensitivityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogSensor analogSensor = o as AnalogSensor;
            if (analogSensor != null)
                analogSensor.OnSensitivityChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceSensitivity(int value)
        {
            // TODO: Keep the proposed value within the desired range.

            // NB. Lower values are higher sensitivity.
            if (value < PhidgetHelper.Constants.SENSITIVITY_MAX)
            {
                value = PhidgetHelper.Constants.SENSITIVITY_MAX;
            }

            // NB. Higher values are lower sensitivity.
            if (value > PhidgetHelper.Constants.SENSITIVITY_MIN)
            {
                value = PhidgetHelper.Constants.SENSITIVITY_MIN;
            }

            return value;
        }

        protected virtual void OnSensitivityChanged(int oldValue, int newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            if (analogSensor != null)
            {
                analogSensor.Sensitivity = newValue;
            }
        }

        public int Sensitivity
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, 
            // do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(SensitivityProperty);
            }
            set
            {
                SetValue(SensitivityProperty, value);
            }
        }

        public virtual void Update(int sensorRawValue, int sensorValue, int dataRate, int sensitivity)
        {
            //Trace.WriteLine(string.Format("T({0}) - Update in AnalogSensor()",
            //    System.Threading.Thread.CurrentThread.ManagedThreadId));

            this.Dispatcher.Invoke(delegate()
            {
                DataRate = dataRate;
                Sensitivity = sensitivity;

                SensorRawValue = sensorRawValue;
                SensorValue = sensorValue;

                FireRangeEvents(sensorValue);
            });
        }

        internal void FireRangeEvents(int sensorValue)
        {
            if ((sensorValue > MinRange) && (sensorValue < MaxRange))
            {
                // Only fire on change of state.
                if (!_IsInRange)
                {
                    FireInRange(new DistanceSensorEventArgs(sensorValue)); ;
                }

                _IsInRange = true;
                _IsOutOfRange = false;
            }
            else
            {
                SensorValue = -1;

                // Only fire on change of state.
                if (!_IsOutOfRange)
                {
                    FireOutOfRange();
                }

                _IsOutOfRange = true;
                _IsInRange = false;
            }
        }

        internal void FireOutOfRange()
        {
            EventHandler temp = Interlocked.CompareExchange(ref OutOfRange, null, null);
            if (temp != null) temp(this, null);
        }

        internal void FireInRange(DistanceSensorEventArgs distanceSensorEventArgs)
        {
            EventHandler<DistanceSensorEventArgs> temp = Interlocked.CompareExchange(ref InRange, null, null);
            if (temp != null) temp(this, distanceSensorEventArgs);
        }
    }
}
