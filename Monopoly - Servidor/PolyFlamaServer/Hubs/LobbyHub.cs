using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Hubs
{
    public class LobbyHub : Hub
    {
        /*
         * Para crear un nuevo lobby, se requiere que Lobby tenga ya dentro el jugador
         * que lo ha creado (posición 0 en la lista)
         */ 
        public void crearNuevoLobby(Lobby lobby)
        {
            /* 
             * Se crea el grupo, que lo gestiona SignalR
             * (Si se desconecta la última persona, se borra, y si se añade alguien a un grupo que no existe, se crea)
            */
            Groups.Add(Context.ConnectionId, lobby.nombre);
            Jugador jugadorCreador = lobby.listadoJugadores[0];
            LobbyInfo.listadoLobbies.AddOrUpdate(lobby.nombre, lobby, (key, value) => value);
            LobbyInfo.listadoLobbiesNumeroJugadores.AddOrUpdate(lobby.nombre, 1, (key, value) => value);
            Clients.Caller.crearLobby(true);
            Clients.Others.actualizarListadoLobbies(LobbyInfo.listadoLobbies);
        }

        public void comprobarContrasena(string nombreLobby, string contrasena)
        {
            if (LobbyInfo.listadoLobbies[nombreLobby].maxJugadores < LobbyInfo.listadoLobbiesNumeroJugadores[nombreLobby])
            {
                if (LobbyInfo.listadoLobbies[nombreLobby].contrasena == contrasena)
                {
                    //Si la contraseña es correcta, añadimos el jugador al grupo
                    Groups.Add(Context.ConnectionId, nombreLobby);
                    LobbyInfo.listadoLobbiesNumeroJugadores[nombreLobby]++;
                    Clients.Caller.contrasena(true);
                }
                else
                    Clients.Caller.contrasena(false);
            }
            else
                Clients.Caller.lobbyCompleto();
            
        }

        public void unirALobby(string nombreLobby, Jugador jugador)
        {
            //Añadimos el jugador al listado de jugadores del lobby
            LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores.Add(jugador);
            //Avisamos a los otros jugadores de que se ha unido
            Clients.Group(nombreLobby).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);

            if (LobbyInfo.listadoLobbies[nombreLobby].maxJugadores == LobbyInfo.listadoLobbiesNumeroJugadores[nombreLobby])
            {
                //Crear una partida nueva
                Partida partida = GestoraPartida.generarPartidaNueva();
                Random random = new Random();
                //Generar el índice del jugador que va a salir primero
                partida.turnoActual = random.Next(0, LobbyInfo.listadoLobbies[nombreLobby].maxJugadores);
                //Asignar la partida al lobby
                LobbyInfo.listadoLobbies[nombreLobby].partida = partida;
                Clients.Group(nombreLobby).empezarPartida(LobbyInfo.listadoLobbies[nombreLobby]);
            }
                
        }

        public void salirDeLobby(string nombreLobby, Jugador jugador)
        {
            LobbyInfo.listadoLobbiesNumeroJugadores[nombreLobby]--;
            Groups.Remove(Context.ConnectionId, nombreLobby);
            if(jugador != null)
            {
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores.Remove(jugador);
                //Avisamos a los otros jugadores de que se ha desconectado
                Clients.Group(nombreLobby).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
            }
                
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