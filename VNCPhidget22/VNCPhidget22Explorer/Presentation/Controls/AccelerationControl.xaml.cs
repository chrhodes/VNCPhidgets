using System;
using System.Windows;

using VNCPhidget22Explorer.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using System.Linq;

namespace VNCPhidget22Explorer.Presentation.Controls
{
    public partial class AccelerationControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public AccelerationControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            lgMain.DataContext = this;
            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IAccelerationControlViewModel)DataContext;

            // Can create directly
            // ViewModel = AccelerationControlViewModel();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public AccelerationControl(IAccelerationControlViewModel viewModel)
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
            // Hook eventhandlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private static object OnCoerceCurrent(DependencyObject o, object value)
        {
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                return accelerationControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                accelerationControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
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
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                return accelerationControl.OnCoerceMin((Double?)value);
            else
                return value;
        }

        private static void OnMinChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                accelerationControl.OnMinChanged((Double?)e.OldValue, (Double?)e.NewValue);
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
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                return accelerationControl.OnCoerceMax((Double?)value);
            else
                return value;
        }

        private static void OnMaxChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AccelerationControl accelerationControl = o as AccelerationControl;
            if (accelerationControl != null)
                accelerationControl.OnMaxChanged((Double?)e.OldValue, (Double?)e.NewValue);
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

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)


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

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(Double?), typeof(AccelerationControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMaxChanged), new CoerceValueCallback(OnCoerceMax)));

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register("Min", typeof(Double?), typeof(AccelerationControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnMinChanged), new CoerceValueCallback(OnCoerceMin)));

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Double?), typeof(AccelerationControl), 
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCurrentChanged), new CoerceValueCallback(OnCoerceCurrent)));

        private void SpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            // NOTE(crhodes)
            // If we don't do this the Servo does not get new value.
            // Odd that it seemed like the UI was updating the servo before.
            // Put a break point on ServoProperties Acceleration to see.
            Current = Double.Parse(e.NewValue.ToString());
        }
    }
}
