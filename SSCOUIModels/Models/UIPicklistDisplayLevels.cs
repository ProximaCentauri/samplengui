using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCOUIModels.Models
{
    public class UIPicklistDisplayLevels
    {
        public int TabSelected
        {
            get { return tabSelected; }
        }

        public string CurrentCategory
        {
            get { return currentCategory; }
        }

        public string SearchKey
        {
            get { return searchKey; }
        }

        public UIPicklistDisplayLevels()
        {
        }

        public UIPicklistDisplayLevels(string displayLevels) 
        {
            string[] properties = displayLevels.Split(FieldPropertySeparator);
            foreach (string property in properties)
            {
                string[] nameValue = property.Split(FieldValueSeparator);

                switch (nameValue[0])
                {
                    case KeyTabSelected:
                        int.TryParse(nameValue[1], out tabSelected);
                        break;
                    case KeyCurrentCategory:
                        currentCategory = nameValue[1];
                        break;
                    case KeySearchKey:
                        searchKey = nameValue[1];
                        break;
                    default:
                        break;
                }
            }
        }

        private int tabSelected = 0;
        private string currentCategory;
        private string searchKey;

        private const char FieldValueSeparator = '=';
        private const char FieldPropertySeparator = ';';

        private const string KeyTabSelected = "TabSelected";
        private const string KeyCurrentCategory = "CurrentCategory";
        private const string KeySearchKey = "SearchKey";
    }
}
