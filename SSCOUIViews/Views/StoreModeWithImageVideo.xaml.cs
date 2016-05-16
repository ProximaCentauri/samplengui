using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using RPSWNET;
using SSCOControls;
using SSCOUIModels;
using SSCOUIModels.Models;
using SSCOUIModels.Helpers;
using SSCOUIModels.Controls;
using FPsxWPF.Controls;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreModeWithImageVideo.xaml
    /// 
    /// This class has handling for store mode contexts "SmDataEntryWithImageControl" 
    /// and "SmDataEntryWithVideoControl"
    /// 
    /// </summary>
    public partial class StoreModeWithImageVideo : BackgroundView
    {
        public StoreModeWithImageVideo(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            Visibility imageCtrlVisibility = Visibility.Visible;

            if (param.Equals("StoreModeWithVideo"))
            {
                imageCtrlVisibility = Visibility.Collapsed;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "StoreModeWithImageVideo => Enable Media Display");
            }
            else
            {                        
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "StoreModeWithImageVideo => Enable Image Display");
            }

            // Set the visibility state of the Image and Video control 
            // based on current state parameter.
            this.ImageControl.Visibility = imageCtrlVisibility;                   
            this.VideoControl.Visibility = (Visibility.Visible == imageCtrlVisibility) ? Visibility.Collapsed : Visibility.Visible;

            UpdateUI_OnStateParam(param);
        }


        private void UpdateUI_OnStateParam(string param)
        {
            UpdateSMLineText();
            UpdateSMLineShown();
        }
        
        public override void OnPropertyChanged(string name, object value)
        {
            switch(name)
            {
                case "DataEntryImgCtrl":
                    if (value != null && value.ToString().Length > 0)
                    {
                        SetCtrlImage(value as String);
                    }
                    break;
                case "DataEntryVdoCtrl":
                    if (value != null && value.ToString().Length > 0)
                    {
                        SetCtrlVideo(value as String);
                    }
                    break;
                case "LeadthruText2Text":
                case "LeadthruText2TextShown":
                case "SMLeadthruText":
                case "SMLeadthruTextShown":
                    UpdateSMLineText();
                    UpdateSMLineShown();
                    break;
            }            
        }

        private void UpdateSMLineShown()
        {
            this.SMLineText.Visibility = !(this.SMLineText.Text.Equals(String.Empty)) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMLineText()
        {
            string line1 = string.Empty;
            string line2 = string.Empty;
            if ((this.GetPropertyBoolValue("SMLeadthruTextShown") && this.GetPropertyStringValue("SMLeadthruText").Length > 0))
            {
                line1 = this.GetPropertyStringValue("SMLeadthruText");
            }
            if ((this.GetPropertyBoolValue("LeadthruText2TextShown") && this.GetPropertyStringValue("LeadthruText2Text").Length > 0))
            {
                line2 = this.GetPropertyStringValue("LeadthruText2Text");
            }
            this.SMLineText.Text = UIFormat.MultiLineFormat(new string[] { line1, line2 });
        }      
        
        /// <summary>
        /// Set the image for the control.
        /// </summary>
        /// <param name="strImagePath"></param>
        private void SetCtrlImage(string strImagePath)
        {
            try
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "SetCtrlImage() : " + strImagePath);
                if (!System.IO.File.Exists(strImagePath))
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "Unable to find the Image File: {0}", strImagePath);
                    return;
                }

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bi.UriSource = new Uri(String.Format(CultureInfo.CurrentCulture, strImagePath), UriKind.Absolute);
                bi.EndInit();
                this.ImageControl.Source = bi;
            }
            catch (Exception ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError,
                            "StoreModeWithImageVideo::SetCtrlImage() - Caught Exception {0} ", ex.Message);
            }
        }

        /// <summary>
        /// Set media source and play it.
        /// </summary>
        /// <param name="strVideoPath"></param>
        private void SetCtrlVideo(string strVideoPath)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "SetCtrlVideo() : " + strVideoPath);
            if (!System.IO.File.Exists(strVideoPath))
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "Unable to find the Media File: {0}", strVideoPath);
                return;
            }

            this.VideoControl.Source = new Uri(String.Format(CultureInfo.CurrentCulture, strVideoPath), UriKind.Absolute);
            PlayVideo();
        }

        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlayVideo();
        }

        private void VideoControl_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskError,
                "StoreModeWithImageVideo::VideoControl_MediaFailed() - Caught Exception {0} {1}",
                e.ToString(), e.ErrorException.Message);
        }

        private void PlayVideo()
        {
            this.VideoControl.Position = TimeSpan.FromSeconds(0);
            this.VideoControl.Play();
        }
    }
}
