using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace SSCOUIModels.Helpers
{
    public class StateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (Array.IndexOf(parameter.ToString().Split(','), value.ToString()) > -1) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception)
            {                
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }        
    }
}
