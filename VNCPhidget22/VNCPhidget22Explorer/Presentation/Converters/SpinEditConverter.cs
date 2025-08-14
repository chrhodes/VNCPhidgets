using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using VNC;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class SpinEditConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            if (value != null)
                return value;

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double? result = Convert.ToDouble(value);

            return result;
        }
    }
}
