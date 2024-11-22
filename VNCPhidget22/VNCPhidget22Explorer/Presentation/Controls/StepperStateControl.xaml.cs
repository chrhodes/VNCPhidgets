﻿using System;
using System.Linq;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidget22Explorer.Presentation.Controls
{
    public partial class StepperStateControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public StepperStateControl()
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.Constructor) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            // Expose ViewModel

            // If View First with ViewModel in Xaml

            // ViewModel = (IStepperStateControlViewModel)DataContext;

            // Can create directly
            // ViewModel = StepperStateControlViewModel();

            InitializeView();

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public StepperStateControl(IStepperStateControlViewModel viewModel)
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

            lgMain.DataContext = this;

            if (Common.VNCLogging.ViewLow) Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (none)


        #endregion

        #region Structures (none)


        #endregion

        #region Fields and Properties

        #region Attached

        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.Register(
            "IsAttached",
            typeof(Boolean?),
            typeof(StepperStateControl),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnIsAttachedChanged),
                new CoerceValueCallback(OnCoerceIsAttached)));

        public Boolean? IsAttached
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(IsAttachedProperty);
            set => SetValue(IsAttachedProperty, value);
        }

        private static object OnCoerceIsAttached(DependencyObject o, object value)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                return stepperStateControl.OnCoerceIsAttached((Boolean?)value);
            else
                return value;
        }

        private static void OnIsAttachedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                stepperStateControl.OnIsAttachedChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        protected virtual Boolean? OnCoerceIsAttached(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnIsAttachedChanged(Boolean? oldValue, Boolean? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Current

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(
            "Current", 
            typeof(Double?), 
            typeof(StepperStateControl),
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
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                return stepperStateControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                stepperStateControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
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

        #region IsMoving

        public static readonly DependencyProperty IsMovingProperty = DependencyProperty.Register(
            "IsMoving", 
            typeof(Boolean?), 
            typeof(StepperStateControl),
            new FrameworkPropertyMetadata(
                null, 
                new PropertyChangedCallback(OnIsMovingChanged), 
                new CoerceValueCallback(OnCoerceIsMoving)));

        public Boolean? IsMoving
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(IsMovingProperty);
            set => SetValue(IsMovingProperty, value);
        }

        private static object OnCoerceIsMoving(DependencyObject o, object value)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                return stepperStateControl.OnCoerceIsMoving((Boolean?)value);
            else
                return value;
        }

        private static void OnIsMovingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                stepperStateControl.OnIsMovingChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        protected virtual Boolean? OnCoerceIsMoving(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnIsMovingChanged(Boolean? oldValue, Boolean? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Engaged

        public static readonly DependencyProperty EngagedProperty = DependencyProperty.Register(
            "Engaged", 
            typeof(Boolean?), 
            typeof(StepperStateControl),
            new FrameworkPropertyMetadata(null, 
                new PropertyChangedCallback(OnEngagedChanged), 
                new CoerceValueCallback(OnCoerceEngaged)));

        public Boolean? Engaged
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(EngagedProperty);
            set => SetValue(EngagedProperty, value);
        }

        private static object OnCoerceEngaged(DependencyObject o, object value)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                return stepperStateControl.OnCoerceEngaged((Boolean?)value);
            else
                return value;
        }

        private static void OnEngagedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl stepperStateControl = o as StepperStateControl;
            if (stepperStateControl != null)
                stepperStateControl.OnEngagedChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        protected virtual Boolean? OnCoerceEngaged(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnEngagedChanged(Boolean? oldValue, Boolean? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Position

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(Double?),
            typeof(StepperStateControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnPositionChanged),
                new CoerceValueCallback(OnCoercePosition)
                )
            );

        public Double? Position
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        private static object OnCoercePosition(DependencyObject o, object value)
        {
            StepperStateControl positionControl = o as StepperStateControl;
            if (positionControl != null)
                return positionControl.OnCoercePosition((Double?)value);
            else
                return value;
        }

        private static void OnPositionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl positionControl = o as StepperStateControl;
            if (positionControl != null)
                positionControl.OnPositionChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoercePosition(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnPositionChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        #endregion

        #region Velocity

        public static readonly DependencyProperty VelocityProperty = DependencyProperty.Register(
            "Velocity",
            typeof(Double?),
            typeof(StepperStateControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnVelocityChanged),
                new CoerceValueCallback(OnCoerceVelocity)
                )
            );

        public Double? Velocity
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(VelocityProperty);
            set => SetValue(VelocityProperty, value);
        }

        private static object OnCoerceVelocity(DependencyObject o, object value)
        {
            StepperStateControl positionControl = o as StepperStateControl;
            if (positionControl != null)
                return positionControl.OnCoerceVelocity((Double?)value);
            else
                return value;
        }

        private static void OnVelocityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            StepperStateControl positionControl = o as StepperStateControl;
            if (positionControl != null)
                positionControl.OnVelocityChanged((Double?)e.OldValue, (Double?)e.NewValue);
        }

        protected virtual Double? OnCoerceVelocity(Double? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnVelocityChanged(Double? oldValue, Double? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
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