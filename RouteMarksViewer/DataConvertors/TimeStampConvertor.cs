using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RouteMarksViewer.DataConvertors
{
    public class TimeStampConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            if(parameter == null)
            {
                return epoch.AddSeconds(System.Convert.ToDouble(value));
            }
            else if ((string)parameter == "1")
            {
                return epoch.AddSeconds(System.Convert.ToDouble(value)).ToLongTimeString();
            }
            else if ((string)parameter == "2")
            {
                return epoch.AddSeconds(System.Convert.ToDouble(value)).ToShortTimeString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
