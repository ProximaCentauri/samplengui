﻿#pragma checksum "..\..\..\Controls\SystemFunctionsControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FB85E1494F9CB6810A3D4DB3B6EC26D5"
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
using SSCOUIModels.Helpers;
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


namespace SSCOUIViews.Controls {
    
    
    /// <summary>
    /// SystemFunctionsControl
    /// </summary>
    public partial class SystemFunctionsControl : System.Windows.Controls.Grid, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel SystemFunctions;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton StoreButton8;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton AssistanceButton;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton LanguageButton;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton VolumeButton;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton OwnBagButton;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel WeightDetailsPanel;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.MeasureTextBlock WeightTextBlock;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ScaleLogo;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\Controls\SystemFunctionsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run ScaleInfo;
        
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
            System.Uri resourceLocater = new System.Uri("/SSCOUIViews;component/controls/systemfunctionscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\SystemFunctionsControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 9 "..\..\..\Controls\SystemFunctionsControl.xaml"
            ((SSCOUIViews.Controls.SystemFunctionsControl)(target)).DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Grid_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SystemFunctions = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.StoreButton8 = ((SSCOControls.ImageButton)(target));
            
            #line 38 "..\..\..\Controls\SystemFunctionsControl.xaml"
            this.StoreButton8.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.StoreButton8_TouchDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AssistanceButton = ((SSCOControls.ImageButton)(target));
            return;
            case 5:
            this.LanguageButton = ((SSCOControls.ImageButton)(target));
            return;
            case 6:
            this.VolumeButton = ((SSCOControls.ImageButton)(target));
            
            #line 58 "..\..\..\Controls\SystemFunctionsControl.xaml"
            this.VolumeButton.Click += new System.Windows.RoutedEventHandler(this.VolumeButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.OwnBagButton = ((SSCOControls.ImageButton)(target));
            return;
            case 8:
            this.WeightDetailsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            this.WeightTextBlock = ((SSCOControls.MeasureTextBlock)(target));
            return;
            case 10:
            this.ScaleLogo = ((System.Windows.Controls.Image)(target));
            return;
            case 11:
            this.ScaleInfo = ((System.Windows.Documents.Run)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

