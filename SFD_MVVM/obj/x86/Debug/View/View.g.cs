﻿#pragma checksum "..\..\..\..\View\View.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1112A9F0F63E5CAF024E580900292CD6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SFD_MVVM;
using SFD_MVVM.View;
using SFD_MVVM.ViewModel;
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


namespace SFD_MVVM {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem viewMenuShowTabCntl;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem StreamMenu;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid mainGrid;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition colDefForImg;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tabControl;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SldrFrom;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SldrTo;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Sldr;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFD_MVVM.View.UpDown FramesNumber;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFD_MVVM.View.UpDown SeriesNumber;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFD_MVVM.View.FilePathPicker HDRI_Directory;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SFD_MVVM.View.FilePathPicker VideoDirectory;
        
        #line default
        #line hidden
        
        
        #line 173 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton saveSingleImageRadioBtn;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox useSelectionCheckBox;
        
        #line default
        #line hidden
        
        
        #line 341 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox autoManualChekBox;
        
        #line default
        #line hidden
        
        
        #line 345 "..\..\..\..\View\View.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Image;
        
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
            System.Uri resourceLocater = new System.Uri("/SFD_MVVM;component/view/view.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\View.xaml"
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
            
            #line 7 "..\..\..\..\View\View.xaml"
            ((SFD_MVVM.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\View\View.xaml"
            ((SFD_MVVM.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.viewMenuShowTabCntl = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 3:
            this.StreamMenu = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 4:
            this.mainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.colDefForImg = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 6:
            this.tabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 7:
            this.SldrFrom = ((System.Windows.Controls.Slider)(target));
            return;
            case 8:
            this.SldrTo = ((System.Windows.Controls.Slider)(target));
            return;
            case 9:
            this.Sldr = ((System.Windows.Controls.Slider)(target));
            return;
            case 10:
            this.FramesNumber = ((SFD_MVVM.View.UpDown)(target));
            return;
            case 11:
            this.SeriesNumber = ((SFD_MVVM.View.UpDown)(target));
            return;
            case 12:
            this.HDRI_Directory = ((SFD_MVVM.View.FilePathPicker)(target));
            return;
            case 13:
            this.VideoDirectory = ((SFD_MVVM.View.FilePathPicker)(target));
            return;
            case 14:
            this.saveSingleImageRadioBtn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 15:
            this.useSelectionCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 193 "..\..\..\..\View\View.xaml"
            this.useSelectionCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.useSelectionCheckBox_Unchecked);
            
            #line default
            #line hidden
            
            #line 194 "..\..\..\..\View\View.xaml"
            this.useSelectionCheckBox.Checked += new System.Windows.RoutedEventHandler(this.useSelectionCheckBox_Checked);
            
            #line default
            #line hidden
            return;
            case 16:
            this.autoManualChekBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 17:
            this.Image = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

