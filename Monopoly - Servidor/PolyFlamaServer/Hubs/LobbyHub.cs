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
            GameInfo.listadoLobbies.AddOrUpdate(lobby.nombre, lobby, (key, value) => value);
            GameInfo.listadoLobbiesNumeroJugadores.AddOrUpdate(lobby.nombre, 1, (key, value) => value);
            Clients.Caller.crearLobby(true);
            Clients.Others.actualizarListadoLobbies(GameInfo.listadoLobbies);
        }

        public void comprobarContrasena(string nombreLobby, string contrasena)
        {
            if (GameInfo.listadoLobbies[nombreLobby].contrasena == contrasena)
            {
                //Si la contraseña es correcta, añadimos el jugador al grupo
                Groups.Add(Context.ConnectionId, nombreLobby);
                Clients.Caller.contrasena(true);
            }
            else
                Clients.Caller.unirALobby(false);
        }

        public void unirALobby(string nombreLobby, Jugador jugador)
        {
            //Añadimos el jugador al listado de jugadores del lobby
            GameInfo.listadoLobbies[nombreLobby].listadoJugadores.Add(jugador);
            //Avisamos a los otros jugadores de que se ha unido
            Clients.OthersInGroup(nombreLobby).actualizarListadoJugadores(GameInfo.listadoLobbies[nombreLobby]);
            //Unimos 
            Clients.Caller.unirALobby(true, GameInfo.listadoLobbies[nombreLobby]);
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