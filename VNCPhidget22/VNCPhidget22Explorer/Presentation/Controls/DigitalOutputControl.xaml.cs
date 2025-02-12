using System;
using System.Linq;
using System.Windows;

using Phidgets=Phidget22;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using DevExpress.Xpf.Editors;
using System.Windows.Input;
using DevExpress.Xpf.LayoutControl;

namespace VNCPhidget22Explorer.Presentation.Controls
{
    public partial class DigitalOutputControl: ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public DigitalOutputControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IDigitalOutputControlViewModel)DataContext;

            // Can create directly
            // ViewModel = DigitalOutputControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public DigitalOutputControl(IDigitalOutputControlViewModel viewModel)
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
            lgLogging.IsCollapsed = true;

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
            typeof(DigitalOutputControl), 
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
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                return digitalOutputControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                digitalOutputControl.OnControlTitleChanged((string)e.OldValue, (string)e.NewValue);
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

        #region ChannelNumber

        public static readonly DependencyProperty ChannelNumberProperty = DependencyProperty.Register(
            "ChannelNumber",
            typeof(string),
            typeof(DigitalOutputControl),
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
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceChannelNumber((string)value);
            else
                return value;
        }

        private static void OnChannelNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnChannelNumberChanged((string)e.OldValue, (string)e.NewValue);
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

        #region LogPhidgetsEvents

        public static readonly DependencyProperty LogPhidgetEventsProperty = DependencyProperty.Register("LogPhidgetEvents", typeof(Boolean), typeof(DigitalOutputControl), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnLogPhidgetEventsChanged), new CoerceValueCallback(OnCoerceLogPhidgetEvents)));


        public Boolean LogPhidgetEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogPhidgetEventsProperty);
            set => SetValue(LogPhidgetEventsProperty, value);
        }

        private static object OnCoerceLogPhidgetEvents(DependencyObject o, object value)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnLogPhidgetEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        public static readonly DependencyProperty LogErrorEventsProperty = DependencyProperty.Register("LogErrorEvents", typeof(Boolean), typeof(DigitalOutputControl), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnLogErrorEventsChanged), new CoerceValueCallback(OnCoerceLogErrorEvents)));

        public Boolean LogErrorEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogErrorEventsProperty);
            set => SetValue(LogErrorEventsProperty, value);
        }

        private static object OnCoerceLogErrorEvents(DependencyObject o, object value)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnLogErrorEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        public static readonly DependencyProperty LogPropertyChangeEventsProperty = DependencyProperty.Register("LogPropertyChangeEvents", typeof(Boolean), typeof(DigitalOutputControl), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnLogPropertyChangeEventsChanged), new CoerceValueCallback(OnCoerceLogPropertyChangeEvents)));

        public Boolean LogPropertyChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogPropertyChangeEventsProperty);
            set => SetValue(LogPropertyChangeEventsProperty, value);
        }

        private static object OnCoerceLogPropertyChangeEvents(DependencyObject o, object value)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnLogPropertyChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region IsAttached

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.Register("IsAttached", typeof(Boolean), typeof(DigitalOutputControl), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsAttachedChanged), new CoerceValueCallback(OnCoerceIsAttached)));

        public Boolean IsAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(IsAttachedProperty);
            set => SetValue(IsAttachedProperty, value);
        }

        private static object OnCoerceIsAttached(DependencyObject o, object value)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceIsAttached((Boolean)value);
            else
                return value;
        }

        private static void OnIsAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnIsAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region State

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State",
            typeof(Boolean),
            typeof(DigitalOutputControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnStateChanged),
                new CoerceValueCallback(OnCoerceState)));

        public Boolean State
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        private static object OnCoerceState(DependencyObject o, object value)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                return DigitalOutputControl.OnCoerceState((Boolean)value);
            else
                return value;
        }

        private static void OnStateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl DigitalOutputControl = o as DigitalOutputControl;
            if (DigitalOutputControl != null)
                DigitalOutputControl.OnStateChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceState(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStateChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinDutyCycle

        public static readonly DependencyProperty MinDutyCycleProperty = DependencyProperty.Register(
            "MinDutyCycle",
            typeof(Double),
            typeof(DigitalOutputControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinDutyCycleChanged),
                new CoerceValueCallback(OnCoerceMinDutyCycle)
                )
            );

        public Double MinDutyCycle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinDutyCycleProperty);
            set => SetValue(MinDutyCycleProperty, value);
        }

        private static object OnCoerceMinDutyCycle(DependencyObject o, object value)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                return digitalOutputControl.OnCoerceMinDutyCycle((Double)value);
            else
                return value;
        }

        private static void OnMinDutyCycleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                digitalOutputControl.OnMinDutyCycleChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinDutyCycle(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinDutyCycleChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region DutyCycle

        public static readonly DependencyProperty DutyCycleProperty = DependencyProperty.Register(
            "DutyCycle",
            typeof(Double),
            typeof(DigitalOutputControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnDutyCycleChanged),
                new CoerceValueCallback(OnCoerceDutyCycle)
                )
            );

        public Double DutyCycle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(DutyCycleProperty);
            set => SetValue(DutyCycleProperty, value);
        }

        private static object OnCoerceDutyCycle(DependencyObject o, object value)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                return digitalOutputControl.OnCoerceDutyCycle((Double)value);
            else
                return value;
        }

        private static void OnDutyCycleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                digitalOutputControl.OnDutyCycleChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceDutyCycle(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDutyCycleChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxDutyCycle

        public static readonly DependencyProperty MaxDutyCycleProperty = DependencyProperty.Register(
            "MaxDutyCycle",
            typeof(Double),
            typeof(DigitalOutputControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxDutyCycleChanged),
                new CoerceValueCallback(OnCoerceMaxDutyCycle)
                )
            );

        public Double MaxDutyCycle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxDutyCycleProperty);
            set => SetValue(MaxDutyCycleProperty, value);
        }

        private static object OnCoerceMaxDutyCycle(DependencyObject o, object value)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                return digitalOutputControl.OnCoerceMaxDutyCycle((Double)value);
            else
                return value;
        }

        private static void OnMaxDutyCycleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalOutputControl digitalOutputControl = o as DigitalOutputControl;
            if (digitalOutputControl != null)
                digitalOutputControl.OnMaxDutyCycleChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxDutyCycle(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxDutyCycleChanged(Double oldValue, Double newValue)
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
