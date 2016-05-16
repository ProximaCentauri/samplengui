using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;

namespace SSCOControls
{
    public class WatermarkTextBox : PreviewTextBox, ISupportUserInput
    {
        public WatermarkTextBox()
        {
            this.Loaded += TextBox_Loaded;
            this.IsEnabledChanged += TextBox_IsEnabledChanged;         
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.caret = this.Template.FindName("PART_Caret", this) as Border;
            this.clearButton = this.Template.FindName("PART_ClearButton", this) as Button;
            if (null != this.clearButton)
            {
                this.clearButton.Click += clearButton_Click;
            }
            this.contentHost = this.Template.FindName("PART_ContentHost", this) as Control;
            if (null != this.contentHost)
            {
                this.contentHost.TouchDown += contentHost_TouchDown;
                this.contentHost.TouchUp += contentHost_TouchUp;
            }            
        }

        public bool IsWatermarkShown
        {
            get
            {
                return Convert.ToBoolean(GetValue(IsWatermarkShownProperty));
            }
            protected set
            {
                SetValue(IsWatermarkShownProperty, value);
            }
        }

        public bool KeyboardFocus
        {
            get
            {
                return Convert.ToBoolean(GetValue(KeyboardFocusProperty));
            }
            set
            {
                SetValue(KeyboardFocusProperty, value);
            }
        }

        public string Watermark
        {
            get
            {
                return GetValue(WatermarkProperty) as string;
            }
            set
            { 
                SetValue(WatermarkProperty, value);
            }
        }

        public bool PasswordMode 
        {
            get
            {
                return Convert.ToBoolean(GetValue(PasswordModeProperty));
            }
            set
            {
                SetValue(PasswordModeProperty, value);
            }
        }

        public string PasswordText
        {
            get { return GetValue(PasswordTextProperty) as string; }
            set { SetValue(PasswordTextProperty, value); }
        }

        public char PasswordChar
        {
            get { return (char)(GetValue(PasswordCharProperty)); }
            set { SetValue(PasswordTextProperty, value); }
        }

        public bool AllowUserInput
        {
            get { return (bool)GetValue(AllowUserInputProperty); }
            set { SetValue(AllowUserInputProperty, value); }
        }

        public Style ClearButtonStyle
        {
            get { return (Style)GetValue(ClearButtonStyleProperty); }
            set { SetValue(ClearButtonStyleProperty, value); }
        }
        
        public event RoutedEventHandler KeyboardFocusEventHandler
        {
            add { AddHandler(KeyboardFocusEvent, value); }
            remove { RemoveHandler(KeyboardFocusEvent, value); }
        }

        public static readonly RoutedEvent KeyboardFocusEvent = EventManager.RegisterRoutedEvent("KeyboardFocus", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(WatermarkTextBox));

        public static readonly DependencyProperty AllowUserInputProperty = DependencyProperty.Register("AllowUserProperty", typeof(bool),
            typeof(WatermarkTextBox), new PropertyMetadata(true, new PropertyChangedCallback(OnAllowUserInputChanged)));

        public static readonly DependencyProperty KeyboardFocusProperty = DependencyProperty.Register("KeyboardFocus", typeof(bool),
            typeof(WatermarkTextBox), new PropertyMetadata(false, new PropertyChangedCallback(OnKeyboardFocusChanged)));

        public static DependencyProperty IsWatermarkShownProperty = DependencyProperty.Register("IsWatermarkShown", typeof(bool),
            typeof(WatermarkTextBox));

        public static DependencyProperty PasswordModeProperty = DependencyProperty.Register("PasswordMode", typeof(bool), typeof(WatermarkTextBox));

