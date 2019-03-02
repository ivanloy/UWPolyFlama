using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class GameViewModel : clsVMBaseHilo
    {

        #region Propiedades privadas

        private Lobby _lobby;
        private DelegateCommand _tirarDadosCommand;
        private Jugador _jugadorCliente;

        #endregion

        #region Propiedades publicas

        public Jugador jugadorCliente
        {
            get { return _jugadorCliente; }
            set { _jugadorCliente = value; }
        }

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        public Lobby lobby
        {
            get { return _lobby; }
            set
            {
                _lobby = value;
                NotifyPropertyChanged("lobby");
            }
        }

        #endregion

        #region Tirar dados command


        public DelegateCommand tirarDadosCommand
        {
            get
            {
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
            //NotifyPropertyChanged("lobby"); //Esto ya no hace falta con el nuevo notify
        }

        #endregion

        #region Constructores

        public GameViewModel()
        {
            conn = new HubConnection("http://localhost:51144/");
            proxy = conn.CreateHubProxy("GameHub");
            conn.Start();
            proxy.On<Lobby>("actualizarLobby", actualizarLobby);
            proxy.On("moverCasillas", moverCasillas);
            proxy.On("comprarPropiedad", comprarPropiedad);
            proxy.On("conectar", conectar);
            proxy.On("todosConectados", todosConectados);
            proxy.On("partidaPerdida", partidaPerdida);
            lobby = new Lobby();
        }

        #endregion

        #region Metodos Signalr

        private void comprarPropiedad()
        {

            dialogComprar();

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
                    NotifyPropertyChanged("lobby");
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

        private async void todosConectados()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   var messageDialog = new MessageDialog("Everyone has loaded in! The game can begin. GLHF");
                   await messageDialog.ShowAsync();
               }
            );
        }

        private async void partidaPerdida()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   var messageDialog = new MessageDialog("You lost :( GL next time");
                   await messageDialog.ShowAsync();
               }
            );
        }

        #endregion

        #region Otros

        private async void dialogComprar()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   ContentDialog logDialog = new ContentDialog()
                   {
                       Title = "Buy property",
                       Content = "Do you want to buy this property?",
                       PrimaryButtonText = "For sure!",
                       CloseButtonText = "Meh, maybe next time"

                   };

                   ContentDialogResult result = await logDialog.ShowAsync();

                   if (result == ContentDialogResult.Primary)
                   {
                       await proxy.Invoke("comprarPropiedad", _lobby.nombre);
                   }
               }
               );
        }

        #endregion





    }
}