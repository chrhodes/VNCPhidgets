using System;
using System.Windows;

using VNCPhidgets21Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.XtraSpellChecker.Parser;
using System.Threading;
using System.Linq;

namespace VNCPhidgets21Explorer.Presentation.Controls
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
            
            lgMain.DataContext = this;
            liPositionRange.DataContext = this;

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

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        //private static object OnCoerceDeviceMax(DependencyObject o, object value)
        //{
        //    StepperPositionControl positionControl = o as StepperPositionControl;
        //    if (positionControl != null)
        //        return positionControl.OnCoerceDeviceMax((Int64?)value);
        //    else
        //        return value;
        //}

        //private static void OnDeviceMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    StepperPositionControl positionControl = o as StepperPositionControl;
        //    if (positionControl != null)
        //        positionControl.OnDeviceMaxChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceDeviceMax(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnDeviceMaxChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}
        //private static object OnCoerceDeviceMin(DependencyObject o, object value)
        //{
        //    StepperPositionControl positionControl = o as StepperPositionControl;
        //    if (positionControl != null)
        //        return positionControl.OnCoerceDeviceMin((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnDeviceMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    StepperPositionControl positionControl = o as StepperPositionControl;
        //    if (positionControl != null)
        //        positionControl.OnDeviceMinChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceDeviceMin(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnDeviceMinChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties

        public Int32? StepperIndex
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(StepperIndexProperty);
            set => SetValue(StepperIndexProperty, value);
        }

        //public Double? DeviceMax
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(DeviceMaxProperty);
        //    set => SetValue(DeviceMaxProperty, value);
        //}
        //public Double? DeviceMin
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(DeviceMinProperty);
        //    set => SetValue(DeviceMinProperty, value);
        //}

        public Int64? Min
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int64?)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public Int64? CurrentPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int64?)GetValue(CurrentPositionProperty);
            set => SetValue(CurrentPositionProperty, value);
        }

        public Int64? TargetPosition
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int64?)GetValue(TargetPositionProperty);
            set => SetValue(TargetPositionProperty, value);
        }

        public Int64? Max
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int64?)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public Int32 PositionRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(PositionRangeProperty);
            set => SetValue(PositionRangeProperty, value);
        }

        #region DependencyProperties

        //public static readonly DependencyProperty DeviceMinProperty = DependencyProperty.Register("DeviceMin", typeof(Double?), typeof(StepperPositionControl),
        //    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceMinChanged), new CoerceValueCallback(OnCoerceDeviceMin)));
        //public static readonly DependencyProperty DeviceMaxProperty = DependencyProperty.Register("DeviceMax", typeof(Double?), typeof(StepperPositionControl),
        //    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceMaxChanged), new CoerceValueCallback(OnCoerceDeviceMax)));

        public static readonly DependencyProperty StepperIndexProperty = DependencyProperty.Register("StepperIndex", typeof(Int32?), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnStepperIndexChanged), new CoerceValueCallback(OnCoerceStepperIndex)));


        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(Int64?), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMinChanged), new CoerceValueCallback(OnCoerceMin)));

        public static readonly DependencyProperty CurrentPositionProperty = DependencyProperty.Register("CurrentPosition", typeof(Int64?), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCurrentPositionChanged), new CoerceValueCallback(OnCoerceCurrentPosition)));

        public static readonly DependencyProperty TargetPositionProperty = DependencyProperty.Register("TargetPosition", typeof(Int64?), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTargetPositionChanged), new CoerceValueCallback(OnCoerceTargetPosition)));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(Int64?), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMaxChanged), new CoerceValueCallback(OnCoerceMax)));

        public static readonly DependencyProperty PositionRangeProperty = DependencyProperty.Register("PositionRange", typeof(Int32), typeof(StepperPositionControl),
            new FrameworkPropertyMetadata(10, new PropertyChangedCallback(OnPositionRangeChanged), new CoerceValueCallback(OnCoercePositionRange)));

        #endregion

        #endregion

        #region Event Handlers

        private void TrackBarEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var centerPosition = (Max - Min) / 2 + Min;
            ((TrackBarEdit)sender).Value = (Double)centerPosition;
        }

        private void SetPositionRange_Click(object sender, RoutedEventArgs e)
        {
            Min = CurrentPosition - PositionRange;
            Max = CurrentPosition + PositionRange;
        }

        private void TestFullPositionRange_Click(object sender, RoutedEventArgs e)
        {
            var rea = e;
            var s = sender;

            var goToWhere = ((System.Windows.Controls.Button)sender).CommandParameter;

            switch (goToWhere)
            {
                case "Min":
                    CurrentPosition = Min;
                    break;

                case "R-5":
                    CurrentPosition = CurrentPosition -= 5;
                    break;

                case "Center":
                    CurrentPosition = (Max - Min) / 2 + Min;
                    break;

                case "R+5":
                    CurrentPosition = CurrentPosition += 5;
                    break;

                case "Max":
                    CurrentPosition = Max;
                    break;

                default:
                    Log.Error($"Unknown goToWhere >{goToWhere}<", Common.LOG_CATEGORY);
                    break;
            }
        }
        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods (None)


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods

        #region StepperIndex

        private static object OnCoerceStepperIndex(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceStepperIndex((Int32?)value);
            else
                return value;
        }

        private static void OnStepperIndexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnStepperIndexChanged((Int32?)e.OldValue, (Int32?)e.NewValue);
        }

        protected virtual Int32? OnCoerceStepperIndex(Int32? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStepperIndexChanged(Int32? oldValue, Int32? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Min

        private static object OnCoerceMin(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMin((Int64?)value);
            else
                return value;
        }

        private static void OnMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnMinChanged((Int64?)e.OldValue, (Int64?)e.NewValue);
        }

        protected virtual Int64? OnCoerceMin(Int64? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinChanged(Int64? oldValue, Int64? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region CurrentPosition

        private static object OnCoerceCurrentPosition(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceCurrentPosition((Int64?)value);
            else
                return value;
        }

        private static void OnCurrentPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnCurrentPositionChanged((Int64?)e.OldValue, (Int64?)e.NewValue);
        }

        protected virtual Int64? OnCoerceCurrentPosition(Int64? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnCurrentPositionChanged(Int64? oldValue, Int64? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region TargetPosition

        private static object OnCoerceTargetPosition(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceTargetPosition((Int64?)value);
            else
                return value;
        }

        private static void OnTargetPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnTargetPositionChanged((Int64?)e.OldValue, (Int64?)e.NewValue);
        }

        protected virtual Int64? OnCoerceTargetPosition(Int64? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnTargetPositionChanged(Int64? oldValue, Int64? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Max

        private static object OnCoerceMax(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMax((Int64?)value);
            else
                return value;
        }

        private static void OnMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnMaxChanged((Int64?)e.OldValue, (Int64?)e.NewValue);
        }

        protected virtual Int64? OnCoerceMax(Int64? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxChanged(Int64? oldValue, Int64? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region PositionRange

        private static object OnCoercePositionRange(DependencyObject o, object value)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperPositionControl positionControl = o as StepperPositionControl;
            if (positionControl != null)
                positionControl.OnPositionRangeChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual Int32 OnCoercePositionRange(Int32 value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPositionRangeChanged(Int32 oldValue, Int32 newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

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

