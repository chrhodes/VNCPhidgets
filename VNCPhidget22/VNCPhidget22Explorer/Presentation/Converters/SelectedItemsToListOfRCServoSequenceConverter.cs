using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using VNC.Phidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.Converters
{

    /// <summary>
    /// Converts to and from a MultiItem Select ComboBox Control
    /// </summary>
    public class SelectedItemsToListOfRCServoSequenceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new List<object>((IEnumerable<RCServoSequence>)value);
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<RCServoSequence> result = new List<RCServoSequence>();
            var enumerable = (List<object>)value;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    result.Add((RCServoSequence)item);
                }
            }

            return result;
        }
    }
}
