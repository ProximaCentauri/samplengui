namespace SSCOUIModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Dynamic;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Linq;
    using SSCOUIModels.Commands;
    using SSCOUIModels.Models;
    using SSCOUIModels.Controls;
    using SSCOUIModels.Properties;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using FPsxWPF;
    using RPSWNET;
    using SSCOControls;
    
    public class MainViewModel : DynamicObject, IMainViewModel
    {
        public MainViewModel() : this(new FPsx(Application.Current.Dispatcher, Settings.Default.RemoteSendInterval, Settings.Default.RemoteKeepAliveInterval))
        {
        }

        public MainViewModel(FPsx fpsxInstance)
        {
            fpsx = fpsxInstance;
            fpsx.RemoteConnect += new FPsxEventHandler(FPsxRemoteConnect);
            fpsx.RemoteDisconnect += new FPsxEventHandler(FPsxRemoteError);
            fpsx.RemoteError += new FPsxEventHandler(FPsxRemoteError);
            fpsx.DisplayCreated += new FPsxEventHandler(FPsxDisplayCreated);            
            fpsx.LanguageChange += new FPsxEventHandler(FPsxLanguageChange);
            
            ActionCommand = new ActionCommand(this);
            Perfcheck = new PerfCheck();

            properties.Add("ActiveStateParam", new Property("ActiveStateParam"));
            properties.Add("AttendantMode", new Property("AttendantMode"));
            properties.Add("BackgroundStateParam", new Property("BackgroundStateParam"));
            properties.Add("CurrentItem", new Property("CurrentItem"));
            properties.Add("CustomerBackgroundStateParam", new Property("CustomerBackgroundStateParam"));
            properties.Add("CustomerLanguage", new Property("CustomerLanguage"));
            properties.Add("DegradedMode", new Property("DegradedMode"));
            properties.Add("Language", new Property("Language"));
            properties.Add("State", new Property("State"));
            properties.Add("StateParam", new Property("StateParam"));
            properties.Add("StoreBackgroundStateParam", new Property("StoreBackgroundStateParam"));
            properties.Add("StoreMode", new Property("StoreMode"));
            properties.Add("TrainingMode", new Property("TrainingMode"));
            properties.Add("UIEchoField", new Property("UIEchoField"));
            properties.Add("TBState", new Property("TBState"));
            properties.Add("UIPicklistDisplayLevels", new Property("UIPicklistDisplayLevels"));
            properties.Add("UNav", new Property("UNav"));
            LoadFromConfig();
                        
            Clear();
            fpsx.ConnectProxy((int)Settings.Default.RemoteProxyPort, Settings.Default.RemoteProxyHost);
        }

        /// <summary>
        /// Releases the resources used by the instance.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (null != timer)
                {
                    timer.Dispose();
                    timer = null;
                }
                fpsx.Dispose();
            }
        }

        public object GetPropertyValue(string propertyName)
        {
            return this.properties.ContainsKey(propertyName) ? this.properties[propertyName].Value : null;            
        }
                               
        /// <summary>
        /// Called when the remote PSX Context changes.
        /// </summary>
        /// <param name="name">
        /// The name of the new Context.
        /// </param>
        public void SetPsxContext(string name)
        {
            if (this.firstStateChange && !name.Equals(InitialStateParam))
            {
                this.delayedContextChange = name;
                return;
            }
            Perfcheck.StartEventLog(PerfMeasureEvents.SetContext, string.Format("MainViewModel.SetPsxContext({0})", name));
            if (0 == name.Length && BackgroundStateParam.Length > 0 && !BackgroundStateParam.Equals(StateParam))
            {
                // Return from Popup StateParam to Background StateParam //
                SetPropertyValue("ActiveStateParam", this.BackgroundStateParam); 
                SetPropertyValue("StateParam", this.BackgroundStateParam);                
            }
            else
            {
                Context context = GetContextFromName(name);
                if (null != context)
                {
                    if (this.delayedState.Length > 0)
                    {
                        ChangeState(this.delayedState);
                        this.delayedState = String.Empty;
                    }
                    if (context.Param.Length > 0)
                    {
                        SetPropertyValue("ActiveStateParam", context.Param); 
                        SetPropertyValue("StateParam", context.Param);
                    }
                    if (context.Action.Length > 0)
                    {
                        this.fpsxDisplay.Sequences.Execute(context.Action);
                    }
                }
                else
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "MainViewModel.SetPsxContext({0}) - Unknown context, State unchanged", name);
                }                               
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool ret = base.TryGetMember(binder, out result);
            if (!ret)
            {
                result = GetPropertyValue(binder.Name);
                if (result != null)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!base.TrySetMember(binder, value))
            {
                SetPropertyValue(binder.Name, value);
            }
            return true;
        }

        public string ActiveStateParam
        {
            get
            {
                return GetPropertyValue("ActiveStateParam") as String;
            }
        }

        public ICommand ActionCommand { get; set; }

        public bool AttendantMode
        {
            get
            {
                return Convert.ToBoolean(GetPropertyValue("AttendantMode"));
            }
        }

        public string BackgroundStateParam
        {
            get
            {
                return GetPropertyValue("BackgroundStateParam") as String;
            }
        }

        public Dictionary<string, Context> ParamToViews
        {
            get
            {
                return this.paramToViews;
            }
        }
       
        public CustomerReceiptItem CurrentItem
        {
            get
            {
                return GetPropertyValue("CurrentItem") as CustomerReceiptItem;
            }
        }

        public string CustomerBackgroundStateParam
        {
            get
            {
                return GetPropertyValue("CustomerBackgroundStateParam") as String;
            }
        }

        public int CustomerLanguage
        {
            get
            {
                object language = GetPropertyValue("CustomerLanguage");
                return null != language ? (int)language : 0;
            }
        }

        public UIEchoField UIEchoField
        {
            get
            {
                return GetPropertyValue("UIEchoField") as UIEchoField;
            }
        }

        public UIPicklistDisplayLevels UIPicklistDisplayLevels
        {
            get
            {
                return GetPropertyValue("UIPicklistDisplayLevels") as UIPicklistDisplayLevels;
            }
        }

        public string DegradedMode
        {
            get
            {
                return GetPropertyValue("DegradedMode").ToString();
            }
        }

        public int Language
        {
            get
            {
                object language = GetPropertyValue("Language");
                return null != language ? (int)language : 0;
            }
        }

        public bool ShowTransitionEffects { get; set; }

        public string State
        {
            get
            {
                return GetPropertyValue("State") as String;
            }
        }

        public string StateParam
        {
            get
            {
                return GetPropertyValue("StateParam") as String;
            }
        }

        public string StoreBackgroundStateParam
        {
            get
            {
                return GetPropertyValue("StoreBackgroundStateParam") as String;
            }
        }

        public bool StoreMode
        {
            get
            {
                return Convert.ToBoolean(GetPropertyValue("StoreMode"));
            }
        }

        public bool TrainingMode
        {
            get
            {
                return Convert.ToBoolean(GetPropertyValue("TrainingMode"));
            }
        }   
     
        public string TBState
        {
            get
            {
                return GetPropertyValue("TBState") as string;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public PerfCheck Perfcheck { get; set; }

        public bool UNav
        {
            get
            {
                return Convert.ToBoolean(GetPropertyValue("UNav"));
            }
        }

        internal void SetPropertyValue(string propertyName, object value)
        {
            if (properties.ContainsKey(propertyName) && !PropertyEquals(propertyName, value))
            {
                object previousValue = properties[propertyName].Value;
                properties[propertyName].Value = value;
                OnPropertyChanged(propertyName, previousValue);
            }
        }
       
        private void ChangeState(string state)
        {
            bool isStore = this.StoreMode;
            SetPropertyValue("StoreMode", state.Equals("Store"));
            SetPropertyValue("State", state);            
            string backgroundStateParam = this.StoreMode ? this.StoreBackgroundStateParam : this.CustomerBackgroundStateParam;
            bool isFirst = this.firstStateChange && (state.Equals(InitialSCOState) || backgroundStateParam.Length > 0);
            if (isFirst)
            {
                this.firstStateChange = false;                    
            }
            if (isFirst || isStore != this.StoreMode)
            {
                if (backgroundStateParam.Length > 0)
                {
                    SetPropertyValue("ActiveStateParam", backgroundStateParam);
                    SetPropertyValue("StateParam", backgroundStateParam);
                }
            }
            if (!this.firstStateChange && this.delayedContextChange.Length > 0)
            {
                SetPsxContext(this.delayedContextChange);
                this.delayedContextChange = String.Empty;
            }
        }

        private void Clear()
        {
            if (null != this.fpsxDisplay)
            {
                this.fpsxDisplay.PropertyChange -= new FPsxEventHandler(FPsxPropertyChange);
            }
            this.fpsxDisplay = null;

            CultureInfo primaryCulture = new CultureInfo(Settings.Default.PrimaryLanguage);
            Application.Current.Dispatcher.Thread.CurrentUICulture = primaryCulture;
            LocalizationManager.SecondaryCultureInfo = primaryCulture;
                        
            SetPropertyValue("AttendantMode", false);
            SetPropertyValue("BackgroundStateParam", String.Empty);
            SetPropertyValue("CurrentItem", new CustomerReceiptItem());
            SetPropertyValue("CustomerBackgroundStateParam", String.Empty);            
            SetPropertyValue("CustomerLanguage", Application.Current.Dispatcher.Thread.CurrentUICulture.LCID);            
            SetPropertyValue("DegradedMode", String.Empty);
            SetPropertyValue("Language", Application.Current.Dispatcher.Thread.CurrentUICulture.LCID);
            SetPropertyValue("StateParam", String.Empty);
            SetPropertyValue("StoreBackgroundStateParam", InitialStoreBackgroundContext);
            SetPropertyValue("StoreMode", false);
            SetPropertyValue("TrainingMode", false);
            SetPropertyValue("UIEchoField", new UIEchoField());
            SetPropertyValue("TBState", String.Empty);
            SetPropertyValue("UIPicklistDisplayLevels", new UIPicklistDisplayLevels());
            SetPropertyValue("UNav", false);

            SetPropertyValue("State", String.Empty);            
            SetPsxContext(InitialStateParam);

            this.delayedContextChange = String.Empty;
            this.delayedState = String.Empty;
            this.firstStateChange = true;
        }

        private void FPsxDisplayCreated(object sender, FPsxEventArgs e)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "MainViewModel.FPsxDisplayCreated({0})", e.DisplayName);
            fpsxDisplay = fpsx.GetDisplay(e.DisplayName);
            fpsxDisplay.Notify = this;            
            fpsxDisplay.PropertyChange += new FPsxEventHandler(FPsxPropertyChange);
            ((ActionCommand)ActionCommand).Sequences = fpsxDisplay.Sequences;
            List<string> includePsxControls = new List<string>();
            foreach (Property properties in watches.Values)
            {
                if (properties.PsxProperty.Equals("GridData"))
                {
                    includePsxControls.Add(properties.PsxControl);
                    SetPropertyValue(properties.Name, fpsxDisplay.GetGrid(properties.PsxControl));
                }
                else if (properties.PsxProperty.Equals("ReceiptData"))
                {
                    includePsxControls.Add(properties.PsxControl);
                    Type itemType = null;
                    Type subItemType = null;
                    if (properties.ItemType.Length > 0)
                    {
                        itemType = Type.GetType("SSCOUIModels.Models." + properties.ItemType + "ReceiptItem");
                        subItemType = Type.GetType("SSCOUIModels.Models." + properties.ItemType + "ReceiptSubItem");                            
                    }
                    SetPropertyValue(properties.Name, fpsxDisplay.GetReceipt(properties.PsxControl, itemType, subItemType).Items);
                }
                else if (properties.PsxProperty.Equals("Toggle"))
                {
                    fpsxDisplay.AddWatch(properties.PsxControl, "State");
                }
                else
                {
                    fpsxDisplay.AddWatch(properties.PsxControl, properties.PsxProperty);                        
                }                    
            }
            fpsxDisplay.IgnoreControls(includePsxControls, false);                                
            fpsxDisplay.AddWatch(PsxWatchCurrentItem, "Variable");
            fpsxDisplay.AddWatch(PsxWatchCustomerBackgroundContext, "Variable");
            fpsxDisplay.AddWatch(PsxWatchStoreBackgroundContext, "Variable");
            fpsxDisplay.AddWatch(PsxWatchCustomerLanguage, "Variable");            
            fpsxDisplay.AddWatch(PsxWatchMode, "Variable");
            fpsxDisplay.AddWatch(PsxWatchSetFrame, "Variable");
            fpsxDisplay.AddWatch(PsxWatchState, "Variable");
            fpsxDisplay.AddWatch(PsxWatchUIEchoField, "Variable");
            fpsxDisplay.AddWatch(PsxWatchTBState, "Variable");
            fpsxDisplay.AddWatch(PsxWatchUIPicklistDisplayLevels, "Variable");
                
            SetBackgroundContexts();
        }

        private void FPsxLanguageChange(object sender, FPsxEventArgs e)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "MainViewModel.FPsxLanguageChange({0})", e.Value);
            SetPropertyValue("Language", e.Value);              
        }
        
        private void FPsxPropertyChange(object sender, FPsxEventArgs e)
        {
            CmDataCapture.MaskedCaptureFormat(CmDataCapture.MaskExtensive, e.ControlName, StateParam, "MainViewModel.FPsxPropertyChange({0}, {1}, {2})", e.ControlName, e.PropertyName, e.Value);
            if (e.ControlName.Equals(PsxWatchSetFrame))
            {
                Context context = GetContextFromName(e.Value.ToString());                
                if (null != context && context.Param.Length > 0)
                {
                    SetPropertyValue("ActiveStateParam", context.Param);    
                }
            }            
            else if (e.ControlName.Equals(PsxWatchCurrentItem))
            {
                SetPropertyValue("CurrentItem", new CustomerReceiptItem(e.Value.ToString()));
            }
            else if (e.ControlName.Equals(PsxWatchCustomerBackgroundContext))
            {
                Context context = GetContextFromName(e.Value.ToString());
                if (null != context && context.Param.Length > 0)
                {
                    SetPropertyValue("CustomerBackgroundStateParam", context.Param);
                    if (this.firstStateChange && this.State.Length > 0)
                    {
                        ChangeState(this.State);
                    }
                }
            }
            else if (e.ControlName.Equals(PsxWatchCustomerLanguage))
            {
                SetPropertyValue("CustomerLanguage", Int32.Parse(e.Value.ToString()));
            }           
            else if (e.ControlName.Equals(PsxWatchUIEchoField))
            {
                SetPropertyValue("UIEchoField", new UIEchoField(e.Value.ToString()));
            }
            else if (e.ControlName.Equals(PsxWatchUIPicklistDisplayLevels))
            {
                SetPropertyValue("UIPicklistDisplayLevels", new UIPicklistDisplayLevels(e.Value.ToString()));
            }
            else if (e.ControlName.Equals(PsxWatchMode))
            {
                string[] properties = e.Value.ToString().Split(KeyValueSeparator);
                foreach (string property in properties)
                {
                    string[] nameValue = property.Split(ValueSeparator);
                    if (2 == nameValue.Length)
                    {
                        if (nameValue[0].Equals(PsxKeyAttendantMode))
                        {
                            SetPropertyValue("AttendantMode", Boolean.Parse(nameValue[1]));
                        }
                        else if (nameValue[0].Equals(PsxKeyTrainingMode))
                        {
                            SetPropertyValue("TrainingMode", Boolean.Parse(nameValue[1]));
                        }
                        else if (nameValue[0].Equals(PsxKeyDegradedMode))
                        {
                            SetPropertyValue("DegradedMode", nameValue[1].Equals("false") ? String.Empty : nameValue[1]);
                        }
                    }
                }
            }
            else if (e.ControlName.Equals(PsxWatchState))
            {
                string newState = e.Value.ToString();
                newState = newState.Length > 0 ? newState : InitialSCOState; 
                if (this.firstStateChange)
                {
                    ChangeState(newState);
                }
                else
                {
                    this.delayedState = newState;
                }
            }
            else if (e.ControlName.Equals(PsxWatchStoreBackgroundContext))
            {
                Context context = GetContextFromName(e.Value.ToString());
                if (null != context && context.Param.Length > 0)
                {
                    SetPropertyValue("StoreBackgroundStateParam", context.Param);
                    if (this.firstStateChange && this.State.Length > 0)
                    {
                        ChangeState(this.State);
                    }
                }
            }
            else if (e.ControlName.Equals(PsxWatchTBState))
            {
                TBStateType tbState = (TBStateType)Enum.Parse(typeof(TBStateType), e.Value.ToString());
                SetPropertyValue("TBState", tbState.ToString());
            }                 
            else
            {
                Property watch;
                if (watches.TryGetValue(e.ControlName + e.PropertyName, out watch))
                {
                    if (watch.PsxProperty.Equals("State"))
                    {
                        watch.Value = !e.Value.ToString().Equals("2");
                    }
                    else if (watch.PsxProperty.Equals("Visible"))
                    {
                        watch.Value = FPsx.ConvertFromOleBool(e.Value.ToString());
                    }
                    else
                    {
                        watch.Value = e.Value;
                    }
                    OnPropertyChanged(watch.Name, null);
                }
                else if (watches.TryGetValue(e.ControlName + "Toggle", out watch))
                {
                    watch.Value = e.Value.ToString().Equals("1");
                    OnPropertyChanged(watch.Name, null);
                }
                else
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "MainViewModel.FPsxPropertyChange() - Could not find PropertyWatch for {0}", e.ControlName + e.PropertyName);
                }
            }
        }

        private void FPsxRemoteConnect(object sender, FPsxEventArgs e)
        {
            CmDataCapture.Capture(CmDataCapture.MaskExtensive, "MainViewModel.FPsxRemoteConnect()");
            if (null != timer)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void FPsxRemoteError(object sender, FPsxEventArgs e)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning | CmDataCapture.MaskExtensive, "MainViewModel.FPsxRemoteError() - {0}", e.Value);
            if (!disposed && null != Application.Current)
            {
                if (!StateParam.Equals(InitialStateParam))
                {
                    Clear();                                       
                }
                if (null == timer)
                {
                    timer = new Timer(new TimerCallback(TimerHandler), null, Settings.Default.ConnectionPeriod, 0);
                }
            }
        }

        private Context GetContextFromName(string name)
        {
            Context matchedContext = null;
            foreach (Context context in this.contexts)
            {
                if (context.Name[0].Equals('^') ? Regex.IsMatch(name, context.Name) : name.Equals(context.Name))
                {
                    matchedContext = context;
                    break;
                }
            }
            return matchedContext;
        }

        private object GetInitialValue(string property, string control)
        {
            object value = null;
            if (property.Equals("GridData") || property.Equals("ReceiptData"))
            {
                value = null;
            }
            else if (property.Equals("State") || property.Equals("Visible") || property.Equals("Toggle"))
            {
                value = false;
            }
            else
            {
                value = String.Empty;
            }
            return value;
        }

        private void LoadFromConfig()
        {
            ConfigSection configSection = null;
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configSection = configFile.GetSection("ConfigurationSettingsModels") as ConfigSection;                
            }
            catch (Exception caught)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "MainViewModel.LoadFromConfig() - Unable to load ConfigurationSettings from config file({0})", caught.Message);
                return;
            }
            if (configSection == null) return;
            ContextToViewElement element;
            Type viewType;
            Context context;
            for (int i = 0; i < configSection.ContextToViews.Count; i++)
            {
                element = configSection.ContextToViews[i];
                viewType = Type.GetType("SSCOUIViews.Views." + element.View + ", SSCOUIViews");
                context = new Context(element.Context, viewType, element.Param, element.Action, element.Primary);
                this.contexts.Add(context);
                if (element.Param != null && element.Param.Length > 0 && !this.paramToViews.ContainsKey(element.Param))
                {
                    this.paramToViews.Add(element.Param, context);  
                }
            }
            ActionToSequenceElement element2;
            for (int i = 0; i < configSection.ActionToSequences.Count; i++)
            {
                element2 = configSection.ActionToSequences[i];
                ((ActionCommand)ActionCommand).actionsToPsxSequences.Add(element2.Action, element2.Sequence);
            }
            PropertyWatchElement element3;
            Property watch;
            for (int i = 0; i < configSection.PropertyWatches.Count; i++)
            {
                element3 = configSection.PropertyWatches[i];
                watch = new Property(element3.Name, element3.Control, element3.Property, element3.Type);
                watches.Add(element3.Control + element3.Property, watch);
                if (element3.Name.Length > 0)
                {
                    properties.Add(element3.Name, watch);
                    SetPropertyValue(element3.Name, GetInitialValue(element3.Property, element3.Control));
                }                
            }
        }

        private void OnPropertyChanged(string propertyName, object previousValue)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetBackgroundContexts()
        {
            if (this.fpsxDisplay != null)
            {
                List<string> contextNames = new List<string>();
                foreach (Context context in this.contexts)
                {
                    if (context.Param.Length > 0 && context.View != null && typeof(BackgroundView).IsAssignableFrom(context.View))
                    {
                        contextNames.Add(context.Name);
                    }
                }
                if (contextNames.Count > 0)
                {
                    this.fpsxDisplay.SetBackgroundContexts(String.Join(",", contextNames.ToArray()));
                }
            }
        }
                        
        private void TimerHandler(object state)
        {
            timer = null;
            if (!fpsx.ProxyConnected)
            {
                fpsx.ConnectProxy((int)Settings.Default.RemoteProxyPort, Settings.Default.RemoteProxyHost);
            }
        }

        /// <summary>
        /// Check if a property value equals the value in the parameter
        /// </summary>
        /// <param name="propertyName">The name of a property.</param>
        /// <param name="value">The value to compare.</param>
        /// <returns>True if the property value equals the value passed in, false otherwise.</returns>
        private bool PropertyEquals(string propertyName, object value)
        {
            object propertyValue = this.properties[propertyName].Value;
            if (propertyValue == null)
            {
                return value == null;
            }
            return propertyValue.Equals(value);
        }

        private const char KeyValueSeparator = ';';
        private const char ValueSeparator = '=';
        private const string InitialSCOState = "Loading";
        private const string InitialStateParam = "Disconnected";
        private const string InitialStoreBackgroundContext = "SmAuthorization";
        private const string PsxKeyAttendantMode = "attendant";
        private const string PsxKeyDegradedMode = "degraded";
        private const string PsxKeyTrainingMode = "training";        
        private const string PsxWatchCurrentItem = "NextGenUICurrentItem";
        private const string PsxWatchCustomerBackgroundContext = "NextGenUICustomerBackgroundContext";
        private const string PsxWatchCustomerLanguage = "NextGenUICustomerLanguage";        
        private const string PsxWatchMode = "NextGenUIMode";
        private const string PsxWatchSetFrame = "NextGenUISetFrame";
        private const string PsxWatchState = "NextGenUIApplicationState";
        private const string PsxWatchStoreBackgroundContext = "NextGenUIStoreBackgroundContext";
        private const string PsxWatchUIEchoField = "NextGenUIEchoField";
        private const string PsxWatchTBState = "NextGenUITBLastState";
        private const string PsxWatchUIPicklistDisplayLevels = "NextGenUIPicklistDisplayLevels";
        private List<Context> contexts = new List<Context>();
        private string delayedContextChange = String.Empty;
        private string delayedState = String.Empty;
        private bool disposed;
        private bool firstStateChange = true;
        private FPsx fpsx;
        private Display fpsxDisplay;
        private Dictionary<string, Context> paramToViews = new Dictionary<string, Context>();
        private Dictionary<string, Property> properties = new Dictionary<string, Property>();        
        private Timer timer;        
        private Dictionary<string, Property> watches = new Dictionary<string, Property>();        
    }
}