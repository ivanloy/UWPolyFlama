using Microsoft.AspNet.SignalR.Client;
using PantallasMonopoly.Connection;
using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.ViewModels
{
    public class GameViewModel : clsVMBase
    {

        private Lobby _lobby;
        private DelegateCommand _tirarDadosCommand;
        private Jugador _jugadorCliente;

        public Jugador jugadorCliente {
            get { return _jugadorCliente; }
            set { _jugadorCliente = value; }
        }

        public HubConnection conn { get; set; }
        public IHubProxy proxy { get; set; }
        public Lobby lobby {
            get { return _lobby; }
            set {
                _lobby = value;
                NotifyPropertyChanged("lobby");
            }
        }
        public DelegateCommand tirarDadosCommand {
            get {
                _tirarDadosCommand = new DelegateCommand(_tirarDadosCommand_Executed, _tirarDadosCommand_CanExecute);
                return _tirarDadosCommand;
            }
        }
        private bool _tirarDadosCommand_CanExecute()
        {
            return true;
        }

        private async void _tirarDadosCommand_Executed()
        {
           await proxy.Invoke("tirarDados", lobby.nombre);
        }

        public GameViewModel()
        {
            conn = new HubConnection("http://polyflama.azurewebsites.net/");
            proxy = conn.CreateHubProxy("GameHub");
            conn.Start();
            proxy.On<Lobby, bool?>("actualizarLobby", actualizarLobby);
            proxy.On("connected", connected);
            lobby = new Lobby();
        }

        private void actualizarLobby(Lobby arg1, bool? arg2)
        {
            this._lobby = arg1;
            NotifyPropertyChanged("lobby");
        }
        private void connected()
        {
            proxy.Invoke("connected", _lobby.nombre, _jugadorCliente.nombre);
        }

    }
}