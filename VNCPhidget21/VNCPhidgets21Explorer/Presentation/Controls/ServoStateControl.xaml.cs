using System;
using System.Windows;

using VNC;
using VNC.Core.Mvvm;

namespace VNCPhidgets21Explorer.Presentation.Controls
{
    public partial class ServoStateControl : ViewBase, IInstanceCountV
    {
        #region Constructors, Initialization, and Load
        
        public ServoStateControl()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InstanceCountV++;
            InitializeComponent();

            lgMain.DataContext = this;
			// Expose ViewModel
						
            // If View First with ViewModel in Xaml

            // ViewModel = (IServoStateControlViewModel)DataContext;

            // Can create directly
            // ViewModel = ServoStateControlViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //public ServoStateControl(IServoStateControlViewModel viewModel)
        //{
        //    Int64 startTicks = Log.CONSTRUCTOR($"Enter viewModel({viewModel.GetType()}", Common.LOG_CATEGORY);

        //    InstanceCountV++;
        //    InitializeComponent();

        //    ViewModel = viewModel;

        //    InitializeView();

        //    Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        //}

        private static object OnCoerceSpeedRamping(DependencyObject o, object value)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                return servoStateControl.OnCoerceSpeedRamping((Boolean?)value);
            else
                return value;
        }

        private static void OnSpeedRampingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                servoStateControl.OnSpeedRampingChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        protected virtual Boolean? OnCoerceSpeedRamping(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnSpeedRampingChanged(Boolean? oldValue, Boolean? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceCurrent(DependencyObject o, object value)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                return servoStateControl.OnCoerceCurrent((Double?)value);
            else
                return value;
        }

        private static void OnCurrentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                servoStateControl.OnCurrentChanged((Double?)e.OldValue, (Double?)e.NewValue);
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

        private static object OnCoerceStopped(DependencyObject o, object value)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                return servoStateControl.OnCoerceStopped((Boolean?)value);
            else
                return value;
        }

        private static void OnStoppedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                servoStateControl.OnStoppedChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
        }

        protected virtual Boolean? OnCoerceStopped(Boolean? value)
        {
            // TODO: Keep the proposed value within the desired range.
            return value;
        }

        protected virtual void OnStoppedChanged(Boolean? oldValue, Boolean? newValue)
        {
            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        private static object OnCoerceEngaged(DependencyObject o, object value)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                return servoStateControl.OnCoerceEngaged((Boolean?)value);
            else
                return value;
        }

        private static void OnEngagedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ServoStateControl servoStateControl = o as ServoStateControl;
            if (servoStateControl != null)
                servoStateControl.OnEngagedChanged((Boolean?)e.OldValue, (Boolean?)e.NewValue);
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
        private void InitializeView()
        {
            Int64 startTicks = Log.VIEW_LOW("Enter", Common.LOG_CATEGORY);

            // NOTE(crhodes)
            // Put things here that initialize the View

            Log.VIEW_LOW("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums (None)


        #endregion

        #region Structures (None)


        #endregion

        #region Fields and Properties (None)


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


        #endregion

        #region IInstanceCount

        private static int _instanceCountV;


        public Boolean? SpeedRamping
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(SpeedRampingProperty);
            set => SetValue(SpeedRampingProperty, value);
        }
        public int InstanceCountV
        {
            get => _instanceCountV;
            set => _instanceCountV = value;
        }

        #endregion

        public Double? Current
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Double?)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }
        public Boolean? Stopped
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(StoppedProperty);
            set => SetValue(StoppedProperty, value);
        }

        public Boolean? Engaged
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (Boolean?)GetValue(EngagedProperty);
            set => SetValue(EngagedProperty, value);
        }

        public static readonly DependencyProperty EngagedProperty = DependencyProperty.Register("Engaged", typeof(Boolean?), typeof(ServoStateControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEngagedChanged), new CoerceValueCallback(OnCoerceEngaged)));

        public static readonly DependencyProperty StoppedProperty = DependencyProperty.Register("Stopped", typeof(Boolean?), typeof(ServoStateControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnStoppedChanged), new CoerceValueCallback(OnCoerceStopped)));

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Double?), typeof(ServoStateControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCurrentChanged), new CoerceValueCallback(OnCoerceCurrent)));

        public static readonly DependencyProperty SpeedRampingProperty = DependencyProperty.Register("SpeedRamping", typeof(Boolean?), typeof(ServoStateControl), 
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSpeedRampingChanged), new CoerceValueCallback(OnCoerceSpeedRamping)));
        
        

    }
}
