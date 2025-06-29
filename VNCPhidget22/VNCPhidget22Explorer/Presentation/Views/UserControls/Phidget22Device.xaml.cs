﻿using System;
using System.Linq;
using System.Windows;

using Phidgets = Phidget22;

using VNC;
using VNC.Core.Mvvm;

using VNCPhidget22Explorer.Presentation.Views;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class Phidget22Device : ViewBase, IPhidget//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public Phidget22Device()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IPhidgetViewModel)DataContext;

            // Can create directly
            // ViewModel = PhidgetViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public Phidget(IPhidgetViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountV++;
        //    InitializeComponent();

        //    ViewModel = viewModel;

        //    InitializeView();

        //    Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}

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

            lgPhidgetDevice.DataContext = this;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        protected virtual string OnCoerceDeviceLibraryVersion(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceLibraryVersionChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceDeviceLibraryVersion(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceLibraryVersion((string)value);
            else
                return value;
        }

        private static void OnDeviceLibraryVersionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceLibraryVersionChanged((string)e.OldValue, (string)e.NewValue);
        }

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        //public static readonly DependencyProperty PhidgetProperty = DependencyProperty.Register("Phidget", typeof(Phidget22.Phidget), typeof(Phidget), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPhidgetChanged), new CoerceValueCallback(OnCoercePhidget)));

        public Phidget22.Phidget AttachedPhidgetDevice
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidget22.Phidget)GetValue(AttachedPhidgetDeviceProperty);
            set => SetValue(AttachedPhidgetDeviceProperty, value);
        }

        public string DeviceAddress
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceAddressProperty);
            set => SetValue(DeviceAddressProperty, value);
        }

        public Boolean? DeviceAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(DeviceAttachedProperty);
            set => SetValue(DeviceAttachedProperty, value);
        }

        //public Boolean? DeviceAttachedToServer
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Boolean?)GetValue(DeviceAttachedToServerProperty);
        //    set => SetValue(DeviceAttachedToServerProperty, value);
        //}

        public string DeviceClass
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceClassProperty);
            set => SetValue(DeviceClassProperty, value);
        }

        public string DeviceID
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceIDProperty);
            set => SetValue(DeviceIDProperty, value);
        }

        public string DeviceLabel
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceLabelProperty);
            set => SetValue(DeviceLabelProperty, value);
        }

        public string DeviceLibraryVersion
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceLibraryVersionProperty);
            set => SetValue(DeviceLibraryVersionProperty, value);
        }
        public string DeviceName
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceNameProperty);
            set => SetValue(DeviceNameProperty, value);
        }

        public Int32? DevicePort
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DevicePortProperty);
            set => SetValue(DevicePortProperty, value);
        }

        public Int32? DeviceSerialNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DeviceSerialNumberProperty);
            set => SetValue(DeviceSerialNumberProperty, value);
        }
        public string DeviceType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(DeviceTypeProperty);
            set => SetValue(DeviceTypeProperty, value);
        }

        public Int32? DeviceVersion
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(DeviceVersionProperty);
            set => SetValue(DeviceVersionProperty, value);
        }

        #region Dependency Properties


        public static readonly DependencyProperty AttachedPhidgetDeviceProperty = DependencyProperty.Register("AttachedPhidgetDevice", typeof(Phidget22.Phidget), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAttachedPhidgetDeviceChanged), new CoerceValueCallback(OnCoerceAttachedPhidgetDevice)));

        public static readonly DependencyProperty DeviceAddressProperty = DependencyProperty.Register("DeviceAddress", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceAddressChanged), new CoerceValueCallback(OnCoerceDeviceAddress)));
        public static readonly DependencyProperty DeviceAttachedProperty = DependencyProperty.Register("DeviceAttached", typeof(Boolean?), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceAttachedChanged), new CoerceValueCallback(OnCoerceDeviceAttached)));
        //public static readonly DependencyProperty DeviceAttachedToServerProperty = DependencyProperty.Register("DeviceAttachedToServer", typeof(Boolean?), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceAttachedToServerChanged), new CoerceValueCallback(OnCoerceDeviceAttachedToServer)));
        public static readonly DependencyProperty DeviceClassProperty = DependencyProperty.Register("DeviceClass", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceClassChanged), new CoerceValueCallback(OnCoerceDeviceClass)));
        public static readonly DependencyProperty DeviceIDProperty = DependencyProperty.Register("DeviceID", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceIDChanged), new CoerceValueCallback(OnCoerceDeviceID)));
        public static readonly DependencyProperty DeviceLabelProperty = DependencyProperty.Register("DeviceLabel", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceLabelChanged), new CoerceValueCallback(OnCoerceDeviceLabel)));
        public static readonly DependencyProperty DeviceLibraryVersionProperty = DependencyProperty.Register("DeviceLibraryVersion", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceLibraryVersionChanged), new CoerceValueCallback(OnCoerceDeviceLibraryVersion)));
        public static readonly DependencyProperty DeviceNameProperty = DependencyProperty.Register("DeviceName", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceNameChanged), new CoerceValueCallback(OnCoerceDeviceName)));
        public static readonly DependencyProperty DevicePortProperty = DependencyProperty.Register("DevicePort", typeof(Int32?), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDevicePortChanged), new CoerceValueCallback(OnCoerceDevicePort)));
        public static readonly DependencyProperty DeviceSerialNumberProperty = DependencyProperty.Register("DeviceSerialNumber", typeof(Int32?), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceSerialNumberChanged), new CoerceValueCallback(OnCoerceDeviceSerialNumber)));
        public static readonly DependencyProperty DeviceTypeProperty = DependencyProperty.Register("DeviceType", typeof(string), typeof(Phidget22Device), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnDeviceTypeChanged), new CoerceValueCallback(OnCoerceDeviceType)));
        public static readonly DependencyProperty DeviceVersionProperty = DependencyProperty.Register("DeviceVersion", typeof(Int32?), typeof(Phidget22Device), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceVersionChanged), new CoerceValueCallback(OnCoerceDeviceVersion)));

        protected virtual void OnAttachedPhidgetDeviceChanged(Phidget22.Phidget oldValue, Phidget22.Phidget newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual Phidget22.Phidget OnCoerceAttachedPhidgetDevice(Phidget22.Phidget value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual string OnCoerceDeviceAddress(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual Boolean? OnCoerceDeviceAttached(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        //protected virtual Boolean? OnCoerceDeviceAttachedToServer(Boolean? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        protected virtual string OnCoerceDeviceClass(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual string OnCoerceDeviceID(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual string OnCoerceDeviceLabel(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual string OnCoerceDeviceName(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual Int32? OnCoerceDevicePort(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual Int32? OnCoerceDeviceSerialNumber(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual string OnCoerceDeviceType(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual Int32? OnCoerceDeviceVersion(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceAddressChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceAttachedChanged(Boolean? oldValue, Boolean? newValue)
        {
            if (newValue is true)
            {
                DeviceAddress = AttachedPhidgetDevice.ServerPeerName;
                //DeviceAttachedToServer = AttachedPhidgetDevice.AttachedToServer;
                DeviceClass = AttachedPhidgetDevice.DeviceClassName;
                DeviceID = AttachedPhidgetDevice.DeviceID.ToString();
                DeviceLabel = AttachedPhidgetDevice.DeviceLabel;
                DeviceLibraryVersion = Phidgets.Phidget.LibraryVersion; // This is a static field
                DeviceName = AttachedPhidgetDevice.DeviceName;
                //DevicePort = AttachedPhidgetDevice.Port;
                DeviceSerialNumber = AttachedPhidgetDevice.DeviceSerialNumber;
                //DeviceType = AttachedPhidgetDevice.Type;
                DeviceVersion = AttachedPhidgetDevice.DeviceVersion;
            }
            else
            {
                DeviceAddress = "";
                //DeviceAttachedToServer = null;
                DeviceClass = "";
                DeviceID = "";
                DeviceLabel = "";
                DeviceLibraryVersion = "";
                DeviceName = "";
                DevicePort = null;
                DeviceSerialNumber = null;
                DeviceType = "";
                DeviceVersion = null;
            }

            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        //protected virtual void OnDeviceAttachedToServerChanged(Boolean? oldValue, Boolean? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        protected virtual void OnDeviceClassChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceIDChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceLabelChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceNameChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDevicePortChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceSerialNumberChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceTypeChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        protected virtual void OnDeviceVersionChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static void OnAttachedPhidgetDeviceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnAttachedPhidgetDeviceChanged((Phidget22.Phidget)e.OldValue, (Phidget22.Phidget)e.NewValue);
        }

        private static object OnCoerceAttachedPhidgetDevice(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceAttachedPhidgetDevice((Phidget22.Phidget)value);
            else
                return value;
        }

        private static object OnCoerceDeviceAddress(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceAddress((string)value);
            else
                return value;
        }

        private static object OnCoerceDeviceAttached(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceAttached((Boolean?)value);
            else
                return value;
        }

        //private static object OnCoerceDeviceAttachedToServer(DependencyObject o, object value)
        //{
        //    Phidget22Device phidget = (Phidget22Device)o;
        //    if (phidget != null)
        //        return phidget.OnCoerceDeviceAttachedToServer((Boolean?)value);
        //    else
        //        return value;
        //}

        private static object OnCoerceDeviceClass(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceClass((string)value);
            else
                return value;
        }

        private static object OnCoerceDeviceID(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceID((string)value);
            else
                return value;
        }

        private static object OnCoerceDeviceLabel(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceLabel((string)value);
            else
                return value;
        }

        private static object OnCoerceDeviceName(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceName((string)value);
            else
                return value;
        }

        private static object OnCoerceDevicePort(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDevicePort((Int32?)value);
            else
                return value;
        }

        private static object OnCoerceDeviceSerialNumber(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceSerialNumber((Int32?)value);
            else
                return value;
        }

        private static object OnCoerceDeviceType(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceType((string)value);
            else
                return value;
        }

        private static object OnCoerceDeviceVersion(DependencyObject o, object value)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                return phidget.OnCoerceDeviceVersion((Int32?)value);
            else
                return value;
        }

        private static void OnDeviceAddressChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceAddressChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static void OnDeviceAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceAttachedChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        //private static void OnDeviceAttachedToServerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    Phidget22Device phidget = (Phidget22Device)o;
        //    if (phidget != null)
        //        phidget.OnDeviceAttachedToServerChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        //}

        private static void OnDeviceClassChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceClassChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static void OnDeviceIDChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceIDChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static void OnDeviceLabelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceLabelChanged((string)e.OldValue, (string)e.NewValue);
        }
        private static void OnDeviceNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceNameChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static void OnDevicePortChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDevicePortChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }

        private static void OnDeviceSerialNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceSerialNumberChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }
        private static void OnDeviceTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceTypeChanged((string)e.OldValue, (string)e.NewValue);
        }

        private static void OnDeviceVersionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Phidget22Device phidget = (Phidget22Device)o;
            if (phidget != null)
                phidget.OnDeviceVersionChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }

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
