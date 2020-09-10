using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IMark.Convertor
{
    public class StatusToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            string colorCode = string.Empty;
            switch(status)
            {
                case "UnFulfilled":
                    colorCode = "#FEA100";
                    break;
                case "Partialy Paid":
                    colorCode = "#42B986";
                    break;
                case "Fulfilled":
                    colorCode = "#FE4E00";
                    break;
            }
            return colorCode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
