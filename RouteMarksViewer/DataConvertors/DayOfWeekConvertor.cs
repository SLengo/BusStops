using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;


namespace RouteMarksViewer.DataConvertors
{
    public class DayOfWeekConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int dayOfWeek = System.Convert.ToInt32(value);
            string dayOfWeekStr = "";
            switch(dayOfWeek)
            {
                case 1:
                    {
                        dayOfWeekStr = "Понедельник";
                        break;
                    }
                case 2:
                    {
                        dayOfWeekStr = "Вторник";
                        break;
                    }
                case 3:
                    {
                        dayOfWeekStr = "Среда";
                        break;
                    }
                case 4:
                    {
                        dayOfWeekStr = "Четверг";
                        break;
                    }
                case 5:
                    {
                        dayOfWeekStr = "Пятница";
                        break;
                    }
                case 6:
                    {
                        dayOfWeekStr = "Суббота";
                        break;
                    }
                case 7:
                    {
                        dayOfWeekStr = "Воскресенье";
                        break;
                    }
            }
            return dayOfWeekStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
