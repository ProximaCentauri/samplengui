using SSCOControls;
using System;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SSCOUIModels.Controls
{
    public class SSCOUITouchListBox : TouchListBox
    {
        public SSCOUITouchListBox()
        {
            TouchUp += TouchListBox_TouchUp;
            ManipulationBoundaryFeedback += TouchListBox_ManipulationBoundaryFeedback;
        }

        private void TouchListBox_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void TouchListBox_TouchUp(object sender, TouchEventArgs e)
        {
            IMainViewModel viewModel = DataContext as IMainViewModel;
            if (viewModel != null)
            {
                viewModel.ActionCommand.Execute("UIActivity");
            }
        }
    }
}
