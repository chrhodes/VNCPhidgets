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
            Int64 startTicks = 0;
            if (Common.VNCLogging.PresentationLow) startTicks = Log.CONSTRUCTOR("Enter/Exit", Common.LOG_CATEGORY);

            if (value != null)
                return value;

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int64 startTicks = 0;
            if (Common.VNCLogging.PresentationLow) startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            Double? result = Convert.ToDouble(value);

            if (Common.VNCLogging.Constructor) Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);

            return result;
        }
    }
}
