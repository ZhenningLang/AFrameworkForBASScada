﻿#pragma checksum "..\..\..\..\Common\Views\AboutBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FD4568D28711BBF28CF53F0BD51EB498"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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


namespace FrontFramework.Common.Views {
    
    
    /// <summary>
    /// AboutBox
    /// </summary>
    public partial class AboutBox : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\Common\Views\AboutBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelName;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Common\Views\AboutBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelAuthor;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Common\Views\AboutBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelContact;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Common\Views\AboutBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button okButton;
        
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
            System.Uri resourceLocater = new System.Uri("/FrontFramework;component/common/views/aboutbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Common\Views\AboutBox.xaml"
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
            this.labelName = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.labelAuthor = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.labelContact = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            
            #line 42 "..\..\..\..\Common\Views\AboutBox.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.emailOnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.okButton = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\Common\Views\AboutBox.xaml"
            this.okButton.Click += new System.Windows.RoutedEventHandler(this.okButtonOnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

