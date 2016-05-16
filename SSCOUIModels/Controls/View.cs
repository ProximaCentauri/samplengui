using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;
using SSCOUIModels.Models;
using SSCOUIModels.Helpers;
using SSCOControls;

namespace SSCOUIModels.Controls
{
    public abstract class View : Page
    {
        public View(IMainViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            AddHandler(WatermarkTextBox.KeyboardFocusEvent, (RoutedEventHandler)HandleKeyboardFocusChanged);
        }

        public virtual void Show(bool isShowing)
        {
        }

        public virtual void OnPropertyChanged(string name, object value)
        {            
        }

        public virtual void OnStateChanged(string state)
        {            
        }

        public virtual void OnStateParamChanged(string param)
        {
        }

        public bool IsPrimaryLanguage { get; set; }

        public FrameworkElement KeyboardFocusedElement { get; private set; }

        protected bool GetPropertyBoolValue(string property)
        {
            var value = this.viewModel.GetPropertyValue(property);
            var retValue = false;
            if (null != value)
            {
                retValue = (bool)value;
            }
            return retValue;
        }

        protected string GetPropertyStringValue(string property)
        {
            var value = this.viewModel.GetPropertyValue(property);
            var retValue = string.Empty;
            if (null != value)
            {
                retValue = value.ToString();
            }
            return retValue;
        }

        protected IMainViewModel viewModel;

        private void HandleKeyboardFocusChanged(object sender, RoutedEventArgs args)
        {
            WatermarkTextBox tb = args.OriginalSource as WatermarkTextBox;
            IEnumerable<WatermarkTextBox> textBoxes = UIControlFinder.FindVisualChildren<WatermarkTextBox>(this);
            if (null != textBoxes)
            {
                foreach (WatermarkTextBox textBox in textBoxes)
                {
                    if (textBox != tb)
                    {
                        textBox.KeyboardFocus = false;
                    }
                }
            }
            this.KeyboardFocusedElement = tb;
            args.Handled = true;
        }        
    }
}
