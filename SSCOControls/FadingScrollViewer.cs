using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SSCOControls
{
    public class FadingScrollViewer : ScrollViewer
    {
        public FadingScrollViewer()
        {
            this.ScrollChanged += FadingScrollViewer_ScrollChanged;
            this.ManipulationBoundaryFeedback += FadingScrollViewer_ManipulationBoundaryFeedback;
        }

        static FadingScrollViewer()
        {
            if (null == TopOpacityMask)
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0, 0);
                brush.EndPoint = new Point(0, 1);
                brush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.20));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
                TopOpacityMask = brush;
            }
            if (null == TopBottomOpacityMask)
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0, 0);
                brush.EndPoint = new Point(0, 1);
                brush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.20));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
                brush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.80));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                TopBottomOpacityMask = brush;
            }
            if (null == BottomOpacityMask)
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0, 0);
                brush.EndPoint = new Point(0, 1);
                brush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.80));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                BottomOpacityMask = brush;
            }
        }

        private void FadingScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void FadingScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (scrollIconUp == null || scrollIconDown == null)
            {
                return;
            }

            if (ScrollableHeight == 0)
            {
                scrollIconUp.Visibility = Visibility.Collapsed;
                scrollIconDown.Visibility = Visibility.Collapsed;
                if (grid != null && ShowFade) grid.OpacityMask = null;
            }
            else if (ScrollableHeight == e.VerticalOffset)
            {
                scrollIconUp.Visibility = Visibility.Visible;
                scrollIconDown.Visibility = Visibility.Collapsed;
                if (grid != null && ShowFade) grid.OpacityMask = TopOpacityMask;
            }
            else if (ScrollableHeight > e.VerticalOffset && e.VerticalOffset > 0)
            {
                scrollIconUp.Visibility = Visibility.Visible;
                scrollIconDown.Visibility = Visibility.Visible;
                if (grid != null && ShowFade) grid.OpacityMask = TopBottomOpacityMask;
            }
            else if (ScrollableHeight > 0 && e.VerticalOffset == 0)
            {
                scrollIconUp.Visibility = Visibility.Collapsed;
                scrollIconDown.Visibility = Visibility.Visible;
                if (grid != null && ShowFade) grid.OpacityMask = BottomOpacityMask;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            grid = this.Template.FindName(SCROLL_PRESENTER_GRID, this) as UIElement;
            scrollIconUp = this.Template.FindName(SCROLL_PRESENTER_ICON_UP, this) as UIElement;
            scrollIconDown = this.Template.FindName(SCROLL_PRESENTER_ICON_DOWN, this) as UIElement;
        }

        public bool ShowFade
        {
            get
            {
                return Convert.ToBoolean(GetValue(ShowFadeProperty));
            }
            set
            {
                SetValue(ShowFadeProperty, value);
            }
        }

        public static DependencyProperty ShowFadeProperty = DependencyProperty.Register("ShowFade", typeof(bool), typeof(FadingScrollViewer));

        protected static LinearGradientBrush TopOpacityMask = null;
        protected static LinearGradientBrush TopBottomOpacityMask = null;
        protected static LinearGradientBrush BottomOpacityMask = null;

        private UIElement grid;
        private UIElement scrollIconUp;
        private UIElement scrollIconDown;
        private const string PART_SCROLL_PRESENTER_CONTAINER_NAME = "PART_ScrollContentPresenterContainer";
        private const string SCROLL_PRESENTER_ICON_CONTAINER = "ScrollContentPresenterIconContainer";
        private const string SCROLL_PRESENTER_ICON_UP = "ScrollUpIcon";
        private const string SCROLL_PRESENTER_ICON_DOWN = "ScrollDownIcon";
        private const string SCROLL_PRESENTER_GRID = "FadingGrid";
    }
}