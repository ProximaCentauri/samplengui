using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SSCOUIModels.Controls;
using FPsxWPF.Controls;
using SSCOUIModels;
using SSCOControls;
using RPSWNET;
using System.Text.RegularExpressions;
using SSCOUIModels.Models;
using SSCOUIModels.Helpers;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for CashManagement.xaml
    /// </summary>
    public partial class CashManagement : BackgroundView
    {
        public CashManagement(IMainViewModel viewModel)
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
            UpdateTitleText();
            UpdateLeadthruText();
            UpdateLeadthruTitleText();
            UpdateTotalChangedText();
            UpdatePurgeArea();
            UpdateTotalDescriptionArea();
            UpdateControlVisibility();
            UpdateAlertBox();
            UpdateActionButtons();
            UpdateChangeConfirmationButtons();
            UpdateMenuButtons();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "XMLeadthruText":
                    UpdateLeadthruText();
                    break;
                case "XMLeadthruTextTitle":
                    UpdateLeadthruTitleText();
                    break;
                case "XMTotalAddedText":
                case "XMTotalRemovedText":
                    UpdateTotalChangedText();
                    break;
                case "XMTotalCoinCountTextShown":
                case "XMTotalNoteCountTextShown":
                case "XMPendingCoinCountTextShown":
                case "XMPendingNoteCountTextShown":
                    UpdateTotalDescriptionArea();
                    break;
                case "XMErrorBoxShown":
                case "XMErrorBoxText":
                case "XMErrorAlertBoxShown":
                case "XMErrorAlertBoxText":
                case "XMAlertBoxShown":
                case "XMAlertBoxText":
                case "XMAlertNegativeLoaderShown":
                case "XMAlertNegativeLoaderText":
                case "XMManualCashRemovalAlertShown":
                case "XMManualCashRemovalAlertText":
                    UpdateAlertBox();
                    break;
                case "XMButton2Enabled":
                case "XMButton2Shown":
                case "XMButton2Text":
                case "XMButton5Enabled":
                case "XMButton5Shown":
                case "XMButton5Text":
                case "XMButton9Enabled":
                case "XMButton9Shown":
                case "XMButton9Text":
                    UpdateActionButton1();
                    break;
                case "XMButton4Enabled":
                case "XMButton4Shown":
                case "XMButton4Text":
                case "XMButton6Enabled":
                case "XMButton6Shown":
                case "XMButton6Text":
                case "XMButton10Enabled":
                case "XMButton10Shown":
                case "XMButton10Text":
                case "XMNonDispenseCylinder501EmptyButtonEnabled":
                case "XMNonDispenseCylinder501EmptyButtonShown":
                case "XMNonDispenseCylinder501EmptyButtonText":
                    UpdateActionButton2();
                    break;
                case "XMButton7Enabled":
                case "XMButton7Shown":
                case "XMButton7Text":
                case "XMNonDispenseCylinder401EmptyButtonEnabled":
                case "XMNonDispenseCylinder401EmptyButtonShown":
                case "XMNonDispenseCylinder401EmptyButtonText":
                    UpdateActionButton3();
                    break;
                case "XMButton3Enabled":
                case "XMButton3Shown":
                case "XMButton3Text":
                    UpdateActionButton2();
                    UpdateActionButton4();
                    break;
                case "XMButton1Shown":
                case "XMButton1Enabled":
                    UpdateApplyChangesButton();
                    break;
                case "XMCancelChangesShown":
                case "XMCancelChangesEnabled":
                    UpdateCancelChangesButton();
                    break;
                case "XMMenuButton5001Toggled":
                    UpdateMenuButton1();
                    break;
                case "XMMenuButton5002Toggled":
                    UpdateMenuButton2();
                    break;
                case "XMMenuButton5003Toggled":
                    UpdateMenuButton3();
                    break;
                case "XMMenuButton5004Toggled":
                    UpdateMenuButton4();
                    break;
            }
        }

        private void UpdateTitleText()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashStatus":
                case "XMCashStatusGlory":
                    this.CashManagementTitle.Property(TextBlock.TextProperty).SetResourceValue("XM_CashStatus");
                    break;
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                    this.CashManagementTitle.Property(TextBlock.TextProperty).SetResourceValue("XM_CashRemove");
                    break;
                case "XMCashReplenish":
                    this.CashManagementTitle.Property(TextBlock.TextProperty).SetResourceValue("XM_CashReplenishment");
                    break;
            }
        }

        private void UpdateLeadthruText()
        {
            this.CashManagementLeadthru.Text = GetPropertyStringValue("XMLeadthruText");
        }

        private void UpdateLeadthruTitleText()
        {
            this.CashManagementLeadthruTitle.Text = GetPropertyStringValue("XMLeadthruTextTitle");
        }

        private void UpdateTotalChangedText()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemoveGlory":
                    this.CashManagementTotalChanged.Text = GetPropertyStringValue("XMTotalRemovedText");
                    break;
                case "XMCashStatusGlory":
                    this.CashManagementTotalChanged.Text = GetPropertyStringValue("XMTotalAddedText");
                    break;
            }
        }

        private void UpdateTotalDescriptionArea()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                    this.TotalCoinArea.Visibility = this.TotalNoteArea.Visibility = Visibility.Collapsed;
                    this.TotalCoins.Visibility = this.TotalNotes.Visibility = Visibility.Collapsed;
                    this.PendingCoins.Visibility = this.PendingNotes.Visibility = Visibility.Visible;
                    this.PendingCoinValue.Visibility = this.PendingCoinCount.Visibility = GetPropertyBoolValue("XMPendingCoinCountTextShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.PendingNoteValue.Visibility = this.PendingNoteCount.Visibility = GetPropertyBoolValue("XMPendingNoteCountTextShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "XMCashReplenish":
                case "XMCashStatus":
                case "XMCashStatusGlory":
                    this.TotalCoinArea.Property(TextBlock.TextProperty).SetResourceValue("XM_TotalCoinsInserted");
                    this.TotalNoteArea.Property(TextBlock.TextProperty).SetResourceValue("XM_TotalNotesInserted");
                    this.TotalCoinArea.Visibility = this.TotalNoteArea.Visibility = Visibility.Visible;
                    this.TotalCoins.Visibility = this.TotalNotes.Visibility = Visibility.Visible;
                    this.PendingCoins.Visibility = this.PendingNotes.Visibility = Visibility.Collapsed;

                    // Mask obscures controls in legacy Cash Management UI.
                    this.TotalCoinPanel.Visibility = GetPropertyBoolValue("XMTotalCoinAreaShown") ? Visibility.Collapsed : Visibility.Visible;
                    this.TotalNotePanel.Visibility = GetPropertyBoolValue("XMTotalNoteAreaShown") ? Visibility.Collapsed : Visibility.Visible;
                    this.TotalCoinValue.Visibility = this.TotalCoinCount.Visibility = GetPropertyBoolValue("XMTotalCoinCountTextShown") ? Visibility.Visible : Visibility.Collapsed;
                    this.TotalNoteValue.Visibility = this.TotalNoteCount.Visibility = GetPropertyBoolValue("XMTotalNoteCountTextShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }
        }

        private void UpdatePurgeArea()
        {
            this.PurgeCount.Visibility = GetPropertyBoolValue("XMPurgeOperationCountShown") ? Visibility.Visible : Visibility.Collapsed;
            this.YieldIcon.Visibility = GetPropertyBoolValue("XMPurgeOperationAlertShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateControlVisibility()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashStatus":
                    this.CashManagementGrandTotal.Visibility = this.CashManagementTotalChanged.Visibility = Visibility.Collapsed;
                    this.LockScreenButton.Visibility = Visibility.Collapsed;
                    this.ActionButtonPanel.Visibility = this.MenuButtonPanel.Visibility = Visibility.Collapsed;
                    this.CashStatusButtonPanel.Visibility = Visibility.Visible;
                    this.ConfirmChangesPanel.Visibility = Visibility.Collapsed;
                    this.CashManagementLeadthruTitle.Visibility = Visibility.Collapsed;
                    break;
                case "XMCashStatusGlory":
                    this.CashManagementGrandTotal.Visibility = this.CashManagementTotalChanged.Visibility = Visibility.Visible;
                    this.AddCashButton.Visibility = Visibility.Hidden;
                    this.NonDispenseCylinder401.Visibility = Visibility.Collapsed;
                    this.PurgeInstructionHolder.Visibility = this.PurgeCountHolder.Visibility = Visibility.Collapsed;
                    this.LockScreenButton.Visibility = Visibility.Collapsed;
                    this.ActionButtonPanel.Visibility =  this.MenuButtonPanel.Visibility = Visibility.Collapsed;
                    this.CashStatusButtonPanel.Visibility = Visibility.Visible;
                    this.ConfirmChangesPanel.Visibility = Visibility.Collapsed;
                    this.CashManagementLeadthruTitle.Visibility = Visibility.Collapsed;
                    break;
                case "XMCashReplenish":
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                    this.CashManagementGrandTotal.Visibility = this.CashManagementTotalChanged.Visibility = Visibility.Collapsed;
                    this.LockScreenButton.Visibility = Visibility.Visible;
                    this.CashStatusButtonPanel.Visibility = Visibility.Collapsed;
                    this.MenuButtonPanel.Visibility = this.ActionButtonPanel.Visibility = Visibility.Visible;
                    this.ConfirmChangesPanel.Visibility = Visibility.Visible;
                    this.CashManagementLeadthruTitle.Visibility = Visibility.Visible;
                    break;
                case "XMCashRemoveGlory":
                    this.CashManagementGrandTotal.Visibility = this.CashManagementTotalChanged.Visibility = Visibility.Visible;
                    this.NonDispenseCylinder401.Visibility = Visibility.Collapsed;
                    this.PurgeInstructionHolder.Visibility = this.PurgeCountHolder.Visibility = Visibility.Collapsed;
                    this.LockScreenButton.Visibility = Visibility.Visible;
                    this.CashStatusButtonPanel.Visibility = Visibility.Collapsed;
                    this.MenuButtonPanel.Visibility = this.ActionButtonPanel.Visibility = Visibility.Visible;
                    this.ConfirmChangesPanel.Visibility = Visibility.Visible;
                    this.CashManagementLeadthruTitle.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void UpdateAlertBox()
        {
            if(GetPropertyBoolValue("XMErrorBoxShown"))
            {
                this.AlertBoxText.Style = this.FindResource("cashManagementAlertBoxStyle") as Style;
                this.AlertBoxText.Text = GetPropertyStringValue("XMErrorBoxText");
                this.AlertBoxControl.Visibility = Visibility.Visible;
            }
            else if(GetPropertyBoolValue("XMErrorAlertBoxShown"))
            {
                this.AlertBoxText.Style = this.FindResource("cashManagementAlertBoxStyle") as Style;
                this.AlertBoxText.Text = GetPropertyStringValue("XMErrorAlertBoxText");
                this.AlertBoxControl.Visibility = Visibility.Visible;
            }
            else if(GetPropertyBoolValue("XMAlertBoxShown"))
            {
                this.AlertBoxText.Style = this.FindResource("cashManagementAlertBoxStyle") as Style;
                this.AlertBoxText.Text = GetPropertyStringValue("XMAlertBoxText");
                this.AlertBoxControl.Visibility = Visibility.Visible;
            }
            else if (GetPropertyBoolValue("XMAlertNegativeLoaderShown"))
            {
                this.AlertBoxText.Style = this.FindResource("cashManagementAlertBoxLongStyle") as Style;
                this.AlertBoxText.Text = GetPropertyStringValue("XMAlertNegativeLoaderText");
                this.AlertBoxControl.Visibility = Visibility.Visible;
            }
            else if (GetPropertyBoolValue("XMManualCashRemovalAlertShown"))
            {
                this.AlertBoxText.Style = this.FindResource("cashManagementAlertBoxStyle") as Style;
                this.AlertBoxText.Text = GetPropertyStringValue("XMManualCashRemovalAlertText");
                this.AlertBoxControl.Visibility = Visibility.Visible;
            }
            else
            {
                this.AlertBoxControl.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateActionButtons()
        {
            UpdateActionButton1();
            UpdateActionButton2();
            UpdateActionButton3();
            UpdateActionButton4();
        }

        private void UpdateActionButton1()
        {
            if (GetPropertyBoolValue("XMButton2Shown"))
            {
                this.ActionButton1.Text = GetPropertyStringValue("XMButton2Text");
                this.ActionButton1.CommandParameter = "XMButton2";
                this.ActionButton1.IsEnabled = GetPropertyBoolValue("XMButton2Enabled");
                this.ActionButton1.Visibility = Visibility.Visible;
            }
            else if (GetPropertyBoolValue("XMButton5Shown"))
            {
                this.ActionButton1.Text = GetPropertyStringValue("XMButton5Text");
                this.ActionButton1.CommandParameter = "XMButton5";
                this.ActionButton1.IsEnabled = GetPropertyBoolValue("XMButton5Enabled");
                this.ActionButton1.Visibility = Visibility.Visible;
            }
            else if (GetPropertyBoolValue("XMButton9Shown"))
            {
                this.ActionButton1.Text = GetPropertyStringValue("XMButton9Text");
                this.ActionButton1.CommandParameter = "XMButton9";
                this.ActionButton1.IsEnabled = GetPropertyBoolValue("XMButton9Enabled");
                this.ActionButton1.Visibility = Visibility.Visible;
            }
            else
            {
                this.ActionButton1.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateActionButton2()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                    if (GetPropertyBoolValue("XMButton4Shown"))
                    {
                        this.ActionButton2.Text = GetPropertyStringValue("XMButton4Text");
                        this.ActionButton2.CommandParameter = "XMButton4";
                        this.ActionButton2.IsEnabled = GetPropertyBoolValue("XMButton4Enabled");
                        this.ActionButton2.Visibility = Visibility.Visible;
                    }
                    else if (GetPropertyBoolValue("XMButton6Shown"))
                    {
                        this.ActionButton2.Text = GetPropertyStringValue("XMButton6Text");
                        this.ActionButton2.CommandParameter = "XMButton6";
                        this.ActionButton2.IsEnabled = GetPropertyBoolValue("XMButton6Enabled");
                        this.ActionButton2.Visibility = Visibility.Visible;
                    }
                    else if (GetPropertyBoolValue("XMButton10Shown"))
                    {
                        this.ActionButton2.Text = GetPropertyStringValue("XMButton10Text");
                        this.ActionButton2.CommandParameter = "XMButton10";
                        this.ActionButton2.IsEnabled = GetPropertyBoolValue("XMButton10Enabled");
                        this.ActionButton2.Visibility = Visibility.Visible;
                    }
                    else if (GetPropertyBoolValue("XMNonDispenseCylinder501EmptyButtonShown"))
                    {
                        this.ActionButton2.Text = GetPropertyStringValue("XMNonDispenseCylinder501EmptyButtonText");
                        this.ActionButton2.CommandParameter = "XMNonDispenseCylinder501EmptyButton";
                        this.ActionButton2.IsEnabled = GetPropertyBoolValue("XMNonDispenseCylinder501EmptyButtonEnabled");
                        this.ActionButton2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.ActionButton2.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "XMCashReplenish":
                    if (GetPropertyBoolValue("XMButton3Shown"))
                    {
                        this.ActionButton2.Text = GetPropertyStringValue("XMButton3Text");
                        this.ActionButton2.CommandParameter = "XMButton3";
                        this.ActionButton2.IsEnabled = GetPropertyBoolValue("XMButton3Enabled");
                        this.ActionButton2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.ActionButton2.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void UpdateActionButton3()
        {
            if (GetPropertyBoolValue("XMButton7Shown"))
            {
                this.ActionButton3.Text = GetPropertyStringValue("XMButton7Text");
                this.ActionButton3.CommandParameter = "XMButton7";
                this.ActionButton3.IsEnabled = GetPropertyBoolValue("XMButton7Enabled");
                this.ActionButton3.Visibility = Visibility.Visible;
            }
            else if (GetPropertyBoolValue("XMNonDispenseCylinder401EmptyButtonShown"))
            {
                this.ActionButton3.Text = GetPropertyStringValue("XMNonDispenseCylinder401EmptyButtonText");
                this.ActionButton3.CommandParameter = "XMNonDispenseCylinder401EmptyButton";
                this.ActionButton3.IsEnabled = GetPropertyBoolValue("XMNonDispenseCylinder401EmptyButtonEnabled");
                this.ActionButton3.Visibility = Visibility.Visible;
            }
            else
            {
                this.ActionButton3.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateActionButton4()
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                    if (GetPropertyBoolValue("XMButton3Shown"))
                    {
                        this.ActionButton4.Text = GetPropertyStringValue("XMButton3Text");
                        this.ActionButton4.CommandParameter = "XMButton3";
                        this.ActionButton4.IsEnabled = GetPropertyBoolValue("XMButton3Enabled");
                        this.ActionButton4.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.ActionButton4.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "XMCashReplenish":
                    this.ActionButton4.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateChangeConfirmationButtons()
        {
            UpdateApplyChangesButton();
            UpdateCancelChangesButton();
        }

        private void UpdateApplyChangesButton()
        {
            this.ApplyChangesButton.IsEnabled = GetPropertyBoolValue("XMButton1Enabled");
            this.ApplyChangesButton.Visibility = GetPropertyBoolValue("XMButton1Shown") ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateCancelChangesButton()
        {
            this.CancelChangesButton.IsEnabled = GetPropertyBoolValue("XMCancelChangesEnabled");
            this.CancelChangesButton.Visibility = GetPropertyBoolValue("XMCancelChangesShown") ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateMenuButtons()
        {
            UpdateMenuButton1();
            UpdateMenuButton2();
            UpdateMenuButton3();
            UpdateMenuButton4();
        }

        private void UpdateMenuButton1()
        {
            this.MenuButton1.Visibility = GetPropertyBoolValue("XMMenuButton5001Shown") ? Visibility.Visible : Visibility.Hidden;
            this.MenuButton1.IsChecked = GetPropertyBoolValue("XMMenuButton5001Toggled");
        }

        private void UpdateMenuButton2()
        {
            this.MenuButton2.Visibility = GetPropertyBoolValue("XMMenuButton5002Shown") ? Visibility.Visible : Visibility.Hidden;
            this.MenuButton2.IsChecked = GetPropertyBoolValue("XMMenuButton5002Toggled");
        }

        private void UpdateMenuButton3()
        {
            this.MenuButton3.Visibility = GetPropertyBoolValue("XMMenuButton5003Shown") ? Visibility.Visible : Visibility.Hidden;
            this.MenuButton3.IsChecked = GetPropertyBoolValue("XMMenuButton5003Toggled");
        }

        private void UpdateMenuButton4()
        {
            this.MenuButton4.Visibility = GetPropertyBoolValue("XMMenuButton5004Shown") ? Visibility.Visible : Visibility.Hidden;
            this.MenuButton4.IsChecked = GetPropertyBoolValue("XMMenuButton5004Toggled");
        }

        private void ExitButton_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                case "XMCashReplenish":
                    this.viewModel.ActionCommand.Execute("XMButton8");
                    break;
                case "XMCashStatus":
                case "XMCashStatusGlory":
                case "SmCardManagement":
                    this.viewModel.ActionCommand.Execute("SMButton8");
                    break;
                default:
                    break;
            }
        }

        private void XMNumericKey_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.NumericPad.IsEnabled = (bool)e.NewValue;
        }

        private void XMNumericKey_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as ImageButton;
            if (null != button)
            {
                GridItem item = button.DataContext as GridItem;
                if (null != item)
                {
                    this.NumericPad.IsEnabled = item.IsEnabled;
                }
            }
        }
    }
}