using System;
using System.Windows.Data;

using VNC.Phidget22;

namespace VNCPhidget22Explorer.Presentation.Converters
{
    public class SerialHubPortChannelToChannelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((SerialHubPortChannel)value).Channel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
