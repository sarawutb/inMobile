using Innovation.Mobile.App.Enumerations;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Converters
{
    public class CardViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (bool)value;
            if(a == false)
            {
                value = CardViewType.Wait;
            }
            else
            {
                value = CardViewType.Checked;
            }

            var type = (CardViewType)value;
            switch (type)
            {
                case CardViewType.Wait:
                    return Color.Gray;
                case CardViewType.Checked:
                    return Color.Green;
                default:
                    return Color.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not needed here
            throw new NotImplementedException();
        }
    }
}
