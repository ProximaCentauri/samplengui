using System;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.IO;
using RPSWNET;

namespace SSCOUIModels.Helpers
{
    public class ItemImageConverter : IValueConverter
    {
        private const char DataValueSeparator = '@';
        private const string AppDrive = "APP_DRIVE";
        private const string AppDir = "APP_DIR";
        private static string scotDir = string.Empty;
        private readonly string[] quickPickListImageExtensions = { ".jpg", ".png" };
        private readonly string[] defaultImageExtensions = { ".jpg", ".png" };
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var cacheItemImages = CacheItemImages.Instance;
            string imagePanel = null == parameter ? string.Empty : parameter.ToString();
            string imageValue = null == value ? string.Empty : value.ToString();
            string imageSource;
            BitmapImage image = null;

            if (string.IsNullOrEmpty(scotDir))
            {
                scotDir = GetScotDirectory();
            }
            imageSource = scotDir + "\\image\\";

            switch (imagePanel)
            {
                case "QuickPick":
                    imageValue = GetItemCode(imageValue);
                    imageSource += "Items\\" + imagePanel + "\\";
                    image = getCacheImage(cacheItemImages,cacheItemImages.QuickPickItemImages, imageSource, imageValue, quickPickListImageExtensions);
                    break;
                case "PickList":
                    imageValue = GetItemCode(imageValue);
                    imageSource += "Items\\" + imagePanel + "\\";
                    image = getCacheImage(cacheItemImages, cacheItemImages.PicklistItemImages, imageSource, imageValue, quickPickListImageExtensions);
                    break;
                case "Card":
                    imageSource += "Items\\";
                    image = getCacheImage(cacheItemImages, cacheItemImages.CardItemImages, imageSource, imageValue, defaultImageExtensions);                    
                    break;
                case "Containers":
                    imageSource += imagePanel + "\\";
                    imageValue = Path.GetFileNameWithoutExtension(imageValue);
                    image = getCacheImage(cacheItemImages, cacheItemImages.ContainersItemImages, imageSource, imageValue, defaultImageExtensions);                    
                    break;
                default:
                    image = getCacheImage(cacheItemImages, null, imageSource, imageValue, null);
                    break;
            }
            
            return image;
        }
        
        public static string GetScotDirectory()
        {
            string appDriveValue = Environment.GetEnvironmentVariable(AppDrive, EnvironmentVariableTarget.Machine);
            string appDirValue = Environment.GetEnvironmentVariable(AppDir, EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(appDriveValue))
            {
                appDriveValue = "C:";
            }
            if (string.IsNullOrEmpty(appDirValue))
            {
                appDirValue = "scot";
            }
            return scotDir = appDriveValue + "\\" + appDirValue;
        }

        private BitmapImage getCacheImage(CacheItemImages cacheItemImages, Dictionary<string, BitmapImage> images, string imageSource, string imageValue, string[] extensions)
        {
            if (null != images)
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                BitmapImage image = null;
                if (images.ContainsKey(imageValue))
                {   
                    image = images[imageValue];
                }
                else
                {
                    image = cacheItemImages.addCacheImage(images, imageSource + imageValue, extensions);
                }                
                TimeSpan span = sw.Elapsed;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "Time:{0} ms to load image:{1}", span.Milliseconds, imageValue);
                return image;
            }
            else
            {
                return cacheItemImages.createBitmapImage(imageValue);
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        private string GetItemCode(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string[] itemDetails = data.Split(DataValueSeparator);
                if (itemDetails.Length >= 7)
                {
                    if (itemDetails[6].Equals("False") || itemDetails[4].Equals("True"))
                    {
                        return itemDetails[0] + "_cat";
                    }
                }                
                return itemDetails[0];
            }
            return string.Empty;
        }
    }
}
