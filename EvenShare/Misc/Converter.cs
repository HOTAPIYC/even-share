using System;
using System.Globalization;
using Xamarin.Forms;

namespace EvenShare
{
    public class CentsToEuro : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(Math.Round((double)value * 100, 0));
        }
    }
}
