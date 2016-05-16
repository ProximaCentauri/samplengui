using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SSCOUIModels.Controls;
using FPsxWPF.Controls;
using SSCOUIModels;
using SSCOControls;
using RPSWNET;
using System.Text.RegularExpressions;
using SSCOUIModels.Models;
using SSCOUIModels.Helpers;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : BackgroundView
    {
        public Search(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            InitAlphaNumericKeyboard();

            // Initialize properties
            this.IsMultipick = MultiPickPanel.Visibility == Visibility.Visible;
            this.ShowAlphaNumericKeyboard = GenericAlphaNumericKeyboard.Visibility == Visibility.Visible;
            this.SearchResultsView = false;

            categoryListSource = viewModel.GetPropertyValue("SearchCategories") as ObservableCollection<GridItem>;
            categoryListSource.CollectionChanged += CategoryList_CollectionChanged;

            SearchList.SwipeEvent += SearchList_SwipeEvent;

            if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
            {
                SearchList.PerfLogging = true;
                SearchList.LogEvent += SearchList_LogEvent;
            }

            try
            {
                ((INotifyCollectionChanged)SearchList.ItemsSource).CollectionChanged += SearchList_CollectionChanged;
                ((INotifyCollectionChanged)PickListReceipt.ItemsSource).CollectionChanged += PickListReceipt_CollectionChanged;
            }
            catch
            { }

            ResizeSearchList();
        }

        private void InitAlphaNumericKeyboard()
        {
            this.GenericAlphaNumericKeyboard.setAlphaNumLineKeys("PFKBAlphaNumKeysLine");
            this.GenericAlphaNumericKeyboard.setHotKeysLine("PFKBHotKeys");
            this.GenericAlphaNumericKeyboard.removeAddEvents();
            this.GenericAlphaNumericKeyboard.initKeyboardProperties();
            this.GenericAlphaNumericKeyboard.TouchDown += InputTextBox_TouchDown;
            this.GenericAlphaNumericKeyboard.EnterButton.Click += EnterButton_Click;
            this.GenericAlphaNumericKeyboard.InputTextBox.Property(WatermarkTextBox.WatermarkProperty).SetResourceValue("SearchWatermark");
        }

        void InputTextBox_TouchDown(object sender, TouchEventArgs e)
        {
            // for reconnection, do not subscribe until user has actually pressed the onscreen keyboard
            this.GenericAlphaNumericKeyboard.InputTextBoxTextChanged += InputTextBox_TextChanged;
            this.GenericAlphaNumericKeyboard.TouchDown -= InputTextBox_TouchDown;
        }

        void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.GenericAlphaNumericKeyboard.InputTextBox.IsWatermarkShown
                && this.GenericAlphaNumericKeyboard.InputTextBox.Text.Length > 0)
            {
                // Disable the Enter button when clicked
                this.GenericAlphaNumericKeyboard.EnterButton.IsEnabled = false;
                this.GenericAlphaNumericKeyboard.InputTextBox.Clear();

                if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                {
                    viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.KeyInCode, string.Format("Item code: {0}", this.GenericAlphaNumericKeyboard.InputTextBox.Text));
                }
            }
        }

        private void SearchList_LogEvent(object sender, LogEventArgs e)
        {
            if (e.LogMessage.Equals("StartItemListRender"))
            {
                viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.PickListItemsRender);
            }
            else if (e.LogMessage.Equals("EndItemListRender"))
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.PickListItemsRender, string.Format("Items rendered: {0}", e.RenderedItemCount));
            }
        }

        private void CategoryList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RefreshCategories();
            }
        }

        private void SearchList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (SearchResultsView)
                {
                    if ((viewModel.GetPropertyValue("SearchItems") as ObservableCollection<GridItem>).Count < 1)
                    {
                        emptySearchResult = true;
                        IndicatorGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    }
                    else
                    {
                        if (SearchList.PageCount > 1)
                        {
                            IndicatorGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                        }
                    }
                }
            }

            if (viewModel.Perfcheck.KeywordSearchStarted)
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.KeywordSearch, string.Format("returned {0} items", SearchList.Items.Count));
            }
            if (viewModel.Perfcheck.CollectionUpdateStarted)
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.CollectionUpdate, string.Format("Item count: {0}", SearchList.Items.Count));
            }
        }

        private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = SearchList.SelectedItem as GridItem;

            if (null != item)
            {
                viewModel.ActionCommand.Execute(String.Format("SearchItem({0})", item.Data));

                viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.PickListItemSelect, string.Format("Item: {0}", item.Data));

                if (IsMultipick)
                {
                    isPicking = true;
                }
                else
                {
                    SearchResultsView = false;
                }

                SearchList.SelectedIndex = -1;
            }
        }

        private void CategoryList_TouchDown(object sender, TouchEventArgs e)
        {
            ListBoxItem item = ItemsControl.ContainerFromElement(CategoryList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                object picklistSoundPath = new PathConverter().Convert(null, null, TryFindResource("bttnValidClickSound"), null);
                if (null != picklistSoundPath)
                {
                    ControlsAudio.PlaySound(picklistSoundPath.ToString());
                }
                ShowAlphaNumericKeyboard = false;

                GridItem catItem = (GridItem)item.DataContext;
                if (null != catItem)
                {
                    SelectCategory(catItem);
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "Clicked category \"{0}\"", catItem.Data);
                    SearchResultsView = false;
                }
            }
        }

        private void SelectCategory(GridItem item)
        {
            IndicatorGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);

            if (null != item)
            {
                viewModel.ActionCommand.Execute(String.Format("SearchCategory({0})", item.Data));
                viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.CollectionUpdate, string.Format("Category \"{0}\"", item.Data));
                CategoryList.SelectedItem = item;
            }
        }

        /// <summary>
        /// Gets the first active category tab which has property Visible=True.
        /// </summary>
        /// <returns></returns>
        private GridItem GetDefaultActiveTab()
        {
            GridItem catItem = null;

            if (viewModel.UIPicklistDisplayLevels.TabSelected < CategoryList.Items.Count)
            {
                int index = viewModel.UIPicklistDisplayLevels.TabSelected > 0 ? viewModel.UIPicklistDisplayLevels.TabSelected : 0;
                GridItem item = (GridItem)CategoryList.Items[index];

                if (item.IsVisible)
                {
                    catItem = item;
                }
                else
                {
                    foreach (GridItem visible in CategoryList.Items)
                    {
                        if (visible.IsVisible)
                        {
                            catItem = visible;
                            break;
                        }
                    }
                }
            }

            return catItem;
        }

        private void SearchList_SwipeEvent(object sender, SwipeEventArgs e)
        {
            switch (e.Action)
            {
                case SwipeAction.Right:
                case SwipeAction.Left:
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "SearchList_SwipeEvent() Swipe direction: {0}", e.Action.ToString());
                    break;
            }
        }

        void PickListReceipt_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsMultipick)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    this.isNMIPopupPending = true;
                }
            }
        }

        private void RefreshCategories()
        {
            ObservableCollection<GridItem> collection = new ObservableCollection<GridItem>();

            foreach (GridItem item in categoryListSource)
            {
                // Move the Favorites tab to the first column
                if (item.Data.Equals(FavoritesData))
                {
                    collection.Insert(0, item);
                    favoritesItem = item;
                }
                else
                {
                    collection.Add(item);
                }
            }
            CategoryList.ItemsSource = collection;
            HighlightTab(this.viewModel.UIPicklistDisplayLevels.TabSelected);
        }

        /// <summary>
        /// Updates the navigation text 
        /// </summary>
        private void UpdateBreadcrumb()
        {
            int subCount = 0;

            string category = viewModel.UIPicklistDisplayLevels.CurrentCategory;
            SearchResultsView = !string.IsNullOrEmpty(viewModel.UIPicklistDisplayLevels.SearchKey);

            if (SearchResultsView)
            {
                this.navigationLabel.Property(TextBlock.TextProperty).SetResourceFormattedValue("SearchResultsFor", viewModel.UIPicklistDisplayLevels.SearchKey);
            }
            else
            {
                if (string.IsNullOrEmpty(category))
                {
                    navigationLabel.Text = string.Empty;
                }
                else
                {
                    subCount = category.Split(',').Length - 1;
                    navigationLabel.Text = category.Replace(",", "\\");
                    LevelUpButton.Visibility = subCount > 0 ? Visibility.Visible : Visibility.Collapsed;
                    CategoryList.ItemContainerStyle = subCount > 0 ? this.FindResource("categoryListBoxItemStyle") as Style : this.FindResource("categoryListBoxItemStyleWithArrow") as Style;
                }
            }

            HighlightTab(this.viewModel.UIPicklistDisplayLevels.TabSelected);
        }
        
        /// <summary>
        /// Translate ADK TabSelected to actual Category tab selection
        /// ADK TabSelected is 1-based, NextGen tab is 0-based
        /// Note that SelectedIndex = n will not change index even when there are collapsed items
        /// Favorites item tab is always at SelectedIndex = 0 in the main category
        /// </summary>
        /// <param name="tabSelected"></param>
        private void HighlightTab(int tabSelected)
        {
            if (CategoryList.Items.Count > 0)
            {
                if (tabSelected == 0)
                {
                    string category = this.viewModel.UIPicklistDisplayLevels.CurrentCategory;
                    if (!string.IsNullOrEmpty(category))
                    {
                        if (category.Equals((CategoryList.Items[0] as GridItem).Text)) // CategoryList.Items[0] should be popular text or language equivalent
                        {
                            CategoryList.SelectedIndex = tabSelected;
                        }
                    }
                    return;
                }
                else if ((CategoryList.Items[0] as GridItem).Data.Equals(FavoritesData) && tabSelected > 0)
                {
                    CategoryList.SelectedIndex = tabSelected; // Since Favorites item always occupy (visible or hidden) the SelectedIndex = 0, the TabSelected is effectively equal to the 0-based index
                }
                else if (tabSelected > 0)
                {
                    CategoryList.SelectedIndex = tabSelected - 1; // if there is no favorites item, deduct 1
                }
                else
                {
                    CategoryList.SelectedIndex = tabSelected;
                }
            }
        }

        /// <summary>
        /// Resize the SearchList after new items arrive or keyboard or numpad collapses
        /// </summary>
        private void ResizeSearchList()
        {
            if (null != this.GenericAlphaNumericKeyboard)
            {
                if (Visibility.Visible == this.GenericAlphaNumericKeyboard.Visibility)
                {
                    SearchList.Height = 226;
                    PickListReceipt.Height = 208;
                }
                else
                {
                    SearchList.Height = 450;
                    PickListReceipt.Height = 432;
                }
            }
        }


        /// <summary>
        /// Show the Search input 
        /// </summary>
        /// <param name="show">true to show, otherwise hide</param>
        private bool ShowAlphaNumericKeyboard
        {
            get
            {
                return showAlphaNumericKeyboard;
            }
            set
            {
                if (null != this.GenericAlphaNumericKeyboard)
                {
                    this.GenericAlphaNumericKeyboard.Visibility = value ? Visibility.Visible : Visibility.Hidden; // using Hidden instead of Collapsed so retrieveCustomKeyboardProperties() can populate it
                    if (null != ReturnToLookupButton)
                    {
                        this.ReturnToLookupButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                    }

                    if (null != this.GenericAlphaNumericKeyboard.InputTextBox)
                    {
                        if (value == false)
                        {
                            this.GenericAlphaNumericKeyboard.InputTextBox.Clear();
                        }
                    }

                    showAlphaNumericKeyboard = value;
                    ResizeSearchList();
                    if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                    {
                        CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "Search.{0} method set ShowAlphaNumericKeyboard = {1}", new StackFrame(1).GetMethod().Name, value);
                    }
                }
            }
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            keywordInputTimer.Stop();

            bool SearchTrigger = !this.GenericAlphaNumericKeyboard.InputTextBox.IsWatermarkShown
                               && this.GenericAlphaNumericKeyboard.InputTextBox.Text.Length > 0;

            // when user empties the input using backspace it consequently shows the watermark
            // select Favorites tab and do not trigger a search
            if (SearchResultsView && GenericAlphaNumericKeyboard.InputTextBox.IsWatermarkShown)
            {
                if (favoritesItem != null)
                {
                    SelectCategory(favoritesItem.IsVisible ? favoritesItem : GetDefaultActiveTab());
                }
                else
                {
                    SelectCategory(GetDefaultActiveTab());
                }
                SearchResultsView = false;
            }

            this.GenericAlphaNumericKeyboard.EnterButton.IsEnabled = SearchTrigger;
            this.GenericAlphaNumericKeyboard.BackSpaceButton.IsEnabled = SearchTrigger;

            if (SearchTrigger)
            {
                if (Regex.IsMatch(this.GenericAlphaNumericKeyboard.InputTextBox.Text, @"^\d+$"))
                {
                    // Enter KeyInCode if all inputs are numeric.
                    this.GenericAlphaNumericKeyboard.EnterButton.CommandParameter = String.Format("EnterCode({0})",
                        this.GenericAlphaNumericKeyboard.InputTextBox.Text);
                }
                else
                {
                    keywordInputTimer.Start();
                    this.GenericAlphaNumericKeyboard.EnterButton.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Search mode will select the first column and replaces the tab name with "Search"
        /// It will update the text message also under it
        /// </summary>
        private bool SearchResultsView
        {
            get
            {
                return searchResultsView;
            }
            set
            {
                if (value)
                {
                    LevelUpButton.Visibility = Visibility.Collapsed;
                }

                searchResultsView = value;
                if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "Search.{0} method set SearchResultsView = {1}", new StackFrame(1).GetMethod().Name, value);
                }
            }
        }

        /// <summary>
        /// Sends search command to SCO on tick, timers stops.
        /// Timer starts when input detected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerKeywordSearch(object sender, EventArgs e)
        {
            keywordInputTimer.Stop();
            if (!this.GenericAlphaNumericKeyboard.InputTextBox.IsWatermarkShown
                && this.GenericAlphaNumericKeyboard.InputTextBox.Text.Length > 0)
            {
                if (this.viewModel.StateParam.Equals("ProduceFavorites") || this.viewModel.StateParam.Equals("MultiSelectProduceFavorites"))
                {
                    string str = String.Format("EnterSearch({0})", this.GenericAlphaNumericKeyboard.InputTextBox.Text);
                    viewModel.ActionCommand.Execute(str);
                    viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.KeywordSearch, str);
                    this.navigationLabel.Property(TextBlock.TextProperty).SetResourceFormattedValue("SearchResultsFor",
                        this.GenericAlphaNumericKeyboard.InputTextBox.Text);
                }
            }
            else
            {
                emptySearchResult = true;
            }
            SearchResultsView = true;
        }

        /// <summary>
        /// Shows the search input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToLookupButton_Click(object sender, RoutedEventArgs e)
        {
            SearchResultsView = false;
            ShowAlphaNumericKeyboard = true;
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            if (viewModel.Perfcheck.PickListItemSelectStarted)
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.PickListItemSelect);
            }
            else if (viewModel.Perfcheck.KeyInCodeStarted)
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.KeyInCode);
            }
            switch (param)
            {
                case "ProduceFavorites":
                    updateQuickPickSelection(viewModel.GetPropertyValue("IsQuickPickSelection"));
                    IsMultipick = false;
                    ResetSearchView();
                    break;
                case "MultiSelectProduceFavorites":
                    updateQuickPickSelection(viewModel.GetPropertyValue("IsQuickPickSelection"));
                    IsMultipick = true;
                    updatePicklistReceiptCount(viewModel.GetPropertyValue("LookupItemCount") as string);
                    ResetSearchView();
                    ShowNMIPopup();
                    break;
            }
        }

        private bool IsMultipick
        {
            get
            {
                return isMultipick;
            }
            set
            {
                this.returnScanButton.Property(ImageButton.TextProperty).SetResourceValue(value ? "FinishAndAdd" : "ReturnToScan");
                this.returnScanButton.Style = this.FindResource(value ? "multipickFinishButtonStyle" : "returnToScanButtonStyle") as Style;
                if (null != MultiPickPanel) { MultiPickPanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
                if (null != SearchList)
                {
                    SearchList.Width = value ? 700 : 1007;
                    if (null != SearchListPageIndicator)
                    {
                        SearchListPageIndicator.MaxWidth = value ? 410 : 760;
                        SearchListPageIndicatorLabel.Margin = value ? new Thickness(0, 0, 20, 10) : new Thickness(0, 0, 10, 10);
                    }
                }

                isMultipick = value;
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("LookupItemCount"))
            {
                updatePicklistReceiptCount(value.ToString());
            }
            else if (name.Equals("IsQuickPickSelection"))
            {
                updateQuickPickSelection(value);
            }
            else if (name.Equals("UIPicklistDisplayLevels"))
            {
                UpdateBreadcrumb();
            }
            else if (name.Equals("NextGenUIHotKeyState") 
                && this.GenericAlphaNumericKeyboard.ShiftButton.IsEnabled)
            {
                this.GenericAlphaNumericKeyboard.setHotKeysButtonState();
                this.GenericAlphaNumericKeyboard.setShiftButtonState();
            }
        }

        private void updateQuickPickSelection(object value)
        {
            int retval = 0;
            int.TryParse(value as string, out retval);
            quickPickSelection = retval == -1;
        }

        private void updatePicklistReceiptCount(string value)
        {
            int lookupItemCount = 0;
            if (MultiPickPanel.Visibility == Visibility.Visible)
            {
                string str = value.ToString();
                if (str.Length > 0)
                {
                    int.TryParse(str, out lookupItemCount);
                    this.picklistCountText.Property(TextBlock.TextProperty).SetResourceFormattedValue(lookupItemCount > 1 ? "ItemsAddedFormat" :
                        "SingleItemAddedFormat", lookupItemCount);
                }
            }
        }

        private void ResetSearchView()
        {
            if (SearchResultsView) // coming from keyword search
            {
                if (emptySearchResult)
                {
                    this.GenericAlphaNumericKeyboard.InputTextBox.Clear(); // clearing the textbox will reset category selection
                    emptySearchResult = false;
                }
            }
            else if (IsMultipick && isPicking)
            {
                // shopper is multi-picking, keep the search contents even when the state changes
                // also keep the keyboard contents
            }
            else
            {
                if (!IsMultipick)
                {
                    //not in multipick mode, screen stays in search screen after clearing popup
                    HighlightTab(this.viewModel.UIPicklistDisplayLevels.TabSelected);
                    ShowAlphaNumericKeyboard = true;
                }
                this.GenericAlphaNumericKeyboard.InputTextBox.Clear();
                UpdateBreadcrumb();
            }

            IndicatorGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        }

        /// <summary>
        /// Assigns the initial values of category selection and other flags
        /// This must be called only once every transition to Search view
        /// </summary>
        private void OnInitialSelection()
        {
            isPicking = false;

            RefreshCategories();
            ResetSearchView();
            if (CategoryList.Items.Count > 0)
            {
                if (quickPickSelection)
                {
                    ShowAlphaNumericKeyboard = false;
                    this.GenericAlphaNumericKeyboard.InputTextBox.Clear();
                }
                else
                {
                    ShowAlphaNumericKeyboard = true;
                }
            }
            this.GenericAlphaNumericKeyboard.InputTextBox.Clear();
            keywordInputTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(Properties.Settings.Default.InputTimerInterval) };
            keywordInputTimer.Tick += OnTimerKeywordSearch;
        }

        public override void Show(bool isShowing)
        {
            if (isShowing)
            {
                OnInitialSelection();
                ShowNMIPopup();
            }
            base.Show(isShowing);
        }

        private bool ShowNMIPopup()
        {
            if (this.isNMIPopupPending)
            {
                if (IsLoaded && SSCOUIViews.Properties.Settings.Default.NMI && this.viewModel.StateParam.Equals("MultiSelectProduceFavorites"))
                {
                    this.isNMIPopupPending = false;
                    viewModel.ActionCommand.Execute("ViewModelSet(Context;NMIPopup)");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Up tab button will get to top category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LevelUpButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionCommand.Execute("LevelUp");
        }

        private void returnScanButton_Click(object sender, RoutedEventArgs e)
        {
            keywordInputTimer.Stop();
            isPicking = false;
            SearchResultsView = false;
            CategoryList.SelectedIndex = -1;
            this.isNMIPopupPending = false;
        }

        private void Keyboard_Loaded(object sender, RoutedEventArgs e)
        {
            this.GenericAlphaNumericKeyboard.retrieveCustomKeyboardProperties();

            if (!string.IsNullOrEmpty(viewModel.UIPicklistDisplayLevels.SearchKey))
            {
                this.GenericAlphaNumericKeyboard.InputTextBox.Text = viewModel.UIPicklistDisplayLevels.SearchKey;
                this.GenericAlphaNumericKeyboard.InputTextBox.CaretIndex = Int32.MaxValue;
            }
        }

        /// <summary>
        /// Data string for Favorites tab
        /// </summary>
        private const string FavoritesData = "Favorites";

        /// <summary>
        /// Timer for the search input
        /// </summary>
        private DispatcherTimer keywordInputTimer;

        /// <summary>
        /// The Categories collection
        /// </summary>
        private ObservableCollection<GridItem> categoryListSource;

        /// <summary>
        /// Flag for SearchResultsView property
        /// </summary>
        private bool searchResultsView;

        /// <summary>
        /// Flag for ShowAlphaNumericKeyboard property
        /// </summary>
        private bool showAlphaNumericKeyboard;

        /// <summary>
        /// Flag for search result is empty
        /// </summary>
        private bool emptySearchResult = false;

        /// <summary>
        /// Flag for Multipick mode
        /// </summary>
        private bool isMultipick;

        /// <summary>
        /// Flag if user has started to pick items on a multipick mode
        /// </summary>
        private bool isPicking = false;

        /// <summary>
        /// Flag if user selected from QuickPick
        /// </summary>
        private bool quickPickSelection;

        /// <summary>
        /// The container for Favorites tab
        /// </summary>
        private GridItem favoritesItem = null;

        private bool isNMIPopupPending;
    }
}