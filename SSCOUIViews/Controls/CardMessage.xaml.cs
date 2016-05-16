using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SSCOUIModels;
using SSCOUIModels.Models;
using FPsxWPF.Controls;
using SSCOControls;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for CardMessage.xaml
    /// </summary>
    public partial class CardMessage : MeasureTextBlock
    {
        public CardMessage()
        {
            InitializeComponent();
            this.viewModel = (IMainViewModel)Application.Current.MainWindow.DataContext;
        }

        public bool IsQuickPick
        { 
            get
            {
                return isQuickPick;
            }
            set
            {
                if (value != isQuickPick)
                {
                    isQuickPick = value;
                }                
            }
        }

        public bool IsSmaller
        {
            get
            {
                return this.isSmaller;
            }
            set
            {
                if (value != this.isSmaller)
                {
                    this.isSmaller = value;
                }
            }
        }

        private void CardMessage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.currentItem = this.DataContext as CustomerReceiptItem;
            ShowCardMessage();
        }

        private void CardMessage_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
            ShowCardMessage();
        }

        private void ApplyStyle()
        {
            if (this.IsSmaller)
            {
                this.Style = this.IsQuickPick ? this.FindResource("quickPickLoyaltySmallTextStyle") as Style : this.FindResource("loyaltySmallTextStyle") as Style;
            }
            else
            {
                this.Style = this.IsQuickPick ? this.FindResource("quickPickloyaltyTextStyle") as Style : this.FindResource("loyaltyTextStyle") as Style;
            }     
        }

        private void ShowCardMessage()
        {
            object TBState = this.viewModel.GetPropertyValue("TBState");
            if (null != TBState)
            {
                if (null != this.currentItem && this.currentItem.Coupon && TBState.Equals("TB_ITEMSOLD"))
                {
                    this.Property(TextBlock.TextProperty).SetResourceValue("CouponMessage");
                }
                else if (TBState.Equals("TB_LOYALTYCARD"))
                {
                    this.Text = viewModel.GetPropertyValue("ScanBagTextArea").ToString();
                }
                else if (TBState.Equals("TB_VOIDITEMBEFORECOUPON"))
                {
                    this.Property(TextBlock.TextProperty).SetResourceValue("VoidItemBeforeCouponMessage");
                }
                else
                {
                    this.Property(TextBlock.TextProperty).Clear();
                    this.Text = String.Empty;
                }
            }
            if (this.Text.Length != 0)
            {                             
                ApplyStyle();
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("ScanBagTextArea"))
            {
                ShowCardMessage();
            }
        }

        private void CardMessage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private CustomerReceiptItem currentItem;
        private static bool isQuickPick;
        private bool isSmaller;        
        private IMainViewModel viewModel;                
    }     
}
