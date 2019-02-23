using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private ObservableCollection<Mensaje> _chatGlobal;

        private String _nuevoMensaje;

        private DelegateCommand _actualizarCommand;

        private String _visibilidad;
        private String _password;

        private Regex _regex;
        private MatchCollection _match;

        private INavigationService _navigationService;

        #endregion

        #region Constructores

        public searchVM(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _visibilidad = "Collapsed";

            _password = "";

            _chatGlobal = new ObservableCollection<Mensaje>();

            proxy = conexionPadre.proxy;

            proxy.Invoke("unirChatGlobal");

            proxy.On<List<Lobby>>("actualizarListadoLobbies", actualizarListadoLobbies);
            proxy.On<int>("contrasena", contrasena);
            proxy.On<Mensaje>("imprimirMensajeGlobal", imprimirMensajeGlobal);

            proxy.Invoke("obtenerListadoLobbies");

            _regex = new Regex(@".*[^ ].*");
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

        public ObservableCollection<Mensaje> chatGlobal
        {

            get
            {

                return _chatGlobal;
            }


            set
            {

                _chatGlobal = value;
                NotifyPropertyChanged("chatGlobal");
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

               }
               );


        }

     
        private async void contrasena(int entra)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        switch (entra)
                        {

                            case 1: //Entra

                                await conexionPadre.proxy.Invoke("salirChatGlobal", _lobbySeleccionado.nombre);
                                _navigationService.Navigate(typeof(CreatePlayer), _lobbySeleccionado);


                                break;


                            case -1: //Completo

                                var messageDialog = new MessageDialog("Lobby is full");
                                await messageDialog.ShowAsync();

                                break;

                            case 0: //Contraseña cacas

                                var messageDialog2 = new MessageDialog("Incorrect password");
                                await messageDialog2.ShowAsync();

                                break;


                        }

                    }
                    );


        }

        private async void imprimirMensajeGlobal(Mensaje message)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {


                        _chatGlobal.Insert(0, message);  //Facil
                        NotifyPropertyChanged("chatGlobal");

                    }
                    );

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

        public void enviarMensaje()
        {

            _match = _regex.Matches(_nuevoMensaje);

            if (_nuevoMensaje != "" && _match.Count != 0)
            { //Facil

                proxy.Invoke("enviarMensaje", _nuevoMensaje, true);

                _nuevoMensaje = "";
                NotifyPropertyChanged("nuevoMensaje");

            }

        }

        #endregion




    }
}
