using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SSCOUIModels.Controls;
using SSCOUIModels;
using SSCOControls;
using System.ComponentModel;
using SSCOUIModels.Helpers;
using System.Windows.Data;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreModeWithKeypad.xaml
    /// </summary>
    public partial class StoreModeWithKeypad : BackgroundView
    {
        public StoreModeWithKeypad(IMainViewModel viewModel)
            : base(viewModel)
        {
            
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            this.UpdateSMButton1Shown();
            this.UpdateSMButton2Shown();
            this.UpdateSMButton3Shown();
            this.UpdateSMButton4Shown();
            this.UpdateSMButton5Shown();
            this.UpdateSMButton6Shown();
            this.UpdateSMButton7Shown();
            this.UpdateSMButton1Enabled();
            this.UpdateSMButton2Enabled();
            this.UpdateSMButton3Enabled();
            this.UpdateSMButton4Enabled();
            this.UpdateSMButton5Enabled();
            this.UpdateSMButton6Enabled();
            this.UpdateSMButton7Enabled();
            this.UpdateSMButton1Text();
            this.UpdateSMButton2Text();
            this.UpdateSMButton3Text();
            this.UpdateSMButton4Text();
            this.UpdateSMButton5Text();
            this.UpdateSMButton6Text();
            this.UpdateSMButton7Text();
            this.UpdateSMLineText();
            this.UpdateSMLineShown();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "SMButton1Shown":
                    this.UpdateSMButton1Shown();
                    break;
                case "SMButton2Shown":
                    this.UpdateSMButton2Shown(); 
                    break;
                case "SMButton3Shown":
                    this.UpdateSMButton3Shown();
                    break;
                case "SMButton4Shown":
                    this.UpdateSMButton4Shown();
                    break;
                case "SMButton5Shown":
                    this.UpdateSMButton5Shown();
                    break;
                case "SMButton6Shown":
                    this.UpdateSMButton6Shown();
                    break;
                case "SMButton7Shown":
                    this.UpdateSMButton7Shown();
                    break;
                case "SMButton1Text":
                    this.UpdateSMButton1Text();
                    break;
                case "SMButton2Text":
                    this.UpdateSMButton2Text();
                    break;
                case "SMButton3Text":
                    this.UpdateSMButton3Text();
                    break;
                case "SMButton4Text":
                    this.UpdateSMButton4Text();
                    break;
                case "SMButton5Text":
                    this.UpdateSMButton5Text();
                    break;
                case "SMButton6Text":
                    this.UpdateSMButton6Text();
                    break;
                case "SMButton7Text":
                    this.UpdateSMButton7Text();
                    break;
                case "SMButton1Enabled":
                    this.UpdateSMButton1Enabled();
                    break;
                case "SMButton2Enabled":
                    this.UpdateSMButton2Enabled();
                    break;
                case "SMButton3Enabled":
                    this.UpdateSMButton3Enabled();
                    break;
                case "SMButton4Enabled":
                    this.UpdateSMButton4Enabled();
                    break;
                case "SMButton5Enabled":
                    this.UpdateSMButton5Enabled();
                    break;
                case "SMButton6Enabled":
                    this.UpdateSMButton6Enabled();
                    break;
                case "SMButton7Enabled":
                    this.UpdateSMButton7Enabled();
                    break;
                case "SMLeadthruText":
                case "SMLeadthruTextShown":                 
                case "LeadthruText2Text":
                case "LeadthruText2TextShown":
                case "LeadthruText":
                case "LeadthruTextShown":                    
                case "MessageBoxNoEcho":                
                case "MessageBoxNoEchoShown":
                    this.UpdateSMLineText();
                    this.UpdateSMLineShown();
                    break;                    
            }           
        }

        private void UpdateSMButton1Shown()
        {
            this.StoreButton1.Visibility = (this.GetPropertyBoolValue("SMButton1Shown") && this.GetPropertyStringValue("SMButton1Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton2Shown()
        {
            this.StoreButton2.Visibility = (this.GetPropertyBoolValue("SMButton2Shown") && this.GetPropertyStringValue("SMButton2Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton3Shown()
        {
            this.StoreButton3.Visibility = (this.GetPropertyBoolValue("SMButton3Shown") && this.GetPropertyStringValue("SMButton3Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton4Shown()
        {
            this.StoreButton4.Visibility = (this.GetPropertyBoolValue("SMButton4Shown") && this.GetPropertyStringValue("SMButton4Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton5Shown()
        {
            this.StoreButton5.Visibility = (this.GetPropertyBoolValue("SMButton5Shown") && this.GetPropertyStringValue("SMButton5Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton6Shown()
        {
            this.StoreButton6.Visibility = (this.GetPropertyBoolValue("SMButton6Shown") && this.GetPropertyStringValue("SMButton6Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton7Shown()
        {
            this.StoreButton7.Visibility = (this.GetPropertyBoolValue("SMButton7Shown") && this.GetPropertyStringValue("SMButton7Text").Length > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton1Text()
        {
            this.StoreButton1.Text = this.GetPropertyStringValue("SMButton1Text");
        }
        private void UpdateSMButton2Text()
        {
            this.StoreButton2.Text = this.GetPropertyStringValue("SMButton2Text");
        }
        private void UpdateSMButton3Text()
        {
            this.StoreButton3.Text = this.GetPropertyStringValue("SMButton3Text");
        }
        private void UpdateSMButton4Text()
        {
            this.StoreButton4.Text = this.GetPropertyStringValue("SMButton4Text");
        }
        private void UpdateSMButton5Text()
        {
            this.StoreButton5.Text = this.GetPropertyStringValue("SMButton5Text");
        }
        private void UpdateSMButton6Text()
        {
            this.StoreButton6.Text = this.GetPropertyStringValue("SMButton6Text");
        }
        private void UpdateSMButton7Text()
        {
            this.StoreButton7.Text = this.GetPropertyStringValue("SMButton7Text");
        }

        private void UpdateSMButton1Enabled()
        {
            this.StoreButton1.IsEnabled = this.GetPropertyBoolValue("SMButton1Enabled");
        }
        private void UpdateSMButton2Enabled()
        {
            this.StoreButton2.IsEnabled = this.GetPropertyBoolValue("SMButton2Enabled");
        }
        private void UpdateSMButton3Enabled()
        {
            this.StoreButton3.IsEnabled = this.GetPropertyBoolValue("SMButton3Enabled");
        }
        private void UpdateSMButton4Enabled()
        {
            this.StoreButton4.IsEnabled = this.GetPropertyBoolValue("SMButton4Enabled");
        }
        private void UpdateSMButton5Enabled()
        {
            this.StoreButton5.IsEnabled = this.GetPropertyBoolValue("SMButton5Enabled");
        }
        private void UpdateSMButton6Enabled()
        {
            this.StoreButton6.IsEnabled = this.GetPropertyBoolValue("SMButton6Enabled");
        }
        private void UpdateSMButton7Enabled()
        {
            this.StoreButton7.IsEnabled = this.GetPropertyBoolValue("SMButton7Enabled");
        }       

        private void UpdateSMLineShown()
        {
            this.SMLineText.Visibility = !(this.SMLineText.Text.Equals(String.Empty)) ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private void UpdateSMLineText()
        {
            string line1 = string.Empty;
            string line2 = string.Empty;
            string line3 = string.Empty;
            if ((this.GetPropertyBoolValue("SMLeadthruTextShown") && this.GetPropertyStringValue("SMLeadthruText").Length > 0))
            {
                line1 = this.GetPropertyStringValue("SMLeadthruText");
            }
            if ((this.GetPropertyBoolValue("LeadthruText2TextShown") && this.GetPropertyStringValue("LeadthruText2Text").Length > 0))
            {
                line2 = this.GetPropertyStringValue("LeadthruText2Text");
            }
            else if ((this.GetPropertyBoolValue("LeadthruTextShown") && this.GetPropertyStringValue("LeadthruText").Length > 0))
            {
                line2 = this.GetPropertyStringValue("LeadthruText");
            }
            if ((this.GetPropertyBoolValue("MessageBoxNoEchoShown") && this.GetPropertyStringValue("MessageBoxNoEcho").Length > 0))
            {
                line3 = this.GetPropertyStringValue("MessageBoxNoEcho");
            }
            this.SMLineText.Text = UIFormat.MultiLineFormat(new string[] { line1, line2, line3 });        
        }
        
    }
}
