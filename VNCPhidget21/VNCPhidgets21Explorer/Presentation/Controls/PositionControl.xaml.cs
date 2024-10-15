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
    public partial class PositionControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public PositionControl()
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
            // Hook eventhandlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        private static object OnCoercePositionRange(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoercePositionRange((Int32)value);
            else
                return value;
        }

        private static void OnPositionRangeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
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
        private static object OnCoerceServoIndex(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceServoIndex((Int32?)value);
            else
                return value;
        }

        private static void OnServoIndexChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
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
        private static object OnCoerceDeviceMax(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceDeviceMax((Double?)value);
            else
                return value;
        }

        private static void OnDeviceMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnDeviceMaxChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceDeviceMax(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceMaxChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceDeviceMin(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceDeviceMin((Double?)value);
            else
                return value;
        }

        private static void OnDeviceMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnDeviceMinChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceDeviceMin(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnDeviceMinChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)

        public Int32 PositionRange
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32)GetValue(PositionRangeProperty);
            set => SetValue(PositionRangeProperty, value);
        }
        public Int32? ServoIndex
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Int32?)GetValue(ServoIndexProperty);
            set => SetValue(ServoIndexProperty, value);
        }
        public Double? DeviceMax
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(DeviceMaxProperty);
            set => SetValue(DeviceMaxProperty, value);
        }
        public Double? DeviceMin
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(DeviceMinProperty);
            set => SetValue(DeviceMinProperty, value);
        }
        public Double? Current
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }
        public Double? Min
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public Double? Max
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMaxChanged), new CoerceValueCallback(OnCoerceMax)));

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMinChanged), new CoerceValueCallback(OnCoerceMin)));

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCurrentChanged), new CoerceValueCallback(OnCoerceCurrent)));
        #endregion

        #region Event Handlers (None)


        #endregion

        #region Commands (None)

        #endregion

        #region Public Methods (None)


        #endregion

        #region Protected Methods (None)


        #endregion

        #region Private Methods (None)


        private static object OnCoerceCurrent(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
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
        private static object OnCoerceMin(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMin((Double?)value);
            else
                return value;
        }

        private static void OnMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnMinChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMin(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMinChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }
        private static object OnCoerceMax(DependencyObject o, object value)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                return positionControl.OnCoerceMax((Double?)value);
            else
                return value;
        }

        private static void OnMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PositionControl positionControl = o as PositionControl;
            if (positionControl != null)
                positionControl.OnMaxChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceMax(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnMaxChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }


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

        private void TrackBarEdit_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var centerPosition = (Max - Min) / 2 + Min;
            ((TrackBarEdit)sender).Value = (Double)centerPosition;
        }

        public static readonly DependencyProperty DeviceMinProperty = DependencyProperty.Register("DeviceMin", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceMinChanged), new CoerceValueCallback(OnCoerceDeviceMin)));
        public static readonly DependencyProperty DeviceMaxProperty = DependencyProperty.Register("DeviceMax", typeof(Double?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDeviceMaxChanged), new CoerceValueCallback(OnCoerceDeviceMax)));

        public static readonly DependencyProperty ServoIndexProperty = DependencyProperty.Register("ServoIndex", typeof(Int32?), typeof(PositionControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnServoIndexChanged), new CoerceValueCallback(OnCoerceServoIndex)));

        public static readonly DependencyProperty PositionRangeProperty = DependencyProperty.Register("PositionRange", typeof(Int32), typeof(PositionControl), 
            new FrameworkPropertyMetadata(10, new PropertyChangedCallback(OnPositionRangeChanged), new CoerceValueCallback(OnCoercePositionRange)));

        private void SetPositionRange_Click(object sender, RoutedEventArgs e)
        {
            Min = Current - PositionRange;
            Max = Current + PositionRange;
        }

        private void TestFullPositionRange_Click(object sender, RoutedEventArgs e)
        {
            var rea = e;
            var s = sender;

            var goToWhere = ((System.Windows.Controls.Button)sender).CommandParameter;

            switch (goToWhere)
            {
                case "Min":
                    Current = Min;
                    break;

                case "R-5":
                    Current = Current -= 5;
                    break;

                case "Center":
                    Current = (Max - Min) / 2 + Min;
                    break;

                case "R+5":
                    Current = Current += 5;
                    break;

                case "Max":
                    Current = Max;
                    break;

                default:
                    Log.Error($"Unknown goToWhere >{goToWhere}<", Common.LOG_CATEGORY);
                    break;
            }
        }
    }
}

