using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using VNCPhidget22.Configuration;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    /// <summary>
    /// Converts to and from a MultiItem Select ComboBox Control
    /// </summary>
    public class SelectedItemsToListOfStepperSequenceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new List<object>((IEnumerable<StepperSequence>)value);
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<StepperSequence> result = new List<StepperSequence>();
            var enumerable = (List<object>)value;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    result.Add((StepperSequence)item);
                }
            }

            return result;
        }
    }
}
