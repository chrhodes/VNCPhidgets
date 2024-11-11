using System;
using System.Linq;
using System.Windows;

using Phidgets=Phidget22;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using System.DirectoryServices.ActiveDirectory;

namespace VNCPhidget22Explorer.Presentation.Controls
{
    public partial class RCServoontrol: ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public RCServoontrol()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IVoltageInputControlViewModel)DataContext;

            // Can create directly
            // ViewModel = VoltageInputControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public VoltageInputControl(IVoltageInputControlViewModel viewModel)
        // {
        // Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        // InstanceCountV++;
        // InitializeComponent();

        // ViewModel = viewModel;

        // InitializeView();

        // Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        // }

         private void InitializeView()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.ViewLow) startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View
            // Hook event handlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            lgMain.DataContext = this;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        #region ControlTitle

        public static readonly DependencyProperty ControlTitleProperty = DependencyProperty.Register(
            "ControlTitle",
            typeof(string),
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnControlTitleChanged),
                new CoerceValueCallback(OnCoerceControlTitle)
                )
            );

        public Phidgets.VoltageRange VoltageRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.VoltageRange)GetValue(VoltageRangeProperty);
            set => SetValue(VoltageRangeProperty, value);
        }
        public Double MaxVoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageChangeTriggerProperty);
            set => SetValue(MaxVoltageChangeTriggerProperty, value);
        }
        public Double VoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageChangeTriggerProperty);
            set => SetValue(VoltageChangeTriggerProperty, value);
        }
        public Double MinVoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageChangeTriggerProperty);
            set => SetValue(MinVoltageChangeTriggerProperty, value);
        }
        public Double MaxVoltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageProperty);
            set => SetValue(MaxVoltageProperty, value);
        }
        public Double Voltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageProperty);
            set => SetValue(VoltageProperty, value);
        }
        public Double MinVoltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageProperty);
            set => SetValue(MinVoltageProperty, value);
        }
        public Double MaxDataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxDataRateProperty);
            set => SetValue(MaxDataRateProperty, value);
        }
        public Double DataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(DataRateProperty);
            set => SetValue(DataRateProperty, value);
        }
        public Double MinDataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinDataRateProperty);
            set => SetValue(MinDataRateProperty, value);
        }
        public Int32 MaxDataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(MaxDataIntervalProperty);
            set => SetValue(MaxDataIntervalProperty, value);
        }
        public Int32 DataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(DataIntervalProperty);
            set => SetValue(DataIntervalProperty, value);
        }
        public Int32 MinDataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(MinDataIntervalProperty);
            set => SetValue(MinDataIntervalProperty, value);
        }
        public Double SensorValue
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(SensorValueProperty);
            set => SetValue(SensorValueProperty, value);
        }
        public Double SensorValueChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(SensorValueChangeTriggerProperty);
            set => SetValue(SensorValueChangeTriggerProperty, value);
        }
        public Phidgets.PowerSupply PowerSupply
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.PowerSupply)GetValue(PowerSupplyProperty);
            set => SetValue(PowerSupplyProperty, value);
        }
        public Phidgets.Unit SensorUnit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.Unit)GetValue(SensorUnitProperty);
            set => SetValue(SensorUnitProperty, value);
        }
        public Phidgets.VoltageSensorType SensorType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.VoltageSensorType)GetValue(SensorTypeProperty);
            set => SetValue(SensorTypeProperty, value);
        }
        public Boolean LogVoltageChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogVoltageChangeEventsProperty);
            set => SetValue(LogVoltageChangeEventsProperty, value);
        }
        public Boolean LogSensorChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogSensorChangeEventsProperty);
            set => SetValue(LogSensorChangeEventsProperty, value);
        }
        public string ControlTitle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(ControlTitleProperty);
            set => SetValue(ControlTitleProperty, value);
        }

        private static object OnCoerceControlTitle(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                VoltageInputControl.OnControlTitleChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceControlTitle(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnControlTitleChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region IsAttached

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.Register(
            "IsAttached",
            typeof(Boolean),
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(false,
                new PropertyChangedCallback(OnIsAttachedChanged),
                new CoerceValueCallback(OnCoerceIsAttached)
                )
            );

        public Boolean IsAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(IsAttachedProperty);
            set => SetValue(IsAttachedProperty, value);
        }

        private static object OnCoerceIsAttached(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceIsAttached((Boolean)value);
            else
                return value;
        }

        private static void OnIsAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                VoltageInputControl.OnIsAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceIsAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnIsAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region LogPhidgetEvents

        public static readonly DependencyProperty LogPhidgetEventsProperty = DependencyProperty.Register(
            "LogPhidgetEvents",
            typeof(Boolean),
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogPhidgetEventsChanged),
                new CoerceValueCallback(OnCoerceLogPhidgetEvents)
                )
            );

        public Boolean LogPhidgetEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogPhidgetEventsProperty);
            set => SetValue(LogPhidgetEventsProperty, value);
        }

        private static object OnCoerceLogPhidgetEvents(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                VoltageInputControl.OnLogPhidgetEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogPhidgetEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogPhidgetEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogErrorEvents

        public static readonly DependencyProperty LogErrorEventsProperty = DependencyProperty.Register(
            "LogErrorEvents",
            typeof(Boolean),
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogErrorEventsChanged),
                new CoerceValueCallback(OnCoerceLogErrorEvents)
                ))
            ;

        public Boolean LogErrorEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogErrorEventsProperty);
            set => SetValue(LogErrorEventsProperty, value);
        }

        private static object OnCoerceLogErrorEvents(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                VoltageInputControl.OnLogErrorEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogErrorEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogErrorEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogPropertyChangeEvents

        public static readonly DependencyProperty LogPropertyChangeEventsProperty = DependencyProperty.Register(
            "LogPropertyChangeEvents",
            typeof(Boolean),
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogPropertyChangeEventsChanged),
                new CoerceValueCallback(OnCoerceLogPropertyChangeEvents)));

        public Boolean LogPropertyChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogPropertyChangeEventsProperty);
            set => SetValue(LogPropertyChangeEventsProperty, value);
        }

        private static object OnCoerceLogPropertyChangeEvents(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = o as VoltageInputControl;
            if (VoltageInputControl != null)
                VoltageInputControl.OnLogPropertyChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogPropertyChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogPropertyChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region LogSensorChangeEvents

        public static readonly DependencyProperty LogSensorChangeEventsProperty = DependencyProperty.Register(
            "LogSensorChangeEvents", 
            typeof(Boolean), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                false, 
                new PropertyChangedCallback(OnLogSensorChangeEventsChanged), 
                new CoerceValueCallback(OnCoerceLogSensorChangeEvents)
                )
            );


        #endregion

        #region LogVoltageChangeEvents

        public static readonly DependencyProperty LogVoltageChangeEventsProperty = DependencyProperty.Register(
            "LogVoltageChangeEvents", 
            typeof(Boolean), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                false, 
                new PropertyChangedCallback(OnLogVoltageChangeEventsChanged), 
                new CoerceValueCallback(OnCoerceLogVoltageChangeEvents)
                )
            );


        #endregion

        #region SensorType

        public static readonly DependencyProperty SensorTypeProperty = DependencyProperty.Register(
            "SensorType", 
            typeof(Phidgets.VoltageSensorType), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.VoltageSensorType.Voltage, 
                new PropertyChangedCallback(OnSensorTypeChanged), 
                new CoerceValueCallback(OnCoerceSensorType)
                )
            );


        #endregion

        #region SensorUnit

        public static readonly DependencyProperty SensorUnitProperty = DependencyProperty.Register(
            "SensorUnit", 
            typeof(Phidgets.Unit), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.Unit.Volt, 
                new PropertyChangedCallback(OnSensorUnitChanged), 
                new CoerceValueCallback(OnCoerceSensorUnit)
                )
            );


        #endregion

        #region PowerSupply

        public static readonly DependencyProperty PowerSupplyProperty = DependencyProperty.Register(
            "PowerSupply", 
            typeof(Phidgets.PowerSupply), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(Phidgets.PowerSupply.Off, 
                new PropertyChangedCallback(OnPowerSupplyChanged), 
                new CoerceValueCallback(OnCoercePowerSupply)
                )
            );


        #endregion

        #region SensorValueChangeTrigger

        public static readonly DependencyProperty SensorValueChangeTriggerProperty = DependencyProperty.Register(
            "SensorValueChangeTrigger", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnSensorValueChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceSensorValueChangeTrigger)
                )
            );


        #endregion

        #region SensorValue
        public static readonly DependencyProperty SensorValueProperty = DependencyProperty.Register(
            "SensorValue", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnSensorValueChanged), 
                new CoerceValueCallback(OnCoerceSensorValue)
                )
            );


        #endregion

        #region MinDataInterval

        public static readonly DependencyProperty MinDataIntervalProperty = DependencyProperty.Register(
            "MinDataInterval", 
            typeof(Int32), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnMinDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceMinDataInterval)
                )
            );


        #endregion

        #region DataInterval

        public static readonly DependencyProperty DataIntervalProperty = DependencyProperty.Register(
            "DataInterval", 
            typeof(Int32), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceDataInterval)
                )
            );


        #endregion

        #region MaxDataInterval

        public static readonly DependencyProperty MaxDataIntervalProperty = DependencyProperty.Register(
            "MaxDataInterval", 
            typeof(Int32), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnMaxDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceMaxDataInterval)
                )
            );


        #endregion

        #region MinDataRate

        public static readonly DependencyProperty MinDataRateProperty = DependencyProperty.Register(
            "MinDataRate", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinDataRateChanged), 
                new CoerceValueCallback(OnCoerceMinDataRate)
                )
            );


        #endregion

        #region DataRate

        public static readonly DependencyProperty DataRateProperty = DependencyProperty.Register(
            "DataRate", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnDataRateChanged),
                new CoerceValueCallback(OnCoerceDataRate)
                )
            );


        #endregion

        #region MaxDataRate

        public static readonly DependencyProperty MaxDataRateProperty = DependencyProperty.Register(
            "MaxDataRate", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxDataRateChanged), 
                new CoerceValueCallback(OnCoerceMaxDataRate)
                )
            );


        #endregion

        #region MinVoltage
        public static readonly DependencyProperty MinVoltageProperty = DependencyProperty.Register(
            "MinVoltage", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinVoltageChanged), 
                new CoerceValueCallback(OnCoerceMinVoltage)
                )
            );


        #endregion

        #region Voltage
        public static readonly DependencyProperty VoltageProperty = DependencyProperty.Register(
            "Voltage", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnVoltageChanged), 
                new CoerceValueCallback(OnCoerceVoltage)
                )
            );


        #endregion

        #region MaxVoltage
        public static readonly DependencyProperty MaxVoltageProperty = DependencyProperty.Register(
            "MaxVoltage", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxVoltageChanged), 
                new CoerceValueCallback(OnCoerceMaxVoltage)
                )
            );


        #endregion

        #region MinVoltageChangeTrigger

        public static readonly DependencyProperty MinVoltageChangeTriggerProperty = DependencyProperty.Register(
            "MinVoltageChangeTrigger", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinVoltageChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceMinVoltageChangeTrigger)
                )
            );


        #endregion

        #region VoltageChangeTrigger

        public static readonly DependencyProperty VoltageChangeTriggerProperty = DependencyProperty.Register(
            "VoltageChangeTrigger", 
            typeof(Double), 
            typeof(VoltageInputControl),
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnVoltageChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceVoltageChangeTrigger)
                )
            );


        #endregion

        #region MaxVoltageChangeTrigger

        public static readonly DependencyProperty MaxVoltageChangeTriggerProperty = DependencyProperty.Register(
            "MaxVoltageChangeTrigger", 
            typeof(Double), 
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxVoltageChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceMaxVoltageChangeTrigger)
                )
            );


        #endregion

        #region VoltageRange

        public static readonly DependencyProperty VoltageRangeProperty = DependencyProperty.Register(
            "VoltageRange", 
            typeof(Phidgets.VoltageRange),
            typeof(VoltageInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.VoltageRange.Auto, 
                new PropertyChangedCallback(OnVoltageRangeChanged), 
                new CoerceValueCallback(OnCoerceVoltageRange)
                )
            );
        
        
        #endregion

        #endregion

        #region Event Handlers (none)


        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        #region IInstanceCount

        private static int _instanceCountV;

        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        private static int _instanceCountVP;

        public int InstanceCountVP
        {
            get => _instanceCountVP;
            set => _instanceCountVP = value;
        }


        #endregion        
    }
}
