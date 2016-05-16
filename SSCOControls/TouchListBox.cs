using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PsxNet;

namespace SSCOControls
{
    public enum AutoScrollSelection
    {
        None,
        Top,
        Bottom,
    }

    public class TouchListBox : ListBox, ISupportUserInput
    {
        public TouchListBox()
        {
            TouchDown += (sender, eventArgs) =>
            {
                if (!DisableSelection)
                {
                    FrameworkElement lb = sender as FrameworkElement;
                    dragIndex = GetItemIndex(eventArgs);
                    touchPoint = eventArgs.GetTouchPoint(lb);
                }
            };
            TouchUp += (sender, eventArgs) =>
            {
                if (!DisableSelection)
                {
                    FrameworkElement lb = sender as FrameworkElement;
                    if (-1 != dragIndex)
                    {
                        TouchPoint tp = eventArgs.GetTouchPoint(lb);
                        if (dragIndex == GetItemIndex(eventArgs) && Math.Abs(touchPoint.Position.X - tp.Position.X) < minDragDiff &&
                            Math.Abs(touchPoint.Position.Y - tp.Position.Y) < minDragDiff)
                        {
                            this.SelectedIndex = dragIndex;
                        }
                        dragIndex = -1;
                        touchPoint = null;
                    }
                }
            };
            SelectionChanged += (sender, eventArgs) =>
            {
                if (null != SelectionSound)
                {
                    bool play = true;

                    if (sender.GetType().IsSubclassOf(typeof(TouchListBox)))
                    {
                        object item = (sender as TouchListBox).SelectedItem;
                        if (item != null)
                        {
                            play = (bool)item.GetType().GetProperty("IsEnabled").GetValue(item, null);
                        }
                        else
                        {
                            play = false;
                        }
                    }

                    if (play)
                    {
                        ControlsAudio.PlaySound(SelectionSound);
                    }
                }
                TouchListBox touchListBox = sender as TouchListBox;
                if (null != touchListBox)
                {
                    this.ScrollToViewItem(touchListBox);
                }
            };
            Loaded += (sender, eventArgs) =>
            {
                TouchListBox touchListBox = sender as TouchListBox;
                if (null != touchListBox)
                {
                    this.ScrollToViewItem(touchListBox);
                }
            };
            RequestBringIntoView += (sender, eventArgs) =>
            {
                TouchListBox touchListBox = sender as TouchListBox;
                if (null != touchListBox)
                {
                    this.ScrollToViewItem(touchListBox);
                }
            };
            this.IsEnabledChanged += TouchListBox_IsEnabledChanged;
        }

        void TouchListBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SolidColorBrush backgroundColor;

            if (this.Background != null && !String.IsNullOrEmpty(this.Background.ToString()))
            {
                backgroundColor = (SolidColorBrush)(new BrushConverter().ConvertFrom(this.Background.ToString()));
            }
            else
            {
                backgroundColor = new SolidColorBrush(Colors.Transparent);
            }

            if (this.Resources.Contains(SystemColors.ControlBrushKey))
                this.Resources.Remove(SystemColors.ControlBrushKey);

            if (this.Resources.Contains(SystemColors.HighlightBrushKey))
                this.Resources.Remove(SystemColors.HighlightBrushKey);

            if (this.Resources.Contains(SystemColors.WindowBrushKey))
                this.Resources.Remove(SystemColors.WindowBrushKey);

            if (!this.IsEnabled)
            {
                this.Resources.Add(SystemColors.ControlBrushKey, backgroundColor);
                this.Resources.Add(SystemColors.HighlightBrushKey, backgroundColor);
                this.Resources.Add(SystemColors.WindowBrushKey, backgroundColor);
            }
            else
            {
                this.Resources.Add(SystemColors.ControlBrushKey, new SolidColorBrush(Colors.White));
                this.Resources.Add(SystemColors.HighlightBrushKey, new SolidColorBrush(Colors.White));
                this.Resources.Add(SystemColors.WindowBrushKey, backgroundColor);
            }
        }


