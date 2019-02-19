using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class lobbyVM : clsVMBase
    {

        #region Propiedades privadas

        private Lobby _lobby;

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
  

        #endregion


        #region Constructores

        public lobbyVM(INavigationService navigationService)
        {


            //conn = conexionPadre.conn;
            proxy = conexionPadre.proxy;

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

            if (_lobby != null && _lobby.listadoJugadores.Count >=2)
            {

                sePuedeJugar = true;
            }

            return sePuedeJugar;
        }

        private void jugarCommand_Executed()
        {

            //Aqui hay una llamada al server

            //_navigationService.Navigate(); Aqui llamara a la partida
           
        }


        #endregion

     



    }
}
