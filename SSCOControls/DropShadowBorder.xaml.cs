// <copyright file="DropShadowBorder.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
namespace SSCOControls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for DropShadow.xaml
    /// </summary>
    public partial class DropShadowBorder : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the DropShadowBorder class.
        /// </summary>
        public DropShadowBorder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// BorderPath_Loaded
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param> 
        private void BorderPath_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BorderPath.Parent is FrameworkElement)
            {
                FrameworkElement p = BorderPath.Parent as FrameworkElement;
                p.SizeChanged += new SizeChangedEventHandler(BorderPath_SizeChanged);
            }
            BorderPath_SizeChanged(null, null);
        }

        /// <summary>
        /// BorderPath_Unloaded
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param> 
        private void BorderPath_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.BorderPath.Parent is FrameworkElement)
            {
                FrameworkElement p = BorderPath.Parent as FrameworkElement;
                p.SizeChanged -= new SizeChangedEventHandler(BorderPath_SizeChanged);
            }
        }


        /// <summary>
        /// BorderPath_SizeChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param> 
        private void BorderPath_SizeChanged(object sender, RoutedEventArgs e)
        {
            if (this.BorderPath.Parent is FrameworkElement)
            {
                FrameworkElement p = BorderPath.Parent as FrameworkElement;
                if (p.ActualWidth > 0 && p.ActualHeight > 0)
                {
                    double pointX = p.ActualWidth;
                    double pointY = p.ActualHeight;

                    RectangleGeometry g1 = new RectangleGeometry(new Rect(new Point(0, 0), new Point(pointX - Offset, pointY - Offset)));
                    PathGeometry g2 = new PathGeometry();

                    if (ShowTip)
                    {
                        PathFigure p1 = new PathFigure();
                        Point startPoint;
                        Point midPoint;
                        Point endPoint;

                        if (TipAlignment.Equals("Top"))
                        {
                            double top = pointY - TipHeight - Offset;
                            g1 = new RectangleGeometry(new Rect(new Point(0, 0), new Point(pointX - Offset, top)));

                            startPoint = new Point(TipOffSet, top);
                            midPoint = new Point(TipWidth / 2 + TipOffSet, top + TipHeight);
                            endPoint = new Point(TipWidth + TipOffSet, top);
                        }
                        else if (TipAlignment.Equals("Bottom"))
                        {
                            double bottom = TipHeight;
                            g1 = new RectangleGeometry(new Rect(new Point(0, bottom), new Point(pointX, pointY)));

                            startPoint = new Point(0 + TipOffSet, bottom);
                            midPoint = new Point(TipWidth / 2 + TipOffSet, 0);
                            endPoint = new Point(TipWidth + TipOffSet, bottom);
                        }
                        else if (TipAlignment.Equals("Left"))
                        {
                            double left = TipHeight;
                            g1 = new RectangleGeometry(new Rect(new Point(left, 0), new Point(pointX, pointY)));

                            startPoint = new Point(left, TipOffSet);
                            midPoint = new Point(0, TipWidth / 2 + TipOffSet);
                            endPoint = new Point(left, TipWidth + TipOffSet);
                        }
                        else
                        {
                            ////this.tipAlignment equals TipAlignmentType.Right
                            double right = pointX - TipHeight;
                            g1 = new RectangleGeometry(new Rect(new Point(0, 0), new Point(right, pointY)));

                            startPoint = new Point(right, TipOffSet);
                            midPoint = new Point(pointX, TipWidth / 2 + TipOffSet);
                            endPoint = new Point(right, TipWidth + TipOffSet);
                        }

                        p1.StartPoint = startPoint;
                        LineSegment l1 = new LineSegment(midPoint, true);
                        LineSegment l2 = new LineSegment(endPoint, true);
                        p1.Segments.Add(l1);
                        p1.Segments.Add(l2);
                        g2.Figures.Add(p1);

                        BorderPath.Data = new CombinedGeometry(GeometryCombineMode.Union, g1, g2);
                    }
                    else
                    {
                        BorderPath.Data = g1;
                    }

                    if (EnableDropShadow)
                    {
                        if (ShowTip)
                        {
                            BorderPathShadow.Data = new CombinedGeometry(GeometryCombineMode.Union, g1, g2);
                        }
                        else
                        {
                            if (isPopup)
                            {
                                //popup
                                BorderPathShadow.Data = g1;
                            }
                            else
                            {
                                //images,watermark textbox
                                BorderPathShadow.Data = new RectangleGeometry(new Rect(new Point(0, 0), new Point(pointX, pointY)));
                            }
                        }
                        
                        BorderPathShadow.Style = this.FindResource(ShadowStyle) as Style;
                    }
                }
            }
        }

        /// <summary>
        /// ShowTip property
        /// </summary>
        public bool ShowTip
        {
            get { return Convert.ToBoolean(GetValue(ShowTipProperty).ToString()); }
            set { SetValue(ShowTipProperty, value); }
        }

        public static DependencyProperty ShowTipProperty = DependencyProperty.Register("ShowTip", typeof(bool), typeof(DropShadowBorder));

        /// <summary>
        /// TipOffSet property
        /// </summary>
        public double TipOffSet
        {
            get { return Convert.ToDouble(GetValue(TipOffSetProperty).ToString()); }
            set { SetValue(TipOffSetProperty, value); }
        }

        public static DependencyProperty TipOffSetProperty = DependencyProperty.Register("TipOffSet", typeof(double), typeof(DropShadowBorder));

        /// <summary>
        /// TipWidth property
        /// </summary>
        public double TipWidth
        {
            get { return Convert.ToDouble(GetValue(TipWidthProperty).ToString()); }
            set { SetValue(TipWidthProperty, value); }
        }

        public static DependencyProperty TipWidthProperty = DependencyProperty.Register("TipWidth", typeof(double), typeof(DropShadowBorder));

        /// <summary>
        /// TipHeight property
        /// </summary>
        public double TipHeight
        {
            get { return Convert.ToDouble(GetValue(TipHeightProperty).ToString()); }
            set { SetValue(TipHeightProperty, value); }
        }

        public static DependencyProperty TipHeightProperty = DependencyProperty.Register("TipHeight", typeof(double), typeof(DropShadowBorder));

        /// <summary>
        /// TipAlignment property
        /// </summary>
        public string TipAlignment
        {
            get { return GetValue(TipAlignmentProperty).ToString(); }
            set { SetValue(TipAlignmentProperty, value); }
        }

        public static DependencyProperty TipAlignmentProperty = DependencyProperty.Register("TipAlignment", typeof(string), typeof(DropShadowBorder));

        /// <summary>
        /// enableDropShadow variable
        /// </summary>
        private bool enableDropShadow = true;

        /// <summary>
        /// EnableDropShadow property
        /// </summary>
        public bool EnableDropShadow
        {
            get
            { return enableDropShadow; }
            set
            { enableDropShadow = value; }
        }        

        public string ShadowStyle
        {
            get { return GetValue(ShadowStyleProperty).ToString(); }
            set { SetValue(ShadowStyleProperty, value); }
        }

        public static DependencyProperty ShadowStyleProperty = DependencyProperty.Register("ShadowStyle", typeof(string), typeof(DropShadowBorder));

        /// <summary>
        /// Offset property
        /// </summary>
        public double Offset
        {
            get { return Convert.ToDouble(GetValue(OffsetProperty).ToString()); }
            set { SetValue(OffsetProperty, value); }
        }

        public static DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(DropShadowBorder));
                

        /// <summary>
        /// ShadowMarginIsPopup property
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }        

        /// <summary>
        /// isPopup variable
        /// </summary>
        private bool isPopup = true;
    }
}
