using System;
using System.Globalization;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Converters
{
    public class ButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ClickedEventArgs eventArgs)
            {
                return eventArgs.Parameter;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
