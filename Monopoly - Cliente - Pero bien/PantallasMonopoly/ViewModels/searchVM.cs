using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace PantallasMonopoly.ViewModels
{
    public class searchVM : clsVMBase
    {

        #region Propiedades privadas

        private List<Lobby> _listadoLobby;

        private Lobby _lobbySeleccionado;

        private DelegateCommand _actualizarCommand;

        private String _visibilidad;
        private String _password;

        private INavigationService _navigationService;
      
        #endregion

        #region Constructores

        public searchVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _visibilidad = "Collapsed";

            _password = "";

            proxy = conexionPadre.proxy;

            proxy.On<List<Lobby>>("actualizarListadoLobbies", actualizarListadoLobbies);
            proxy.On<int>("contrasena", contrasena);
            proxy.On("lobbyCompleto", lobbyCompleto);

            proxy.Invoke("obtenerListadoLobbies");          
        }


        #endregion


        #region Propiedades publicas

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }

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
                if (value != null)
                {
                    _lobbySeleccionado = value;
                    NotifyPropertyChanged("lobbySeleccionado");

                    if (_lobbySeleccionado.listadoJugadores.Count < _lobbySeleccionado.maxJugadores)
                    {

                        if (_lobbySeleccionado.tieneContrasena())
                        {

                            mostrarInputContrasena();
                        }
                        else
                        {
                            proxy.Invoke("comprobarContrasena", _lobbySeleccionado.nombre, "");
                        }

                    }
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

            proxy.Invoke("obtenerListadoLobbies");

        }

        #endregion



        #region Metodos SignalR

        private async void actualizarListadoLobbies(List<Lobby> listado)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _listadoLobby = listado;
                   NotifyPropertyChanged("listadoLobby");

                   //Temporal
                   List<Lobby> prueba = new List<Lobby>();

                   prueba.Add(new Lobby("Prueba 1 mieo", "", 3, new Jugador(), new Partida()));
                   prueba.Add(new Lobby("Prueba 2 mieo", "Tengo pass", 3, new Jugador(), new Partida()));
                   prueba.Add(new Lobby("Prueba 3 mieo", "", 3, new Jugador(), new Partida()));
                   _listadoLobby = prueba;
                   NotifyPropertyChanged("listadoLobby");
               }
               );

            


        }

        /*
         1 Entra
         0 Incorrecto
         -1 Lobby completo
         */

        private async void contrasena(int entra)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        switch (entra)
                        {

                            case 1: //Entra

                                _navigationService.Navigate(typeof(CreatePlayer), _lobbySeleccionado);

                                break;
 

                        }
                    
                    }
                    );

       
        }


        private async void lobbyCompleto()
        {

            var messageDialog = new MessageDialog("Lobby is full");
            await messageDialog.ShowAsync();

        }

        #endregion


        #region Otros

        private async void mostrarInputContrasena()
        {

            PasswordBox inputPass = new PasswordBox();
            inputPass.Height = 22;


            ContentDialog logDialog = new ContentDialog()
            {
                Title = "Contraseña para lobby",
                Content = inputPass,
                PrimaryButtonText = "Aceptar"

            };

            ContentDialogResult result = await logDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

                _password = inputPass.Password;
                NotifyPropertyChanged("password");

                logDialog.Hide();
            }

            await proxy.Invoke("comprobarContrasena", _lobbySeleccionado.nombre, _password);


        }

        #endregion




    }
}
