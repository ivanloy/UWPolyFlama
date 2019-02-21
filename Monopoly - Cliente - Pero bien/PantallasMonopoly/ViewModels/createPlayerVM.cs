using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using PantallasMonopoly.Models.Enums;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class createPlayerVM : clsVMBase
    {

        #region Propiedades privadas

        private String _nickname;
        private List<Ficha> _listadoFichas;
        private Ficha _fichaSeleccionada;

        private Lobby _lobbyAEntrar;

        private DelegateCommand _crearCommand;
        private INavigationService _navigationService;



        #endregion


        #region Propiedades publicas

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        public String nickname
        {
            get
            {
                return _nickname;
            }

            set
            {
                _nickname = value;
                NotifyPropertyChanged("nickname");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Ficha> listadoFichas
        {
            get
            {
                return _listadoFichas;
            }

            set
            {
                _listadoFichas = value;
                NotifyPropertyChanged("listadoFichas");
            }
        }

        public Ficha fichaSeleccionada
        {
            get
            {
                return _fichaSeleccionada;
            }

            set
            {
                _fichaSeleccionada = value;
                NotifyPropertyChanged("fichaSeleccionada");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        public Lobby lobbyAEntrar
        {
            get
            {
                return _lobbyAEntrar;
            }

            set
            {
                _lobbyAEntrar = value;
            }
        }

        #endregion


        #region Constructores

        public createPlayerVM(INavigationService navigationService)
        {
            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("LobbyHub");
            conn.Start();

            _navigationService = navigationService;
            _nickname = "";
            _listadoFichas = generadorFichas.listadoFichas();
            _fichaSeleccionada = new Ficha();

            proxy.On<Lobby>("unirALobby", unirALobby);

        }



        #endregion


        #region Crear command

        public DelegateCommand crearCommand
        {
            get
            {
                _crearCommand = new DelegateCommand(crearCommand_Executed, crearCommand_CanExecute);
                return _crearCommand;
            }
        }

        private bool crearCommand_CanExecute()
        {
            bool sePuedeCrear = false;

            if (!_nickname.Equals("") && _fichaSeleccionada.nombre != null)
            {

                sePuedeCrear = true;
            }

            return sePuedeCrear;
        }

        private void crearCommand_Executed()
        {
            if (_lobbyAEntrar == null)
            {

                _navigationService.Navigate(typeof(CreateMenu), new Jugador(_nickname, _fichaSeleccionada));

            }
            else
            {

                proxy.Invoke("unirALobby", _lobbyAEntrar.nombre, new Jugador(_nickname, _fichaSeleccionada));

               
            }



        }


        #endregion


        #region Metodos SignalR

        private async void unirALobby(Lobby lobby)
        {

            if (lobby != null)
            {

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            _navigationService.Navigate(typeof(LobbyMenu), lobby);
                            
                        }
                        );

            }
            else
            {

               //Mensaje de error

            }

        }

        #endregion

    }
}
