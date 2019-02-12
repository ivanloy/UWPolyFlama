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
        public void tirarDadoInicial(string nombreLobby, string nombreJugador)
        {
            if (!GameInfo.listadoTiradasIniciales.ContainsKey(nombreLobby))
                GameInfo.listadoTiradasIniciales.AddOrUpdate(nombreLobby, new ConcurrentDictionary<string, int>(), (key, value) => value);

            Random random = new Random();
            int dado1 = random.Next(1, 7); //Upper bound es exclusive
            int dado2 = random.Next(1, 7); //Upper bound es exclusive
            int suma = dado1 + dado2;

            GameInfo.listadoTiradasIniciales[nombreLobby].AddOrUpdate(nombreJugador, suma, (key, value) => value);

            if (GameInfo.listadoTiradasIniciales[nombreLobby].Count == LobbyInfo.listadoLobbies[nombreLobby].maxJugadores)
            {
                GameInfo.listadoTiradasIniciales[nombreLobby].OrderBy(x => x.Value);
                bool hayTiradasIguales = false;
            }
            else
                Clients.Caller.tiradaInicial(suma);
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