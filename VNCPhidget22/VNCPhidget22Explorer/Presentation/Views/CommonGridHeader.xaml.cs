using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace VNCPhidget22Explorer.Presentation.Views
{
    public partial class CommonGridHeader : UserControl, INotifyPropertyChanged
    {
        public CommonGridHeader()
        {
            InitializeComponent();
            tbCount.DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Dependency Properties

        // NOTE(crhodes)
        // Old approach was to have DependencyProperty here.  Now we search tree in Xaml to FindAncestor
        //#region OutputFileNameAndPath

        //public static readonly DependencyProperty OutputFileNameAndPathProperty = DependencyProperty.Register("OutputFileNameAndPath",
        //    typeof(string), typeof(CommonGridHeader), new PropertyMetadata(null, new PropertyChangedCallback(OnOutputFileNameAndPathChanged)));

        //private static void OnOutputFileNameAndPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    CommonGridHeader commonGridHeader = o as CommonGridHeader;
        //    if (commonGridHeader != null)
        //        commonGridHeader.OnOutputFileNameAndPathChanged((string)e.OldValue, (string)e.NewValue);
        //}

        //protected virtual void OnOutputFileNameAndPathChanged(string oldValue, string newValue)
        //{
        //    // TODO: Add your property changed side-effects. Descendants can override as well.
        //    teOutput.Text = newValue;
        //}

        //public string OutputFileNameAndPath
        //{
        //    // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
        //    get => (string)GetValue(OutputFileNameAndPathProperty);
        //    set => SetValue(OutputFileNameAndPathProperty, value);
        //}

        //#endregion

        #region Count

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
            "Count",
            typeof(string), 
            typeof(CommonGridHeader), 
            new PropertyMetadata(null, new PropertyChangedCallback(OnCountChanged)));

        private static void OnCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            CommonGridHeader commonGridHeader = (CommonGridHeader)o;
            if (commonGridHeader != null)
               commonGridHeader.OnCountChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnCountChanged(string oldValue, string newValue)
        {
            //Count = newValue;
            tbCount.Text = newValue;

            // TODO: Add your property changed side-effects. Descendants can override as well.
        }

        public string Count
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get => (string)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        #endregion

        #endregion

    }
}
