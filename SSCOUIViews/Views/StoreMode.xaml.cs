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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SSCOUIModels.Controls;
using SSCOUIModels;
using FPsxWPF.Controls;
using System.Globalization;
using RPSWNET;
using SSCOUIModels.Helpers;
using SSCOUIModels.Models;
using SSCOControls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreMode.xaml
    /// </summary>
    public partial class StoreMode : BackgroundView
    {
        public StoreMode(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            UpdateStateParam(param);
        }

        private void UpdateStateParam(string param)
        {
            this.SMLineText.Visibility = Visibility.Collapsed;
            UpdateSMButton1();
            UpdateSMButton2();
            UpdateSMButton3();
            UpdateSMButton4();
            UpdateSMButton5();
            UpdateSMButton6();
            UpdateSMButton7();
            UpdateCMButton1Med();
            UpdateCMButton2Med();
            UpdateCMButton1Lg();
            UpdateCMButton2();
            UpdateXMButton1();
            UpdateXMButton2();
            UpdateItemsControlPanel();
            UpdateSMLineText();
            UpdateSMLineShown();
        }

        private void StoreButton1_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CouponNoMatch":
                case "AM_CouponNotAllowed":
                case "AM_CouponTooHigh":
                case "AM_CrateableItem":
                case "AM_CustomMessage":
                case "AM_ItemQuantityExceeded":
                case "AM_RestrictedNotAllowed":
                case "AM_UpdateNotAllowed":
                case "AM_VoidNoMatch":
                    viewModel.ActionCommand.Execute("CMButton1Med");
                    break;
                case "AM_VoidItem":
                    viewModel.ActionCommand.Execute("CancelItem");
                    break;
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    viewModel.ActionCommand.Execute("XMButton1");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_CloseLane");
                    break;
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_KeyInCode({0})", "OnKeyInCode"));
                    break;
                case "SmAssistMenuFinalize":
                    viewModel.ActionCommand.Execute("CmdBtn_SystemFunctions");
                    break;
                case "SmAssistMenuFinalizeHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Suspend({0})", String.Empty));
                    break;
                case "SmAssistUpdateItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_ChangePrice({0})", "OnChangePrice"));
                    break;
                case "SmAssistEnterItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Button1({0})", "OnKeyInCode"));
                    break;
                case "SmAssistModifyItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Button1({0})", "OnChangePrice"));
                    break;
                case "SmAssistModifyTrxMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Button1({0})", "Function=EnterEmployeeNumber"));
                    break;
                case "SmUpdateItem":
                    ObservableCollection<ReceiptItem> storeModeReceiptCollection = this.viewModel.GetPropertyValue("StoreModeReceipt") as ObservableCollection<ReceiptItem>;
                    CustomerReceiptItem currentReceiptItem = CollectionViewSource.GetDefaultView(storeModeReceiptCollection).CurrentItem as CustomerReceiptItem;
                    viewModel.ActionCommand.Execute(String.Format("SMSelectReceiptItem({0})", currentReceiptItem.Data));
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton1");
                    break;
            }
        }

        private void StoreButton2_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                    viewModel.ActionCommand.Execute("CMButton2Med");
                    break;
                case "AM_VoidItem":
                    viewModel.ActionCommand.Execute("CancelAll");
                    break;
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    viewModel.ActionCommand.Execute("XMButton2");
                    break;
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_VoidItem({0})", "OnVoidButton"));
                    break;
                case "SmAssistMenuFinalize":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_MediaStatus({0})", String.Empty));
                    break;
                case "SmAssistMenuFinalizeHC":
                    viewModel.ActionCommand.Execute("CmdBtn_SystemFunctions");
                    break;
                case "SmAssistUpdateItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_ChangeQuantity({0})", "OnChangeQuantity"));
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Suspend({0})", String.Empty));
                    break;
                case "SmAssistEnterItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Button2({0})", "SellByDept"));
                    break;
                case "SmAssistModifyItemMenu":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Button2({0})", "OnChangeQuantity"));
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton2");
                    break;
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "SMButton1Shown":
                case "SMButton1Enabled":
                case "SMButton1Text":
                case "CmdBtn_KeyInCodeText":
                case "CmdBtn_KeyInCodeEnabled":
                case "CmdBtn_KeyInCodeShown":
                case "CmdBtn_ChangePriceText":
                case "CmdBtn_ChangePriceEnabled":
                case "CmdBtn_ChangePriceShown":
                case "CmdBtn_Button1Text":
                case "CmdBtn_Button1Enabled":
                case "CmdBtn_Button1Shown":
                    UpdateSMButton1();
                    break;
                case "SMButton2Shown":
                case "SMButton2Enabled":
                case "SMButton2Text":
                case "CmdBtn_VoidItemText":
                case "CmdBtn_VoidItemEnabled":
                case "CmdBtn_VoidItemShown":
                case "CmdBtn_SystemFunctionsText":
                case "CmdBtn_SystemFunctionsEnabled":
                case "CmdBtn_SystemFunctionsShown":
                case "CmdBtn_ChangeQuantityText":
                case "CmdBtn_ChangeQuantityEnabled":
                case "CmdBtn_ChangeQuantityShown":
                case "CmdBtn_Button2Text":
                case "CmdBtn_Button2Enabled":
                case "CmdBtn_Button2Shown":
                    UpdateSMButton2();
                    break;
                case "CMButton1MedShown":
                case "CMButton1MedText":
                case "CMButton1MedEnabled":
                    UpdateCMButton1Med();
                    break;
                case "CMButton2MedShown":
                case "CMButton2MedText":
                case "CMButton2MedEnabled":
                    UpdateCMButton2Med();
                    break;
                case "CMButton1LgShown":
                case "CMButton1LgText":
                case "CMButton1LgEnabled":
                    UpdateCMButton1Lg();
                    break;
                case "CancelAllShown":
                case "CMButton2Text":
                case "CMButton2Enabled":
                    UpdateCMButton2();
                    break;
                case "XMButton1Shown":
                case "XMButton1Text":
                case "XMButton1Enabled":
                    UpdateXMButton1();
                    break;
                case "XMButton2Shown":
                case "XMButton2Text":
                case "XMButton2Enabled":
                    UpdateXMButton2();
                    break;
                case "SMButton3Shown":
                case "SMButton3Text":
                case "SMButton3Enabled":
                case "CmdBtn_UpdateItemText":
                case "CmdBtn_UpdateItemEnabled":
                case "CmdBtn_UpdateItemShown":
                case "CmdBtn_MediaStatusText":
                case "CmdBtn_MediaStatusEnabled":
                case "CmdBtn_MediaStatusShown":
                case "CmdBtn_Button3Text":
                case "CmdBtn_Button3Enabled":
                case "CmdBtn_Button3Shown":
                    UpdateSMButton3();
                    break;
                case "SMButton4Shown":
                case "SMButton4Text":
                case "SMButton4Enabled":
                case "CmdBtn_SuspendText":
                case "CmdBtn_SuspendEnabled":
                case "CmdBtn_SuspendShown":
                    UpdateSMButton4();
                    break;
                case "SMButton5Shown":
                case "SMButton5Text":
                case "SMButton5Enabled":
                case "CmdBtn_PrintBarcodeText":
                case "CmdBtn_PrintBarcodeEnabled":
                case "CmdBtn_PrintBarcodeShown":
                    UpdateSMButton5();
                    break;
                case "SMButton6Shown":
                case "SMButton6Text":
                case "SMButton6Enabled":
                case "CmdBtn_ReportingText":
                case "CmdBtn_ReportingEnabled":
                case "CmdBtn_ReportingShown":
                    UpdateSMButton6();
                    break;
                case "SMButton7Shown":
                case "SMButton7Text":
                case "SMButton7Enabled":
                case "CmdBtn_FullAssistText":
                case "CmdBtn_FullAssistEnabled":
                case "CmdBtn_FullAssistShown":
                    UpdateSMButton7();
                    break;
                case "EchoMessage":
                    ShowEchoPopup();
                    break;
                case "SMLeadthruText":
                case "SMLeadthruTextShown":
                case "LeadthruText2Text":
               case "LeadthruText2TextShown":
                case "LeadthruText":
                case "LeadthruTextShown":
                case "Instructions":
                case "MessageBoxNoEcho":
                case "XMLeadthruText":
                    UpdateSMLineText();
                    UpdateSMLineShown();
                    break;
            }
            if (name.Equals("BackgroundStateParam"))
            {
                UpdateButtonOrientationForSmRestrictedItems();
            }
        }

        private void ShowEchoPopup()
        {
            if (this.viewModel.StoreMode)
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

        private void UpdateButtonOrientationForSmRestrictedItems()
        {
            if (viewModel.StateParam.Equals("SmRestrictedItems"))
            {
                this.StoreButton3.Margin = new Thickness(322, 0, 0, 0);
            }
            else if (viewModel.StateParam.Equals("SmDataEntryWithDetailsList"))
            {
                this.StoreButton3.Margin = new Thickness(6, 0, 6, 12);
            }
            else 
            {
                this.StoreButton3.Margin = new Thickness(12, 0, 12, 12);
            }
        }

        private void UpdateItemsControlPanel()
        {
            if (this.viewModel.StateParam.Equals("SmDataEntryWithCmdBtnList"))
            {
                this.ButtonListStackPanelForSmDataEntryWithCmdBtnList.Visibility = Visibility.Visible;
                this.ContainerStoreModeButtonsItemsControl.Visibility = Visibility.Collapsed;
                this.ButtonListStackPanelForSmAssistSelectDept.Visibility = Visibility.Collapsed;
            }
            else if (this.viewModel.StateParam.Equals("SmAssistSelectDept"))
            {
                this.ButtonListStackPanelForSmDataEntryWithCmdBtnList.Visibility = Visibility.Collapsed;
                this.ContainerStoreModeButtonsItemsControl.Visibility = Visibility.Collapsed;
                this.ButtonListStackPanelForSmAssistSelectDept.Visibility = Visibility.Visible;
            }
            else if (this.viewModel.StateParam.Equals("SmDataEntryWithDetailsList"))
            {
                this.ButtonListStackPanelForSmDataEntryWithCmdBtnList.Visibility = Visibility.Collapsed;
                this.ButtonListStackPanelForSmAssistSelectDept.Visibility = Visibility.Collapsed;
                this.StoreModeBorder.Width = 532;
                this.ContainerStoreModeButtonsItemsControl.Width = 496;
                this.ContainerStoreModeButtonsItemsControl.Visibility = Visibility.Visible;
            }
            else if (this.viewModel.StateParam.Equals("SmCardManagement"))
            {
                this.StoreModeBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ButtonListStackPanelForSmDataEntryWithCmdBtnList.Visibility = Visibility.Collapsed;
                this.ButtonListStackPanelForSmAssistSelectDept.Visibility = Visibility.Collapsed;
                this.StoreModeBorder.Width = 644;
                this.ContainerStoreModeButtonsItemsControl.Width = 620;
                this.ContainerStoreModeButtonsItemsControl.Visibility = Visibility.Visible;
            }
        }

        private void UpdateSMButton1Text()
        {
            this.StoreButton1.Text = this.StoreButton1SmDataEntryWithCmdBtnList.Text = GetPropertyStringValue("SMButton1Text");
        }

        private void UpdateSMButton1Shown()
        {
            this.StoreButton1.Visibility = this.StoreButton1SmDataEntryWithCmdBtnList.Visibility = GetPropertyBoolValue("SMButton1Shown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateSMButton1Enabled()
        {
            this.StoreButton1.IsEnabled = this.StoreButton1SmDataEntryWithCmdBtnList.IsEnabled = GetPropertyBoolValue("SMButton1Enabled");
        }

        private void UpdateSMButton3Text()
        {
            this.StoreButton3.Text = GetPropertyStringValue("SMButton3Text");
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
            this.StoreButton4.Text = GetPropertyStringValue("SMButton4Text");
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
            this.StoreButton5.Text = GetPropertyStringValue("SMButton5Text");
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
            this.StoreButton6.Text = GetPropertyStringValue("SMButton6Text");
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

        private void UpdateCMButton1MedShown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCMButton1MedEnabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("CMButton1MedEnabled");
        }

        private void UpdateCMButton2MedShown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("CMButton2MedShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCMButton2MedEnabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("CMButton2MedEnabled");
        }

        private void UpdateCMButton1LgShown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("CMButton1LgShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCMButton1LgEnabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("CMButton1LgEnabled");
        }

        private void UpdateCMButton2Shown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("CancelAllShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCMButton2Enabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("CMButton2Enabled");
        }

        private void UpdateXMButton1Shown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("XMButton1Shown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateXMButton1Enabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("XMButton1Enabled");
        }

        private void UpdateXMButton2Shown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("XMButton2Shown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateXMButton2Enabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("XMButton2Enabled");
        }

        private void UpdateCMButton1MedText()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_CouponTooHigh":
                    this.StoreButton1.Property(ImageButton.TextProperty).SetResourceValue("OK");
                    break;
                default:
                    this.StoreButton1.Text = GetPropertyStringValue("CMButton1MedText");
                    break;
            }
        }

        private void UpdateCMButton2MedText()
        {
            this.StoreButton2.Text = GetPropertyStringValue("CMButton2Med");
        }

        private void UpdateCMButton1LgText()
        {
            this.StoreButton1.Text = GetPropertyStringValue("CMButton1LgText");
        }

        private void UpdateCMButton2Text()
        {
            this.StoreButton2.Text = GetPropertyStringValue("CMButton2Text");
        }

        private void UpdateXMButton1Text()
        {
            this.StoreButton1.Text = GetPropertyStringValue("XMButton1Text");
        }

        private void UpdateXMButton2Text()
        {
            this.StoreButton2.Text = GetPropertyStringValue("XMButton2Text");
        }
        private void UpdateCmdBtn_KeyInCodeText()
        {
            this.StoreButton1.Text = GetPropertyStringValue("CmdBtn_KeyInCodeText");
        }
        private void UpdateCmdBtn_KeyInCodeTextEnabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("CmdBtn_KeyInCodeEnabled");
        }
        private void UpdateCmdBtn_KeyInCodeTextShown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("CmdBtn_KeyInCodeShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_VoidItemText()
        {
            this.StoreButton2.Text = GetPropertyStringValue("CmdBtn_VoidItemText");
        }
        private void UpdateCmdBtn_VoidItemEnabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("CmdBtn_VoidItemEnabled");
        }
        private void UpdateCmdBtn_VoidItemShown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("CmdBtn_VoidItemShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_UpdateItemText()
        {
            this.StoreButton3.Text = GetPropertyStringValue("CmdBtn_UpdateItemText");
        }
        private void UpdateCmdBtn_UpdateItemEnabled()
        {
            this.StoreButton3.IsEnabled = GetPropertyBoolValue("CmdBtn_UpdateItemEnabled");
        }
        private void UpdateCmdBtn_UpdateItemShown()
        {
            this.StoreButton3.Visibility = GetPropertyBoolValue("CmdBtn_UpdateItemShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_SuspendText()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton4.Text = GetPropertyStringValue("CmdBtn_SuspendText");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton1.Text = GetPropertyStringValue("CmdBtn_SuspendText");
                    break;
            }
        }
        private void UpdateCmdBtn_SuspendEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton4.IsEnabled = GetPropertyBoolValue("CmdBtn_SuspendEnabled");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton1.IsEnabled = GetPropertyBoolValue("CmdBtn_SuspendEnabled");
                    break;
            }
        }
        private void UpdateCmdBtn_SuspendShown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton4.Visibility = GetPropertyBoolValue("CmdBtn_SuspendShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton1.Visibility = GetPropertyBoolValue("CmdBtn_SuspendShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }
        private void UpdateCmdBtn_PrintBarcodeText()
        {
            this.StoreButton5.Text = GetPropertyStringValue("CmdBtn_PrintBarcodeText");
        }
        private void UpdateCmdBtn_PrintBarcodeEnabled()
        {
            this.StoreButton5.IsEnabled = GetPropertyBoolValue("CmdBtn_PrintBarcodeEnabled");
        }
        private void UpdateCmdBtn_PrintBarcodeShown()
        {
            this.StoreButton5.Visibility = GetPropertyBoolValue("CmdBtn_PrintBarcodeShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_ReportingText()
        {
            this.StoreButton6.Text = GetPropertyStringValue("CmdBtn_ReportingText");
        }
        private void UpdateCmdBtn_ReportingEnabled()
        {
            this.StoreButton6.IsEnabled = GetPropertyBoolValue("CmdBtn_ReportingEnabled");
        }
        private void UpdateCmdBtn_ReportingShown()
        {
            this.StoreButton6.Visibility = GetPropertyBoolValue("CmdBtn_ReportingShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_FullAssistText()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton7.Text = GetPropertyStringValue("CmdBtn_FullAssistText");
                    break;
                case "SmAssistMenuFinalize":
                    this.StoreButton3.Text = GetPropertyStringValue("CmdBtn_FullAssistText");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton4.Text = GetPropertyStringValue("CmdBtn_FullAssistText");
                    break;
            }
        }
        private void UpdateCmdBtn_FullAssistEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton7.IsEnabled = GetPropertyBoolValue("CmdBtn_FullAssistEnabled");
                    break;
                case "SmAssistMenuFinalize":
                    this.StoreButton3.IsEnabled = GetPropertyBoolValue("CmdBtn_FullAssistEnabled");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton4.IsEnabled = GetPropertyBoolValue("CmdBtn_FullAssistEnabled");
                    break;
            }

        }
        private void UpdateCmdBtn_FullAssistShown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    this.StoreButton7.Visibility = GetPropertyBoolValue("CmdBtn_FullAssistShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAssistMenuFinalize":
                    this.StoreButton3.Visibility = GetPropertyBoolValue("CmdBtn_FullAssistShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.StoreButton7.Visibility = Visibility.Collapsed;
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton4.Visibility = GetPropertyBoolValue("CmdBtn_FullAssistShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.StoreButton7.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void UpdateCmdBtn_SystemFunctionsText()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton1.Text = GetPropertyStringValue("CmdBtn_SystemFunctionsText");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton2.Text = GetPropertyStringValue("CmdBtn_SystemFunctionsText");
                    break;
            }
        }
        private void UpdateCmdBtn_SystemFunctionsEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton1.IsEnabled = GetPropertyBoolValue("CmdBtn_SystemFunctionsEnabled");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton2.IsEnabled = GetPropertyBoolValue("CmdBtn_SystemFunctionsEnabled");
                    break;
            }
        }
        private void UpdateCmdBtn_SystemFunctionsShown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton1.Visibility = GetPropertyBoolValue("CmdBtn_SystemFunctionsShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton2.Visibility = GetPropertyBoolValue("CmdBtn_SystemFunctionsShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }
        private void UpdateCmdBtn_MediaStatusText()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton2.Text = GetPropertyStringValue("CmdBtn_MediaStatusText");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton3.Text = GetPropertyStringValue("CmdBtn_MediaStatusText");
                    break;
            }
        }
        private void UpdateCmdBtn_MediaStatusEnabled()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton2.IsEnabled = GetPropertyBoolValue("CmdBtn_MediaStatusEnabled");
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton3.IsEnabled = GetPropertyBoolValue("CmdBtn_MediaStatusEnabled");
                    break;
            }
        }
        private void UpdateCmdBtn_MediaStatusShown()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuFinalize":
                    this.StoreButton2.Visibility = GetPropertyBoolValue("CmdBtn_MediaStatusShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmAssistMenuFinalizeHC":
                    this.StoreButton3.Visibility = GetPropertyBoolValue("CmdBtn_MediaStatusShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }
        private void UpdateCmdBtn_ChangePriceText()
        {
            this.StoreButton1.Text = GetPropertyStringValue("CmdBtn_ChangePriceText");
        }
        private void UpdateCmdBtn_ChangePriceEnabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("CmdBtn_ChangePriceEnabled");
        }
        private void UpdateCmdBtn_ChangePriceShown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("CmdBtn_ChangePriceShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_ChangeQuantityText()
        {
            this.StoreButton2.Text = GetPropertyStringValue("CmdBtn_ChangeQuantityText");
        }
        private void UpdateCmdBtn_ChangeQuantityEnabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("CmdBtn_ChangeQuantityEnabled");
        }
        private void UpdateCmdBtn_ChangeQuantityShown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("CmdBtn_ChangeQuantityShown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_Button1Text()
        {
            this.StoreButton1.Text = GetPropertyStringValue("CmdBtn_Button1Text");
        }
        private void UpdateCmdBtn_Button1Enabled()
        {
            this.StoreButton1.IsEnabled = GetPropertyBoolValue("CmdBtn_Button1Enabled");
        }
        private void UpdateCmdBtn_Button1Shown()
        {
            this.StoreButton1.Visibility = GetPropertyBoolValue("CmdBtn_Button1Shown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_Button2Text()
        {
            this.StoreButton2.Text = GetPropertyStringValue("CmdBtn_Button2Text");
        }
        private void UpdateCmdBtn_Button2Enabled()
        {
            this.StoreButton2.IsEnabled = GetPropertyBoolValue("CmdBtn_Button2Enabled");
        }
        private void UpdateCmdBtn_Button2Shown()
        {
            this.StoreButton2.Visibility = GetPropertyBoolValue("CmdBtn_Button2Shown") ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateCmdBtn_Button3Text()
        {
            this.StoreButton3.Text = GetPropertyStringValue("CmdBtn_Button3Text");
        }
        private void UpdateCmdBtn_Button3Enabled()
        {
            this.StoreButton3.IsEnabled = GetPropertyBoolValue("CmdBtn_Button3Enabled");
        }
        private void UpdateCmdBtn_Button3Shown()
        {
            this.StoreButton3.Visibility = GetPropertyBoolValue("CmdBtn_Button3Shown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSmDataEntrySMButton2Shown()
        {
            StoreButton2.Visibility = GetPropertyBoolValue("SMButton2Shown") ? Visibility.Visible : Visibility.Collapsed;            
        }

        private void UpdateSmDataEntrySMButton2Text()
        {
            StoreButton2.Text = GetPropertyStringValue("SMButton2Text");
        }
        
        private void UpdateSMButton1()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_KeyInCodeTextShown();
                    UpdateCmdBtn_KeyInCodeTextEnabled();
                    UpdateCmdBtn_KeyInCodeText();
                    break;
                case "SmAssistMenuFinalizeHC":
                    UpdateCmdBtn_SuspendShown();
                    UpdateCmdBtn_SuspendEnabled();
                    UpdateCmdBtn_SuspendText();
                    break;
                case "SmAssistUpdateItemMenu":
                    UpdateCmdBtn_ChangePriceShown();
                    UpdateCmdBtn_ChangePriceEnabled();
                    UpdateCmdBtn_ChangePriceText();
                    break;
                case "SmAssistEnterItemMenu":
                case "SmAssistModifyItemMenu":
                case "SmAssistModifyTrxMenu":
                    UpdateCmdBtn_Button1Shown();
                    UpdateCmdBtn_Button1Enabled();
                    UpdateCmdBtn_Button1Text();
                    break;
                case "SmAssistMenuFinalize":
                    UpdateCmdBtn_SystemFunctionsShown();
                    UpdateCmdBtn_SystemFunctionsEnabled();
                    UpdateCmdBtn_SystemFunctionsText();
                    break;
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                case "AM_CouponNoMatch":
                case "AM_CouponNotAllowed":
                case "AM_CouponTooHigh":
                case "AM_CustomMessage":
                case "AM_ItemQuantityExceeded":
                case "AM_RestrictedNotAllowed":
                case "AM_UpdateNotAllowed":
                case "AM_VoidNoMatch":
                    UpdateCMButton1MedShown();
                    UpdateCMButton1MedEnabled();
                    UpdateCMButton1MedText();
                    break;
                case "AM_VoidItem":
                    UpdateCMButton1LgShown();
                    UpdateCMButton1LgEnabled();
                    UpdateCMButton1LgText();
                    break;
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    UpdateXMButton1Shown();
                    UpdateXMButton1Enabled();
                    UpdateXMButton1Text();
                    break;
                default:                    
                    UpdateSMButton1Shown();
                    UpdateSMButton1Enabled();
                    UpdateSMButton1Text();
                    break;
            }
        }

        private void UpdateSMButton2()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_VoidItemShown();
                    UpdateCmdBtn_VoidItemEnabled();
                    UpdateCmdBtn_VoidItemText();
                    break;
                case "SmAssistMenuFinalizeHC":
                    UpdateCmdBtn_SystemFunctionsShown();
                    UpdateCmdBtn_SystemFunctionsEnabled();
                    UpdateCmdBtn_SystemFunctionsText();
                    break;
                case "SmAssistUpdateItemMenu":
                    UpdateCmdBtn_ChangeQuantityShown();
                    UpdateCmdBtn_ChangeQuantityEnabled();
                    UpdateCmdBtn_ChangeQuantityText();
                    break;
                case "SmAssistEnterItemMenu":
                case "SmAssistModifyItemMenu":
                case "SmAssistModifyTrxMenu":
                    UpdateCmdBtn_Button2Shown();
                    UpdateCmdBtn_Button2Enabled();
                    UpdateCmdBtn_Button2Text();
                    break;
                case "SmAssistMenuFinalize":
                    UpdateCmdBtn_MediaStatusShown();
                    UpdateCmdBtn_MediaStatusEnabled();
                    UpdateCmdBtn_MediaStatusText();
                    break;
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                    UpdateCMButton2MedShown();
                    UpdateCMButton2MedEnabled();
                    UpdateCMButton2MedText();
                    break;
                case "AM_VoidItem":
                    UpdateCMButton2Shown();
                    UpdateCMButton2Enabled();
                    UpdateCMButton2Text();
                    break;
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    UpdateXMButton2Shown();
                    UpdateXMButton2Enabled();
                    UpdateXMButton2Text();
                    break;
                case "SmDataEntry":
                case "SmRestrictedItems":             
                case "SmSystemFunctions":
                case "SmSelectModeOfOperation":
                case "SmAbort":
                case "SmAborted":
                    UpdateSmDataEntrySMButton2Shown();
                    UpdateSmDataEntrySMButton2Text();
                    break;
                case "SmAssistMenu":
                case "SmVisualItems":
                case "SmCouponItems":
                case "SmUpdateItem":
                case "AM_Processing":
                case "SmDataEntrySmall":
                    StoreButton2.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateSMButton3()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_UpdateItemShown();
                    UpdateCmdBtn_UpdateItemEnabled();
                    UpdateCmdBtn_UpdateItemText();
                    break;
                case "SmAssistMenuFinalizeHC":
                    UpdateCmdBtn_MediaStatusShown();
                    UpdateCmdBtn_MediaStatusEnabled();
                    UpdateCmdBtn_MediaStatusText();
                    break;
                case "SmAssistEnterItemMenu":
                    UpdateCmdBtn_Button3Shown();
                    UpdateCmdBtn_Button3Enabled();
                    UpdateCmdBtn_Button3Text();
                    break;
                case "SmAssistMenuFinalize":
                    UpdateCmdBtn_FullAssistShown();
                    UpdateCmdBtn_FullAssistEnabled();
                    UpdateCmdBtn_FullAssistText();
                    break;
                default:
                    UpdateSMButton3Shown();
                    UpdateSMButton3Enabled();
                    UpdateSMButton3Text();
                    break;
            }
        }

        private void UpdateSMButton4()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_SuspendShown();
                    UpdateCmdBtn_SuspendEnabled();
                    UpdateCmdBtn_SuspendText();
                    break;
                case "SmAssistMenuFinalizeHC":
                    UpdateCmdBtn_FullAssistShown();
                    UpdateCmdBtn_FullAssistEnabled();
                    UpdateCmdBtn_FullAssistText();
                    break;
                default:
                    UpdateSMButton4Shown();
                    UpdateSMButton4Enabled();
                    UpdateSMButton4Text();
                    break;
            }
        }

        private void UpdateSMButton5()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_PrintBarcodeShown();
                    UpdateCmdBtn_PrintBarcodeEnabled();
                    UpdateCmdBtn_PrintBarcodeText();
                    break;
                default:
                    UpdateSMButton5Shown();
                    UpdateSMButton5Enabled();
                    UpdateSMButton5Text();
                    break;
            }
            
        }

        private void UpdateSMButton6()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    UpdateCmdBtn_ReportingShown();
                    UpdateCmdBtn_ReportingEnabled();
                    UpdateCmdBtn_ReportingText();
                    break;
                default:
                    UpdateSMButton6Shown();
                    UpdateSMButton6Enabled();
                    UpdateSMButton6Text();
                    break;
            }
        }

        private void UpdateSMButton7()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                case "SmAssistMenuFinalizeHC":
                    UpdateCmdBtn_FullAssistShown();
                    UpdateCmdBtn_FullAssistEnabled();
                    UpdateCmdBtn_FullAssistText();
                    break;
                default:
                    UpdateSMButton7Shown();
                    UpdateSMButton7Enabled();
                    UpdateSMButton7Text();
                    break;
            }
        }

        private void UpdateCMButton1Med()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_ConfirmAbort":
                case "AM_ConfirmQuantity":
                case "AM_ConfirmVoid":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                case "AM_CouponNoMatch":
                case "AM_CouponNotAllowed":
                case "AM_CouponTooHigh":
                case "AM_CustomMessage":
                case "AM_ItemQuantityExceeded":
                case "AM_RestrictedNotAllowed":
                case "AM_UpdateNotAllowed":
                case "AM_VoidNoMatch":
                    UpdateCMButton1MedShown();
                    UpdateCMButton1MedEnabled();
                    UpdateCMButton1MedText();
                    break;
            }
        }

        private void UpdateCMButton2Med()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_ConfirmAbort":
                case "AM_ConfirmQuantity":
                case "AM_ConfirmVoid":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                    UpdateCMButton2MedShown();
                    UpdateCMButton2MedEnabled();
                    UpdateCMButton2MedText();
                    break;
            }
        }

        private void UpdateCMButton1Lg()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_VoidItem":
                    UpdateCMButton1LgShown();
                    UpdateCMButton1LgEnabled();
                    UpdateCMButton1LgText();
                    break;
            }
        }

        private void UpdateCMButton2()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_VoidItem":
                    UpdateCMButton2Shown();
                    UpdateCMButton2Enabled();
                    UpdateCMButton2Text();
                    break;
            }
        }

        private void UpdateXMButton1()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    UpdateXMButton1Shown();
                    UpdateXMButton1Enabled();
                    UpdateXMButton1Text();
                    break;
            }
        }

        private void UpdateXMButton2()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMLoanFailure":
                case "XMPickupFailure":
                case "XMMediaStatus":
                    UpdateXMButton2Shown();
                    UpdateXMButton2Enabled();
                    UpdateXMButton2Text();
                    break;
            }
        }

        private void UpdateSMLineShown()
        {
            this.SMLineText.Visibility = !(this.SMLineText.Text.Equals(String.Empty)) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMLineText()
        {
            this.SMLineText.Text = UIFormat.MultiLineFormat(new string[] { GetLine1Text(), GetLine2Text(), GetLine3Text() });
        }

        private string GetLine1Text()
        {
            string line = string.Empty;
            switch (this.viewModel.StateParam)
            {
                case "SmAssistEnterItemMenu":
                case "SmAssistModifyItemMenu":
                case "SmAssistModifyTrxMenu":
                case "SmAssistSelectDept":
                    if ((this.GetPropertyBoolValue("LeadthruTextShown") && this.GetPropertyStringValue("LeadthruText").Length > 0))
                    {
                        line = GetPropertyStringValue("LeadthruText");
                    }
                    break;
                case "XMLoanFailure":
                case "XMPickupFailure":
                    if (GetPropertyStringValue("XMLeadthruText").ToString().Length > 0)
                    {
                        line = GetPropertyStringValue("XMLeadthruText").ToString();
                    }
                    break;
                default:
                    if ((this.GetPropertyBoolValue("SMLeadthruTextShown") && this.GetPropertyStringValue("SMLeadthruText").Length > 0))
                    {
                        line = GetPropertyStringValue("SMLeadthruText");
                    }
                    break;
            }
            return line;
        }

        private string GetLine2Text()
        {
            string line = string.Empty;
            if (this.GetPropertyBoolValue("LeadthruText2TextShown") && this.GetPropertyStringValue("LeadthruText2Text").Length > 0)
            {
                line = this.GetPropertyStringValue("LeadthruText2Text");
            }
            return line;
        }

        private string GetLine3Text()
        {
            string line = string.Empty;
            switch (this.viewModel.StateParam)
            {
                case "AM_ConfirmQuantity":
                case "AM_ContinueTrans":
                case "AM_CrateableItem":
                case "AM_CouponNoMatch":
                case "AM_CouponNotAllowed":
                case "AM_CouponTooHigh":
                case "AM_CustomMessage":
                case "AM_EnterWeight":
                case "AM_ItemQuantityExceeded":
                case "AM_Processing":
                case "AM_RestrictedNotAllowed":
                case "AM_ScaleBroken":
                case "AM_SelectTare":
                case "AM_UpdateNotAllowed":
                case "AM_VoidNoMatch":
                    line = GetPropertyStringValue("Instructions");                    
                    break;
                case "AM_VoidItem":
                    line = GetPropertyStringValue("MessageBoxNoEcho").ToString();
                    break;
            }
            return line;
        }


        private void StoreButton3_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute("CmdBtn_UpdateItem");
                    break;
                case "SmAssistMenuFinalize":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_FullAssist({0})", "OnAssistModeButton"));
                    break;
                case "SmAssistMenuFinalizeHC":
                    viewModel.ActionCommand.Execute("CmdBtn_MediaStatus");
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_Reporting");
                    break;
                case "SmAssistEnterItemMenu":
                    viewModel.ActionCommand.Execute("CmdBtn_Button3");
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton3");
                    break;
            }
        }

        private void StoreButton4_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_Suspend({0})", "OnSuspendButton"));
                    break;
                case "SmAssistMenuFinalizeHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_FullAssist({0})", "OnAssistModeButton"));
                    break;
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_SystemFunctions");
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton4");
                    break;
            }
        }

        private void StoreButton5_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute("CmdBtn_MediaStatus");
                    break;
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute("CmdBtn_PrintBarcode");
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton5");
                    break;
            }
        }

        private void StoreButton6_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAuthorizationHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_FullAssist({0})", String.Empty));
                    break;
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute("CmdBtn_Reporting");
                    break;
                default:                    
                    viewModel.ActionCommand.Execute("SMButton6");
                    break;
            }
        }

        private void StoreButton7_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmAssistMenuHC":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_FullAssist({0})", "OnAssistModeButton"));
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton7");
                    break;
            }
        }

        private void StoreButton1SmDataEntryWithCmdBtnList_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "SmDataEntryWithCmdBtnList":
                    viewModel.ActionCommand.Execute("SMButton1");
                    break;
            }
        }

        private void ContainerButtonListForSmDataEntryWithCmdBtnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = ContainerButtonListForSmDataEntryWithCmdBtnList.SelectedItem as GridItem;
            if (null != item)
            {
                if (item.IsEnabled)
                {
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "DataNeededButtonList({0})", item.Data));
                }
            }
        }

        private void ContainerButtonListForSmAssistSelectDept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = ContainerButtonListForSmAssistSelectDept.SelectedItem as GridItem;
            if (null != item)
            {
                if (item.IsEnabled)
                {
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "SMAssistSelectDeptButtonList({0})", item.Data));
                }
            }
        }
    }
}