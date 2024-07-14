using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using DevExpress.Xpf.Printing;

//using Microsoft.AspNetCore.SignalR.Protocol;

using static Phidgets.ServoServo;

namespace VNCPhidgets21Explorer.Presentation.Converters
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

            //return enumValue == null ? Depen;
            //Type hum = value.GetType();
            //if (value != null)
            //    return new List<hum.>((IEnumerable<string>)value.ToString());

            //return null;
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
            return value.ToString();
            //List<string> result = new List<string>();
            //var enumerable = (List<object>)value;
            //if (enumerable != null)
            //    foreach (object item in enumerable)
            //        result.Add((string)item);
            //return result;
        }
    }
}
