using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

using VNC;
using VNC.Core.Mvvm;

using Phidgets = Phidget22;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class StepperPositionControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public StepperPositionControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IPositionControlViewModel)DataContext;

            // Can create directly
            // ViewModel = PositionControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public PositionControl(IPositionControlViewModel viewModel)
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

            //var coo = ((FrameworkElement)lgPosition.DataContext).DataContext;

            //btnZeroPosition.DataContext = ((FrameworkElement)lgPosition.DataContext).DataContext;

            //var foo = VNC.Core.Xaml.PhysicalTree.FindAncestor<StepperControl>(this.btnZeroPosition);

            //var bar = VNC.Core.Xaml.LogicalTree.FindAncestor<StepperControl>(this.btnZeroPosition);

            //var jar = VNC.Core.Xaml.PhysicalTree.FindVisualParent<StepperControl>(this.btnZeroPosition);

            lgPosition.DataContext = this;

            spDeveloperInfo.DataContext = this;

            var dc = btnZeroPosition1.DataContext;

            //var xx = this == thisControl;

            //var pp = thisControl.Parent;

            //var parent = this.Parent;


            //var thisDC = this.DataContext;

            //var parentDC = ((System.Windows.Controls.UserControl)thisDC).DataContext;

            //var grandParentDC = ((System.Windows.Controls.UserControl)parentDC).DataContext;

            //var stepperControl = this.Parent as System.Windows.Controls.UserControl;
            //var pdc = stepperControl.DataContext;

            //btnZeroPosition.DataContext = pdc;
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceControlTitle((string)value);
            else
                return value;
        }

        private static void OnControlTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnControlTitleChanged((string)e.OldValue, (string)e.NewValue);
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

        #region StepperNumber

        public static readonly DependencyProperty StepperNumberProperty = DependencyProperty.Register(
            "StepperNumber",
            typeof(string),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                "0",
                new PropertyChangedCallback(OnStepperNumberChanged),
                new CoerceValueCallback(OnCoerceStepperNumber)
                )
            );

        public string StepperNumber
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(StepperNumberProperty);
            set => SetValue(StepperNumberProperty, value);
        }

        private static object OnCoerceStepperNumber(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceStepperNumber((string)value);
            else
                return value;
        }

        private static void OnStepperNumberChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnStepperNumberChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual string OnCoerceStepperNumber(string value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStepperNumberChanged(string oldValue, string newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region LogPhidgetEvents

        public static readonly DependencyProperty LogPhidgetEventsProperty = DependencyProperty.Register(
            "LogPhidgetEvents",
            typeof(Boolean),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogPhidgetEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPhidgetEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogPhidgetEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogErrorEventsChanged),
                new CoerceValueCallback(OnCoerceLogErrorEvents)
                )
            );

        public Boolean LogErrorEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogErrorEventsProperty);
            set => SetValue(LogErrorEventsProperty, value);
        }

        private static object OnCoerceLogErrorEvents(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogErrorEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogErrorEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogErrorEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogPropertyChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPropertyChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogPropertyChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogPositionChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogPositionChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogPositionChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogVelocityChangeEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogVelocityChangeEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogVelocityChangeEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region LogStoppedEvents

        public static readonly DependencyProperty LogStoppedEventsProperty = DependencyProperty.Register(
            "LogStoppedEvents",
            typeof(Boolean),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnLogStoppedEventsChanged),
                new CoerceValueCallback(OnCoerceLogStoppedEvents)
                )
            );

        public Boolean LogStoppedEvents
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(LogStoppedEventsProperty);
            set => SetValue(LogStoppedEventsProperty, value);
        }

        private static object OnCoerceLogStoppedEvents(DependencyObject o, object value)
        {

            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceLogStoppedEvents((Boolean)value);
            else
                return value;
        }

        private static void OnLogStoppedEventsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnLogStoppedEventsChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceLogStoppedEvents(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnLogStoppedEventsChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Attached

        public static readonly DependencyProperty AttachedProperty = DependencyProperty.Register(
            "Attached",
            typeof(Boolean),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceAttached((Boolean)value);
            else
                return value;
        }

        private static void OnAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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
            StepperAttached = newValue;
        }

        #endregion

        #region StepperAttached

        public static readonly DependencyProperty StepperAttachedProperty = DependencyProperty.Register(
            "StepperAttached",
            typeof(Boolean),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnStepperAttachedChanged),
                new CoerceValueCallback(OnCoerceStepperAttached)
                )
            );

        public Boolean StepperAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean)GetValue(StepperAttachedProperty);
            set => SetValue(StepperAttachedProperty, value);
        }

        private static object OnCoerceStepperAttached(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceStepperAttached((Boolean)value);
            else
                return value;
        }

        private static void OnStepperAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnStepperAttachedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual Boolean OnCoerceStepperAttached(Boolean value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStepperAttachedChanged(Boolean oldValue, Boolean newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Engaged

        public static readonly DependencyProperty EngagedProperty = DependencyProperty.Register(
            "Engaged",
            typeof(Boolean),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceEngaged((Boolean)value);
            else
                return value;
        }

        private static void OnEngagedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnEngagedChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region MinCurrentLimit

        public static readonly DependencyProperty MinCurrentLimitProperty = DependencyProperty.Register(
            "MinCurrentLimit",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinCurrentLimitChanged),
                new CoerceValueCallback(OnCoerceMinCurrentLimit)
                )
            );

        public Double MinCurrentLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinCurrentLimitProperty);
            set => SetValue(MinCurrentLimitProperty, value);
        }
        private static object OnCoerceMinCurrentLimit(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinCurrentLimit((Double)value);
            else
                return value;
        }

        private static void OnMinCurrentLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinCurrentLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinCurrentLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinCurrentLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region CurrentLimit

        public static readonly DependencyProperty CurrentLimitProperty = DependencyProperty.Register(
            "CurrentLimit",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnCurrentLimitChanged),
                new CoerceValueCallback(OnCoerceCurrentLimit)
                )
            );

        public Double CurrentLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(CurrentLimitProperty);
            set => SetValue(CurrentLimitProperty, value);
        }
        private static object OnCoerceCurrentLimit(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceCurrentLimit((Double)value);
            else
                return value;
        }

        private static void OnCurrentLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnCurrentLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceCurrentLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnCurrentLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region HoldingCurrentLimit

        public static readonly DependencyProperty HoldingCurrentLimitProperty = DependencyProperty.Register(
            "HoldingCurrentLimit",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnHoldingCurrentLimitChanged),
                new CoerceValueCallback(OnCoerceHoldingCurrentLimit)
                )
            );

        public Double HoldingCurrentLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(HoldingCurrentLimitProperty);
            set => SetValue(HoldingCurrentLimitProperty, value);
        }
        private static object OnCoerceHoldingCurrentLimit(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceHoldingCurrentLimit((Double)value);
            else
                return value;
        }

        private static void OnHoldingCurrentLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnHoldingCurrentLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceHoldingCurrentLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnHoldingCurrentLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxCurrentLimit

        public static readonly DependencyProperty MaxCurrentLimitProperty = DependencyProperty.Register(
            "MaxCurrentLimit",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxCurrentLimitChanged),
                new CoerceValueCallback(OnCoerceMaxCurrentLimit)
                )
            );

        public Double MaxCurrentLimit
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxCurrentLimitProperty);
            set => SetValue(MaxCurrentLimitProperty, value);
        }
        private static object OnCoerceMaxCurrentLimit(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxCurrentLimit((Double)value);
            else
                return value;
        }

        private static void OnMaxCurrentLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxCurrentLimitChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxCurrentLimit(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxCurrentLimitChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinDataInterval

        public static readonly DependencyProperty MinDataIntervalProperty = DependencyProperty.Register(
            "MinDataInterval",
            typeof(Int32),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMinDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxDataInterval((Int32)value);
            else
                return value;
        }

        private static void OnMaxDataIntervalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxDataIntervalChanged((Int32)e.OldValue, (Int32)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinDataRate((Double)value);
            else
                return value;
        }

        private static void OnMinDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceDataRate((Double)value);
            else
                return value;
        }

        private static void OnDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxDataRate((Double)value);
            else
                return value;
        }

        private static void OnMaxDataRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxDataRateChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
        private static object OnCoerceMinAcceleration (DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinAcceleration ((Double)value);
            else
                return value;
        }

        private static void OnMinAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinAcceleration (Double value)
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
            typeof(StepperPositionControl),
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
        private static object OnCoerceAcceleration (DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceAcceleration ((Double)value);
            else
                return value;
        }

        private static void OnAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceAcceleration (Double value)
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
            typeof(StepperPositionControl),
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
        private static object OnCoerceMaxAcceleration (DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxAcceleration ((Double)value);
            else
                return value;
        }

        private static void OnMaxAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxAccelerationChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxAcceleration (Double value)
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinFailsafeTIme((Double)value);
            else
                return value;
        }

        private static void OnMinFailsafeTImeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinFailsafeTImeChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxFailsafeTIme((Double)value);
            else
                return value;
        }

        private static void OnMaxFailsafeTImeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxFailsafeTImeChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceIsMoving((Boolean)value);
            else
                return value;
        }

        private static void OnIsMovingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnIsMovingChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
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

        #region MinPositionStepper

        public static readonly DependencyProperty MinPositionStepperProperty = DependencyProperty.Register(
            "MinPositionStepper",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPositionStepperChanged),
                new CoerceValueCallback(OnCoerceMinPositionStepper)
                )
            );

        public Double MinPositionStepper
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MinPositionStepperProperty);
            set => SetValue(MinPositionStepperProperty, value);
        }
        private static object OnCoerceMinPositionStepper(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinPositionStepper((Double)value);
            else
                return value;
        }

        private static void OnMinPositionStepperChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinPositionStepperChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMinPositionStepper(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionStepperChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPosition

        public static readonly DependencyProperty MinPositionProperty = DependencyProperty.Register(
            "MinPosition",
            typeof(Double),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinPosition((Double)value);
            else
                return value;
        }

        private static void OnMinPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinPositionChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinPositionStop((Double)value);
            else
                return value;
        }

        private static void OnMinPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinPositionStopChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoercePosition((Double)value);
            else
                return value;
        }

        private static void OnPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnPositionChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxPositionStop((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxPositionStopChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxPosition((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxPositionChanged((Double)e.OldValue, (Double)e.NewValue);
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

        #region MaxPositionStepper

        public static readonly DependencyProperty MaxPositionStepperProperty = DependencyProperty.Register(
            "MaxPositionStepper",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPositionStepperChanged),
                new CoerceValueCallback(OnCoerceMaxPositionStepper)
                )
            );

        public Double MaxPositionStepper
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(MaxPositionStepperProperty);
            set => SetValue(MaxPositionStepperProperty, value);
        }
        private static object OnCoerceMaxPositionStepper(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxPositionStepper((Double)value);
            else
                return value;
        }

        private static void OnMaxPositionStepperChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxPositionStepperChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceMaxPositionStepper(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionStepperChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region TargetPosition

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register(
            "TargetPosition",
            typeof(Double),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceTargetPosition((Double)value);
            else
                return value;
        }

        private static void OnTargetPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnTargetPositionChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinTorque((Double)value);
            else
                return value;
        }

        private static void OnMinTorqueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinTorqueChanged((Double)e.OldValue, (Double)e.NewValue);
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

        #region Velocity

        public static readonly DependencyProperty VelocityProperty = DependencyProperty.Register(
            "Velocity",
            typeof(Double),
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceVelocity((Double)value);
            else
                return value;
        }

        private static void OnVelocityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnVelocityChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMinVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnMinVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMinVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
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
            typeof(StepperPositionControl),
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
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceMaxVelocityLimit((Double)value);
            else
                return value;
        }

        private static void OnMaxVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnMaxVelocityLimitChanged((Double)e.OldValue, (Double)e.NewValue);
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

        #region RescaleFactor

        public static readonly DependencyProperty RescaleFactorProperty = DependencyProperty.Register(
            "RescaleFactor",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnRescaleFactorChanged),
                new CoerceValueCallback(OnCoerceRescaleFactor)
                )
            );

        public Double RescaleFactor
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(RescaleFactorProperty);
            set => SetValue(RescaleFactorProperty, value);
        }

        private static object OnCoerceRescaleFactor(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceRescaleFactor((Double)value);
            else
                return value;
        }

        private static void OnRescaleFactorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnRescaleFactorChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceRescaleFactor(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnRescaleFactorChanged(Double oldValue, Double newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region ControlMode

        public static readonly DependencyProperty ControlModeProperty = DependencyProperty.Register(
            "ControlMode",
            typeof(Phidgets.StepperControlMode),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                Phidgets.StepperControlMode.Step,
                new PropertyChangedCallback(OnControlModeChanged),
                new CoerceValueCallback(OnCoerceControlMode)
                )
            );

        public Phidgets.StepperControlMode ControlMode
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Phidgets.StepperControlMode)GetValue(ControlModeProperty);
            set => SetValue(ControlModeProperty, value);
        }

        private static object OnCoerceControlMode(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceControlMode((Phidgets.StepperControlMode)value);
            else
                return value;
        }

        private static void OnControlModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnControlModeChanged((Phidgets.StepperControlMode)e.OldValue, (Phidgets.StepperControlMode)e.NewValue);
        }

        protected virtual Phidgets.StepperControlMode OnCoerceControlMode(Phidgets.StepperControlMode value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnControlModeChanged(Phidgets.StepperControlMode oldValue, Phidgets.StepperControlMode newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region PositionScaleRange

        public static readonly DependencyProperty PositionScaleRangeProperty = DependencyProperty.Register(
            "PositionScaleRange",
            typeof(Int32),
            typeof(StepperPositionControl),
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
            StepperPositionControl positionControl = (StepperPositionControl)o;
            if (positionControl != null)
                return positionControl.OnCoercePositionScaleRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionScaleRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = (StepperPositionControl)o;
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
            typeof(StepperPositionControl),
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
            StepperPositionControl positionControl = (StepperPositionControl)o;
            if (positionControl != null)
                return positionControl.OnCoercePositionStopRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionStopRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = (StepperPositionControl)o;
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

        #region StepAngle

        public static readonly DependencyProperty StepAngleProperty = DependencyProperty.Register(
            "StepAngle",
            typeof(Double),
            typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnStepAngleChanged),
                new CoerceValueCallback(OnCoerceStepAngle)
                )
            );

        public Double StepAngle
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double)GetValue(StepAngleProperty);
            set => SetValue(StepAngleProperty, value);
        }
        private static object OnCoerceStepAngle(DependencyObject o, object value)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                return stepperControl.OnCoerceStepAngle((Double)value);
            else
                return value;
        }

        private static void OnStepAngleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl stepperControl = (StepperPositionControl)o;
            if (stepperControl != null)
                stepperControl.OnStepAngleChanged((Double)e.OldValue, (Double)e.NewValue);
        }

        protected virtual Double OnCoerceStepAngle(Double value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStepAngleChanged(Double oldValue, Double newValue)
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

        private void PositionIncrement_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton radioButton = (System.Windows.Controls.RadioButton)sender;

            seTargetPosition.Increment = Int32.Parse(radioButton.Content.ToString());
        }

        private void SetTargetPosition(object sender, RoutedEventArgs e)
        {
            var rea = e;
            var s = sender;

            var direction = ((System.Windows.Controls.Button)sender).CommandParameter;

            var stepAngle = Double.Parse(cbeStepAngle.Text);

            Double circle = 360;
            var circleSteps = circle / stepAngle;

            var degrees = Double.Parse(teDegrees.Text);

            var stepsToMove = (degrees / stepAngle) * 16; // 1/16 steps

            //stepsToMove = stepsToMove * 16; // 1/16 steps

            //switch (direction)
            //{
            //    case "CW":
            //        StepperProperties[0].TargetPosition += stepsToMove;
            //        break;

            //    case "CCW":
            //        StepperProperties[0].TargetPosition -= stepsToMove;
            //        break;

            //    default:
            //        Log.ERROR($"Unexpected direction:>{direction}", Common.LOG_CATEGORY);
            //        break;
            //}

            switch (direction)
            {
                case "CW":
                    TargetPosition += stepsToMove;
                    break;

                case "CCW":
                    TargetPosition -= stepsToMove;
                    break;

                default:
                    Log.ERROR($"Unknown goToWhere >{direction}<", Common.LOG_CATEGORY);
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

        #region IInstanceCountV

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

