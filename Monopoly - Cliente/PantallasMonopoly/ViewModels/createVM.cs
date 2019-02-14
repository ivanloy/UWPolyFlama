using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PantallasMonopoly.ViewModels
{
    public class createVM : clsVMBase
    {

        #region Propiedades privadas

        private String _nombreLobby;
        private int _numeroJugadoresLobby;
        private Jugador _creadorSala;
        private String _passwordLobby;

        private DelegateCommand _crearCommand;

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

        #endregion


        #region Propiedades publicas

        public String nombreLobby
        {
            get
            {
                return _nombreLobby;
            }

            set
            {
                _nombreLobby = value;
                NotifyPropertyChanged("nombreLobby");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        public int numeroJugadoresLobby
        {
            get
            {
                return _numeroJugadoresLobby;
            }

            set
            {
                _numeroJugadoresLobby = value;
                NotifyPropertyChanged("numeroJugadoresLobby");
                _crearCommand.RaiseCanExecuteChanged();
            }
        }

        public String passwordLobby
        {
            get
            {
                return _passwordLobby;
            }

            set
            {
                _passwordLobby = value;
            }
        }

        public Jugador creadorSala
        {

            get
            {

                return _creadorSala;
            }

            set
            {
                _creadorSala = value;
            }
        }

        #endregion


        #region Constructores

        public createVM()
        {
            _nombreLobby = "";
            _numeroJugadoresLobby = 0;
            _passwordLobby = "";

            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("LobbyHub");
            conn.Start();

            proxy.On<bool>("crearLobby", crearLobby);

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

            if (!_nombreLobby.Equals("") && _numeroJugadoresLobby !=0)
            {
                
                sePuedeCrear = true;
            }

            return sePuedeCrear;
        }

        private async void crearCommand_Executed()
        {

            //Aqui hay una llamada al server
            await proxy.Invoke("crearNuevoLobby", new Lobby(_nombreLobby," ", _numeroJugadoresLobby,_creadorSala));

           
        }


        #endregion


        #region Metodos SignalR

        private void crearLobby(bool entra)
        {


            Debug.WriteLine(entra);

        }

        #endregion



    }
}
