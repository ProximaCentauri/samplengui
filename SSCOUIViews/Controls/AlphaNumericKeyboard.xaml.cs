using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SSCOUIModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using FPsxWPF.Controls;
using FPsxWPF.Helpers;
using SSCOControls;
using System.Windows.Controls.Primitives;
using SSCOUIModels.Helpers;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Collections.Specialized;
using SSCOUIModels.Controls;
using RPSWNET;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for AlphaNumericKeyboard.xaml
    /// </summary>
    public partial class AlphaNumericKeyboard : Grid
    {
        public AlphaNumericKeyboard()
        {
            InitializeComponent();
            initKeypadStyle();
            UpdateButtonControlText();
        }

        public void initKeypadStyle()
        {
            alphaNumericKeypadStyle = this.FindResource("alphaNumericKeypadButtonStyle") as Style;
            alphaNumericHotKeypadStyle = this.FindResource("alphaNumericHotKeypadButtonStyle") as Style;
            storeModeAlphaNumericKeypadStyle = this.FindResource("storeModeAlphaNumericKeypadButtonStyle") as Style;
            storeModeAlphaNumericHotKeypadStyle = this.FindResource("storeModeAlphaNumericHotKeypadButtonStyle") as Style;
            searchKeypadButtonStyle = this.FindResource("searchKeypadButtonStyle") as Style;
            searchWatermarkTextboxStyle = this.FindResource("itemSearchTextBoxStyle") as Style;
            alphaNumericTextboxBorderStyle = this.FindResource("popupTextBoxBorderStyle") as Style;
            alphaNumericTextboxInnerShadowStyle = this.FindResource("popupTextBoxInnerShadowStyle") as Style;
            searchTextboxBorderStyle = this.FindResource("backgroundTextBoxBorderStyle") as Style;
            searchTextboxInnerShadowStyle = this.FindResource("backgroundTextBoxInnerShadowStyle") as Style;
            alphaNumericEnterButtonStyle = this.FindResource("alphaNumericEnterButtonStyle") as Style;
            alphaNumericBackspaceButtonStyle = this.FindResource("backSpaceButtonStyle") as Style;
            alphaNumericSpaceButtonStyle = this.FindResource("alphaNumericSpaceButtonStyle") as Style;
            storeModeAlphaNumericEnterButtonStyle = this.FindResource("storeModeEnterButtonStyle") as Style;
            storeModeBackspaceButtonStyle = this.FindResource("storeBackSpaceButtonStyle") as Style;
            storeModeAlphaNumericSpaceButtonStyle = this.FindResource("storeModeAlphanumericSpaceButtonStyle") as Style;
            searchEnterButtonStyle = this.FindResource("mainButtonStyle") as Style;
            alphaNumericGoBackButtonStyle = this.FindResource("buttonGoBackStyle") as Style;
            storeModeAlphaNumericGoBackButtonStyle = this.FindResource("storeModeButtonGoBackStyle") as Style;
            alphaNumericTextBoxStyle = this.FindResource("alphanumericTextBoxStyle") as Style;
            storeModeAlphaNumericTextBoxStyle = this.FindResource("storeModeAlphanumericTextBoxStyle") as Style;
        }

        private void UpdateButtonControlText()
        {
            this.SpaceButton.Property(ImageButton.TextProperty).SetResourceValue("SpaceButtonText");
            this.GoBackButton.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
            this.EnterButton.Property(ImageButton.TextProperty).SetResourceValue("EnterButtonText");
        }

        public void initKeyboardProperties()
        {
            ExceededKeysCollection = new ObservableCollection<GridItem>();
        }

        public void retrieveCustomKeyboardProperties()
        {
            this.InputTextBox.Clear();
            clearKeysItemSources();

            Line1AlphaNumKeys = UIControlFinder.FindVisualChild<UniformGrid>(Line1Keys, "Line1AlphaNumKeys") as UniformGrid;
            Line2AlphaNumKeys = UIControlFinder.FindVisualChild<UniformGrid>(Line2Keys, "Line2AlphaNumKeys") as UniformGrid;
            Line3AlphaNumKeys = UIControlFinder.FindVisualChild<UniformGrid>(Line3Keys, "Line3AlphaNumKeys") as UniformGrid;
            HotKeys = UIControlFinder.FindVisualChild<UniformGrid>(HotkeysItemsControl, "HotKeys") as UniformGrid;
            ExceededKeysUniformGrid = UIControlFinder.FindVisualChild<UniformGrid>(ExceededKeysItemsControl, "ExceededKeysUniformGrid") as UniformGrid;

            ExceededKeysCollection.Clear();

            bindAlphaLineKeys();
            bindNumLineKeys();
            bindHotKeys();

            ShowShiftKey();

            if (BackgroundViewMode)
            {
                this.InputTextBox.Style = searchWatermarkTextboxStyle;
                this.InputTextboxBorder.Style = searchTextboxBorderStyle;
                this.InputTextboxInnerShadow.Style = searchTextboxInnerShadowStyle;
                this.EnterButton.Style = searchEnterButtonStyle;
                this.BackSpaceButton.Style = alphaNumericBackspaceButtonStyle;
                this.SpaceButton.Style = alphaNumericSpaceButtonStyle;
                this.GoBackButton.Style = alphaNumericGoBackButtonStyle;
            }
            else if (viewModel.StoreMode)
            {
                this.InputTextBox.Style = storeModeAlphaNumericTextBoxStyle;
                this.InputTextboxBorder.Style = alphaNumericTextboxBorderStyle;
                this.InputTextboxInnerShadow.Style = alphaNumericTextboxInnerShadowStyle;
                this.EnterButton.Style = storeModeAlphaNumericEnterButtonStyle;
                this.BackSpaceButton.Style = storeModeBackspaceButtonStyle;
                this.SpaceButton.Style = storeModeAlphaNumericSpaceButtonStyle;
                this.GoBackButton.Style = storeModeAlphaNumericGoBackButtonStyle;
            }
            else
            {
                this.InputTextBox.Style = alphaNumericTextBoxStyle;
                this.InputTextboxBorder.Style = alphaNumericTextboxBorderStyle;
                this.InputTextboxInnerShadow.Style = alphaNumericTextboxInnerShadowStyle;
                this.EnterButton.Style = alphaNumericEnterButtonStyle;
                this.BackSpaceButton.Style = alphaNumericBackspaceButtonStyle;
                this.SpaceButton.Style = alphaNumericSpaceButtonStyle;
                this.GoBackButton.Style = alphaNumericGoBackButtonStyle;
            }

            hotKeysButtons = UIControlFinder.FindVisualChildren<ImageToggleButton>(this.HotkeysItemsControl);
            resetHotkeysButtonState();
            setHotkeyButtonState(ShiftButton, false);

            AddNumLineExtraKeysToExceededKeysCollection();
            AddExceededKeysToTheLeastAlphaNumLines();
            setShiftKeyButtonWidth();
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventHandler tempHandler = EnterButtonClick;
            if (null != tempHandler)
            {
                tempHandler(sender, e);
            }
        }

        private void EnterButton_TouchUp(object sender, TouchEventArgs e)
        {
            EventHandler<TouchEventArgs> tempHandler = EnterButtonTouchUp;
            if (null != tempHandler)
            {
                tempHandler(sender, e);
            }
        }

        private void ShiftButton_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!shiftShown && ShiftKeySupport)
            {
                //call NextGen shift implementation if no shiftkey shown in classic PSX				
                ToggleShift(ShiftButton.IsChecked.Value);
                setHotkeyButtonState(ShiftButton, ShiftButton.IsChecked.Value);
            }
            else
            {
                //call the shiftkey click event in classic PSX to get the correct key accents i.e ~ ` ^                 
                ImageToggleButton button = sender as ImageToggleButton;
                if (null != button)
                {
                    this.viewModel.ActionCommand.Execute("ShiftKey");
                }
            }
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventHandler<TextChangedEventArgs> tempHandler = InputTextBoxTextChanged;
            if (null != tempHandler)
            {
                tempHandler(sender, e);
            }
            else
            {
                handleBaseInputTextBox_TextChange();
            }
        }

        private void handleBaseInputTextBox_TextChange()
        {
            this.BackSpaceButton.IsEnabled = (this.InputTextBox.Text.Length > 0 && !this.InputTextBox.IsWatermarkShown);
            this.EnterButton.IsEnabled = (this.InputTextBox.Text.Length >= MinInput && !this.InputTextBox.IsWatermarkShown);
        }

        private bool isSymbols(Key key)
        {
            bool result = false;
            switch (key)
            {
                case Key.OemOpenBrackets:
                case Key.OemCloseBrackets:
                case Key.OemQuestion:
                case Key.OemTilde:
                case Key.D0:
                case Key.D1:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.OemPlus:
                case Key.OemBackslash:
                case Key.OemPipe:
                case Key.OemSemicolon:
                    result = true;
                    break;
            }
            return result;
        }

        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement textBox = null;
            View view = UIControlFinder.FindAncestorOrSelf<View>(this);
            if (null != view)
            {
                textBox = view.KeyboardFocusedElement;
            }
            if (null == textBox)
            {
                return;
            }
            bool shiftOn = ShiftButton.IsChecked.Value;
            ImageButton button = sender as ImageButton;
            if (null != button)
            {
                if (!String.IsNullOrEmpty(button.Text))
                {
                    if (!char.IsDigit(button.Text, 0))
                    {
                        Key key = KeyCode(button.Text);
                        if (shiftOn && isSymbols(key))
                        {
                            textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                             new TextComposition(InputManager.Current, textBox, String.Empty)) { RoutedEvent = TextCompositionManager.TextInputEvent });
                            return;
                        }
                    }
                }
            }
            RoutedEventHandler tempHandler = KeyboardButtonClick;
            if (null != tempHandler)
            {
                tempHandler(sender, e);
            }
            else
            {
                handleBaseKeyboardButton_Click(button, textBox);
            }
        }

        private void handleBaseKeyboardButton_Click(ImageButton button, FrameworkElement textBox)
        {
            if (null != button.CommandParameter)
            {
                Key key = KeyCode(button.CommandParameter.ToString());
                if (key != Key.None)
                {
                    textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(textBox), 0, key) { RoutedEvent = Keyboard.KeyDownEvent });
                }
            }
            else
            {
                textBox.RaiseEvent(new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                    new TextComposition(InputManager.Current, textBox, button.Text)) { RoutedEvent = TextCompositionManager.TextInputEvent });
            }
        }

        public Key KeyCode(string key)
        {
            Key finalKey = Key.None;
            switch (key)
            {
                case "{":
                    key = "OPENBRACKETS";
                    break;
                case "}":
                    key = "CLOSEBRACKETS";
                    break;
                case "?":
                    key = "QUESTION";
                    break;
                case "~":
                    key = "TILDE";
                    break;
                case ")":
                    key = "D0";
                    break;
                case "!":
                    key = "D1";
                    break;
                case "#":
                    key = "D3";
                    break;
                case "$":
                    key = "D4";
                    break;
                case "%":
                    key = "D5";
                    break;
                case "^":
                    key = "D6";
                    break;
                case "&":
                    key = "D7";
                    break;
                case "*":
                    key = "D8";
                    break;
                case "(":
                    key = "D9";
                    break;
                case "+":
                    key = "PLUS";
                    break;
                case "/":
                    key = "BACKSLASH";
                    break;
                case "|":
                    key = "PIPE";
                    break;
                case ":":
                    key = "SEMICOLON";
                    break;
                case "=":
                    key = "PLUS";
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

        public void ToggleShift(bool isUpper)
        {
            UniformGrid[] rows = { Line1AlphaNumKeys, Line2AlphaNumKeys, Line3AlphaNumKeys, ExceededKeysUniformGrid };

            for (int i = 0; i < rows.Length; i++)
            {
                ToggleShift(isUpper, rows[i]);
            }
        }

        private void ToggleShift(bool isUpper, UniformGrid lineKeys)
        {
            foreach (ImageButton button in UIControlFinder.FindVisualChildren<ImageButton>(lineKeys))
            {
                button.Text = isUpper ? button.Text.ToUpper() : button.Text.ToLower();
            }
        }

        private void invokeButtonClick(Button btn)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(btn);
            IInvokeProvider invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            if (null != invokeProvider)
            {
                invokeProvider.Invoke();
            }
        }

        private void Line1KeysCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && replaceBindValues(sender))
            {
                Line1Keys.ItemsSource = null;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindLineKeys Line1Keys");
                bindLineKeys(Line1KeysCollection, Line1Keys, Line1AlphaNumKeys, "AlphaNumP1", "Line1Alpha");

                line1Updated = true;
                AddExceededKeysToKeyboard();
            }
        }

        private void Line2KeysCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && replaceBindValues(sender))
            {
                Line2Keys.ItemsSource = null;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindLineKeys Line2Keys");
                bindLineKeys(Line2KeysCollection, Line2Keys, Line2AlphaNumKeys, "AlphaNumP4", "");

                line2Updated = true;
                AddExceededKeysToKeyboard();
            }
        }

        private void Line3KeysCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && replaceBindValues(sender))
            {
                Line3Keys.ItemsSource = null;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindLineKeys Line3Keys");
                bindLineKeys(Line3KeysCollection, Line3Keys, Line3AlphaNumKeys, "AlphaNumP2", "");

                line3Updated = true;
                AddExceededKeysToKeyboard();
            }
        }

        private void Line4KeysCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && replaceBindValues(sender))
            {
                ExceededKeysCollection.Clear();
                bindNumLineKeys();
            }
        }

        private void HotKeysCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && replaceBindValues(sender))
            {
                bindHotKeys();
            }
        }

        private void AddExceededKeysToKeyboard()
        {
            if (line1Updated && line2Updated && line3Updated)
            {
                AddNumLineExtraKeysToExceededKeysCollection();
                AddExceededKeysToTheLeastAlphaNumLines();
                setShiftKeyButtonWidth();
                line3Updated = false;
                line2Updated = false;
                line1Updated = false;
            }
        }

        private bool replaceBindValues(object sender)
        {
            var gridItem = sender as ObservableCollection<GridItem>;
            if (null != gridItem && gridItem[gridItem.Count - 1].Text != null && gridItem[gridItem.Count - 1].Data != null)
            {
                return true;
            }
            return false;
        }

        private void bindAlphaLineKeys()
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindAlphaLineKeys");
            bindLineKeys(Line1KeysCollection, Line1Keys, Line1AlphaNumKeys, "AlphaNumP1", "Line1Alpha");
            bindLineKeys(Line2KeysCollection, Line2Keys, Line2AlphaNumKeys, "AlphaNumP4", "");
            bindLineKeys(Line3KeysCollection, Line3Keys, Line3AlphaNumKeys, "AlphaNumP2", "");
        }

        private void bindNumLineKeys()
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindNumLineKeys");
            bindLineKeys(Line4KeysCollection, NumericLine1Keys, "AlphaNumP1", "Line1Numeric;Columns=3;Row=1");
            bindLineKeys(Line4KeysCollection, NumericLine2Keys, "AlphaNumP1", "Line1Numeric;Columns=3;Row=2");
            bindLineKeys(Line4KeysCollection, NumericLine3Keys, "AlphaNumP1", "Line1Numeric;Columns=3;Row=3");
            bindLineKeys(Line4KeysCollection, NumericLine4Keys, "AlphaNumP1", "Line1Numeric;Columns=3;Row=4");
        }

        private void bindHotKeys()
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindHotKeys");
            bindLineKeys(HotKeysCollection, HotkeysItemsControl, HotKeys, "", "");
        }

        private void bindLineKeys(ObservableCollection<GridItem> items, ItemsControl lineKey, UniformGrid alphaNumKeys, string propertyName, string converterParam)
        {
            if (null != items && null != alphaNumKeys)
            {
                if (!lineKey.Name.Equals("ExceededKeysItemsControl"))
                {
                    StoreKeysToExceededKeysCollection(items, MaxLineKeysLimit(lineKey));
                }

                bindLineKeys(items, lineKey, propertyName, converterParam);
                alphaNumKeys.Columns = (lineKey.ItemsSource as ObservableCollection<GridItem>).Count;
                UpdateKeypadButtonStyle(MaxColumnsCount());
            }
        }

        private void bindLineKeys(ObservableCollection<GridItem> items, ItemsControl lineKey, string propertyName, string converterParam)
        {
            if (null != items)
            {
                MultiBinding mb = new MultiBinding();
                mb.ConverterParameter = converterParam;
                mb.Converter = new AlphaNumericKeysHandler();
                mb.Bindings.Add(new Binding() { Source = items });
                mb.Bindings.Add(new Binding() { Source = viewModel.GetPropertyValue(propertyName) });
                mb.Bindings.Add(new Binding() { Source = (bool)this.viewModel.GetPropertyValue("ShiftKeyShown") });
                mb.Bindings.Add(new Binding() { Source = ShiftButton.IsChecked });
                mb.Bindings.Add(new Binding() { Source = (int)MaxLineKeysLimit(lineKey) });
                BindingOperations.SetBinding(lineKey, ItemsControl.ItemsSourceProperty, mb);

                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+BindLineKeys success. LineKey:{0}", lineKey.Name);
            }
        }

        private void StoreKeysToExceededKeysCollection(ObservableCollection<GridItem> items, int maxKeysLimit)
        {
            ObservableCollection<GridItem> val = items;

            if (null != val)
            {
                foreach (GridItem item in val)
                {
                    if (null != item && null != item.Text
                        && val.IndexOf(item) >= maxKeysLimit
                        && !ExceededKeysCollection.Contains(item))
                    {
                        ExceededKeysCollection.Add(item);
                    }
                }
            }
        }

        private int MaxLineKeysLimit(ItemsControl lineKey)
        {
            string lineKeyName = lineKey.Name;
            switch (lineKeyName)
            {
                case "Line1Keys":
                case "Line2Keys":
                    return 11;
                case "Line3Keys":
                    return 9;
                default:
                    return 11;
            }
        }

        private void HotKeys_TouchDown(object sender, TouchEventArgs e)
        {
            ImageToggleButton button = sender as ImageToggleButton;
            if (null != button)
            {
                GridItem item = button.DataContext as GridItem;
                if (null != item)
                {
                    if (BackgroundViewMode)
                    {
                        this.viewModel.ActionCommand.Execute(string.Format("PFKBHotKeysClick({0})", item.Data));
                    }
                    else
                    {
                        this.viewModel.ActionCommand.Execute(string.Format("HotKeysClick({0})", item.Data));
                    }
                }
            }
        }

        public void setHotKeysButtonState()
        {
            var hotkeys = viewModel.GetPropertyValue("NextGenUIHotKeyState").ToString().Split(';');
            if (hotkeys.Length > 1)
            {
                var arrHotkey = hotkeys[0].Split('=');
                string hotkey = string.Empty;
                if (arrHotkey.Length > 1)
                {
                    hotkey = arrHotkey[1];
                }

                if (hotkey.Equals("Text"))
                {
                    resetHotkeysButtonState();
                }
                else
                {
                    setHotkeyButtonState(hotkey);
                }
            }
        }

        private void HotKeys_Loaded(object sender, RoutedEventArgs e)
        {
            setHotKeysButtonState();
        }

        private void ShiftButton_Loaded(object sender, RoutedEventArgs e)
        {
            setShiftButtonState();
        }

        private void resetHotkeysButtonState()
        {
            if (null != hotKeysButtons)
            {
                foreach (ImageToggleButton btn in hotKeysButtons)
                {
                    setHotkeyButtonState(btn, false);
                }
            }
        }

        private void setHotkeyButtonState(string hotKey)
        {
            if (null != hotKeysButtons)
            {
                foreach (ImageToggleButton btn in hotKeysButtons)
                {
                    if (!btn.Tag.ToString().Equals(string.Empty) && hotKey.Contains(btn.Tag.ToString().Substring(2)))
                    {
                        setHotkeyButtonState(btn, true);
                    }
                    else
                    {
                        setHotkeyButtonState(btn, false);
                    }
                }
            }
        }

        private void setHotkeyButtonState(ImageToggleButton hotKey, bool IsChecked)
        {
            if (null != hotKey)
            {
                hotKey.IsChecked = IsChecked;
                if (viewModel.StoreMode)
                {
                    hotKey.Background = IsChecked && hotKey.IsEnabled ? TryFindResource("buttonSelectedBackgroundColorBrush") as Brush : TryFindResource("storeModeMainButtonBackgroundColorBrush") as Brush;
                }
                else
                {
                    hotKey.Background = IsChecked && hotKey.IsEnabled ? TryFindResource("buttonSelectedBackgroundColorBrush") as Brush : TryFindResource("mainButtonBackgroundColorBrush") as Brush;
                }
            }
        }

        public void setShiftButtonState()
        {
            var hotkeys = viewModel.GetPropertyValue("NextGenUIHotKeyState").ToString().Split(';');
            if (hotkeys.Length > 1)
            {
                var arrHotkey = hotkeys[1].Split('=');
                if (arrHotkey.Length > 1)
                {
                    bool shiftOn = false;
                    bool.TryParse(arrHotkey[1], out shiftOn);
                    setHotkeyButtonState(ShiftButton, shiftOn);
                }
            }
        }

        private void AddExceededKeysToTheLeastAlphaNumLines()
        {
            ObservableCollection<GridItem> val = ExceededKeysCollection;
            ObservableCollection<GridItem> retVal = new ObservableCollection<GridItem>();

            if (null != val)
            {
                foreach (GridItem item in val)
                {
                    if (null != item && null != item.Text)
                    {
                        if (AddToTheLeastItemsControlCount(item))
                        {
                            retVal.Add(item);
                        }
                    }
                }

                if (retVal.Count > 0)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "+ExceededKeys");
                    bindLineKeys(retVal, ExceededKeysItemsControl, ExceededKeysUniformGrid, "", "");
                }
            }
        }

        private bool AddToTheLeastItemsControlCount(GridItem item)
        {
            var Line1KeysItemsSource = Line1Keys.ItemsSource;
            var Line2KeysItemsSource = Line2Keys.ItemsSource;
            var Line3KeysItemsSource = Line3Keys.ItemsSource;

            bool addToUniformGrid = false;

            if (Line1KeysItemsSource != null
                && Line2KeysItemsSource != null
                && Line3KeysItemsSource != null)
            {
                int line1KeysCount = Line1KeysCollection.Count;
                int line2KeysCount = Line2KeysCollection.Count;
                int line3KeysCount = Line3KeysCollection.Count;

                if (line1KeysCount >= 11
                    && line2KeysCount >= 11
                    && line3KeysCount >= 9
                    && !ExceededKeysItemsControl.Items.Contains(item))
                {
                    addToUniformGrid = true;

                    if (HotKeysCollection.Count > 4)
                    {
                        ShiftButton.Width = 58;
                    }
                }
                else
                {
                    ObservableCollection<GridItem> LineKeysCollectionWithExtraKeys = new ObservableCollection<GridItem>();

                    if (line3KeysCount < 9 && !(Line3KeysItemsSource as ObservableCollection<GridItem>).Contains(item))
                    {
                        LineKeysCollectionWithExtraKeys = Line3Keys.ItemsSource as ObservableCollection<GridItem>;
                        LineKeysCollectionWithExtraKeys.Add(item);
                        bindLineKeys(LineKeysCollectionWithExtraKeys, Line3Keys, Line3AlphaNumKeys, "AlphaNumP2", "");
                    }
                    else if (line1KeysCount <= line2KeysCount && !(Line1KeysItemsSource as ObservableCollection<GridItem>).Contains(item))
                    {
                        LineKeysCollectionWithExtraKeys = Line1Keys.ItemsSource as ObservableCollection<GridItem>;
                        LineKeysCollectionWithExtraKeys.Add(item);
                        bindLineKeys(LineKeysCollectionWithExtraKeys, Line1Keys, Line1AlphaNumKeys, "AlphaNumP1", "Line1Alpha");
                    }
                    else if (line2KeysCount < line1KeysCount && !(Line2KeysItemsSource as ObservableCollection<GridItem>).Contains(item))
                    {
                        LineKeysCollectionWithExtraKeys = Line2Keys.ItemsSource as ObservableCollection<GridItem>;
                        LineKeysCollectionWithExtraKeys.Add(item);
                        bindLineKeys(LineKeysCollectionWithExtraKeys, Line2Keys, Line2AlphaNumKeys, "AlphaNumP4", "");
                    }
                }
            }

            return addToUniformGrid;
        }

        private void AddNumLineExtraKeysToExceededKeysCollection()
        {
            if (Line4KeysCollection != null && Line4KeysCollection.Count > 0)
            {
                if (Line4KeysCollection.Count >= 11
                    && !ExceededKeysCollection.Contains(Line4KeysCollection[10] as GridItem))
                {
                    ExceededKeysCollection.Add(Line4KeysCollection[10] as GridItem);
                }

                if (Line4KeysCollection.Count >= 12
                    && !ExceededKeysCollection.Contains(Line4KeysCollection[11] as GridItem))
                {
                    ExceededKeysCollection.Add(Line4KeysCollection[11] as GridItem);
                }
            }
        }

        private int ChildrenCount(StackPanel sp)
        {
            int count = 0;
            for (int i = 0; i < sp.Children.Count; i++)
            {
                if ((sp.Children[i] as FrameworkElement).Visibility == Visibility.Visible)
                {
                    count++;
                }
            }
            return count;
        }

        private bool IsCharacterExist(string data)
        {
            ObservableCollection<GridItem>[] items = { Line1KeysCollection, Line2KeysCollection, Line3KeysCollection, Line4KeysCollection };
            for (int i = 0; i < items.Length; i++)
            {
                foreach (GridItem item in items[i])
                {
                    if (null != item.Text && item.Text.Equals(data))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int MaxColumnsCount()
        {
            if (null != Line1AlphaNumKeys && null != Line2AlphaNumKeys && null != Line3AlphaNumKeys)
            {
                int[] columnsCount = { Line1AlphaNumKeys.Columns, Line2AlphaNumKeys.Columns, Line3AlphaNumKeys.Columns };
                return columnsCount.Max();
            }
            return 0;
        }

        private void UpdateKeypadButtonStyle(int columns)
        {
            if (null != Line1AlphaNumKeys && null != Line2AlphaNumKeys && null != Line3AlphaNumKeys)
            {
                if (null != Line1KeysCollection && null != Line2KeysCollection && null != Line3KeysCollection && null != Line4KeysCollection && columns > 0)
                {
                    if (BackgroundViewMode)
                    {
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, searchKeypadButtonStyle, columns, "alphaNumericKeypadButtonStyle");
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, searchKeypadButtonStyle, columns, "numericLineKeypadButtonZeroStyle");
                    }
                    else if (viewModel.StoreMode)
                    {
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, storeModeAlphaNumericKeypadStyle, columns, "alphaNumericKeypadButtonStyle");
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, storeModeAlphaNumericKeypadStyle, columns, "numericLineKeypadButtonZeroStyle");
                    }
                    else
                    {
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, alphaNumericKeypadStyle, columns, "alphaNumericKeypadButtonStyle");
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageButton) }, alphaNumericKeypadStyle, columns, "numericLineKeypadButtonZeroStyle");
                    }

                    if (viewModel.StoreMode)
                    {
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageToggleButton) }, storeModeAlphaNumericHotKeypadStyle, columns, "alphaNumericHotKeypadButtonStyle");
                    }
                    else
                    {
                        UpdateKeypadButtonStyle(new Style { TargetType = typeof(ImageToggleButton) }, alphaNumericHotKeypadStyle, columns, "alphaNumericHotKeypadButtonStyle");
                    }
                }
            }
        }

        private void UpdateKeypadButtonStyle(Style newStyle, Style baseOnStyle, int columns, string resourceKey)
        {
            newStyle.BasedOn = baseOnStyle;
            newStyle.Setters.Add(new Setter(Button.MarginProperty, new Thickness(5.0)));

            if (resourceKey.Equals("numericLineKeypadButtonZeroStyle"))
            {
                newStyle.Setters.Add(new Setter(Button.MarginProperty, new Thickness(18, 5, 5, 5)));
            }

            this.Resources[resourceKey] = newStyle;
        }

        public void setAlphaNumLineKeys(string alphaNumKeysLine)
        {
            this.Line1KeysCollection = viewModel.GetPropertyValue(alphaNumKeysLine + "2") as ObservableCollection<GridItem>;
            this.Line2KeysCollection = viewModel.GetPropertyValue(alphaNumKeysLine + "3") as ObservableCollection<GridItem>;
            this.Line3KeysCollection = viewModel.GetPropertyValue(alphaNumKeysLine + "4") as ObservableCollection<GridItem>;
            this.Line4KeysCollection = viewModel.GetPropertyValue(alphaNumKeysLine + "1") as ObservableCollection<GridItem>;
        }

        public void setHotKeysLine(string hotKeysLine)
        {
            this.HotKeysCollection = viewModel.GetPropertyValue(hotKeysLine) as ObservableCollection<GridItem>;
        }

        public void removeAddEvents()
        {
            Line1KeysCollection.CollectionChanged -= Line1KeysCollection_CollectionChanged;
            Line1KeysCollection.CollectionChanged += Line1KeysCollection_CollectionChanged;
            Line2KeysCollection.CollectionChanged -= Line2KeysCollection_CollectionChanged;
            Line2KeysCollection.CollectionChanged += Line2KeysCollection_CollectionChanged;
            Line3KeysCollection.CollectionChanged -= Line3KeysCollection_CollectionChanged;
            Line3KeysCollection.CollectionChanged += Line3KeysCollection_CollectionChanged;
            Line4KeysCollection.CollectionChanged -= Line4KeysCollection_CollectionChanged;
            Line4KeysCollection.CollectionChanged += Line4KeysCollection_CollectionChanged;
            HotKeysCollection.CollectionChanged -= HotKeysCollection_CollectionChanged;
            HotKeysCollection.CollectionChanged += HotKeysCollection_CollectionChanged;
        }

        public static DependencyProperty BackgroundViewModeProperty = DependencyProperty.Register("BackgroundViewMode", typeof(bool), typeof(AlphaNumericKeyboard));
        public static DependencyProperty ShiftKeySupportProperty = DependencyProperty.Register("ShiftKeySupport", typeof(bool), typeof(AlphaNumericKeyboard));

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

        public bool ShiftKeySupport
        {
            get
            {
                return Convert.ToBoolean(GetValue(ShiftKeySupportProperty));
            }
            set
            {
                SetValue(ShiftKeySupportProperty, value);
            }
        }

        private void ShowShiftKey()
        {
            shiftShown = (bool)this.viewModel.GetPropertyValue("ShiftKeyShown");

            if (!shiftShown && !ShiftKeySupport)
            {
                this.ShiftButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ShiftButton.Visibility = Visibility.Visible;
                this.ShiftButton.IsChecked = false;
            }
        }

        private void Keyboard_UnLoaded(object sender, RoutedEventArgs e)
        {
            clearKeysItemSources();
        }

        private void clearKeysItemSources()
        {
            Line1Keys.ItemsSource = null;
            Line2Keys.ItemsSource = null;
            Line3Keys.ItemsSource = null;
            ExceededKeysItemsControl.ItemsSource = null;
            HotkeysItemsControl.ItemsSource = null;
        }

        private void setShiftKeyButtonWidth()
        {
            if (ExceededKeysCollection.Count > 1 && HotKeysCollection.Count > 4)
            {
                this.ShiftButton.Width = 58;
            }
            else
            {
                this.ShiftButton.Width = 100;
            }
        }

        public void setGoBackButtonVisibility(bool isVisible)
        {
            GoBackButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private int MinInput = 1;
        IMainViewModel viewModel;
        private bool shiftShown;
        private bool line1Updated = false;
        private bool line2Updated = false;
        private bool line3Updated = false;
        private ObservableCollection<GridItem> Line1KeysCollection;
        private ObservableCollection<GridItem> Line2KeysCollection;
        private ObservableCollection<GridItem> Line3KeysCollection;
        private ObservableCollection<GridItem> Line4KeysCollection;
        private ObservableCollection<GridItem> HotKeysCollection;
        public event RoutedEventHandler KeyboardButtonClick;
        public event RoutedEventHandler EnterButtonClick;
        public event EventHandler<TouchEventArgs> EnterButtonTouchUp;
        public event EventHandler<TextChangedEventArgs> InputTextBoxTextChanged;
        private UniformGrid Line1AlphaNumKeys;
        private UniformGrid Line2AlphaNumKeys;
        private UniformGrid Line3AlphaNumKeys;
        private UniformGrid HotKeys;
        private UniformGrid ExceededKeysUniformGrid;
        private IEnumerable<ImageToggleButton> hotKeysButtons;
        private ObservableCollection<GridItem> ExceededKeysCollection;
        private Style alphaNumericKeypadStyle;
        private Style alphaNumericHotKeypadStyle;
        private Style storeModeAlphaNumericKeypadStyle;
        private Style storeModeAlphaNumericHotKeypadStyle;
        private Style alphaNumericTextboxBorderStyle;
        private Style alphaNumericTextboxInnerShadowStyle;
        private Style searchTextboxBorderStyle;
        private Style searchTextboxInnerShadowStyle;
        private Style searchKeypadButtonStyle;
        private Style searchWatermarkTextboxStyle;
        private Style alphaNumericEnterButtonStyle;
        private Style alphaNumericBackspaceButtonStyle;
        private Style alphaNumericSpaceButtonStyle;
        private Style storeModeAlphaNumericEnterButtonStyle;
        private Style storeModeBackspaceButtonStyle;
        private Style storeModeAlphaNumericSpaceButtonStyle;
        private Style searchEnterButtonStyle;
        private Style alphaNumericGoBackButtonStyle;
        private Style storeModeAlphaNumericGoBackButtonStyle;
        private Style alphaNumericTextBoxStyle;
        private Style storeModeAlphaNumericTextBoxStyle;
    }
}
