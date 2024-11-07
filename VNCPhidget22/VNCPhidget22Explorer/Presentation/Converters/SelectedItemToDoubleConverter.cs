using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class SelectedItemToDoubleConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return (Double)value;

            return null;
        }
        //object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    ObservableCollection<string> result = new ObservableCollection<string>();
        //    var enumerable = (List<object>)value;
        //    if (enumerable != null)
        //        foreach (object item in enumerable)
        //            result.Add((string)item);
        //    return result;
        //}

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result;

            result = value.ToString();

            return result;
        }
    }
}
