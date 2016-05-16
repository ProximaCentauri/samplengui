// <copyright file="Welcome.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
namespace SSCOUIViews.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using SSCOUIModels;
    using SSCOUIModels.Controls;
    using FPsxWPF.Controls;    
    using SSCOUIModels.Helpers;
    using RPSWNET;

    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : BackgroundView
    {
        /// <summary>
        /// Initializes a new instance of the Welcome class        
        /// </summary>
        /// <param name="viewModel">viewModel type of parameter</param>
        public Welcome(IMainViewModel viewModel)
            : base(viewModel)
        {
            this.InitializeComponent();
            LoadWelcomeImage();
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(String param)
        {
            if (param.Equals("Welcome"))
            {
                AttractLanguageStateChange();
            }
            else if (param.Equals("AttractMultiLanguage"))
            {
                this.AttractMultiLanguageStateChange();
            }
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("Language"))
            {
                AttractLanguageStateChange();
            }
        }

        /// <summary>
        /// Attract MultiLanguage State Change Method.
        /// </summary>
        private void AttractMultiLanguageStateChange()
        {
            this.languageCollection = viewModel.GetPropertyValue("Languages") as ObservableCollection<GridItem>;
            int index = 0;
            if (this.languageCollection.Count > 0)
            {
                foreach (GridItem item in languageCollection)
                {
                    int LanguageData = int.Parse(item.Data.ToString().Split(';')[1], System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                    if (LanguageData == viewModel.Language)
                    {
                        index = languageCollection.IndexOf(item);
                        break;
                    }
                }
            }
            InstructionBox.CommandParameter = String.Format("CMButton{0}MidiList", index + 1);
            SearchKeyInItemButton.CommandParameter = String.Format("SearchKeyInItem({0})", index + 1);
        }

        /// <summary>
        /// Attract Single and Dual Language State Change Method.
        /// </summary>
        private void AttractLanguageStateChange()
        {
            if (viewModel.StateParam.Equals("Welcome"))
            {
                InstructionBox.CommandParameter = "Scan";
                SearchKeyInItemButton.CommandParameter = "SearchKeyInItem";
                if (!GetPropertyBoolValue("LanguageWelcomeShown"))
                {
                    this.languageCollection = viewModel.GetPropertyValue("Languages") as ObservableCollection<GridItem>;
                    int language = viewModel.Language;
                    if (this.languageCollection != null
                        && this.languageCollection.Count == 2 && language != 0)
                    {
                        string langSelected = language.ToString("X", CultureInfo.CurrentCulture);
                        string primeLang = (null != this.languageCollection[0].Data) ? this.languageCollection[0].Data.Split(';')[1] : language.ToString("X", CultureInfo.CurrentCulture);

                        if (!primeLang.Contains(langSelected))
                        {
                            InstructionBox.CommandParameter = "ScanForLanguage2";
                            SearchKeyInItemButton.CommandParameter = "SearchKeyInItemForLanguage2";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the Welcome image if any.
        /// </summary>
        /// <param name="imagePath"></param>
        private void LoadWelcomeImage()
        {
            try
            {
                string imagePath = ItemImageConverter.GetScotDirectory() + "\\image\\Welcome.png";
                if (!System.IO.File.Exists(imagePath))
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo,
                        "SSCOUIViews.Views.Welcome.LoadWelcomeImage() - No welcome image found: {0}", imagePath);
                    return;
                }
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bi.UriSource = new Uri(String.Format(CultureInfo.CurrentCulture, imagePath), UriKind.Relative);
                bi.EndInit();
                this.WelcomeImage.Source = bi;
            }
            catch (Exception ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError,
                    "SSCOUIViews.Views.Welcome.LoadWelcomeImage() - Caught Exception {0} ", ex.Message);
            }
        }

        /// <summary>
        /// languageCollection variable
        /// </summary>
        private ObservableCollection<GridItem> languageCollection;
    }
}
