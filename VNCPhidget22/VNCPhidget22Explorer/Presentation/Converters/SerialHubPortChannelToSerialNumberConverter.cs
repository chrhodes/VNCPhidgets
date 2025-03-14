using System;
using System.Windows.Data;

using VNC.Phidget22;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class SerialHubPortChannelToSerialNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((SerialHubPortChannel)value).SerialNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
