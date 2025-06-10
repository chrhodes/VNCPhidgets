﻿using System;
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
    public partial class VoltageInputControl: ViewBase
    {
        #region Constructors, Initialization, and Load
        
        public VoltageInputControl()
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
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

        public Boolean LogSensorChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogSensorChangeEventsProperty);
            set => SetValue(LogSensorChangeEventsProperty, value);
        }

        private static object OnCoerceLogSensorChangeEvents(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceLogSensorChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogSensorChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnLogSensorChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        public Boolean LogVoltageChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogVoltageChangeEventsProperty);
            set => SetValue(LogVoltageChangeEventsProperty, value);
        }

        private static object OnCoerceLogVoltageChangeEvents(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceLogVoltageChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogVoltageChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnLogVoltageChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogVoltageChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogVoltageChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #endregion

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
        public string ControlTitle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(ControlTitleProperty);
            set => SetValue(ControlTitleProperty, value);
        }

        private static object OnCoerceControlTitle(DependencyObject o, object value)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
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

        #region SerialHubPortChannel

        public static readonly DependencyProperty SerialHubPortChannelProperty = DependencyProperty.Register(
            "SerialHubPortChannel",
            typeof(SerialHubPortChannel),
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSerialHubPortChannel((SerialHubPortChannel)value);
            else
                return value;
        }

        private static void OnSerialHubPortChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSerialHubPortChannelChanged((SerialHubPortChannel)e.OldValue, (SerialHubPortChannel)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSerialNumber((Int32)value);
            else
                return value;
        }

        private static void OnSerialNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSerialNumberChanged((Int32)e.OldValue, (Int32)e.NewValue);
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

        #region HubPort

        public static readonly DependencyProperty HubPortProperty = DependencyProperty.Register(
            "HubPort",
            typeof(Int32),
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceHubPort((Int32)value);
            else
                return value;
        }

        private static void OnHubPortChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnHubPortChanged((Int32)e.OldValue, (Int32)e.NewValue);
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

        #region Channel

        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register(
            "Channel",
            typeof(Int32),
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceChannel((Int32)value);
            else
                return value;
        }

        private static void OnChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnChannelChanged((Int32)e.OldValue, (Int32)e.NewValue);
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

        #region ChannelNumber

        public static readonly DependencyProperty ChannelNumberProperty = DependencyProperty.Register(
            "ChannelNumber",
            typeof(string),
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceChannelNumber((string)value);
            else
                return value;
        }

        private static void OnChannelNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnChannelNumberChanged((string)e.OldValue, (string)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceAttached((Boolean)value);
            else
                return value;
        }

        private static void OnAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        public Phidgets.VoltageSensorType SensorType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.VoltageSensorType)GetValue(SensorTypeProperty);
            set => SetValue(SensorTypeProperty, value);
        }

        private static object OnCoerceSensorType(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceSensorType((Phidgets.VoltageSensorType)value);
            else
                return value;
        }

        private static void OnSensorTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnSensorTypeChanged((Phidgets.VoltageSensorType)e.OldValue, (Phidgets.VoltageSensorType)e.NewValue);
        }

        protected virtual Phidgets.VoltageSensorType OnCoerceSensorType(Phidgets.VoltageSensorType value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSensorTypeChanged(Phidgets.VoltageSensorType oldValue, Phidgets.VoltageSensorType newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SensorDescription

        public static readonly DependencyProperty SensorDescriptionProperty = DependencyProperty.Register(
            "SensorDescription",
            typeof(string),
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSensorDescription((string)value);
            else
                return value;
        }

        private static void OnSensorDescriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSensorDescriptionChanged((string)e.OldValue, (string)e.NewValue);
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

        public Phidgets.Unit SensorUnit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.Unit)GetValue(SensorUnitProperty);
            set => SetValue(SensorUnitProperty, value);
        }

        private static object OnCoerceSensorUnit(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceSensorUnit((Phidgets.Unit)value);
            else
                return value;
        }

        private static void OnSensorUnitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnSensorUnitChanged((Phidgets.Unit)e.OldValue, (Phidgets.Unit)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSensorUnit_Unit((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_UnitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSensorUnit_UnitChanged((string)e.OldValue, (string)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSensorUnit_Name((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_NameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSensorUnit_NameChanged((string)e.OldValue, (string)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSensorUnit_Symbol((string)value);
            else
                return value;
        }

        private static void OnSensorUnit_SymbolChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSensorUnit_SymbolChanged((string)e.OldValue, (string)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoercePowerSupply((Phidgets.PowerSupply)value);
            else
                return value;
        }

        private static void OnPowerSupplyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnPowerSupplyChanged((Phidgets.PowerSupply)e.OldValue, (Phidgets.PowerSupply)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceSensorValueChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnSensorValueChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnSensorValueChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceSensorValue((Double)value);
            else
                return value;
        }

        private static void OnSensorValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnSensorValueChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(VoltageInputControl),
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
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                return VoltageInputControl.OnCoerceSensorValueOutOfRange((Boolean)value);
            else
                return value;
        }

        private static void OnSensorValueOutOfRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl VoltageInputControl = (VoltageInputControl)o;
            if (VoltageInputControl != null)
                VoltageInputControl.OnSensorValueOutOfRangeChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMinDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMinDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMinDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMaxDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMaxDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMaxDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMinDataRate((Double)value);
            else
                return value;
        }

        private static void OnMinDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMinDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceDataRate((Double)value);
            else
                return value;
        }

        private static void OnDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(VoltageInputControl), 
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
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMaxDataRate((Double)value);
            else
                return value;
        }

        private static void OnMaxDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMaxDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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

        public Double MinVoltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageProperty);
            set => SetValue(MinVoltageProperty, value);
        }

        private static void OnMinVoltageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMinVoltageChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinVoltage(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinVoltageChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceMinVoltage(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMinVoltage((Double)value);
            else
                return value;
        }

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

        public Double Voltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageProperty);
            set => SetValue(VoltageProperty, value);
        }

        private static void OnVoltageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnVoltageChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVoltage(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVoltageChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }


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

        public Double MaxVoltage
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageProperty);
            set => SetValue(MaxVoltageProperty, value);
        }

        private static object OnCoerceMaxVoltage(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMaxVoltage((Double)value);
            else
                return value;
        }

        private static void OnMaxVoltageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMaxVoltageChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxVoltage(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxVoltageChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceVoltage(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceVoltage((Double)value);
            else
                return value;
        }

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

        public Double MinVoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVoltageChangeTriggerProperty);
            set => SetValue(MinVoltageChangeTriggerProperty, value);
        }


        private static object OnCoerceMinVoltageChangeTrigger(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMinVoltageChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnMinVoltageChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMinVoltageChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinVoltageChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinVoltageChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

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

        public Double VoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VoltageChangeTriggerProperty);
            set => SetValue(VoltageChangeTriggerProperty, value);
        }

        private static object OnCoerceVoltageChangeTrigger(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceVoltageChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnVoltageChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnVoltageChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVoltageChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVoltageChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
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

        public Double MaxVoltageChangeTrigger
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVoltageChangeTriggerProperty);
            set => SetValue(MaxVoltageChangeTriggerProperty, value);
        }


        private static object OnCoerceMaxVoltageChangeTrigger(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceMaxVoltageChangeTrigger((Double)value);
            else
                return value;
        }

        private static void OnMaxVoltageChangeTriggerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnMaxVoltageChangeTriggerChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxVoltageChangeTrigger(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxVoltageChangeTriggerChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

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

        public Phidgets.VoltageRange VoltageRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.VoltageRange)GetValue(VoltageRangeProperty);
            set => SetValue(VoltageRangeProperty, value);
        }

        private static object OnCoerceVoltageRange(DependencyObject o, object value)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                return voltageInputControl.OnCoerceVoltageRange((Phidgets.VoltageRange)value);
            else
                return value;
        }

        private static void OnVoltageRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VoltageInputControl voltageInputControl = (VoltageInputControl)o;
            if (voltageInputControl != null)
                voltageInputControl.OnVoltageRangeChanged((Phidgets.VoltageRange)e.OldValue, (Phidgets.VoltageRange)e.NewValue);
        }

        protected virtual Phidgets.VoltageRange OnCoerceVoltageRange(Phidgets.VoltageRange value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVoltageRangeChanged(Phidgets.VoltageRange oldValue, Phidgets.VoltageRange newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

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
            var sensorType = e.NewValue?? e.OldValue;

            SensorDescription = ((Phidgets.VoltageSensorType)sensorType).AsString(EnumFormat.Description);

            // TODO(crhodes)
            // Can get fance and use colors, etc.

            switch (sensorType)
            {
                case Phidgets.VoltageSensorType.Voltage:
                    lgVoltage.IsEnabled = true;
                    lgSensor.IsEnabled = false;
                    break;

                default:
                    lgVoltage.IsEnabled = false;
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
