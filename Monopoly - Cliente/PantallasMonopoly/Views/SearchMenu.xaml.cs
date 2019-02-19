﻿
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PantallasMonopoly.Views;
using PantallasMonopoly.ViewModels;
using PantallasMonopoly.Util;
using PantallasMonopoly.Models;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PantallasMonopoly
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class SearchMenu : Page
    {
        public SearchMenu()
        {
            this.InitializeComponent();
            var vm = new searchVM(new NavigationService());
            this.DataContext = vm;

        }
      
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            searchVM viewModel;
            viewModel = (searchVM)this.DataContext;

            if (e.Parameter is Jugador)
            {
                viewModel.jugadorAIntroducir = (Jugador)e.Parameter;
            }
        }


    }
}
