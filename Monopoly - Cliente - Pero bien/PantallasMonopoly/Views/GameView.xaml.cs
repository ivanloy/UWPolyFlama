using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
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
using Windows.UI.Xaml.Media.Imaging;
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
            GameViewModel miVM;
            this.InitializeComponent();
            miVM = new GameViewModel(new NavigationService());
            this.DataContext = miVM;
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
        private void Image_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Image image = (Image)sender;
            int row = (int)image.GetValue(Grid.RowProperty);
            int col = (int)image.GetValue(Grid.ColumnProperty);
            BitmapImage bm = getUriCartaRecta(row, col, image.Source);
            if (bm == null)
                img_cartaHover.Source = image.Source;
            else
                img_cartaHover.Source = bm;
                
        }

        private void Image_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            img_cartaHover.Source = null;
            //imageCenter.Source = null;
            //imageCenter2.Source = null;
            //image.Scale(duration: 200, delay: 0, centerX: 0.5f, centerY: 0.5f, scaleX: 1f, scaleY: 1f).StartAsync();
            //image.Offset(offsetX: 0f, offsetY: 0f, duration: 200, delay: 200, easingType: EasingType.Linear);

        }

        private BitmapImage getUriCartaRecta(int row, int col, ImageSource bitmapImage)
        {
            if (row == 10) return null;
            else if (row == 0 && col == 1) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/dora.png"));
            else if (row == 0 && col == 2) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/luck.png"));
            else if (row == 0 && col == 3) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/discord.png"));
            else if (row == 0 && col == 4) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/android.png"));
            else if (row == 0 && col == 5) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/eclipse.png"));
            else if (row == 0 && col == 6) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/ubuntu.png"));
            else if (row == 0 && col == 7) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/deloitte.png"));
            else if (row == 0 && col == 8) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/aws.png"));
            else if (row == 0 && col == 9) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/dual.png"));
            else if (row == 1 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/pccomponentes.png"));
            else if (row == 2 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/xiaomi.png"));
            else if (row == 3 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/comunidad.png"));
            else if (row == 4 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/polvillo.png"));
            else if (row == 5 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/intelij.png"));
            else if (row == 6 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/luck.png"));
            else if (row == 7 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/twitch.png"));
           // else if (row == 8 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/azureS1.png"));
            else if (row == 9 && col == 10) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/java.png"));
            else if (row == 1 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/conan.png"));
            else if (row == 2 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/github.png"));
            else if (row == 3 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/comunidad.png"));
            else if (row == 4 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/chollometro.png"));
            else if (row == 5 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/netbeans.png"));
            else if (row == 6 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/overwatch.png"));
            else if (row == 7 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/myanimelist.png"));
        //    else if (row == 8 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/azure.png"));
            else if (row == 9 && col == 0) return new BitmapImage(new Uri("ms-appx://ExamenDI/CustomAssets/RECTAS/cookie.png"));
            return null;
        }

    }
}