using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PantallasMonopoly.ViewModels
{
    public class searchVM : clsVMBase
    {

        #region Propiedades privadas

        private List<Lobby> _listadoLobby;

        private Lobby _lobbySeleccionado;

        private Jugador _jugadorAIntroducir;

        private DelegateCommand _actualizarCommand;
        private DelegateCommand _confirmarPassCommand;

        private String _visibilidad;
        private String _password;
        private bool _puedeEntrar;

        private INavigationService _navigationService;

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        #endregion

        #region Constructores

        public searchVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _visibilidad = "Collapsed";

            _password = "";

            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("LobbyHub");
            conn.Start().Wait();

            proxy.On<List<Lobby>>("actualizarListadoLobbies", actualizarListadoLobbies);
            proxy.On<bool>("contrasena", contrasena);

            proxy.Invoke("obtenerListadoLobbies").Wait();
        }


        #endregion


        #region Propiedades publicas

        public List<Lobby> listadoLobby
        {
            get
            {
                return _listadoLobby;
            }

            set
            {
                _listadoLobby = value;
            }
        }

        public Lobby lobbySeleccionado
        {
            get
            {
                return _lobbySeleccionado;

            }

            set
            {
                _lobbySeleccionado = value;
                NotifyPropertyChanged("lobbySeleccionado");

                if (_lobbySeleccionado.tieneContrasena())
                {
                    _visibilidad = "Visible";
                    NotifyPropertyChanged("visibilidad");
                }
                else
                {
                    _navigationService.Navigate(typeof(CreatePlayer), _lobbySeleccionado);
                }

            }
        }

        public String visibilidad
        {
            get
            {
                return _visibilidad;
            }

            set
            {
                _visibilidad = value;
                NotifyPropertyChanged("visibilidad");

            }
        }

        public String password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                NotifyPropertyChanged("password");
                _confirmarPassCommand.RaiseCanExecuteChanged();


            }
        }


        public Jugador jugadorAIntroducir
        {
            get
            {
                return _jugadorAIntroducir;
            }

            set
            {
                _jugadorAIntroducir = value;


            }
        }





        #endregion


        #region Actualizar command

        public DelegateCommand actualizarCommand
        {
            get
            {
                _actualizarCommand = new DelegateCommand(actualizarCommand_Executed);
                return _actualizarCommand;
            }
        }


        private void actualizarCommand_Executed()
        {

            proxy.Invoke("obtenerListadoLobbies").Wait();

        }

        #endregion


        #region Confirmar command

        public DelegateCommand confirmarPassCommand
        {
            get
            {
                _confirmarPassCommand = new DelegateCommand(confirmarPassCommand_Executed, confirmarPassCommand_CanExecute);
                return _confirmarPassCommand;
            }
        }

        private bool confirmarPassCommand_CanExecute()
        {
            bool puedeComprobar = false;


            if (!_password.Equals(""))
            {


                puedeComprobar = true;

            }

            return puedeComprobar;
        }

        private void confirmarPassCommand_Executed()
        {

            proxy.Invoke("comprobarContrasena", _lobbySeleccionado.nombre, _password);

        }


        #endregion


        #region Metodos SignalR

        private void actualizarListadoLobbies(List<Lobby> listado)
        {

            _listadoLobby = listado;

        }


        private async void contrasena(bool entra)
        {
            if (entra)
            {
                await proxy.Invoke("unirALobby", _lobbySeleccionado.nombre, _jugadorAIntroducir);
            }

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (entra)
                    {

                        _navigationService.Navigate(typeof(lobbyVM));

                    }
                }
                );



        }

        #endregion


    }
}
