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
    /// Interaction logic for YesNo.xaml
    /// </summary>
    public partial class YesNo : PopupView
    {
        public YesNo(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(String param)
        {
            if (param.Equals("ConfirmAbort"))
            {
                this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("Confirm");
            }
            else if (param.Equals("ConfirmQuantity"))
            {
                this.Instructions.Property(TextBlock.TextProperty).SetResourceFormattedValue("ConfirmQuantity", this.viewModel.CurrentItem.Quantity);
            }
            else if (param.Equals("Continue"))
            {
                this.Instructions.Property(TextBlock.TextProperty).SetResourceValue("Continue");
            }
        }
    }
}
