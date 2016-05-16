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

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for YesNo.xaml
    /// </summary>
    public partial class Help : PopupView
    {
        public Help(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            this.HelpOnWay.Visibility = param.Equals("HelpOnWay") ? Visibility.Visible : Visibility.Collapsed;
            this.CallForHelp.Visibility = param.Equals("ContextHelp") ? Visibility.Visible : Visibility.Collapsed;
            this.OkButton.Visibility = param.Equals("ContextHelp") || param.Equals("HelpOnWay") ? Visibility.Visible : Visibility.Collapsed;
            this.StoreLoginButton.Visibility = this.CancelButton.Visibility = param.Equals("ContextHelp") ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
