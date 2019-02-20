using Microsoft.AspNet.SignalR.Client;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace PantallasMonopoly.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();

            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            //Esto es para crear la conexion nada mas iniciar el videojoc
            IHubProxy prox = conexionPadre.proxy;
        }

        private void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {      
            this.Frame.Navigate(typeof(CreatePlayer), TipoEntrada.SEARCH);
        }

        private void Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreatePlayer), TipoEntrada.CREATE);
        }

        private void About_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutMenu));
        }

        private void Exit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
