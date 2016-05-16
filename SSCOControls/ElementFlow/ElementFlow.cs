using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Collections.Specialized;

namespace SSCOControls
{
    public partial class ElementFlow : Selector, ISupportUserInput
    {
        public enum NavigationDirection
        {
            Left,
            Right,
        }
        
        public ElementFlow()
        {
            SetupCommands();
            this.Layout = new Wall();            
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Viewport = GetTemplateChild("PART_Viewport") as Viewport3D;
            this.ModelContainer = GetTemplateChild("PART_ModelContainer") as ContainerUIElement3D;
            this.ModelContainer.TouchDown += OnContainerTouchDown;
            this.Loaded += ElementFlow_Loaded;
            if (this.IsLoaded)
            {
                ElementFlow_Loaded(this, null);
            }
        }

        public bool AllowUserInput
        {
            get { return (bool)GetValue(AllowUserInputProperty); }
            set { SetValue(AllowUserInputProperty, value); }
        }
        
        public LayoutBase Layout
        {
            get { return (LayoutBase)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }

        public double TiltAngle
        {
            get { return (double)GetValue(TiltAngleProperty); }
            set { SetValue(TiltAngleProperty, value); }
        }

        public double ItemGap
        {
            get { return (double)GetValue(ItemGapProperty); }
            set { SetValue(ItemGapProperty, value); }
        }

        public double FrontItemGap
        {
            get { return (double)GetValue(FrontItemGapProperty); }
            set { SetValue(FrontItemGapProperty, value); }
        }

        public double PopoutDistance
        {
            get { return (double)GetValue(PopoutDistanceProperty); }
            set { SetValue(PopoutDistanceProperty, value); }
        }

        public double ElementWidth
        {
            get { return (double)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }

        public double ElementHeight
        {
            get { return (double)GetValue(ElementHeightProperty); }
            set { SetValue(ElementHeightProperty, value); }
        }

        public PerspectiveCamera Camera
        {
            get { return (PerspectiveCamera)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public bool HasReflection { get; set; }
        
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(ElementFlow));

        [Category("Behavior")]
        public event RoutedEventHandler Click
        {
            add
            {
                AddHandler(ClickEvent, value);
            }
            remove
            {
                RemoveHandler(ClickEvent, value);
            }
        }

        public static RoutedCommand NavigateLeft = new RoutedCommand("NavigateLeft", typeof(ElementFlow),
            new InputGestureCollection() {new KeyGesture(Key.Left)});

        public static RoutedCommand NavigateRight = new RoutedCommand("NavigateRight", typeof(ElementFlow),
            new InputGestureCollection() {new KeyGesture(Key.Right)});

        public static RoutedCommand NavigateUp = new RoutedCommand("NavigateUp", typeof(ElementFlow),
            new InputGestureCollection() {new KeyGesture(Key.Up)});

        public static RoutedCommand NavigateDown = new RoutedCommand("NavigateDown", typeof(ElementFlow),
            new InputGestureCollection() {new KeyGesture(Key.Down)});

        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(
            "Camera", typeof(PerspectiveCamera), typeof(ElementFlow),
            new PropertyMetadata(null, OnCameraChanged));

        public static readonly DependencyProperty AllowUserInputProperty = DependencyProperty.Register("AllowUserProperty", typeof(bool), typeof(ElementFlow),
               new PropertyMetadata(true, new PropertyChangedCallback(OnAllowUserInputChanged)));

        public static readonly DependencyProperty LayoutProperty =
            DependencyProperty.Register("Layout", typeof(LayoutBase), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register("ElementHeight", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(300.0));

        public static readonly DependencyProperty ElementWidthProperty =
            DependencyProperty.Register("ElementWidth", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(400.0));

        public static readonly DependencyProperty FrontItemGapProperty =
            DependencyProperty.Register("FrontItemGap", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(0.65, OnFrontItemGapChanged));

        public static readonly DependencyProperty HasReflectionProperty =
            DependencyProperty.Register("HasReflection", typeof(bool), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ItemGapProperty =
            DependencyProperty.Register("ItemGap", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(0.25, OnItemGapChanged));

        public static readonly DependencyProperty PopoutDistanceProperty =
            DependencyProperty.Register("PopoutDistance", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(1.0, OnPopoutDistanceChanged));

        public static readonly DependencyProperty TiltAngleProperty =
            DependencyProperty.Register("TiltAngle", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(45.0, OnTiltAngleChanged));
        static ElementFlow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementFlow), new FrameworkPropertyMetadata(typeof(ElementFlow)));
            internalResources = Application.LoadComponent(new Uri("/SSCOControls;component/ElementFlow/InternalResources.xaml", UriKind.Relative)) as ResourceDictionary;
        }

        internal int GetContainerCount()
        {
            return (null != this.ModelContainer) ? this.ModelContainer.Children.Count : 0;
        }

        internal int GetIndexFromModel(ModelUIElement3D model)
        {
            return GetIndexFromElement(model.GetValue(LinkedElementProperty) as FrameworkElement);
        }

        internal ModelUIElement3D GetModel(int index)
        {
            ModelUIElement3D model = GetModelFromIndex(index);
            if (null == model)
            {
                ModelUIElement3D newModel;
                for (int i = 0; i < GetContainerCount(); i++)
                {
                    newModel = this.ModelContainer.Children[i] as ModelUIElement3D;
                    if (Visibility.Hidden == newModel.Visibility)
                    {
                        model = newModel;
                        break;
                    }
                }
                GeometryModel3D geom3D;
                if (null == model)
                {
                    model = internalResources["ElementModel"] as ModelUIElement3D;
                    double aspect = ElementWidth / ElementHeight;
                    double reflectionFactor = HasReflection ? 1.0 : 0.5;
                    Point3DCollection positions = new Point3DCollection();
                    positions.Add(new Point3D(-aspect / 2, 1 * reflectionFactor, 0));
                    positions.Add(new Point3D(aspect / 2, 1 * reflectionFactor, 0));
                    positions.Add(new Point3D(aspect / 2, -1 * reflectionFactor, 0));
                    positions.Add(new Point3D(-aspect / 2, -1 * reflectionFactor, 0));
                    geom3D = model.Model as GeometryModel3D;
                    (geom3D.Geometry as MeshGeometry3D).Positions = positions;
                    this.ModelContainer.Children.Add(model);
                }
                else
                {
                    geom3D = model.Model as GeometryModel3D;
                }
                FrameworkElement elt = GetElementFromIndex(index);
                model.SetValue(LinkedElementProperty, elt);
                elt.SetValue(LinkedModelProperty, model);
                VisualBrush brush = new VisualBrush(elt);
                RenderOptions.SetCachingHint(brush, CachingHint.Cache);
                RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
                RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2.0);
                (geom3D.Material as DiffuseMaterial).Brush = brush;
                model.Visibility = Visibility.Visible;
            }
            return model;
        }
      
        internal Storyboard PrepareTemplateStoryboard(int index)
        {
            Storyboard sb = internalResources["ElementAnimator"] as Storyboard;
            Rotation3DAnimation rotAnim = sb.Children[0] as Rotation3DAnimation;
            Storyboard.SetTargetProperty(rotAnim, BuildTargetPropertyPath(index, AnimationType.Rotation));
            DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
            Storyboard.SetTargetProperty(xAnim, BuildTargetPropertyPath(index, AnimationType.TranslationX));
            DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
            Storyboard.SetTargetProperty(yAnim, BuildTargetPropertyPath(index, AnimationType.TranslationY));
            DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;
            Storyboard.SetTargetProperty(zAnim, BuildTargetPropertyPath(index, AnimationType.TranslationZ));
            DoubleAnimation oAnim = sb.Children[4] as DoubleAnimation;
            Storyboard.SetTargetProperty(oAnim, BuildTargetPropertyPath(index, AnimationType.Opacity));
            return sb;
        }

        internal void RemoveModel(ModelUIElement3D model)
        {
            FrameworkElement elt = GetElementFromIndex(GetIndexFromModel(model));
            if (null != elt)
            {
                elt.ClearValue(LinkedModelProperty);
            }
            model.ClearValue(LinkedElementProperty);
            model.Visibility = Visibility.Hidden;
        }

        internal ContainerUIElement3D ModelContainer;
        internal Viewport3D Viewport { get; private set; }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionChangedAction.Add == e.Action)
            {
                this.added = true;
            }
            else if (NotifyCollectionChangedAction.Reset == e.Action)
            {
                this.containerSelectedIndex = -1;
                ClearModels();
                if (null != this.Items && this.Items.Count > 0)
                {
                    this.added = true;
                    if (-1 == this.SelectedIndex)
                    {
                        this.SelectedIndex = 0;
                    }
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                int count = base.VisualChildrenCount;
                count = (count == 0) ? 0 : 1;
                return count;
            }
        }

