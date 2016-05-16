using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using SSCOUIModels;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;

namespace SSCOUI
{
    /// <summary>
    /// Interaction logic for ExtendedWindow.xaml
    /// </summary>
    public partial class ExtendedWindow : Window
    {
        public ExtendedWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
        }

        private IMainViewModel viewModel;
        private PopupView currentPopupView;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);            
            this.ExtendedCartControl.DataContext = this.viewModel;            
            ShowControls();            
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("State"))
            {
                ShowControls();
            }
            else if (e.PropertyName.Equals("AttendantMode"))
            {
                ShowControls();
                ShowPopup();
            }
            else if (e.PropertyName.Equals("StoreMode") || e.PropertyName.Equals("StateParam"))
            {
                ShowPopup();
            }
        }

        private void ShowPopup()
        {
            if (this.viewModel.AttendantMode && (this.viewModel.StoreMode || this.viewModel.StateParam.Equals("WaitForRemoteAssistance")))
            {
                Context context;
                if (this.viewModel.ParamToViews.TryGetValue("ExtendedWindowPopup", out context) && null != context.View)
                {
                    PopupView popupView = App.GetCachedView(context.View, context.Primary) as PopupView;
                    if (null == this.currentPopupView || !this.currentPopupView.GetType().Equals(popupView.GetType()))
                    {
                        this.currentPopupView = popupView;
                        PopupControl.ShowPopup(true, popupView, true);
                    }
                }
            }
            else
            {
                this.currentPopupView = null;
                this.PopupControl.ShowPopup(false, null, true);
            }
        }

        private void ShowControls()
        {
            this.ExtendedCartControl.Visibility = ((this.viewModel.State.Equals("InTransaction") || this.viewModel.State.Equals("InTransactionVoid")) && this.viewModel.AttendantMode) ? Visibility.Visible : Visibility.Collapsed;
            this.HeaderControl.Visibility = this.ExtendedCartControl.Visibility.Equals(Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.PropertyChanged -= new PropertyChangedEventHandler(ViewModel_PropertyChanged);
        }
    }
}
