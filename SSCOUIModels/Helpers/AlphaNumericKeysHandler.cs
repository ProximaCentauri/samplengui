using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections.ObjectModel;
using FPsxWPF.Controls;
using System.Windows;
using RPSWNET;

namespace SSCOUIModels.Helpers
{
    public class AlphaNumericKeysHandler : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null != parameter && null != values && values[0].ToString().Length > 0)
            {
                if (parameter.ToString().ToLower().Equals("line1alpha"))
                {
                    return manageLine1AlphaNumericKeys(values, parameter.ToString());
                }
                else if (parameter.ToString().ToLower().Equals("line1alphacount"))
                {
                    return manageLine1AlphaNumericKeys(values, parameter.ToString()).Count;
                }
                else if (parameter.ToString().ToLower().Contains("line1numeric") && parameter.ToString().ToLower().Contains("columns=") && parameter.ToString().ToLower().Contains("row="))
                {
                    return manageLine1NumericKeys(values, parameter.ToString());
                }
                else if (parameter.ToString().ToLower().Equals("count"))
                {
                    if (values[0] != null && ((ObservableCollection<GridItem>)values[0]).Count > 0)
                    {
                        return ((ObservableCollection<GridItem>)getCollectionForCount(values[0], (int)values[4])).Count;
                    }
                    else if (values[1] != null)
                    {
                        return ((ObservableCollection<GridItem>)getCollectionForCount(values[1], (int)values[4])).Count;
                    }
                }
            }
            if (values[0].ToString().Length > 0 && values[0] != null)
            {
                return getCollection(new object[] { values[0], values[2], values[3], (int)values[4] });
            }
            return null;
        }

        private List<string> getStrings(ObservableCollection<GridItem> item)
        {
            List<string> s = new List<string>();
            if (null != item)
            {
                foreach (GridItem i in item)
                {
                    if (null != i.Text)
                    {
                        s.Add(i.Text);
                    }
                }
            }
            return s;
        }

        private ObservableCollection<GridItem> manageLine1AlphaNumericKeys(object[] values, string parameter)
        {
            if (null != values[0] as ObservableCollection<GridItem> && ((ObservableCollection<GridItem>)values[0]).Count > 1)
            {
                return getCollection(new object[] { values[0], values[2], values[3], (int)values[4] }) as ObservableCollection<GridItem>;
            }
            var line1 = values[1] as ObservableCollection<GridItem>;
            if (null != line1)
            {
                if (parameter.ToString().ToLower().Equals("line1alpha") || parameter.ToString().ToLower().Equals("line1alphacount"))
                {
                    line1 = getLine1Keys(line1, true, (bool)values[2], (bool)values[3]);
                }
                else if (parameter.ToString().ToLower().Equals("line1numeric") || parameter.ToString().ToLower().Equals("line1numericcount"))
                {
                    line1 = getLine1Keys(line1, false, false, false);
                }
            }
            return line1;
        }

        private ObservableCollection<GridItem> manageLine1NumericKeys(object[] values, string parameter)
        {
            ObservableCollection<GridItem> tempItems = manageLine1AlphaNumericKeys(values, "line1numeric");
            string[] arrParams = parameter.Split(';');
            int column = 0;
            int row = 0;
            foreach (string param in arrParams)
            {
                if (param.ToLower().Contains("row=") && param.Split('=').Length > 0)
                {
                    int.TryParse(param.Split('=')[1], out row);
                }
                else if (param.ToLower().Contains("columns=") && param.Split('=').Length > 0)
                {
                    int.TryParse(param.Split('=')[1], out column);
                }
            }
            int y = (tempItems.Count % column) > 0 ? (tempItems.Count / column) + 1 : tempItems.Count / column;
            List<ObservableCollection<GridItem>> items = new List<ObservableCollection<GridItem>>();
            for (int i = 0; i < y; i++)
            {
                ObservableCollection<GridItem> tempItem = new ObservableCollection<GridItem>();
                for (int x = 0; x < column; x++)
                {
                    if (tempItems.Count > 0)
                    {
                        tempItem.Add(tempItems[0]);
                        tempItems.RemoveAt(0);
                    }
                }
                items.Add(tempItem);
            }
            if (items.Count > 0 && items.Count >= row - 1)
            {
                return items[row - 1];
            }
            return null;
        }

        private ObservableCollection<GridItem> getLine1Keys(ObservableCollection<GridItem> line1, bool isAlpha, bool shiftShown, bool isUpper)
        {
            var tempLine1 = new ObservableCollection<GridItem>();
            foreach (GridItem item in line1)
            {
                if (null == item.Text)
                    return tempLine1;

                int result = 0;
                if (isAlpha)
                {
                    if (!int.TryParse(item.Text, out result))
                    {
                        if (!shiftShown && !isUpper)
                        {
                            GridItem tempItem = item;
                            item.Text = tempItem.Text.ToLower();
                            tempLine1.Add(item);
                        }
                        else
                        {
                            tempLine1.Add(item);
                        }
                    }
                }
                else
                {
                    if (int.TryParse(item.Text, out result))
                    {
                        tempLine1.Add(item);
                    }
                }
            }
            return tempLine1;
        }

        private ObservableCollection<GridItem> getCollectionForCount(object values, int maxLimit)
        {
            ObservableCollection<GridItem> retVal = new ObservableCollection<GridItem>();
            ObservableCollection<GridItem> val = values as ObservableCollection<GridItem>;
            if (null != val)
            {
                foreach (GridItem item in val)
                {
                    if (null != item && item.Text.Length > 0 && val.IndexOf(item) < maxLimit)
                    {
                        retVal.Add(item);
                    }
                }
            }

            return retVal;
        }

        private ObservableCollection<GridItem> getCollection(object[] values)
        {
            bool shiftShown = (bool)values[1];
            bool isUpper = (bool)values[2];
            int maxLimit = (int)values[3];
            ObservableCollection<GridItem> retVal = new ObservableCollection<GridItem>();
            ObservableCollection<GridItem> val = values[0] as ObservableCollection<GridItem>;
            if (null != val)
            {
                foreach (GridItem item in val)
                {
                    if (null != item && item.Text.Length > 0 && val.IndexOf(item) < maxLimit)
                    {
                        if (!shiftShown && !isUpper)
                        {
                            GridItem tempItem = item;
                            item.Text = tempItem.Text.ToLower();
                            retVal.Add(item);
                        }
                        else
                        {
                            retVal.Add(item);
                        }
                    }
                }
            }

            return retVal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
