﻿#pragma checksum "..\..\..\Views\TenderOptions.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7CA1C2C82F16AC29294A5238F76574B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SSCOControls;
using SSCOUIModels.Controls;
using SSCOUIViews.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SSCOUIViews.Views {
    
    
    /// <summary>
    /// TenderOptions
    /// </summary>
    public partial class TenderOptions : SSCOUIModels.Controls.BackgroundView, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Views\TenderOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CashOptions;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Views\TenderOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CashBackPayment;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Views\TenderOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.MeasureTextBlock CashBackPaymentOption;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Views\TenderOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.PageIndicator pageIndicator;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\Views\TenderOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOUIModels.Controls.SSCOUISlidingGridPage CashBackOptions;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SSCOUIViews;component/views/tenderoptions.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\TenderOptions.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\..\Views\TenderOptions.xaml"
            ((SSCOUIViews.Views.TenderOptions)(target)).Loaded += new System.Windows.RoutedEventHandler(this.TenderOptions_Loaded);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\Views\TenderOptions.xaml"
            ((SSCOUIViews.Views.TenderOptions)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.TenderOptions_UnLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CashOptions = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.CashBackPayment = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.CashBackPaymentOption = ((SSCOControls.MeasureTextBlock)(target));
            return;
            case 5:
            this.pageIndicator = ((SSCOControls.PageIndicator)(target));
            return;
            case 6:
            this.CashBackOptions = ((SSCOUIModels.Controls.SSCOUISlidingGridPage)(target));
            
            #line 65 "..\..\..\Views\TenderOptions.xaml"
            this.CashBackOptions.TouchUp += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.CashBack_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

