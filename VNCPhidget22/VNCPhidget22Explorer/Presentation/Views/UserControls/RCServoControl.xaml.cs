using System;
using System.Linq;
using System.Windows;

using Phidgets = Phidget22;

using VNC;
using VNC.Core.Mvvm;
using VNC.Phidget22;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Input;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Editors;
using VNC.Phidget22.Ex;
using VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class RCServoControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load

        public RCServoControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IRCServoControlViewModel)DataContext;

            // Can create directly
            // ViewModel = RCServoControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public RCServoControl(IRCServoControlViewModel viewModel)
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
            //liConfigureServo.DataContext = this.Parent;
            lgMovementCharacteristics.IsCollapsed = true;
            lgConfigureServo.IsCollapsed = true;
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
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                "",
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnControlTitleChanged((string)e.OldValue, (string)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                return RCServoControl.OnCoerceSerialHubPortChannel((SerialHubPortChannel)value);
            else
                return value;
        }

        private static void OnSerialHubPortChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                RCServoControl.OnSerialHubPortChannelChanged((SerialHubPortChannel)e.OldValue, (SerialHubPortChannel)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                return RCServoControl.OnCoerceSerialNumber((Int32)value);
            else
                return value;
        }

        private static void OnSerialNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                RCServoControl.OnSerialNumberChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                return RCServoControl.OnCoerceHubPort((Int32)value);
            else
                return value;
        }

        private static void OnHubPortChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                RCServoControl.OnHubPortChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                return RCServoControl.OnCoerceChannel((Int32)value);
            else
                return value;
        }

        private static void OnChannelChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl RCServoControl = o as RCServoControl;
            if (RCServoControl != null)
                RCServoControl.OnChannelChanged((Int32)e.OldValue, (Int32)e.NewValue);
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

        #region Attached

        public static readonly DependencyProperty AttachedProperty = DependencyProperty.Register(
            "Attached",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceAttached((Boolean)value);
            else
                return value;
        }

        private static void OnAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
            // NOTE(crhodes)
            // This tells the ViewModel that the control has changed
            // So the Open/Close buttons behave properly when Performance stuff
            // Opens/Closes channel
            RCServoAttached = newValue;
        }

        #endregion

        #region RCServoAttached

        public static readonly DependencyProperty RCServoAttachedProperty = DependencyProperty.Register(
            "RCServoAttached",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnRCServoAttachedChanged),
                new CoerceValueCallback(OnCoerceRCServoAttached)
                )
            );

        public Boolean RCServoAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(RCServoAttachedProperty);
            set => SetValue(RCServoAttachedProperty, value);
        }

        private static object OnCoerceRCServoAttached(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceRCServoAttached((Boolean)value);
            else
                return value;
        }

        private static void OnRCServoAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnRCServoAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceRCServoAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnRCServoAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Engaged

        public static readonly DependencyProperty EngagedProperty = DependencyProperty.Register(
            "Engaged",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnEngagedChanged),
                new CoerceValueCallback(OnCoerceEngaged)
                )
            );

        public Boolean Engaged
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(EngagedProperty);
            set => SetValue(EngagedProperty, value);
        }

        private static object OnCoerceEngaged(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceEngaged((Boolean)value);
            else
                return value;
        }

        private static void OnEngagedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnEngagedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceEngaged(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnEngagedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogPhidgetEvents

        public static readonly DependencyProperty LogPhidgetEventsProperty = DependencyProperty.Register(
            "LogPhidgetEvents",
            typeof(Boolean),
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogPhidgetEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogErrorEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogPropertyChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region LogPositionChangeEvents

        public static readonly DependencyProperty LogPositionChangeEventsProperty = DependencyProperty.Register(
            "LogPositionChangeEvents",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogPositionChangeEventsChanged),
                new CoerceValueCallback(OnCoerceLogPositionChangeEvents)
                )
            );

        public Boolean LogPositionChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogPositionChangeEventsProperty);
            set => SetValue(LogPositionChangeEventsProperty, value);
        }

        private static object OnCoerceLogPositionChangeEvents(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogPositionChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPositionChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogPositionChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogPositionChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogPositionChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogVelocityChangeEvents

        public static readonly DependencyProperty LogVelocityChangeEventsProperty = DependencyProperty.Register(
            "LogVelocityChangeEvents",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogVelocityChangeEventsChanged),
                new CoerceValueCallback(OnCoerceLogVelocityChangeEvents)
                )
            );

        public Boolean LogVelocityChangeEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogVelocityChangeEventsProperty);
            set => SetValue(LogVelocityChangeEventsProperty, value);
        }

        private static object OnCoerceLogVelocityChangeEvents(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogVelocityChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogVelocityChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogVelocityChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogVelocityChangeEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogVelocityChangeEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogTargetPositionReachedEvents

        public static readonly DependencyProperty LogTargetPositionReachedEventsProperty = DependencyProperty.Register(
            "LogTargetPositionReachedEvents",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogTargetPositionReachedEventsChanged),
                new CoerceValueCallback(OnCoerceLogTargetPositionReachedEvents)
                )
            );

        public Boolean LogTargetPositionReachedEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogTargetPositionReachedEventsProperty);
            set => SetValue(LogTargetPositionReachedEventsProperty, value);
        }

        private static object OnCoerceLogTargetPositionReachedEvents(DependencyObject o, object value)
        {
            
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceLogTargetPositionReachedEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogTargetPositionReachedEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnLogTargetPositionReachedEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogTargetPositionReachedEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogTargetPositionReachedEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region ServoType

        public static readonly DependencyProperty ServoTypeProperty = DependencyProperty.Register(
            "RCServoType", 
            typeof(RCServoType), 
            typeof(RCServoControl), 
            new FrameworkPropertyMetadata(
                RCServoType.DEFAULT, 
                new PropertyChangedCallback(OnServoTypeChanged), 
                new CoerceValueCallback(OnCoerceServoType)
                )
            );

        public RCServoType ServoType
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (RCServoType)GetValue(ServoTypeProperty);
            set => SetValue(ServoTypeProperty, value);
        }

        private static object OnCoerceServoType(DependencyObject o, object value)
        {
            RCServoControl rCServoControl = o as RCServoControl;
            if (rCServoControl != null)
                return rCServoControl.OnCoerceServoType((RCServoType)value);
            else
                return value;
        }

        private static void OnServoTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rCServoControl = o as RCServoControl;
            if (rCServoControl != null)
                rCServoControl.OnServoTypeChanged((RCServoType)e.OldValue, (RCServoType)e.NewValue);
        }

        protected virtual RCServoType OnCoerceServoType(RCServoType value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnServoTypeChanged(RCServoType oldValue, RCServoType newValue)
        {
            MinPulseWidth = VNC.Phidget22.PhidgetDeviceLibrary.RCServoTypes[newValue].MinPulseWidth;
            MaxPulseWidth = VNC.Phidget22.PhidgetDeviceLibrary.RCServoTypes[newValue].MaxPulseWidth;
            // TODO: Add your property changed side-effects. Descendants can override as well.
            //RCServoEx.RCServoConfiguration servoConfiguration = RCServoEx.RCServoTypes[ServoType];
            //MinPulseWidth = servoConfiguration.MinPulseWidth;
            //MaxPulseWidth = servoConfiguration.MaxPulseWidth;
        }

        #endregion

        #region ServoNumber

        public static readonly DependencyProperty ServoNumberProperty = DependencyProperty.Register(
            "ServoNumber",
            typeof(string),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                "0",
                new PropertyChangedCallback(OnServoNumberChanged),
                new CoerceValueCallback(OnCoerceServoNumber)
                )
            );

        public string ServoNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(ServoNumberProperty);
            set => SetValue(ServoNumberProperty, value);
        }

        private static object OnCoerceServoNumber(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceServoNumber((string)value);
            else
                return value;
        }

        private static void OnServoNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnServoNumberChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceServoNumber(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnServoNumberChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Current

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(
            "Current",
            typeof(Double?),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata
            (null,
                new PropertyChangedCallback(OnCurrentChanged),
                new CoerceValueCallback(OnCoerceCurrent)));

        public Double? Current
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }
        private static object OnCoerceCurrent(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceCurrent(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnCurrentChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }


        #endregion

        #region MinDataInterval

        public static readonly DependencyProperty MinDataIntervalProperty = DependencyProperty.Register(
            "MinDataInterval",
            typeof(Int32),
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMinDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMaxDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinDataRate((Double)value);
            else
                return value;
        }

        private static void OnMinDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceDataRate((Double)value);
            else
                return value;
        }

        private static void OnDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(RCServoControl),
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
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxDataRate((Double)value);
            else
                return value;
        }

        private static void OnMaxDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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

        #region MinAcceleration

        public static readonly DependencyProperty MinAccelerationProperty = DependencyProperty.Register(
            "MinAcceleration",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinAccelerationChanged),
                new CoerceValueCallback(OnCoerceMinAcceleration)
                )
            );

        public Double MinAcceleration
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinAccelerationProperty);
            set => SetValue(MinAccelerationProperty, value);
        }
        private static object OnCoerceMinAcceleration(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinAcceleration((Double)value);
            else
                return value;
        }

        private static void OnMinAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinAcceleration(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinAccelerationChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Acceleration

        public static readonly DependencyProperty AccelerationProperty = DependencyProperty.Register(
            "Acceleration",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnAccelerationChanged),
                new CoerceValueCallback(OnCoerceAcceleration)
                )
            );

        public Double Acceleration
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(AccelerationProperty);
            set => SetValue(AccelerationProperty, value);
        }
        private static object OnCoerceAcceleration(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceAcceleration((Double)value);
            else
                return value;
        }

        private static void OnAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceAcceleration(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnAccelerationChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxAcceleration

        public static readonly DependencyProperty MaxAccelerationProperty = DependencyProperty.Register(
            "MaxAcceleration",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxAccelerationChanged),
                new CoerceValueCallback(OnCoerceMaxAcceleration)
                )
            );

        public Double MaxAcceleration
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxAccelerationProperty);
            set => SetValue(MaxAccelerationProperty, value);
        }
        private static object OnCoerceMaxAcceleration(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxAcceleration((Double)value);
            else
                return value;
        }

        private static void OnMaxAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxAcceleration(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxAccelerationChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinFailsafeTIme

        public static readonly DependencyProperty MinFailsafeTImeProperty = DependencyProperty.Register(
            "MinFailsafeTIme",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinFailsafeTImeChanged),
                new CoerceValueCallback(OnCoerceMinFailsafeTIme)
                )
            );

        public Double MinFailsafeTIme
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinFailsafeTImeProperty);
            set => SetValue(MinFailsafeTImeProperty, value);
        }
        private static object OnCoerceMinFailsafeTIme(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinFailsafeTIme((Double)value);
            else
                return value;
        }

        private static void OnMinFailsafeTImeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinFailsafeTImeChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinFailsafeTIme(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinFailsafeTImeChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxFailsafeTIme

        public static readonly DependencyProperty MaxFailsafeTImeProperty = DependencyProperty.Register(
            "MaxFailsafeTIme",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxFailsafeTImeChanged),
                new CoerceValueCallback(OnCoerceMaxFailsafeTIme)
                )
            );

        public Double MaxFailsafeTIme
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxFailsafeTImeProperty);
            set => SetValue(MaxFailsafeTImeProperty, value);
        }
        private static object OnCoerceMaxFailsafeTIme(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxFailsafeTIme((Double)value);
            else
                return value;
        }

        private static void OnMaxFailsafeTImeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxFailsafeTImeChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxFailsafeTIme(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxFailsafeTImeChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region IsMoving

        public static readonly DependencyProperty IsMovingProperty = DependencyProperty.Register(
            "IsMoving",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnIsMovingChanged),
                new CoerceValueCallback(OnCoerceIsMoving)
                )
            );

        public Boolean IsMoving
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(IsMovingProperty);
            set => SetValue(IsMovingProperty, value);
        }

        private static object OnCoerceIsMoving(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceIsMoving((Boolean)value);
            else
                return value;
        }

        private static void OnIsMovingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnIsMovingChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceIsMoving(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnIsMovingChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPositionServo

        public static readonly DependencyProperty MinPositionServoProperty = DependencyProperty.Register(
            "MinPositionServo",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPositionServoChanged),
                new CoerceValueCallback(OnCoerceMinPositionServo)
                )
            );

        public Double MinPositionServo
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPositionServoProperty);
            set => SetValue(MinPositionServoProperty, value);
        }
        private static object OnCoerceMinPositionServo(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinPositionServo((Double)value);
            else
                return value;
        }

        private static void OnMinPositionServoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinPositionServoChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPositionServo(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionServoChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPosition

        public static readonly DependencyProperty MinPositionProperty = DependencyProperty.Register(
            "MinPosition",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPositionChanged),
                new CoerceValueCallback(OnCoerceMinPosition)
                )
            );

        public Double MinPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPositionProperty);
            set => SetValue(MinPositionProperty, value);
        }
        private static object OnCoerceMinPosition(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinPosition((Double)value);
            else
                return value;
        }

        private static void OnMinPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinPositionChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPosition(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPositionStop

        public static readonly DependencyProperty MinPositionStopProperty = DependencyProperty.Register(
            "MinPositionStop",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPositionStopChanged),
                new CoerceValueCallback(OnCoerceMinPositionStop)
                )
            );

        public Double MinPositionStop
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPositionStopProperty);
            set => SetValue(MinPositionStopProperty, value);
        }
        private static object OnCoerceMinPositionStop(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinPositionStop((Double)value);
            else
                return value;
        }

        private static void OnMinPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinPositionStopChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPositionStop(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionStopChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Position

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnPositionChanged),
                new CoerceValueCallback(OnCoercePosition)
                )
            );

        public Double Position
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
        private static object OnCoercePosition(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoercePosition((Double)value);
            else
                return value;
        }

        private static void OnPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnPositionChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoercePosition(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPositionChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPositionStop

        public static readonly DependencyProperty MaxPositionStopProperty = DependencyProperty.Register(
            "MaxPositionStop",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPositionStopChanged),
                new CoerceValueCallback(OnCoerceMaxPositionStop)
                )
            );

        public Double MaxPositionStop
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPositionStopProperty);
            set => SetValue(MaxPositionStopProperty, value);
        }
        private static object OnCoerceMaxPositionStop(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxPositionStop((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxPositionStopChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPositionStop(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionStopChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPosition

        public static readonly DependencyProperty MaxPositionProperty = DependencyProperty.Register(
            "MaxPosition",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPositionChanged),
                new CoerceValueCallback(OnCoerceMaxPosition)
                )
            );

        public Double MaxPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPositionProperty);
            set => SetValue(MaxPositionProperty, value);
        }
        private static object OnCoerceMaxPosition(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxPosition((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxPositionChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPosition(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPositionServo

        public static readonly DependencyProperty MaxPositionServoProperty = DependencyProperty.Register(
            "MaxPositionServo",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPositionServoChanged),
                new CoerceValueCallback(OnCoerceMaxPositionServo)
                )
            );

        public Double MaxPositionServo
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPositionServoProperty);
            set => SetValue(MaxPositionServoProperty, value);
        }
        private static object OnCoerceMaxPositionServo(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxPositionServo((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionServoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxPositionServoChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPositionServo(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionServoChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPulseWidth

        public static readonly DependencyProperty MinPulseWidthProperty = DependencyProperty.Register(
            "MinPulseWidth",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPulseWidthChanged),
                new CoerceValueCallback(OnCoerceMinPulseWidth)
                )
            );

        public Double MinPulseWidth
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPulseWidthProperty);
            set => SetValue(MinPulseWidthProperty, value);
        }
        private static object OnCoerceMinPulseWidth(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinPulseWidth((Double)value);
            else
                return value;
        }

        private static void OnMinPulseWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinPulseWidthChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPulseWidth(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPulseWidthChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPulseWidth

        public static readonly DependencyProperty MaxPulseWidthProperty = DependencyProperty.Register(
            "MaxPulseWidth",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPulseWidthChanged),
                new CoerceValueCallback(OnCoerceMaxPulseWidth)
                )
            );

        public Double MaxPulseWidth
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPulseWidthProperty);
            set => SetValue(MaxPulseWidthProperty, value);
        }
        private static object OnCoerceMaxPulseWidth(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxPulseWidth((Double)value);
            else
                return value;
        }

        private static void OnMaxPulseWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxPulseWidthChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPulseWidth(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPulseWidthChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPulseWidthLimit

        public static readonly DependencyProperty MinPulseWidthLimitProperty = DependencyProperty.Register(
            "MinPulseWidthLimit",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPulseWidthLimitChanged),
                new CoerceValueCallback(OnCoerceMinPulseWidthLimit)
                )
            );

        public Double MinPulseWidthLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPulseWidthLimitProperty);
            set => SetValue(MinPulseWidthLimitProperty, value);
        }
        private static object OnCoerceMinPulseWidthLimit(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinPulseWidthLimit((Double)value);
            else
                return value;
        }

        private static void OnMinPulseWidthLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinPulseWidthLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPulseWidthLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPulseWidthLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPulseWidthLimit

        public static readonly DependencyProperty MaxPulseWidthLimitProperty = DependencyProperty.Register(
            "MaxPulseWidthLimit",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPulseWidthLimitChanged),
                new CoerceValueCallback(OnCoerceMaxPulseWidthLimit)
                )
            );

        public Double MaxPulseWidthLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPulseWidthLimitProperty);
            set => SetValue(MaxPulseWidthLimitProperty, value);
        }
        private static object OnCoerceMaxPulseWidthLimit(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxPulseWidthLimit((Double)value);
            else
                return value;
        }

        private static void OnMaxPulseWidthLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxPulseWidthLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPulseWidthLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPulseWidthLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region SpeedRampingState

        public static readonly DependencyProperty SpeedRampingStateProperty = DependencyProperty.Register(
            "SpeedRampingState",
            typeof(Boolean),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnSpeedRampingStateChanged),
                new CoerceValueCallback(OnCoerceSpeedRampingState)
                )
            );

        public Boolean SpeedRampingState
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(SpeedRampingStateProperty);
            set => SetValue(SpeedRampingStateProperty, value);
        }

        private static object OnCoerceSpeedRampingState(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceSpeedRampingState((Boolean)value);
            else
                return value;
        }

        private static void OnSpeedRampingStateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnSpeedRampingStateChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceSpeedRampingState(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSpeedRampingStateChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region TargetPosition

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register(
            "TargetPosition",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnTargetPositionChanged),
                new CoerceValueCallback(OnCoerceTargetPosition)
                )
            );

        public Double TargetPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(TargetPositionProperty);
            set => SetValue(TargetPositionProperty, value);
        }
        private static object OnCoerceTargetPosition(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceTargetPosition((Double)value);
            else
                return value;
        }

        private static void OnTargetPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnTargetPositionChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceTargetPosition(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnTargetPositionChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinTorque

        public static readonly DependencyProperty MinTorqueProperty = DependencyProperty.Register(
            "MinTorque",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinTorqueChanged),
                new CoerceValueCallback(OnCoerceMinTorque)
                )
            );

        public Double MinTorque
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinTorqueProperty);
            set => SetValue(MinTorqueProperty, value);
        }
        private static object OnCoerceMinTorque(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinTorque((Double)value);
            else
                return value;
        }

        private static void OnMinTorqueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinTorqueChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinTorque(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinTorqueChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Torque

        public static readonly DependencyProperty TorqueProperty = DependencyProperty.Register(
            "Torque",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnTorqueChanged),
                new CoerceValueCallback(OnCoerceTorque)
                )
            );

        public Double Torque
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(TorqueProperty);
            set => SetValue(TorqueProperty, value);
        }
        private static object OnCoerceTorque(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceTorque((Double)value);
            else
                return value;
        }

        private static void OnTorqueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnTorqueChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceTorque(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnTorqueChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxTorque

        public static readonly DependencyProperty MaxTorqueProperty = DependencyProperty.Register(
            "MaxTorque",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxTorqueChanged),
                new CoerceValueCallback(OnCoerceMaxTorque)
                )
            );

        public Double MaxTorque
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxTorqueProperty);
            set => SetValue(MaxTorqueProperty, value);
        }
        private static object OnCoerceMaxTorque(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxTorque((Double)value);
            else
                return value;
        }

        private static void OnMaxTorqueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxTorqueChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxTorque(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxTorqueChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Velocity

        public static readonly DependencyProperty VelocityProperty = DependencyProperty.Register(
            "Velocity",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnVelocityChanged),
                new CoerceValueCallback(OnCoerceVelocity)
                )
            );

        public Double Velocity
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VelocityProperty);
            set => SetValue(VelocityProperty, value);
        }
        private static object OnCoerceVelocity(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceVelocity((Double)value);
            else
                return value;
        }

        private static void OnVelocityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnVelocityChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVelocity(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVelocityChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinVelocityLimit

        public static readonly DependencyProperty MinVelocityLimitProperty = DependencyProperty.Register(
            "MinVelocityLimit",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinVelocityLimitChanged),
                new CoerceValueCallback(OnCoerceMinVelocityLimit)
                )
            );

        public Double MinVelocityLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinVelocityLimitProperty);
            set => SetValue(MinVelocityLimitProperty, value);
        }
        private static object OnCoerceMinVelocityLimit(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMinVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnMinVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMinVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinVelocityLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinVelocityLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region VelocityLimit

        public static readonly DependencyProperty VelocityLimitProperty = DependencyProperty.Register(
            "VelocityLimit",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnVelocityLimitChanged),
                new CoerceValueCallback(OnCoerceVelocityLimit)
                )
            );

        public Double VelocityLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(VelocityLimitProperty);
            set => SetValue(VelocityLimitProperty, value);
        }
        private static object OnCoerceVelocityLimit(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceVelocityLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVelocityLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxVelocityLimit

        public static readonly DependencyProperty MaxVelocityLimitProperty = DependencyProperty.Register(
            "MaxVelocityLimit",
            typeof(Double),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxVelocityLimitChanged),
                new CoerceValueCallback(OnCoerceMaxVelocityLimit)
                )
            );

        public Double MaxVelocityLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxVelocityLimitProperty);
            set => SetValue(MaxVelocityLimitProperty, value);
        }
        private static object OnCoerceMaxVelocityLimit(DependencyObject o, object value)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                return rcServoControl.OnCoerceMaxVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnMaxVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl rcServoControl = o as RCServoControl;
            if (rcServoControl != null)
                rcServoControl.OnMaxVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxVelocityLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxVelocityLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region PositionScaleRange

        public static readonly DependencyProperty PositionScaleRangeProperty = DependencyProperty.Register(
            "PositionScaleRange",
            typeof(Int32),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                10,
                new PropertyChangedCallback(OnPositionScaleRangeChanged),
                new CoerceValueCallback(OnCoercePositionScaleRange)
                )
            );

        public Int32 PositionScaleRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(PositionScaleRangeProperty);
            set => SetValue(PositionScaleRangeProperty, value);
        }

        private static object OnCoercePositionScaleRange(DependencyObject o, object value)
        {
            RCServoControl positionControl = o as RCServoControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionScaleRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionScaleRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl positionControl = o as RCServoControl;
            if (positionControl != null)
                positionControl.OnPositionScaleRangeChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoercePositionScaleRange(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPositionScaleRangeChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region PositionStopRange

        public static readonly DependencyProperty PositionStopRangeProperty = DependencyProperty.Register(
            "PositionStopRange",
            typeof(Int32),
            typeof(RCServoControl),
            new FrameworkPropertyMetadata(
                10,
                new PropertyChangedCallback(OnPositionStopRangeChanged),
                new CoerceValueCallback(OnCoercePositionStopRange)
                )
            );

        public Int32 PositionStopRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(PositionStopRangeProperty);
            set => SetValue(PositionStopRangeProperty, value);
        }

        private static object OnCoercePositionStopRange(DependencyObject o, object value)
        {
            RCServoControl positionControl = o as RCServoControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionStopRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionStopRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoControl positionControl = o as RCServoControl;
            if (positionControl != null)
                positionControl.OnPositionStopRangeChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoercePositionStopRange(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPositionStopRangeChanged(Int32 oldValue, Int32 newValue)
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

        public void UpdateProperties(RCServoEx servo)
        {
            Attached = servo.Attached;
            Engaged = servo.Engaged;
        }

        #endregion

        #region Protected Methods (none)


        #endregion

        #region Private Methods (none)


        #endregion

        #region IInstanceCount

        private static Int32 _instanceCountV;

        public Int32 InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        private static Int32 _instanceCountVP;

        public Int32 InstanceCountVP
        {
            get => _instanceCountVP;
            set => _instanceCountVP = value;
        }

        #endregion
    }
}
