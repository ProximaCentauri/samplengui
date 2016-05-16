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
            UpdateCurrentItem();            
            UpdateGoBackButtonText();
            UpdateInstructions();
            UpdateLeadthruText();
            switch (param)
            {
                case "EnterId":
                    showImageOnPopup(false);
                    this.Instructions.Property(TextBlock.TextProperty).Clear();                    
                    this.Instructions.Text = string.Empty;
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
                default:
                    this.Instructions.Property(TextBlock.TextProperty).Clear();
                    this.GoBackButton.Property(ImageButton.TextProperty).Clear();                    
                    break;
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "NextGenData":                
                case "MessageBoxNoEcho":
                    UpdateInstructions();
                    break;
                case "CurrentItem":
                    UpdateCurrentItem();
                    break;
                case "LeadthruText":
                    UpdateLeadthruText();
                    break;                
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
            TitleControl.Text = GetPropertyStringValue("LeadthruText");
        }

        private void UpdateGoBackButtonText()
        {
            if (viewModel.StateParam.Equals("EnterId"))
            {                
                GoBackButton.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
            }
            else
            {
                this.GoBackButton.Property(ImageButton.TextProperty).Clear();
                this.GoBackButton.Text = string.Empty;
            }            
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
                default:
                    this.Instructions.Property(TextBlock.TextProperty).Clear();
                    this.GoBackButton.Property(ImageButton.TextProperty).Clear();
                    this.Instructions.Text = string.Empty;
                    this.GoBackButton.Text = string.Empty;
                    break;
            }
        }

        private void GoBackButton_TouchDown(object sender, TouchEventArgs e)
        {
            if (viewModel.StateParam.Equals("EnterCouponValue") && viewModel.BackgroundStateParam.Equals("Search"))
            {
                viewModel.ActionCommand.Execute("SearchPopupGoBack");
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
                GoBackButton.Visibility = Visibility.Visible;
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
