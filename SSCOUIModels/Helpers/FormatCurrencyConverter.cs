using System;
using System.Windows.Data;
using System.Globalization;

namespace SSCOUIModels.Helpers
{
    public class FormatCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string inputString = value.ToString();

            if (0 != inputString.Length)
            {                
                inputString = inputString.Replace(RegionInfo.CurrentRegion.CurrencySymbol, "");
                inputString = inputString.Replace("-", "");
                inputString = inputString.Replace(",", "");

                double price = 0;
                double.TryParse(inputString, NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out price);                

                if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits == 0)
                {
                    inputString = inputString.Replace(",", "");
                }

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
