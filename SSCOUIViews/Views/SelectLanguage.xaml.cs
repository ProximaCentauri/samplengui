// <copyright file="SelectLanguage.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
namespace SSCOUIViews.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using SSCOUIModels.Controls;
    using SSCOUIModels.Models;
    using SSCOUIModels;
    using System.Collections.ObjectModel;
    using FPsxWPF.Controls;
    using SSCOControls;
    using System.Collections;
    using System.Globalization;

    /// <summary>
    /// Interaction logic for SelectLanguage.xaml
    /// </summary>
    public partial class SelectLanguage : PopupView
    {
        /// <summary>
        /// Initializes a new instance of the SelectLanguage class.
        /// </summary>
        public SelectLanguage(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        /// PopupView Loaded() that refreshes selectLanguage popup's curent language
        /// </summary>
        /// <param name="sender">This is a parameter with a type of object</param>
        /// <param name="e">This is a parameter with a type of RoutedEventArgs</param>
        private void PopupView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (GridItem item in this.LanguageOptions.Items)
            {
                string[] languageDataArray = item.Data.ToString().Split(';');
                if (languageDataArray.Length > 1)
                {
                    int languageData = int.Parse(languageDataArray[1], System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    if (languageData == this.viewModel.CustomerLanguage)
                    {
                        int index = this.LanguageOptions.Items.IndexOf(item);
                        this.LanguageOptions.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// SelectLanguage_Click() executed when a language button is clicked
        /// inside the LanguageOptions TouchListBox
        /// <param name="sender">object sender</param>
        /// <param name="e"> RoutedEventArgs e</param>
        /// </summary>
        private void SelectLanguage_Click(object sender, RoutedEventArgs e)
        {
            GridItem item = this.LanguageOptions.SelectedItem as GridItem;
            if(null != item)
            {
                if (item.IsEnabled && item.IsVisible)
                {
                    this.viewModel.ActionCommand.Execute(String.Format(CultureInfo.InvariantCulture, "Languages({0})", LanguageOptions.SelectedIndex));
                }
            }
        }
    }
}
