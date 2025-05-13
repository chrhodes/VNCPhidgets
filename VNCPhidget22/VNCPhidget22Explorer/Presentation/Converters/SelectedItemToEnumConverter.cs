using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

//using Microsoft.AspNetCore.SignalR.Protocol;


namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class SelectedItemToEnumConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = value as Enum;
            return enumValue;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
