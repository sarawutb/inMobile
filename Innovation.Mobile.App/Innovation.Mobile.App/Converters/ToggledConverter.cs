using System;
using System.Globalization;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Converters
{
    public class ToggledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToggledEventArgs eventArgs)
                return eventArgs.Value;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
