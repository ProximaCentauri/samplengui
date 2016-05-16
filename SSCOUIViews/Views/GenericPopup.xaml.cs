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
using System.ComponentModel;
using System.Collections.Generic;
using SSCOControls;


namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for the generic popup
    /// </summary>
    public partial class GenericPopup : PopupView
    {
        public GenericPopup(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            UpdateStateParam(param);
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("Instructions"))
            {
                UpdateInstructions();
            }
            else if (name.Equals("LeadthruText"))
            {
                UpdateLeadthruText();
            }
            else if (name.Equals("ButtonStoreLogInShown") || name.Equals("CMButton1StoreLogInShown"))
            {
                UpdateStoreLogin();
            }
            else if (name.Equals("CMButton1MedShown") || name.Equals("CMButton1MedText"))
            {
                UpdateItemRemoved();
            }
            else if (name.Equals("MessageBoxNoEcho"))
            {
                UpdateMessageBox();
            }
            else if (name.Equals("CMButton1MidiList"))
            {
                UpdateMidiList1();
            }
            else if (name.Equals("CMButton2MidiList"))
            {
                UpdateMidiList2();
            }
            else if (name.Equals("CMButton3MidiList"))
            {
                UpdateMidiList3();
            }
            else if (name.Equals("CMButton4MidiList"))
            {
                UpdateMidiList4();
            }
            else if (name.Equals("CMButton5MidiList"))
            {
                UpdateMidiList5();
            }
            else if (name.Equals("CMButton6MidiList"))
            {
                UpdateMidiList6();
            }
            else if (name.Equals("CMButton7MidiList"))
            {
                UpdateMidiList7();
            }
            else if (name.Equals("NextGenData"))
            {
                UpdateNextGenData();
            }
            else if (name.Equals("EchoMessage"))
            {
                UpdateEchoMessage();
                UpdateSMButton8();
            }
            else if (name.Equals("RemoteButton1Shown"))
            {
                UpdateGenericOkButton();
            }
        }

        private void UpdateEchoMessage()
        {
            switch (this.viewModel.StateParam)
            {
                case "EchoPopup":
                    this.EchoPopupMessage.Text = GetPropertyStringValue("EchoMessage");
                    this.EchoPopupMessage.Visibility = Visibility.Visible;
                    this.LoadingAnimation.Style = this.FindResource("storeModeLoadingAnimationControlStyle") as Style;
                    break;
                default:
                    this.EchoPopupMessage.Visibility = Visibility.Collapsed;
                    this.LoadingAnimation.Style = this.FindResource("loadingAnimationControlStyle") as Style;
                    break;
            }
        }

        private void UpdateSMButton8()
        {
            if (this.viewModel.StateParam.Equals("EchoPopup") && this.viewModel.BackgroundStateParam.Equals("SmReportsMenu"))
            {
                this.ControlsToAllowEnabled = "StoreButton8";
            }
        }

        private void UpdateNextGenData()
        {
            switch (this.viewModel.StateParam)
            {
                case "Startup":
                case "OutOfService":
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue(this.viewModel.GetPropertyValue("NextGenData").ToString().Equals("Unloading") ?
                        "ShuttingDown" : "LoadingText");
                    this.Instructions.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void UpdateGenericOkButton()
        {
            switch (this.viewModel.StateParam)
            {
                case "CheckBasket":
                    this.GenericOkButton.Visibility = Visibility.Visible;
                    this.GenericOkButton.Property(ImageButton.TextProperty).SetResourceValue("OK");
                    this.GenericOkButton.CommandParameter = "OKCheckBasket";
                    break;
                case "WaitForRemoteAssistance":
                    if (GetPropertyBoolValue("RemoteButton1Shown"))
                    {
                        this.GenericOkButton.Visibility = Visibility.Visible;
                        this.GenericOkButton.Property(ImageButton.TextProperty).SetResourceValue("Done");
                        this.GenericOkButton.CommandParameter = "RemoteButton1";
                    }
                    break;
                default:
                    this.GenericOkButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateMidiList1()
        {
            switch (this.viewModel.StateParam)
            {
                case "DataNeededConfirm":
                case "DataNeededMsg":
                case "DataNeededScanCard":
                    this.Ok_Button.Text = GetPropertyStringValue("CMButton1MidiList");
                    break;
            }
            this.CMButton1MidiList.Visibility = GetPropertyBoolValue("CMButton1MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList2()
        {
            switch (this.viewModel.StateParam)
            {
                case "DataNeededConfirm":
                case "DataNeededMsg":
                case "DataNeededScanCard":
                    this.GenericButton.Text = GetPropertyStringValue("CMButton2MidiList");
                    break;
            }
            this.CMButton2MidiList.Visibility = GetPropertyBoolValue("CMButton2MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList3()
        {
            this.CMButton3MidiList.Visibility = GetPropertyBoolValue("CMButton3MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList4()
        {
            this.CMButton4MidiList.Visibility = GetPropertyBoolValue("CMButton4MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList5()
        {
            this.CMButton5MidiList.Visibility = GetPropertyBoolValue("CMButton5MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList6()
        {
            this.CMButton6MidiList.Visibility = GetPropertyBoolValue("CMButton6MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMidiList7()
        {
            this.CMButton7MidiList.Visibility = GetPropertyBoolValue("CMButton7MidiListShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateMessageBox()
        {
            switch (this.viewModel.StateParam)
            {
                case "DataNeededSelection":
                case "DataNeededConfirm":
                case "DataNeededMsg":
                case "DataNeededScanCard":
                    this.Instructions.Text = GetPropertyStringValue("MessageBoxNoEcho");
                    break;
            }
            UpdateEchoMessage();
        }

        private void UpdateItemRemoved()
        {
            switch (this.viewModel.StateParam)
            {
                case "PickingUpItems":
                case "SecUnexpectedDecrease":
                    this.ItemRemovedButton.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.ItemRemovedButton.CommandParameter = "OKUnknownItem";
                    this.ItemRemovedButton.Property(ImageButton.TextProperty).SetResourceValue("RemovedItem");
                    break;
                case "SecUnExpectedIncrease":
                case "SecMisMatchWeight":
                    this.ItemRemovedButton.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.ItemRemovedButton.CommandParameter = "OKUnknownItem";
                    this.ItemRemovedButton.Property(ImageButton.TextProperty).SetResourceValue("UseOwnBag");
                    break;
                case "EnterWeight":
                    this.ItemRemovedButton.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.ItemRemovedButton.CommandParameter = "Yes";
                    this.ItemRemovedButton.Text = GetPropertyStringValue("CMButton1MedText");
                    break;
                default:
                    this.ItemRemovedButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateStoreLogin()
        {
            if (this.viewModel.StateParam.Equals("Disconnected"))
            {
                StoreLogin.Visibility = Visibility.Collapsed;
            }
            else
            {
                StoreLogin.Visibility = GetPropertyBoolValue("ButtonStoreLogInShown") || GetPropertyBoolValue("CMButton1StoreLogInShown") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void UpdateInstructions()
        {
            switch (this.viewModel.StateParam)
            {
                case "SecNewItemEntry":
                case "CustomMessage":
                case "AssistedTender":
                case "CardMisRead":
                case "CouponNotAllowed":
                case "SavePreferences":
                case "CmDataEntry0":
                case "Processing":
                case "BiometricProcessing":
                case "ConfirmEBTAmount":
                case "CouponTooHigh":
                case "CrateableItem":
                case "VoidNoMatch":
                case "DropoffCoupons":
                case "LimitedEBTBenefitOK":
                case "LimitedEBTBenefit":
                case "OperatorPasswordStateInvalidPasswordWithFP":
                case "OperatorPasswordStateInvalidPassword":
                case "ConfirmDebitAmount":
                case "SecBagAreaBackup":
                case "SignCharge":
                case "TakeCard":
                case "CheckBasket":
                case "SystemMessageOpPass":
                case "TakeLoyaltyCard":
                case "WaitForRemoteAssistance":
                case "SecOperatorPWStateInvalidPassword":
                case "TenderBalance":
                case "ConfirmQuantity":
                case "SellBagsConfirmQuantity":
                case "RestrictedNotAllowed":
                case "CouponNoMatch":
                case "Continue":
                case "WaitApproval":
                case "CardProcessing":
                case "SecItemRemovedThreshold":
                case "VoidApproval":
                case "SecBagBoxThreshold":
                case "CollectGiftCard":
                case "FatalError":
                case "Suspend":
                case "UnDeActivatedItemApproval":
                case "SecSkipBaggingThreshold":
                case "RAPDataNeeded":
                case "UnknownItem":
                case "AM_ConfirmAbort":
                case "AM_ConfirmVoid":                    
                    this.Instructions.Text = GetPropertyStringValue("Instructions");
                    this.Instructions.Visibility = Visibility.Visible;
                    break;
                case "SmAbort":
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("ConfirmAbortTitle");
                    this.Instructions.Visibility = Visibility.Visible;
                    break;
                case "SecurityAnalysis":
                    this.Instructions.Text = GetPropertyStringValue("Instructions").Replace(".  ", ".\n");
                    this.Instructions.Visibility = Visibility.Visible;
                    break;
                default:
                    this.Instructions.Property(TextBlock.TextProperty).Clear();
                    break;
            }
        }

        private void UpdateLeadthruText()
        {            
            switch (this.viewModel.StateParam)
            {
                case "CustomMessage":
                case "AssistedTender":
                case "CardMisRead":
                case "CouponNotAllowed":
                case "SavePreferences":
                case "CmDataEntry0":
                case "Processing":
                case "BiometricProcessing":
                case "DataNeededConfirm":
                case "DataNeededMsg":
                case "DataNeededScanCard":
                case "InvalidBarcodeScan":
                case "DataNeededSelection":
                case "UnknownItem":
                case "CardProcessing":
                case "OperatorPasswordStateInvalidPassword":
                case "SecurityAnalysis":
                    this.PopupTitle.Text = GetPropertyStringValue("LeadthruText");
                    this.PopupTitle.Visibility = Visibility.Visible;
                    break;
                case "AM_ConfirmAbort":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ConfirmAbortTitle");
                    this.PopupTitle.Visibility = Visibility.Visible;
                    break;
                case "SmAbort":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("TransactionCancelTitle");
                    this.PopupTitle.Visibility = Visibility.Visible;                    
                    break;
                case "AM_ConfirmVoid":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ConfirmVoidTitle");                    
                    this.PopupTitle.Visibility = Visibility.Visible;                    
                    break;
                default:
                    this.PopupTitle.Property(TextBlock.TextProperty).Clear();
                    break;
            }
        }

        private void UpdateStateParam(string param)
        {
            this.ShowBackground = true;
            this.Height = Double.NaN;
            this.Width = Double.NaN;
            this.ControlsToAllowEnabled = "AssistanceButton,StoreButton8";
            UpdateInstructions();
            UpdateNextGenData();
            UpdateLeadthruText();
            UpdateStoreLogin();
            UpdateItemRemoved();
            UpdateMessageBox();
            UpdateMidiList1();
            UpdateMidiList2();
            UpdateMidiList3();
            UpdateMidiList4();
            UpdateMidiList5();
            UpdateMidiList6();
            UpdateMidiList7();
            UpdateScanAnimationStyle();
            UpdateStoreModeButtonStyle();
            UpdateYesNoButtonCommandParameter();
            UpdateElements();
            UpdateEchoMessage();
            UpdateYesNoText();
            UpdateGenericOkButton();
            PopupAlignment alignment = PopupAlignment.Center;
            this.XMLockScreenPanel.Visibility = Visibility.Collapsed;
            switch (param)
            {
                case "CollectGiftCard":
                case "FatalError":
                case "Suspend":
                case "VoidApproval":
                case "UnDeActivatedItemApproval":
                case "SecSkipBaggingThreshold":
                case "SecItemRemovedThreshold":
                case "SecBagBoxThreshold":
                case "RAPDataNeeded":
                case "SecNewItemEntry":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("AssistanceNeededTitle");
                    break;
                case "ConfirmEBTAmount":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ConfirmEBTAmountTitle");
                    break;
                case "CouponTooHigh":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("CouponTooHighTitle");
                    break;
                case "CrateableItem":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("PurchaseCrateTitle");
                    break;
                case "VoidNoMatch":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("VoidNoMatchTitle");
                    break;
                case "DropoffCoupons":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("DropoffCouponsTitle");
                    break;
                case "LimitedEBTBenefitOK":
                case "LimitedEBTBenefit":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("InsufficientBalanceTitle");
                    break;
                case "OperatorPasswordStateInvalidPasswordWithFP":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("OperatorPasswordStateInvalidPasswordWithFPTitle");
                    break;
                case "ConfirmDebitAmount":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ConfirmDebitAmountTitle");
                    break;
                case "SecBagAreaBackup":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("SecBagAreaBackupTitle");
                    break;
                case "SignCharge":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("SignChargeTitle");
                    break;
                case "TakeCard":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("TakeCardTitle");
                    break;
                case "CheckBasket":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("CheckBasketTitle");
                    break;
                case "SystemMessageOpPass":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ApprovalNeededTitle");
                    break;
                case "TakeLoyaltyCard":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("TakeLoyaltyCardTitle");
                    break;
                case "WaitForRemoteAssistance":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("WaitForRemoteAssistanceTitle");
                    break;
                case "ItemNotForSale":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("RecalledItemTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("UnknownItemMessage");
                    break;
                case "UnknownPrice":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("UnknownPriceTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("UnknownPriceMessage");
                    break;
                case "SetAllItemsAside":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("SetAllItemsAsideTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("SetAllItemsAsideMessage");
                    break;
                case "SecOperatorPWStateInvalidPassword":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("IncorrectLoginTitle");
                    break;
                case "TimeRestrictedItem":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("TimeRestrictedItem");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("TimeRestrictedItemMessage");
                    break;
                case "TenderBalance":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("GiftCard");
                    break;
                case "ItemQuantityExceeded":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_496");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_495");
                    break;
                case "SecMisMatchWeight":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("WeightMismatchTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("WeightMismatch");
                    break;
                case "SecBagBoxNotValid":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("VerifyBags");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("VerifyBagsMessage");
                    break;
                case "VoidItem":
                case "VoidItemWithReward":
                case "AM_VoidItem":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("CancelItems");
                    this.ControlsToAllowEnabled = "AssistanceButton,cartControlBox,GoBackBtn,CancelAllBtn,CartReceipt,StoreButton8";
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("VoidItem");
                    this.Instructions.MaxWidth = 475;
                    alignment = PopupAlignment.Left;
                    this.LeftPadding = 450;
                    this.Height = Double.NaN;
                    this.Width = Double.NaN;
                    break;
                case "ConfirmQuantity":
                case "SellBagsConfirmQuantity":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ConfirmQuantityTitle");
                    break;
                case "RestrictedNotAllowed":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("RestrictedItemTitle");
                    break;
                case "CouponNoMatch":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("CouponNoMatchTitle");
                    break;
                case "ScaleBroken":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ScaleBrokenTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("ScaleBroken");
                    break;
                case "Continue":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("ContinueTitle");
                    this.ControlsToAllowEnabled = "AssistanceButton";
                    break;
                case "WaitApproval":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("AssistanceNeededTitle");
                    this.Height = Double.NaN;
                    this.Width = 400;
                    break;
                case "InvalidBarcodeScan":
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("InvalidBarcodeScan");
                    break;
                case "SecUnExpectedIncrease":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("SecUnExpectedIncreaseTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("UnexpectedIncreaseMessage");
                    break;
                case "PickingUpItems":
                case "SecUnexpectedDecrease":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("RemovedItemTitle");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("UnexpectedDecreaseMessage");
                    break;
                case "EnterWeight":
                case "EnterWtForPriceEmbedded":
                    this.PopupTitle.Property(TextBlock.TextProperty).SetResourceValue("EnterWeightTitle");
                    if (itemImage.Source != null)
                    {
                        EnterWeightImage.Visibility = Visibility.Visible;
                        CheckItemDescriptionExists();
                    }
                    else
                    {
                        EnterWeightImage.Visibility = Visibility.Collapsed;
                    }
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("EnterWeight");
                    break;
                case "Closed":
                    this.PopupTitle.Text = String.Empty;
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("LaneClosedMessage");
                    this.Height = 520;
                    this.Width = 400;
                    this.ShowBackground = false;
                    break;
                case "Startup":
                case "OutOfService":
                    this.PopupTitle.Text = String.Empty;
                    this.Height = 520;
                    this.Width = 400;
                    this.ShowBackground = false;
                    break;
                case "Offline":
                    this.PopupTitle.Text = String.Empty;
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("HostOfflineMessage");
                    break;
                case "Disconnected":
                    this.PopupTitle.Text = String.Empty;
                    this.Instructions.Text = String.Empty;
                    this.Height = 520;
                    this.Width = 400;
                    this.ShowBackground = false;
                    break;
                case "EchoPopup":
                    this.PopupTitle.Text = String.Empty;
                    this.Instructions.Text = String.Empty;
                    this.Height = Double.NaN;
                    this.Width = 400;
                    UpdateSMButton8();
                    break;
                case "XMLockScreen":
                    this.PopupTitle.Text = String.Empty;
                    this.Instructions.Text = String.Empty;
                    this.XMLockScreenText.Property(TextBlock.TextProperty).SetResourceValue("XM_SystemBusy");
                    this.Width = 500;
                    this.Height = 200;
                    this.ShowBackground = false;
                    this.XMLockScreenPanel.Visibility = Visibility.Visible;
                    break;                
            }
            this.PopupTitle.Visibility = this.PopupTitle.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
            this.Instructions.Visibility = this.Instructions.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
            this.Alignment = alignment;
        }

        private void OkButton_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "UnknownItem":
                case "VoidNoMatch":
                case "TenderBalance":
                case "SetAllItemsAside":
                case "InvalidBarcodeScan":
                case "UnknownPrice":
                case "TimeRestrictedItem":
                case "ItemQuantityExceeded":
                case "OperatorPasswordStateInvalidPassword":
                case "CouponTooHigh":
                case "LimitedEBTBenefitOK":
                case "CouponNotAllowed":
                case "CouponNoMatch":
                case "DropoffCoupons":
                case "OperatorPasswordStateInvalidPasswordWithFP":
                case "Suspend":
                case "ConfirmDebitAmount":
                case "ConfirmEBTAmount":
                case "CardMisRead":
                case "RestrictedNotAllowed":
                case "CustomMessage":
                case "ItemNotForSale":
                    viewModel.ActionCommand.Execute("OKUnknownItem");
                    break;
                case "DataNeededConfirm":
                case "DataNeededMsg":
                case "DataNeededScanCard":
                    viewModel.ActionCommand.Execute("DataNeededButton1");
                    break;
                default:
                    break;
            }
        }

        private void StoreLogin_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "WaitApproval":
                case "SignCharge":
                case "Closed":
                case "SystemMessageOpPass":
                    viewModel.ActionCommand.Execute("WaitApprovalStoreLogin");
                    break;
                case "VoidApproval":
                case "SecBagBoxNotValid":
                case "SecUnexpectedDecrease":
                case "SecNewItemEntry":
                case "CollectGiftCard":
                case "FatalError":
                case "RAPDataNeeded":
                case "SecBagBoxThreshold":
                case "SecSkipBaggingThreshold":
                case "UnDeActivatedItemApproval":
                case "SecUnExpectedIncrease":
                case "SecItemRemovedThreshold":
                case "SecMisMatchWeight":
                case "PickingUpItems":
                    viewModel.ActionCommand.Execute("ButtonStoreLogIn");
                    break;
                default:
                    break;
            }
        }

        private void CheckItemDescriptionExists()
        {
            ItemImageUPC.Visibility = String.IsNullOrEmpty(ItemImageName.Text) ? Visibility.Visible : Visibility.Collapsed;
            ItemImageName.Visibility = String.IsNullOrEmpty(ItemImageName.Text) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateScanAnimationStyle()
        {
            VoidItemAnimation.Visibility = Visibility.Collapsed;
            switch (viewModel.StateParam)
            { 
                case "VoidItem":
                case "VoidItemWithReward":
                    VoidItemAnimation.Style = this.FindResource("can-scanAnimationControlStyle") as Style;
                    VoidItemAnimation.Visibility = Visibility.Visible;
                    break;
                case "AM_VoidItem":
                    VoidItemAnimation.Style = this.FindResource("can-StoreModeScanAnimationControlStyle") as Style;
                    VoidItemAnimation.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void UpdateStoreModeButtonStyle()
        {
            No_Button.Style = Yes_Button.Style = viewModel.StateParam.Equals("SmAbort") ||
            viewModel.StateParam.Equals("AM_ConfirmAbort") || viewModel.StateParam.Equals("AM_ConfirmVoid") ? 
            this.FindResource("storeModeMainButtonStyle") as Style :  this.FindResource("mainButtonStyle") as Style;
        }

        private void UpdateYesNoButtonCommandParameter()
        {
            switch (viewModel.StateParam)
            {
                case "SmAbort":
                    this.Yes_Button.CommandParameter = "SmAbortYesButton";
                    this.No_Button.CommandParameter = "SmAbortNoButton";
                    break;
                default:
                    this.Yes_Button.CommandParameter = "Yes";
                    this.No_Button.CommandParameter = "No";
                    break;
            }
        }

        private void UpdateYesNoText()
        {
            switch (viewModel.StateParam)
            {
                case "AM_ConfirmQuantity":
                case "AM_ConfirmVoid":
                case "AM_CreateableItem":
                case "AM_ConfirmAbort":
                case "AM_ContinueTrans":                    
                    Yes_Button.Property(ImageButton.TextProperty).SetResourceValue("YesButtonText");
                    No_Button.Property(ImageButton.TextProperty).SetResourceValue("NoButtonText");
                    break;
                default:
                    Yes_Button.Property(ImageButton.TextProperty).SetResourceValue("Yes");
                    No_Button.Property(ImageButton.TextProperty).SetResourceValue("No");
                    break;
            }            
        }

        private void UpdateElements()
        {
            this.Ok_Button.Visibility = this.viewModel.StateParam.Equals("InvalidBarcodeScan") || this.viewModel.StateParam.Equals("UnknownPrice") ||
                this.viewModel.StateParam.Equals("SetAllItemsAside") || this.viewModel.StateParam.Equals("OperatorPasswordStateInvalidPassword") || this.viewModel.StateParam.Equals("UnknownItem") ||
                this.viewModel.StateParam.Equals("ItemNotForSale") || this.viewModel.StateParam.Equals("TimeRestrictedItem") || this.viewModel.StateParam.Equals("DataNeededMsg") ||
                this.viewModel.StateParam.Equals("DataNeededConfirm") || this.viewModel.StateParam.Equals("DataNeededScanCard") || this.viewModel.StateParam.Equals("ItemQuantityExceeded") ||
                this.viewModel.StateParam.Equals("TenderBalance") || this.viewModel.StateParam.Equals("VoidNoMatch") || this.viewModel.StateParam.Equals("CouponTooHigh") ||
                this.viewModel.StateParam.Equals("RestrictedNotAllowed") || this.viewModel.StateParam.Equals("LimitedEBTBenefitOK") || this.viewModel.StateParam.Equals("CustomMessage") ||
                this.viewModel.StateParam.Equals("CouponNotAllowed") || this.viewModel.StateParam.Equals("CouponNoMatch") || this.viewModel.StateParam.Equals("DropoffCoupons") ||
                this.viewModel.StateParam.Equals("OperatorPasswordStateInvalidPasswordWithFP") || this.viewModel.StateParam.Equals("Suspend") || this.viewModel.StateParam.Equals("ConfirmDebitAmount") ||
                this.viewModel.StateParam.Equals("CardMisRead") || this.viewModel.StateParam.Equals("LimitedEBTBenefitOK") || this.viewModel.StateParam.Equals("ConfirmEBTAmount") ?
                Visibility.Visible : Visibility.Collapsed;
            this.YesNoStackPanel.Visibility = paramListForYesNoButtonToShow.Contains(viewModel.StateParam) ? Visibility.Visible : Visibility.Collapsed;
            this.CancelButton.Visibility = this.viewModel.StateParam.Equals("EnterWtForPriceEmbedded") ?
                Visibility.Visible : Visibility.Collapsed;
            this.CancelAcceptWeight.Visibility = this.viewModel.StateParam.Equals("EnterWeight") ? Visibility.Visible : Visibility.Collapsed;
            this.GoBackButton.Visibility = this.viewModel.StateParam.Equals("CheckBasket") || this.viewModel.StateParam.Equals("SecOperatorPWStateInvalidPassword") ||
                this.viewModel.StateParam.Equals("AssistedTender") || this.viewModel.StateParam.Equals("ScaleBroken") ? Visibility.Visible : Visibility.Collapsed;
            this.ScaleBrokenControl.Visibility = this.viewModel.StateParam.Equals("ScaleBroken") ? Visibility.Visible : Visibility.Collapsed;
            this.DataNeededScanCardControl.Visibility = this.viewModel.StateParam.Equals("DataNeededScanCard") ? Visibility.Visible : Visibility.Collapsed;
            this.ConfirmEBTDebitAmountButton.Visibility = this.viewModel.StateParam.Equals("ConfirmDebitAmount") || this.viewModel.StateParam.Equals("ConfirmEBTAmount") ? Visibility.Visible : Visibility.Collapsed;
            this.MessageEcho.Visibility = this.viewModel.StateParam.Equals("Startup") || this.viewModel.StateParam.Equals("OutOfService") ? Visibility.Visible : Visibility.Collapsed;

            this.UnexpectedDecreaseControl.Visibility = viewModel.StateParam.Equals("SecUnexpectedDecrease") || viewModel.StateParam.Equals("PickingUpItems") ? Visibility.Visible : Visibility.Collapsed;
            this.UnexpectedIncreaseControl.Visibility = viewModel.StateParam.Equals("SecUnExpectedIncrease") || viewModel.StateParam.Equals("SecBagAreaBackup") ? Visibility.Visible : Visibility.Collapsed;
            this.WeightExpectedLabel.Visibility = this.viewModel.StateParam.Equals("SecBagBoxNotValid") || this.viewModel.StateParam.Equals("SecMisMatchWeight") ? Visibility.Visible : Visibility.Collapsed;
            this.NumberofItemsLabel.Visibility = this.WeightObservedLabel.Visibility = this.viewModel.StateParam.Equals("PickingUpItems") || this.viewModel.StateParam.Equals("SecBagBoxNotValid") ||
                this.viewModel.StateParam.Equals("SecMisMatchWeight") || this.viewModel.StateParam.Equals("SecUnexpectedDecrease") || this.viewModel.StateParam.Equals("SecUnExpectedIncrease") ? Visibility.Visible : Visibility.Collapsed;
            this.GenericButton.Visibility = this.viewModel.StateParam.Equals("DataNeededConfirm") ? Visibility.Visible : Visibility.Collapsed;
            this.DisconnectedText.Visibility = this.LoadingText.Visibility = this.viewModel.StateParam.Equals("Disconnected") ? Visibility.Visible : Visibility.Collapsed;
            this.DataNeededSelectionStackPanel.Visibility = this.viewModel.StateParam.Equals("DataNeededSelection") ? Visibility.Visible : Visibility.Collapsed;

            this.LaneImage.Visibility = this.viewModel.StateParam.Equals("Closed") || this.viewModel.StateParam.Equals("OutOfService") || this.viewModel.StateParam.Equals("Offline") ||
                this.viewModel.StateParam.Equals("Startup") || this.viewModel.StateParam.Equals("Disconnected") ? Visibility.Visible : Visibility.Collapsed;
            this.CardAnimation.Visibility = this.viewModel.StateParam.Equals("SecurityAnalysis") || this.viewModel.StateParam.Equals("CardProcessing") ? Visibility.Visible : Visibility.Collapsed;
            this.LoadingAnimation.Visibility = this.viewModel.StateParam.Equals("Processing") || this.viewModel.StateParam.Equals("RAPDataNeeded") ||
                this.viewModel.StateParam.Equals("SecBagBoxThreshold") || this.viewModel.StateParam.Equals("SecItemRemovedThreshold") ||
                this.viewModel.StateParam.Equals("SecBagBoxNotValid") || this.viewModel.StateParam.Equals("BiometricProcessing") ||
                this.viewModel.StateParam.Equals("WaitForRemoteAssistance") || this.viewModel.StateParam.Equals("VoidApproval") || this.viewModel.StateParam.Equals("SecMisMatchWeight") ||
                this.viewModel.StateParam.Equals("SecSkipBaggingThreshold") || this.viewModel.StateParam.Equals("EchoPopup") ? Visibility.Visible : Visibility.Collapsed;
            this.VoidItemAnimation.Visibility = this.viewModel.StateParam.Equals("VoidItem") || this.viewModel.StateParam.Equals("VoidItemWithReward") || this.viewModel.StateParam.Equals("AM_VoidItem") ? Visibility.Visible : Visibility.Collapsed;
            this.WeightAnimation.Visibility = this.viewModel.StateParam.Equals("EnterWeight") || this.viewModel.StateParam.Equals("EnterWtForPriceEmbedded") ?
                Visibility.Visible : Visibility.Collapsed;
            if (!this.viewModel.StateParam.Equals("EnterWeight") && !this.viewModel.StateParam.Equals("EnterWtForPriceEmbedded"))
            {
                this.EnterWeightImage.Visibility = Visibility.Collapsed;
            }
        }

        private static List<string> paramListForYesNoButtonToShow = new List<string>() { "ConfirmQuantity", "Continue", "CrateableItem", "SellBagsConfirmQuantity", "SavePreferences", "LimitedEBTBenefit", "AM_ConfirmAbort", "AM_ConfirmVoid", "SmAbort"};
    }
}
