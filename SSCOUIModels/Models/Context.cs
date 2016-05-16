using System;

namespace SSCOUIModels.Models
{
    public class Context
    {
        public Context(string name, Type view, string param, string action, bool primary)
        {
            Name = name;
            View = view;
            Param = param;           
            Action = action;
            Primary = primary;
        }

        public string Action { get; private set; }

        public string Name { get; private set; }
        
        public string Param { get; private set; }

        public bool Primary { get; private set; }

        public Type View { get; private set; }
    }
}
