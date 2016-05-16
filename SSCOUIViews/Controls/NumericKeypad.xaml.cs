using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using SSCOControls;
using SSCOUIModels;
using SSCOUIModels.Helpers;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for NumericKeypad.xaml
    /// </summary>
    public partial class NumericKeypad : Grid
    {
        public NumericKeypad()
        {
            InitializeComponent();
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void NumericKeypad_Loaded(object sender, RoutedEventArgs e)
        {
            TelephoneTypeChanged();
        }

        private void TelephoneTypeChanged()
        {
            if (Convert.ToBoolean(TelephoneType))
            {
                firstRow.SetValue(Grid.RowProperty, 0);
                thirdRow.SetValue(Grid.RowProperty, 2);
            }
            else
            {
                firstRow.SetValue(Grid.RowProperty, 2);
                thirdRow.SetValue(Grid.RowProperty, 0);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            { 
                case "StateParam":
                    OnStateParamChanged(viewModel.StateParam);
                    break;
                case "UIEchoField":
                    UpdateUIEchoField();
                    break;
                case "NextGenData":
                    UpdateInstructions();
                    break;
                case "MessageBoxEcho":
                    UpdateInstructionTextBox();
                    break;
                case "StoreMode":
                    UpdateUIEchoField();
                    if (viewModel.StateParam.Equals("XMCashierPassword"))
                    {
                        BindingTextProperty("PasswordText");
                    }
                    else
                    {
                        BindingTextProperty("Text");
                    }
                    UpdateInstructions();
                    break;
            }          
        }

        private void OnStateParamChanged(String param)
        {
            inputValue = "";
            InputTextBox.Clear();
            OKButton.IsEnabled = false;
            InputTextBox.PasswordMode = false;
            BindingTextProperty("Text");
            MinInput = 1;
            UpdateUIEchoField();
            UpdateInstructions();
            UpdateElements();
            DecimalMarkKey.IsEnabled = true;
            MinusKey.IsEnabled = true;
            
            if (param.Equals("XMCashierPassword"))
            {
                this.InputTextBox.PasswordMode = true;
                BindingTextProperty("PasswordText");
            }
            else if (param.Equals("SmmKeyInWtTol"))
            {
                UpdateDecimalMarkKeyText();
            }
        }

        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            WatermarkTextBox textBox = null;
            View view = UIControlFinder.FindAncestorOrSelf<View>(this);
            if (null != view)
            {
                textBox = view.KeyboardFocusedElement as WatermarkTextBox;
            }
            if (null == textBox)
            {
                return;
            }
            ImageButton button = sender as ImageButton;
            if (null != button.CommandParameter)
            {
                Key key = Key.None;
                try
                {
                    key = (Key)new KeyConverter().ConvertFromString(button.CommandParameter.ToString());
                }
                catch (InvalidCastException)
                {
                }
                if (key != Key.None)
                {
                    if (key.Equals(Key.Back))
                    {
                        if (inputValue.Length > 0)
                        {
                            inputValue = inputValue.Substring(0, inputValue.Length - 1);
                        }

                        if (InputTextBox.Text[InputTextBox.Text.Length - 1] == '.')
                        {
                            DecimalMarkKey.IsEnabled = true;
                        }
                    }

                    int textBoxValue;
                    int.TryParse(inputValue, out textBoxValue);

                    if ((viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled()) && (inputValue.Length == 0 || textBoxValue < 1))
                    {
                        textBox.Clear();
                    }

                    textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(textBox), 0, key) { RoutedEvent = Keyboard.KeyDownEvent });

                    if ((viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled()) && inputValue.Length > 0)
                    {
                        OKButton.IsEnabled = calculateTotalAmount("") > 0;
                    }
                }
            }
            else
            {
                if (Char.IsDigit(button.Text[0]) && inputValue.Length < textBox.MaxLength)
                {
                    if (!((viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled()) && 
                        inputValue.Length == 0 && button.Text.Equals("0")))
                    {
                        textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                            new TextComposition(InputManager.Current, textBox, button.Text)) { RoutedEvent = TextCompositionManager.TextInputEvent });

                        if (viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled())
                        {
                            OKButton.IsEnabled = calculateTotalAmount(button.Text) > 0;
                        }
                    }
                }
                else
                {

                    textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                        new TextComposition(InputManager.Current, textBox, button.Text)) { RoutedEvent = TextCompositionManager.TextInputEvent });
                    Key key = KeyCode(button.Text);
                    if (key == Key.Decimal)
                    {
                        DecimalMarkKey.IsEnabled = false;
                    }
                    else if(key == Key.OemMinus)
                    {
                        MinusKey.IsEnabled = false;
                    }
                }
            }
        }

        public Key KeyCode(string key)
        {
            Key finalKey = Key.None;
            switch (key)
            {
                case ".":
                    key = "DECIMAL";
                    break;
                case "-":
                    key = "MINUS";
                    break;              
            }
            try
            {
                finalKey = (Key)new KeyConverter().ConvertFromString(key);
            }
            catch (Exception)
            {
                finalKey = Key.None;
            }
            return finalKey;
        }

        private void UpdateUIEchoField()
        {
            MinInput = viewModel.UIEchoField.MinLength;
            InputTextBox.MaxLength = viewModel.UIEchoField.MaxLength;
            UpdateNumericTextBoxStyle();
        }

        private bool IsCurrencyEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmDataEntryNumericSmall":
                case "SmDataEntryWithListBG":
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                case "XMCashReplenish":
                    return viewModel.UIEchoField.CurrencyEnabled ? true : false;
            }
            return false;
        }

        private bool IsTrimDecimalEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                case "XMCashReplenish":
                    return viewModel.UIEchoField.TrimDecimalEnabled ? true : false;
            }
            return false;
        }

        /// <summary>
        /// If true, keypad number starts with 1 2 3
        /// else, keypad number starts with 7 8 9
        /// </summary>
        public bool TelephoneType
        {
            get
            {
                return Convert.ToBoolean(GetValue(TelephoneTypeProperty));
            }
            set
            {
                SetValue(TelephoneTypeProperty, value);                
            }
        }

        public static DependencyProperty TelephoneTypeProperty = DependencyProperty.Register("TelephoneType", typeof(bool), typeof(NumericKeypad));
        
        private void UpdateNumericTextBoxStyle()
        {
            if (InputTextBox.MaxLength >= 7 && InputTextBox.MaxLength < 10)
            {
                InputTextBox.Style = this.FindResource("mediumNumericTextBoxStyle") as Style;
            }
            else if (InputTextBox.MaxLength >= 10)
            {
                InputTextBox.Style = this.FindResource("smallNumericTextBoxStyle") as Style;
            }
            else
            {
                InputTextBox.Style = this.FindResource("numericTextBoxStyle") as Style;
            }
        }

        private void UpdateInstructions()
        {
            if (viewModel.StateParam.Equals("EnterId") || viewModel.StateParam.Equals("SmCashierPassword"))
            {
                this.InputTextBox.Clear();
                if (viewModel.GetPropertyValue("NextGenData").Equals("EnterPassword"))
                {
                    this.OKButton.IsEnabled = false;
                    this.MinInput = 1;
                    this.InputTextBox.PasswordMode = true;
                    BindingTextProperty("PasswordText");
                }
                else if (viewModel.GetPropertyValue("NextGenData").Equals("EnterID"))
                {
                    this.InputTextBox.PasswordMode = false;
                    BindingTextProperty("Text");
                }
            }
        }

        private void UpdateInstructionTextBox()
        {
            if (String.IsNullOrWhiteSpace(viewModel.GetPropertyValue("MessageBoxEcho").ToString()))
            {
                this.InputTextBox.Clear();
            }
        }

        private void UpdateElements()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmCashierPassword":
                case "SmDataEntryNumericSmall":
                case "SmDataEntryWithListBG":
                case "SmEnterBirthdate":
                case "SmLoadLift":
                case "SmmKeyInItemCode":
                case "SmmKeyInWtTol":
                case "AM_EnterCouponValue":
                case "AM_EnterQuantity":
                case "AM_KeyInCode":
                case "AM_KeyInPrice":
                case "AM_KeyInQuantity":
                    this.MainKeypadGrid.RowDefinitions[0].Height = new GridLength(48);
                    this.WaterMarkTextBoxBorder.Width = 286;
                    this.WaterMarkTextBoxBorder.Height = 48;
                    this.WaterMarkTextBoxBorder.Margin = new Thickness(0);
                    this.InputTextBox.Width = 286;
                    this.InputTextBox.Height = 48;
                    this.InputTextBox.Padding = new Thickness(0, 0, 0, 0);
                    this.KeypadGrid.Margin = new Thickness(31, 19, 30, 0);
                    this.KeypadGrid.Width = 225;
                    this.KeypadButton0.Width = 140;
                    this.BackspaceButton.Width = 65;                   
                    this.lastRow.Width = 225;
                    this.OKButton.Width = 215;
                    this.OKButton.Property(ImageButton.TextProperty).SetResourceValue("Enter");
                    this.UpdateAdditionalKeys();
                    this.UpdateStyles(true);                    
                    break;
                case "CmDataEntryWithKeyBoard":
                case "EnterCouponValue":
                case "EnterId":
                case "EnterQuantity":
                case "SellBags":
                    this.MainKeypadGrid.RowDefinitions[0].Height = new GridLength(64);
                    this.WaterMarkTextBoxBorder.Width = 236;
                    this.WaterMarkTextBoxBorder.Height = 54;
                    this.WaterMarkTextBoxBorder.Margin = new Thickness(5, 0, 5, 5);
                    this.InputTextBox.Width = 236;
                    this.InputTextBox.Height = 54;
                    this.InputTextBox.Padding = new Thickness(12, 0, 0, 0);
                    this.KeypadGrid.Margin = new Thickness(0);
                    this.KeypadGrid.Width = 246;
                    this.KeypadButton0.Width = 72;
                    this.BackspaceButton.Width = 72;
                    this.lastRow.Width = 246;
                    this.OKButton.Width = 72;
                    this.OKButton.Property(ImageButton.TextProperty).SetResourceValue("OK");
                    this.DecimalMarkKey.Visibility = Visibility.Collapsed;
                    this.MinusKey.Visibility = Visibility.Collapsed;
                    this.UpdateStyles(false);
                    break;
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                case "XMCashReplenish":
                    if (BackgroundViewMode)
                    {
                        this.MainKeypadGrid.RowDefinitions[0].Height = new GridLength(48);
                        this.WaterMarkTextBoxBorder.Width = 286;
                        this.WaterMarkTextBoxBorder.Height = 48;
                        this.WaterMarkTextBoxBorder.Margin = new Thickness(0);
                        this.InputTextBox.Width = 286;
                        this.InputTextBox.Height = 48;
                        this.InputTextBox.Padding = new Thickness(0, 0, 0, 0);
                        this.KeypadGrid.RowDefinitions[0].Height = new GridLength(60);
                        this.KeypadGrid.RowDefinitions[1].Height = new GridLength(60);
                        this.KeypadGrid.RowDefinitions[2].Height = new GridLength(60);
                        this.KeypadGrid.Margin = new Thickness(31, 5, 30, 0);
                        this.KeypadGrid.Width = 225;
                        this.KeypadButton0.Width = 140;
                        this.BackspaceButton.Width = 65;
                        this.lastRow.Width = 225;
                        this.OKButton.Width = 215;
                        this.OKButton.Property(ImageButton.TextProperty).SetResourceValue("Enter");
                        this.UpdateAdditionalKeys();
                        this.UpdateStyles(true);
                    }
                    break;
                case "XMCashierID":
                case "XMCashierPassword":
                    if (!BackgroundViewMode)
                    {
                        this.MainKeypadGrid.RowDefinitions[0].Height = new GridLength(64);
                        this.WaterMarkTextBoxBorder.Width = 236;
                        this.WaterMarkTextBoxBorder.Height = 54;
                        this.WaterMarkTextBoxBorder.Margin = new Thickness(5, 0, 5, 5);
                        this.InputTextBox.Width = 236;
                        this.InputTextBox.Height = 54;
                        this.InputTextBox.Padding = new Thickness(12, 0, 0, 0);
                        this.KeypadGrid.Margin = new Thickness(0);
                        this.KeypadGrid.Width = 246;
                        this.KeypadButton0.Width = 153;
                        this.BackspaceButton.Width = 72;
                        this.lastRow.Width = 246;
                        this.OKButton.Width = 236;
                        this.OKButton.Property(ImageButton.TextProperty).SetResourceValue("Enter");
                        this.UpdateAdditionalKeys();
                        this.UpdateStyles(true);
                    }
                    break;
            }
        }

        private void UpdateAdditionalKeys()
        {
            this.DecimalMarkKey.Visibility = Visibility.Collapsed;
            this.MinusKey.Visibility = Visibility.Collapsed;

            if (viewModel.StateParam.Equals("SmmKeyInWtTol"))
            {
                this.DecimalMarkKey.Visibility = Visibility.Visible;
                this.KeypadButton0.Width = 65;
            }
            else if (viewModel.StateParam.Equals("SmLoadLift"))
            {
                this.MinusKey.Visibility = Visibility.Visible;
                this.KeypadButton0.Width = 65;
            }
        }

        private void UpdateStyles(bool storeMode)
        {
            if (storeMode)
            {
                this.BackspaceButton.Style = this.FindResource("storeBackSpaceButtonStyle") as Style;
                this.OKButton.Style = this.FindResource("storeNumericOkButtonStyle") as Style;
                this.KeypadButton0.Style = this.FindResource("storeNumericMainButtonStyle") as Style;
                this.DecimalMarkKey.Style = this.FindResource("storeNumericMainButtonStyle") as Style;
                this.MinusKey.Style = this.FindResource("storeNumericMainButtonStyle") as Style;                
            }
            else
            {
                this.BackspaceButton.Style = this.FindResource("backSpaceButtonStyle") as Style;
                this.OKButton.Style = this.FindResource("numericOkButtonStyle") as Style;
                this.KeypadButton0.Style = this.FindResource("mainButtonStyle") as Style;
                this.DecimalMarkKey.Style = this.FindResource("mainButtonStyle") as Style;
                this.MinusKey.Style = this.FindResource("mainButtonStyle") as Style;
            }            
            this.UpdateButtonStyles(storeMode);            
        }

        private void UpdateButtonStyles(bool storeMode)
        {
            Style buttonStyle;
            UniformGrid[] rows = { firstRow, secondRow, thirdRow };

            if (storeMode)
            {
                buttonStyle = this.FindResource("storeNumericMainButtonStyle") as Style;
            }
            else
            {
                buttonStyle = this.FindResource("mainButtonStyle") as Style;
            }

            for (int i = 0; i < rows.Length; i++)
            {
                UpdateButtons(storeMode, rows[i], buttonStyle);
            }            
        }

        public void UpdateButtons(bool storeMode, UniformGrid buttonGrid, Style buttonStyle)
        {
            foreach (ImageButton button in UIControlFinder.FindVisualChildren<ImageButton>(buttonGrid))
            {
                button.Style = buttonStyle;
            }
        }

        private void UpdateDecimalMarkKeyText()
        {
            string decimalSeparator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            if (!String.IsNullOrEmpty(decimalSeparator))
            {
                DecimalMarkKey.Text = decimalSeparator;
            }
        }

        private double calculateTotalAmount(string initialAmount)
        {
            inputValue += initialAmount;

            priceTextBlock.IsTrimDecimal = IsCurrencyEnabled() && IsTrimDecimalEnabled() ? true : false;
            priceTextBlock.Value = inputValue;

            InputTextBox.Text = priceTextBlock.Text;
            InputTextBox.Select(InputTextBox.Text.Length, 0);

            return Convert.ToDouble(priceTextBlock.Text.Replace(RegionInfo.CurrentRegion.CurrencySymbol, ""));
        }

        private void BindingTextProperty(string propertyName)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyName);
            binding.ElementName = InputTextBox.Name;
            if (viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled())
            {
                binding.Converter = new FormatCurrencyConverter();
            }
            else if (viewModel.StateParam.Equals("SmmKeyInWtTol"))
            {
                binding.Converter = new FormatDecimalSeparatorConverter();
            }
            else
            {
                binding.Converter = new FormatConverter();
            }
            if (viewModel.StoreMode)
            {
                if (viewModel.StateParam.StartsWith("XMCashRemove") || viewModel.StateParam.Equals("XMCashReplenish"))
                {
                    binding.ConverterParameter = "CashManagementKeypad({0})";
                }
                else if (viewModel.StateParam.StartsWith("XM"))
                {
                    binding.ConverterParameter = "StoreCMEnterNumeric({0})";
                }
                else if (viewModel.StateParam.Equals("SmmKeyInItemCode"))
                {
                    binding.ConverterParameter = "StoreSmmEnterNumeric({0})";
                }
                else if (viewModel.StateParam.Equals("SmmKeyInWtTol"))
                {
                    binding.ConverterParameter = "StoreEnterNumeric({0},.,KeyBoardP3)";
                }
                else if (viewModel.StateParam.Equals("SmLoadLift"))
                {
                    binding.ConverterParameter = "StoreEnterNumeric({0},-,MinusKey)";
                }
                else
                {
                    binding.ConverterParameter = "StoreEnterNumeric({0})";
                }              
            }
            else
            {
                binding.ConverterParameter = "EnterNumeric({0})";
            }

            BindingOperations.SetBinding(OKButton, ImageButton.CommandParameterProperty, binding);
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !(e.Key == Key.System || e.Key == Key.Back || Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)));
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel.StateParam.Equals("EnterId") || viewModel.StateParam.Equals("SellBags") || viewModel.State.Equals("Store"))
            {
                OKButton.IsEnabled = InputTextBox.Text.Length > 0;
            }
            if (viewModel.StateParam.Equals("CmDataEntryWithKeyBoard") || (viewModel.StateParam.Equals("SmDataEntryWithListBG") || viewModel.StateParam.Equals("SmDataEntryNumericSmall") && !IsCurrencyEnabled()))
            {
                OKButton.IsEnabled = InputTextBox.Text.Length >= MinInput;
            }
            else if (viewModel.StateParam.Equals("EnterQuantity") || viewModel.StateParam.Equals("AM_EnterQuantity") || viewModel.StateParam.Equals("AM_KeyInQuantity"))
            {
                int input = InputTextBox.Text.Length > 0 ? Convert.ToInt32(InputTextBox.Text, CultureInfo.CurrentCulture) : 0;
                OKButton.IsEnabled = input > 0;
            }
            else if ((viewModel.StateParam.Equals("EnterCouponValue") || viewModel.StateParam.Equals("AM_EnterCouponValue") || viewModel.StateParam.Equals("AM_KeyInPrice") || IsCurrencyEnabled()) && InputTextBox.Text.Length <= 0)
            {
                OKButton.IsEnabled = false;
                inputValue = "";
            }
            else if (viewModel.StateParam.Equals("SmmKeyInWtTol"))
            {
                if (InputTextBox.Text.Length == 0)
                    DecimalMarkKey.IsEnabled = true;
            }
            else if (viewModel.StateParam.Equals("SmLoadLift"))
            {
                double input = 0;
                double.TryParse(InputTextBox.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out input);
                OKButton.IsEnabled = input != 0;
                MinusKey.IsEnabled = InputTextBox.Text.Length == 0;
            }
            BackspaceButton.IsEnabled = InputTextBox.Text.Length > 0;
        }

        public static DependencyProperty BackgroundViewModeProperty = DependencyProperty.Register("BackgroundViewMode", typeof(bool), typeof(NumericKeypad));

        public bool BackgroundViewMode
        {
            get
            {
                return Convert.ToBoolean(GetValue(BackgroundViewModeProperty));
            }
            set
            {
                SetValue(BackgroundViewModeProperty, value);
            }
        }

        private IMainViewModel viewModel;
        private string inputValue = "";
        private static PriceTextBlock priceTextBlock = new PriceTextBlock();
        private int MinInput = 1;

        private void OKButton_TouchUp(object sender, TouchEventArgs e)
        {
            InputTextBox.Clear();
        }
    }
}
