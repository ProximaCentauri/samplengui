using System;
using System.Windows.Data;
using System.Globalization;

namespace SSCOUIModels.Helpers
{
    public class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string inputString = value.ToString();

            if (0 != inputString.Length)
            {
                return String.Format(parameter.ToString(), inputString);
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
