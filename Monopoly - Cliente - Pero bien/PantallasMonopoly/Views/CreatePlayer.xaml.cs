
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PantallasMonopoly.Views;
using PantallasMonopoly.ViewModels;
using PantallasMonopoly.Util;
using System;
using PantallasMonopoly.Models;
using PantallasMonopoly.Models.Enums;
using PantallasMonopoly.Connection;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PantallasMonopoly
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CreatePlayer : Page
    {
        createPlayerVM miVM;

        public CreatePlayer()
        {
            this.InitializeComponent();
            miVM = new createPlayerVM(new NavigationService());
            this.DataContext = miVM;

        }
      
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (miVM.lobbyAEntrar != null) {

                conexionPadre.proxy.Invoke("salirDeLobby", miVM.lobbyAEntrar.nombre);
            } 

            this.Frame.Navigate(typeof(MainMenu));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

           
            if (e.Parameter is Lobby)
            {
                miVM.lobbyAEntrar = (Lobby)e.Parameter;
            }
        }


    }
}
