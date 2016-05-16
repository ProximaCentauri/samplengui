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
using SSCOUIModels;
using System.ComponentModel;
using SSCOUIModels.Models;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for ExtendedCartControl.xaml
    /// </summary>
    public partial class ExtendedCartControl : Grid
    {
        public ExtendedCartControl()
        {
            InitializeComponent();
            CartReceipt.SelectionChanged += CartReceipt_SelectionChanged;
        }

        private void CartReceipt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CartReceipt.Items.Count > 0 && CartReceipt.SelectedIndex > -1)
            {
                int cnt = CartReceipt.Items.Count -1;
                while(cnt >= 0)
                {
                    if (selectItem(cnt))
                    {
                        break;
                    }
                    cnt--;
                }
                if (cnt == -1)
                {
                    CartReceipt.SelectedIndex = -1;
                }
            }
        }

        private bool selectItem(int index)
        {
            cartReceiptItem = CartReceipt.Items[index] as CustomerReceiptItem;
            if (cartReceiptItem.Voidable && !cartReceiptItem.Strikeout)
            {
                CartReceipt.SelectedIndex = index;
                return true;
            }            
            return false;
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = DataContext as IMainViewModel;
            viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
            OnAmountDueChanged();
            OnTotalChanged();
            OnTaxChanged();
            ShowHideTax();                     
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {   
            if (e.PropertyName.Equals("Total"))
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
            object val = this.viewModel.GetPropertyValue("Total");
            if (null != val)
            {         
                totalAmountValue.Value = val.ToString();
            }
        }

        private void OnAmountDueChanged()
        {
            object due = this.viewModel.GetPropertyValue("NextGenUIAmountDue");
            if (null != due)
            {
                dueAmountValue.Value = due.ToString();
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
        private IMainViewModel viewModel;
        private bool taxShown;
        private CustomerReceiptItem cartReceiptItem;
    }
}
