using System;
using System.Windows;
using SSCOUIModels.Controls;
using SSCOUIModels;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for CustomPopup.xaml
    /// </summary>
    public partial class CustomPopup : PopupView
    {
        public CustomPopup(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("ButtonGoBackShown"))
            {
                this.GoBackButton.Visibility = GetPropertyBoolValue("ButtonGoBackShown") ? Visibility.Visible : Visibility.Collapsed;
            }
            if (name.Equals("LeadthruText"))
            {
                this.LeadthruText.Text = value.ToString();
            }
            else if (name.Equals("Instructions"))
            {
                this.Instructions.Text = value.ToString();
            }
            else if (name.Equals("ButtonGoBack"))
            {
                this.GoBackButton.Text = value.ToString();
            }
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            this.LeadthruText.Text = GetPropertyStringValue("LeadthruText");
            this.Instructions.Text = GetPropertyStringValue("Instructions");
            this.GoBackButton.Text = GetPropertyStringValue("ButtonGoBack");
            this.GoBackButton.Visibility = GetPropertyBoolValue("ButtonGoBackShown") ? Visibility.Visible : Visibility.Collapsed;
            this.List1Button.Visibility = param.Equals("CmDataEntry") || param.Equals("CmDataEntry1") || param.Equals("CmDataEntry2") || param.Equals("CmDataEntry3") ||
                param.Equals("CmDataEntry4") || param.Equals("SelectContainer") || param.Equals("SelectContainerWith3TareA") ? Visibility.Visible : Visibility.Collapsed;
            this.List2Button.Visibility = param.Equals("CmDataEntry") || param.Equals("CmDataEntry2") || param.Equals("CmDataEntry3") || param.Equals("CmDataEntry4") ||
                param.Equals("SelectContainer") || param.Equals("SelectContainerWith3TareA") || param.Equals("SelectContainerWith3TareB") ? Visibility.Visible : Visibility.Collapsed;
            this.List3Button.Visibility = param.Equals("CmDataEntry3") || param.Equals("SelectContainerWith3TareA") ||
            param.Equals("CmDataEntry4") || param.Equals("SelectContainerWith3TareB") || param.Equals("SelectContainer") ? Visibility.Visible : Visibility.Collapsed;
            this.List4Button.Visibility = param.Equals("CmDataEntry4") || param.Equals("SelectContainerWith3TareB") || param.Equals("SelectContainer") ? Visibility.Visible : Visibility.Collapsed;
            this.List5Button.Visibility = this.List6Button.Visibility = param.Equals("SelectContainerWith2Tare") ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