        private void ScrollToViewItem(TouchListBox touchListBox)
        {
            if (touchListBox.Items.Count > 0 && touchPoint == null)
            {
                if (VisualTreeHelper.GetChildrenCount(this) > 0)
                {
                    var scrollViewer = GetScrollViewer(this) as ScrollViewer;
                    if (null != scrollViewer)
                    {
                        if (AutoScrollIntoView == AutoScrollSelection.Top)
                        {
                            scrollViewer.ScrollToTop();
                        }
                        else if (AutoScrollIntoView == AutoScrollSelection.Bottom)
                        {
                            scrollViewer.ScrollToBottom();
                        }
                    }
                }
                else
                {
                    if (AutoScrollIntoView == AutoScrollSelection.Top)
                    {
                        touchListBox.ScrollIntoView(touchListBox.Items[0]);
                    }
                    else if (AutoScrollIntoView == AutoScrollSelection.Bottom)
                    {
                        touchListBox.ScrollIntoView(touchListBox.Items[touchListBox.Items.Count - 1]);
                    }
                }
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as ScrollViewer) ?? GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private ScrollViewer Scrollviewer
        {
            get
            {
                if (VisualTreeHelper.GetChildrenCount(this) > 0)
                {
                    DependencyObject depObj = VisualTreeHelper.GetChild(this, 0);
                    if (depObj != null)
                    {
                        return (ScrollViewer)VisualTreeHelper.GetChild(depObj, 0);
                    }
                }
                return null;
            }
        }

        public AutoScrollSelection AutoScrollIntoView
        {
            get
            {
                return (AutoScrollSelection)GetValue(AutoScrollSelectionIntoViewProperty);
            }
            set
            {
                SetValue(AutoScrollSelectionIntoViewProperty, value);
            }
        }

        public bool DisableSelection
        {
            get
            {
                return Convert.ToBoolean(GetValue(DisableSelectionProperty));
            }
            set
            {
                SetValue(DisableSelectionProperty, value);
            }
        }

        public string SelectionSound
        {
            get
            {
                return GetValue(SelectionSoundProperty) as string;
            }
            set
            {
                SetValue(SelectionSoundProperty, value);
            }
        }

        public bool AllowUserInput
        {
            get { return (bool)GetValue(AllowUserInputProperty); }
            set { SetValue(AllowUserInputProperty, value); }
        }

        public static readonly DependencyProperty AllowUserInputProperty =
        DependencyProperty.Register(
                "AllowUserProperty",
                typeof(bool),
                typeof(TouchListBox),
                new PropertyMetadata(
                        true,
                        new PropertyChangedCallback(OnAllowUserInputChanged)));

        public static DependencyProperty DisableSelectionProperty = DependencyProperty.Register("DisableSelection", typeof(bool), typeof(TouchListBox));
        public static DependencyProperty SelectionSoundProperty = DependencyProperty.Register("SelectionSound", typeof(string), typeof(TouchListBox));
        public static DependencyProperty AutoScrollSelectionIntoViewProperty = DependencyProperty.Register("AutoScrollSelectionIntoView", typeof(AutoScrollSelection), typeof(TouchListBox),
            new PropertyMetadata(AutoScrollSelection.None));

        protected override bool IsEnabledCore
        {
            get
            {
                return base.IsEnabledCore && AllowUserInput;
            }
        }

        private static void OnAllowUserInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(UIElement.IsEnabledProperty);
        }

        protected int GetItemIndex(TouchEventArgs e)
        {
            int index = -1;
            Visual lbi = null;
            for (int i = 0; i < Items.Count; i++)
            {
                lbi = ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                if (lbi == null)
                {
                    continue;
                }
                if (VisualTreeHelper.GetDescendantBounds(lbi).Contains(e.GetTouchPoint((IInputElement)lbi).Position))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private int dragIndex = -1;
        private const int minDragDiff = 15;
        protected TouchPoint touchPoint;
    }
}
