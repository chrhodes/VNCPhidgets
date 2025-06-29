﻿using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class VelocityLimitControl : ViewBase//, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public VelocityLimitControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IVelocityControlViewModel)DataContext;

            // Can create directly
            // ViewModel = VelocityControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        // public VelocityControl(IVelocityControlViewModel viewModel)
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

            //lgMain.DataContext = this;
            spDeveloperInfo.DataContext = this;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        //#region MinVelocityLimit

        //public static readonly DependencyProperty MinVelocityLimitProperty = DependencyProperty.Register(
        //    "MinVelocityLimit", 
        //    typeof(Double?), 
        //    typeof(VelocityLimitControl),
        //    new FrameworkPropertyMetadata(
        //        0.0, 
        //        new PropertyChangedCallback(OnMinVelocityLimitChanged), 
        //        new CoerceValueCallback(OnCoerceMinVelocityLimit)
        //        )
        //    );

        //public Double? MinVelocityLimit
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(MinVelocityLimitProperty);
        //    set => SetValue(MinVelocityLimitProperty, value);
        //}

        //private static object OnCoerceMinVelocityLimit(DependencyObject o, object value)
        //{
        //    VelocityLimitControl velocityLimitControl = o as VelocityLimitControl;
        //    if (velocityLimitControl != null)
        //        return velocityLimitControl.OnCoerceMinVelocityLimit((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnMinVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    VelocityLimitControl velocityLimitControl = o as VelocityLimitControl;
        //    if (velocityLimitControl != null)
        //        velocityLimitControl.OnMinVelocityLimitChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceMinVelocityLimit(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnMinVelocityLimitChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        //#region VelocityLimit

        //public static readonly DependencyProperty VelocityLimitProperty = DependencyProperty.Register(
        //    "VelocityLimit", 
        //    typeof(Double?), 
        //    typeof(VelocityLimitControl),
        //    new FrameworkPropertyMetadata(
        //        0.0, 
        //        new PropertyChangedCallback(OnVelocityLimitChanged), 
        //        new CoerceValueCallback(OnCoerceVelocityLimit)
        //        )
        //    );

        //public Double? VelocityLimit
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(VelocityLimitProperty);
        //    set => SetValue(VelocityLimitProperty, value);
        //}

        //private static object OnCoerceVelocityLimit(DependencyObject o, object value)
        //{
        //    VelocityLimitControl velocityControl = o as VelocityLimitControl;
        //    if (velocityControl != null)
        //        return velocityControl.OnCoerceVelocityLimit((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    VelocityLimitControl velocityControl = o as VelocityLimitControl;
        //    if (velocityControl != null)
        //        velocityControl.OnVelocityLimitChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceVelocityLimit(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnVelocityLimitChanged(Double? oldValue, Double? newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //}

        //#endregion

        //#region MaxVelocityLimit

        //public static readonly DependencyProperty MaxVelocityLimitProperty = DependencyProperty.Register(
        //    "MaxVelocityLimit", 
        //    typeof(Double?), 
        //    typeof(VelocityLimitControl),
        //    new FrameworkPropertyMetadata(
        //        0.0, 
        //        new PropertyChangedCallback(OnMaxVelocityLimitChanged), 
        //        new CoerceValueCallback(OnCoerceMaxVelocityLimit)
        //        )
        //    );

        //public Double? MaxVelocityLimit
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (Double?)GetValue(MaxVelocityLimitProperty);
        //    set => SetValue(MaxVelocityLimitProperty, value);
        //}

        //private static object OnCoerceMaxVelocityLimit(DependencyObject o, object value)
        //{
        //    VelocityLimitControl velocityLimitControl = o as VelocityLimitControl;
        //    if (velocityLimitControl != null)
        //        return velocityLimitControl.OnCoerceMaxVelocityLimit((Double?)value);
        //    else
        //        return value;
        //}

        //private static void OnMaxVelocityLimitChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    VelocityLimitControl velocityLimitControl = o as VelocityLimitControl;
        //    if (velocityLimitControl != null)
        //        velocityLimitControl.OnMaxVelocityLimitChanged((Double?)e.OldValue, (Double?)e.NewValue);
        //}

        //protected virtual Double? OnCoerceMaxVelocityLimit(Double? value)
        //{
        //    // TODO: Keep the proposed value within the desired range.
        //    return value;
        //}

        //protected virtual void OnMaxVelocityLimitChanged(Double? oldValue, Double? newValue)
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
        //    // Put a break point on ServoProperties VelocityVelocityLimit to see.

        //    // I think this can also be handled in Xaml with UpdateSourceTrigger=PropertyChanged,
        //    VelocityLimit = Double.Parse(e.NewValue.ToString());
        //}

        private void VelocityLimitIncrement_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;

            seVelocityLimit.Increment = Int32.Parse(radioButton.Content.ToString());
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