        public static DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox),
            new PropertyMetadata(new PropertyChangedCallback(OnWatermarkChanged)));

        public static DependencyProperty ClearButtonStyleProperty = DependencyProperty.Register("ClearButtonStyle", typeof(Style),
            typeof(WatermarkTextBox));

        public static DependencyProperty PasswordTextProperty = DependencyProperty.Register("PasswordText", typeof(string), typeof(WatermarkTextBox),
            new UIPropertyMetadata(String.Empty));

        public static DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar", typeof(char), typeof(WatermarkTextBox),
            new UIPropertyMetadata('*'));

        protected override void OnPreviewTextChanged(PreviewTextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Watermark) && IsWatermarkShown)
            {
                this.IsWatermarkShown = false;
                this.Text = " ";
                e.Handled = true;
                this.SelectionStart = 1;                
            } 
            base.OnPreviewTextChanged(e);
        }
        
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!String.IsNullOrEmpty(Watermark) && IsWatermarkShown)
            {
                IsWatermarkShown = false;
                Text = e.Text;
                e.Handled = true;
                SelectionStart = 1;                
            } 
            base.OnPreviewTextInput(e);
        }

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            ShowCaret();
            base.OnSelectionChanged(e);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if (PasswordMode && PasswordText.Length < MaxLength)
            {
                PasswordText += e.Text;
                this.Text += PasswordChar;
                this.Select(this.Text.Length, this.Text.Length); // sets cursor in the left.
            }
            else
            {
                base.OnTextInput(e);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Watermark) && IsWatermarkShown && !Text.Equals(Watermark))
            {
                if (e.Changes.Count > 0)
                {
                    foreach (TextChange change in e.Changes)
                    {
                        IsWatermarkShown = false;
                        Text = Text.Substring(change.Offset, change.AddedLength);
                        e.Handled = true;
                        SelectionStart = 1;
                        break;
                    }
                }
            } 
            base.OnTextChanged(e);
            ShowWatermark();
            if (PasswordMode && !PasswordText.Length.Equals(Text.Length))
            {
                PasswordText = PasswordText.Remove(Text.Length);
            }
        }

        protected override bool IsEnabledCore
        {
            get
            {
                return base.IsEnabledCore && AllowUserInput;
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void contentHost_TouchDown(object sender, TouchEventArgs e)
        {
            Control content = sender as Control;
            if (null != content)
            {
                TouchPoint tp = e.GetTouchPoint(content);
                Rect bounds = new Rect(new Point(0, 0), content.RenderSize);
                if (bounds.Contains(tp.Position))
                {
                    WatermarkTextBox tb = content.TemplatedParent as WatermarkTextBox;
                    if (null != tb)
                    {
                        tb.KeyboardFocus = true;
                    }
                }
                content.ReleaseTouchCapture(e.TouchDevice);
            }
        }

        private void contentHost_TouchUp(object sender, TouchEventArgs e)
        {
            Control content = sender as Control;
            if (null != content)
            {
                content.CaptureTouch(e.TouchDevice);
            }
        }

        private static void OnAllowUserInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(UIElement.IsEnabledProperty);
        }

        private static void OnKeyboardFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WatermarkTextBox tb = d as WatermarkTextBox;
            tb.ShowCaret();
            if (tb.KeyboardFocus)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(WatermarkTextBox.KeyboardFocusEvent);
                tb.RaiseEvent(newEventArgs);
            }
        }

        private static void OnWatermarkChanged(DependencyObject sender, DependencyPropertyChangedEventArgs ea)
        {
            WatermarkTextBox textBox = sender as WatermarkTextBox;
            if (textBox.IsWatermarkShown)
            {
                textBox.ShowWatermark();
            }
        }

        private void ShowCaret()
        {
            if (null != this.caret && null != this.contentHost)
            {
                Rect caretRect = GetRectFromCharacterIndex(this.CaretIndex);
                if (caretRect.Right < this.contentHost.RenderSize.Width)
                {
                    if (!double.IsInfinity(caretRect.X))
                    {
                        Canvas.SetLeft(this.caret, caretRect.X);
                    }
                    if (!double.IsInfinity(caretRect.Y))
                    {
                        Canvas.SetTop(this.caret, caretRect.Y);
                    }
                    if (!double.IsInfinity(caretRect.Height))
                    {
                        this.caret.Height = caretRect.Height;
                    }
                    this.caret.Visibility = this.IsEnabled && this.KeyboardFocus ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    this.caret.Visibility = Visibility.Collapsed;
                }                
            }
        }

        private void ShowWatermark()
        {
            bool canClear = !this.IsWatermarkShown && this.Text.Length > 0;
            if (!IsWatermarkShown && 0 == Text.Length && !String.IsNullOrEmpty(Watermark))
            {
                this.IsWatermarkShown = true;
                this.Text = Watermark;
            }
            if (null != this.clearButton)
            {
                this.clearButton.IsEnabled = canClear;
            }
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WatermarkTextBox tb = sender as WatermarkTextBox;
            tb.ShowCaret();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ShowWatermark();            
            ShowCaret();
            if (this.KeyboardFocus)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(WatermarkTextBox.KeyboardFocusEvent);
                RaiseEvent(newEventArgs);
            }
        }

        private Border caret;
        private Button clearButton;
        private Control contentHost;        
    }
}
