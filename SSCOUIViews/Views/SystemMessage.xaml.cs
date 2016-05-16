using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Forms;
using SSCOUIModels.Controls;
using SSCOUIModels;
using SSCOUIModels.Models;
using SSCOControls;
using RPSWNET;
using PsxNet;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for SystemMessage.xaml
    /// </summary>
    public partial class SystemMessage : BackgroundView
    {
        public SystemMessage(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        /// OnStateParamChanged that accepts param that is set in app.config.
        /// </summary>
        /// <param name="param">String type of param.</param>
        public override void OnStateParamChanged(String param)
        {
            switch (param)
            {
                case "SystemMessageHopperFailure":
                case "SystemMessageHopperSubstitution":
                case "SystemMessageDegradedMode":
                case "SystemMessage":
                case "SystemMessageDegradedModeWithBitmap":
                case "SystemMessageWithBitmap":
                case "SystemMessageWithAVI":
                case "AM_SystemMessageWithAVI":
                case "AM_SystemMessageDegradedModeWithBitmap":
                case "AM_SystemMessageWithBitmap":
                case "AM_SystemMessage":
                case "AM_SystemMessageDegradedMode":
                case "AM_SystemMessageHopperFailure":
                case "AM_SystemMessageHopperSubstitution":
                    this.SystemMessageGrid.Visibility = Visibility.Visible;
                    this.SystemMessageWebGrid.Visibility = Visibility.Collapsed;
                    this.SystemMessageImage.Visibility = Visibility.Collapsed;
                    this.SystemMessageVideo.Visibility = Visibility.Collapsed;
                    this.TitleLine1.Visibility = Visibility.Collapsed;
                    this.TitleLine2.Visibility = Visibility.Collapsed;
                    this.TitleLine3.Visibility = Visibility.Collapsed;
                    this.SystemMessageVideo.Stop();
                    break;

                case "SystemMessageWithWebControl":
                case "SystemMessageDegradedModeWithWebControl":
                case "SystemMessageHopperFailureWithWebControl":
                case "SystemMessageHopperSubstitutionWithWebControl":
                case "AM_SystemMessageWithWebControl":
                case "AM_SystemMessageDegradedModeWithWebControl":
                case "AM_SystemMessageHopperFailureWithWebControl":
                case "AM_SystemMessageHopperSubstitutionWithWebControl":
                    this.SystemMessageWebGrid.Visibility = Visibility.Visible;
                    this.SystemMessageGrid.Visibility = Visibility.Collapsed;
                    break;

                default:
                    this.SystemMessageWebGrid.Visibility = Visibility.Collapsed;
                    this.SystemMessageGrid.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SystemMessageVideo_Ended(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void PlayVideo()
        {
            this.SystemMessageVideo.Position = TimeSpan.FromSeconds(0);
            this.SystemMessageVideo.Play();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "DeviceErrorWebControl":
                    if (viewModel.StateParam.Contains("WithWebControl"))
                    {
                        if (!String.IsNullOrEmpty(value.ToString()))
                        {
                            try
                            {
                                this.SystemMessageWebControl.Navigate(new Uri(String.Format(CultureInfo.CurrentCulture, value.ToString()), UriKind.Absolute));
                            }
                            catch (Exception e)
                            {
                                CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "SystemMessage::OnPropertyChanged DeviceErrorWebControl - Caught exception {0}", e.Message);
                            }
                        }
                    }
                    break;
                case "DeviceErrorImage":
                    if (viewModel.StateParam.Contains("WithBitmap"))
                    {
                        if (!String.IsNullOrEmpty(value.ToString()))
                        {
                            try
                            {
                                BitmapImage image = new BitmapImage();
                                image.BeginInit();
                                image.UriSource = new Uri(String.Format(CultureInfo.CurrentCulture, value.ToString()), UriKind.Absolute);
                                image.EndInit();
                                this.SystemMessageImage.Source = image;
                                this.SystemMessageImage.Visibility = Visibility.Visible;
                                this.SystemMessageVideo.Visibility = Visibility.Collapsed;
                            }
                            catch (Exception e)
                            {
                                CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "SystemMessage::OnPropertyChanged DeviceErrorImage - Caught exception {0}", e.Message);
                            }
                        }
                    }
                    break;
                case "DeviceErrorVideo":
                    if (viewModel.StateParam.Contains("WithAVI"))
                    {
                        if (!String.IsNullOrEmpty(value.ToString()))
                        {
                            try
                            {
                                this.SystemMessageVideo.Source = new Uri(String.Format(CultureInfo.CurrentCulture, value.ToString()), UriKind.Absolute);
                                this.SystemMessageImage.Visibility = Visibility.Collapsed;
                                this.SystemMessageVideo.Visibility = Visibility.Visible;
                                PlayVideo();
                            }
                            catch (Exception e)
                            {
                                CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "SystemMessage::OnPropertyChanged DeviceErrorVideo - Caught exception {0}", e.Message);
                            }
                        }
                    }
                    break;
                case "CMButton1MedShown":
                    this.SystemMessageButton1.Style = this.FindResource(this.GetPropertyBoolValue("CMButton2MedShown") ? "systemMessageButtonStyle" : "systemMessageButtonWithCheckStyle") as Style;
                    break;
                case "CMButton2MedShown":
                    this.SystemMessageButton1.Style = this.FindResource(this.GetPropertyBoolValue(name) ? "systemMessageButtonStyle" : "systemMessageButtonWithCheckStyle") as Style;
                    break;
                case "Title2Area":
                    if (!String.IsNullOrEmpty(GetPropertyStringValue(name)))
                    {
                        this.TitleLine1.Text = GetPropertyStringValue(name);
                        this.TitleLine1.Visibility = Visibility.Visible;
                    }
                    break;
                case "InstructionScreenTitle":
                    if (!String.IsNullOrEmpty(GetPropertyStringValue(name)))
                    {
                        this.TitleLine2.Text = GetPropertyStringValue(name);
                        this.TitleLine2.Visibility = Visibility.Visible;
                    }
                    break;
                case "TitleText":
                    if (!String.IsNullOrEmpty(GetPropertyStringValue(name)))
                    {
                        this.TitleLine3.Text = GetPropertyStringValue(name);
                        this.TitleLine3.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void SystemMessage_VideoFailed(object sender, ExceptionRoutedEventArgs e)
        {
            CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "SystemMessage::OnPropertyChanged DeviceErrorVideo - Caught exception {0} {1}", e.ToString(), e.ErrorException.Message);
        }
    }
}
