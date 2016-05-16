using System;

namespace SSCOUIModels.Models
{
    class Property
    {
        public Property(string name)
        {
            Name = name;
        }

        public Property(string name, string psxControl, string psxProperty, string type) : this(name)
        {
            PsxControl = psxControl;
            PsxProperty = psxProperty;
            ItemType = type;
        }

        public string Name { get; private set; }

        public string PsxControl { get; private set; }
        
        public string PsxProperty { get; private set; }

        public string ItemType { get; private set; }

        public object Value { get; internal set; }
    }
}
