using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.IO;
using RPSWNET;


namespace SSCOUIModels.Helpers
{
    public class PathConverter : IValueConverter
    {
        public const string SoundPath = "SoundPath";
        public const string ImagePath = "ImagePath";

        private static string scotDir = string.Empty;

        private const string AppDrive = "APP_DRIVE";
        private const string AppDir = "APP_DIR";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter.ToString();
            string convertedPath = null;

            try
            {
                //getting parameter path
                string path = param.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).GetValue(0).ToString();

                //getting scot directory
                if (string.IsNullOrEmpty(scotDir))
                    scotDir = GetScotDirectory();

                if (path.Equals(SoundPath))
                {
                    convertedPath = param.Replace("[SoundPath]", scotDir + "\\sound\\").ToString();
                }
                else if (path.Equals(ImagePath))
                {
                    convertedPath = param.Replace("[ImagePath]", scotDir + "\\image\\").ToString();
                }
            }
            catch (Exception)
            {
            }

            if (File.Exists(convertedPath))
            {
                return convertedPath;
            }
            else
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "PathConverter.Convert({0}) - Audio file does not exist", convertedPath);
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetScotDirectory()
        {
            //get system variables
            string appDriveValue = Environment.GetEnvironmentVariable(AppDrive, EnvironmentVariableTarget.Machine);
            string appDirValue = Environment.GetEnvironmentVariable(AppDir, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrEmpty(appDriveValue))
                appDriveValue = "C:";
            if (string.IsNullOrEmpty(appDirValue))
                appDirValue = "scot";

            return appDriveValue + "\\" + appDirValue;
        }
    }
}

