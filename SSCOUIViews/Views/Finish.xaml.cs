//-----------------------------------------------------------------------
// <copyright file="Finish.xaml.cs" company="NCR Corporation">
//     Copyright (c) NCR Corporation. All rights reserved.
// </copyright>
// <summary>This is the Finish class.</summary>
//-----------------------------------------------------------------------

namespace SSCOUIViews.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using SSCOUIModels;
    using SSCOUIModels.Controls;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;
    using System.Globalization;
    using SSCOControls;
    using SSCOUIModels.Helpers;
    using RPSWNET;
    
    /// <summary>
    /// Interaction logic for Finish.xaml.
    /// </summary>
    public partial class Finish : BackgroundView
    {
        /// <summary>
        /// Initializes a new instance of the Finish class.        
        /// </summary>
        /// <param name="viewModel">ViewModel type of parameter.</param>
        public Finish(IMainViewModel viewModel)
            : base(viewModel)
        {
            this.InitializeComponent();
            LoadFinishImage();
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(string param)
        {
            switch (param)
            {
                case "TakeChange":
                case "TakeChangeTimeOut":
                    this.HeaderText.Visibility = Visibility.Visible;
                    this.HeaderText.Property(TextBlock.TextProperty).SetResourceValue("TakeChange");
                    this.InstructionTextArea.Text = GetPropertyStringValue("Instructions");
                    this.PrintReceiptButton.Visibility = Visibility.Collapsed;
                    break;
                case "TakeReceipt":
                    this.HeaderText.Visibility = Visibility.Collapsed;
                    this.InstructionTextArea.Property(TextBlock.TextProperty).SetResourceValue("TakeReceipt");
                    this.PrintReceiptButton.Visibility = Visibility.Collapsed;
                    break;
                case "Finish":
                    this.HeaderText.Visibility = Visibility.Visible;
                    this.HeaderText.Property(TextBlock.TextProperty).SetResourceValue("Finish");
                    this.InstructionTextArea.Text = GetPropertyStringValue("Instructions");
                    this.PrintReceiptButton.Visibility = (bool)viewModel.GetPropertyValue("CMButton1MedShown") ? Visibility.Visible : Visibility.Collapsed;
                    break;
            }           
        }

        public override void OnPropertyChanged(string name, object value)
        {
            if (name.Equals("Instructions"))
            {
                OnStateParamChanged(viewModel.StateParam);
            }
            if (name.Equals("CMButton1MedShown"))
            {
                OnStateParamChanged(viewModel.StateParam);
            }
            else if (name.Equals("AmountDue"))
            {
                AmountDue.Text = value.ToString();
            }
            else if (name.Equals("BackgroundStateParam") && viewModel.StateParam.Equals("Finish"))
            {
                OnStateParamChanged(viewModel.StateParam);
            }
        }

        /// <summary>
        /// Loads the Finish image if any.
        /// </summary>
        /// <param name="imagePath"></param>
        private void LoadFinishImage()
        {
            try
            {
                string imagePath = ItemImageConverter.GetScotDirectory() + "\\image\\Finish.png";
                if (!System.IO.File.Exists(imagePath))
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo,
                        "SSCOUIViews.Views.Finish.LoadFinishImage() - No finish image found: {0}", imagePath);
                    return;
                }
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bi.UriSource = new Uri(String.Format(CultureInfo.CurrentCulture, imagePath), UriKind.Relative);
                bi.EndInit();
                this.FinishImage.Source = bi;
            }
            catch (Exception ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError,
                    "SSCOUIViews.Views.Finish.LoadFinishImage() - Caught Exception {0} ", ex.Message);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AmountDue.Text = GetPropertyStringValue("AmountDue");
        }
    }
}
