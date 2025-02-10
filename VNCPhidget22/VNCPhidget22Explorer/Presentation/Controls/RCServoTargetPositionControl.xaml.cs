using System;
using System.Windows;

using VNCPhidget22Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.XtraSpellChecker.Parser;
using System.Threading;
using System.Linq;

namespace VNCPhidget22Explorer.Presentation.Controls
{
    public partial class RCServoTargetPositionControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public RCServoTargetPositionControl()
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

            // Establish any additional DataContext(s), e.g. to things held in this View

            lgMain.DataContext = this;

            // // TODO(crhodes)
            // Do we need to set this separately?

            liPositionScale.DataContext = this;

            var rb1IsChecked = rb1.IsChecked;
            var rb5IsChecked = rb5.IsChecked;
            var rb10IsChecked = rb10.IsChecked;
            var rb50IsChecked = rb50.IsChecked;

            rb1.IsChecked = true;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        #region PositionScaleRange

        public static readonly DependencyProperty PositionScaleRangeProperty = DependencyProperty.Register(
            "PositionScaleRange", 
            typeof(Int32), 
            typeof(RCServoTargetPositionControl),
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
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionScaleRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionScaleRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
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
            typeof(RCServoTargetPositionControl),
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
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionStopRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionStopRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
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

        #region ServoIndex

        public static readonly DependencyProperty ServoIndexProperty = DependencyProperty.Register(
            "ServoIndex", 
            typeof(Int32?), 
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0, 
                new PropertyChangedCallback(OnServoIndexChanged), 
                new CoerceValueCallback(OnCoerceServoIndex)
                )
            );

