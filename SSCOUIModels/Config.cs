using System;
using System.Configuration;
using System.Text;

namespace SSCOUIModels
{
    internal class ConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ContextToViews", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ContextToViewsCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ContextToViewsCollection ContextToViews
        {
            get
            {
                ContextToViewsCollection contextToViewsCollection = (ContextToViewsCollection)base["ContextToViews"];
                return contextToViewsCollection;
            }
        }

        [ConfigurationProperty("ActionToSequences", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ActionToSequencesCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ActionToSequencesCollection ActionToSequences
        {
            get
            {
                ActionToSequencesCollection actionToSequencesCollection = (ActionToSequencesCollection)base["ActionToSequences"];
                return actionToSequencesCollection;
            }
        }

        [ConfigurationProperty("PropertyWatches", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PropertyWatchesCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public PropertyWatchesCollection PropertyWatches
        {
            get
            {
                PropertyWatchesCollection propertyWatchesCollection = (PropertyWatchesCollection)base["PropertyWatches"];
                return propertyWatchesCollection;
            }
        }
    }

    internal class ContextToViewsCollection : ConfigurationElementCollection
    {
        public ContextToViewsCollection()
        {            
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ContextToViewElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ContextToViewElement)element).Context;
        }

        public ContextToViewElement this[int index]
        {
            get
            {
                return (ContextToViewElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ContextToViewElement this[string Context]
        {
            get
            {
                return (ContextToViewElement)BaseGet(Context);
            }
        }

        public int IndexOf(ContextToViewElement contextToView)
        {
            return BaseIndexOf(contextToView);
        }

        public void Add(ContextToViewElement contextToView)
        {
            BaseAdd(contextToView);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ContextToViewElement contextToView)
        {
            if (BaseIndexOf(contextToView) >= 0)
            {
                BaseRemove(contextToView.Context);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string param)
        {
            BaseRemove(param);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    internal class ContextToViewElement : ConfigurationElement
    {
        public ContextToViewElement()
        {                     
        }

        [ConfigurationProperty("context", IsRequired = true, IsKey = true)]
        public string Context
        {
            get
            {
                return (string)this["context"];
            }
            set
            {
                this["context"] = value;
            }
        }

        [ConfigurationProperty("view", IsRequired = false)]
        public string View
        {
            get
            {
                return (string)this["view"];
            }
            set
            {
                this["view"] = value;
            }
        }      

        [ConfigurationProperty("param", IsRequired = false)]
        public string Param
        {
            get
            {
                return (string)this["param"];
            }
            set
            {
                this["param"] = value;
            }
        }

        [ConfigurationProperty("action", IsRequired = false)]
        public string Action
        {
            get
            {
                return (string)this["action"];
            }
            set
            {
                this["action"] = value;
            }
        }

        [ConfigurationProperty("primary", IsRequired = false, DefaultValue = false)]
        public bool Primary
        {
            get
            {
                return (bool)this["primary"];
            }
            set
            {
                this["primary"] = value;
            }
        }
    }

    internal class ActionToSequencesCollection : ConfigurationElementCollection
    {
        public ActionToSequencesCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ActionToSequenceElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ActionToSequenceElement)element).Action;
        }

        public ActionToSequenceElement this[int index]
        {
            get
            {
                return (ActionToSequenceElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ActionToSequenceElement this[string Action]
        {
            get
            {
                return (ActionToSequenceElement)BaseGet(Action);
            }
        }

        public int IndexOf(ActionToSequenceElement actionToSequence)
        {
            return BaseIndexOf(actionToSequence);
        }

        public void Add(ActionToSequenceElement actionToSequence)
        {
            BaseAdd(actionToSequence);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ActionToSequenceElement actionToSequence)
        {
            if (BaseIndexOf(actionToSequence) >= 0)
            {
                BaseRemove(actionToSequence.Action);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string param)
        {
            BaseRemove(param);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    internal class ActionToSequenceElement : ConfigurationElement
    {
        public ActionToSequenceElement()
        {            
        }

        [ConfigurationProperty("action", IsRequired = true, IsKey = true)]
        public string Action
        {
            get
            {
                return (string)this["action"];
            }
            set
            {
                this["action"] = value;
            }
        }

        [ConfigurationProperty("sequence", IsRequired = true)]
        public string Sequence
        {
            get
            {
                return (string)this["sequence"];
            }
            set
            {
                this["sequence"] = value;
            }
        }        
    }

    internal class PropertyWatchesCollection : ConfigurationElementCollection
    {
        public PropertyWatchesCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyWatchElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyWatchElement)element).Key;
        }

        public PropertyWatchElement this[int index]
        {
            get
            {
                return (PropertyWatchElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public PropertyWatchElement this[string Key]
        {
            get
            {
                return (PropertyWatchElement)BaseGet(Key);
            }
        }

        public int IndexOf(PropertyWatchElement propertyWatch)
        {
            return BaseIndexOf(propertyWatch);
        }

        public void Add(PropertyWatchElement propertyWatch)
        {
            BaseAdd(propertyWatch);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(PropertyWatchElement propertyWatch)
        {
            if (BaseIndexOf(propertyWatch) >= 0)
            {
                BaseRemove(propertyWatch.Key);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string param)
        {
            BaseRemove(param);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    internal class PropertyWatchElement : ConfigurationElement
    {
        public PropertyWatchElement()
        {
        }

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("control", IsRequired = true)]
        public string Control
        {
            get
            {
                return (string)this["control"];
            }
            set
            {
                this["control"] = value;
            }
        }

        [ConfigurationProperty("property", IsRequired = true)]
        public string Property
        {
            get
            {
                return (string)this["property"];
            }
            set
            {
                this["property"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        internal string Key
        {
            get
            {
                StringBuilder keyBuilder = new StringBuilder(Name);
                keyBuilder.Append(Control);
                keyBuilder.Append(Property);
                return keyBuilder.ToString();
            }
        }
    }
}
