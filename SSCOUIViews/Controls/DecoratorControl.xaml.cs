using SSCOUIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for DecoratorControl.xaml
    /// </summary>
    public partial class DecoratorControl : Grid
    {
        public DecoratorControl()
        {
            InitializeComponent();           
        }

        public bool ShowIcons
        { 
            get
            {
                return this.showIcons;
            }            
            set
            {
                if (this.showIcons != value)
                {
                    this.showIcons = value;
                    this.attendantIcon.Visibility = this.trainingIcon.Visibility = this.attendantAndTrainingIcon.Visibility =  this.showIcons ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);
            UpdateTrainingAttendantModePanel();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AttendantMode") || e.PropertyName.Equals("TrainingMode"))
            {
                UpdateTrainingAttendantModePanel();
            }
        }

        private void UpdateTrainingAttendantModePanel() 
        {
            attendantModePanel.Visibility = viewModel.AttendantMode & !viewModel.TrainingMode ? Visibility.Visible : Visibility.Collapsed;
            trainingModePanel.Visibility = !viewModel.AttendantMode & viewModel.TrainingMode ? Visibility.Visible : Visibility.Collapsed;
            attendantAndTrainingModePanel.Visibility = viewModel.AttendantMode & viewModel.TrainingMode ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool showIcons = true;
        private IMainViewModel viewModel;        
    }
}
