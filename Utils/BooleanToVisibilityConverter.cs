using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace BookBuddy.Utils
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
                return Visibility.Collapsed;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visibilityValue))
                return false;

            return visibilityValue == Visibility.Visible;
        }
    }
}
