using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class GameViewModel : clsVMBase
    {

        private Lobby _lobby;
        private DelegateCommand _tirarDadosCommand;
        private Jugador _jugadorCliente;

        public Jugador jugadorCliente {
            get { return _jugadorCliente; }
            set { _jugadorCliente = value; }
        }

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }
        public Lobby lobby {
            get { return _lobby; }
            set {
                _lobby = value;
                NotifyPropertyChanged("lobby");
            }
        }
        public DelegateCommand tirarDadosCommand {
            get {
                _tirarDadosCommand = new DelegateCommand(_tirarDadosCommand_Executed, _tirarDadosCommand_CanExecute);
                return _tirarDadosCommand;
            }
        }
        private bool _tirarDadosCommand_CanExecute()
        {
            return true;
        }

        private async void _tirarDadosCommand_Executed()
        {
            await proxy.Invoke("tirarDados", lobby.nombre);
            NotifyPropertyChanged("lobby");
        }

        public GameViewModel()
        {
            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("GameHub");
            conn.Start();
            proxy.On<Lobby>("actualizarLobby", actualizarLobby);
            proxy.On("moverCasillas", moverCasillas);
            proxy.On("comprarPropiedad", comprarPropiedad);
            proxy.On("conectar", conectar);
            lobby = new Lobby();
        }

        private async void comprarPropiedad()
        {

                    dialogComprar();
 
        }

        private async void dialogComprar()
    {
        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
           async () =>
           {
               ContentDialog logDialog = new ContentDialog()
                    {
                        Title = "Comprar Propiedad",
                        Content = "Quiere comprah?",
                        PrimaryButtonText = "Po si",
                        CloseButtonText = "Po no"

                    };

               ContentDialogResult result = await logDialog.ShowAsync();
           }
           );
        }

        private async void moverCasillas()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                }
            );
        }

        private async void actualizarLobby(Lobby arg1)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this._lobby = arg1; //AHHHHHH NO BINDEA
                }
            );
        }
        private async void conectar()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   proxy.Invoke("conectar", _lobby.nombre, _jugadorCliente.nombre);
               }
            );
        }

    }
}