using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using PantallasMonopoly.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class lobbyVM : clsVMBase
    {

        #region Propiedades privadas

        private Lobby _lobby;

        private INavigationService _navigationService;

        private DelegateCommand _jugarCommand;
        private Jugador _jugador;

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        #endregion


        #region Propiedades publicas


        public Lobby lobby
        {
            get
            {
                return _lobby;
            }

            set
            {
                _lobby = value;
                NotifyPropertyChanged("lobby");
            }
        }


        #endregion


        #region Constructores

        public lobbyVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            proxy = conexionPadre.proxy;

            proxy.On<Lobby, bool?>("actualizarLobby", actualizarLobby);

            proxy.On("salirDeLobby", salirDeLobby);
            proxy.On<Jugador>("entrarEnPartida", entrarEnPartida);

        }


        #endregion


        #region Crear command

        public DelegateCommand jugarCommand
        {
            get
            {
                _jugarCommand = new DelegateCommand(jugarCommand_Executed, jugarCommand_CanExecute);
                return _jugarCommand;
            }
        }

        private bool jugarCommand_CanExecute()
        {
            bool sePuedeJugar = false;

            if (_lobby != null && _lobby.listadoJugadores.Count == _lobby.maxJugadores)
            {

                sePuedeJugar = true;
            }

            //return sePuedeJugar;
            return true;
        }

        private void jugarCommand_Executed()
        {
            proxy.Invoke("entrarEnPartida", _lobby.nombre);
        }



        #endregion


        #region metodos SignalR


        private async void actualizarLobby(Lobby obj, bool? esCreador)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _lobby = obj;

                        if (esCreador != null && (bool)esCreador)
                        {

                            _jugarCommand.RaiseCanExecuteChanged();

                        }

                    }
                    );

        }

        private async void entrarEnPartida(Jugador jugadorServer)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                if (jugadorServer != null)
                {
                    _navigationService.Navigate(
                        typeof(GameView),
                        new JugadorConLobby()
                        {
                            jugador = jugadorServer,
                            lobby = _lobby
                        }
                    );
                }
                else
                {
                    throw new NotImplementedException("El jugador es null, no se actualizo");
                }
            }
            );
        }

        private async void salirDeLobby()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        conexionPadre.close();
                        _navigationService.Navigate(typeof(MainMenu));
                    }
                    );



        }

        private async void obtenerJugador(Jugador jugador)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _jugador = jugador;
               }
               );
        }

        #endregion







    }
}
