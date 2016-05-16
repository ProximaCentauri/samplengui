using System;
using System.Windows;
using SSCOUIModels;
using SSCOUIModels.Controls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for ExtendedPopup.xaml
    /// </summary>
    public partial class ExtendedPopup : PopupView
    {
        public ExtendedPopup(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}