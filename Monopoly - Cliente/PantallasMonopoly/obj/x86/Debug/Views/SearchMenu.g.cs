﻿#pragma checksum "C:\Users\ofunes\Desktop\GitHub\UWPolyFlama\Monopoly - Cliente\PantallasMonopoly\Views\SearchMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B345590FD13040CC359DACE732A8F3A0"
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
    partial class SearchMenu : 
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
            case 2: // Views\SearchMenu.xaml line 22
                {
                    this.Header = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                }
                break;
            case 3: // Views\SearchMenu.xaml line 78
                {
                    this.Content = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 5: // Views\SearchMenu.xaml line 39
                {
                    this.icon = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.icon).Click += this.Back_Click;
                }
                break;
            case 6: // Views\SearchMenu.xaml line 62
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

