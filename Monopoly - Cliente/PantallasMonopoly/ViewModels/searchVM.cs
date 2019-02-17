using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
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


        private DelegateCommand _actualizarCommand;

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        #endregion

        #region Constructores

        public searchVM()
        {

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

        #endregion


        #region Metodos SignalR

        private void actualizarListadoLobbies(List<Lobby> listado)
        {

            _listadoLobby = listado;

        }

        #endregion


    }
}
