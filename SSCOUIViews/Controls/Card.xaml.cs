using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SSCOUIModels;
using SSCOUIModels.Models;
using FPsxWPF.Controls;
    
namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : Grid
    {
        public Card()
        {
            InitializeComponent();
            this.viewModel = (IMainViewModel)Application.Current.MainWindow.DataContext;
            this.quickPickCollection = this.viewModel.GetPropertyValue("QuickPickItems") as ObservableCollection<GridItem>;
            IsQuickPick();   
        }

        private void Card_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.currentItem = this.DataContext as CustomerReceiptItem;
            IsQuickPick(); 
            ShowElements();
        }
        
        private void Card_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;           
            ShowCardMessage();
            ShowElements();
        }  

        private void Card_Unloaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void ShowCardMessage()
        {            
            this.CardMessage.Visibility = this.QuickPickCardMessage.Visibility = !this.viewModel.StateParam.Equals("EnterCoupons") && !this.viewModel.StateParam.StartsWith("VoidItem") && 
                (Boolean)this.viewModel.GetPropertyValue("ScanBagTextShown") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowElements()
        {
            if (null != this.currentItem)
            {
                this.ApprovedPanel.Visibility = this.QuickPickApprovedPanel.Visibility = !this.viewModel.BackgroundStateParam.Equals("Bag") &&
                    this.currentItem.IsApproved ? Visibility.Visible : Visibility.Collapsed;
                this.AssistanceAnimation.Visibility = this.QuickPickAssistanceAnimation.Visibility = !this.viewModel.BackgroundStateParam.Equals("Bag") && 
                    this.currentItem.IsIntervention ? Visibility.Visible : Visibility.Collapsed;
            }
            this.CardMessage.IsSmaller = this.QuickPickCardMessage.IsSmaller = Visibility.Visible == this.ApprovedPanel.Visibility
                || Visibility.Visible == this.AssistanceAnimation.Visibility;
        }

        private void IsQuickPick()
        {
            bool isQuickPick = (null != quickPickCollection && quickPickCollection.Count > 0);
            if (isQuickPick && Visibility.Visible == this.FullPanel.Visibility || !isQuickPick && Visibility.Visible == this.QuickPickPanel.Visibility)
            {
                this.FullPanel.Visibility = isQuickPick ? Visibility.Collapsed : Visibility.Visible;
                this.QuickPickPanel.Visibility = isQuickPick ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BackgroundStateParam"))
            {
                ShowElements();
            }
            else if (e.PropertyName.Equals("StateParam"))
            {
                ShowCardMessage();
            }
            else if (e.PropertyName.Equals("ScanBagTextShown"))
            {
                ShowCardMessage();
            }
        }

        CustomerReceiptItem currentItem;
        ObservableCollection<GridItem> quickPickCollection;
        private IMainViewModel viewModel;        
    }
}
