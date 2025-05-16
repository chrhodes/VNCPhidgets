using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using VNC.Phidget22.Configuration;
using VNC.Phidget22.Configuration.Performance;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    /// <summary>
    /// Converts to and from a MultiItem Select ComboBox Control
    /// </summary>
    public class SelectedItemToXXXConverter
        : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    var foo = value;

                    return value;
                }
                catch (Exception ex)
                {
                    var oops = ex.Message;
                }
                //return new List<object>((IEnumerable<Performance>)value);
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Performance performance = null;

            var foo = (DictionaryEntry)value;

            try
            {
                performance = new Performance((Performance)foo.Value);

            }
            catch (Exception ex)
            {
                var oops = ex.Message;
            }

            return performance;

            //List<Performance> result = new List<Performance>();
            //var enumerable = (List<object>)value;

            //if (enumerable != null)
            //{
            //    foreach (object item in enumerable)
            //    {
            //        result.Add((Performance)item);
            //    }
            //}

            //return result;




        }
    }
}
