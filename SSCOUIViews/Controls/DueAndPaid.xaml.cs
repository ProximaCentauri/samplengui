using System;
using System.Windows;
using System.Windows.Controls;
using SSCOUIModels;
using SSCOUIModels.Models;
using System.ComponentModel;
using System.Globalization;
using SSCOControls;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for DueAndPaid.xaml
    /// </summary>
    public partial class DueAndPaid : StackPanel
    {
        public DueAndPaid()
        {
            InitializeComponent();
        }

        private void DueAndPaid_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);
            RefreshAmountDueAndPaid();
        }

        private void DueAndPaid_Unloaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged -= new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);

        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AmountPaid") || e.PropertyName.Equals("AmountDue"))
            {
                RefreshAmountDueAndPaid();
            }
        }

        private void RefreshAmountDueAndPaid()
        {
            String paid = viewModel.GetPropertyValue("AmountPaid") as String;
            String due = viewModel.GetPropertyValue("AmountDue") as String;
            if (!(String.IsNullOrEmpty(paid) || String.IsNullOrEmpty(due)))
            {
                string paid1 = paid.Replace(RegionInfo.CurrentRegion.CurrencySymbol, String.Empty);
                string due1 = due.Replace(RegionInfo.CurrentRegion.CurrencySymbol, String.Empty);
                decimal amountPaid = 0;
                decimal.TryParse(paid1, out amountPaid);
                decimal amountDue = 0;
                decimal.TryParse(due1, out amountDue);
                if (amountPaid > 0 && amountDue > 0)
                {
                    this.AmountDueText.Property(TextBlock.TextProperty).SetResourceFormattedValue("Due", due);
                    this.AmountPaidText.Property(TextBlock.TextProperty).SetResourceFormattedValue("Paid", paid);
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    this.Visibility = Visibility.Collapsed;
                }
            }
        }

        private IMainViewModel viewModel;
    }
}
