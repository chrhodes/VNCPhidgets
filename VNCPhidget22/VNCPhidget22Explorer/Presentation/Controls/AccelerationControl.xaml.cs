using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

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

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IAccelerationControlViewModel)DataContext;

            // Can create directly
            // ViewModel = AccelerationControlViewModel();

            InitializeView();

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
            // Hook event handlers, etc.

            ViewType = this.GetType().ToString().Split('.').Last();

            // Establish any additional DataContext(s), e.g. to things held in this View

            //lgMain.DataContext = this;
            spDeveloperInfo.DataContext = this;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties (none)

        //#region MinAcceleration

        //public static readonly DependencyProperty MinAccelerationProperty = DependencyProperty.Register(
        //    "MinAcceleration",
        //    typeof(Double?),
        //    typeof(AccelerationControl),
        //    new FrameworkPropertyMetadata(
        //        0.0,
        //        new PropertyChangedCallback(OnMinAccelerationChanged),
        //        new CoerceValueCallback(OnCoerceMinAcceleration)
        //        )
        //    );

        //public Double? MinAcceleration
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(MinAccelerationProperty);
        //    set => SetValue(MinAccelerationProperty, value);
        //}

        //private static object OnCoerceMinAcceleration(DependencyObject o, object value)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        return accelerationControl.OnCoerceMinAcceleration((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnMinAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        accelerationControl.OnMinAccelerationChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceMinAcceleration(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnMinAccelerationChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        //#region Acceleration

        //public static readonly DependencyProperty AccelerationProperty = DependencyProperty.Register(
        //    "Acceleration",
        //    typeof(Double?),
        //    typeof(AccelerationControl),
        //    new FrameworkPropertyMetadata(
        //        0.0,
        //        new PropertyChangedCallback(OnAccelerationChanged),
        //        new CoerceValueCallback(OnCoerceAcceleration)
        //        )
        //    );

        //public Double? Acceleration
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(AccelerationProperty);
        //    set => SetValue(AccelerationProperty, value);
        //}

        //private static object OnCoerceAcceleration(DependencyObject o, object value)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        return accelerationControl.OnCoerceAcceleration((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        accelerationControl.OnAccelerationChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceAcceleration(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnAccelerationChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        //#region MaxAcceleration

        //public static readonly DependencyProperty MaxAccelerationProperty = DependencyProperty.Register(
        //    "MaxAcceleration",
        //    typeof(Double?),
        //    typeof(AccelerationControl),
        //    new FrameworkPropertyMetadata(
        //        0.0,
        //        new PropertyChangedCallback(OnMaxAccelerationChanged),
        //        new CoerceValueCallback(OnCoerceMaxAcceleration)
        //        )
        //    );

        //public Double? MaxAcceleration
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(MaxAccelerationProperty);
        //    set => SetValue(MaxAccelerationProperty, value);
        //}

        //private static object OnCoerceMaxAcceleration(DependencyObject o, object value)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        return accelerationControl.OnCoerceMaxAcceleration((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnMaxAccelerationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    AccelerationControl accelerationControl = o as AccelerationControl;
        //    if (accelerationControl != null)
        //        accelerationControl.OnMaxAccelerationChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceMaxAcceleration(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnMaxAccelerationChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        #endregion

        #region Event Handlers

        //private void SpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    // NOTE(crhodes)
        //    // If we don't do this the Servo does not get new value.
        //    // Odd that it seemed like the UI was updating the servo before.
        //    // Put a break point on ServoProperties Acceleration to see.
        //    Acceleration = Double.Parse(e.NewValue.ToString());
        //}

        private void AccelerationIncrement_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;

            seAcceleration.Increment = Int32.Parse(radioButton.Content.ToString());
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
