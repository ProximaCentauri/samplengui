// <copyright file="SystemFunctionsControl.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
namespace SSCOUIViews.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using SSCOUIModels;
    using SSCOUIModels.Models;
    using SSCOControls;
    using FPsxWPF.Controls;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SystemFunctionsControl.xaml
    /// </summary>
    public partial class SystemFunctionsControl : Grid
    {
        /// <summary>
        /// Initializes a new instance of the SystemFunctionsControl class.
        /// </summary>
        public SystemFunctionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// List variable
        /// </summary>
        private static List<string> paramListAssistanceButton = new List<string>() { "Scan", "QuickPick", "ScanWithReward", "QuickPickWithReward", "SecMisMatchWeight", "SecSkipBaggingThreshold" };

        /// <summary>
        /// List variable
        /// </summary>
        private static readonly List<string> paramListForButtonStoreLogin = new List<string>() { "InsertCoupon", "Bag", "InsertGiftCard", "TakeChange" };

        /// <summary>
        /// Grid DataContextChanged
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of DependencyPropertyChangedEventArgs</param> 
        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.viewModel != null)
            {
                this.viewModel.PropertyChanged -= new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);
                (this.viewModel.GetPropertyValue("Languages") as ObservableCollection<GridItem>).CollectionChanged -= languageCollection_CollectionChanged;
            }

            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);

            this.RefreshLanguageEventHandlers();
            this.RefreshLanguages();
            this.RefreshLanguageState();
            this.RefreshLanguageText();
            this.RefreshOwnBagState();
            this.RefreshVolumeState();
            this.OnRefreshWeight();
            this.RefreshAssistanceState();
            this.RefreshScale();
            this.UpdateWeightTextFlowDirection();
        }

        /// <summary>
        /// Language collection events.
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of NotifyCollectionChangedEventArgs</param> 
        private void languageCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RefreshLanguages();
                RefreshLanguageState();
                RefreshLanguageText();
                RefreshOwnBagState();
                UpdateWeightTextFlowDirection();
            }
        }

        /// <summary>
        /// Refresh language collection.
        /// </summary>
        private void RefreshLanguages()
        {
            if (null != this.languageCollection)
            {
                this.languages = this.languageCollection.Count;
            }
            else
            {
                this.languages = 0;
            }
        }

        /// <summary>
        /// Refresh language state.
        /// </summary>
        private void RefreshLanguageState()
        {
            if (viewModel.GetPropertyValue("NextGenData").ToString().Equals("BagAndEAS"))
            {
                LanguageButton.IsEnabled = false;
            }
            else
            {
                if (viewModel.StateParam.Equals("Welcome")
                    || viewModel.StateParam.Equals("AttractMultiLanguage"))
                {
                    LanguageButton.IsEnabled = (this.languages > 1);
                }
                else
                {
                    LanguageButton.IsEnabled = (bool)viewModel.GetPropertyValue("CMButton5MidiListShown");
                }

                if (this.languages < 2 && this.languages != 2)
                {
                    this.LanguageButton.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Refresh language text.
        /// </summary>
        private void RefreshLanguageText()
        {
            if (2 < this.languages)
            {
                this.LanguageButton.Property(ImageButton.TextProperty).SetResourceValue("Language");
            }
            else if (2 == this.languages)
            {
                if (this.viewModel.Language != 0)
                {
                    this.LanguageButton.Property(ImageButton.TextProperty).Clear();
                    string langSelected = this.viewModel.CustomerLanguage.ToString("X", CultureInfo.CurrentCulture);
                    string primeLang = langSelected;
                    if (null != this.languageCollection[0].Data)
                    {
                        string[] splitData = this.languageCollection[0].Data.Split(';');
                        if (splitData.Length > 1)
                        {
                            primeLang = splitData[1];
                        }
                    }
                    if (primeLang.ToLower().Contains(langSelected.ToLower()) && null != this.languageCollection[1].Text)
                    {
                        this.LanguageButton.Text = this.languageCollection[1].Text;                        
                    }
                    else if (null != this.languageCollection[0].Text)
                    {
                        this.LanguageButton.Text = this.languageCollection[0].Text;
                    }
                    else
                    {
                        this.LanguageButton.Property(ImageButton.TextProperty).SetResourceValue("Language");                        
                    }
                }
                else
                {
                    if (null != viewModel.GetPropertyValue("Language"))
                    {
                        this.LanguageButton.Text = viewModel.GetPropertyValue("Language").ToString();
                    }
                    else
                    {
                        this.LanguageButton.Property(ImageButton.TextProperty).SetResourceValue("Language");
                    }
                }
            }
            else
            {
                this.LanguageButton.Property(ImageButton.TextProperty).SetResourceValue("Language");
            }
        }

        /// <summary>
        /// Refresh bag state
        /// </summary>
        private void RefreshOwnBagState()
        {
            if (viewModel.GetPropertyValue("NextGenData").ToString().Equals("BagAndEAS"))
            {
                OwnBagButton.IsEnabled = false;
            }
            else
            {
                if (viewModel.State.Equals("OutOfTransaction"))
                {
                    OwnBagButton.IsEnabled = (2 != this.languages || (bool)viewModel.GetPropertyValue("LanguageWelcomeShown")) ?
                        (bool)viewModel.GetPropertyValue("OwnBagWelcomeShown") : false;
                }
                else
                {
                    OwnBagButton.IsEnabled = (bool)viewModel.GetPropertyValue("OwnBagSaleShown");
                }
            }
        }

        /// <summary>
        /// Refresh volume state
        /// </summary>
        private void RefreshVolumeState()
        {
            if (viewModel.GetPropertyValue("NextGenData").ToString().Equals("BagAndEAS"))
            {
                VolumeButton.IsEnabled = false;
            }
            else
            {
                VolumeButton.IsEnabled = (bool)viewModel.GetPropertyValue("VolumeShown");
            }
        }

        /// <summary>
        /// Refresh assistance state
        /// </summary>
        private void RefreshAssistanceState()
        {
            bool isEnable = false;

            if (viewModel.GetPropertyValue("NextGenData").ToString().Equals("BagAndEAS"))
            {
                if (paramListAssistanceButton.Contains(viewModel.StateParam))
                {
                    isEnable = false;
                }
                else
                {
                    if (viewModel.BackgroundStateParam.Equals("Bag"))
                    {
                        isEnable = (bool)viewModel.GetPropertyValue("ButtonStoreLogInShown");
                    }
                    else if (paramListAssistanceButton.Contains(viewModel.StateParam))
                    {
                        isEnable = (bool)viewModel.GetPropertyValue("AssistanceShown");
                    }
                }
            }
            else
            {
                if (viewModel.BackgroundStateParam.Equals("TakeChange"))
                {
                    if (viewModel.StateParam.Equals("Payment"))
                    {
                        isEnable = (bool)viewModel.GetPropertyValue("AssistanceShown");
                    }
                }                
                else if (viewModel.StateParam.Equals("CashPayment") || viewModel.StateParam.Equals("Finish"))
                {
                    isEnable = (bool)viewModel.GetPropertyValue("AssistanceEnabled");
                }
                else if (paramListForButtonStoreLogin.Contains(viewModel.StateParam))
                {
                    isEnable = (bool)viewModel.GetPropertyValue("ButtonStoreLogInShown");
                }
                else if ((bool)viewModel.GetPropertyValue("AssistanceShown"))
                {
                    if (!viewModel.StateParam.Equals("Disconnected"))
                    {
                        isEnable = true;
                    }
                }
            }
            AssistanceButton.IsEnabled = isEnable;
        }

        /// <summary>
        /// RefreshLanguageEventHandlers
        /// adds an event handler to languageCollection but remove first existing event handler
        /// </summary>
        private void RefreshLanguageEventHandlers()
        {
            if (null != this.languageCollection)
            {
                this.languageCollection.CollectionChanged -= languageCollection_CollectionChanged;
            }
            this.languageCollection = viewModel.GetPropertyValue("Languages") as ObservableCollection<GridItem>;
            if (null != this.languageCollection)
            {
                this.languageCollection.CollectionChanged += languageCollection_CollectionChanged;
            }
        }

        /// <summary> 
        /// ViewModel PropertyChanged 
        /// </summary> 
        /// <param name="sender">This is a parameter with a type of object</param> 
        /// <param name="e">This is a parameter with a type of PropertyChangedEventArgs</param> 
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Language":
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.UpdateWeightTextFlowDirection();
                    break;
                case "Languages":
                    this.RefreshLanguageEventHandlers();
                    this.RefreshLanguages();
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.RefreshOwnBagState();
                    this.UpdateWeightTextFlowDirection();
                    break;
                case "CMButton5MidiListShown":
                    this.RefreshLanguages();
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.UpdateWeightTextFlowDirection();
                    break;
                case "OwnBagSaleShown":
                case "OwnBagWelcomeShown":
                case "LanguageWelcomeShown":
                    this.RefreshOwnBagState();
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.UpdateWeightTextFlowDirection();
                    break;
                case "VolumeShown":
                    this.RefreshVolumeState();
                    break;
                case "State":
                    this.RefreshOwnBagState();
                    break;
                case "StoreMode":
                    this.UpdateWeightDetailsPanel();
                    this.UpdateSMButton8StoreMode();
                    this.UpdateStoreButtonGoBack();
                    break;
                case "Weight":
                    this.OnRefreshWeight();
                    break;
                case "ButtonStoreLogInShown":
                case "AssistanceEnabled":
                case "AssistanceShown":
                    this.RefreshAssistanceState();
                    break;
                case "StateParam":
                    this.UpdateWeightDetailsPanel();
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.RefreshAssistanceState();
                    this.OnRefreshWeight();
                    this.RefreshScale();
                    this.UpdateWeightTextFlowDirection();
                    this.UpdateSMButton8StoreMode();
                    this.UpdateStoreButtonGoBack();
                    break;
                case "NextGenData":
                    this.RefreshAssistanceState();
                    this.RefreshVolumeState();
                    this.RefreshOwnBagState();
                    this.RefreshLanguageState();
                    this.RefreshLanguageText();
                    this.UpdateWeightTextFlowDirection();
                    break;
                case "ScaleLogo":
                case "ScaleInfo":
                case "SMScaleImage":
                    this.RefreshScale();
                    break;
                case "SMButton8Text":
                case "SMButton8Shown":
                case "SMButton8Enabled":
                case "ButtonGoBack":
                case "ButtonGoBackEnabled":
                case "ButtonGoBackShown":
                case "XMButton8Text":
                case "XMButton8Enabled":
                case "XMButton8Shown":
                case "CmdBtn_GoBackText":
                case "CmdBtn_GoBackEnabled":
                case "CmdBtn_GoBackShown":
                    this.UpdateStoreButtonGoBack();
                    break;
                case "BackgroundStateParam":
                    this.RefreshAssistanceState();
                    break;
            }
        }

        private void UpdateSMButton8StoreMode()
        {
            if (this.viewModel.StoreMode)
            {
                this.AssistanceButton.Visibility = this.LanguageButton.Visibility = this.OwnBagButton.Visibility = this.VolumeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.AssistanceButton.Visibility = this.LanguageButton.Visibility = this.OwnBagButton.Visibility = this.VolumeButton.Visibility = Visibility.Visible;
                this.StoreButton8.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateWeightDetailsPanel()
        {
            if (this.viewModel.StateParam.Contains("SystemMessage") && this.viewModel.StoreMode)
            {
                this.WeightDetailsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.WeightDetailsPanel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateSMButton1Text()
        {
            this.StoreButton8.Text = this.viewModel.GetPropertyValue("SMButton1Text").ToString();
        }

        private void UpdateSMButton1Shown()
        {
            this.StoreButton8.Visibility = ((bool)this.viewModel.GetPropertyValue("SMButton1Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton1Enabled()
        {
            this.StoreButton8.IsEnabled = ((bool)this.viewModel.GetPropertyValue("SMButton1Enabled"));
        }

        private void UpdateSMButton8Text()
        {
            this.StoreButton8.Text = this.viewModel.GetPropertyValue("SMButton8Text").ToString();
        }

        private void UpdateSMButton8Shown()
        {
            this.StoreButton8.Visibility = ((bool)this.viewModel.GetPropertyValue("SMButton8Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton8Enabled()
        {
            if (this.viewModel.StateParam.Equals("EchoPopup") && this.viewModel.BackgroundStateParam.Equals("SmSystemFunctions"))
            {
                this.StoreButton8.IsEnabled = false;
            }
            else
            {
                this.StoreButton8.IsEnabled = ((bool)this.viewModel.GetPropertyValue("SMButton8Enabled"));
            }
        }

        private void UpdateButtonGoBackText()
        {
            this.StoreButton8.Text = this.viewModel.GetPropertyValue("ButtonGoBack").ToString();
        }

        private void UpdateButtonGoBackShown()
        {
            this.StoreButton8.Visibility = ((bool)this.viewModel.GetPropertyValue("ButtonGoBackShown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateButtonGoBackEnabled()
        {
            this.StoreButton8.IsEnabled = ((bool)this.viewModel.GetPropertyValue("ButtonGoBackEnabled")) ? true : false;
        }

        private void UpdateXMButton8Text()
        {
            this.StoreButton8.Text = this.viewModel.GetPropertyValue("XMButton8Text").ToString();
        }

        private void UpdateXMButton8Shown()
        {
            this.StoreButton8.Visibility = ((bool)this.viewModel.GetPropertyValue("XMButton8Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateXMButton8Enabled()
        {
            this.StoreButton8.IsEnabled = ((bool)this.viewModel.GetPropertyValue("XMButton8Enabled")) ? true : false;
        }

        private void UpdateCmdBtn_GoBackText()
        {
            this.StoreButton8.Text = this.viewModel.GetPropertyValue("CmdBtn_GoBackText").ToString();
        }

        private void UpdateCmdBtn_GoBackShown()
        {
            this.StoreButton8.Visibility = ((bool)this.viewModel.GetPropertyValue("CmdBtn_GoBackShown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateCmdBtn_GoBackEnabled()
        {
            this.StoreButton8.IsEnabled = ((bool)this.viewModel.GetPropertyValue("CmdBtn_GoBackEnabled")) ? true : false;
        }

        /// <summary>
        /// OnRefreshWeight method
        /// </summary>
        private void OnRefreshWeight()
        {
            if (this.viewModel.StateParam.Equals("Disconnected")
                || (this.viewModel.State.Equals("Loading") && this.viewModel.StateParam.Equals("Startup")))
            {
                WeightTextBlock.Text = String.Empty;
            }
            else
            {
                string weight = this.viewModel.GetPropertyValue("Weight").ToString();

                WeightTextBlock.Text = weight;
            }
        }

        /// <summary>
        /// Update weight text flow direction
        /// </summary>
        private void UpdateWeightTextFlowDirection()
        {
            this.cultureInfo = new CultureInfo(this.viewModel.Language);

            if (this.cultureInfo.TextInfo.IsRightToLeft)
            {
                WeightTextBlock.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                WeightTextBlock.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        /// <summary> 
        /// Refresh Scale 
        /// </summary> 
        private void RefreshScale()
        {
            if (this.viewModel.StateParam.Equals("Disconnected")
                || (this.viewModel.State.Equals("Loading") && this.viewModel.StateParam.Equals("Startup")))
            {
                ScaleLogo.Visibility = Visibility.Collapsed;
                ScaleInfo.Text = String.Empty;
            }
            else
            {
                ScaleLogo.Visibility = Convert.ToBoolean(this.viewModel.GetPropertyValue("ScaleLogo")) || Convert.ToBoolean(this.viewModel.GetPropertyValue("SMScaleImage")) 
                    ? Visibility.Visible : Visibility.Collapsed;
                ScaleInfo.Text = this.viewModel.GetPropertyValue("ScaleInfo").ToString();
            }
        }

        /// <summary>
        /// viewModel variable
        /// </summary>
        private IMainViewModel viewModel;

        /// <summary>
        /// languageCollection variable
        /// </summary>
        private ObservableCollection<GridItem> languageCollection;

        /// <summary>
        /// languages variable
        /// </summary>
        private int languages;

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionCommand.Execute("ViewModelSet(Context;ShowVolume)");
        }

        private CultureInfo cultureInfo;

        private void StoreButton8_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_EnterWeight":
                case "AM_EnterWtForPriceEmbedded":
                case "AM_ScaleBroken":
                case "AM_SelectTare":
                case "AM_VoidItem":
                case "SystemMessageDegradedModeWithBitmap":
                case "SystemMessageWithBitmap":
                case "AM_SystemMessageDegradedModeWithBitmap":
                case "AM_SystemMessageWithBitmap":
                case "SystemMessageWithWebControl":
                case "SystemMessageDegradedModeWithWebControl":
                case "SystemMessageHopperFailureWithWebControl":
                case "SystemMessageHopperSubstitutionWithWebControl":
                case "AM_SystemMessageWithWebControl":
                case "AM_SystemMessageDegradedModeWithWebControl":
                case "AM_SystemMessageHopperFailureWithWebControl":
                case "AM_SystemMessageHopperSubstitutionWithWebControl":
                case "SystemMessageWithAVI":
                case "AM_SystemMessageWithAVI":
                case "SystemMessageHopperFailure":
                case "SystemMessageHopperSubstitution":
                case "SystemMessageDegradedMode":
                case "SystemMessage":
                case "AM_SystemMessage":
                case "AM_SystemMessageDegradedMode":
                case "AM_SystemMessageHopperFailure":
                case "AM_SystemMessageHopperSubstitution":
                case "AM_EnterCouponValue":
                case "AM_EnterQuantity":
                case "AM_KeyInCode":
                    viewModel.ActionCommand.Execute("ButtonGoBack");
                    break;
                case "XMMediaStatus":
                    viewModel.ActionCommand.Execute("XMButton8");
                    break;
                case "SmAuthorizationHC":
                case "SmAssistMenuHC":
                case "SmAssistMenuFinalizeHC":
                case "SmAssistMenuFinalize":
                    viewModel.ActionCommand.Execute(String.Format("CmdBtn_GoBack({0})", "OnReturnToShopping"));
                    break;
                case "SmAssistMode":
                    viewModel.ActionCommand.Execute("SMButton1");
                    break;
                default:
                    viewModel.ActionCommand.Execute("SMButton8");
                    break;
            }
        }

        private void UpdateStoreButtonGoBack()
        {
            switch (this.viewModel.StateParam)
            {
                case "AM_EnterWeight":
                case "AM_EnterWtForPriceEmbedded":
                case "AM_ScaleBroken":
                case "AM_SelectTare":
                case "AM_VoidItem":
                case "AM_EnterCouponValue":
                case "AM_EnterQuantity":
                case "AM_KeyInCode":
                case "SystemMessageDegradedModeWithBitmap":
                case "SystemMessageWithBitmap":
                case "AM_SystemMessageDegradedModeWithBitmap":
                case "AM_SystemMessageWithBitmap":
                case "SystemMessageWithWebControl":
                case "SystemMessageDegradedModeWithWebControl":
                case "SystemMessageHopperFailureWithWebControl":
                case "SystemMessageHopperSubstitutionWithWebControl":
                case "AM_SystemMessageWithWebControl":
                case "AM_SystemMessageDegradedModeWithWebControl":
                case "AM_SystemMessageHopperFailureWithWebControl":
                case "AM_SystemMessageHopperSubstitutionWithWebControl":
                case "SystemMessageWithAVI":
                case "AM_SystemMessageWithAVI":
                case "SystemMessageHopperFailure":
                case "SystemMessageHopperSubstitution":
                case "SystemMessageDegradedMode":
                case "SystemMessage":
                case "AM_SystemMessage":
                case "AM_SystemMessageDegradedMode":
                case "AM_SystemMessageHopperFailure":
                case "AM_SystemMessageHopperSubstitution":
                    this.UpdateButtonGoBackShown();
                    this.UpdateButtonGoBackEnabled();
                    this.UpdateButtonGoBackText();
                    break;
                case "XMMediaStatus":
                    this.UpdateXMButton8Shown();
                    this.UpdateXMButton8Enabled();
                    this.UpdateXMButton8Text();
                    break;
                case "SmAuthorizationHC":
                case "SmAssistMenuHC":
                case "SmAssistMenuFinalizeHC":
                case "SmAssistMenuFinalize":
                    this.UpdateCmdBtn_GoBackShown();
                    this.UpdateCmdBtn_GoBackEnabled();
                    this.UpdateCmdBtn_GoBackText();
                    break;
                case "SmAssistMode":
                    this.UpdateSMButton1Shown();
                    this.UpdateSMButton1Enabled();
                    this.UpdateSMButton1Text();
                    break;
                case "Disconnected":
                    this.StoreButton8.Visibility = Visibility.Collapsed;
                    break;
                default:
                    this.UpdateSMButton8Shown();
                    this.UpdateSMButton8Enabled();
                    this.UpdateSMButton8Text();
                    break;
            }
        }
    }
}
