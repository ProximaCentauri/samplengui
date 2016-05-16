using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Win32;
using System.Globalization;
using SSCOUI.Properties;
using SSCOUIStrings;
using SSCOUIModels;
using SSCOUIModels.Helpers;
using SSCOUIModels.Controls;
using RPSWNET;
using SSCOControls;
using System.IO;
using System.Collections;

namespace SSCOUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static View GetCachedView(Type viewType, bool isPrimaryLanguage)
        {
            object[] args = { Application.Current.MainWindow.DataContext };
            View view;
            if (!cachedViews.ContainsKey(viewType))
            {
                //Create new instance of the View
                view = Activator.CreateInstance(viewType, args) as View;
                cachedViews[viewType] = view;
            }
            else
            {
                //Re-use created View
                view = cachedViews[viewType] as View;
            }
            if (view.IsPrimaryLanguage != isPrimaryLanguage)
            {
                view.IsPrimaryLanguage = isPrimaryLanguage;
                LocalizationManager.UpdateValues();
            }
            return view;
        }

        internal bool delayedLanguageChange;
        internal static readonly string[] LanguageChangeStateParams = { "Finish", "TakeReceipt", "EnterId" };
        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (null != this.viewModel)
            {
                this.viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.ApplicationShutDown, string.Empty);
                this.viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                this.viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.ApplicationShutDown, string.Empty);                
                this.viewModel.Dispose();
                CmDataCapture.Flush();
                CmDataCapture.Cleanup();
                System.AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);                
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ReadRegistryConfig();
            CmDataCapture.Initialize("-- SSCOUI Start Tracing --");
            System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            if (Process.GetProcessesByName(assembly.Name).Length > 1)
            {
                CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "{0} is already running.", assembly.Name);
                Current.Shutdown();
            }
            else
            {
                if (!Settings.Default.ShowCursor)
                {
                    Mouse.OverrideCursor = Cursors.None;
                }                
                DisableWPFTabletSupport();
                int renderingTier = RenderCapability.Tier >> 16;
                CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "App.Application_Startup() - Rendering Tier:{0}", renderingTier);
                if (Settings.Default.ForceRenderingTier > -1)
                {
                    renderingTier = Settings.Default.ForceRenderingTier;
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "App.Application_Startup() - Forcing Rendering Tier:{0}", renderingTier);
                }
                try
                {
                    Resources.MergedDictionaries.Add(LoadComponent(new Uri("SSCOUISkin;component/Skin.xaml", UriKind.Relative)) as ResourceDictionary);
                    if (renderingTier <= 1)
                    {
                        try
                        {
                            Resources.MergedDictionaries.Add(LoadComponent(new Uri("SSCOUISkin;component/SkinDegraded.xaml", UriKind.Relative)) as ResourceDictionary);
                            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "App.Application_Startup() - SkinDregraded.xaml loaded");
                        }
                        catch (Exception)
                        {
                            CmDataCapture.CaptureFormat(CmDataCapture.MaskInfo, "App.Application_Startup() - SkinDregraded.xaml not available");
                        }
                    }
                }
                catch (Exception ex)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "App.Application_Startup() - Error loading SSCOUISkin.dll: {0}", ex.Message);
                }

                PrimaryLanguageCallback languageCallback = new PrimaryLanguageCallback(PrimaryLanguageCallback);
                LocalizationManager.LanguageCallback = languageCallback;
                MeasureTextBlock.LanguageCallback = languageCallback;
                MeasureTextBlock.TopLevelNameSpace = "SSCOUIViews";

                this.viewModel = new MainViewModel();
                this.viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
                StringManager.LoadStringOverrides(configPath);                
                this.viewModel.ShowTransitionEffects = renderingTier > 0;
                this.viewModel.Perfcheck.StartEventLog(PerfMeasureEvents.ApplicationStartup, string.Empty);

                this.MainWindow = new MainWindow(this.viewModel);
                this.MainWindow.Show();
                CacheAllItemImages();
                
                this.viewModel.Perfcheck.EndEventLog(PerfMeasureEvents.ApplicationStartup, string.Empty);
            }
        }

        private void CacheAllItemImages()
        {
            this.cacheItemImages.create("PickList,QuickPick,Containers,Card");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            string error = "CurrentDomain_UnhandledException() - " + exception.ToString();
            while (null != exception.InnerException)
            {
                exception = exception.InnerException;
                error += " : Inner Exception = " + exception.Message;
            }
            CmDataCapture.Capture(CmDataCapture.MaskError, error);
        }

        private static void DisableWPFTabletSupport()
        {
            // Get a collection of the tablet devices for this window. 
            TabletDeviceCollection devices = System.Windows.Input.Tablet.TabletDevices;
            if (devices.Count > 0)
            {
                // Get the Type of InputManager.
                Type inputManagerType = typeof(System.Windows.Input.InputManager);
                // Call the StylusLogic method on the InputManager.Current instance.
                object stylusLogic = inputManagerType.InvokeMember("StylusLogic", BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, InputManager.Current, null);
                if (stylusLogic != null)
                {
                    // Get the type of the device class.
                    Type devicesType = devices.GetType();
                    // Loop until there are no more devices to remove.
                    int count = devices.Count + 1;
                    while (devices.Count > 0)
                    {
                        // Remove the first tablet device in the devices collection.
                        devicesType.InvokeMember("HandleTabletRemoved", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null,
                            devices, new object[] { (uint)0 });
                        count--;
                        if (devices.Count != count)
                        {
                            throw new Win32Exception("Unable to remove real-time stylus support.");
                        }
                    }
                }
            }
        }

        private bool PrimaryLanguageCallback(DependencyObject target)
        {
            bool isPrimaryLanguage = false;
            View view = UIControlFinder.FindAncestorOrSelf<View>(target);
            if (null != view)
            {
                isPrimaryLanguage = view.IsPrimaryLanguage;
            }
            else
            {
                isPrimaryLanguage = this.viewModel.StoreMode;
            }
            return isPrimaryLanguage;
        }

        private void ReadRegistryConfig()
        {
            const string REGISTRY_NGUI = "SOFTWARE\\NCR\\NextGenUI\\";
            if (!CmDataCapture.ReadRegistry(REGISTRY_NGUI, "DataCapture"))
            {
                CmDataCapture.SetCaptureControl(4);
                CmDataCapture.SetPrefix("SSCOUI:");
                CmDataCapture.SetTimeOptions(0x1F00);
                CmDataCapture.SetCaptureMask(0x26);
                CmDataCapture.SetFile("c:\\scot\\logs\\SSCOUI.log", 5000);
            }
            configPath = Environment.ExpandEnvironmentVariables(Registry.GetValue("HKEY_LOCAL_MACHINE\\" + REGISTRY_NGUI, "ConfigFilePath",
                @"C:\\SCOT\\config").ToString());
            if (File.Exists(configPath + "\\SSCOUI.exe.config"))
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configPath + "\\SSCOUI.exe.config");
            }
            else
            {
                Current.Shutdown();
                CmDataCapture.CaptureFormat(CmDataCapture.MaskError,
                    "Error : ReadRegistryConfig() - Config file not found based on the configured directory in the regisrty {0}.", configPath);
            }           
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CustomerLanguage"))
            {
                LocalizationManager.SecondaryCultureInfo = new CultureInfo(this.viewModel.CustomerLanguage, false);
                if (-1 == Array.IndexOf(LanguageChangeStateParams, this.viewModel.StateParam))
                {
                    LocalizationManager.UpdateValues();
                }
                else
                {
                    this.delayedLanguageChange = true;
                }
            }
            else if (e.PropertyName.Equals("Language"))
            {
                if (-1 == Array.IndexOf(LanguageChangeStateParams, this.viewModel.StateParam))
                {
                    LocalizationManager.UpdateValues();                
                }
                else
                {
                    this.delayedLanguageChange = true;
                }
            }
            else if (e.PropertyName.Equals("StateParam"))
            {
                if (this.viewModel.StateParam.Equals(EndOfTransactionStateParam))
                {
                    this.cacheItemImages.create("Card");
                }
            }
            else
            {
                if (e.PropertyName.Equals("NextGenData"))
                {
                    object value = this.viewModel.GetPropertyValue(e.PropertyName);
                    if (null != value && value.Equals("Load Options"))
                    {
                        CacheAllItemImages();
                        StringManager.LoadStringOverrides(configPath);
                        LocalizationManager.UpdateValues();
                    }
                }
            }
        }

        private static Hashtable cachedViews = new Hashtable();
        private CacheItemImages cacheItemImages = CacheItemImages.Instance;
        private string configPath;
        private static readonly string EndOfTransactionStateParam = "Finish";        
        private IMainViewModel viewModel;
    }
}
