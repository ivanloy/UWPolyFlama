﻿#pragma checksum "C:\Users\jobando\Desktop\Git\UWPolyFlama\Monopoly - Cliente\PantallasMonopoly\Views\AboutMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ED02E8AD92E5CC0A90569AAD816A50D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PantallasMonopoly
{
    partial class AboutMenu : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Views\AboutMenu.xaml line 18
                {
                    this.Header = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                }
                break;
            case 3: // Views\AboutMenu.xaml line 74
                {
                    this.Content = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 4: // Views\AboutMenu.xaml line 35
                {
                    this.icon = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.icon).Click += this.Back_Click;
                }
                break;
            case 5: // Views\AboutMenu.xaml line 58
                {
                    this.Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

