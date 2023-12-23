using Innovation.Mobile.App.Enumerations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Converters
{
    public class LabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = (string)value;
            if (string.IsNullOrEmpty(color))
            {
                return Color.White;
            }
            Color returnColor;
            try
            {
                returnColor = Color.FromHex(color);
            }
            catch
            {
                returnColor = Color.White;
            }
            return returnColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
