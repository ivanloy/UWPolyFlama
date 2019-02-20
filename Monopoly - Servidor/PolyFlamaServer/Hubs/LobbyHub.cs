using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;
using System.Collections.Concurrent;
using System.Collections;
using System.Threading;

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
            bool lobbyExists = false;
            foreach(KeyValuePair<string, DatosLobby> datoLobby in LobbyInfo.listadoLobbies)
            {
                if(datoLobby.Value.lobby.nombre == lobby.nombre)
                {
                    lobbyExists = true;
                    break;
                }
            }
            if(!lobbyExists)
            {
                /* 
                 * Se crea el grupo, que lo gestiona SignalR
                 * (Si se desconecta la última persona, se borra, y si se añade alguien a un grupo que no existe, se crea)
                */
                Groups.Add(Context.ConnectionId, lobby.nombre);
                Jugador jugadorCreador = lobby.listadoJugadores[0];

                //Creación y adición de datos del lobby
                DatosLobby datosLobby = new DatosLobby();
                datosLobby.lobby = lobby;
                datosLobby.numeroJugadores = 1;
                datosLobby.listadoJugadoresConnection.AddOrUpdate(jugadorCreador.nombre, Context.ConnectionId, (key, value) => value);

                //Creamos la entrada del diccionario de ese lobby
                LobbyInfo.listadoLobbies.AddOrUpdate(lobby.nombre, datosLobby, (key, value) => value);

                //Llamamos al creador indicándole que todo ha ido bien 👌👌👌
                Clients.Caller.crearLobby(true);
                Clients.Others.actualizarListadoLobbies(LobbyInfo.listadoLobbies);
            }
            else
                Clients.Caller.crearLobby(false);

        }

        public void comprobarContrasena(string nombreLobby, string contrasena)
        {
            if (LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores < LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores)
            {
                if (LobbyInfo.listadoLobbies[nombreLobby].lobby.contrasena == contrasena)
                {
                    //Si la contraseña es correcta, añadimos el jugador al grupo
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores++;
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
            Groups.Add(Context.ConnectionId, nombreLobby).ContinueWith(task =>
            {
                Thread.Sleep(new Random().Next(50, 200));
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Add(jugador);
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.AddOrUpdate(jugador.nombre, Context.ConnectionId, (key, value) => value);
                
                //Avisamos a los otros jugadores de que se ha unido
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                {
                    Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
                }
            });
        }

        public void empezarPartida(string nombreLobby)
        {
            if (LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores == LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores)
            {
                //Crear una partida nueva
                Partida partida = GestoraPartida.generarPartidaNueva();
                Random random = new Random();
                
                //Generar el índice del jugador que va a salir primero
                partida.turnoActual = random.Next(0, LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores);

                //Asignar la partida al lobby
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida = partida;
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partidaEmpezada = true;
                Clients.Group(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).empezarPartida(LobbyInfo.listadoLobbies[nombreLobby].lobby);
            }
        }

        public void salirDeLobby(string nombreLobby, Jugador jugador)
        {
            LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores--;
            Groups.Remove(Context.ConnectionId, LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value);
            if(jugador != null)
            {
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Remove(jugador);
                //Avisamos a los otros jugadores de que se ha desconectado
                Clients.Group(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
                
                if (LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores == 0)
                {
                    DatosLobby lobbyRemoved;
                    LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out lobbyRemoved);
                }

                Clients.Caller.salirDeLobby();
            }
                
        }

        //Actualizar el listado completo de lobbies
        public void obtenerListadoLobbies()
        {
            List<Lobby> listadoLobbies = new List<Lobby>();

            foreach(DatosLobby datosLobby in LobbyInfo.listadoLobbies.Values)
            {
                listadoLobbies.Add(datosLobby.lobby);
            }

            Clients.Caller.actualizarListadoLobbies(listadoLobbies);
        }

        /*Cuando un jugador se desconecte
        public override Task OnDisconnected(bool stopCalled)
        {
            if(!stopCalled)
            {
                bool esJugadorCreador = false;
                string nombreLobby = "";
                foreach (DatosLobby datosLobby in LobbyInfo.listadoLobbies.Values)
                {
                    if (datosLobby.listadoJugadoresConnection.ContainsKey(datosLobby.lobby.listadoJugadores.First().nombre))
                    {
                        esJugadorCreador = true;
                        nombreLobby = datosLobby.lobby.nombre;
                        break;
                    }
                    else if (datosLobby.listadoJugadoresConnection.Values.ToList().Exists(x => x == Context.ConnectionId))
                    {
                        nombreLobby = datosLobby.lobby.nombre;
                        break;
                    }
                }

                if (esJugadorCreador)
                {
                    DatosLobby datosLobby;
                    LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out datosLobby);
                    foreach (string connectionId in datosLobby.listadoJugadoresConnection.Values)
                    {
                        Clients.Client(connectionId).salirDeLobby();
                        Groups.Remove(connectionId, nombreLobby);
                    }

                }
                else if(nombreLobby != "")
                {
                    Groups.Remove(Context.ConnectionId, nombreLobby);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        //Cuando un jugador se conecte
        public override Task OnConnected()
        {
            return base.OnConnected();
        }*/
    }
}