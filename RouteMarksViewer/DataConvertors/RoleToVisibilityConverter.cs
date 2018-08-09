using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace RouteMarksViewer.DataConvertors 
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as Models.User;
            if (user != null)
            {
                string parameterString = parameter as string;
                if (!string.IsNullOrEmpty(parameterString))
                {
                    string[] parameters = parameterString.Split(new char[] { '|' });
                    bool AllRight = false;
                    foreach (string item in parameters)
                    {
                        if (System.Convert.ToInt32(item) == user.UserRoleId)
                        {
                            AllRight = true;
                            break;
                        }
                    }
                    return AllRight ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                    return null;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
