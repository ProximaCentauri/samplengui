﻿#pragma checksum "..\..\..\Controls\SignatureCapture.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A097E5FD029B152C3A7478E5A0F5603B"
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
    /// SignatureCapture
    /// </summary>
    public partial class SignatureCapture : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.MeasureTextBlock Instruction;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border SignatureArea;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas SignatureCanvas;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path SignaturePath;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid SignatureButtons;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton ClearButton;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Controls\SignatureCapture.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SSCOControls.ImageButton OKButton;
        
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
            System.Uri resourceLocater = new System.Uri("/SSCOUIViews;component/controls/signaturecapture.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\SignatureCapture.xaml"
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
            
            #line 12 "..\..\..\Controls\SignatureCapture.xaml"
            ((System.Windows.Controls.Grid)(target)).DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.Grid_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Instruction = ((SSCOControls.MeasureTextBlock)(target));
            return;
            case 3:
            this.SignatureArea = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.SignatureCanvas = ((System.Windows.Controls.Canvas)(target));
            
            #line 41 "..\..\..\Controls\SignatureCapture.xaml"
            this.SignatureCanvas.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.SignatureCanvas_TouchDown);
            
            #line default
            #line hidden
            
            #line 42 "..\..\..\Controls\SignatureCapture.xaml"
            this.SignatureCanvas.TouchMove += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.SignatureCanvas_TouchMove);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\Controls\SignatureCapture.xaml"
            this.SignatureCanvas.TouchLeave += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.SignatureCanvas_TouchLeave);
            
            #line default
            #line hidden
            
            #line 44 "..\..\..\Controls\SignatureCapture.xaml"
            this.SignatureCanvas.TouchUp += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.SignatureCanvas_TouchUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SignaturePath = ((System.Windows.Shapes.Path)(target));
            return;
            case 6:
            this.SignatureButtons = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.ClearButton = ((SSCOControls.ImageButton)(target));
            
            #line 65 "..\..\..\Controls\SignatureCapture.xaml"
            this.ClearButton.TouchUp += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Clear_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.OKButton = ((SSCOControls.ImageButton)(target));
            
            #line 74 "..\..\..\Controls\SignatureCapture.xaml"
            this.OKButton.TouchUp += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.OK_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
