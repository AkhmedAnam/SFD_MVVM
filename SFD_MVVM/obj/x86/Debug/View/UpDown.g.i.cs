﻿#pragma checksum "..\..\..\..\View\UpDown.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "855DEF8C245938DD91D545D695AB808B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SFD_MVVM.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace SFD_MVVM.View {
    
    
    /// <summary>
    /// UpDown
    /// </summary>
    public partial class UpDown : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\View\UpDown.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFD_MVVM.View.UpDown root;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\View\UpDown.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.RepeatButton Btn_Up;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\View\UpDown.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.RepeatButton Btn_Down;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\View\UpDown.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt;
        
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
            System.Uri resourceLocater = new System.Uri("/SFD_MVVM;component/view/updown.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\UpDown.xaml"
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
            this.root = ((SFD_MVVM.View.UpDown)(target));
            return;
            case 2:
            this.Btn_Up = ((System.Windows.Controls.Primitives.RepeatButton)(target));
            
            #line 21 "..\..\..\..\View\UpDown.xaml"
            this.Btn_Up.Click += new System.Windows.RoutedEventHandler(this.Btn_Up_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Btn_Down = ((System.Windows.Controls.Primitives.RepeatButton)(target));
            
            #line 22 "..\..\..\..\View\UpDown.xaml"
            this.Btn_Down.Click += new System.Windows.RoutedEventHandler(this.Btn_Down_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txt = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\..\View\UpDown.xaml"
            this.txt.KeyDown += new System.Windows.Input.KeyEventHandler(this.txt_KeyDown);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\..\View\UpDown.xaml"
            this.txt.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.txt_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
