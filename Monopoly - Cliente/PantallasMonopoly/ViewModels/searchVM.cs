using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.ViewModels
{
    public class searchVM : clsVMBase
    {

        #region Propiedades privadas

        private List<Lobby> _listadoLobby;

        private Lobby _lobbySeleccionado;

        private DelegateCommand _actualizarCommand;

        private String _visibilidad;

        private INavigationService _navigationService;

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        #endregion

        #region Constructores

        public searchVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _visibilidad = "Collapsed";

            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("LobbyHub");
            conn.Start().Wait();
          
            proxy.On<List<Lobby>>("actualizarListadoLobbies", actualizarListadoLobbies);

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
                    _navigationService.Navigate(typeof(CreatePlayer), _lobbySeleccionado.nombre);
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


        #region Metodos SignalR

        private void actualizarListadoLobbies(List<Lobby> listado)
        {

            _listadoLobby = listado;

        }

        #endregion


    }
}
