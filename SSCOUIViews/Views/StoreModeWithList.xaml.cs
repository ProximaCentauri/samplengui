using System;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SSCOControls;
using SSCOUIModels;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using FPsxWPF.Controls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreModeWithList.xaml
    /// </summary>
    public partial class StoreModeWithList : BackgroundView
    {
        public StoreModeWithList(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent(); 
            ((INotifyCollectionChanged)EntryList.Items).CollectionChanged += EntryList_CollectionChanged;
        }

        public override void OnStateParamChanged(string param)
        {
            UpdateStateParam(param);
        }

        private void UpdateStateParam(string param)
        {
            UpdateEntryListItemsSource();
            this.SMLine2Text.Visibility = Visibility.Collapsed;
            UpdateSMButton1();
            UpdateSMButton2();
            UpdateSMButton3();
            UpdateSMButton4();
            UpdateSMButton5();
            UpdateSMButton6();
            UpdateSMButton7();
            UpdateSMLine1();
            UpdateSMLine2();
            UpdateButtonItemsControl();
            UpdateInputPanel();
            UpdateListPanel();
            UpdateInstructionTextPanel();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("SMButton1Shown") || name.Equals("SMButton1Text") || name.Equals("SMButton1Enabled"))
            {
                UpdateSMButton1();
            }
            else if (name.Equals("SMButton2Shown") || name.Equals("SMButton2Text") || name.Equals("SMButton2Enabled"))
            {
                UpdateSMButton2();
            }
            else if (name.Equals("SMButton3Shown") || name.Equals("SMButton3Text") || name.Equals("SMButton3Enabled"))
            {
                UpdateSMButton3();
            }
            else if (name.Equals("SMButton4Shown") || name.Equals("SMButton4Text") || name.Equals("SMButton4Enabled"))
            {
                UpdateSMButton4();
            }
            else if (name.Equals("SMButton5Shown") || name.Equals("SMButton5Text") || name.Equals("SMButton5Enabled"))
            {
                UpdateSMButton5();
            }
            else if (name.Equals("SMButton6Shown") || name.Equals("SMButton6Text") || name.Equals("SMButton6Enabled"))
            {
                UpdateSMButton6();
            }
            else if (name.Equals("SMButton7Shown") || name.Equals("SMButton7Text") || name.Equals("SMButton7Enabled"))
            {
                UpdateSMButton7();
            }
            else if (name.Equals("SMLeadthruTextShown") || name.Equals("SMLeadthruText"))
            {
                UpdateSMLine1();
            }
            else if (name.Equals("LeadthruText2TextShown") || name.Equals("LeadthruText2Text"))
            {
                UpdateSMLine2();
            }
            else if (name.Equals("EchoMessage"))
            {
                ShowEchoPopup();
            }
            else if (name.Equals("SMReportReceipt") || name.Equals("SMMItemList"))
            {
                UpdateEntryListItemsSource();
            }
        }

        private void ShowEchoPopup()
        {
            if (this.viewModel.StateParam.Equals("SmReportsMenu")
                || this.viewModel.StateParam.Equals("EchoPopup"))
            {
                string echoMessage = GetPropertyStringValue("EchoMessage");
                if (!String.IsNullOrEmpty(echoMessage.Trim()) && !echoMessage.Trim().Equals("_"))
                {
                    this.viewModel.ActionCommand.Execute("ViewModelSet(Context;EchoPopup)");
                }
                else
                {
                    this.viewModel.ActionCommand.Execute("ViewModelSet(Context;)");
                }
            }
        }

        private void UpdateSMButton1()
        {
            UpdateSMButton1Shown();
            UpdateSMButton1Enabled();
            UpdateSMButton1Text();
            UpdateSMButton1CommandParameter();
        }

        private void UpdateSMButton2()
        {
            UpdateSMButton2Shown();
            UpdateSMButton2Enabled();
            UpdateSMButton2Text();
        }

        private void UpdateSMButton3()
        {
            UpdateSMButton3Shown();
            UpdateSMButton3Enabled();
            UpdateSMButton3Text();
        }

        private void UpdateSMButton4()
        {
            UpdateSMButton4Shown();
            UpdateSMButton4Enabled();
            UpdateSMButton4Text();
        }

        private void UpdateSMButton5()
        {
            UpdateSMButton5Shown();
            UpdateSMButton5Enabled();
            UpdateSMButton5Text();
        }

        private void UpdateSMButton6()
        {
            UpdateSMButton6Shown();
            UpdateSMButton6Enabled();
            UpdateSMButton6Text();
        }

        private void UpdateSMButton7()
        {
            UpdateSMButton7Shown();
            UpdateSMButton7Enabled();
            UpdateSMButton7Text();
        }

        private void UpdateSMLine1()
        {
            UpdateSMLine1Shown();
            UpdateSMLine1Text();
        }

        private void UpdateSMLine2()
        {
            UpdateSMLine2Shown();
            UpdateSMLine2Text();
        }

        private void UpdateSMButton1Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_605");
                    break;
                case "SmmEditWtExp":
                case "SmmEditWtSubCk":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("YesButtonText");
                    break;
                case "SmmEditWtTolerance":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_620");
                    break;
                case "SmmInvalidWtTol":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_58");
                    break;
                case "SmReportsMenu":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_1090");
                    break;
                case "SmRunReports":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("PrintReportButtonText");
                    break;
                default:
                    this.StoreButton1.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton1.Text = GetPropertyStringValue("SMButton1Text");
                    break;
            }
        }

        private void UpdateSMButton1Shown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("SMButton1Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton1Enabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("SMButton1Enabled");
        }

        private void UpdateSMButton1CommandParameter()
        {
            if (!this.viewModel.StateParam.Equals("SmReportsMenu"))
            {
                this.StoreButton1.CommandParameter = "SMButton1";
            }
        }

        private void UpdateSMButton2Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton2.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_952");
                    break;
                case "SmmEditWtExp":
                case "SmmEditWtSubCk":
                    this.StoreButton2.Property(ImageButton.TextProperty).SetResourceValue("NoButtonText");
                    break;
                case "SmReportsMenu":
                    this.StoreButton2.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_1093");
                    break;
                default:
                    this.StoreButton2.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton2.Text = GetPropertyStringValue("SMButton2Text");
                    break;
            }
        }

        private void UpdateSMButton2Shown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("SMButton2Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton2Enabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("SMButton2Enabled");
        }

        private void UpdateSMButton3Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton3.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_606");
                    break;
                case "SmReportsMenu":
                    this.StoreButton3.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_1094");
                    break;
                default:
                    this.StoreButton3.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton3.Text = GetPropertyStringValue("SMButton3Text");
                    break;
            }
        }

        private void UpdateSMButton3Shown()
        {
            this.StoreButton3.Visibility = GetPropertyBoolValue("SMButton3Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton3Enabled()
        {
            this.StoreButton3.IsEnabled = GetPropertyBoolValue("SMButton3Enabled");
        }

        private void UpdateSMButton4Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton4.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_607");
                    break;
                case "SmReportsMenu":
                    this.StoreButton4.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_1127");
                    break;
                default:
                    this.StoreButton4.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton4.Text = GetPropertyStringValue("SMButton4Text");
                    break;
            }
        }

        private void UpdateSMButton4Shown()
        {
            this.StoreButton4.Visibility = GetPropertyBoolValue("SMButton4Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton4Enabled()
        {
            this.StoreButton4.IsEnabled = GetPropertyBoolValue("SMButton4Enabled");
        }

        private void UpdateSMButton5Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton5.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_608");
                    break;
                default:
                    this.StoreButton5.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton5.Text = GetPropertyStringValue("SMButton5Text");
                    break;
            }
        }

        private void UpdateSMButton5Shown()
        {
            this.StoreButton5.Visibility = GetPropertyBoolValue("SMButton5Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton5Enabled()
        {
            this.StoreButton5.IsEnabled = GetPropertyBoolValue("SMButton5Enabled");
        }

        private void UpdateSMButton6Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                    this.StoreButton6.Property(ImageButton.TextProperty).SetResourceValue("ScotApp_609");
                    break;
                default:
                    this.StoreButton6.Property(ImageButton.TextProperty).Clear();
                    this.StoreButton6.Text = GetPropertyStringValue("SMButton6Text");
                    break;
            }
        }

        private void UpdateSMButton6Shown()
        {
            this.StoreButton6.Visibility = GetPropertyBoolValue("SMButton6Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton6Enabled()
        {
            this.StoreButton6.IsEnabled = GetPropertyBoolValue("SMButton6Enabled");
        }

        private void UpdateSMButton7Text()
        {
            this.StoreButton7.Text = GetPropertyStringValue("SMButton7Text");
        }

        private void UpdateSMButton7Shown()
        {
            this.StoreButton7.Visibility = GetPropertyBoolValue("SMButton7Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton7Enabled()
        {
            this.StoreButton7.IsEnabled = GetPropertyBoolValue("SMButton7Enabled");
        }

        private void UpdateSMLine1Shown()
        {
            this.SMLine1Text.Visibility = this.GetPropertyBoolValue("SMLeadthruTextShown") && this.GetPropertyStringValue("SMLeadthruText").Trim().Length > 0 ? Visibility.Visible : Visibility.Collapsed;

        }

        private void UpdateSMLine1Text()
        {
            this.SMLine1Text.Text = GetPropertyStringValue("SMLeadthruText");
        }

        private void UpdateSMLine2Shown()
        {
            this.SMLine2Text.Visibility = this.GetPropertyBoolValue("LeadthruText2TextShown") && this.GetPropertyStringValue("LeadthruText2Text").Trim().Length > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMLine2Text()
        {
            this.SMLine2Text.Text = GetPropertyStringValue("LeadthruText2Text");
        }

        private void UpdateButtonItemsControl()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmEditAdd":
                case "SmReportsMenu":
                case "SmRunReports":
                case "SmMediaStatus":
                case "SmDataEntryWithMediaList":
                case "SmDataEntryWithDetailsList":
                    this.ButtonListItemsControl.Style = this.FindResource("storeModeItemsControlSmallButtonStyle") as Style;
                    this.ButtonListItemsControl.Width = 280;
                    break;
                default:
                    this.ButtonListItemsControl.Style = this.FindResource("storeModeItemsControlMainButtonStyle") as Style;
                    this.ButtonListItemsControl.Width = 624;
                    break;
            }
        }


        private void UpdateInputPanel()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmAddWeight":
                    this.ButtonListItemsControl.Visibility = Visibility.Collapsed;
                    this.NumericPad.Visibility = Visibility.Collapsed;
                    this.TextPrompt.Visibility = Visibility.Visible;
                    break;
                case "SmmKeyInItemCode":
                case "SmmKeyInWtTol":
                    this.ButtonListItemsControl.Visibility = Visibility.Collapsed;
                    this.NumericPad.Visibility = Visibility.Visible;
                    this.TextPrompt.Visibility = Visibility.Collapsed;
                    break;
                default:
                    this.ButtonListItemsControl.Visibility = Visibility.Visible;
                    this.NumericPad.Visibility = Visibility.Collapsed;
                    this.TextPrompt.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateListPanel()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmmAddWeight":
                    this.BalanceScaleIcon.Visibility = Visibility.Visible;
                    this.EchoMessage.Visibility = Visibility.Visible;
                    this.EntryList.Visibility = Visibility.Collapsed;
                    this.BiggerPanel();
                    break;
                case "SmmEditAdd":
                    this.BalanceScaleIcon.Visibility = Visibility.Visible;
                    this.EchoMessage.Visibility = Visibility.Visible;
                    this.EntryList.Visibility = Visibility.Visible;
                    this.BiggerPanel();
                    this.SecurityListPanel();
                    break;
                case "SmMediaStatus":
                case "SmDataEntryWithMediaList":
                case "SmDataEntryWithDetailsList":
                    this.BalanceScaleIcon.Visibility = Visibility.Collapsed;
                    this.EchoMessage.Visibility = Visibility.Collapsed;
                    this.EntryList.Visibility = Visibility.Visible;
                    this.UpdateMediaListPanel();
                    this.ReportsListPanel();
                    this.EntryList.SelectedIndex = -1;
                    this.EntryList.DisableSelection = true;
                    break;
                case "SmRunReports":
                    this.BalanceScaleIcon.Visibility = Visibility.Collapsed;
                    this.EchoMessage.Visibility = Visibility.Collapsed;
                    this.EntryList.Visibility = Visibility.Visible;
                    this.SmallerPanel();
                    this.ReportsListPanel();
                    this.EntryList.SelectedIndex = -1;
                    this.EntryList.DisableSelection = true;
                    break;
                case "SmReportsMenu":
                    this.BalanceScaleIcon.Visibility = Visibility.Collapsed;
                    this.EchoMessage.Visibility = Visibility.Collapsed;
                    this.EntryList.Visibility = Visibility.Visible;
                    this.SmallerPanel();
                    this.ReportsListPanel();
                    this.EntryList.DisableSelection = false;
                    this.EntryList.SelectionMode = SelectionMode.Single;
                    this.EntryList.SelectedIndex = 0;
                    break;
                default:
                    this.BalanceScaleIcon.Visibility = Visibility.Collapsed;
                    this.EchoMessage.Visibility = Visibility.Collapsed;
                    this.EntryList.Visibility = Visibility.Collapsed;
                    this.BiggerPanel();
                    break;
            }
        }

        private void BiggerPanel()
        {
            this.StoreModeWithListBorder.Height = 560.0;
            this.StoreModeWithListBorder.Width = 1000.0;
            this.StoreModeWithListPanel.Margin = new Thickness(12, 20, 12, 0);
        }

        private void SmallerPanel()
        {
            this.StoreModeWithListBorder.Height = 548.0;
            this.StoreModeWithListBorder.Width = 984.0;
            this.StoreModeWithListPanel.Margin = new Thickness(24, 20, 24, 20);
        }

        private void UpdateMediaListPanel()
        {
            this.StoreModeWithListBorder.Height = 560.0;
            this.StoreModeWithListBorder.Width = 984.0;
            this.StoreModeWithListPanel.Margin = new Thickness(24, 20, 24, 0);
        }

        private void SecurityListPanel()
        {
            this.EntryList.Height = Double.NaN;
            this.EntryList.Width = Double.NaN;
            this.EntryList.Margin = new Thickness(0, 10, 0, 0);
            this.EntryList.Style = this.FindResource("securityMaintenanceListStyle") as Style;
        }

        private void ReportsListPanel()
        {
            this.EntryList.Height = 402;
            this.EntryList.Width = 632;
            this.EntryList.Margin = new Thickness(12, 0, 0, 0);
            this.EntryList.Style = this.FindResource("reportsMenuStyle") as Style;
        }

        private void UpdateInstructionTextPanel()
        {
            switch (viewModel.StateParam)
            {
                case "SmMediaStatus":
                    this.InstructionTextPanel.MinHeight = 35;
                    break;
                default:
                    this.InstructionTextPanel.MinHeight = 86;
                    break;
            }
        }

        private void UpdateEntryListItemsSource()
        {
            switch (viewModel.StateParam)
            {
                case "SmmEditAdd":
                    SetEntryListItemSource(viewModel.GetPropertyValue("SMMItemList") as IEnumerable);
                    break;
                case "SmMediaStatus":
                case "SmDataEntryWithMediaList":
                    SetEntryListItemSource(viewModel.GetPropertyValue("SMMediaList") as IEnumerable);
                    break;
                case "SmDataEntryWithDetailsList":
                    SetEntryListItemSource(viewModel.GetPropertyValue("SMDetailsList") as IEnumerable);
                    break;
                case "SmReportsMenu":
                case "SmRunReports":
                    SetEntryListItemSource(viewModel.GetPropertyValue("SMReportReceipt") as IEnumerable);
                    break;
            }
        }

        private void SetEntryListItemSource(IEnumerable receiptItems)
        {
            Binding receiptBinding = new Binding();
            receiptBinding.Source = receiptItems;
            BindingOperations.SetBinding(this.EntryList, TouchListBox.ItemsSourceProperty, receiptBinding);
        }

        private void EntryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.viewModel.StateParam.Equals("SmReportsMenu") && this.EntryList.SelectedItem != null)
            {
                this.StoreButton1.CommandParameter = String.Format("SMSelectReceiptItem({0})", ((CustomerReceiptItem)this.EntryList.SelectedItem).Data);
            }
        }

        private void EntryList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionChangedAction.Reset == e.Action && this.viewModel.StateParam.Equals("SmReportsMenu"))
            {
                this.EntryList.SelectedIndex = 0;
            }
        }
    }
}