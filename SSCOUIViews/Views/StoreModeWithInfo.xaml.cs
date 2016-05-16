using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using SSCOUIModels;
using SSCOControls;
using System.Globalization;
using SSCOUIModels.Helpers;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class StoreModeWithInfo : BackgroundView
    {
        public StoreModeWithInfo(IMainViewModel viewModel)
            : base(viewModel)
        {
            this.InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            this.UpdateSMInfoBoxField();
            this.UpdateSMInfoBox();
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

        public void StoreButton1_TouchDown(object sender, TouchEventArgs e)
        { 
            switch(this.viewModel.StateParam)
            {
                case "SmAuthorization":                
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton1");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_CloseLane");
                    break;
            }
        }

        public void StoreButton2_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton2");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "CmdBtn_Suspend({0})", String.Empty));

                    break;
            }
        }

        public void StoreButton3_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton3");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_Reporting");
                    break;
            }
        }

        public void StoreButton4_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton4");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_SystemFunctions");
                    break;
            }
        }

        public void StoreButton5_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton5");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_MediaStatus");
                    break;
            }
        }

        public void StoreButton6_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    viewModel.ActionCommand.Execute("SMButton6");
                    break;
                case "SmAuthorizationHC":                    
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "CmdBtn_FullAssist({0})", String.Empty));
                    break;
            }
        }

        public void StoreButton7_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("SMButton7");
                    break;                
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "SMInfoBoxField":
                    this.UpdateSMInfoBoxField();
                    break;
                case "SMInfoBox":
                    this.UpdateSMInfoBox();
                    break;
                case "SMButton1Shown":
                case "CmdBtn_CloseLaneShown":
                    this.UpdateSMButton1Shown();
                    break;
                case "SMButton1Enabled":
                case "CmdBtn_CloseLaneEnabled":
                    this.UpdateSMButton1Enabled();
                    break;
                case "SMButton2Shown":
                case "CmdBtn_SuspendShown":
                    this.UpdateSMButton2Shown();
                    break;
                case "SMButton2Enabled":
                case "CmdBtn_SuspendEnabled":
                    this.UpdateSMButton2Enabled();
                    break;
                case "SMButton3Shown":
                case "CmdBtn_ReportingShown":
                    this.UpdateSMButton3Shown();
                    break;
                case "SMButton3Enabled":
                case "CmdBtn_ReportingEnabled":
                    this.UpdateSMButton3Enabled();
                    break;
                case "SMButton4Shown":
                case "CmdBtn_SystemFunctionsShown":
                    this.UpdateSMButton4Shown();
                    break;
                case "SMButton4Enabled":
                case "CmdBtn_SystemFunctionsEnabled":
                    this.UpdateSMButton4Enabled();
                    break;
                case "SMButton5Shown":
                case "CmdBtn_MediaStatusShown":
                    this.UpdateSMButton5Shown();
                    break;
                case "SMButton5Enabled":
                case "CmdBtn_MediaStatusEnabled":
                    this.UpdateSMButton5Enabled();
                    break;
                case "SMButton6Shown":
                case "CmdBtn_FullAssistShown":
                    this.UpdateSMButton6Shown();
                    break;
                case "SMButton6Enabled":
                case "CmdBtn_FullAssistEnabled":
                    this.UpdateSMButton6Enabled();
                    break;
                case "SMButton7Shown":
                    this.UpdateSMButton7Shown();
                    break;
                case "SMButton7Enabled":
                    this.UpdateSMButton7Enabled();
                    break;
                case "SMButton1Text":
                case "CmdBtn_CloseLaneText":
                    this.UpdateSMButton1Text();
                    break;
                case "SMButton2Text":
                case "CmdBtn_SuspendText":
                    this.UpdateSMButton2Text();
                    break;
                case "SMButton3Text":
                case "CmdBtn_ReportingText":
                    this.UpdateSMButton3Text();
                    break;
                case "SMButton4Text":
                case "CmdBtn_SystemFunctionsText":
                    this.UpdateSMButton4Text();
                    break;
                case "SMButton5Text":
                case "CmdBtn_MediaStatusText":
                    this.UpdateSMButton5Text();
                    break;
                case "SMButton6Text":
                case "CmdBtn_FullAssistText":
                    this.UpdateSMButton6Text();
                    break;
                case "SMButton7Text":
                    this.UpdateSMButton7Text();
                    break;
                case "SMLeadthruText":
                case "LeadthruText2Text":
                case "SMLeadthruTextShown":
                case "LeadthruText2TextShown":
                    this.UpdateSMLineText();
                    this.UpdateSMLineShown();
                    break;
            }
            
        }

        private void UpdateSMButton1Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton1.Visibility = (this.GetPropertyBoolValue("SMButton1Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton1.Visibility = (this.GetPropertyBoolValue("CmdBtn_CloseLaneShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }            
        }

        private void UpdateSMButton2Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton2.Visibility = (this.GetPropertyBoolValue("SMButton2Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton2.Visibility = (this.GetPropertyBoolValue("CmdBtn_SuspendShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }            
        }

        private void UpdateSMButton3Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton3.Visibility = (this.GetPropertyBoolValue("SMButton3Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton3.Visibility = (this.GetPropertyBoolValue("CmdBtn_ReportingShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }            
        }

        private void UpdateSMButton4Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton4.Visibility = (this.GetPropertyBoolValue("SMButton4Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton4.Visibility = (this.GetPropertyBoolValue("CmdBtn_SystemFunctionsShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateSMButton5Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton5.Visibility = (this.GetPropertyBoolValue("SMButton5Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton5.Visibility = (this.GetPropertyBoolValue("CmdBtn_MediaStatusShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateSMButton6Shown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton6.Visibility = (this.GetPropertyBoolValue("SMButton6Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton6.Visibility = (this.GetPropertyBoolValue("CmdBtn_FullAssistShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateSMButton7Shown()
        {
            this.StoreButton7.Visibility = (this.GetPropertyBoolValue("SMButton7Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton1Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton1.IsEnabled = (this.GetPropertyBoolValue("SMButton1Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton1.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_CloseLaneEnabled")) ? true : false;
                    break;
            }
        }

        private void UpdateSMButton2Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton2.IsEnabled = (this.GetPropertyBoolValue("SMButton2Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton2.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_SuspendEnabled")) ? true : false;
                    break;
            }
        }

        private void UpdateSMButton3Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton3.IsEnabled = (this.GetPropertyBoolValue("SMButton3Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton3.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_ReportingEnabled")) ? true : false;
                    break;
            }            
        }

        private void UpdateSMButton4Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton4.IsEnabled = (this.GetPropertyBoolValue("SMButton4Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton4.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_SystemFunctionsEnabled")) ? true : false;
                    break;
            }             
        }

        private void UpdateSMButton5Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton5.IsEnabled = (this.GetPropertyBoolValue("SMButton5Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton5.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_MediaStatusEnabled")) ? true : false;
                    break;
            }            
        }

        private void UpdateSMButton6Enabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton6.IsEnabled = (this.GetPropertyBoolValue("SMButton6Enabled")) ? true : false;
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton6.IsEnabled = (this.GetPropertyBoolValue("CmdBtn_FullAssistEnabled")) ? true : false;
                    break;
            }              
        }

        private void UpdateSMButton7Enabled()
        {
            this.StoreButton7.IsEnabled = (this.GetPropertyBoolValue("SMButton7Enabled")) ? true : false;
        }

        private void UpdateSMButton1Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton1.Text = this.GetPropertyStringValue("SMButton1Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton1.Text = this.GetPropertyStringValue("CmdBtn_CloseLaneText");
                    break;
            }
        }

        private void UpdateSMButton2Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton2.Text = this.GetPropertyStringValue("SMButton2Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton2.Text = this.GetPropertyStringValue("CmdBtn_SuspendText");
                    break;
            }
        }

        private void UpdateSMButton3Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton3.Text = this.GetPropertyStringValue("SMButton3Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton3.Text = this.GetPropertyStringValue("CmdBtn_ReportingText");
                    break;
            }
        }

        private void UpdateSMButton4Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton4.Text = this.GetPropertyStringValue("SMButton4Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton4.Text = this.GetPropertyStringValue("CmdBtn_SystemFunctionsText");
                    break;
            }
        }

        private void UpdateSMButton5Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton5.Text = this.GetPropertyStringValue("SMButton5Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton5.Text = this.GetPropertyStringValue("CmdBtn_MediaStatusText");
                    break;
            }
        }

        private void UpdateSMButton6Text()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorization":
                case "SmConfirmSuspendedTransaction":
                case "SmUtility":
                case "RemoteSystemMessage":
                    this.StoreButton6.Text = this.GetPropertyStringValue("SMButton6Text");
                    break;
                case "SmAuthorizationHC":
                    this.StoreButton6.Text = this.GetPropertyStringValue("CmdBtn_FullAssistText");
                    break;
            }            
        }

        private void UpdateSMButton7Text()
        {
            this.StoreButton7.Text = this.GetPropertyStringValue("SMButton7Text");
        }

        private void UpdateSMInfoBoxField()
        {
            this.SMInfoBoxField.Visibility = (this.GetPropertyBoolValue("SMInfoBoxField")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMInfoBox()
        {
            this.SMInfoBox.Text = this.GetPropertyStringValue("SMInfoBox");

            if (!String.IsNullOrEmpty(this.SMInfoBox.Text))
            {
                this.StoreModeWithInfoFadingScrollViewer.ScrollToTop();
            }
        }        

        private void UpdateSMLineShown()
        {
            this.SMLineText.Visibility = !(this.SMLineText.Text.Equals(String.Empty)) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMLineText()
        {
            string line1 = string.Empty;
            string line2 = string.Empty;
            if ((this.GetPropertyBoolValue("SMLeadthruTextShown") && this.GetPropertyStringValue("SMLeadthruText").Length > 0))
            {
                line1 = this.GetPropertyStringValue("SMLeadthruText");
            }
            if ((this.GetPropertyBoolValue("LeadthruText2TextShown") && this.GetPropertyStringValue("LeadthruText2Text").Length > 0))
            {
                line2 = this.GetPropertyStringValue("LeadthruText2Text");              
            }
            this.SMLineText.Text = UIFormat.MultiLineFormat(new string[] { line1, line2 });  
        }
    }
}
