using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using PantallasMonopoly.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class lobbyVM : clsVMBase
    {

        #region Propiedades privadas

        private Lobby _lobby;

        private ObservableCollection<Mensaje> _chat;

        private String _nuevoMensaje;


        private Regex _regex;
        private MatchCollection _match;


        private INavigationService _navigationService;

        private DelegateCommand _jugarCommand;

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

        public ObservableCollection<Mensaje> chat
        {

            get
            {

                return _chat;
            }


            set
            {

                _chat = value;
                NotifyPropertyChanged("chat");
            }

        }

    
        public String nuevoMensaje
        {

            get
            {

                return _nuevoMensaje;
            }


            set
            {

                _nuevoMensaje = value;
                NotifyPropertyChanged("nuevoMensaje");
            }

        }


        #endregion


        #region Constructores

        public lobbyVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _chat = new ObservableCollection<Mensaje>();

            proxy = conexionPadre.proxy;

            proxy.On<Lobby, bool?>("actualizarLobby", actualizarLobby);

            proxy.On("salirDeLobby", salirDeLobby);

            proxy.On<Mensaje>("imprimirMensajeLobby", imprimirMensajeLobby);

            proxy.On<Jugador>("entrarEnPartida", entrarEnPartida);


            _regex = new Regex(@".*[^ ].*");



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
                        NotifyPropertyChanged("lobby");

                        if (esCreador != null && (bool)esCreador)
                        {

                            _jugarCommand.RaiseCanExecuteChanged();

                        }

                    }
                    );

        }

        private async void entrarEnPartida(Jugador _jugador)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (_jugador != null)
                    {
                        _navigationService.Navigate(
                            typeof(GameView),
                            new JugadorConLobby()
                            {
                                jugador = _jugador,
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


        private async void imprimirMensajeLobby(Mensaje message)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {

                       
                        _chat.Insert(0, message);  //Facil
                        NotifyPropertyChanged("chat");

                    }
                    );

        }


        #endregion


        #region Otros

        public void enviarMensaje() {

            _match = _regex.Matches(_nuevoMensaje);

            if (_nuevoMensaje != "" && _match.Count != 0) { //Facil

                proxy.Invoke("enviarMensaje", _nuevoMensaje, false);

                _nuevoMensaje = "";
                NotifyPropertyChanged("nuevoMensaje");

            }

        }


        #endregion



    }
}
