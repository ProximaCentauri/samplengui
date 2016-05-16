using SSCOUIModels;
using SSCOUIModels.Models;
using SSCOUIModels.Controls;
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
using FPsxWPF.Controls;
using System.Collections;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for CancelYesNo.xaml
    /// </summary>
    public partial class CancelYesNo : PopupView
    {
        public CancelYesNo(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            UpdateLeadthruText();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("LeadthruText"))
            {
                UpdateLeadthruText();
            }            
        }

        private void UpdateLeadthruText()
        {
            string leadthruText = GetPropertyStringValue("LeadthruText");
            if (viewModel.StateParam.Equals("ConfirmAbort"))
            {
                this.ConfirmAbortLeadthruText.Text = leadthruText;
            }
            else if (viewModel.StateParam.Equals("ConfirmVoid"))
            {
                this.ConfirmVoidLeadthruText.Text = leadthruText;
            }
        }
    }
}
