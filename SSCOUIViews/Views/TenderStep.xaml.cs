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
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using SSCOUIModels;
using System.ComponentModel;
using System.Globalization;
using FPsxWPF;
using SSCOControls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for TenderStep.xaml
    /// </summary>
    public partial class TenderStep : BackgroundView
    {
        public TenderStep(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("ChangeDue"))
            {
                changeDue = GetAmountDue(name);
            }
            else if (name.Equals("RefundDue"))
            {
                refundDue = GetAmountDue(name);
            }
            else if (name.Equals("CMButton1MedShown"))
            {
                UpdatePayOtherWay();
            }
            else if (name.Equals("AmountDue"))
            {
                DueAmount.Text = value.ToString();
            }
            else if (name.Equals("ShowSigCapture"))
            {
                UpdateSignatureControls();
            }
            else if (name.Equals("DegradedMode"))
            {
                UpdatePayOtherWay();
            }
        }

        /// <summary>
        /// OnStateParamChanged method
        /// </summary>
        /// <param name="param">This is a parameter with a type of String</param>
        public override void OnStateParamChanged(String param)
        {
            PayOtherWayButton.Visibility = Visibility.Collapsed;
            switch (param)
            {
                case "CashPayment":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("InsertCash");
                    this.GoBack.Style = this.FindResource("cancelPaymentStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("CancelPayment");
                    this.PayOtherWayButton.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
                    ChangeDueLabelText();
                    break;
                case "EnterPin":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("EnterPin");
                    this.GoBack.Style = this.FindResource("cancelPaymentStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("CancelPayment");
                    ChangeDueLabelText();
                    break;
                case "EnterAmount":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("EnterAmount");
                    this.GoBack.Style = this.FindResource("cancelPaymentStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("CancelPayment");
                    ChangeDueLabelText();
                    break;
                case "SwipeCard":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("UsePinpad");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("TouchCancel");
                    this.GoBack.Style = this.FindResource("buttonGoBackStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
                    ChangeDueLabelText();
                    break;
                case "ScanCard":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("ScanGiftCard");
                    this.GoBack.Style = this.FindResource("buttonGoBackStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
                    ChangeDueLabelText();
                    break;
                case "ScanVoucher":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("ScanVoucher");
                    this.CMButton1Med.Property(ImageButton.TextProperty).SetResourceValue("OtherPayment");
                    this.GoBack.Style = this.FindResource("buttonGoBackStyle") as Style;
                    this.GoBack.Property(ImageButton.TextProperty).SetResourceValue("GoBack");
                    ChangeDueLabelText();
                    break;
                case "DepositCoupon":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("DepositCoupon");
                    this.CMButton1Med.Property(ImageButton.TextProperty).SetResourceValue("Done");
                    ChangeDueLabelText();
                    break;
                case "InsertCoupon":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("InsertCoupon");
                    ChangeDueLabelText();
                    break;
                case "DepositGiftCard":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("DepositGiftCard");
                    this.CMButton1Med.Property(ImageButton.TextProperty).SetResourceValue("Done");
                    ChangeDueLabelText();
                    break;
                case "InsertGiftCard":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("DepositGiftCard");
                    this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("DepositCard");
                    ChangeDueLabelText();
                    break;
                case "AcknowledgeSig":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("SignNeeded");
                    this.DueLabel.Property(TextBlock.TextProperty).SetResourceFormattedValue("Due", String.Empty);                    
                    break;
                case "RequestSig":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("SignNeeded");
                    this.DueLabel.Property(TextBlock.TextProperty).SetResourceValue("Change");                    
                    break;
                case "TakeCash":
                    this.LeadthruText.Property(TextBlock.TextProperty).SetResourceValue("TakeCash");
                    this.CMButton1Med.Property(ImageButton.TextProperty).SetResourceValue("Proceed");
                    ChangeDueLabelText();
                    break;
            }
            UpdateSignatureControls();
        }

        private void UpdateSignatureControls()
        {
            if (viewModel.StateParam.Equals("RequestSig")|| viewModel.StateParam.Equals("AcknowledgeSig")||
                viewModel.BackgroundStateParam.Equals("RequestSig")|| viewModel.BackgroundStateParam.Equals("AcknowledgeSig"))
            {
                if (FPsx.ConvertFromOleBool((String)viewModel.GetPropertyValue("ShowSigCapture"))) 
                {
                    signNameAnimation.Visibility = Visibility.Collapsed;
                }
                else
                {
                    signNameAnimation.Visibility = Visibility.Visible;
                }
                SignatureControl.Visibility = Visibility.Visible;
                SignatureControl.RefreshUI();
            }
            else
            {
                signNameAnimation.Visibility = Visibility.Collapsed;
                SignatureControl.Visibility = Visibility.Collapsed;
            }
        }

        public override void Show(bool isShowing)
        {
            UpdateSignatureControls();
            base.Show(isShowing);
        }

        private void ChangeDueLabelText()
        {
            refundDue = GetAmountDue("RefundDue");
            changeDue = GetAmountDue("ChangeDue");
            if (refundDue <= 0 && changeDue <= 0)
            {
                this.DueLabel.Property(TextBlock.TextProperty).SetResourceFormattedValue("Due", String.Empty);                
            }
        }

        private double GetAmountDue(string propertyName)
        {
            string amount = GetPropertyStringValue(propertyName);
            double value = amount != null && amount.Length > 0 ? double.Parse(amount) : 0;
            value = value / 100;
            if (value > 0 && propertyName.Equals("ChangeDue"))
            {
                this.DueLabel.Property(TextBlock.TextProperty).SetResourceValue("Change");                
            }
            else if (value > 0 && propertyName.Equals("RefundDue"))
            {
                this.DueLabel.Property(TextBlock.TextProperty).SetResourceValue("Refunded");                
            }
            return value;
        }

        private void UpdatePayOtherWay()
        {
            if (viewModel.BackgroundStateParam.Equals("CashPayment") && !viewModel.DegradedMode.Equals("cash"))
            {
                PayOtherWayButton.Visibility = GetPropertyBoolValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                PayOtherWayButton.Visibility = Visibility.Collapsed;
            }
        }


        private double refundDue = 0;
        private double changeDue = 0;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DueAmount.Text = GetPropertyStringValue("AmountDue");
            PaidLabel.Property(TextBlock.TextProperty).SetResourceFormattedValue("Paid", string.Empty);
        }
    }
}
