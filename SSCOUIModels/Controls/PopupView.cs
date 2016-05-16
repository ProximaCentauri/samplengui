namespace SSCOUIModels.Controls
{
    using System;
    using System.Linq;

    public class PopupView : View
    {
        public enum PopupBackgroundType
        {
            Darken,
            Shroud
        }

        public enum PopupAlignment
        {
            Center,
            Left,            
            None
        }

        public PopupView(IMainViewModel viewModel) :
            base(viewModel)
        {
            BackgroundType = PopupBackgroundType.Darken;
            Alignment = PopupAlignment.None;
            Duration = 0;
            ShowBackground = true;
        }

        public int LeftPadding { get; set; }

        public PopupAlignment Alignment { get; set; }

        public bool ShowBackground { get; set; }

        public int XOffset { get; set;}

        public int YOffset { get; set;}

        public string ControlsToAllowEnabled
        {
            set
            {
                string[] controls = value.Split(',');
                if (!controls.SequenceEqual<string>(ControlsToAllowEnabledArray))
                {
                    ControlsToAllowEnabledArray = controls;
                    if (null != ControlsToAllowEnableChanged)
                    {
                        ControlsToAllowEnableChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool IsControlToAllowEnabled(string controlName)
        {
            return Array.IndexOf(ControlsToAllowEnabledArray, controlName) >= 0;
        }

        public PopupBackgroundType BackgroundType { get; set; }

        public int Duration { get; set; }

        public event EventHandler ControlsToAllowEnableChanged;

        public string[] ControlsToAllowEnabledArray = new string[0];        
    }
}
