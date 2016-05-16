namespace SSCOUI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Navigation;
    using System.Windows.Media.Animation;
    using SSCOUIModels.Controls;
    using SSCOControls;

    /// <summary>
    /// Interaction logic for PopupControl.xaml
    /// </summary>
    public partial class PopupControl : UserControl
    {
        public PopupControl()
        {
            InitializeComponent();            
        }

        public void DisableControls(PopupView view)
        {
            Window parent = Window.GetWindow(this);
            EnableControls(parent);
            ScanAndDisableControls(parent, view);
        }

        public void ShowPopup(bool isShowing, PopupView popupView, bool deferDisable)
        {
            this.deferDisable = deferDisable;
            if (isShowing)
            {
                if (0.0 == this.PopupFrame.Opacity)
                {
                    this.currentPopupView = popupView;
                    this.currentPopupView.ControlsToAllowEnableChanged += new EventHandler(CurrentPopupView_ControlsToAllowEnableChanged);
                    if (this.popupRendering)
                    {
                        this.PopupFrame.StopLoading();
                    }
                    this.popupRendering = true;                                        
                    if (null == this.PopupFrame.Content || !this.PopupFrame.Content.GetType().Equals(popupView.GetType()))
                    {
                        this.PopupFrame.Navigate(popupView);
                    }
                    else
                    {
                        PopupFrame_ContentRendered(this.PopupFrame, EventArgs.Empty);
                    }
                    ManageShroudEffect(deferDisable);
                }
                else
                {
                    this.pendingPopup = popupView;
                }
            }
            else
            {
                if (this.currentPopupView != null)
                {
                    this.currentPopupView.ControlsToAllowEnableChanged -= new EventHandler(CurrentPopupView_ControlsToAllowEnableChanged);
                    this.popupToClose = this.currentPopupView;
                    this.currentPopupView = null;
                }
                if (popupView == null)
                {
                    ManageShroudEffect(deferDisable);
                }
                Show(false);
                this.pendingPopup = null;
            }
        }

        /// <summary>
        /// FadeDuration will be used as the duration for Fade Out and Fade In animations
        /// </summary>
        public Duration FadeDuration
        {
            get
            {
                return (Duration)GetValue(FadeDurationProperty);
            }
            set
            {
                SetValue(FadeDurationProperty, value);
            }
        }

        public static readonly DependencyProperty FadeDurationProperty = DependencyProperty.Register("FadeDuration", typeof(Duration), typeof(PopupControl),
            new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

        private void CurrentPopupView_ControlsToAllowEnableChanged(object sender, EventArgs e)
        {
            DisableControls(sender as PopupView);
        }

        /// <summary>Scan all the controls in the sub-VisualTree staring at node "depObj", and disable the appropriate ones
        /// A node should be disabled only if it does not satisfy neither of the following two criteria:
        /// (1) it is not configured to be enabled in popupView
        /// (2) none of its subnode is configured to be enabled in popupView
        /// </summary>
        /// <param name="depObj">A node in the visual tree</param>
        /// <param name="popupView">a PopupView</param>
        /// <returns>false if depObj is disabled, yes otherwise</returns>
        private bool ScanAndDisableControls(DependencyObject depObj, PopupView popupView)
        {
            if (depObj == PopupFrame)
            {
                return true;
            }
            FrameworkElement control = depObj as FrameworkElement;
            if (control != null)
            {
                if (popupView != null)
                {
                    if (popupView.IsControlToAllowEnabled(control.Name))
                    {
                        return true;
                    }
                }
            }
            bool hasChildrenToAllowEnabled = false;
            int depObjCount = VisualTreeHelper.GetChildrenCount(depObj);
            for (int i = 0; i < depObjCount; i++)
            {
                if (ScanAndDisableControls(VisualTreeHelper.GetChild(depObj, i), popupView))
                {
                    hasChildrenToAllowEnabled = true;
                }
            }
            if (!hasChildrenToAllowEnabled)
            {
                if (control != null)
                {
                    if (control is ISupportUserInput)
                    {
                        (control as ISupportUserInput).AllowUserInput = false;
                    }
                    else if (control is Canvas)
                    {
                        control.IsEnabled = false;
                    }
                }
                return false;
            }
            return true;
        }

        private void Show(bool isShowing)
        {
            if (!this.FadeDuration.TimeSpan.Equals(TimeSpan.Zero) && !this.popupRendering)
            {
                DoubleAnimation da = new DoubleAnimation(isShowing ? 1.0d : 0.0d, this.FadeDuration);
                if (isShowing)
                {
                    da.AccelerationRatio = 1.0d;
                }
                else
                {
                    da.DecelerationRatio = 1.0d;
                }
                da.Completed += ShowCompleted;
                this.PopupFrame.BeginAnimation(OpacityProperty, da);
            }
            else
            {
                this.PopupFrame.Opacity = isShowing ? 1.0 : 0.0;
                ShowCompleted(null, EventArgs.Empty);
            }
        }

        private void ShowCompleted(object sender, EventArgs e)
        {
            if (null != sender)
            {
                (sender as AnimationClock).Completed -= ShowCompleted;
            }
            if (0.0 == this.PopupFrame.Opacity)
            {
                if (null != this.popupToClose)
                {
                    this.popupToClose.Show(false);
                    this.popupToClose = null;
                }
                if (null != this.pendingPopup)
                {
                    ShowPopup(true, this.pendingPopup, deferDisable);
                    this.pendingPopup = null;
                }
                else
                {
                    this.popupRendering = true;
                    this.PopupFrame.Navigate(null);
                }
            }
        }

        /// <summary>
        /// Enable all the controls in the sub-VisualTree staring at node "depObj"
        /// </summary>
        /// <param name="depObj">A node in the visual tree</param>
        private void EnableControls(DependencyObject depObj)
        {
            FrameworkElement control = depObj as FrameworkElement;
            if (control != null)
            {
                if (control is ISupportUserInput)
                {
                    (control as ISupportUserInput).AllowUserInput = true;
                }
                else if (control is Canvas)
                {
                    control.IsEnabled = true;
                }
            }
            int depObjCount = VisualTreeHelper.GetChildrenCount(depObj);
            for (int i = 0; i < depObjCount; i++)
            {
                EnableControls(VisualTreeHelper.GetChild(depObj, i));
            }
        }

        private void ManageShroudEffect(bool deferDisable)
        {
            if (null != this.currentPopupView)
            {
                PopupBackground.Visibility = this.currentPopupView.BackgroundType == PopupView.PopupBackgroundType.Darken ? Visibility.Visible : Visibility.Hidden;
                PopupShroud.Visibility = this.currentPopupView.BackgroundType == PopupView.PopupBackgroundType.Shroud ? Visibility.Visible : Visibility.Hidden;

                if (!deferDisable)
                {
                    DisableControls(this.currentPopupView);
                }
            }
            else
            {
                PopupBackground.Visibility = Visibility.Hidden;
                PopupShroud.Visibility = Visibility.Hidden;
                EnableControls(Window.GetWindow(this));
            }
        }

        private void PopupFrame_ContentRendered(object sender, EventArgs e)
        {
            if (this.popupRendering)
            {
                this.popupRendering = false;
                if (null != this.currentPopupView)
                {
                    UpdatePopupPosition();
                    Show(true);
                    this.currentPopupView.Show(true);
                }
            }
        }

        private void PopupFrame_Navigated(object sender, NavigationEventArgs e)
        {
            PopupFrame.NavigationService.RemoveBackEntry();
        }

        private void PopupFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.popupRendering && null != this.currentPopupView)
            {
                UpdatePopupPosition();
            }
        }

        private void UpdatePopupPosition()
        {
            if (this.currentPopupView != null)
            {
                if (this.currentPopupView.Alignment == PopupView.PopupAlignment.None)
                {
                    Canvas.SetLeft(this.PopupFrame, this.currentPopupView.XOffset);
                    Canvas.SetTop(this.PopupFrame, this.currentPopupView.YOffset);
                }
                else if (this.currentPopupView.Alignment == PopupView.PopupAlignment.Center)
                {
                    Canvas.SetLeft(this.PopupFrame, (Application.Current.MainWindow.ActualWidth - this.currentPopupView.ActualWidth) / 2.0);
                    Canvas.SetTop(this.PopupFrame, (Application.Current.MainWindow.ActualHeight - this.currentPopupView.ActualHeight) / 2.0);
                }
                else if (this.currentPopupView.Alignment == PopupView.PopupAlignment.Left)
                {
                    Canvas.SetLeft(this.PopupFrame, ((Application.Current.MainWindow.ActualWidth - this.currentPopupView.ActualWidth) - this.currentPopupView.LeftPadding) / 2.0);
                    Canvas.SetTop(this.PopupFrame, (Application.Current.MainWindow.ActualHeight - this.currentPopupView.ActualHeight) / 2.0);
                }
            }
        }

        private PopupView currentPopupView;
        private bool deferDisable;
        private PopupView pendingPopup;
        private bool popupRendering;
        private PopupView popupToClose;        
    }
}

