using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace SSCOUIModels.Helpers
{
    public class StyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String styleName = value.ToString();

            if (String.IsNullOrEmpty(styleName))
                return null;

            Style newStyle = null;
            try
            {
                newStyle = Application.Current.FindResource(styleName) as Style;
            }
            catch (Exception)
            {                
            }

            return newStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
