using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace SSCOControls
{
    /// <summary>
    /// This type is passed by SlidingGridPage control to the PageIndicator control
    /// </summary>
    public struct PageInfoStruct
    {
        private int pageCount;
        private int currentPageIndex;

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

        public int CurrentPage
        {
            get { return currentPageIndex + 1; }
        }

        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set { currentPageIndex = value; }
        }
    };

    public enum SwipeAction { Right, Left };

    public class SwipeEventArgs : EventArgs
    {
        public SwipeAction Action { get; set; }
    }

    public class LogEventArgs : EventArgs
    {
        public string LogMessage { get; set; }
        public int RenderedItemCount { get; set; }
    }

    public class SlidingGridPage : TouchListBox
    {
        public SlidingGridPage()
        {
            Initialized += (object sender, EventArgs e) =>
            {
                SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            };

            TouchDown += (sender, eventArgs) =>
            {
                // prepare screenshot
                if (currentPageIndex == 0)
                {
                    ItemsToBitmap();
                }
            };

            TouchMove += (sender, eventArgs) =>
            {
                var Touch = eventArgs.GetTouchPoint(this);

                if (!alreadySwiped)
                {
                    SwipeEventArgs args = new SwipeEventArgs();

                    if (touchPoint != null && Touch.Position.X < (touchPoint.Position.X - SwipeThreshold))
                    {
                        args.Action = SwipeAction.Left;
                        OnSwipeEvent(args);
                        alreadySwiped = true;
                        SlideLeft();
                    }

                    if (touchPoint != null && Touch.Position.X > (touchPoint.Position.X + SwipeThreshold))
                    {
                        args.Action = SwipeAction.Right;
                        OnSwipeEvent(args);
                        alreadySwiped = true;
                        SlideRight();
                    }
                }
            };

            backgroundImage.Stretch = Stretch.None;
            backgroundImage.AlignmentY = AlignmentY.Center;
        }

        public bool PerfLogging { get; set; }

        public event EventHandler<SwipeEventArgs> SwipeEvent;
        public event EventHandler<LogEventArgs> LogEvent;
        public event EventHandler<EventArgs> PageUpdateEvent;

        protected virtual void OnPageUpdate(EventArgs e)
        {
            EventHandler<EventArgs> handler = PageUpdateEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSwipeEvent(SwipeEventArgs e)
        {
            EventHandler<SwipeEventArgs> handler = SwipeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLogEvent(LogEventArgs e)
        {
            EventHandler<LogEventArgs> handler = LogEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Updates the pages.
        /// </summary>
        public void UpdatePages()
        {
            if (pageCapacity < 1)
            {
                UpdatePageCapacity();
            }
            PopulatePages();
        }

        public int SelectedPage
        {
            get { return currentPageIndex; }
            set
            {
                if (value < pages.Count)
                {
                    LogMessage("StartItemListRender");
                    ItemsSource = pages[value];
                    if (PerfLogging)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(LogPerf));
                    }

                    currentPageIndex = value;
                    UpdatePageInfo();
                }
            }
        }

        public int PageCount
        {
            get { return pages.Count; }
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            alreadySwiped = false;
            base.OnTouchDown(e);
        }

        private void UpdatePageInfo()
        {
            pageInfo.PageCount = pages.Count == 0 ? 1 : pages.Count; // we always have a minimum of 1 page
            pageInfo.CurrentPageIndex = currentPageIndex;
            if (null != pageIndicator)
            {
                pageIndicator.Visibility = pages.Count > 1 ? Visibility.Visible : Visibility.Hidden;
                pageIndicator.UpdateControl(pageInfo);
            }

            if(null != pageIndicatorLabel)
            {
                // hide if just one page
                pageIndicatorLabel.Visibility = pageInfo.PageCount > 1 ? Visibility.Visible : Visibility.Hidden;

                if (!PageIndicatorLabelFormat.Equals(string.Empty))
                {
                    try
                    {
                        if (System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag.Equals("cs-CZ") && pageInfo.PageCount > 4)
                        {
                            pageIndicatorLabel.Text = string.Format(PageIndicatorLabelFormat2, pageInfo.CurrentPage, pageInfo.PageCount);
                        }
                        else
                        {
                            pageIndicatorLabel.Text = string.Format(PageIndicatorLabelFormat, pageInfo.CurrentPage, pageInfo.PageCount);
                        }
                    }
                    catch
                    {
                        pageIndicatorLabel.Text = string.Format(DefaultIndicatorLabelFormat, pageInfo.CurrentPage, pageInfo.PageCount);
                    }
                }
                else
                {
                    pageIndicatorLabel.Text = string.Format(DefaultIndicatorLabelFormat, pageInfo.CurrentPage, pageInfo.PageCount);
                }
            }

            OnPageUpdate(new EventArgs());
        }

        public PageInfoStruct PageInfo
        {
            get
            {
                return pageInfo;
            }
        }

        void GridItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Refresh();
                    break;
            }
        }

        private void Refresh()
        {
            gridItems.Clear();
            foreach (object item in GridItemsSource)
            {
                gridItems.Add(item);
            }

            UpdatePages();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == GridItemsSourceProperty)
            {
                if (null != GridItemsSource)
                {
                    Refresh();

                    if (e.OldValue!=null)
                    {
                        try
                        {
                            ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= GridItemsSource_CollectionChanged;
                        }
                        catch
                        {
                        }
                    }

                    try
                    {
                        ((INotifyCollectionChanged)GridItemsSource).CollectionChanged += GridItemsSource_CollectionChanged;
                    }
                    catch
                    {
                    }
                }
            }
            else if (e.Property == ActualWidthProperty || e.Property == ActualHeightProperty)
            {
                if (!isAnimating)
                {
                    UpdatePageCapacity();
                    PopulatePages();
                }
                else
                {
                    dimensionPropertyChanged = true;
                }
            }
            else if (e.Property == PageIndicatorControlProperty) // control assignment
            {
                pageIndicator = PageIndicatorControl;
                if (SlideOnTouchIndicator)
                {
                    pageIndicator.TouchDown += pageIndicator_TouchDown;
                }
                else
                {
                    pageIndicator.TouchDown -= pageIndicator_TouchDown;
                }
            }
            else if (e.Property == PageIndicatorLabelControlProperty) // control assignment
            {
                pageIndicatorLabel = PageIndicatorLabelControl;
                if (SlideOnTouchIndicatorLabel)
                {
                    pageIndicatorLabel.TouchDown += pageIndicator_TouchDown;
                }
                else
                {
                    pageIndicatorLabel.TouchDown -= pageIndicator_TouchDown;
                }
            }
            else if (e.Property == SlideOnTouchIndicatorProperty)
            {
                if (pageIndicator != null)
                {
                    if (SlideOnTouchIndicator)
                    {
                        pageIndicator.TouchDown += pageIndicator_TouchDown;
                    }
                    else
                    {
                        pageIndicator.TouchDown -= pageIndicator_TouchDown;
                    }
                }
            }
            else if (e.Property == SlideOnTouchIndicatorLabelProperty)
            {
                if (pageIndicatorLabel != null)
                {
                    if (SlideOnTouchIndicatorLabel)
                    {
                        pageIndicatorLabel.TouchDown += pageIndicator_TouchDown;
                    }
                    else
                    {
                        pageIndicatorLabel.TouchDown -= pageIndicator_TouchDown;
                    }
                }
            }
            else if (e.Property == MarginProperty)
            {
                if (!isAnimating)
                {
                    origMargin = Margin;
                }
            }

            base.OnPropertyChanged(e);
        }

        void pageIndicator_TouchDown(object sender, TouchEventArgs e)
        {
            if (IsEnabled)
            {
                if (null != PageIndicatorLabelSound)
                {
                    if (pageInfo.CurrentPage < pageInfo.PageCount && PageIndicatorLabelSound.Length > 0)
                    {
                        ControlsAudio.PlaySound(PageIndicatorLabelSound);
                    }
                }
                SlideLeft();
            }
        }

        private void UpdateMeasurements()
        {
            if (ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                foreach (object listBoxItem in Items)
                {
                    ListBoxItem container = ItemContainerGenerator.ContainerFromItem(listBoxItem) as ListBoxItem;
                    if (null != container)
                    {
                        try
                        {
                            container.UpdateLayout();
                            if (container.ActualWidth > 0)
                            {
                                itemWidth = container.ActualWidth;
                                itemHeight = container.ActualHeight;
                                break;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private void PopulatePages()
        {
            ItemsSource = null;
            pages.Clear();
            currentPageIndex = 0;

            if (pageCapacity < 1)
            {
                UpdatePageCapacity();
            }
            else
            {
                if (gridItems.Count > pageCapacity)
                {
                    int pageCount = (int)Math.Ceiling((double)gridItems.Count / pageCapacity);
                    int nItem = 0;

                    if (gridItems.Count > 0)
                    {
                        for (int i = 0; i < pageCount; i++)
                        {
                            ObservableCollection<object> pageItems = new ObservableCollection<object>();
                            for (int j = 0; j < pageCapacity && nItem < gridItems.Count; j++)
                            {
                                pageItems.Add(gridItems[nItem]);
                                nItem++;
                            }
                            pages.Add(pageItems);
                        }
                    }
                }
            }

            LogMessage("StartItemListRender");
            if (pages.Count > 0)
            {
                ItemsSource = pages[0];
            }
            else
            {
                ItemsSource = gridItems;
            }
            if (PerfLogging)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(LogPerf));
            }

            this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(ItemsToBitmap));
            
            UpdatePageInfo();
        }

        private void LogPerf()
        {
            LogMessage("EndItemListRender");
        }

        private void LogMessage(string msg)
        {
            LogEventArgs args = new LogEventArgs();
            args.LogMessage = msg;
            args.RenderedItemCount = Items.Count;
            OnLogEvent(args);
        }

        private void ItemsToBitmap()
        {
            // take bitmap image for slide transition
            if (null == contentPresenter)
            {
                contentPresenter = FindVisualChild<ContentPresenter>(this);
            }

            if(null != contentPresenter)
            {
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)ActualWidth - 2, (int)ActualHeight - 2, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(contentPresenter);
                backgroundImage.ImageSource = renderTargetBitmap;
            }
        }

        private void UpdatePageCapacity()
        {
            if (itemWidth < 1 || itemHeight < 1)
            {
                UpdateMeasurements();
            }

            if (itemWidth > 1 && itemHeight > 1)
            {
                double xItems = Math.Floor(ActualWidth / itemWidth);
                double yItems = Math.Floor(ActualHeight / itemHeight);
                pageCapacity = (int)(xItems * yItems);
            }
        }

        public void SlideRight()
        {
            if (!isAnimating)
            {
                if (currentPageIndex == 0)
                {
                    return;
                }
                currentPageIndex--;
                UpdatePageInfo();

                // hide the items while a snapshot image takes its place
                contentPresenter.Visibility = Visibility.Hidden;
                Background = backgroundImage;

                LogMessage("StartItemListRender");
                ItemsSource = pages[currentPageIndex];
                if (PerfLogging)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(LogPerf));
                }

                isAnimating = true;
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = origMargin;
                animation.To = new Thickness(ActualWidth, origMargin.Top, origMargin.Right, origMargin.Bottom);
                animation.AccelerationRatio = 0.9;
                animation.Duration = TimeSpan.FromMilliseconds(SlideDuration);

                animation.Completed += (object sender, EventArgs e) =>
                {
                    ThicknessAnimation animation2 = new ThicknessAnimation();
                    animation2.From = new Thickness(-ActualWidth, origMargin.Top, origMargin.Right, origMargin.Bottom);
                    animation2.To = origMargin;
                    animation2.DecelerationRatio = 0.9;
                    animation2.Duration = TimeSpan.FromMilliseconds(SlideDuration);
                    animation2.Completed += AnimationComplete;

                    BeginAnimation(MarginProperty, animation2);

                    Background = null;
                    contentPresenter.Visibility = Visibility.Visible;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(ItemsToBitmap));
                };

                BeginAnimation(MarginProperty, animation);
            }
        }

        public void SlideLeft()
        {
            if (!isAnimating)
            {
                if (currentPageIndex + 2 > pages.Count)
                {
                    return;
                }
                currentPageIndex++;
                UpdatePageInfo();

                // hide the items while a snapshot image takes its place
                contentPresenter.Visibility = Visibility.Hidden;
                Background = backgroundImage;
                
                LogMessage("StartItemListRender");
                ItemsSource = pages[currentPageIndex];
                if (PerfLogging)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(LogPerf));
                }

                isAnimating = true;
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = origMargin;
                animation.To = new Thickness(-ActualWidth, origMargin.Top, origMargin.Right, origMargin.Bottom);
                animation.AccelerationRatio = 0.9;
                animation.Duration = TimeSpan.FromMilliseconds(SlideDuration);

                animation.Completed += (object sender, EventArgs e) =>
                {
                    ThicknessAnimation animation2 = new ThicknessAnimation();
                    animation2.From = new Thickness(ActualWidth, origMargin.Top, origMargin.Right, origMargin.Bottom);
                    animation2.To = origMargin;
                    animation2.DecelerationRatio = 0.9;
                    animation2.Duration = TimeSpan.FromMilliseconds(SlideDuration);
                    animation2.Completed += AnimationComplete;

                    BeginAnimation(MarginProperty, animation2);

                    Background = null;
                    contentPresenter.Visibility = Visibility.Visible;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(ItemsToBitmap));
                };

                BeginAnimation(MarginProperty, animation);
            }
        }

        private void AnimationComplete(object sender, EventArgs e)
        {
            isAnimating = false;

            if (dimensionPropertyChanged)
            {
                UpdatePageCapacity();
                PopulatePages();
                dimensionPropertyChanged = false;
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public IEnumerable GridItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(GridItemsSourceProperty);
            }
            set
            {
                SetValue(GridItemsSourceProperty, value);
            }
        }

        public PageIndicator PageIndicatorControl
        {
            get
            {
                return GetValue(PageIndicatorControlProperty) as PageIndicator;
            }
            set
            {
                SetValue(PageIndicatorControlProperty, value);
            }
        }

        public TextBlock PageIndicatorLabelControl
        {
            get
            {
                return GetValue(PageIndicatorLabelControlProperty) as TextBlock;
            }
            set
            {
                SetValue(PageIndicatorLabelControlProperty, value);
            }
        }

        public string PageIndicatorLabelFormat
        {
            get
            {
                return GetValue(PageIndicatorLabelFormatProperty) as string;
            }
            set
            {
                SetValue(PageIndicatorLabelFormatProperty, value);
            }
        }

        public string PageIndicatorLabelFormat2
        {
            get
            {
                return GetValue(PageIndicatorLabelFormat2Property) as string;
            }
            set
            {
                SetValue(PageIndicatorLabelFormat2Property, value);
            }
        }

        public bool SlideOnTouchIndicator
        {
            get { return (bool)GetValue(SlideOnTouchIndicatorProperty); }
            set { SetValue(SlideOnTouchIndicatorProperty, value); }
        }

        public bool SlideOnTouchIndicatorLabel
        {
            get { return (bool)GetValue(SlideOnTouchIndicatorLabelProperty); }
            set { SetValue(SlideOnTouchIndicatorLabelProperty, value); }
        }

        public string PageIndicatorLabelSound
        {
            get
            {
                return GetValue(PageIndicatorLabelSoundProperty) as string;
            }
            set
            {
                SetValue(PageIndicatorLabelSoundProperty, value);
            }
        }

        public const string DefaultIndicatorLabelFormat = "{0} / {1}";

        private PageIndicator pageIndicator = null;
        private TextBlock pageIndicatorLabel = null;

        private static DependencyProperty PageIndicatorControlProperty = DependencyProperty.Register("PageIndicatorControl", typeof(PageIndicator), typeof(SlidingGridPage));
        private static DependencyProperty PageIndicatorLabelControlProperty = DependencyProperty.Register("PageIndicatorLabelControl", typeof(TextBlock), typeof(SlidingGridPage));
        private static DependencyProperty PageIndicatorLabelFormatProperty = DependencyProperty.Register("PageIndicatorLabelFormat", typeof(string), typeof(SlidingGridPage));
        private static DependencyProperty PageIndicatorLabelFormat2Property = DependencyProperty.Register("PageIndicatorLabelFormat2", typeof(string), typeof(SlidingGridPage));
        private static DependencyProperty GridItemsSourceProperty = DependencyProperty.Register("GridItemsSource", typeof(IEnumerable), typeof(SlidingGridPage));
        private static DependencyProperty PageIndicatorLabelSoundProperty = DependencyProperty.Register("PageIndicatorLabelSound", typeof(string), typeof(SlidingGridPage));

        private static DependencyProperty SlideOnTouchIndicatorProperty = DependencyProperty.Register("SlideOnTouchIndicator", typeof(bool), typeof(SlidingGridPage),
            new PropertyMetadata((bool)false));
        private static DependencyProperty SlideOnTouchIndicatorLabelProperty = DependencyProperty.Register("SlideOnTouchIndicatorLabel", typeof(bool), typeof(SlidingGridPage),
            new PropertyMetadata((bool)false));

        private Collection<ObservableCollection<object>> pages = new Collection<ObservableCollection<object>>();
        private ObservableCollection<object> gridItems = new ObservableCollection<object>();

        private const int SlideDuration = 300;

        private bool alreadySwiped = false;
        private const int SwipeThreshold = 50;

        private int pageCapacity = -1;
        private int currentPageIndex = 0;

        private double itemHeight = -1;
        private double itemWidth = -1;

        private PageInfoStruct pageInfo = new PageInfoStruct();
        private Thickness origMargin = new Thickness(0, 0, 0, 0);
        private bool isAnimating = false;

        private bool dimensionPropertyChanged = false;

        private ContentPresenter contentPresenter;

        private ImageBrush backgroundImage = new ImageBrush();
    }

    /// <summary>
    /// PageIndicator is based on UniformGrid
    /// Supports indicator color and size properties
    /// </summary>
    public class PageIndicator : UniformGrid
    {
        public PageIndicator()
        {
            // default values
            Rows = 1;
            color = Brushes.LightGray;
            currentPageColor = Brushes.White;
            indicatorSize = 10;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IndicatorColorProperty || e.Property == CurrentPageColorProperty)
            {
                if (null != PageIndicatorColor)
                {
                    color = PageIndicatorColor;
                }
                if (null != CurrentPageIndicatorColor)
                {
                    currentPageColor = CurrentPageIndicatorColor;
                }
            }
            else if (e.Property == IndicatorSizeProperty)
            {
                if (IndicatorSize > 0)
                {
                    indicatorSize = IndicatorSize;
                }
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Updates the number of pages and the current page displayed
        /// The indicator is hidden if there is only one page
        /// </summary>
        /// <param name="pageInfo">Page information, this contains number of pages and current page</param>
        public void UpdateControl(PageInfoStruct pageInfo)
        {
            Children.Clear();
            Columns = pageInfo.PageCount;
            Width = Columns * indicatorSize * 2;

            for (int i = 0; i < Columns; i++)
            {
                Ellipse circle = new Ellipse();
                circle.Height = indicatorSize;
                circle.Width = indicatorSize;
                circle.Fill = (i == pageInfo.CurrentPageIndex) ? currentPageColor : color;
                Children.Add(circle);
            }
        }

        public SolidColorBrush PageIndicatorColor
        {
            get
            {
                return GetValue(IndicatorColorProperty) as SolidColorBrush;
            }
            set
            {
                SetValue(IndicatorColorProperty, value);
            }
        }

        public SolidColorBrush CurrentPageIndicatorColor
        {
            get
            {
                return GetValue(CurrentPageColorProperty) as SolidColorBrush;
            }
            set
            {
                SetValue(CurrentPageColorProperty, value);
            }
        }

        public int IndicatorSize
        {
            get
            {
                int size = (int)GetValue(IndicatorSizeProperty);
                return size;
            }
            set
            {
                SetValue(IndicatorSizeProperty, value);
            }
        }

        private static DependencyProperty IndicatorColorProperty = DependencyProperty.Register("PageIndicatorColor", typeof(SolidColorBrush), typeof(PageIndicator));
        private static DependencyProperty CurrentPageColorProperty = DependencyProperty.Register("CurrentPageIndicatorColor", typeof(SolidColorBrush), typeof(PageIndicator));
        private static DependencyProperty IndicatorSizeProperty = DependencyProperty.Register("IndicatorSize", typeof(int), typeof(PageIndicator));

        private SolidColorBrush color = null;
        private SolidColorBrush currentPageColor = null;

        private int indicatorSize = 0;
    }
}