        public Int32? ServoIndex
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(ServoIndexProperty);
            set => SetValue(ServoIndexProperty, value);
        }

        private static object OnCoerceServoIndex(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceServoIndex((Int32?)value);
            else
                return value;
        }

        private static void OnServoIndexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnServoIndexChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }

        protected virtual Int32? OnCoerceServoIndex(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnServoIndexChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPositionServo

        public static readonly DependencyProperty MinPositionServoProperty = DependencyProperty.Register(
            "MinPositionServo", 
            typeof(Double?), 
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinPositionServoChanged), 
                new CoerceValueCallback(OnCoerceMinPositionServo)));

        public Double? MinPositionServo
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MinPositionServoProperty);
            set => SetValue(MinPositionServoProperty, value);
        }

        private static object OnCoerceMinPositionServo(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMinPositionServo((Double?)value);
            else
                return value;
        }

        private static void OnMinPositionServoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMinPositionServoChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMinPositionServo(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionServoChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPosition

        public static readonly DependencyProperty MinPositionProperty = DependencyProperty.Register(
            "MinPosition", 
            typeof(Double?), 
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMinPositionChanged), 
                new CoerceValueCallback(OnCoerceMinPosition)
                )
            );


        public Double? MinPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MinPositionProperty);
            set => SetValue(MinPositionProperty, value);
        }

        private static object OnCoerceMinPosition(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMinPosition((Double?)value);
            else
                return value;
        }

        private static void OnMinPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMinPositionChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMinPosition(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MinPositionStop

        public static readonly DependencyProperty MinPositionStopProperty = DependencyProperty.Register(
            "MinPositionStop",
            typeof(Double?),
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMinPositionStopChanged),
                new CoerceValueCallback(OnCoerceMinPositionStop)
                )
            );


        public Double? MinPositionStop
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MinPositionStopProperty);
            set => SetValue(MinPositionStopProperty, value);
        }

        private static object OnCoerceMinPositionStop(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMinPositionStop((Double?)value);
            else
                return value;
        }

        private static void OnMinPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMinPositionStopChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMinPositionStop(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinPositionStopChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region TargetPosition

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register(
            "TargetPosition",
            typeof(Double?),
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnTargetPositionChanged),
                new CoerceValueCallback(OnCoerceTargetPosition)
                )
            );

        public Double? TargetPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(TargetPositionProperty);
            set => SetValue(TargetPositionProperty, value);
        }

        private static object OnCoerceTargetPosition(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceTargetPosition((Double?)value);
            else
                return value;
        }

        private static void OnTargetPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnTargetPositionChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceTargetPosition(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnTargetPositionChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPositionStop

        public static readonly DependencyProperty MaxPositionStopProperty = DependencyProperty.Register(
            "MaxPositionStop",
            typeof(Double?),
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnMaxPositionStopChanged),
                new CoerceValueCallback(OnCoerceMaxPositionStop)
                )
            );


        public Double? MaxPositionStop
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MaxPositionStopProperty);
            set => SetValue(MaxPositionStopProperty, value);
        }

        private static object OnCoerceMaxPositionStop(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMaxPositionStop((Double?)value);
            else
                return value;
        }

        private static void OnMaxPositionStopChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMaxPositionStopChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMaxPositionStop(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionStopChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPosition

        public static readonly DependencyProperty MaxPositionProperty = DependencyProperty.Register(
            "MaxPosition", 
            typeof(Double?), 
            typeof(RCServoTargetPositionControl),
            new FrameworkPropertyMetadata(
                0.0, 
                new PropertyChangedCallback(OnMaxPositionChanged), 
                new CoerceValueCallback(OnCoerceMaxPosition)));

        public Double? MaxPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MaxPositionProperty);
            set => SetValue(MaxPositionProperty, value);
        }

        private static object OnCoerceMaxPosition(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMaxPosition((Double?)value);
            else
                return value;
        }

        private static void OnMaxPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMaxPositionChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMaxPosition(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region MaxPositionServo

        public static readonly DependencyProperty MaxPositionServoProperty = DependencyProperty.Register(
            "MaxPositionServo", 
            typeof(Double?), 
            typeof(RCServoTargetPositionControl),
                new FrameworkPropertyMetadata(
                    0.0, 
                    new PropertyChangedCallback(OnMaxPositionServoChanged), 
                    new CoerceValueCallback(OnCoerceMaxPositionServo)
                    )
            );

        public Double? MaxPositionServo
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MaxPositionServoProperty);
            set => SetValue(MaxPositionServoProperty, value);
        }

        private static object OnCoerceMaxPositionServo(DependencyObject o, object value)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMaxPositionServo((Double?)value);
            else
                return value;
        }

        private static void OnMaxPositionServoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            RCServoTargetPositionControl positionControl = o as RCServoTargetPositionControl;
            if (positionControl != null)
                positionControl.OnMaxPositionServoChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMaxPositionServo(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxPositionServoChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #endregion

        #region Event Handlers

        private void thisControl_Loaded(object sender, RoutedEventArgs e)
        {
            var rb1IsChecked = rb1.IsChecked;
            var rb5IsChecked = rb5.IsChecked;
            var rb10IsChecked = rb10.IsChecked;
            var rb50IsChecked = rb50.IsChecked;
            rb1.IsChecked = true;
        }

        private void thisControl_Initialized(object sender, EventArgs e)
        {
            var rb1IsChecked = rb1.IsChecked;
            var rb5IsChecked = rb5.IsChecked;
            var rb10IsChecked = rb10.IsChecked;
            var rb50IsChecked = rb50.IsChecked;
            rb1.IsChecked = true;
        }

        private void PositionIncrement_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;

            seTargetPosition.Increment = Int32.Parse(radioButton.Content.ToString());
        }

        private void TrackBarEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var centerPosition = (MaxPosition - MinPosition) / 2 + MinPosition;
            ((TrackBarEdit)sender).Value = (Double)centerPosition;
        }

        private void ResetMinMaxPosition_Click(object sender, RoutedEventArgs e)
        {
            MinPosition = MinPositionStop = MinPositionServo;
            MaxPosition = MaxPositionStop = MaxPositionServo;
        }

        private void SetPositionScale_Click(object sender, RoutedEventArgs e)
        {
            var centerPosition = (MaxPosition - MinPosition) / 2 + MinPosition;

            MinPosition = MinPositionStop = centerPosition - PositionScaleRange;
            MaxPosition = MaxPositionStop = centerPosition + PositionScaleRange;
        }

        private void SetPositionStops_Click(object sender, RoutedEventArgs e)
        {
            var centerPosition = (MaxPosition - MinPosition) / 2 + MinPosition;

            MinPositionStop = centerPosition - PositionStopRange;
            MaxPositionStop = centerPosition + PositionStopRange;
        }

        private void SetTargetPosition(object sender, RoutedEventArgs e)
        {
            var rea = e;
            var s = sender;

            var goToWhere = ((System.Windows.Controls.Button)sender).CommandParameter;

            switch (goToWhere)
            {
                case "Min":
                    TargetPosition = MinPosition;
                    break;

                // NOTE(crhodes)
                // This is now handled with TargetPostion Increment/Decrement
                // and PositionIncrement Scale RadioButtons

                //case "R-5":
                //    TargetPosition = TargetPosition -= 5;
                //    break;

                case "Center":
                    TargetPosition = (MaxPosition - MinPosition) / 2 + MinPosition;
                    break;

                // NOTE(crhodes)
                // This is now handled with TargetPostion Increment/Decrement
                // and PositionIncrement Scale RadioButtons

                //case "R+5":
                //    TargetPosition = TargetPosition += 5;
                //    break;

                case "Max":
                    TargetPosition = MaxPosition;
                    break;

                default:
                    Log.Error($"Unknown goToWhere >{goToWhere}<", Common.LOG_CATEGORY);
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

