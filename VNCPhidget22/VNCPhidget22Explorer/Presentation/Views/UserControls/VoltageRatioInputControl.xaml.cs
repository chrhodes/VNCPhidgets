using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

using EnumsNET;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;

using Phidgets = Phidget22;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class VoltageRatioInputControl: ViewBase//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public VoltageRatioInputControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IVoltageRatioInputControlViewModel)DataContext;

            // Can create directly
            // ViewModel = VoltageRatioInputControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public VoltageRatioInputControl(IVoltageRatioInputControlViewModel viewModel)
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
            ViewDataContextType = this.DataContext?.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            lgMain.DataContext = this;
            lgLogging.IsCollapsed = true;
            lgChangeTrigger.IsCollapsed = true;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        #region Logging

        #region LogPhidgetEvents

        public static readonly DependencyProperty LogPhidgetEventsProperty = DependencyProperty.Register(
            "LogPhidgetEvents",
            typeof(Boolean),
            typeof(VoltageRatioInputControl),
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
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnLogPhidgetEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(VoltageRatioInputControl),
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
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnLogErrorEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(VoltageRatioInputControl),
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
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnLogPropertyChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogSensorChangeEventsChanged),
                new CoerceValueCallback(OnCoerceLogSensorChangeEvents)
                )
            );

        public Boolean LogSensorChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogSensorChangeEventsProperty);
            set => SetValue(LogSensorChangeEventsProperty, value);
        }

        private static object OnCoerceLogSensorChangeEvents(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceLogSensorChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogSensorChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnLogSensorChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogSensorChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogSensorChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogVoltageRatioChangeEvents

        public static readonly DependencyProperty LogVoltageRatioChangeEventsProperty = DependencyProperty.Register(
            "LogVoltageRatioChangeEvents",
            typeof(Boolean),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogVoltageRatioChangeEventsChanged),
                new CoerceValueCallback(OnCoerceLogVoltageRatioChangeEvents)
                )
            );

        public Boolean LogVoltageRatioChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogVoltageRatioChangeEventsProperty);
            set => SetValue(LogVoltageRatioChangeEventsProperty, value);
        }

        private static object OnCoerceLogVoltageRatioChangeEvents(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceLogVoltageRatioChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogVoltageRatioChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnLogVoltageRatioChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogVoltageRatioChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogVoltageRatioChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #endregion

        #region ControlTitle

        public static readonly DependencyProperty ControlTitleProperty = DependencyProperty.Register(
            "ControlTitle",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnControlTitleChanged),
                new CoerceValueCallback(OnCoerceControlTitle)
                )
            );
        public string ControlTitle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(ControlTitleProperty);
            set => SetValue(ControlTitleProperty, value);
        }

        private static object OnCoerceControlTitle(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnControlTitleChanged((string)e.OldValue, (string)e.NewValue);
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

        #region SerialHubPortChannel

        public static readonly DependencyProperty SerialHubPortChannelProperty = DependencyProperty.Register(
            "SerialHubPortChannel",
            typeof(SerialHubPortChannel),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                new SerialHubPortChannel(),
                new PropertyChangedCallback(OnSerialHubPortChannelChanged),
                new CoerceValueCallback(OnCoerceSerialHubPortChannel)
                )
            );

        public SerialHubPortChannel SerialHubPortChannel
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (SerialHubPortChannel)GetValue(SerialHubPortChannelProperty);
            set => SetValue(SerialHubPortChannelProperty, value);
        }

        private static object OnCoerceSerialHubPortChannel(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSerialHubPortChannel((SerialHubPortChannel)value);
            else
                return value;
        }

        private static void OnSerialHubPortChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSerialHubPortChannelChanged((SerialHubPortChannel)e.OldValue, (SerialHubPortChannel)e.NewValue);
        }

        protected virtual SerialHubPortChannel OnCoerceSerialHubPortChannel(SerialHubPortChannel value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSerialHubPortChannelChanged(SerialHubPortChannel oldValue, SerialHubPortChannel newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SerialNumber

        public static readonly DependencyProperty SerialNumberProperty = DependencyProperty.Register(
            "SerialNumber",
            typeof(Int32),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                0,
                new PropertyChangedCallback(OnSerialNumberChanged),
                new CoerceValueCallback(OnCoerceSerialNumber)
                )
            );

        public Int32 SerialNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(SerialNumberProperty);
            set => SetValue(SerialNumberProperty, value);
        }

        private static object OnCoerceSerialNumber(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSerialNumber((Int32)value);
            else
                return value;
        }

        private static void OnSerialNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSerialNumberChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceSerialNumber(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSerialNumberChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Channel

        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register(
            "Channel",
            typeof(Int32),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                0,
                new PropertyChangedCallback(OnChannelChanged),
                new CoerceValueCallback(OnCoerceChannel)
                )
            );

        public Int32 Channel
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(ChannelProperty);
            set => SetValue(ChannelProperty, value);
        }

        private static object OnCoerceChannel(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceChannel((Int32)value);
            else
                return value;
        }

        private static void OnChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnChannelChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceChannel(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnChannelChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region HubPort

        public static readonly DependencyProperty HubPortProperty = DependencyProperty.Register(
            "HubPort",
            typeof(Int32),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                0,
                new PropertyChangedCallback(OnHubPortChanged),
                new CoerceValueCallback(OnCoerceHubPort)
                )
            );

        public Int32 HubPort
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(HubPortProperty);
            set => SetValue(HubPortProperty, value);
        }

        private static object OnCoerceHubPort(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceHubPort((Int32)value);
            else
                return value;
        }

        private static void OnHubPortChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnHubPortChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceHubPort(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnHubPortChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region ChannelNumber

        public static readonly DependencyProperty ChannelNumberProperty = DependencyProperty.Register(
            "ChannelNumber",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                "0",
                new PropertyChangedCallback(OnChannelNumberChanged),
                new CoerceValueCallback(OnCoerceChannelNumber)
                )
            );

        public string ChannelNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(ChannelNumberProperty);
            set => SetValue(ChannelNumberProperty, value);
        }

        private static object OnCoerceChannelNumber(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceChannelNumber((string)value);
            else
                return value;
        }

        private static void OnChannelNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnChannelNumberChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceChannelNumber(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnChannelNumberChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Attached

        public static readonly DependencyProperty AttachedProperty = DependencyProperty.Register(
            "Attached",
            typeof(Boolean),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(false,
                new PropertyChangedCallback(OnAttachedChanged),
                new CoerceValueCallback(OnCoerceAttached)
                )
            );

        public Boolean Attached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(AttachedProperty);
            set => SetValue(AttachedProperty, value);
        }

        private static object OnCoerceAttached(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceAttached((Boolean)value);
            else
                return value;
        }

        private static void OnAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        // TODO(crhodes)
        // 
        // There are two more properties that are not available on InterfaceKit1018
        // Implement when we get a board that supports them
        // BridgeEnabled
        // BridgeGain

        #region SensorType

        public static readonly DependencyProperty SensorTypeProperty = DependencyProperty.Register(
            "SensorType", 
            typeof(Phidgets.VoltageRatioSensorType), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.VoltageRatioSensorType.VoltageRatio, 
                new PropertyChangedCallback(OnSensorTypeChanged), 
                new CoerceValueCallback(OnCoerceSensorType)
                )
            );

        public Phidgets.VoltageRatioSensorType SensorType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.VoltageRatioSensorType)GetValue(SensorTypeProperty);
            set => SetValue(SensorTypeProperty, value);
        }

        private static object OnCoerceSensorType(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceSensorType((Phidgets.VoltageRatioSensorType)value);
            else
                return value;
        }

        private static void OnSensorTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnSensorTypeChanged((Phidgets.VoltageRatioSensorType)e.OldValue, (Phidgets.VoltageRatioSensorType)e.NewValue);
        }

        protected virtual Phidgets.VoltageRatioSensorType OnCoerceSensorType(Phidgets.VoltageRatioSensorType value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorTypeChanged(Phidgets.VoltageRatioSensorType oldValue, Phidgets.VoltageRatioSensorType newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorDescription

        public static readonly DependencyProperty SensorDescriptionProperty = DependencyProperty.Register(
            "SensorDescription",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnSensorDescriptionChanged),
                new CoerceValueCallback(OnCoerceSensorDescription)
                )
            );
        public string SensorDescription
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(SensorDescriptionProperty);
            set => SetValue(SensorDescriptionProperty, value);
        }

        private static object OnCoerceSensorDescription(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSensorDescription((string)value);
            else
                return value;
        }

        private static void OnSensorDescriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSensorDescriptionChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceSensorDescription(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorDescriptionChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        //#region RatioSensorType

        //public static readonly DependencyProperty RatioSensorTypeProperty = DependencyProperty.Register(
        //    "RatioSensorType",
        //    typeof(Phidgets.VoltageRatioSensorType),
        //    typeof(VoltageRatioInputControl),
        //    new FrameworkPropertyMetadata(
        //        Phidgets.VoltageRatioSensorType.VoltageRatio,
        //        new PropertyChangedCallback(OnRatioSensorTypeChanged),
        //        new CoerceValueCallback(OnCoerceRatioSensorType)
        //        )
        //    );

        //public Phidgets.VoltageRatioSensorType RatioSensorType
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Phidgets.VoltageRatioSensorType)GetValue(RatioSensorTypeProperty);
        //    set => SetValue(RatioSensorTypeProperty, value);
        //}

        //private static object OnCoerceRatioSensorType(DependencyObject o, object value)
        //{
        //    VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
        //    if (voltageRatioInputControl != null)
        //        return voltageRatioInputControl.OnCoerceRatioSensorType((Phidgets.VoltageRatioSensorType)value);
        //    else
        //        return value;
        //}

        //private static void OnRatioSensorTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
        //    if (voltageRatioInputControl != null)
        //        voltageRatioInputControl.OnRatioSensorTypeChanged((Phidgets.VoltageRatioSensorType)e.OldValue, (Phidgets.VoltageRatioSensorType)e.NewValue);
        //}

        //protected virtual Phidgets.VoltageRatioSensorType OnCoerceRatioSensorType(Phidgets.VoltageRatioSensorType value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnRatioSensorTypeChanged(Phidgets.VoltageRatioSensorType oldValue, Phidgets.VoltageRatioSensorType newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        #region SensorUnit

        public static readonly DependencyProperty SensorUnitProperty = DependencyProperty.Register(
            "SensorUnit", 
            typeof(Phidgets.Unit), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.Unit.Volt, 
                new PropertyChangedCallback(OnSensorUnitChanged), 
                new CoerceValueCallback(OnCoerceSensorUnit)
                )
            );

        public Phidgets.Unit SensorUnit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.Unit)GetValue(SensorUnitProperty);
            set => SetValue(SensorUnitProperty, value);
        }

        private static object OnCoerceSensorUnit(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceSensorUnit((Phidgets.Unit)value);
            else
                return value;
        }

        private static void OnSensorUnitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnSensorUnitChanged((Phidgets.Unit)e.OldValue, (Phidgets.Unit)e.NewValue);
        }

        protected virtual Phidgets.Unit OnCoerceSensorUnit(Phidgets.Unit value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorUnitChanged(Phidgets.Unit oldValue, Phidgets.Unit newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorUnit_Unit

        public static readonly DependencyProperty SensorUnit_UnitProperty = DependencyProperty.Register(
            "SensorUnit_Unit",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnSensorUnit_UnitChanged),
                new CoerceValueCallback(OnCoerceSensorUnit_Unit)
                )
            );
        public string SensorUnit_Unit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(SensorUnit_UnitProperty);
            set => SetValue(SensorUnit_UnitProperty, value);
        }

        private static object OnCoerceSensorUnit_Unit(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSensorUnit_Unit((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_UnitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSensorUnit_UnitChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceSensorUnit_Unit(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorUnit_UnitChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorUnit_Name

        public static readonly DependencyProperty SensorUnit_NameProperty = DependencyProperty.Register(
            "SensorUnit_Name",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnSensorUnit_NameChanged),
                new CoerceValueCallback(OnCoerceSensorUnit_Name)
                )
            );
        public string SensorUnit_Name
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(SensorUnit_NameProperty);
            set => SetValue(SensorUnit_NameProperty, value);
        }

        private static object OnCoerceSensorUnit_Name(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSensorUnit_Name((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_NameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSensorUnit_NameChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceSensorUnit_Name(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorUnit_NameChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorUnit_Symbol

        public static readonly DependencyProperty SensorUnit_SymbolProperty = DependencyProperty.Register(
            "SensorUnit_Symbol",
            typeof(string),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnSensorUnit_SymbolChanged),
                new CoerceValueCallback(OnCoerceSensorUnit_Symbol)
                )
            );
        public string SensorUnit_Symbol
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(SensorUnit_SymbolProperty);
            set => SetValue(SensorUnit_SymbolProperty, value);
        }

        private static object OnCoerceSensorUnit_Symbol(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSensorUnit_Symbol((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_SymbolChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSensorUnit_SymbolChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceSensorUnit_Symbol(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorUnit_SymbolChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region PowerSupply

        public static readonly DependencyProperty PowerSupplyProperty = DependencyProperty.Register(
            "PowerSupply", 
            typeof(Phidgets.PowerSupply), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                Phidgets.PowerSupply.Off, 
                new PropertyChangedCallback(OnPowerSupplyChanged), 
                new CoerceValueCallback(OnCoercePowerSupply)
                )
            );
        public Phidgets.PowerSupply PowerSupply
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.PowerSupply)GetValue(PowerSupplyProperty);
            set => SetValue(PowerSupplyProperty, value);
        }

        private static object OnCoercePowerSupply(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoercePowerSupply((Phidgets.PowerSupply)value);
            else
                return value;
        }

        private static void OnPowerSupplyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnPowerSupplyChanged((Phidgets.PowerSupply)e.OldValue, (Phidgets.PowerSupply)e.NewValue);
        }

        protected virtual Phidgets.PowerSupply OnCoercePowerSupply(Phidgets.PowerSupply value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPowerSupplyChanged(Phidgets.PowerSupply oldValue, Phidgets.PowerSupply newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorValueChangeTrigger

        public static readonly DependencyProperty SensorValueChangeTriggerProperty = DependencyProperty.Register(
            "SensorValueChangeTrigger", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnSensorValueChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceSensorValueChangeTrigger)
                )
            );
        public Double SensorValueChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(SensorValueChangeTriggerProperty);
            set => SetValue(SensorValueChangeTriggerProperty, value);
        }

        private static object OnCoerceSensorValueChangeTrigger(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceSensorValueChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnSensorValueChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnSensorValueChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceSensorValueChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorValueChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorValue
        
        public static readonly DependencyProperty SensorValueProperty = DependencyProperty.Register(
            "SensorValue", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnSensorValueChanged), 
                new CoerceValueCallback(OnCoerceSensorValue)
                )
            );


        public Double SensorValue
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(SensorValueProperty);
            set => SetValue(SensorValueProperty, value);
        }

        private static object OnCoerceSensorValue(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceSensorValue((Double)value);
            else
                return value;
        }

        private static void OnSensorValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnSensorValueChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceSensorValue(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorValueChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorValueOutOfRange

        public static readonly DependencyProperty SensorValueOutOfRangeProperty = DependencyProperty.Register(
            "SensorValueOutOfRange",
            typeof(Boolean),
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnSensorValueOutOfRangeChanged),
                new CoerceValueCallback(OnCoerceSensorValueOutOfRange)
                ))
            ;

        public Boolean SensorValueOutOfRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(SensorValueOutOfRangeProperty);
            set => SetValue(SensorValueOutOfRangeProperty, value);
        }

        private static object OnCoerceSensorValueOutOfRange(DependencyObject o, object value)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                return VoltageRatioInputControl.OnCoerceSensorValueOutOfRange((Boolean)value);
            else
                return value;
        }

        private static void OnSensorValueOutOfRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl VoltageRatioInputControl = (VoltageRatioInputControl)o;
            if (VoltageRatioInputControl != null)
                VoltageRatioInputControl.OnSensorValueOutOfRangeChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceSensorValueOutOfRange(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorValueOutOfRangeChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinDataInterval

        public static readonly DependencyProperty MinDataIntervalProperty = DependencyProperty.Register(
            "MinDataInterval", 
            typeof(Int32), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnMinDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceMinDataInterval)
                )
            );

        public Int32 MinDataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(MinDataIntervalProperty);
            set => SetValue(MinDataIntervalProperty, value);
        }

        private static object OnCoerceMinDataInterval(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMinDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMinDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMinDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceMinDataInterval(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinDataIntervalChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region DataInterval

        public static readonly DependencyProperty DataIntervalProperty = DependencyProperty.Register(
            "DataInterval", 
            typeof(Int32), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceDataInterval)
                )
            );

        public Int32 DataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(DataIntervalProperty);
            set => SetValue(DataIntervalProperty, value);
        }

        private static object OnCoerceDataInterval(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceDataInterval(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDataIntervalChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxDataInterval

        public static readonly DependencyProperty MaxDataIntervalProperty = DependencyProperty.Register(
            "MaxDataInterval", 
            typeof(Int32), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnMaxDataIntervalChanged), 
                new CoerceValueCallback(OnCoerceMaxDataInterval)
                )
            );

        public Int32 MaxDataInterval
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(MaxDataIntervalProperty);
            set => SetValue(MaxDataIntervalProperty, value);
        }

        private static object OnCoerceMaxDataInterval(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMaxDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMaxDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMaxDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoerceMaxDataInterval(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxDataIntervalChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region MinDataRate

        public static readonly DependencyProperty MinDataRateProperty = DependencyProperty.Register(
            "MinDataRate", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinDataRateChanged), 
                new CoerceValueCallback(OnCoerceMinDataRate)
                )
            );

        public Double MinDataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinDataRateProperty);
            set => SetValue(MinDataRateProperty, value);
        }
        private static object OnCoerceMinDataRate(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMinDataRate((Double)value);
            else
                return value;
        }

        private static void OnMinDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMinDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinDataRate(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinDataRateChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region DataRate

        public static readonly DependencyProperty DataRateProperty = DependencyProperty.Register(
            "DataRate", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnDataRateChanged),
                new CoerceValueCallback(OnCoerceDataRate)
                )
            );

        public Double DataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(DataRateProperty);
            set => SetValue(DataRateProperty, value);
        }

        private static object OnCoerceDataRate(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceDataRate((Double)value);
            else
                return value;
        }

        private static void OnDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceDataRate(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDataRateChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region MaxDataRate

        public static readonly DependencyProperty MaxDataRateProperty = DependencyProperty.Register(
            "MaxDataRate", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxDataRateChanged), 
                new CoerceValueCallback(OnCoerceMaxDataRate)
                )
            );

        public Double MaxDataRate
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxDataRateProperty);
            set => SetValue(MaxDataRateProperty, value);
        }

        private static object OnCoerceMaxDataRate(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMaxDataRate((Double)value);
            else
                return value;
        }

        private static void OnMaxDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMaxDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxDataRate(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxDataRateChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region MinVoltageRatio
        
        public static readonly DependencyProperty MinVoltageRatioProperty = DependencyProperty.Register(
            "MinVoltageRatio", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinVoltageRatioChanged), 
                new CoerceValueCallback(OnCoerceMinVoltageRatio)
                )
            );

        public Double MinVoltageRatio
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageRatioProperty);
            set => SetValue(MinVoltageRatioProperty, value);
        }

        private static void OnMinVoltageRatioChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMinVoltageRatioChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinVoltageRatio(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinVoltageRatioChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceMinVoltageRatio(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMinVoltageRatio((Double)value);
            else
                return value;
        }

        #endregion

        #region VoltageRatio

        public static readonly DependencyProperty VoltageRatioProperty = DependencyProperty.Register(
            "VoltageRatio", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnVoltageRatioChanged), 
                new CoerceValueCallback(OnCoerceVoltageRatio)
                )
            );

        public Double VoltageRatio
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageRatioProperty);
            set => SetValue(VoltageRatioProperty, value);
        }

        private static void OnVoltageRatioChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnVoltageRatioChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVoltageRatio(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVoltageRatioChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxVoltageRatio

        public static readonly DependencyProperty MaxVoltageRatioProperty = DependencyProperty.Register(
            "MaxVoltageRatio", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxVoltageRatioChanged), 
                new CoerceValueCallback(OnCoerceMaxVoltageRatio)
                )
            );

        public Double MaxVoltageRatio
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageRatioProperty);
            set => SetValue(MaxVoltageRatioProperty, value);
        }

        private static object OnCoerceMaxVoltageRatio(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMaxVoltageRatio((Double)value);
            else
                return value;
        }

        private static void OnMaxVoltageRatioChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMaxVoltageRatioChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxVoltageRatio(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxVoltageRatioChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceVoltageRatio(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceVoltageRatio((Double)value);
            else
                return value;
        }

        #endregion

        #region MinVoltageRatioChangeTrigger

        public static readonly DependencyProperty MinVoltageRatioChangeTriggerProperty = DependencyProperty.Register(
            "MinVoltageRatioChangeTrigger", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinVoltageRatioChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceMinVoltageRatioChangeTrigger)
                )
            );

        public Double MinVoltageRatioChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageRatioChangeTriggerProperty);
            set => SetValue(MinVoltageRatioChangeTriggerProperty, value);
        }


        private static object OnCoerceMinVoltageRatioChangeTrigger(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMinVoltageRatioChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnMinVoltageRatioChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMinVoltageRatioChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinVoltageRatioChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinVoltageRatioChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region VoltageRatioChangeTrigger

        public static readonly DependencyProperty VoltageRatioChangeTriggerProperty = DependencyProperty.Register(
            "VoltageRatioChangeTrigger", 
            typeof(Double), 
            typeof(VoltageRatioInputControl),
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnVoltageRatioChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceVoltageRatioChangeTrigger)
                )
            );

        public Double VoltageRatioChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageRatioChangeTriggerProperty);
            set => SetValue(VoltageRatioChangeTriggerProperty, value);
        }

        private static object OnCoerceVoltageRatioChangeTrigger(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceVoltageRatioChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnVoltageRatioChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnVoltageRatioChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVoltageRatioChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVoltageRatioChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        #endregion

        #region MaxVoltageRatioChangeTrigger

        public static readonly DependencyProperty MaxVoltageRatioChangeTriggerProperty = DependencyProperty.Register(
            "MaxVoltageRatioChangeTrigger", 
            typeof(Double), 
            typeof(VoltageRatioInputControl), 
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxVoltageRatioChangeTriggerChanged), 
                new CoerceValueCallback(OnCoerceMaxVoltageRatioChangeTrigger)
                )
            );

        public Double MaxVoltageRatioChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageRatioChangeTriggerProperty);
            set => SetValue(MaxVoltageRatioChangeTriggerProperty, value);
        }


        private static object OnCoerceMaxVoltageRatioChangeTrigger(DependencyObject o, object value)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                return voltageRatioInputControl.OnCoerceMaxVoltageRatioChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnMaxVoltageRatioChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
            if (voltageRatioInputControl != null)
                voltageRatioInputControl.OnMaxVoltageRatioChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxVoltageRatioChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxVoltageRatioChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        //#region VoltageRange

        //public static readonly DependencyProperty VoltageRangeProperty = DependencyProperty.Register(
        //    "VoltageRange", 
        //    typeof(Phidgets.VoltageRange),
        //    typeof(VoltageRatioInputControl), 
        //    new FrameworkPropertyMetadata(
        //        Phidgets.VoltageRange.Auto, 
        //        new PropertyChangedCallback(OnVoltageRangeChanged), 
        //        new CoerceValueCallback(OnCoerceVoltageRange)
        //        )
        //    );

        //public Phidgets.VoltageRange VoltageRange
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Phidgets.VoltageRange)GetValue(VoltageRangeProperty);
        //    set => SetValue(VoltageRangeProperty, value);
        //}

        //private static object OnCoerceVoltageRange(DependencyObject o, object value)
        //{
        //    VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
        //    if (voltageRatioInputControl != null)
        //        return voltageRatioInputControl.OnCoerceVoltageRange((Phidgets.VoltageRange)value);
        //    else
        //        return value;
        //}

        //private static void OnVoltageRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    VoltageRatioInputControl voltageRatioInputControl = (VoltageRatioInputControl)o;
        //    if (voltageRatioInputControl != null)
        //        voltageRatioInputControl.OnVoltageRangeChanged((Phidgets.VoltageRange)e.OldValue, (Phidgets.VoltageRange)e.NewValue);
        //}

        //protected virtual Phidgets.VoltageRange OnCoerceVoltageRange(Phidgets.VoltageRange value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnVoltageRangeChanged(Phidgets.VoltageRange oldValue, Phidgets.VoltageRange newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        #endregion

        #region Event Handlers

        private void LayoutGroup_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutGroup lg = (LayoutGroup)sender;
            var mbe = e;

            var leftAltDown = Keyboard.IsKeyDown(Key.LeftAlt);
            var leftCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl);

            var rightAltDown = Keyboard.IsKeyDown(Key.RightAlt);
            var rightCtrlDown = Keyboard.IsKeyDown(Key.RightCtrl);

            var children = lg.Children;

            foreach (var child in children)
            {
                if (child.GetType() == typeof(DevExpress.Xpf.Editors.CheckEdit))
                {
                    if (leftCtrlDown || rightCtrlDown) { ((CheckEdit)child).IsChecked = true; }
                    if (leftAltDown || rightAltDown) { ((CheckEdit)child).IsChecked = false; }

                }
            }
        }

        private void SensorType_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var sensorType = e.NewValue;

            SensorDescription = ((Phidgets.VoltageSensorType)sensorType).AsString(EnumFormat.Description);

            // TODO(crhodes)
            // Can get fance and use colors, etc.

            switch (sensorType)
            {
                case Phidgets.VoltageRatioSensorType.VoltageRatio:
                    lgVoltageRatio.IsEnabled = true;
                    lgSensor.IsEnabled = false;
                    break;

                default:
                    lgVoltageRatio.IsEnabled = false;
                    lgSensor.IsEnabled = true;
                    break;
            }
        }

        #endregion

        #region Commands (none)

        #endregion

        #region Public Methods (none)


        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        //#region IInstanceCount

        //private static Int32 _instanceCountV;

        //public Int32 InstanceCountV
        //{
        //    get => _instanceCountV;
        //    set => _instanceCountV = value;
        //}

        //private static Int32 _instanceCountVP;

        //public Int32 InstanceCountVP
        //{
        //    get => _instanceCountVP;
        //    set => _instanceCountVP = value;
        //}


        //#endregion
    }
}
