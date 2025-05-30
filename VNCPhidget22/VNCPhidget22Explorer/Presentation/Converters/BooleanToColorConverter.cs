﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Black;
                return brush;
            }

            if ((Boolean)value == true)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Green;
                return brush;
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Red;
                return brush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
