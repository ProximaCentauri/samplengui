using System;

namespace SSCOUIModels.Models
{
    public class UIEchoField
    {
        public UIEchoField()
        { 
        }

        public UIEchoField(string echoFields)
        {
            string[] properties = echoFields.Split(FieldPropertySeparator);
            foreach (string property in properties)
            {
                string[] nameValue = property.Split(FieldValueSeparator);
                if (2 == nameValue.Length)
                {
                    if (nameValue[0].Equals(MinLenProperty))
                    {
                        MinLength = int.Parse(nameValue[1]);
                    }
                    else if (nameValue[0].Equals(MaxLenProperty))
                    {
                        MaxLength = int.Parse(nameValue[1]);
                    }
                    else if (nameValue[0].Equals(PrivateProperty))
                    {
                        IsPrivate = bool.Parse(nameValue[1]);
                    }
                    else if (nameValue[0].Equals(CurrencyProperty))
                    {
                        CurrencyEnabled = bool.Parse(nameValue[1]);
                    }
                    else if (nameValue[0].Equals(TrimDecimalProperty))
                    {
                        TrimDecimalEnabled = bool.Parse(nameValue[1]);
                    }
                }
            }            
        }

        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }
        public bool IsPrivate { get; private set; }
        public bool CurrencyEnabled { get; private set; }
        public bool TrimDecimalEnabled { get; private set; }

        private const char FieldValueSeparator = '=';
        private const char FieldPropertySeparator = ';';
        private const string MinLenProperty = "MinLen";
        private const string MaxLenProperty = "MaxLen";
        private const string PrivateProperty = "Private";
        private const string CurrencyProperty = "Currency";
        private const string TrimDecimalProperty = "TrimDecimal";
    }
}
