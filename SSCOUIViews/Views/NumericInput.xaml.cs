using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using SSCOControls;
using SSCOUIModels;
using SSCOUIModels.Helpers;
using System.Globalization;
using System.Text.RegularExpressions;


namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for NumericInput.xaml
    /// </summary>
    public partial class NumericInput : PopupView
    {
        public NumericInput(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(String param)
        {
            UpdateInstructions();
            UpdateCurrentItem();
            UpdateLeadthruText();
            UpdateGoBackText();
            switch (param)
            {
                case "EnterId":
                    showImageOnPopup(false);
                    this.TitleControl.Text = GetPropertyStringValue("LeadthruText");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterId");
                    break;
                case "EnterQuantity":
                    if (checkItemImageExists())
                    {
                        showImageOnPopup(true);
                        checkItemDescriptionExists();
                    }
                    else
                    {
                        showImageOnPopup(false);
                    }
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("EnterQuantityTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterQuantity");
                    break;
                case "SellBags":
                    showImageOnPopup(false);
                    TitleControl.Text = GetPropertyStringValue("LeadthruText");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("SellBags");
                    break;
                case "CmDataEntryWithKeyBoard":
                    showImageOnPopup(false);
                    TitleControl.Text = GetPropertyStringValue("LeadthruText");
                    break;
                case "EnterCouponValue":
                    showImageOnPopup(false);
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("EnterCouponValueTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterCouponValue");
                    break;
                case "XMCashierID":
                case "XMCashierPassword":
                    showImageOnPopup(false);
                    break;
                default:
                    break;
            }

        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("NextGenData"))
            {
                UpdateInstructions();
            }
            else if (name.Equals("MessageBoxNoEcho"))
            {
                UpdateInstructions();
            }
            else if (name.Equals("CurrentItem"))
            {
                UpdateCurrentItem();
            }
            else if (name.Equals("LeadthruText"))
            {
                UpdateLeadthruText();
            }
            else if (name.Equals("ButtonGoBack"))
            {
                UpdateGoBackText();
            }
        }
        
        private void UpdateCurrentItem()
        {
            switch (viewModel.StateParam)
            {
                case "EnterQuantity":
                    if (Grid_WithImage.Visibility == Visibility.Collapsed && checkItemImageExists())
                    {
                        showImageOnPopup(true);
                        checkItemDescriptionExists();
                    }
                    break;
            }
        }

        private void UpdateLeadthruText()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashierID":
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("XM_ID");
                    break;
                case "XMCashierPassword":
                    this.TitleControl.Property(TextBlock.TextProperty).SetResourceValue("XM_PW");
                    break;
                default:
                    TitleControl.Text = GetPropertyStringValue("LeadthruText");
                    break;
            }
        }

        private void UpdateGoBackText()
        {
            GoBackButton.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
        }

        private void UpdateInstructions()
        {
            switch (this.viewModel.StateParam)
            {
                case "EnterId":
                    if (GetPropertyStringValue("NextGenData").Equals("EnterPassword"))
                    {
                        this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterPassword");
                    }
                    else if (GetPropertyStringValue("NextGenData").Equals("EnterID"))
                    {
                        this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterId");
                    }
                    break;
                case "CmDataEntryWithKeyBoard":
                    this.Instructions.Text = GetPropertyStringValue("MessageBoxNoEcho");
                    break;
                case "XMCashierID":
                case "XMCashierPassword":
                    this.Instructions.Text = GetPropertyStringValue("LeadthruText");
                    break;
            }
        }

        private void GoBackButton_TouchDown(object sender, TouchEventArgs e)
        {
            if (viewModel.StateParam.Equals("EnterCouponValue") && viewModel.BackgroundStateParam.Equals("Search"))
            {
                viewModel.ActionCommand.Execute("SearchPopupGoBack");
            }
            else if (viewModel.StateParam.Equals("XMCashierPassword"))
            {
                viewModel.ActionCommand.Execute("XMButton8");
            }
            else
            {
                viewModel.ActionCommand.Execute("ButtonGoBack");
            }
        }

        private void showImageOnPopup(bool show)
        {
            if (show)
            {
                Instructions.Visibility = Visibility.Collapsed;
                GoBackButton.Visibility = Visibility.Collapsed;
                Grid_WithImage.Visibility = Visibility.Visible;
            }
            else
            {
                Instructions.Visibility = Visibility.Visible;
                GoBackButton.Visibility = viewModel.StateParam.Equals("XMCashierID") ? Visibility.Hidden : Visibility.Visible;
                Grid_WithImage.Visibility = Visibility.Collapsed;
            }
        }

        private void checkItemDescriptionExists()
        {
            if (String.IsNullOrEmpty(ItemImageName.Text))
            {
                ItemImageUPC.Visibility = Visibility.Visible;
                ItemImageName.Visibility = Visibility.Collapsed;
            }
            else
            {
                ItemImageUPC.Visibility = Visibility.Collapsed;
                ItemImageName.Visibility = Visibility.Visible;
            }
        }

        private bool checkItemImageExists()
        {
            if (itemImage.Source != null)
            {
                return true;
            }
            return false;
        }

    }
}
