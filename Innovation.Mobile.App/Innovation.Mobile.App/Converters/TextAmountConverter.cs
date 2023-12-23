using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Converters
{
    public class TextAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextChangedEventArgs eventArgs)
            {
                string strValue = eventArgs.OldTextValue;
                if (!string.IsNullOrWhiteSpace(eventArgs.NewTextValue))
                {
                    bool isValid = eventArgs.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                    strValue = isValid ? eventArgs.NewTextValue : strValue;
                }
                else
                {
                    strValue = "";
                }
                return strValue;
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
