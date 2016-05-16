using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using SSCOUIModels.Controls;
using SSCOUIModels.Helpers;
using SSCOUIModels;
using SSCOUIModels.Models;
using FPsxWPF.Controls;
using RPSWNET;
using PsxNet;
using SSCOControls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for Sale.xaml
    /// </summary>
    public partial class Sale : BackgroundView
    {
        public Sale(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            if (CmDataCapture.IsCaptureActive(CmDataCapture.MaskExtensive))
            {
                this.QuickPickItemList.PerfLogging = true;
                this.QuickPickItemList.LogEvent += QuickPickItemList_LogEvent;
            }            
        }

        private void CustomerReceipt_CurrentChanged(object sender, EventArgs e)
        {
            this.currentReceiptItem = CollectionViewSource.GetDefaultView(this.customerReceiptCollection).CurrentItem as CustomerReceiptItem;
            ShowStatus();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("AttendantMode"))
            {
                OnAttendantModeChanged();
            }
            else if (name.Equals("NextGenData"))
            {
                OnNextGenDataChanged();
            }
            else if (name.Equals("CustomerReceipt"))
            {
                OnCustomerReceiptChanged();
            }
            else if (name.Equals("QuickPickItems"))
            {
                OnQuickPickChanged();
            }
            else if (name.Equals("CMButton1MedShown"))
            {
                UpdateSkipBaggingPanel();
            }
            else if (name.Equals("ScanBagTextShown"))
            {
                ShowCardMessage();            
            }
            else if (name.Equals("ScanBagTextArea"))
            {
                ShowCardMessage();
            }
        }

        public override void OnStateParamChanged(String param)
        {
            this.cancelCoupons = false;
    	    this.QuickPickItemList.SelectedIndex = -1;
            OnAttendantModeChanged();
            OnCustomerReceiptChanged();
            OnQuickPickChanged();            
            ShowCardMessage();
            ShowElements();
            OnNextGenDataChanged();
            UpdateSkipBaggingPanel();            
        }

        private void CancelCoupon_TouchDown(object sender, TouchEventArgs e)
        {
            this.cancelCoupons = true;
            ShowElements();
            viewModel.ActionCommand.Execute("CancelCoupon");
        }

        private void CustomerReceipt_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((NotifyCollectionChangedAction.Add == e.Action || NotifyCollectionChangedAction.Remove == e.Action || NotifyCollectionChangedAction.Reset == e.Action) &&
                (0 == this.customerReceiptCollection.Count || 1 == this.customerReceiptCollection.Count))
            {
                ShowElements();
            }
        }

        private void IsQuickPick()
        {
            bool isQuickPick = null != this.quickPickCollection && this.quickPickCollection.Count > 0;
            if (this.isQuickPick != isQuickPick)
            {
                this.isQuickPick = isQuickPick;
                ShowCardMessage();
                ShowElements();
            }            
        }

        private void OnAttendantModeChanged()
        {
            this.allowFilmstrip = (this.viewModel.ShowTransitionEffects && !this.viewModel.AttendantMode &&
                Properties.Settings.Default.ShowFilmstrip);
            if ((this.allowFilmstrip && Visibility.Collapsed == this.FilmstripPanel.Visibility) ||
                !this.allowFilmstrip && Visibility.Collapsed == this.SingleCardPanel.Visibility)
            {
                this.FilmstripPanel.Visibility = this.allowFilmstrip ? Visibility.Visible : Visibility.Collapsed;
                this.SingleCardPanel.Visibility = this.allowFilmstrip ? Visibility.Collapsed : Visibility.Visible;
                if (this.allowFilmstrip)
                {
                    BindingOperations.SetBinding(this.FilmstripPanel, ItemsControl.ItemsSourceProperty,
                        new Binding() { Source = this.viewModel.GetPropertyValue("CustomerReceipt") });                    
                }
                else
                {
                    BindingOperations.ClearBinding(this.FilmstripPanel, ItemsControl.ItemsSourceProperty);
                }
            }
        }

        private void OnCustomerReceiptChanged()
        {
            ObservableCollection<ReceiptItem> customerReceiptCollection = this.viewModel.GetPropertyValue("CustomerReceipt") as ObservableCollection<ReceiptItem>;
            if (customerReceiptCollection != this.customerReceiptCollection)
            {
                if (null != this.customerReceiptCollection)
                {
                    CollectionViewSource.GetDefaultView(this.customerReceiptCollection).CurrentChanged -= CustomerReceipt_CurrentChanged;
                    this.customerReceiptCollection.CollectionChanged -= CustomerReceipt_CollectionChanged;
                    this.currentReceiptItem = null;
                }
                this.customerReceiptCollection = customerReceiptCollection;
                if (null != this.customerReceiptCollection)
                {
                    this.customerReceiptCollection.CollectionChanged += CustomerReceipt_CollectionChanged;
                    CollectionViewSource.GetDefaultView(this.customerReceiptCollection).CurrentChanged += CustomerReceipt_CurrentChanged;
                    if (this.allowFilmstrip)
                    {
                        BindingOperations.SetBinding(this.FilmstripPanel, ItemsControl.ItemsSourceProperty,
                            new Binding() { Source = this.viewModel.GetPropertyValue("CustomerReceipt") });                        
                    }
                }                
            }            
        }

        private void OnQuickPickChanged()
        {
            ObservableCollection<GridItem> quickPickCollection = this.viewModel.GetPropertyValue("QuickPickItems") as ObservableCollection<GridItem>;
            if (quickPickCollection != this.quickPickCollection)
            {
                if (null != this.quickPickCollection)
                {
                    this.quickPickCollection.CollectionChanged -= quickPickCollection_CollectionChanged;
                    this.isQuickPick = false;
                }
                this.quickPickCollection = quickPickCollection;
                if (null != this.quickPickCollection)
                {
                    this.quickPickCollection.CollectionChanged += quickPickCollection_CollectionChanged;
                }
                IsQuickPick();
            }
        }

        private void OnNextGenDataChanged()
        {
            bool enabled = !GetPropertyStringValue("NextGenData").Equals("BagAndEAS");
            this.QuickPickItemList.IsEnabled = enabled;
            this.scanItemSearchKeyInItem.IsEnabled = enabled;
            this.FilmstripPanel.IsEnabled = enabled;
        }

        private void ShowCardMessage()
        {
            this.CardMessagePanel.Visibility = (Boolean)this.viewModel.GetPropertyValue("ScanBagTextShown") && this.viewModel.GetPropertyValue("ScanBagTextArea").ToString().Length != 0 ? Visibility.Visible : Visibility.Collapsed;
            this.NoItemsContent.Visibility = Visibility.Collapsed == this.CardMessagePanel.Visibility && !this.isQuickPick ?
                Visibility.Visible : Visibility.Collapsed;
            this.QuickPickNoItemsContent.Visibility = Visibility.Collapsed == this.CardMessagePanel.Visibility && this.isQuickPick ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowElements()
        {
            this.NoItemsPanel.Height = this.isQuickPick ? 310 : 458;
            if (this.allowFilmstrip)
            {
                this.FilmstripPanel.Camera.Position = this.isQuickPick ? new Point3D(0, 0, 1.8) : new Point3D(0, 0, 1.2);                
            }
            this.CardPanel.Height = this.FilmstripPanel.ElementHeight = this.isQuickPick ? 310 : 458;
            this.EnterCouponsPanel.Visibility = this.viewModel.StateParam.Equals("EnterCoupons") && !this.cancelCoupons ?
                Visibility.Visible : Visibility.Collapsed;            
            this.QuickPickListPanel.Visibility = this.isQuickPick &&  Visibility.Collapsed == this.EnterCouponsPanel.Visibility ?
                Visibility.Visible : Visibility.Collapsed;
            this.NoItemsPanel.Visibility = (null == this.customerReceiptCollection || this.customerReceiptCollection.Count == 0) &&
                Visibility.Collapsed == this.EnterCouponsPanel.Visibility ? Visibility.Visible : Visibility.Collapsed;
            this.CardPanel.Visibility = Visibility.Collapsed == this.NoItemsPanel.Visibility && Visibility.Collapsed == this.EnterCouponsPanel.Visibility ?
                Visibility.Visible : Visibility.Collapsed;
            this.CardMessage.IsQuickPick = this.isQuickPick;
            ShowStatus();
        }

        private void ShowStatus()
        {
            this.skipBagging.Visibility = this.viewModel.StateParam.Equals("Bag") ? Visibility.Visible : Visibility.Collapsed;
            this.ScanItemTextBlock.Property(TextBlock.TextProperty).SetResourceValue(null != this.currentReceiptItem && this.currentReceiptItem.IsIntervention ?
                "AssistanceComing" : "ScanNextItem");            
            this.scanItemStatus.Visibility = (Visibility.Visible != this.skipBagging.Visibility && Visibility.Visible != this.EnterCouponsPanel.Visibility) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void quickPickCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NotifyCollectionChangedAction.Add == e.Action || NotifyCollectionChangedAction.Remove == e.Action || NotifyCollectionChangedAction.Reset == e.Action)
            {
                IsQuickPick();
            }
        }

        private void QuickPickItemList_LogEvent(object sender, SSCOControls.LogEventArgs e)
        {
            if (e.LogMessage.Equals("StartItemListRender"))
            {
                viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.PickListItemsRender, string.Empty);
            }
            else if (e.LogMessage.Equals("EndItemListRender"))
            {
                viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.PickListItemsRender, string.Format("Items rendered: {0}", e.RenderedItemCount));
            }
        }

        private void QuickPickList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = QuickPickItemList.SelectedItem as GridItem;
            if (null != item)
            {                
                viewModel.ActionCommand.Execute(String.Format(CultureInfo.CurrentCulture, "QuickPickItem({0})", item.Data));
            }
        }

        private void UpdateSkipBaggingPanel()
        {
            if (this.viewModel.StateParam.Equals("Bag"))
            {
                if (GetPropertyBoolValue("CMButton1MedShown"))
                {
                    this.skipBaggingText.Property(TextBlock.TextProperty).SetResourceValue("BagItemOrSkip");
                    this.skipBaggingButton.Visibility = Visibility.Visible;
                }
                else
                {
                    this.skipBaggingText.Property(TextBlock.TextProperty).SetResourceValue("BagItem");
                    this.skipBaggingButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool allowFilmstrip;
        private bool cancelCoupons;
        private CustomerReceiptItem currentReceiptItem;
        private ObservableCollection<ReceiptItem> customerReceiptCollection; 
        private bool isQuickPick;
        private ObservableCollection<GridItem> quickPickCollection;           
    }
}
