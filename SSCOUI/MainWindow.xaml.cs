namespace SSCOUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Runtime.InteropServices;
    using System.Windows.Navigation;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using System.Diagnostics;
    using RPSWNET;
    using SSCOControls;
    using SSCOUI.Controls;
    using SSCOUIModels;
    using SSCOUIModels.Controls;
    using SSCOUIModels.Models;
    using SSCOUI.Properties;
    using System.Threading;
    using System.Reflection;
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
            {
                this.memoryLogTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(60000) };
                this.memoryLogTimer.Tick += memoryLogTimer_Tick;
                this.memoryLogTimer.Start();
            }
            this.DataContext = viewModel;
            InitializeComponent();
            this.MainAreaControl.FadeDuration = viewModel.ShowTransitionEffects ? new Duration(TimeSpan.FromMilliseconds(500)) : new Duration(TimeSpan.Zero);
            this.PopupControl.FadeDuration = this.MainAreaControl.FadeDuration;
            MouseTouchDevice.RegisterEvents(this);            
            this.DataContext = viewModel;    
        }

        private void CheckForDelayedLanguageChange()
        {
            if (((App)Application.Current).delayedLanguageChange && -1 == Array.IndexOf(App.LanguageChangeStateParams, this.viewModel.StateParam))
            {
                ((App)Application.Current).delayedLanguageChange = false;
                LocalizationManager.UpdateValues();
            }            
        }

        private void memoryLogTimer_Tick(object sender, EventArgs e)
        {
            long memUsage = Process.GetCurrentProcess().PrivateMemorySize64;
            // only logs over 1MB differences
            if (Math.Abs(lastMemUsage - memUsage) > 1048576)
            {
                lastMemUsage = memUsage;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskPerformance, "memoryLogTimer_Tick() [Performance] Memory usage: {0} KB", memUsage / 1024);
            }
        }

        private void OnStateChanged(string state)
        {
            if (null != this.currentView)
            {
                this.currentView.OnStateChanged(state);
            }
            if (null != this.currentPopupView)
            {
                this.currentPopupView.OnStateChanged(state);
            }           
            ShowControls();            
        }

        private void PopupExpired(object state)
        {
            this.popupTimer.Dispose();
            this.popupTimer = null;
            this.Dispatcher.Invoke((Action)(() =>
            {
                viewModel.ActionCommand.Execute("ViewModelSet(Context;)");
            }));
        }

        private void SetInitialFocus()
        {
            if (this.viewModel.UNav)
            {
                UIElement element = FocusManager.GetFocusedElement(this) as UIElement;
                if (null == element)
                {
                    FieldInfo mostRecentInputDeviceField = typeof(InputManager).GetMember("_mostRecentInputDevice", MemberTypes.Field,
                                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)[0] as FieldInfo;
                    mostRecentInputDeviceField.SetValue(InputManager.Current, InputManager.Current.PrimaryKeyboardDevice);
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
        }

        private void ShowControls()
        {
            viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.ShowControls, string.Empty);
            if (null != this.currentView)
            {
                this.CartControl.Visibility = this.currentView.ShowCart && (this.viewModel.State.Equals("InTransaction") ||
                    this.viewModel.State.Equals("InTransactionVoid") || this.viewModel.StoreMode) ? Visibility.Visible : Visibility.Collapsed;
                this.HeaderControl.Visibility = this.currentView.ShowHeader ? Visibility.Visible : Visibility.Collapsed;
                this.SystemFunctionsControl.Visibility = this.currentView.ShowSystemFunctions ? Visibility.Visible : Visibility.Collapsed;
                this.DecoratorControl.ShowIcons = this.currentView.ShowDecorator;
            }
            if (null != this.currentPopupView)
            {
                this.MainAreaControl.Visibility = this.currentPopupView.ShowBackground ? Visibility.Visible : Visibility.Collapsed;               
            }
            if (viewModel.Perfcheck.ShowControlsStarted)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                {
                    viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.ShowControls, string.Empty);
                }));
            }
        }
        
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ActiveStateParam"))
            {
                OnActiveStateParamChanged(this.viewModel.ActiveStateParam);
            }            
            else if (e.PropertyName.Equals("State"))
            {
                OnStateChanged(this.viewModel.State);
            }
            else if (e.PropertyName.Equals("StateParam"))
            {
                OnStateParamChanged(this.viewModel.StateParam);
            }
            else if (e.PropertyName.Equals("StoreMode"))
            {
                LocalizationManager.UpdateValues();
            }
            else if (e.PropertyName.Equals("UNav"))
            {
                ImageButton.UNav = this.viewModel.UNav;
                ImageToggleButton.UNav = this.viewModel.UNav;
                MeasureTextBlock.UNav = this.viewModel.UNav;
                KeyboardNavigation.SetTabNavigation(this, this.viewModel.UNav ? KeyboardNavigationMode.Cycle : KeyboardNavigationMode.None);
                KeyboardNavigation.SetDirectionalNavigation(this, this.viewModel.UNav ? KeyboardNavigationMode.Cycle : KeyboardNavigationMode.None);                
                if (this.viewModel.UNav)
                {
                    SetInitialFocus();                    
                }
                else
                {
                    Keyboard.ClearFocus();
                }
            }
            else
            {
                if (null != this.activeView)
                {
                    object value = this.viewModel.GetPropertyValue(e.PropertyName);
                    this.activeView.OnPropertyChanged(e.PropertyName, value);
                    if (null != this.currentView && this.activeView != this.currentView &&
                        this.viewModel.StateParam.Equals(this.viewModel.ActiveStateParam))
                    {
                        this.currentView.OnPropertyChanged(e.PropertyName, value);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
            OnActiveStateParamChanged(this.viewModel.ActiveStateParam);
            
            View view = this.activeView;
            if (view is PopupView)
            {
                OnActiveStateParamChanged(this.viewModel.BackgroundStateParam);
                OnStateParamChanged(this.viewModel.BackgroundStateParam);
                this.activeView = view;
            }
			ShowExtended();            
			OnStateParamChanged(this.viewModel.StateParam);        
		}

        private void ShowExtended()
        {
            if (System.Windows.Forms.Screen.AllScreens.Length > 1)
            {
                if (Settings.Default.ShowExtendedDisplay)
                {
                    System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[1];
                    System.Drawing.Rectangle rectangle = screen.WorkingArea;
                    exWindow = new ExtendedWindow(this.viewModel);
                    exWindow.Top = rectangle.Top;
                    exWindow.Left = rectangle.Left;
                    exWindow.Show();
                }
                else
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "Extended Window not supported. Configuration SupportExtendedDisplay is set to False");
                }                
            }
            else
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "System does not support Extended Window. SupportExtendedDisplay:{0}",
                    Settings.Default.ShowExtendedDisplay.ToString());
            }
        }

        private void Window_UnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.PropertyChanged -= new PropertyChangedEventHandler(ViewModel_PropertyChanged);
            if (null != this.popupTimer)
            {
                this.popupTimer.Dispose();
                this.popupTimer = null;
            }
            if (null != exWindow)
            {
                exWindow.Close();
            }
        }

        private void OnActiveStateParamChanged(string param)
        {
            if (param.Length > 0)
            {
                Context context;
                if (this.viewModel.ParamToViews.TryGetValue(param, out context) && null != context.View)
                {
                    this.activeView = App.GetCachedView(context.View, context.Primary);
                }
                else
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "MainWindow.ViewModel_PropertyChanged(ActiveStateParam) - View not configured for this state param:{0}",
                        param);
                }
            }
        }
        
        private void OnStateParamChanged(String param)
        {
            if (null != this.activeView)
            {
                var t = Environment.TickCount;
                viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.StateParamChanged, string.Format("OnStateParamChanged({0})", param));
                View view = this.activeView;
                if (view is PopupView)
                {
                    PopupView popupView = view as PopupView;
                    viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.ShowPopupView, popupView.ToString());
                    if (this.currentPopupView == null || !this.currentPopupView.GetType().Equals(popupView.GetType()))
                    {
                        if (null != this.currentPopupView)
                        {
                            PopupControl.ShowPopup(false, popupView, this.backgroundRendering);
                        }
                        if (popupView.Duration > 0 && null == popupTimer)
                        {
                            this.popupTimer = new Timer(new TimerCallback(PopupExpired), this, popupView.Duration, 0);
                        }
                        this.currentPopupView = popupView;
                        this.currentPopupView.OnStateChanged(this.viewModel.State);
                        this.currentPopupView.OnStateParamChanged(param);
                        PopupControl.ShowPopup(true, popupView, this.backgroundRendering);                                                
                    }
                    else
                    {
                        this.currentPopupView.OnStateParamChanged(param);
                    }
                }
                else
                {
                    if (currentPopupView != null)
                    {
                        this.currentPopupView = null;
                        this.PopupControl.ShowPopup(false, null, this.backgroundRendering);
                    }
                    this.MainAreaControl.Visibility = Visibility.Visible;
                    if (this.currentView == null || !this.currentView.GetType().Equals(view.GetType()))
                    {
                        if (null != this.currentView)
                        {
                            this.currentView.Show(false);
                        }
                        this.currentView = view as BackgroundView;
                        this.currentView.OnStateChanged(this.viewModel.State);
                        this.currentView.OnStateParamChanged(param);
                        this.MainAreaControl.Navigate(this.currentView);
                        this.backgroundRendering = true;
                        this.currentView.Show(true);
                    }
                    else
                    {
                        this.currentView.OnStateParamChanged(param);
                    }
                    viewModel.ActionCommand.Execute(String.Format("ViewModelSet(BackgroundStateParam;{0})", param));
                }
                ShowControls();
                if (viewModel.Perfcheck.SetContextStarted ||
                    viewModel.Perfcheck.ShowPopupViewStarted ||
                    viewModel.Perfcheck.StateParamChangedStarted)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.SetContext, "Done rendering.");
                        viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.ShowPopupView);
                        viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.StateParamChanged, string.Format("took {0} ms", Environment.TickCount - t));
                    }));
                }                
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "MainWindow.OnStateParamChanged({0}) - StateParam={1}; BackgroundStateParam={2}",
                    param, viewModel.StateParam, viewModel.BackgroundStateParam);
            }            
        }
        
        private void MainAreaControl_ContentRendered(object sender, EventArgs e)
        {
            if (this.backgroundRendering && null != this.currentPopupView)
            {
                this.PopupControl.DisableControls(this.currentPopupView);
            }                            
            this.backgroundRendering = false;
            CheckForDelayedLanguageChange();
        }

        private View activeView;
        private bool backgroundRendering;
        private PopupView currentPopupView;
        private BackgroundView currentView;
        private ExtendedWindow exWindow;
        private long lastMemUsage = 0;
        private DispatcherTimer memoryLogTimer;        
        private Timer popupTimer;
        private IMainViewModel viewModel;       
    }
}
