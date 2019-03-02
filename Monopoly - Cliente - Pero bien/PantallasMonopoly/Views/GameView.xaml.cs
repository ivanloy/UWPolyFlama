using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using PantallasMonopoly.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class GameView : Page
    {

        public GameView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            GameViewModel viewModel;
            viewModel = (GameViewModel)this.DataContext;

            if (e.Parameter is JugadorConLobby)
            {
                JugadorConLobby model = (JugadorConLobby)e.Parameter;
                viewModel.jugadorCliente = model.jugador;
                viewModel.lobby = model.lobby;
            }
        }

    }
}