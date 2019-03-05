using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using PantallasMonopoly.Views;
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
        private bool _esMiTurno;
        private INavigationService _navigationService;

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
        public Colores colores { get; set; }

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
            return _esMiTurno;
        }

        private async void _tirarDadosCommand_Executed()
        {
            _esMiTurno = false;
            _tirarDadosCommand.RaiseCanExecuteChanged();
            playDiceSound();
            await proxy.Invoke("tirarDados", lobby.nombre);
            //NotifyPropertyChanged("lobby"); //Esto ya no hace falta con el nuevo notify
        }

        #endregion

        #region Constructores

        public GameViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            colores = new Colores();
            conn = new HubConnection(conexionPadre.conexionURL);
            proxy = conn.CreateHubProxy("GameHub");
            conn.Start();
            proxy.On<Lobby>("actualizarLobby", actualizarLobby);
            //proxy.On("moverCasillas", moverCasillas);
            proxy.On<Propiedad>("comprarPropiedad", comprarPropiedad);
            proxy.On("conectar", conectar);
            proxy.On("todosConectados", todosConectados);
            proxy.On("partidaPerdida", partidaPerdida);
            proxy.On("partidaGanada", partidaGanada);
            proxy.On("esTuTurno", esTuTurno);
            proxy.On("salirDePartida", salirDePartida);
            proxy.On<string>("mostrarMensaje", mostrarMensaje);
            lobby = new Lobby();
        }


        #endregion

        #region Metodos Signalr

        private void comprarPropiedad(Propiedad propiedad)
        {

            dialogComprar(propiedad);

        }
        private async void esTuTurno()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async() =>
                {
                    _esMiTurno = true;
                    _tirarDadosCommand.RaiseCanExecuteChanged();

                    var messageDialog = new MessageDialog("It's your turn");
                    await messageDialog.ShowAsync();
                }
                );
        }

        private async void moverCasillas()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _tirarDadosCommand.RaiseCanExecuteChanged();
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

        private async void partidaGanada()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   var messageDialog = new MessageDialog("You won :D Way to rekt em boi");
                   await messageDialog.ShowAsync();
               }
            );
        }
        private async void salirDePartida()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                  () =>
                  {
                      conn.Stop();
                      proxy = null;
                      _navigationService.Navigate(typeof(MainMenu));
                  }
                  );
        }
        private async void mostrarMensaje(string msn)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   var messageDialog = new MessageDialog(msn);
                   await messageDialog.ShowAsync();
               }
               );
        }

        #endregion

        #region Otros

        private async void dialogComprar(Propiedad propiedad)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   ContentDialog logDialog = new ContentDialog()
                   {
                       Title = "Buy property",
                       Content = $"Do you want to buy {propiedad.nombre} for ${propiedad.precio}?",
                       PrimaryButtonText = "For sure!",
                       CloseButtonText = "Meh, maybe next time"

                   };

                   ContentDialogResult result = await logDialog.ShowAsync();

                   //Llama con true si quiere comprar, false si le ha dado al otro botón
                   await proxy.Invoke("comprarPropiedad", _lobby.nombre, result == ContentDialogResult.Primary);
               }
               );
        }

        private async void playDiceSound()
        {
            MediaElement mysong = new MediaElement();
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("dice.wav");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            mysong.SetSource(stream, file.ContentType);
            mysong.Play();
        }

        #endregion





    }
}