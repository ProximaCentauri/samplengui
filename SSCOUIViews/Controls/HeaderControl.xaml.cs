using SSCOUIModels;
using SSCOControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : StackPanel
    {
        public HeaderControl()
        {
            InitializeComponent();
        }
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title1Text":
                case "Title1Shown":
                    this.UpdateTitle1Text(viewModel.StateParam);
                    this.UpdateTitle1Shown(viewModel.StateParam);
                    break;
                case "Title2Text":
                    this.UpdateTitle2Text(viewModel.StateParam);
                    break;
                case "Title2Shown":
                    this.UpdateTitle2Shown(viewModel.StateParam);
                    break;
                case "XMTitle1Text":
                    this.UpdateXMTitle1Text(viewModel.StateParam);
                    break;
                case "XMTitle1Shown":
                    this.UpdateXMTitle1Shown(viewModel.StateParam);
                    break;
                case "XMTitle2Text":
                    this.UpdateXMTitle2Text(viewModel.StateParam);
                    break;
                case "XMTitle2Shown":
                    this.UpdateXMTitle2Shown(viewModel.StateParam);
                    break;
                case "LeadthruText":
                    this.UpdateLeadthruText(viewModel.StateParam);
                    break;
                case "LeadthruTextShown":
                    this.UpdateLeadthruTextShown(viewModel.StateParam);
                    break;
                case "StateParam":
                    this.UpdateTitle1Text(viewModel.StateParam);
                    this.UpdateTitle1Shown(viewModel.StateParam);
                    this.UpdateTitle2Text(viewModel.StateParam);
                    this.UpdateTitle2Shown(viewModel.StateParam);
                    this.UpdateXMTitle1Text(viewModel.StateParam);
                    this.UpdateXMTitle1Shown(viewModel.StateParam);
                    this.UpdateXMTitle2Text(viewModel.StateParam);
                    this.UpdateXMTitle2Shown(viewModel.StateParam);
                    this.UpdateLeadthruText(viewModel.StateParam);
                    this.UpdateLeadthruTextShown(viewModel.StateParam);
                    break;
                case "StoreMode":
                    this.UpdateStoreModeHeader();
                    break;
            }
        }
        private void StackPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = DataContext as IMainViewModel;
            viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
        }
        private void UpdateStoreModeHeader()
        {
            if (this.viewModel.StoreMode)
            {
                this.StoreModeHeader.Visibility = Visibility.Visible;                
            }
            else
            {
                this.StoreModeHeader.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForXMTitle = new List<string>() { "XMMediaStatus", "XMLoanFailure", "XMPickupFailure" };

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListForLeadthruText = new List<string>() { "SystemMessageDegradedModeWithBitmap", "SystemMessageWithBitmap", "AM_SystemMessageDegradedModeWithBitmap", 
            "AM_SystemMessageWithBitmap", "SystemMessageWithWebControl", "SystemMessageDegradedModeWithWebControl", "SystemMessageHopperFailureWithWebControl", 
            "SystemMessageHopperSubstitutionWithWebControl", "AM_SystemMessageWithWebControl", "AM_SystemMessageDegradedModeWithWebControl", "AM_SystemMessageHopperFailureWithWebControl", 
            "AM_SystemMessageHopperSubstitutionWithWebControl", "SystemMessageWithAVI", "AM_SystemMessageWithAVI", "SystemMessageHopperFailure", "SystemMessageHopperSubstitution",
            "SystemMessageDegradedMode", "SystemMessage", "AM_SystemMessage", "AM_SystemMessageDegradedMode", "AM_SystemMessageHopperFailure", "AM_SystemMessageHopperSubstitution" };

        private void UpdateXMTitle1Text(String param)
        {
            switch (param)
            {
                case "XMLoanFailure":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("XM_LoanFailure");
                    break;
                case "XMPickupFailure":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("XM_PickUpFailure");
                    break;
                default:
                    if (null != param)
                    {
                        if (paramListForXMTitle.Contains(param))
                        {
                            this.StoreModeScreenTitle1.Text = this.viewModel.GetPropertyValue("XMTitle1Text").ToString();
                        }
                    }
                    break;
            }
        }

        private void UpdateLeadthruText(String param) 
        {
            if(null != param)
            {
                if(paramListForLeadthruText.Contains(param))
                {
                    this.StoreModeScreenTitle1.Text = this.viewModel.GetPropertyValue("LeadthruText").ToString();
                }
            }
        }
        
        private void UpdateTitle1Text(String param)
        {
          
            switch (param)
            {
                case "SmmAddWeight":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_612");
                    break;
                case "SmmEditAdd":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_603");
                    break;
                case "SmmEditWtExp":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_614");
                    break;
                case "SmmEditWtSubCk":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_616");
                    break;
                case "SmmEditWtTolerance":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_618");
                    break;
                case "SmmInvalidWtTol":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_624");
                    break;
                case "SmmKeyInItemCode":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_569");
                    break;
                case "SmmKeyInWtTol":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_622");
                    break;
                case "AcknowledgeSig":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_485");
                    break;
                case "Attract":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_92");
                    break;
                case "AttractMultiLanguage":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_92");
                    break;
                case "BagAndEAS":
                case"SmConfirmSuspendedTransaction":
                    break;
                case "CashPayment":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_136");
                    break;
                case "DepositCoupons":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_131");
                    break;
                case "EnterAlphaNumericID":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_424");
                    break;
                case "EnterAlphaNumericInfo":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_813");
                    break;
                case "EnterAlphaNumericPassword":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_813");
                    break;
                case "SmEnterBirthdate":
                case "SmVoidedItems":
                case "SmCouponItems":
                case "SmVisualItems":
                case "SmRestrictedItems":
                case "SmSecurityLogs":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_40");
                    break;
                case "SmMediaStatus":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_351");
                    break;
                case "SmmMain":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_600");
                    break;
                case "SmSystemFunctions":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_354");
                    break;
                case "SmAbort":
                case"SmAborted":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_39");
                    break;
                case "SmIgnoreChanges":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_1571");
                    break;
                case "SmShowPickupDumpFailure":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_1441");
                    break;
                case "SmSelectModeOfOperation":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_1801");
                    break;
                case "SmVoidAndRemoveItem":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_545");
                    break;
                case "SmReportsMenu":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_1097");
                    break;
                case "SmRunReports":
                    this.StoreModeScreenTitle1.Property(TextBlock.TextProperty).SetResourceValue("SmRunReportsTitle");
                    break;
                default:
                    if (null != param)
                    {
                        if (!paramListForXMTitle.Contains(param) && !paramListForLeadthruText.Contains(param))
                        {
                            this.StoreModeScreenTitle1.Text = this.viewModel.GetPropertyValue("Title1Text").ToString();
                        }
                    }
                    break;
            }
        }

        private void UpdateXMTitle1Shown(String param)
        {
            if (null != param)
            {
                if (paramListForXMTitle.Contains(param))
                {
                    if (String.IsNullOrEmpty(this.viewModel.GetPropertyValue("XMTitle1Text").ToString()))
                    {
                        this.StoreModeScreenTitle1.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.StoreModeScreenTitle1.Visibility = ((bool)this.viewModel.GetPropertyValue("XMTitle1Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        private void UpdateLeadthruTextShown(String param)
        {
            if (null != param)
            {
                if (paramListForLeadthruText.Contains(param))
                {
                    this.StoreModeScreenTitle1.Visibility = ((bool)this.viewModel.GetPropertyValue("LeadthruTextShown")) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void UpdateTitle1Shown(String param)
        {
            if (null != param)
            {
                if (!paramListForXMTitle.Contains(param) && !paramListForLeadthruText.Contains(param))
                {
                    if (String.IsNullOrEmpty(this.viewModel.GetPropertyValue("Title1Text").ToString()))
                    {
                        this.StoreModeScreenTitle1.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.StoreModeScreenTitle1.Visibility = ((bool)this.viewModel.GetPropertyValue("Title1Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        private void UpdateXMTitle2Text(String param)
        {
            if (null != param)
            {
                if (paramListForXMTitle.Contains(param))
                {
                    this.StoreModeScreenTitle2.Text = this.viewModel.GetPropertyValue("XMTitle2Text").ToString(); 
                }
            }
        }

        private void UpdateTitle2Text(String param)
        {
            switch (param)
            {
                case "SmSecurityLogs":
                    this.StoreModeScreenTitle2.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_278");
                    break;
                case "SmEnterBirthdate":
                case "SmRestrictedItems":
                    this.StoreModeScreenTitle2.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_41");
                    break;
                case "SmVoidedItems":
                     this.StoreModeScreenTitle2.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_42");
                    break;
                  case "SmCouponItems":
                     this.StoreModeScreenTitle2.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_271");
                    break;
                  case "SmVisualItems":
                    this.StoreModeScreenTitle2.Property(TextBlock.TextProperty).SetResourceValue("ScotApp_512");
                    break;
                default:
                    if (null != param)
                    {
                        if (!paramListForXMTitle.Contains(param) && !paramListForLeadthruText.Contains(param))
                        {
                            this.StoreModeScreenTitle2.Text = this.viewModel.GetPropertyValue("Title2Text").ToString();
                        }
                    }
                    break;
            }
        }

        private void UpdateXMTitle2Shown(String param)
        {
            if (null != param)
            {
                if (paramListForXMTitle.Contains(param))
                {
                    if (String.IsNullOrEmpty(this.viewModel.GetPropertyValue("XMTitle2Text").ToString()))
                    {
                        this.StoreModeScreenTitle2.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.StoreModeScreenTitle2.Visibility = ((bool)this.viewModel.GetPropertyValue("XMTitle2Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    } 
                }
            }
        }

        private void UpdateTitle2Shown(String param)
        {
            if (null != param)
            {
                if (!paramListForXMTitle.Contains(param) && !paramListForLeadthruText.Contains(param))
                {
                    if (String.IsNullOrEmpty(this.viewModel.GetPropertyValue("Title2Text").ToString()))
                    {
                        this.StoreModeScreenTitle2.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.StoreModeScreenTitle2.Visibility = ((bool)this.viewModel.GetPropertyValue("Title2Shown")) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// viewModel variable
        /// </summary>
        private IMainViewModel viewModel;
    }
}
