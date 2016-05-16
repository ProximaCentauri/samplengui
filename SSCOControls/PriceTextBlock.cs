using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace SSCOControls
{
    public class PriceTextBlock : MeasureTextBlock
    {
        public PriceTextBlock()
        {
        }

        public bool IsPartialTender
        {
            get
            {
                return Convert.ToBoolean(GetValue(IsPartialTenderProperty));
            }
            set
            {
                SetValue(IsPartialTenderProperty, value);
            }
        }

        public bool IsTrimDecimal
        {
            get
            {
                return Convert.ToBoolean(GetValue(IsTrimDecimalProperty));
            }
            set
            {
                SetValue(IsTrimDecimalProperty, value);
            }
        }

        public object Value
        {
            get
            {
                return GetValue(ValueProperty) as string;
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static void OnIsPartialTenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != e.NewValue)
            {
                if (Convert.ToBoolean(e.NewValue))
                {
                    PriceTextBlock textBlock = d as PriceTextBlock;
                    if (null != textBlock)
                    {
                        var numberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                        numberFormat.CurrencyNegativePattern = NumberFormatInfo.CurrentInfo.CurrencyNegativePattern;
                        double value = 0;
                        double.TryParse(textBlock.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out value);
                        if (value > 0)
                        {
                            value = value * -1;
                        }
                        textBlock.Text = value.ToString("C", numberFormat);
                    }
                }
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != e.NewValue)
            {
                string value = e.NewValue.ToString();
                PriceTextBlock textBlock = d as PriceTextBlock;
                if (0 != value.Length)
                {
                    Regex regex = new Regex(@"[0-9|\-\(\)]+");
                    string inputString = String.Empty;
                    foreach (Match m in regex.Matches(value))
                    {
                        inputString += m.Value;
                    }
                    double price = 0;
                    double.TryParse(inputString, NumberStyles.Any, CultureInfo.CurrentCulture, out price);
                    if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits > 0 && !textBlock.IsTrimDecimal)
                    {
                        price /= 100;
                    }
                    if (d != null)
                    {
                        var numberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                        if(textBlock.IsTrimDecimal)
                        {
                            numberFormat.CurrencyDecimalDigits = 0;
                        }

                        if (price < 0)
                        {
                            numberFormat.CurrencyNegativePattern = NumberFormatInfo.CurrentInfo.CurrencyNegativePattern;
                        }
                        else
                        {
                            numberFormat.CurrencyPositivePattern = NumberFormatInfo.CurrentInfo.CurrencyPositivePattern;
                        }
                        value = price.ToString("c", numberFormat);
                    }
                }
                textBlock.Text = value;
            }
        }

        private static DependencyProperty IsPartialTenderProperty = DependencyProperty.Register("IsPartialTender", typeof(bool), typeof(PriceTextBlock),
            new PropertyMetadata(false, OnIsPartialTenderChanged));

        private static DependencyProperty IsTrimDecimalProperty = DependencyProperty.Register("IsTrimDecimal", typeof(bool), typeof(PriceTextBlock));

        private static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PriceTextBlock),
            new PropertyMetadata(null, OnValueChanged));
    }
}
