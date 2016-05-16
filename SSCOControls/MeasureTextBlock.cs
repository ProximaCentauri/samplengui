﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Text;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using RPSWNET;

namespace SSCOControls
{
    public class MeasureTextBlock : TextBlock
    {
        public MeasureTextBlock()
        {
            this.IsVisibleChanged += OnVisibilityChanged;
            this.Loaded += OnLoaded;
            this.textChangeNotifier = new PropertyChangeNotifier(this, "Text");
            this.textChangeNotifier.ValueChanged += new EventHandler(OnTextChanged);
        }

        public string FocusSound
        {
            get
            {
                return GetValue(FocusSoundProperty) as string;
            }
            set
            {
                SetValue(FocusSoundProperty, value);
            }
        }

        public static PrimaryLanguageCallback LanguageCallback { get; set; }

        public static string TopLevelNameSpace { get; set; }
        
        public static bool UNav { get; set; }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (UNav)
            {
                string audio = null != this.FocusSound ? this.FocusSound : this.Text;
                if (null != audio && audio.Length > 0)
                {
                    ControlsAudio.PlaySound('#' + audio + '#');
                }
            }
        }

        private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MeasureTextBlock textBlock = sender as MeasureTextBlock;
            if (null != textBlock)
            {
                if (textBlock.IsVisible)
                {
                    textBlock.MeasureAndResize();
                }
            }
        }

        public double PreferredFontSize
        {
            get
            {
                double value = 0.0;
                double.TryParse(GetValue(PreferredFontSizeProperty).ToString(), out value);
                return value;
            }
            set
            {
                SetValue(PreferredFontSizeProperty, value);
            }
        }

        public double MinimumFontSize
        {
            get
            {
                double value = 0.0;
                double.TryParse(GetValue(MinimumFontSizeProperty).ToString(), out value);
                return value;
            }
            set
            {
                SetValue(MinimumFontSizeProperty, value);
            }
        }

        public bool NMI
        {
            get
            {
                return Convert.ToBoolean(GetValue(NMIProperty));
            }
            set
            {
                SetValue(NMIProperty, value);
            }
        }
        
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (this.NMI)
            {
                InitializeTextBlock(this.PreferredFontSize);
            }
            this.waitForSizeChange = false;
            MeasureAndResize();
        }
        
        private static bool CheckDisplaySize(string[] deviceIDList)
        {
            foreach (string s in deviceIDList)
            {
                if (DeviceID.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
        
        private String ControlHierarchyTrace<T>(FrameworkElement child) where T : DependencyObject
        {
            FrameworkElement parent = child;
            StringBuilder controlHierarchy = new StringBuilder(ControlName(parent));
            while (null != (parent = VisualTreeHelper.GetParent(parent) as FrameworkElement))
            {
                controlHierarchy.Insert(0, ControlName(parent) + ".");
                if (null != TopLevelNameSpace && parent.DependencyObjectType.SystemType.Namespace.Contains(TopLevelNameSpace))
                {
                    break;
                }
            }
            return controlHierarchy.ToString();
        }

        private static string ControlName(FrameworkElement control)
        {
            return !String.IsNullOrEmpty(control.Name) ? control.Name : control.DependencyObjectType.Name;
        }

        private double FindBiggestFontSizeThatFits(double currentFontSize, double minimumFontSize, double preferredFontSize)
        {
            if (currentFontSize == minimumFontSize)
            {
                return currentFontSize;
            }
            else if (currentFontSize == preferredFontSize)
            {
                return currentFontSize - 1;
            }
            else
            {
                if (MeasureString(currentFontSize, false))
                {
                    return FindBiggestFontSizeThatFits(Math.Round(((preferredFontSize - currentFontSize) / 2) + currentFontSize), currentFontSize,
                        preferredFontSize);
                }
                else
                {
                    return FindBiggestFontSizeThatFits(Math.Round(((currentFontSize - minimumFontSize) / 2) + minimumFontSize), minimumFontSize, currentFontSize);
                }
            }
        }

        private CultureInfo GetCurrentCulture()
        {
            CultureInfo currentCulture = Application.Current.Dispatcher.Thread.CurrentUICulture;
            if (null != LanguageCallback && !LanguageCallback(this))
            {
                currentCulture = LocalizationManager.SecondaryCultureInfo;
            }
            return currentCulture;
        }

        private static void GetDeviceID()
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);
            try
            {
                for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        d.cb = Marshal.SizeOf(d);
                        EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "Device Name is: {0}, Device ID:{1}", d.DeviceName, d.DeviceID);

                        if (!d.DeviceName.Equals(DISPLAY1))
                        {
                            DeviceID = d.DeviceID;
                        }
                    }
                    d.cb = Marshal.SizeOf(d);
                }
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "Device ID:{0}", DeviceID);
            }
            catch (Exception ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "ERROR in retrieving the device ID. {0}", ex.ToString());
            }
        }

        private FormattedText GetFormattedText(string text, Typeface typeface, double fontSize)
        {
            FormattedText formattedText = new FormattedText(text, Application.Current.Dispatcher.Thread.CurrentUICulture, this.FlowDirection, typeface,
                fontSize, this.Foreground);
            formattedText.TextAlignment = this.TextAlignment;
            formattedText.Trimming = this.TextTrimming;
            return formattedText;
        }
                
        private void InitializeTextBlock(double fontSize)
        {
            if (this.NMI)
            {
                if (DeviceID.Equals(string.Empty))
                {
                    GetDeviceID();
                }
                ManageMeasurements(this, fontSize);
            }
        }
        
        private bool IsCKJ()
        {
            CultureInfo currentCulture = GetCurrentCulture();
            return currentCulture.TwoLetterISOLanguageName.Equals("ja") || currentCulture.TwoLetterISOLanguageName.Equals("ko") ||
                currentCulture.TwoLetterISOLanguageName.Equals("zh") || currentCulture.TwoLetterISOLanguageName.Equals("th");
        }
        
        /// <summary>
        /// Method to manipulate font sizes for previous item and tax line
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="fontSize"></param>
        private static void IsNotSelectedItemMeasurements(MeasureTextBlock textBlock, double fontSize)
        {
            double newFontSizeMultiplier = 1.6;
            if (CheckDisplaySize(DISPLAY2_6POINT5INCH))
            {
                textBlock.FontSize = fontSize * 2.6; // 4mm for 6.5" display            
            }
            else if (CheckDisplaySize(DISPLAY2_15INCH))
            {
                textBlock.FontSize = fontSize * newFontSizeMultiplier; // 6mm for 15" display
            }
            else
            {
                textBlock.FontSize = fontSize * newFontSizeMultiplier;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "Setting default font size:{0} for previous item, Device ID:{1} not supported.",
                    textBlock.FontSize, DeviceID);
            }
        }

        /// <summary>
        /// Method to manipulate font sizes for current item and total line
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="fontSize"></param>
        private static void IsSelectedItemMeasurements(MeasureTextBlock textBlock, double fontSize)
        {
            double newFontSizeMultiplier = 2.1;
            if (CheckDisplaySize(DISPLAY2_6POINT5INCH))
            {
                textBlock.FontSize = fontSize * newFontSizeMultiplier; // 9.5mm meet NMI requirements for 6.5" display
            }
            else if (CheckDisplaySize(DISPLAY2_15INCH))
            {
                textBlock.FontSize = fontSize; // 9.5mm meet NMI requirements for 15" display
            }
            else
            {
                textBlock.FontSize = fontSize;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "Setting default font size:{0} for current item, Device ID:{1} not supported.",
                    textBlock.FontSize, DeviceID);
            }
        }

        private void LogClippedText(double rectWidth, double rectHeight, double textWidth, double textHeight, double fontSize)
        {
            if (!this.logged)
            {
                this.logged = true;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive,
                    "MeasureTextBlock:LogClippedText(), Location:{0}, Language:{1}, ControlSize({2}, {3}), TextSize({4}, {5}), FontSize:{6} Text:{7}, Text String does not fit.",
                    ControlHierarchyTrace<DependencyObject>(this), GetCurrentCulture().LCID.ToString("X4"), rectWidth, rectHeight, textWidth, textHeight, fontSize, this.Text);
            }
        }

        private static void ManageMeasurements(MeasureTextBlock textBlock, double fontSize)
        {
            if (fontSize == selectedItemFontSize)
            {
                IsSelectedItemMeasurements(textBlock, fontSize);
            }
            else if(!Double.IsNaN(fontSize))
            {
                IsNotSelectedItemMeasurements(textBlock, fontSize);
            }
        }
                
        private void MeasureAndResize()
        {
            bool canResize = !this.NMI && !Double.IsNaN(this.MinimumFontSize) && !this.waitForSizeChange && this.FontSize > this.MinimumFontSize;
            if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive) || canResize)
            {
                if (!MeasureString(this.FontSize, true) && canResize)
                {
                    ResizeFontSize();
                }                
            }            
        }

        private bool MeasureString(double fontSize, bool log)
        {
            bool fits = true;
            if (null != this.VisualClip)
            {
                Size size = new Size(this.VisualClip.Bounds.Width - this.Padding.Left - this.Padding.Right,
                    this.VisualClip.Bounds.Height - this.Padding.Top - this.Padding.Bottom);
                if (this.IsVisible && null != this.Text && this.Text.Length > 0 && size.Width > 0 && size.Height > 0)
                {
                    Typeface typeface = new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch);
                    size.Width = Math.Round(size.Width, 2);
                    size.Height = Math.Round(size.Height, 2);
                    double textWidth = 0;
                    double textHeight = 0;
                    string[] words = this.Text.Split(wordSeparator);
                    foreach (string word in words)
                    {
                        FormattedText formattedWord = GetFormattedText(word, typeface, fontSize);
                        textWidth = Math.Round(formattedWord.Width, 2);
                        textHeight = Math.Round(formattedWord.Height, 2);
                        if (textWidth > size.Width || textHeight > size.Height)
                        {
                            fits = false;                                                        
                            break;
                        }
                    }
                    if (fits)
                    {
                        FormattedText formattedText = GetFormattedText(this.Text, typeface, fontSize);
                        formattedText.MaxTextWidth = this.VisualClip.Bounds.Width;
                        textWidth = Math.Round(formattedText.Width, 2);
                        textHeight = Math.Round(formattedText.Height, 2);
                        if (textWidth > size.Width || textHeight > size.Height)
                        {
                            fits = false;
                        }
                    }
                    if (!fits && log && CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        LogClippedText(size.Width, size.Height, textWidth, textHeight, fontSize);
                    }
                }
            }
            return fits;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MeasureTextBlock textBlock = sender as MeasureTextBlock;
            if (null != textBlock)
            {
                textBlock.InitializeTextBlock(textBlock.PreferredFontSize);
            }
        }

        private static void OnMinimumFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != e.NewValue)
            {
                MeasureTextBlock textBlock = d as MeasureTextBlock;
                if (null != textBlock && !String.IsNullOrEmpty(textBlock.Text))
                {
                    textBlock.MeasureAndResize();
                }
            }
        }

        private static void OnPreferredFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != e.NewValue)
            {
                double fontSize = (double)e.NewValue;
                MeasureTextBlock textBlock = d as MeasureTextBlock;
                if (null != textBlock)
                {
                    if (!Double.IsNaN(fontSize))
                    {
                        textBlock.FontSize = fontSize;
                    }
                    if (!String.IsNullOrEmpty(textBlock.Text))
                    {
                        if (textBlock.NMI)
                        {
                            if (Double.IsNaN(fontSize))
                            {
                                fontSize = 17.0;
                            }
                            ManageMeasurements(textBlock, fontSize);
                        }
                        textBlock.MeasureAndResize();
                    }
                }
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            MeasureTextBlock textBlock = sender as MeasureTextBlock;
            if (null != textBlock)
            {
                textBlock.logged = false;
                if (!textBlock.NMI && !Double.IsNaN(textBlock.MinimumFontSize) && textBlock.PreferredFontSize > 0)
                {
                    if (textBlock.Text.Length > 0 && !IsCKJ() && 1 == textBlock.Text.Split(wordSeparator).Length)
                    {
                        textBlock.noWrapLayoutChange = true;
                        textBlock.TextWrapping = TextWrapping.NoWrap;
                    }
                    else if (textBlock.noWrapLayoutChange)
                    {
                        textBlock.noWrapLayoutChange = false;
                        textBlock.TextWrapping = TextWrapping.Wrap;
                    }
                    if (textBlock.TextWrapping == TextWrapping.WrapWithOverflow)
                    {
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning,
                            "MeasureTextBlock.OnTextChanged(), Text:{0} - Warning TextWrapping=WrapWithOverflow not supported with resize. Changing to Wrap",
                            textBlock.Text);
                    }                                
                    textBlock.waitForSizeChange = true;
                    textBlock.FontSize = this.PreferredFontSize;
                }
            }
        }
               
        private void ResizeFontSize()
        {
            this.logged = false;
            this.FontSize = FindBiggestFontSizeThatFits(Math.Round(((this.FontSize - this.MinimumFontSize) / 2) + this.MinimumFontSize), this.MinimumFontSize,
                this.PreferredFontSize);
            CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive,
                "MeasureTextBlock.ResizeFontSize(), PreferredFontSize:{0}, FontSize:{1}, Control:{2}, Text:{3}", this.PreferredFontSize, this.FontSize,
                ControlHierarchyTrace<DependencyObject>(this), this.Text);                
        }

        private static DependencyProperty FocusSoundProperty = DependencyProperty.Register("FocusSound", typeof(string), typeof(MeasureTextBlock));        
        private static DependencyProperty MinimumFontSizeProperty = DependencyProperty.Register("MinimumFontSize", typeof(double), typeof(MeasureTextBlock),
            new PropertyMetadata(Double.NaN, OnMinimumFontSizeChanged));
        private static DependencyProperty NMIProperty = DependencyProperty.Register("NMI", typeof(bool), typeof(MeasureTextBlock));
        private static DependencyProperty PreferredFontSizeProperty = DependencyProperty.Register("PreferredFontSize", typeof(double), typeof(MeasureTextBlock),
           new PropertyMetadata(Double.NaN, OnPreferredFontSizeChanged));        
        
        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);
                
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [Flags()]
        private enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        private static string DeviceID = string.Empty;
        /// <summary>
        /// Primary display device name
        /// </summary>
        private const string DISPLAY1 = "\\\\.\\DISPLAY1\\Monitor0";
        /// <summary>
        /// Display Device ID with 6.5" dimension
        /// </summary>
        private static readonly string[] DISPLAY2_6POINT5INCH = { "NCR5982", "Monitor\\NCR0000\\{4D36E96E-E325-11CE-BFC1-08002BE10318}\\0016" };
        /// <summary>
        /// Display Device ID with 15" dimension
        /// </summary>
        private static readonly string[] DISPLAY2_15INCH = { "PNL0013" };
        /// <summary>
        /// Font size for selected item on 15" dimension. 
        /// </summary>
        private const double selectedItemFontSize = 46.0;

        private bool logged;
        private bool noWrapLayoutChange;
        private PropertyChangeNotifier textChangeNotifier;
        private bool waitForSizeChange;        
        private static readonly char[] wordSeparator = new char[] { ' ', '\n' };
    }
}