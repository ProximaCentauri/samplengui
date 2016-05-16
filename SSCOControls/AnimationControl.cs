﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SSCOControls
{
    public class AnimationControl : ContentControl
    {
        public AnimationControl()
        {
            this.IsVisibleChanged += OnIsVisibleChanged;
            this.IsTabStop = false;
            this.Loaded += OnLoaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.isTemplateApplied = true;
            PlayAnimation();            
        }
        
        public System.Windows.Media.Animation.Storyboard Storyboard { get; set; }

        public static DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard",
            typeof(System.Windows.Media.Animation.Storyboard), typeof(AnimationControl),
            new PropertyMetadata(null, new PropertyChangedCallback(OnStoryboardChanged)));

        protected override void OnStyleChanged(Style oldStyle, Style newStyle)
        {
            if (this.isPlaying)
            {
                this.isPlaying = false;
                this.Storyboard.Stop(this);
            }
            this.isTemplateApplied = false;
            base.OnStyleChanged(oldStyle, newStyle);
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AnimationControl control = sender as AnimationControl;
            if (null != control)
            {
                control.PlayAnimation();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AnimationControl control = sender as AnimationControl;
            if (null != control)
            {
                control.PlayAnimation();
            }
        }

        private static void OnStoryboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimationControl control = d as AnimationControl;
            if (null != control)
            {
                control.Storyboard = (System.Windows.Media.Animation.Storyboard)e.NewValue;
                control.PlayAnimation();
            }
        }

        private void PlayAnimation()
        {
            if (this.isTemplateApplied && null != this.Storyboard && this.IsLoaded)
            {
                if (this.IsVisible)
                {
                    if (!this.isPlaying)
                    {
                        this.isPlaying = true;
                        this.Storyboard.Begin(this, this.Template, true);
                    }
                }
                else
                {
                    if (this.isPlaying)
                    {
                        this.isPlaying = false;
                        this.Storyboard.Stop(this);
                    }
                }
            }
        }

        private bool isPlaying;
        private bool isTemplateApplied;
    }
}