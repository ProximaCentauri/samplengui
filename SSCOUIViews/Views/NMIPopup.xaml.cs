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

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for CustomPopup.xaml
    /// </summary>
    public partial class NMIPopup : PopupView
    {
        public NMIPopup(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

    }
}
