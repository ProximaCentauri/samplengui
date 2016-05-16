using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using RPSWNET;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for Tender.xaml
    /// </summary>
    public partial class Tender : BackgroundView
    {
        private ObservableCollection<GridItem> tenders = null;
        private ObservableCollection<GridItem> othertenderitems = null;

        public Tender(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            GetButtonList();
        }

        private void tenders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                MainPaymentScreen();
                MainPaymentSeparatorVisibility();
            }
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            if (param.Equals("Payment"))
            {
                ShowMainPaymentScreen(true);
            }
        }

        /// <summary>
        /// ViewModel PropertyChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of DependencyPropertyChangedEventArgs</param> 
        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("TenderList"))
            {
                GetButtonList();
            }
        }

        private void GetButtonList()
        {
            this.tenders = viewModel.GetPropertyValue("TenderList") as ObservableCollection<GridItem>;
            if (null != this.tenders)
            {
                this.tenders.CollectionChanged += tenders_CollectionChanged;
                MainPaymentScreen();
                MainPaymentSeparatorVisibility();
            }
        }

        /// <summary>
        /// OtherPayment_Click sets the Other Payment Grid to be displayed in the Tender.xaml
        /// when the Other Payment Options Button is clicked in Main Payment Grid.
        /// </summary>
        private void OtherPayment_Click(object sender, RoutedEventArgs e)
        {
            ShowMainPaymentScreen(false);
            OtherTenderList.SelectedPage = 0;
        }

        /// <summary>
        /// GoBackTender_Click() sets the Main Payment Grid to be displayed in the Tender.xaml
        /// when the Go Back Button is clicked in Other Payment Grid.
        /// <param name="sender">Object sender</param>
        /// <param name="e">RoutedEventArgs e</param>
        /// </summary>
        private void GoBackTender_Click(object sender, RoutedEventArgs e)
        {
            ShowMainPaymentScreen(true);
        }

        /// <summary>
        /// Tender_Click() executed when a tender button was clicked 
        /// inside the Other Tenders TouchListBox
        /// <param name="sender">object sender</param>
        /// <param name="e"> RoutedEventArgs e</param>
        /// </summary>
        private void Tender_Click(object sender, RoutedEventArgs e)
        {
            var listbox = sender as SSCOUISlidingGridPage;

            if (-1 != listbox.SelectedIndex && null != listbox.SelectedItem)
            {
                GridItem item = listbox.SelectedItem as GridItem;
                if (item.IsEnabled && item.IsVisible)
                {
                    int index = listbox.SelectedIndex + ((listbox.PageInfo.CurrentPageIndex) * 6);
                    string[] data = othertenderitems[index].Data.Split(';');
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "Tender({0})", data[0]));
                }
            }
        }

        /// <summary>
        /// ShowMainPaymentScreen() sets the visibility of either the Main payment Screen 
        /// or the Other Payment screen base on the parameter visibility.
        /// <param name="visibility">bool visibility</param>
        /// </summary>
        private void ShowMainPaymentScreen(bool visibility)
        {
            //Mainpayment Screen
            MainPayment.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
            GoBackSale.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;

            //OtherPayment Screen
            OtherPayment.Visibility = visibility ? Visibility.Collapsed : Visibility.Visible;
            GoBackTender.Visibility = visibility ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// MainPaymentSeparatorVisibility() sets the Line Separators(1 & 2) in the Main Payment Grid 
        /// to be displayed in Tender.xaml
        /// </summary>
        private void MainPaymentSeparatorVisibility()
        {
            LineSeparator1.Height = Tender1.Height;
            LineSeparator2.Height = Tender2.Height;
            LineSeparator3.Height = Tender3.Height;

            LineSeparator2.Visibility = Visibility.Collapsed;
            LineSeparator1.Visibility = Visibility.Collapsed;
            if (Visibility.Visible == Tender3.Visibility &&
               (Visibility.Visible == Tender2.Visibility ||
                Visibility.Visible == Tender1.Visibility))
            {
                LineSeparator2.Visibility = Visibility.Visible;
            }
            if (Visibility.Visible == Tender1.Visibility && Visibility.Visible == Tender2.Visibility)
            {
                LineSeparator1.Visibility = Visibility.Visible;
            }
        }

        private int mainTenderCount = 3; // default value

        /// <summary>
        /// Set the Main Tender button types
        /// The first tenders, determined by index, will occupy the main tenders. The other remaining goes to Other tenders
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tenderInfo"></param>
        private void SetTenderButton(int index, GridItem tenderInfo)
        {
            if (index <= mainTenderCount)
            {
                switch (index)
                {
                    case 1:
                        SetTenderType(index, Tender1, tenderInfo);
                        break;
                    case 2:
                        SetTenderType(index, Tender2, tenderInfo);
                        break;
                    case 3:
                        SetTenderType(index, Tender3, tenderInfo);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                AddToOtherTenders(tenderInfo);
            }
        }

        private void AddToOtherTenders(GridItem tenderInfo)
        {
            String[] tendertype = null;
            int pos = -1;

            tendertype = tenderInfo.Data.Split(';');
            pos = tenderInfo.Text.LastIndexOf(' ');

            tenderInfo.Text = tenderInfo.Text.Replace("\n", "");
            if (-1 < pos)
                tenderInfo.Text = tenderInfo.Text.Insert(pos + 1, "\n");

            tenderInfo.Logo = SetOtherTenderImage(tendertype[1]);

            if (tenderInfo.IsVisible)
                othertenderitems.Add(tenderInfo);
        }

        private void SetTenderType(int index, ImageButton tenderButton, GridItem tenderInfo)
        {
            String[] tendertype = null;
            bool error = false;

            tendertype = tenderInfo.Data.Split(';');
            switch (tendertype[1])
            {
                case "1":
                    tenderButton.Style = this.FindResource("cashTenderButtonStyle") as Style;
                    break;
                case "11":
                case "2":
                    tenderButton.Style = this.FindResource("cardTenderButtonStyle") as Style;
                    break;
                case "12":
                case "3":
                case "5":
                case "15":
                case "6":
                case "16":
                    tenderButton.Style = this.FindResource("debitTenderButtonStyle") as Style;
                    break;
                case "3cb":
                case "13":
                    tenderButton.Style = this.FindResource("debitCashBackTenderButtonStyle") as Style;
                    break;
                case "4":
                case "14":
                    tenderButton.Style = this.FindResource("loyaltyTenderButtonStyle") as Style;
                    break;
                case "7":
                case "17":
                    tenderButton.Style = this.FindResource("ebtTenderButtonStyle") as Style;
                    break;
                case "8":
                case "18":
                    tenderButton.Style = this.FindResource("ebtFoodStampsTenderButtonStyle") as Style;
                    break;
                case "9":
                case "19":
                    tenderButton.Style = this.FindResource("giftCardTenderButtonStyle") as Style;
                    break;
                case "10":
                    tenderButton.Style = this.FindResource("genericTenderButtonStyle") as Style;
                    break;
                case "40":
                    tenderButton.Style = this.FindResource("checkTenderButtonStyle") as Style;
                    break;
                case "42":
                case "-1":
                    tenderButton.Style = this.FindResource("otherTenderButtonStyle") as Style;
                    break;
                case "43":
                    tenderButton.Style = this.FindResource("voucherTenderButtonStyle") as Style;
                    break;
                case "-2":
                    tenderButton.Style = this.FindResource("couponTenderButtonStyle") as Style;
                    break;
                default:
                    error = true;
                    CmDataCapture.Capture(CmDataCapture.MaskError, "SSCOUIViews.Views.SetTenderType() Tender type error.");
                    break;
            }

            if (!error)
            {
                tenderButton.Text = tenderInfo.Text;
                tenderButton.CommandParameter = String.Format(CultureInfo.InvariantCulture, "Tender({0})", index);
                tenderButton.Visibility = tenderInfo.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                tenderButton.IsEnabled = tenderInfo.IsEnabled;
            }
        }

        /// <summary>
        /// MainPaymentScreen() sets the Main Tender Buttons(Debit, Credit, or Cash)
        /// in the Main Payment Grid to be displayed in Tender.xaml
        /// </summary>
        private void MainPaymentScreen()
        {
            int index = 1;
            if (SSCOUIViews.Properties.Settings.Default.MainTenderCount >= 1 && SSCOUIViews.Properties.Settings.Default.MainTenderCount <= 3)
            {
                mainTenderCount = SSCOUIViews.Properties.Settings.Default.MainTenderCount;
            }

            ShowMainPaymentScreen(true);
            othertenderitems = new ObservableCollection<GridItem>();
            
            Tender1.Visibility = Visibility.Collapsed;
            Tender2.Visibility = Visibility.Collapsed;
            Tender3.Visibility = Visibility.Collapsed;

            try
            {
                foreach (GridItem element in tenders)
                {
                    SetTenderButton(index, element);
                    index++;
                }
                OtherTenderList.GridItemsSource = othertenderitems;
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// SetOtherTenderImage() sets the Other Tender Buttons's Image
        /// of the Other Payment Grid to be displayed in Tender.xaml
        /// <param name="tendertype">string type of tendertype</param>
        /// <returns>String Logo</returns>
        /// </summary>
        private static String SetOtherTenderImage(string type)
        {
            String Logo = "";
            switch (type)
            {
                case "-2":  //COUPON
                    Logo = "couponTenderLogoStyle";
                    break;
                case "-1":  //OTHER PAYMENT
                case "42":  //ASSISTTENDER
                    Logo = "otherTenderLogoStyle";
                    break;
                case "1":
                    Logo = "cashTenderLogoStyle";
                    break;
                case "11":  //PIP_CREDIT
                case "2":
                    Logo = "cardTenderLogoStyle";
                    break;
                case "12":  //PIP_DEBIT
                case "3":   //DEBIT CARD
                case "5":   //DEBITSAVING
                case "15":  //PIP_DEBITSAVING
                case "6":   //DEBITCHECKING
                case "16":  //PIP_DEBITCHECKING
                    Logo = "debitTenderLogoStyle";
                    break;
                case "3cb": //DEBIT_CASHBACK
                case "13":  //PIP_DEBIT_CASHBACK
                    Logo = "debitCashBackTenderLogoStyle";
                    break;
                case "4":   //LOYALTY
                case "14":  //PIP_LOYALTY
                    Logo = "loyaltyTenderLogoStyle";
                    break;
                case "7":   //EBT
                case "17":  //PIP_EBT
                    Logo = "ebtTenderLogoStyle";
                    break;
                case "8":   //EBTFOODSTAMPS
                case "18":  //PIP_EBTFOODSTAMPS
                    Logo = "ebtFoodStampsTenderLogoStyle";
                    break;
                case "9":   //GIFTCARD
                case "19":  //PIP_GIFTCARD
                    Logo = "giftCardTenderLogoStyle";
                    break;
                case "10":  //GENERIC
                    Logo = "genericTenderLogoStyle";
                    break;
                case "40":  //CHECK
                    Logo = "checkTenderLogoStyle";
                    break;
                case "43":  //VOUCHER
                    Logo = "voucherTenderLogoStyle";
                    break;
            }
            return Logo;
        }
    }
}
