using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FPsxWPF.Controls;
using SSCOControls;
using SSCOUIModels.Controls;
using SSCOUIModels;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for TenderOptions.xaml
    /// </summary>
    public partial class TenderOptions : BackgroundView
    {
        private ObservableCollection<GridItem> cashback = null;
        private string cashbackstring = null;
        private ObservableCollection<GridItem> cashbackitems = null;

        public TenderOptions(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
        
        private void TenderOptions_Loaded(object sender, RoutedEventArgs e)
        {
            this.cashback.CollectionChanged += this.cashback_CollectionChanged;
            GetCashBackList();
        }

        private void TenderOptions_UnLoaded(object sender, RoutedEventArgs e)
        {
            this.cashback.CollectionChanged -= this.cashback_CollectionChanged;
        }

        private void cashback_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                GetCashBackList();
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("CashBack") || name.Equals("EBTCashBack"))
            {
                GetCashBackList();
            }
        }


        /// <summary>
        /// GetButtonList
        /// </summary>
        private void GetCashBackList()
        {
            this.cashback = viewModel.GetPropertyValue(cashbackstring) as ObservableCollection<GridItem>;
            if (null != this.cashback)
            {
                CashBackScreen();
            }
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {            
            switch (param)
            {
                case "SelectPayment":
                    cashbackstring = "CashBack";
                    GetCashBackList();
                    break;
                case "SelectPaymentEBT":
                    cashbackstring = "EBTCashBack";
                    GetCashBackList();
                    break;
            }
        }

        /// <summary>
        /// CashBack_Click() executed when a cashback button was clicked
        /// inside the CashBack TouchListBox
        /// <param name="sender">object sender</param>
        /// <param name="e"> RoutedEventArgs e</param>
        /// </summary>
        private void CashBack_Click(object sender, RoutedEventArgs e)
        {
            var listbox = sender as SSCOUISlidingGridPage;
            if (-1 != listbox.SelectedIndex && null != listbox.SelectedItem)
            {
                GridItem item = listbox.SelectedItem as GridItem;
                if (item.IsEnabled && item.IsVisible)
                {
                    int index = listbox.SelectedIndex + ((listbox.PageInfo.CurrentPageIndex) * 6) + 1;
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, cashbackstring + "({0})", index));
                }
            }
        }

        /// <summary>
        /// CashBackScreen() sets the buttons to be displayed in the TenderOptions.xaml
        /// </summary>
        private void CashBackScreen()
        {
            try
            {
                int pos = -1;
                cashbackitems = new ObservableCollection<GridItem>();
                String Logo = String.Empty;

                if (cashbackstring.Equals("CashBack"))
                {
                    Logo = "debitCashBackTenderLogoStyle";
                }
                else
                {
                    Logo = "ebtTenderLogoStyle";
                }
                foreach (GridItem item in cashback)
                {
                    item.Text = item.Text.Replace("\n", " ");
                    pos = item.Text.LastIndexOf('(');
                    if (-1 < pos)
                        item.Text = item.Text.Insert(pos, "\n");
                    item.Logo = Logo;
                    cashbackitems.Add(item);
                }
                CashBackOptions.GridItemsSource = cashbackitems;
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
