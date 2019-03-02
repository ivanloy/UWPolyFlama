
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PantallasMonopoly.Views;
using PantallasMonopoly.ViewModels;
using PantallasMonopoly.Util;
using PantallasMonopoly.Models;
using PantallasMonopoly.Connection;
using Windows.System;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PantallasMonopoly
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class LobbyMenu : Page
    {
        lobbyVM miVM;

        public LobbyMenu()
        {
            this.InitializeComponent();
            miVM = new lobbyVM(new NavigationService());
            this.DataContext = miVM;


        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

            conexionPadre.proxy.Invoke("salirDeLobby", miVM.lobby.nombre);

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter is Lobby)
            {
                miVM.lobby = (Lobby)e.Parameter;
            }
        }

        private void ChatInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            
            if (e.Key == VirtualKey.Enter) {

                miVM.enviarMensaje();

            }

        }
    }
}
