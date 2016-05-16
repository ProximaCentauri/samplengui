using SSCOUIModels;
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
using SSCOControls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for CashManagementConfirmAction.xaml
    /// </summary>
    public partial class CashManagementConfirmAction : BackgroundView
    {
        public CashManagementConfirmAction(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
