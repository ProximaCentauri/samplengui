using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SSCOUIModels.Controls;
using SSCOUIModels;
using SSCOControls;
using FPsxWPF.Controls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class AlphaNumericInput : PopupView
    {
        public AlphaNumericInput(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            this.GenericAlphaNumericKeyboard.initKeyboardProperties();
            subscribeToEvents();
        }

        /// <summary>
        /// Keyboard click events
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param>
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param>
        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement textBox = this.KeyboardFocusedElement;
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
                if (key != Key.None && key != Key.Space)
                {
                    textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(textBox), 0, key) { RoutedEvent = Keyboard.KeyDownEvent });
                }
                else if (key == Key.Space)
                {
                    textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                        new TextComposition(InputManager.Current, textBox, " ")) { RoutedEvent = TextCompositionManager.TextInputEvent });
                }
            }
            else
            {
                textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                        new TextComposition(InputManager.Current, textBox, button.Text)) { RoutedEvent = TextCompositionManager.TextInputEvent });
            }
        }

        public override void Show(bool isShowing)
        {
            if (!isShowing)
            {
                this.GenericAlphaNumericKeyboard.ShiftButton.IsChecked = false;
            }
            else
            {
                this.GenericAlphaNumericKeyboard.retrieveCustomKeyboardProperties();
                this.OnNextGenDataChanged();
            }
            base.Show(isShowing);
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            this.Instructions.Visibility = contextForInstructionControlShow.Contains(param) ? Visibility.Visible : Visibility.Collapsed;
            UpdateText("LeadthruText", this.TitleControl);
            UpdateText("MessageBoxNoEcho", this.Message);
            UpdateText("InstructionScreenTitle", this.Instructions);
            this.TitleControl.Visibility = stateParamToShowTitleControl.Contains(this.viewModel.StateParam) ? Visibility.Visible : Visibility.Collapsed;
            this.GenericAlphaNumericKeyboard.InputTextBox.Clear();
            this.GenericAlphaNumericKeyboard.InputTextBox.PasswordMode = false;
            UpdateTitleStyles(false);
            this.setDefaultKeyboard();

            switch (param)
            {
                case "OperatorKeyboard":
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterIDText");
                    this.Message.Property(TextBlock.TextProperty).SetResourceValue("EnterIDMessage");
                    break;
                case "StoreModeKeyboard":
                    UpdateTitleStyles(true);
                    break;
                case "CashierIDKeyboard":
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("XM_ID");
                    UpdateText("LeadthruText", this.Message);
                    this.setCashManagementKeyboard();
                    GenericAlphaNumericKeyboard.setGoBackButtonVisibility(false);
                    UpdateTitleStyles(true);
                    break;
                case "CashierPasswordKeyboard":
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("XM_PW");
                    UpdateText("LeadthruText", this.Message);
                    this.setCashManagementKeyboard();
                    GenericAlphaNumericKeyboard.setGoBackButtonVisibility(true);
                    GenericAlphaNumericKeyboard.InputTextBox.PasswordMode = true;
                    UpdateTitleStyles(true);
                    break;
            }

            this.TitleControl.Visibility = contextForTitleControlShow.Contains(param) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateTitleStyles(bool storeMode)
        {
            if (storeMode)
            {
                this.TitleControl.Style = this.FindResource("storeModeAlphanumericPopupTitleStyle") as Style;
                this.Instructions.Style = this.FindResource("storeModeAlphanumericPopupTitleStyle") as Style;
                this.Message.Style = this.FindResource("storeModeAlphaNumericPopupTextStyle") as Style;
            }
            else
            {
                this.TitleControl.Style = this.FindResource("alphaNumericPopupTitleStyle") as Style;
                this.Instructions.Style = this.FindResource("alphaNumericPopupTitleStyle") as Style;
                this.Message.Style = this.FindResource("alphaNumericPopupTextStyle") as Style;
            }
        }

        /// <summary>
        /// EnterButton click events
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param>
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string commandParam = String.Empty;
            if (null != viewModel.StateParam)
            {
                switch (viewModel.StateParam)
                {
                    case "ExtendedAlphaNumeric":
                        commandParam = "EnterAlphaNum({0}?@?AtSignKey?.?KeyBoardP3?-?MinusKey)";
                        break;
                    case "CashierIDKeyboard":
                    case "CashierPasswordKeyboard":
                        commandParam = "CashManagementEnterKeyboard({0})";
                        break;
                    default:
                        commandParam = "EnterKeyboard({0})";
                        break;
                }
            }
            this.GenericAlphaNumericKeyboard.EnterButton.CommandParameter = String.Format(commandParam, this.GenericAlphaNumericKeyboard.InputTextBox.PasswordMode ? this.GenericAlphaNumericKeyboard.InputTextBox.PasswordText : this.GenericAlphaNumericKeyboard.InputTextBox.Text);
            this.GenericAlphaNumericKeyboard.InputTextBox.Text = string.Empty;
        }

        public override void OnPropertyChanged(string name, object value)
        {

            switch (name)
            {
                case "NextGenData":
                    OnNextGenDataChanged();
                    break;
                case "LeadthruText":
                    UpdateLeadthruText();
                    break;
                case "MessageBoxNoEcho":
                    UpdateText(name, this.Message);
                    break;
                case "InstructionScreenTitle":
                    UpdateText(name, this.Instructions);
                    break;
                case "UIEchoField":
                    MinInput = viewModel.UIEchoField.MinLength;
                    GenericAlphaNumericKeyboard.InputTextBox.MaxLength = viewModel.UIEchoField.MaxLength;
                    break;
                case "NextGenUIHotKeyState":
                    this.GenericAlphaNumericKeyboard.setHotKeysButtonState();
                    this.GenericAlphaNumericKeyboard.setShiftButtonState();
                    break;
            }
        }

        private void UpdateText(string property, MeasureTextBlock tb)
        {
            string propertyText = GetPropertyStringValue(property);

            if (propertyText.Length > 0)
            {
                string[] headerText = propertyText.Split(':');
                tb.Text = headerText[0];
                if (headerText.Length > 1)
                {
                    tb.Text = tb.Text + ":\n" + headerText[1];
                }
            }
        }

        private void subscribeToEvents()
        {
            this.GenericAlphaNumericKeyboard.KeyboardButtonClick -= KeyboardButton_Click;
            this.GenericAlphaNumericKeyboard.KeyboardButtonClick += KeyboardButton_Click;
            this.GenericAlphaNumericKeyboard.EnterButtonClick -= EnterButton_Click;
            this.GenericAlphaNumericKeyboard.EnterButtonClick += EnterButton_Click;
        }

        private void setDefaultKeyboard()
        {
            this.GenericAlphaNumericKeyboard.setAlphaNumLineKeys("AlphaNumKeysLine");
            this.GenericAlphaNumericKeyboard.setHotKeysLine("HotKeys");
            this.GenericAlphaNumericKeyboard.removeAddEvents();
        }

        private void setCashManagementKeyboard()
        {
            this.GenericAlphaNumericKeyboard.setAlphaNumLineKeys("XMAlphaNumKeysLine");
            this.GenericAlphaNumericKeyboard.setHotKeysLine("HotKeys");
            this.GenericAlphaNumericKeyboard.removeAddEvents();
        }

        private void UpdateLeadthruText()
        {
            switch (this.viewModel.StateParam)
            {
                case "CashierIDKeyboard":
                case "CashierPasswordKeyboard":
                    UpdateText("LeadthruText", this.Message);
                    break;
                default:
                    UpdateText("LeadthruText", this.TitleControl);
                    break;
            }
        }

        private void OnNextGenDataChanged()
        {
            string value = GetPropertyStringValue("NextGenData");
            GenericAlphaNumericKeyboard.InputTextBox.Clear();
            if (viewModel.StateParam.Equals("OperatorKeyboard"))
            {
                if (value.ToString().Equals("EnterPassword"))
                {
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterPasswordText");
                    this.Message.Property(TextBlock.TextProperty).SetResourceValue("EnterPasswordMessage");
                    GenericAlphaNumericKeyboard.InputTextBox.PasswordMode = true;
                }
                else if (value.ToString().Equals("EnterID"))
                {
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterIDText");
                    this.Message.Property(TextBlock.TextProperty).SetResourceValue("EnterIDMessage");
                    GenericAlphaNumericKeyboard.InputTextBox.PasswordMode = false;
                }
            }
        }

        private static List<string> stateParamToShowTitleControl = new List<string>() { "AlphaNumeric", "ExtendedAlphaNumeric", "AlphaNumericPassword" };
        private static List<string> stateParamToShowExtendedPad = new List<string>() { "ExtendedAlphaNumeric", "OperatorKeyboard", "ContextHelp", "HelpOnWay", "Keyboard0409" };
        private int MinInput = 1;

        private static List<string> contextForTitleControlShow = new List<string>() { "AlphaNumeric", "ExtendedAlphaNumeric", 
            "AlphaNumericPassword", "CashierIDKeyboard", "CashierPasswordKeyboard" };

        private static List<string> contextForInstructionControlShow = new List<string>() { "Keyboard0409", "OperatorKeyboard", 
            "AlphaNumericPassword", "AlphaNumericKeyboard", "StoreModeKeyboard",  };
    }
}
