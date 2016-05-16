using System;
using System.Windows;
using SSCOControls;
using SSCOUIModels;
using SSCOUIModels.Controls;
using FPsxWPF.Controls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreModeFullAssist.xaml
    /// </summary>
    public partial class StoreModeFullAssist : BackgroundView
    {
        public StoreModeFullAssist(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            this.UpdateInputBox1();
            this.UpdateInputBox2();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "MessageBoxEcho":
                    this.UpdateInputBox1();
                    break;
                case "MessageBoxEcho1":
                    this.UpdateInputBox2();
                    break;
            }
        }

        private void UpdateInputBox1()
        {
            this.FullAssistInput1.Text = this.viewModel.GetPropertyValue("MessageBoxEcho").ToString();
        }

        private void UpdateInputBox2()
        {
            this.FullAssistInput2.Text = this.viewModel.GetPropertyValue("MessageBoxEcho1").ToString();
        }

        private void AssistKeyboard_TouchDown(object sender, RoutedEventArgs e)
        {
            var button = sender as ImageButton;
            if (null != button)
            {
                GridItem item = button.DataContext as GridItem;
                if (null != item)
                {
                    this.viewModel.ActionCommand.Execute(string.Format("{0}Click({1})", button.Name, item.Data));
                }
            }
        }
    }
}