        protected override bool IsEnabledCore
        {
            get
            {
                return base.IsEnabledCore && this.AllowUserInput;
            }
        }
                
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            SelectItemCore(this.SelectedIndex);
            base.OnSelectionChanged(e);
        }
        
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ClearModel(element as FrameworkElement, item);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.IsEnabled)
            {
                if (e.Delta < 0)
                {
                    Navigate(NavigationDirection.Left);
                }
                else if (e.Delta > 0)
                {
                    Navigate(NavigationDirection.Right);
                }
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            FrameworkElement frkElt = element as FrameworkElement;
            if (frkElt.Width != this.ElementWidth)
            {
                frkElt.Width = this.ElementWidth;            
            }
            if (frkElt.Height != this.ElementHeight)
            {
                frkElt.Height = this.ElementHeight;
            }
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            if (this.IsEnabled)
            {
                this.touchStart = e.GetTouchPoint(this);
            }
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            if (this.IsEnabled)
            {
                if (null != this.touchStart)
                {
                    TouchPoint touch = e.GetTouchPoint(this);
                    if (this.touchStart != null && touch.Position.X > (this.touchStart.Position.X + 100))
                    {
                        Navigate(NavigationDirection.Left);
                        this.touchStart = null;
                    }
                    if (touchStart != null && touch.Position.X < (this.touchStart.Position.X - 100))
                    {
                        Navigate(NavigationDirection.Right);
                        this.touchStart = null;
                    }
                }
            }
            else
            {
                this.touchStart = null;
            }
        }

        protected bool IsNavigatingLeft;
        protected bool IsNavigatingRight;

        private PropertyPath BuildTargetPropertyPath(int index, AnimationType animType)
        {
            PropertyDescriptor childDesc = TypeDescriptor.GetProperties(ModelContainer).Find("Children", true);
            string pathString = string.Empty;
            switch (animType)
            {
                case AnimationType.Rotation:
                    pathString = "(0)[0].(1)[" + index + "].(2).(3)[0].(4)";
                    break;
                case AnimationType.TranslationX:
                    pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(5)";
                    break;
                case AnimationType.TranslationY:
                    pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(6)";
                    break;
                case AnimationType.TranslationZ:
                    pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(7)";
                    break;
                case AnimationType.Opacity:
                    pathString = "(0)[0].(1)[" + index + "].Model.Material.Brush.Opacity";
                    return new PropertyPath(pathString, Viewport3D.ChildrenProperty, childDesc);
            }
            return new PropertyPath(pathString, Viewport3D.ChildrenProperty, childDesc, ModelUIElement3D.TransformProperty,
                Transform3DGroup.ChildrenProperty, RotateTransform3D.RotationProperty, TranslateTransform3D.OffsetXProperty,
                TranslateTransform3D.OffsetYProperty, TranslateTransform3D.OffsetZProperty, Selector.OpacityProperty);
        }

        private void ClearModel(FrameworkElement elt, object item)
        {
            ModelUIElement3D model = elt.GetValue(LinkedModelProperty) as ModelUIElement3D;
            elt.ClearValue(LinkedModelProperty);
            if (null != model)
            {
                model.Visibility = Visibility.Hidden;
                model.ClearValue(LinkedElementProperty);
                this.containerSelectedIndex = -1;
                ReflowItems();                
            }
        }

        private void ClearModels()
        {
            ModelUIElement3D model;
            for (int i = 0; i < GetContainerCount(); i++)
            {
                model = this.ModelContainer.Children[i] as ModelUIElement3D;
                RemoveModel(model);
            }
        }

        private void ElementFlow_Loaded(object sender, RoutedEventArgs e)
        {
            ReflowItems();
            if (this.Camera != this.Viewport.Camera)
            {
                this.Viewport.Camera = this.Camera;
            }
        }

        private void SetupCommands()
        {
            CommandBindings.Add(new CommandBinding(NavigateLeft, OnNavigateLeft, CanNavigateLeft));
            CommandBindings.Add(new CommandBinding(NavigateRight, OnNavigateRight, CanNavigateRight));
        }

        private void CanNavigateLeft(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedIndex > 0;
        }

        private void CanNavigateRight(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedIndex < Items.Count;
        }

        private int GetElementCount()
        {
            int count = 0;
            Canvas elementContainer = GetTemplateChild("PART_HiddenPanel") as Canvas;
            if (null != elementContainer && null != elementContainer.Children)
            {
                count = elementContainer.Children.Count;
            }
            return count;
        }

        private FrameworkElement GetElementFromIndex(int index)
        {
            FrameworkElement element = null;
            Canvas elementContainer = GetTemplateChild("PART_HiddenPanel") as Canvas;
            if (index > -1 && null != elementContainer && null != elementContainer.Children && elementContainer.Children.Count > index)
            {
                element = elementContainer.Children[index] as FrameworkElement;
            }
            return element;
        }

        private int GetIndexFromElement(FrameworkElement element)
        {
            Canvas elementContainer = GetTemplateChild("PART_HiddenPanel") as Canvas;
            if (null != elementContainer && null != elementContainer.Children)
            {
                for (int i = 0; i < elementContainer.Children.Count; i++)
                {
                    if (element == elementContainer.Children[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private ModelUIElement3D GetModelFromElement(FrameworkElement elt)
        {
            ModelUIElement3D model = null;
            int count = GetContainerCount();
            for (int i = 0; i < count; i++)
            {
                model = ModelContainer.Children[i] as ModelUIElement3D;
                if (Visibility.Visible == model.Visibility && elt == model.GetValue(LinkedElementProperty))
                {
                    return model;
                }
            }
            return null;
        }

        private ModelUIElement3D GetModelFromIndex(int index)
        {
            ModelUIElement3D model = null;
            int count = GetContainerCount();
            for (int i = 0; i < count; i++)
            {
                model = ModelContainer.Children[i] as ModelUIElement3D;
                if (Visibility.Visible == model.Visibility &&
                    index == GetIndexFromElement(model.GetValue(LinkedElementProperty) as FrameworkElement))
                {
                    return model;
                }
            }
            return null;
        }    
         
        private void OnNavigateLeft(object sender, ExecutedRoutedEventArgs e)
        {
            Navigate(NavigationDirection.Left);
        }

        private void OnNavigateRight(object sender, ExecutedRoutedEventArgs e)
        {
            Navigate(NavigationDirection.Right);
        }

        private void Navigate(NavigationDirection direction)
        {
            int index = -1;
            switch (direction)
            {
                case NavigationDirection.Left:
                    index = Math.Max(-1, this.SelectedIndex - 1);
                    break;
                case NavigationDirection.Right:
                    index = Math.Min(this.Items.Count - 1, this.SelectedIndex + 1);
                    break;                
            }
            if (index != -1 && this.SelectedIndex != index)
            {
                this.IsNavigatingLeft = NavigationDirection.Right == direction;
                this.IsNavigatingRight = !this.IsNavigatingLeft;
                this.SelectedIndex = index;
            }
        }

        private static void OnAllowUserInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(UIElement.IsEnabledProperty);
        }

        private static void OnTiltAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow cf = d as ElementFlow;
            cf.ReflowItems();
        }

        private static void OnItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }

        private static void OnFrontItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }

        private static void OnPopoutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }
        
        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            PerspectiveCamera camera = e.NewValue as PerspectiveCamera;
            if (camera == null)
            {
                throw new ArgumentNullException("e", "The Camera cannot be null");
            }
            if (ef.IsLoaded)
            {
                if (null != ef.Viewport)
                {
                    ef.Viewport.Camera = camera;
                }                
            }
        }

        private void OnContainerTouchDown(object sender, TouchEventArgs e)
        {
            if (this.IsEnabled)
            {
                ModelUIElement3D model = e.Source as ModelUIElement3D;
                if (null != model)
                {
                    int index = GetIndexFromModel(model);
                    if (index != this.SelectedIndex)
                    {
                        e.Handled = true;
                        this.touchStart = null;
                        this.IsNavigatingLeft = index > this.SelectedIndex;
                        this.IsNavigatingRight = !IsNavigatingLeft;
                        this.SelectedIndex = index;
                    }
                }
            }
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            var oldView = e.OldValue as LayoutBase;
            if (oldView != null)
            {
                oldView.Owner = null;
            }
            LayoutBase newView = e.NewValue as LayoutBase;
            if (newView == null)
            {
                throw new ArgumentNullException("e", "The Layout cannot be null");
            }
            newView.Owner = ef;
            ef.ReflowItems();
        }

        private void ReflowItems()
        {
            SelectItemCore(this.SelectedIndex);
        }

        private void SelectItemCore(int index)
        {
            if (this.IsLoaded && index >= 0 && index != this.containerSelectedIndex && index < GetElementCount())
            {
                this.Layout.SelectElement(index, this.added, this.IsNavigatingLeft, this.IsNavigatingRight);
                this.containerSelectedIndex = index;
                this.added = false;
                this.IsNavigatingLeft = false;
                this.IsNavigatingRight = false;
            }
        }

        private static readonly DependencyProperty LinkedElementProperty =
            DependencyProperty.Register("LinkedElement", typeof(UIElement), typeof(ElementFlow));

        private static readonly DependencyProperty LinkedModelProperty =
            DependencyProperty.Register("LinkedModel", typeof(ModelUIElement3D), typeof(ElementFlow));
        
        private bool added;
        private int containerSelectedIndex = -1;
        private static ResourceDictionary internalResources { get; set; }        
        private TouchPoint touchStart;     
    }

    internal enum AnimationType
    {
        Rotation,
        TranslationX,
        TranslationY,
        TranslationZ,
        Opacity
    }
}