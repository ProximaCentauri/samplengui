using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using RPSWNET;

namespace SSCOUIModels.Helpers
{
    public class CacheItemImages
    {
        private static readonly CacheItemImages instance = new CacheItemImages();
        static CacheItemImages() { }
        public static CacheItemImages Instance
        {
            get
            {
                return instance;
            }
        }

        public Dictionary<string, BitmapImage> PicklistItemImages { get { return picklistItemImages; } }
        public Dictionary<string, BitmapImage> QuickPickItemImages { get { return quickPickItemImages; } }
        public Dictionary<string, BitmapImage> CardItemImages { get { return cardItemImages; } }
        public Dictionary<string, BitmapImage> ContainersItemImages { get { return containersItemImages; } }
        
        public int Count
        {
            get
            {
                return picklistItemImages.Count + quickPickItemImages.Count + cardItemImages.Count + containersItemImages.Count;
            }
        }

        private string uri = "pack://siteoforigin:,,,/{0}";
        private readonly string[] quickPickListImageExtensions = { ".jpg", ".png" };
        private Dictionary<string, BitmapImage> picklistItemImages;
        private Dictionary<string, BitmapImage> quickPickItemImages;
        private Dictionary<string, BitmapImage> cardItemImages;
        private Dictionary<string, BitmapImage> containersItemImages;
        
        public void create(string dir)
        {
            if (!string.IsNullOrWhiteSpace(dir))
            {
                string[] arrDir = dir.Split(',');
                foreach (string d in arrDir)
                {
                    switch (d.Trim().ToLower())
                    {
                        case "picklist":
                            picklistItemImages = clearCache(picklistItemImages);
                            break;
                        case "quickpick":
                            quickPickItemImages = clearCache(quickPickItemImages);
                            break;
                        case "card":
                            cardItemImages = clearCache(cardItemImages);                            
                            break;
                        case "containers":
                            containersItemImages = clearCache(containersItemImages);
                            break;
                        default:
                            break;
                    }
                }                
            }
        }

        private Dictionary<string, BitmapImage> clearCache(Dictionary<string, BitmapImage> cache)
        {            
            if (null == cache)
            {
                return new Dictionary<string, BitmapImage>();
            }
            cache.Clear();
            return cache;
        }

        public BitmapImage addCacheImage(Dictionary<string, BitmapImage> itemImages, string fileName)
        {
            return addCacheImage(itemImages, fileName, null);
        }

        public BitmapImage addCacheImage(Dictionary<string, BitmapImage> itemImages, string fileName, string[] extensions)
        {
            string itemCode = Path.GetFileNameWithoutExtension(RemoveIllegalChars(fileName));
            if (!itemImages.ContainsKey(itemCode))
            {
                if (null == extensions)
                {
                    BitmapImage image = createBitmapImage(fileName);
                    if (null != image)
                    {
                        itemImages.Add(itemCode, image);
                        return image;
                    }
                }
                else
                {
                    foreach (string extension in extensions)
                    {
                        BitmapImage image = createBitmapImage(fileName, extension);
                        if (null != image)
                        {
                            itemImages.Add(itemCode, image);
                            return image;
                        }
                    }
                }
            }
            return null;
        }

        public BitmapImage createBitmapImage(string fileName)
        {
            return createBitmapImage(fileName, null);
        }

        public BitmapImage createBitmapImage(string fileName, string extension)
        {
            try
            {
                extension = string.IsNullOrWhiteSpace(extension) ? string.Empty : extension;
                if (!IsFileExist(fileName + extension))
                    return null;

                var bi = new BitmapImage();
                var uriSource = new Uri(String.Format(CultureInfo.CurrentCulture, uri, fileName + extension), UriKind.Absolute);                    
                bi.BeginInit();                
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.IgnoreColorProfile;
                bi.UriSource = uriSource;
                bi.EndInit();
                return bi;
            }
            catch (FileNotFoundException)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "CacheItemImages.createBitmapImage({0}) - image file does not exist", fileName);
            }
            catch (Exception ex)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "Generic exception occured: ", ex.ToString());
            }

            return null;
        }

        private bool IsFileExist(string fileName)
        {
            if (!File.Exists(fileName))
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskExtensive, "CacheItemImages.createBitmapImage({0}) - image file does not exist", fileName);
                return false;
            }
            return true;
        }

        private string RemoveIllegalChars(string fileName)
        {
            string text = fileName;
            string regexSearch = new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            text = r.Replace(text, "");
            return text;
        }

        private IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));
        }
    }
}
