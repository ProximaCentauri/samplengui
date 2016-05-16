using System;

namespace SSCOUIModels.Controls
{
    public class BackgroundView : View
    {
        public BackgroundView(IMainViewModel viewModel) :
            base(viewModel)
        {
            ShowCart = true;
            ShowDecorator = true;
            ShowHeader = true;            
            ShowSystemFunctions = true;
        }
        
        public bool ShowCart { get; set; }

        public bool ShowDecorator { get; set; }

        public bool ShowHeader { get; set; }

        public bool ShowSystemFunctions { get; set; }
    }
}
