
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PantallasMonopoly.Views;
using PantallasMonopoly.ViewModels;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PantallasMonopoly
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CreateMenu : Page
    {
        public CreateMenu()
        {
            this.InitializeComponent();
            var vm = new createVM(new NavigationService());
            this.DataContext = vm;


        }
      
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainMenu));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            createVM viewModel;
            viewModel = (createVM)this.DataContext;

            if (e.Parameter is Jugador)
            {
                viewModel.creadorSala = (Jugador)e.Parameter;
            }
        }


    }
}
