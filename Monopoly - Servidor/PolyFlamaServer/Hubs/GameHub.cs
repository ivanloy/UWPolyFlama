using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;
using System.Collections.Concurrent;

namespace PolyFlamaServer.Hubs
{
    public class GameHub : Hub
    {
        public void tirarDados(string nombreLobby)
        {
            Random random = new Random();
            int dado1 = random.Next(1, 7);
            int dado2 = random.Next(1, 7);

			int posicionActual = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].posicion;

			//Actualizamos el lobby con los dados y lo pasamos a todos
			LobbyInfo.listadoLobbies[nombreLobby].partida.arrayDados = new int[] { dado1, dado2 };
			LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
            Clients.Group(nombreLobby).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
            Clients.Group(nombreLobby).moverCasillas();
        }

        //Cuando un jugador se desconecte
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        //Cuando un jugador se conecte
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}