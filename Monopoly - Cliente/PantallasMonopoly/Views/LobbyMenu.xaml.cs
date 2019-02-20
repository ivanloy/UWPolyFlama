
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PantallasMonopoly
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class LobbyMenu : Page
    {


        public LobbyMenu()
        {
            this.InitializeComponent();
            var vm = new lobbyVM(new NavigationService());
            this.DataContext = vm;

        }
      
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            lobbyVM viewModel;
            viewModel = (lobbyVM)this.DataContext;
            //this.Frame.Navigate(typeof(MainMenu));

            conexionPadre.proxy.Invoke("salirDeLobby", viewModel.lobby.nombre);
            
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            lobbyVM viewModel;
            viewModel = (lobbyVM)this.DataContext;

            if (e.Parameter is Lobby)
            {
                viewModel.lobby = (Lobby)e.Parameter;
            }
        }


    }
}
