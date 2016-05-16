using SSCOControls;
using System;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace SSCOUIModels.Controls
{
    public class SSCOUIElementFlow : ElementFlow
    {
        public SSCOUIElementFlow()
        {
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (this.IsNavigatingLeft || this.IsNavigatingRight)
            {
                IMainViewModel viewModel = DataContext as IMainViewModel;
                if (viewModel != null)
                {
                    viewModel.ActionCommand.Execute("UIActivity");
                }
            }
            base.OnSelectionChanged(e);            
        }
    }
}
