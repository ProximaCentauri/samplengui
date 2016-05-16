namespace SSCOControls
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Navigation;
    using System.Windows.Threading;

    public class FaderFrame : Frame
    {
        public FaderFrame()
        {
            Navigating += OnNavigating;
        }
        
        public override void OnApplyTemplate()
        {
            // get a reference to the frame's content presenter
            // this is the element we will fade in and out
            this.contentPresenter = GetTemplateChild("PART_FrameCP") as ContentPresenter;
            base.OnApplyTemplate();
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

        public static readonly DependencyProperty FadeDurationProperty = DependencyProperty.Register("FadeDuration", typeof(Duration), typeof(FaderFrame),
            new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));
        
        protected void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            // if we did not internally initiate the navigation:
            //   1. cancel the navigation,
            //   2. cache the target,
            //   3. disable hittesting during the fade, and
            //   4. fade out the current content
            this.NavigationService.RemoveBackEntry();
            if (!this.FadeDuration.TimeSpan.Equals(TimeSpan.Zero))
            {
                if (null != this.Content && !this.allowDirectNavigation && null != this.contentPresenter)
                {
                    e.Cancel = true;
                    this.navArgs = e;
                    this.contentPresenter.IsHitTestVisible = false;
                    DoubleAnimation da = new DoubleAnimation(0.0d, this.FadeDuration);
                    da.DecelerationRatio = 1.0d;
                    da.Completed += FadeOutCompleted;
                    this.contentPresenter.BeginAnimation(OpacityProperty, da);
                }
                this.allowDirectNavigation = false;
            }            
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            // after the fade out
            //   1. re-enable hittesting
            //   2. initiate the delayed navigation
            //   3. invoke the FadeIn animation at Loaded priority
            (sender as AnimationClock).Completed -= FadeOutCompleted;
            if (null != this.contentPresenter)
            {
                this.contentPresenter.IsHitTestVisible = true;
                this.allowDirectNavigation = true;
                switch (this.navArgs.NavigationMode)
                {
                    case NavigationMode.New:
                        if (null != this.navArgs.Uri)
                        {
                            this.NavigationService.Navigate(this.navArgs.Uri);
                        }
                        else
                        {
                            this.NavigationService.Navigate(this.navArgs.Content);
                        }
                        break;
                    case NavigationMode.Back:
                        if (this.NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                        break;
                    case NavigationMode.Forward:
                        this.NavigationService.GoForward();
                        break;
                    case NavigationMode.Refresh:
                        NavigationService.Refresh();
                        break;
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)delegate()
                {
                    DoubleAnimation da = new DoubleAnimation(1.0d, FadeDuration);
                    da.AccelerationRatio = 1.0d;
                    this.contentPresenter.BeginAnimation(OpacityProperty, da);
                });
            }
        }

        private bool allowDirectNavigation;
        private ContentPresenter contentPresenter;
        private NavigatingCancelEventArgs navArgs;
    }
}
