using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SSCOUIModels;
using RPSWNET;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for CurrencyCylinder.xaml
    /// </summary>
    public partial class CurrencyCylinder : Grid
    {
        public CurrencyCylinder()
        {
            InitializeComponent();
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (this.viewModel.ActiveStateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                case "XMCashReplenish":
                case "XMCashStatus":
                case "XMCashStatusGlory":
                    if (e.PropertyName.Equals("StateParam") || e.PropertyName.StartsWith(Cylinder))
                    {
                        Redraw();
                    }
                    break;
            }
        }

        private void Redraw()
        {
            ParseTransactionVariableData(this.viewModel.GetPropertyValue(Cylinder).ToString());
            UpdateStyle();
            UpdateVisibility();
            UpdateFillHeight();
            UpdateFillColor();
            UpdateCurrentValues();
            UpdateChangeInValues();
            UpdateLoaderValues();
            UpdateDenomination();
            UpdateBaseLevel();
            UpdateEnabledStatus();
            UpdateSelectedStatus();
        }

        private void UpdateStyle()
        {
            if (CylinderType.Equals("NonDispense"))
            {
                this.CylinderInfo.Text = this.viewModel.GetPropertyValue(Cylinder + "InfoText").ToString();
                this.CylinderBody.Style = this.FindResource("cashManagementNonDispenseCylinderBodyStyle") as Style;
                this.CylinderFill.Style = this.FindResource("cashManagementNonDispenseCylinderStyle") as Style;
                this.CylinderValues.Style = this.FindResource("cashManagementNonDispenseValuesStyle") as Style;
                this.CylinderChangeInValue.Style = this.FindResource("cashManagementNonDispenseValueChangeStyle") as Style;
                this.CylinderChangeInCount.Style = this.FindResource("cashManagementNonDispenseCountChangeStyle") as Style;
                this.CylinderCurrentValue.Style = this.CylinderCurrentCount.Style = this.FindResource("cashManagementNonDispenseCurrentValuesStyle") as Style;
                this.CylinderDenomination.Style = this.FindResource("cashManagementNonDispenseCylinderDenominationStyle") as Style;
            }
            else
            {
                this.CylinderInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateFillHeight()
        {
            string value;
            if (null!= properties && properties.TryGetValue(fillLevelKey, out value))
            {
                double fillPercentage;
                if (Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out fillPercentage) && fillPercentage >= 0)
                {
                    CylinderFill.Height = ((CylinderBody.Height - fillHeightBorderOffset) * (fillPercentage / 100.0));
                }
                else
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "SSCOUIViews.Controls.CurrencyCylinder.UpdateFillHeight() - Invalid Fill Height Value: Cylinder=[{0}], Value=[{1}]",
                        Cylinder, value);
                }
            }
        }

        private void UpdateFillColor()
        {
            string value;
            if (null != properties && properties.TryGetValue(fillColorKey, out value))
            {
                if (value.Equals(fillColorValueGreen))
                {
                    CylinderFill.Background = TryFindResource("cylinderGreenFillColorBrush") as Brush;
                }
                else if (value.Equals(fillColorValueRed))
                {
                    CylinderFill.Background = TryFindResource("cylinderRedFillColorBrush") as Brush;
                }
                else if (value.Equals(fillColorValueYellow))
                {
                    CylinderFill.Background = TryFindResource("cylinderYellowFillColorBrush") as Brush;
                }
            }
        }

        private void UpdateVisibility()
        {
            if (CylinderType.Equals("Generic") && (null == properties || this.viewModel.GetPropertyValue(Cylinder).ToString().Trim().Equals("")))
            {
                CylinderControl.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateCurrentValues()
        {
            string count = this.viewModel.GetPropertyValue(Cylinder + "CountText").ToString();
            string value = this.viewModel.GetPropertyValue(Cylinder + "ValueText").ToString();
            if (CylinderType.Equals("Generic") && (Boolean)this.viewModel.GetPropertyValue(Cylinder + "PercentFullShown"))
            {
                string percentFull = this.viewModel.GetPropertyValue(Cylinder + "PercentFullText").ToString();
                CylinderCurrentPercentage.Visibility = CylinderCurrentValue.Visibility = CylinderCurrentCount.Visibility = Visibility.Visible;
                CylinderCurrentPercentage.Text = percentFull;
                CylinderCurrentValue.Text = value;
                CylinderCurrentCount.Text = count;
            }
            else if (count.Equals(unusedText) || value.Equals(unusedText))
            {
                CylinderCurrentPercentage.Visibility = CylinderCurrentValue.Visibility = CylinderCurrentCount.Visibility = Visibility.Hidden;
            }
            else
            {
                CylinderCurrentValue.Visibility = CylinderCurrentCount.Visibility = Visibility.Visible;
                CylinderCurrentPercentage.Visibility = Visibility.Hidden;
                CylinderCurrentValue.Text = value;
                CylinderCurrentCount.Text = count;
            }
        }

        private void UpdateChangeInValues()
        {
            CylinderChangeInCount.Text = this.viewModel.GetPropertyValue(Cylinder + "ChangeInCountText").ToString();
            CylinderChangeInValue.Text = this.viewModel.GetPropertyValue(Cylinder + "ChangeInValueText").ToString();
            if (CylinderChangeInCount.Text.Equals(unusedText) || CylinderChangeInValue.Text.Equals(unusedText) || !(Boolean)this.viewModel.GetPropertyValue(Cylinder + "ChangeInCountTextShown"))
            {
                CylinderChangeInCount.Visibility = CylinderChangeInValue.Visibility = Visibility.Hidden;
            }
            else
            {
                CylinderChangeInCount.Visibility = CylinderChangeInValue.Visibility = Visibility.Visible;
            }
        }

        private void UpdateLoaderValues()
        {
            if(CylinderType.Equals("Generic") && CurrencyType.Equals("Notes"))
            {
                string count = this.viewModel.GetPropertyValue(Cylinder + "LoaderCountText").ToString();
                string value = this.viewModel.GetPropertyValue(Cylinder + "LoaderValueText").ToString();
                
                if (!Convert.ToBoolean(this.viewModel.GetPropertyValue(Cylinder + "LoaderCountShown")) || count.Equals(unusedText))
                {
                    CylinderLoaderValue.Visibility = CylinderLoaderCount.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CylinderLoaderValue.Visibility =  CylinderLoaderCount.Visibility = Visibility.Visible;
                    CylinderLoaderValue.Text = value;
                    CylinderLoaderCount.Text = count;
                }
            }
            else
            {
                CylinderLoaderValue.Visibility = CylinderLoaderCount.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateDenomination()
        {
            if (CylinderType.Equals("Generic"))
            {
                CylinderDenomination.Text = this.viewModel.GetPropertyValue(Cylinder + "DenomText").ToString();
            }
        }

        private void UpdateBaseLevel()
        {
            if (CylinderType.Equals("Generic") && (Boolean)this.viewModel.GetPropertyValue(Cylinder + "BaseLineShown"))
            {
                string value;
                if (null != properties && properties.TryGetValue(baseLevelKey, out value))
                {
                    double levelPercentage;
                    if (Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out levelPercentage) && levelPercentage >= 0)
                    {
                        Thickness margin = CylinderBaseLevel.Margin;
                        margin.Bottom = CylinderDenomination.Height + ((CylinderBody.Height - baseLevelBorderOffset) * (levelPercentage / 100.0));
                        CylinderBaseLevel.Margin = margin;
                        CylinderBaseLevel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "SSCOUIViews.Controls.CurrencyCylinder.UpdateBaseLevel() - Invalid Base Level Value: Cylinder=[{0}], Value=[{1}]",
                            Cylinder, value);
                    }
                }
            }
            else
            {
                CylinderBaseLevel.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateEnabledStatus()
        {
            if (CylinderType.Equals("Generic"))
            {
                string value;
                if (null != properties && properties.TryGetValue(enabledKey, out value))
                {
                    bool enabled;
                    if (Boolean.TryParse(value, out enabled))
                    {
                        if (enabled)
                        {
                            CylinderBody.IsEnabled = true;
                            CylinderBody.Style = this.FindResource("cashManagementEnabledCylinderBodyStyle") as Style;
                            CylinderDenomination.Style = this.FindResource("cashManagementEnabledCylinderDenominationStyle") as Style;
                        }
                        else
                        {
                            CylinderBody.IsEnabled = false;
                            CylinderBody.Style = this.FindResource("cashManagementCylinderBodyStyle") as Style;
                            CylinderDenomination.Style = this.FindResource("cashManagementCylinderDenominationStyle") as Style;
                        }
                    }
                }
            }
        }

        private void UpdateSelectedStatus()
        {
            string value;
            if (null != properties && properties.TryGetValue(selectedKey, out value))
            {
                bool selected;
                if (Boolean.TryParse(value, out selected))
                {
                    CylinderHighlight.Visibility = selected ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void CylinderBody_TouchDown(object sender, RoutedEventArgs e)
        {
            switch (this.viewModel.ActiveStateParam)
            {
                case "XMCashRemove":
                case "XMCashRemoveBNR":
                case "XMCashRemoveGlory":
                    this.viewModel.ActionCommand.Execute(string.Format("XM{0}RemoveButtonClick({1})", CurrencyType, CylinderNumber));
                    break;
                case "XMCashReplenish":
                    this.viewModel.ActionCommand.Execute(string.Format("XM{0}AddButtonClick({1})", CurrencyType, CylinderNumber));
                    break;
            }
        }

        private void CylinderValues_TouchDown(object sender, RoutedEventArgs e)
        {
            if (CylinderBody.IsEnabled)
            {
                CylinderBody.RaiseEvent(e);
            }
        }

        private void ParseTransactionVariableData(string varStr)
        {
            string[] vars = varStr.Split(KeyValueDelimiter);
            string[] pair = null;

            if (properties != null)
            {
                properties.Clear();
                for (int i = 0; i < vars.Length; i++)
                {
                    pair = vars[i].Split(ValueDelimiter);
                    if (2 == pair.Length)
                    {
                        properties.Add(pair[0], pair[1]);
                    }
                }
            }
        }

        public string Cylinder
        {
            get
            {
                return Convert.ToString(GetValue(CylinderProperty));
            }
            set
            {
                SetValue(CylinderProperty, value);
            }
        }

        public static DependencyProperty CylinderProperty = DependencyProperty.Register("Cylinder", typeof(string), typeof(CurrencyCylinder));

        public string CylinderNumber
        {
            get
            {
                return Convert.ToString(GetValue(CylinderNumberProperty));
            }
            set
            {
                SetValue(CylinderNumberProperty, value);
            }
        }

        public static DependencyProperty CylinderNumberProperty = DependencyProperty.Register("CylinderNumber", typeof(string), typeof(CurrencyCylinder));

        public string CylinderType
        {
            get
            {
                return Convert.ToString(GetValue(CylinderTypeProperty));
            }
            set
            {
                SetValue(CylinderTypeProperty, value);
            }
        }

        public static DependencyProperty CylinderTypeProperty = DependencyProperty.Register("CylinderType", typeof(string), typeof(CurrencyCylinder));

        public string CurrencyType
        {
            get
            {
                return Convert.ToString(GetValue(CurrencyTypeProperty));
            }
            set
            {
                SetValue(CurrencyTypeProperty, value);
            }
        }

        public static DependencyProperty CurrencyTypeProperty = DependencyProperty.Register("CurrencyType", typeof(string), typeof(CurrencyCylinder));

        private IMainViewModel viewModel;
        private const char KeyValueDelimiter = ';';
        private const char ValueDelimiter = '=';
        private const string baseLevelKey = "base_level";
        private const string enabledKey = "enabled";
        private const string fillLevelKey = "fill_level";
        private const string fillColorKey = "fill_color";
        private const string selectedKey = "selected";
        private const string fillColorValueGreen = "green";
        private const string fillColorValueRed = "red";
        private const string fillColorValueYellow = "yellow";
        private const string unusedText = "()";
        private const int baseLevelBorderOffset = 3;
        private const int fillHeightBorderOffset = 2;
        private Dictionary<string, string> properties = new Dictionary<string, string>();
    }
}