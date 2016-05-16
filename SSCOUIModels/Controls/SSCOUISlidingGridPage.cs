using SSCOControls;
using System.Windows.Input;

namespace SSCOUIModels.Controls
{
    public class SSCOUISlidingGridPage : SlidingGridPage
    {
        public SSCOUISlidingGridPage()
        {
            TouchMove += SlidingGridPage_TouchMove;
        }

        private void SlidingGridPage_TouchMove(object sender, TouchEventArgs e)
        {
            IMainViewModel viewModel = DataContext as IMainViewModel;
            if (viewModel != null)
            {
                viewModel.ActionCommand.Execute("UIActivity");
            }
        }
    }
}
