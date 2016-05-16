using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SSCOUIModels;
using SSCOUIModels.Models;
using FPsxWPF.Controls;
using System.Collections.Specialized;
using System.Globalization;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for CartListItem.xaml
    /// </summary>
    public partial class CartListItem : Grid
    {
        public CartListItem()
        {
            InitializeComponent();
            this.viewModel = (IMainViewModel)Application.Current.MainWindow.DataContext;            
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.currentItem = this.DataContext as CustomerReceiptItem;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateCartListStyle();                        
            base.OnRenderSizeChanged(sizeInfo);   
        }

        private void UpdateCartListStyle()
        {
            if (NMI)
            {
                UpdateStyle(typeof(StackPanel), StackPanel.MarginProperty, new Thickness(8), CartListItemStackPanel);
                if (ShowInterventionSection)
                {
                    this.Resources["cartListBoxItemTextStyle"] = this.FindResource("NMIListItemDescTextStyle") as Style;
                    this.Resources["cartListItemPriceTextStyle"] = this.FindResource("NMIListItemPriceTextStyle") as Style;
                    this.Resources["cartListBoxSubItemTextStyle"] = this.FindResource("NMICartListBoxSubItemTextStyle") as Style;
                    this.Resources["cartListBoxSubItemDescTextStyle"] = this.FindResource("NMICartListBoxSubItemDescTextStyle") as Style;
                    this.Resources["cartListBoxSubItemPriceTextStyle"] = this.FindResource("NMICartListBoxSubItemPriceTextStyle") as Style;
                    this.Resources["CartDelayedInterventionStyle"] = this.FindResource("NMICartDelayedInterventionStyle") as Style;
                    this.Resources["cartListBoxLineNumberTextStyle"] = this.FindResource("NMICartListBoxLineNumberTextStyle") as Style;       
                }
            }
            else if (ShowInterventionSection)
            {                
                this.Resources["cartListBoxItemTextStyle"] = !viewModel.StoreMode ? this.FindResource("cartControlListItemDescTextStyle") as Style :
                    this.FindResource("SMCartListBoxItemTextStyle") as Style;
                this.Resources["cartListItemPriceTextStyle"] = !viewModel.StoreMode ? this.FindResource("cartListItemPriceTextStyle") as Style :
                    this.FindResource("SMCartListItemPriceTextStyle") as Style;
                this.Resources["cartListBoxSubItemTextStyle"] = !viewModel.StoreMode ? this.FindResource("cartListBoxSubItemTextStyle") as Style :
                    this.FindResource("SMCartListBoxSubItemTextStyle") as Style;                       
                this.Resources["cartListBoxSubItemDescTextStyle"] = !viewModel.StoreMode ? this.FindResource("cartListBoxSubItemDescTextStyle") as Style :
                    this.FindResource("SMCartListBoxSubItemDescTextStyle") as Style;                       
                this.Resources["cartListBoxSubItemPriceTextStyle"] = !viewModel.StoreMode ? this.FindResource("cartListBoxSubItemPriceTextStyle") as Style :
                    this.FindResource("SMCartListBoxSubItemPriceTextStyle") as Style;
            }
            if (NMIPopup)
            {
                UpdateStyle(typeof(DockPanel), DockPanel.MarginProperty, new Thickness(12, 10, 6, 10), ItemDockPanel);
                UpdateStyle(typeof(ItemsControl), ItemsControl.MarginProperty, new Thickness(12, 0, 6, 0), subItemsControl);
                this.Resources["cartListBoxItemTextStyle"] = this.FindResource("NMIPopupListItemDescTextStyle") as Style;
                this.Resources["cartListItemPriceTextStyle"] = this.FindResource("NMIPopupListItemPriceTextStyle") as Style;
                this.Resources["cartListBoxSubItemTextStyle"] = this.FindResource("NMIPopupListBoxSubItemTextStyle") as Style;
                this.Resources["cartListBoxSubItemDescTextStyle"] = this.FindResource("NMIPopupCartListBoxSubItemDescTextStyle") as Style;
                this.Resources["cartListBoxSubItemPriceTextStyle"] = this.FindResource("NMIPopupCartListBoxSubItemPriceTextStyle") as Style;
            }            
        }

        private void UpdateStyle(Type targetType, DependencyProperty property, object value, UIElement control)
        {
            Style style = new Style { TargetType = targetType };
            style.Setters.Add(new Setter(property, value));
            ((FrameworkElement)control).Style = style;
        }

        private void RemoveButton_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (null != currentItem)
            {
                string cancelItem = viewModel.StoreMode ? "SMCancelItem({0})" : "CancelItem({0})";
                this.viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, cancelItem, this.currentItem.Data));                        
            }            
        }

        public bool ShowInterventionSection
        {
            get
            {
                return Convert.ToBoolean(GetValue(ShowInterventionSectionProperty));
            }
            set
            {
                SetValue(ShowInterventionSectionProperty, value);
            }
        }

        public static DependencyProperty ShowInterventionSectionProperty = DependencyProperty.Register("ShowInterventionSection", typeof(bool), typeof(CartListItem));

        public bool ShowRemoveSection
        {
            get
            {
                return Convert.ToBoolean(GetValue(ShowRemoveSectionProperty));
            }
            set
            {
                SetValue(ShowRemoveSectionProperty, value);
            }
        }

        public static DependencyProperty ShowRemoveSectionProperty = DependencyProperty.Register("ShowRemoveSection", typeof(bool), typeof(CartListItem));

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

        public static DependencyProperty NMIProperty = DependencyProperty.Register("NMI", typeof(bool), typeof(CartListItem));

        public bool NMIPopup
        {
            get
            {
                return Convert.ToBoolean(GetValue(NMIPopupProperty));
            }
            set
            {
                SetValue(NMIPopupProperty, value);
            }
        }

        public static DependencyProperty NMIPopupProperty = DependencyProperty.Register("NMIPopup", typeof(bool), typeof(CartListItem));

        private IMainViewModel viewModel;
        private CustomerReceiptItem currentItem; 
    }
}
