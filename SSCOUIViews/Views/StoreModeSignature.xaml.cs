using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Text;
using System.IO;
using SSCOUIModels.Controls;
using SSCOUIModels.Helpers;
using SSCOUIModels;
using SSCOUIModels.Models;
using FPsxWPF.Controls;
using SSCOControls;
using RPSWNET;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for StoreModeSignature.xaml
    /// </summary>
    public partial class StoreModeSignature : BackgroundView
    {
        public StoreModeSignature(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        public override void OnStateParamChanged(string param)
        {
            this.UpdateSMButton1Shown();
            this.UpdateSMButton2Shown();
            this.UpdateSMButton1Enabled();
            this.UpdateSMButton2Enabled();
        }

        public override void OnPropertyChanged(string name, object value)
        {
            switch (name)
            {
                case "SMButton1Shown":
                    this.UpdateSMButton1Shown();
                    break;
                case "SMButton1Enabled":
                    this.UpdateSMButton1Enabled();
                    break;
                case "SMButton2Shown":
                    this.UpdateSMButton2Shown();
                    break;
                case "SMButton2Enabled":
                    this.UpdateSMButton2Enabled();
                    break;
                case "SigCap":
                    DrawSignature();
                    break;
            }
        }

        private void UpdateSMButton1Shown()
        {
            this.StoreButton1.Visibility = (this.GetPropertyBoolValue("SMButton1Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton2Shown()
        {
            this.StoreButton2.Visibility = (this.GetPropertyBoolValue("SMButton2Shown")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSMButton1Enabled()
        {
            this.StoreButton1.IsEnabled = (this.GetPropertyBoolValue("SMButton1Enabled"));
        }

        private void UpdateSMButton2Enabled()
        {
            this.StoreButton2.IsEnabled = (this.GetPropertyBoolValue("SMButton2Enabled"));
        }

        private void DrawSignature()
        {
            SignatureArea.Background = Brushes.White; // this is currently background color of the bitmap
            string hexImage = (String)(viewModel.GetPropertyValue("SigCap")); // SigCap value is encoded in Hex
            if (hexImage.Length > 0)
            {
                try
                {
                    BitmapImage sigImg = new BitmapImage();
                    sigImg.BeginInit();
                    sigImg.CacheOption = BitmapCacheOption.OnLoad;
                    sigImg.StreamSource = new MemoryStream(StringToByteArray(hexImage)); // convert the hex to byte array and assign to streamsource
                    sigImg.EndInit();
                    SignatureImage.Source = sigImg;
                }
                catch
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "StoreModeSignature.DrawSignature() Error displaying signature image.");
                }
            }
        }

        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
