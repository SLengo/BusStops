using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RouteMarksViewer.DataConvertors
{
    public class WindowStartTimeConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "Автобус может отправиться с ";

            TimeSpan time_start = TimeSpan.FromSeconds(
                (int)values[0]
                );
            TimeSpan time_delta = TimeSpan.FromSeconds(
                (int)values[1]
                );
            TimeSpan time_from = time_start - time_delta < new TimeSpan(-0, 0, 0) ? (time_start - time_delta + new TimeSpan(24,0,0)) 
                : time_start - time_delta;
            TimeSpan time_to = time_start + time_delta > new TimeSpan(24, 0, 0) ? (time_start + time_delta - new TimeSpan(24, 0, 0))
                : time_start + time_delta;
            res += time_from.ToString() + " по " + time_to.ToString();

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
