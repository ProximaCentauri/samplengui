// <copyright file="CartControl.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
namespace SSCOUIViews.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Animation;
    using SSCOControls;
    using SSCOUIModels;
    using SSCOUIModels.Models;
    using System.Windows.Media;
    using SSCOUIModels.Helpers;
    using System.Windows.Input;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows.Threading;
    using RPSWNET;
    using System.Globalization;
    using System.Collections.ObjectModel;
    using System.Collections;
    using FPsxWPF.Controls;

    /// <summary>
    /// Interaction logic for CartControl.xaml
    /// </summary>
    public partial class CartControl : Grid
    {
        /// <summary>
        /// Initializes a new instance of the CartControl class.
        /// </summary>
        public CartControl()
        {
            InitializeComponent();         
        }

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForRewardInfoToShow = new List<string>() { "ScanWithReward", "QuickPickWithReward", "VoidItemWithReward" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForRewardInfoToHide = new List<string>() { "Scan", "QuickPick", "VoidItem" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForPayBtnShowAndCartCollapsed = new List<string>() { "Scan", "QuickPick", "ScanWithReward", "QuickPickWithReward", "ProduceFavorites", "MultiSelectProduceFavorites", "Finish", "EnterCoupons", "CashPayment", "SmAssistMenu", "SmAborted", "SmDataEntry", "SmVisualItems", "SmCouponItems" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForCancelAllVisibility = new List<string>() { "Scan", "ContextHelp", "HelpOnWay", "Continue", "WaitApproval", "VoidItem", "QuickPick", "ScanWithReward", "QuickPickWithReward", "VoidItemWithReward", "ConfirmAbort", "AM_VoidItem" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForPayBtnHideAndCartExpand = new List<string>() { "VoidItem", "VoidItemWithReward", "ConfirmVoid", "ConfirmAbort", "VoidApproval", "AM_VoidItem", "AM_ConfirmAbort","AM_ConfirmVoid" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForPayAndEditCartBtnEnable = new List<string>() { "Bag", "CheckBasket", "ConfirmVoid", "UnDeActivatedItemApproval", "VoidItem", "VoidItemWithReward", "VoidApproval"};

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForSale = new List<string>() { "Bag", "EnterCoupons", "QuickPick", "QuickPickWithReward", "Scan", "ScanWithReward"};

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForPayAndEditCartHideAndCartCollapsed = new List<string>() { "ScanVoucher", "CardMisRead", "DepositCoupon" };

        /// <summary>
        /// taxShown variable
        /// </summary>
        private bool taxShown;

        /// <summary>
        /// CartReceipt Loaded
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param> 
        private void CartReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
            {
                try
                {
                    ((INotifyCollectionChanged)CartReceipt.Items).CollectionChanged += CartReceipt_CollectionChanged;
                }
                catch { }
            }

            OnNextGenDataChanged();
        }

        /// <summary>
        /// CartReceipt CollectionChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of NotifyCollectionChangedEventArgs</param> 
        void CartReceipt_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.CartReceiptRender);
            if (viewModel.Perfcheck.CartReceiptRenderStarted)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                {
                    viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.CartReceiptRender);
                }));
            }

            ObservableCollection<ReceiptItem> customerReceiptCollection = this.viewModel.GetPropertyValue("CustomerReceipt") as ObservableCollection<ReceiptItem>;
            if (NotifyCollectionChangedAction.Add == e.Action)
            {
                UpdateCartReceiptIndexSelected(viewModel.StateParam);                
            }            
        }

        /// <summary>
        /// Grid DataContextChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of DependencyPropertyChangedEventArgs</param> 
        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = DataContext as IMainViewModel;
            viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
            UpdateCartReceiptItemsSource();
            OnBackgroundStateParamChanged(viewModel.BackgroundStateParam);
            OnStateParamChanged(viewModel.StateParam);
            OnTotalChanged();
            OnAmountDueChanged();
            OnTaxChanged();
            ShowHideTax();
            OnReceiptItemCountChange();
            SetPanelZIndex();
            UpdateCartReceiptIndexSelected(viewModel.StateParam);
            UpdateEditCartCommandParameter();
            
            if ((bool)viewModel.GetPropertyValue("CancelAllShown"))
                UpdateCancelAllButton(viewModel.StateParam);
        }

        /// <summary>
        /// OnBackgroundStateParamChanged method
        /// </summary>
        /// /// <param name="param">This is a parameter with a type of String</param>
        private void OnBackgroundStateParamChanged(String param)
        {
            if (null != param)
            {
                ShowPayButton(paramListForSale.Contains(param));
            }
        }

        /// <summary>
        /// OnStateParamChanged method
        /// </summary>   
        /// <param name="param">This is a parameter with a type of String</param>
        private void OnStateParamChanged(String param)
        {
            if (null != param)
            {
                UpdateCartReceiptItemsSource();
                UpdateCartReceiptIndexSelected(param);
                cartControlBox.IsEnabled = true;
                CartReceipt.DisableSelection = param.Equals("Bag") || param.Equals("AM_KeyInPrice") || param.Equals("AM_KeyInQuantity");
                SetPanelZIndex();
                if (paramListForRewardInfoToShow.Contains(param))
                {
                    RewardInfo.Visibility = Visibility.Visible;
                }
                else if (paramListForRewardInfoToHide.Contains(param))
                {
                    RewardInfo.Visibility = Visibility.Collapsed;
                }

                if (paramListForPayBtnShowAndCartCollapsed.Contains(param))
                {
                    CartControlExpand(false);
                    GoBackBtn.Visibility = Visibility.Collapsed;
                    CancelAllBtn.Visibility = Visibility.Collapsed;
                    ShowPayButton(true);
                    if (CartReceipt.Items.Count > 0)
                    {
                        CartReceipt.ScrollIntoView(CartReceipt.Items[CartReceipt.Items.Count - 1]);
                    }
                }
                else if (paramListForPayBtnHideAndCartExpand.Contains(param))
                {
                    GoBackBtn.Visibility = param.Equals("AM_VoidItem") || param.Equals("AM_ConfirmAbort") || param.Equals("AM_ConfirmVoid") ? Visibility.Collapsed : Visibility.Visible;
                    CancelAllBtn.Visibility = (bool)viewModel.GetPropertyValue("CancelAllShown") ? Visibility.Visible : Visibility.Collapsed;
                    CartControlExpand(true);                    
                    ShowPayButton(false);
                }
                else if (param.Equals("Finish"))
                {
                    taxShown = false;
                    OnReceiptItemCountChange();
                }
                else if (param.Equals("Payment"))
                {
                    OnTotalChanged();
                    OnAmountDueChanged();
                    OnTaxChanged();
                    CartControlExpand(false);
                    GoBackBtn.Visibility = Visibility.Collapsed;
                    CancelAllBtn.Visibility = Visibility.Collapsed;
                }
                else if (param.Equals("Bag"))
                {
                    if (CartReceipt.Items.Count > 0)
                    {
                        CartReceipt.ScrollIntoView(CartReceipt.Items[CartReceipt.Items.Count - 1]);
                    }
                }
                else if (paramListForPayAndEditCartHideAndCartCollapsed.Contains(param))
                {
                    OnTotalChanged();
                    OnAmountDueChanged();
                    OnTaxChanged();
                    ShowPayButton(false);
                    CartControlExpand(false);
                    GoBackBtn.Visibility = Visibility.Collapsed;
                    CancelAllBtn.Visibility = Visibility.Collapsed;
                }                
                UpdateFinishButton();
                UpdateEditCartCommandParameter();
            }
        }

        /// <summary>
        /// UpdateCartReceiptIndexSelected method
        /// </summary>
        /// <param name="param">This is a parameter with a type of string</param>
        private void UpdateCartReceiptIndexSelected(string param)
        {
            switch (param)
            {
                case "Scan":
                case "QuickPick":
                case "ScanWithReward":
                case "QuickPickWithReward":
                    if (CartReceipt.Items.Count > 0 && CartReceipt.SelectedIndex == -1)
                    {
                        CartReceipt.SelectedIndex = 0;
                    }
                    break;
            }        
        }

        /// <summary>
        /// UpdateCancelAllButton method
        /// </summary>
        /// <param name="param">This is a parameter with a type of string</param>
        private void UpdateCancelAllButton(string param)
        {
            if (paramListForCancelAllVisibility.Contains(param))
            {
                this.CancelAllBtn.Visibility = (this.GoBackBtn.Visibility.Equals(Visibility.Visible) ||
                    viewModel.StateParam.Equals("AM_VoidItem")) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// ShowPayButton method
        /// </summary>   
        /// <param name="visibility">This is a parameter with a type of bool</param>
        private void ShowPayButton(bool visibility)
        {
            if (viewModel.StateParam.Equals("EnterCoupons"))
            {
                EditCartBtn.Visibility = Visibility.Collapsed;
                PayButton.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (viewModel.StateParam.Equals("CashPayment"))
            {
                EditCartBtn.Visibility = PayButton.Visibility = Visibility.Collapsed;                 
                Total.Visibility = Visibility.Visible;
            }
            else if (viewModel.StateParam.Equals("SmAssistMenu"))
            {   
                Total.Visibility = Visibility.Visible;
                EditCartBtn.Visibility = Convert.ToBoolean(viewModel.GetPropertyValue("SMButton2Shown")) ? Visibility.Visible : Visibility.Collapsed;
                GoBackBtn.Visibility = PayButton.Visibility = Visibility.Collapsed;
            }            
            else
            {
                EditCartBtn.Visibility = PayButton.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
                Total.Visibility = visibility ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// CartControlExpand method
        /// </summary>   
        /// <param name="expand">This is a parameter with a type of bool</param>
        private void CartControlExpand(bool expand)
        {
            Storyboard storyboard = null;
            DoubleAnimation ExpandAnimation = null;
            Double expandedCartWidth = 452;
            Double minimizedCartWidth = 340;

            if (viewModel.ShowTransitionEffects)
            {
                storyboard = new Storyboard();
                ExpandAnimation = new DoubleAnimation();
            }

            if (expand && cartControlBox.Width != expandedCartWidth)
            {
                if (null != ExpandAnimation)
                {
                    ExpandAnimation.From = minimizedCartWidth;
                    ExpandAnimation.To = expandedCartWidth;
                }
                else
                {
                    cartControlBox.Width = expandedCartWidth;
                }

            }
            else if (!expand && cartControlBox.Width > minimizedCartWidth)
            {
                if (null != ExpandAnimation)
                {
                    ExpandAnimation.From = expandedCartWidth;
                    ExpandAnimation.To = minimizedCartWidth;
                }
                else
                {
                    cartControlBox.Width = minimizedCartWidth;
                }
            }

            if (null != ExpandAnimation && null != ExpandAnimation.From)
            {
                ExpandAnimation.Duration = new Duration(TimeSpan.FromSeconds(.5));
                ExpandAnimation.AutoReverse = false;
                Storyboard.SetTargetName(ExpandAnimation, "cartControlBox");
                Storyboard.SetTargetProperty(ExpandAnimation, new PropertyPath(Grid.WidthProperty));
                storyboard.Children.Add(ExpandAnimation);
                storyboard.Begin(this);
            }
        }

        /// <summary>
        /// UpdateFinishButton method
        /// </summary>  
        private void UpdateFinishButton()
        {
            if (!viewModel.StoreMode)
            {
                EditCartBtn.IsEnabled = PayButton.IsEnabled = !paramListForPayAndEditCartBtnEnable.Contains(viewModel.StateParam);
            }
            else
            {
                EditCartBtn.IsEnabled = Convert.ToBoolean(viewModel.GetPropertyValue("SMButton2Enabled"));
            }
        }

        /// <summary>
        /// ViewModel PropertyChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of PropertyChangedEventArgs</param> 
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BackgroundStateParam"))
            {
                OnBackgroundStateParamChanged(viewModel.BackgroundStateParam);
            }
            else if (e.PropertyName.Equals("StoreMode"))
            {
                OnStoreModePropertyChanged();   
            }
            else if (e.PropertyName.Equals("StateParam"))
            {
                OnStateParamChanged(viewModel.StateParam);
            }
            else if (e.PropertyName.Equals("Total") || e.PropertyName.Equals("SMTotal"))
            {
                OnTotalChanged();
            }
            else if (e.PropertyName.Equals("Tax"))
            {
                OnTaxChanged();
            }
            else if (e.PropertyName.Equals("NextGenUIAmountDue"))
            {
                OnAmountDueChanged();
            }
            else if (e.PropertyName.Equals("TaxShown"))
            {
                ShowHideTax();
            }
            else if (e.PropertyName.Equals("ReceiptItemCount"))
            {
                OnReceiptItemCountChange();
            }
            else if (e.PropertyName.Equals("NextGenData"))
            {
                OnNextGenDataChanged();
            }
            else if (e.PropertyName.Equals("CurrentItem"))
            {
                UpdateCartReceiptIndexSelected(viewModel.StateParam);
            }
            else if (e.PropertyName.Equals("SMButton2Enabled"))
            {
                EditCartBtn.IsEnabled = Convert.ToBoolean(viewModel.GetPropertyValue("SMButton2Enabled"));
            }

            if ((bool)viewModel.GetPropertyValue("CancelAllShown"))
                UpdateCancelAllButton(viewModel.StateParam);
        }

        /// <summary>
        /// ShowHideTax method
        /// </summary>
        private void ShowHideTax()
        {
            taxShown = Convert.ToBoolean(viewModel.GetPropertyValue("TaxShown").ToString());
            if (taxShown)
            {
                TaxText.Visibility = TaxValue.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TaxText.Visibility = TaxValue.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// OnTotalChanged method
        /// </summary>
        private void OnTotalChanged()
        {
            string totalName = viewModel.StoreMode ? "SMTotal" : "Total";
            object total = this.viewModel.GetPropertyValue(totalName);            
            if (null != total)
            {
                double totalVal = 0;
                double.TryParse(total.ToString(), out totalVal);
                totalAmountValue.Value = totalVal;
            }
        }

        private void OnAmountDueChanged()
        {
            object due = this.viewModel.GetPropertyValue("NextGenUIAmountDue");
            if (null != due)
            {
                dueAmountValue.Value = payAmountValue.Value = due.ToString();
            }
        }

        /// <summary>
        /// OnTaxChanged method
        /// </summary>
        private void OnTaxChanged()
        {
            object val = this.viewModel.GetPropertyValue("Tax");
            if (null != val)
            {
                this.TaxValue.Value = val.ToString();
            }

        }

        /// <summary>
        /// OnReceiptItemCountChange method
        /// </summary>
        private void OnReceiptItemCountChange()
        {
            int count = 0;
            object val = viewModel.GetPropertyValue("ReceiptItemCount");
            if (null != val)
            {
                if (!int.TryParse(val.ToString(), out count))
                {
                    count = 0;
                }
            }
            this.ReceiptItemCount.Property(TextBlock.TextProperty).SetResourceFormattedValue(count > 1 ? "Items" : "Item", count);
        }

        /// <summary>
        /// OnNextGenDataChanged method
        /// </summary>
        private void OnNextGenDataChanged()
        {
            if ((viewModel.GetPropertyValue("NextGenData") as String).Equals("BagAndEAS"))
            {
                IsEnabled = false;
                CmDataCapture.Capture(CmDataCapture.MaskPerformance, "CartControl NextGenData=BagAndEAS");
            }
            else
            {
                IsEnabled = true;
            }
        }

        private void SetPanelZIndex()
        {
            if (viewModel.StateParam.Equals("VoidItem") || 
                viewModel.StateParam.Equals("VoidItemWithReward") ||
                viewModel.StateParam.Equals("AM_VoidItem"))
            {
                Panel.SetZIndex(this.cartControlBox, 1);
            }
            else
            {
                Panel.SetZIndex(this.cartControlBox, 0);
            }
        }

        private void UpdateCartReceiptItemsSource()
        {   
            switch (viewModel.StateParam)
            {                
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CouponNoMatch":
                case "AM_CouponNotAllowed":
                case "AM_CouponTooHigh":
                case "AM_CrateableItem":
                case "AM_CustomMessage":
                case "AM_EnterCouponValue":
                case "AM_EnterQuantity":
                case "AM_EnterWeight":
                case "AM_EnterWtForPriceEmbedded":
                case "AM_ItemQuantityExceeded":
                case "AM_KeyInCode":
                case "AM_KeyInPrice":
                case "AM_KeyInQuantity":
                case "AM_Processing":
                case "AM_RestrictedNotAllowed":
                case "AM_ScaleBroken":
                case "AM_UpdateNotAllowed":
                case "AM_VoidNoMatch":
                case "RemoteSystemMessage":
                case "SmAbort":
                case "SmAborted":
                case "SmAssistMenu":
                case "SmAssistEnterItemMenu":
                case "SmAssistMenuFinalize":
                case "SmAssistMenuFinalizeHC":
                case "SmAssistMenuHC":
                case "SmAssistUpdateItemMenu":
                case "SmAssistModifyTrxMenu":
                case "SmAssistSelectDept":
                case "SmAuthorization":
                case "SmAuthorizationHC":
                case "SmConfirmOnScreenSignature":
                case "SmConfirmSignature":
                case "SmConfirmSuspendedTransaction":
                case "SmDataEntry":
                case "SmDataEntryNumericSmall":
                case "SmDataEntrySmall":
                case "SmDataEntryWithDetailsList":
                case "SmDataEntryWithImageControl":
                case "SmDataEntryWithListBG":
                case "SmDataEntryWithVideoControl":
                case "SmEnterBirthdate":
                case "SmLoadLift":
                case "SmmKeyInItemCode":
                case "SmmKeyInWtTol":
                case "SmSelectModeOfOperation":
                case "SmSystemFunctions":
                case "SmUpdateItem":
                case "SmUtility":                
                case "SmVoidedItems":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceipt") as IEnumerable);
                    break;
                case "SmCouponItems":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceiptCoupon") as IEnumerable);
                    break;
                case "SmVisualItems":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceiptVisualItem") as IEnumerable);
                    break;
                case "SmRestrictedItems":
                case "SmVoidAndRemoveItem":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceiptRestricted") as IEnumerable);
                    break;
                case "AM_ConfirmAbort":
                case "AM_ConfirmVoid":
                case "AM_VoidItem":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceiptVoid") as IEnumerable);
                    break;
                case "SmSecurityLogs":
                    SetCartReceiptItemSource(viewModel.GetPropertyValue("StoreModeReceiptDelaySecurityList") as IEnumerable);
                    break;
                default:
                    if (!viewModel.StoreMode)
                    {
                        SetCartReceiptItemSource(viewModel.GetPropertyValue("CustomerReceipt") as IEnumerable);                        
                    }
                    break;
            }            
        }

        private void OnStoreModePropertyChanged()
        {
            if (viewModel.StoreMode && !viewModel.StateParam.Equals("AM_VoidItem"))
            {
                GoBackBtn.Visibility = Visibility.Collapsed;
                CancelAllBtn.Visibility = Visibility.Collapsed;
                CartControlExpand(false);
                RewardInfo.Visibility = Visibility.Collapsed;
                ShowPayButton(false);
            }
            UpdateCartReceiptItemsSource();
            OnTotalChanged();
        }

        private void SetCartReceiptItemSource(IEnumerable receiptItems)
        {
            Binding receiptBinding = new Binding();
            receiptBinding.Source = receiptItems;
            BindingOperations.SetBinding(CartReceipt, TouchListBox.ItemsSourceProperty, receiptBinding);
        }

        private void UpdateEditCartCommandParameter()
        {
            switch (viewModel.StateParam)
            { 
                case "SmAssistMenu":
                    EditCartBtn.CommandParameter = "SMButton2";                    
                    break;
                default:
                    EditCartBtn.CommandParameter = "VoidItem";
                    break;
            }
        }

        private void CartReceipt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.StoreMode)                
            {
                if (e.RemovedItems.Count != 0 && e.RemovedItems[0] != CartReceipt.SelectedValue && CartReceipt.Items.Count > 0)
                {
                    var item = CartReceipt.Items[CartReceipt.SelectedIndex < 0 ? 0 : CartReceipt.SelectedIndex] as CustomerReceiptItem;
                    if (null != item && !item.Voidable)
                    {
                        CartReceipt.SelectedValue = e.RemovedItems[0];
                    }
                }
                else if (e.RemovedItems.Count == 0 && !(CartReceipt.SelectedValue as CustomerReceiptItem).Voidable)
                {
                    CartReceipt.SelectedValue = null;
                }
            }            
        }       

        /// <summary>
        /// viewModel variable
        /// </summary>
        private IMainViewModel viewModel;     
    }
}
