﻿#pragma checksum "..\..\..\Component\TrackPlayer.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BB09923D61F549FD34C15D629089145BD57A0B73"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace HongLi.MapControl.Component {
    
    
    /// <summary>
    /// TrackPlayer
    /// </summary>
    public partial class TrackPlayer : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Btn;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PId;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PSpeed;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PTime;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PLat;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Slider;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Component\TrackPlayer.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Img;
        
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
            System.Uri resourceLocater = new System.Uri("/HongLi.MapControl;component/component/trackplayer.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Component\TrackPlayer.xaml"
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
            
            #line 6 "..\..\..\Component\TrackPlayer.xaml"
            ((HongLi.MapControl.Component.TrackPlayer)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Btn = ((System.Windows.Controls.Image)(target));
            
            #line 19 "..\..\..\Component\TrackPlayer.xaml"
            this.Btn.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Btn_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PId = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.PSpeed = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.PTime = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.PLat = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.Slider = ((System.Windows.Controls.Slider)(target));
            
            #line 39 "..\..\..\Component\TrackPlayer.xaml"
            this.Slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Img = ((System.Windows.Controls.Image)(target));
            
            #line 45 "..\..\..\Component\TrackPlayer.xaml"
            this.Img.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Close_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
