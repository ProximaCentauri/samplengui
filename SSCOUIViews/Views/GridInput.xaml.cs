using System;
using System.Windows;
using System.Windows.Controls;
using SSCOUIModels.Controls;
using FPsxWPF.Controls;
using SSCOUIModels;
using System.Globalization;
using System.Collections;
using System.Windows.Data;
using SSCOControls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for GridInput.xaml
    /// </summary>
    public partial class GridInput : BackgroundView
    {
        public GridInput(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("LeadthruText"))
            {
                UpdateLeadthruText();
            }
            else if (name.Equals("MessageBoxNoEcho"))
            {
                UpdateMessageBoxNoEcho();
            }
            else if (name.Equals("ButtonGoBackShown"))
            {
                UpdateButtonGoBack();
            }
            else if (name.Equals("CMButton2MidiListShown"))
            {
                UpdateMidiList2();
            }
            else if (name.Equals("SMLeadthruText"))
            {
                UpdateSMLine1Text();
            }
        }

        public override void OnStateParamChanged(string param)
        {
            UpdateStateParam(param);
        }

        private void ContainerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = ContainerList.SelectedItem as GridItem;
            if (null != item)
            {
                if (item.IsEnabled)
                {
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "SelectTare({0})", item.Data));
                }
            }
        }

        private void ContainerButtonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridItem item = ContainerButtonList.SelectedItem as GridItem;
            if (null != item)
            {
                if (item.IsEnabled)
                {
                    viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "DataNeededButtonList({0})", item.Data));
                }
            }
        }

        private void UpdateLeadthruText()
        {
            switch (this.viewModel.StateParam)
            {
                case "SelectTare":
                case "CmDataEntryWithButtonList":
                case "CmDataEntryWithCmdList":
                    GridInputHeader.Text = this.GetPropertyStringValue("LeadthruText");
                    break;  
            }            
        }

        private void UpdateMessageBoxNoEcho()
        {
            switch (this.viewModel.StateParam)
            {
                case "CmDataEntryWithButtonList":
                case "CmDataEntryWithCmdList":
                    GridInputSubMessage.Text = this.GetPropertyStringValue("MessageBoxNoEcho");
                    break;
            }
        }

        private void UpdateButtonGoBack()
        {
            switch (this.viewModel.StateParam)
            {
                case "SelectTare":
                case "CmDataEntryWithButtonList":
                case "CmDataEntryWithCmdList":
                    goBackButton.Visibility = (this.GetPropertyBoolValue("ButtonGoBackShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmDataEntryWithCmdList":
                case "AM_SelectTare":
                    goBackButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateMidiList2()
        {
            switch (this.viewModel.StateParam)
            {
                case "SelectTare":
                case "CmDataEntryWithButtonList":
                case "CmDataEntryWithCmdList":
                    cmBackButton.Visibility = (this.GetPropertyBoolValue("CMButton2MidiListShown")) ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "SmDataEntryWithCmdList":
                case "AM_SelectTare":
                    cmBackButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateHeader()
        {
            switch (this.viewModel.StateParam)
            {
                case "SmDataEntryWithCmdList":
                case "AM_SelectTare":
                    this.ShowHeader = true;
                    break;
                default:
                    this.ShowHeader = false;
                    break;
            }
        }

        private void UpdateStateParam(string param)
        {
            UpdateLeadthruText();
            UpdateMessageBoxNoEcho();
            UpdateButtonGoBack();
            UpdateMidiList2();
            UpdateHeader();
            UpdateSMLine1Text();
            UpdateSMLine2Text();

            switch (param)
            {
                case "SelectTare":
                    ContainerListStackPanel.Visibility = Visibility.Visible;
                    ButtonListStackPanel.Visibility = Visibility.Collapsed;
                    CmdListStackPanel.Visibility = Visibility.Collapsed;
                    GridInputHeader.Visibility = Visibility.Visible;
                    GridInputSubMessage.Visibility = Visibility.Collapsed;
                    break;
                case "CmDataEntryWithButtonList":
                    ContainerListStackPanel.Visibility = Visibility.Collapsed;
                    ButtonListStackPanel.Visibility = Visibility.Visible;
                    CmdListStackPanel.Visibility = Visibility.Collapsed;
                    GridInputHeader.Visibility = Visibility.Visible;
                    GridInputSubMessage.Visibility = Visibility.Visible;
                    break;
                case "CmDataEntryWithCmdList":
                    ContainerListStackPanel.Visibility = Visibility.Collapsed;
                    ButtonListStackPanel.Visibility = Visibility.Collapsed;
                    CmdListStackPanel.Visibility = Visibility.Visible;
                    GridInputHeader.Visibility = Visibility.Visible;
                    GridInputSubMessage.Visibility = Visibility.Visible;
                    UpdateCmdListItemsSource();
                    UpdateCmdListPanel();
                    break;
                case "SmDataEntryWithCmdList":
                case "AM_SelectTare":
                    ContainerListStackPanel.Visibility = Visibility.Collapsed;
                    ButtonListStackPanel.Visibility = Visibility.Collapsed;
                    CmdListStackPanel.Visibility = Visibility.Visible;
                    GridInputHeader.Visibility = Visibility.Collapsed;
                    GridInputSubMessage.Visibility = Visibility.Collapsed;
                    UpdateCmdListItemsSource();
                    UpdateCmdListPanel();
                    break;
                default:
                    break;
            }
        }

        private void UpdateSMLine1Text()
        {
            switch (viewModel.StateParam)
            {
                case "SmDataEntryWithCmdList":
                    this.SMLine1Text.Text = GetPropertyStringValue("SMLeadthruText");
                    break;
                case "AM_SelectTare":
                    this.SMLine1Text.Property(TextBlock.TextProperty).SetResourceValue("SelectContainerTitle");
                    break;
                default:
                    this.SMLine1Text.Property(TextBlock.TextProperty).Clear();
                    this.SMLine1Text.Text = String.Empty;
                    break;
            }
        }

        private void UpdateSMLine2Text()
        {
            switch (viewModel.StateParam)
            {
                case "AM_SelectTare":
                    this.SMLine2Text.Property(TextBlock.TextProperty).SetResourceValue("SelectContainerLeadthru");
                    break;
                default:
                    this.SMLine2Text.Property(TextBlock.TextProperty).Clear();
                    this.SMLine2Text.Text = String.Empty;
                    break;
            }
        }

        private void UpdateCmdListPanel()
        {
            switch (viewModel.StateParam)
            {
                case "SmDataEntryWithCmdList":
                case "AM_SelectTare":
                    this.CmdListStackPanel.Style = null;
                    this.CmdListBorder.Margin = new Thickness(24, 0, 24, 0);
                    this.CmdListBorder.Style = this.FindResource("glowBorderStyle") as Style;
                    this.ContainerCmdList.Height = 430;
                    this.InstructionTextPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    this.CmdListStackPanel.Style = this.FindResource("cmdListStackPanelStyle") as Style;
                    this.CmdListBorder.Margin = new Thickness(0, 0, 0, 0);
                    this.CmdListBorder.Style = this.FindResource("containerCmdListBorderStyle") as Style;
                    this.ContainerCmdList.Height = 522;
                    this.InstructionTextPanel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateCmdListItemsSource()
        {
            switch (viewModel.StateParam)
            {
                case "CmDataEntryWithCmdList":
                case "SmDataEntryWithCmdList":;
                    SetCmdListItemSource(viewModel.GetPropertyValue("DataEntryCmdList") as IEnumerable);
                    break;
                case "AM_SelectTare":
                    SetCmdListItemSource(viewModel.GetPropertyValue("DataEntryTareList") as IEnumerable);
                    break;
            }
        }

        private void SetCmdListItemSource(IEnumerable receiptItems)
        {
            Binding receiptBinding = new Binding();
            receiptBinding.Source = receiptItems;
            BindingOperations.SetBinding(this.ContainerCmdList, TouchListBox.ItemsSourceProperty, receiptBinding);
        }

    }
}

