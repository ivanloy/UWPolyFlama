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
        private static readonly string nombreChatGlobal = "global";
        private static readonly object lockUnirSalir = new object();
        private static readonly object lockCrear = new object();
        private static readonly object lockDisconnect = new object();

        /*
         * Para crear un nuevo lobby, se requiere que Lobby tenga ya dentro el jugador
         * que lo ha creado (posición 0 en la lista)
         */ 
        public void crearNuevoLobby(Lobby lobby)
        {
            //Bloquear el acceso por si dos personas crean lobbies a la vez
            lock (lockCrear)
            {
                bool lobbyExists = false;

                foreach (KeyValuePair<string, DatosLobby> datoLobby in LobbyInfo.listadoLobbies)
                {
                    if (datoLobby.Value.lobby.nombre == lobby.nombre)
                    {
                        lobbyExists = true;
                        break;
                    }
                }

                if(lobbyExists)
                    Clients.Caller.crearLobby(false);
                else
                {
                    //Sacamos el jugador creador
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
                    Thread.Sleep(500);
                    Clients.Caller.imprimirMensajeLobby(new Mensaje("[SYSTEM] Remember not to share your password with anyone 🤫", "Red"));
                    Clients.Caller.imprimirMensajeLobby(new Mensaje("[LOBBY] Lobby created successfully 👍", "#2196F3"));
                }
            }
        }

        //Método para comprobar la contraseña
        public void comprobarContrasena(string nombreLobby, string contrasena)
        {
            //Si hay menos jugadores conectados que el maxJugadores
            if (LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Count < LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores)
            {
                //Si la contraseña es correcta
                if (LobbyInfo.listadoLobbies[nombreLobby].lobby.contrasena == contrasena)
                {
                    //Añadimos el jugador al grupo
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores++;
                    Clients.Caller.contrasena(1);
                }
                else
                    Clients.Caller.contrasena(0);
            }
            else
                Clients.Caller.contrasena(-1);

        }

        //Método para unir al lobby
        public void unirALobby(string nombreLobby, Jugador jugador)
        {
            bool puedeContinuar = false;

            //Asegurar que solo uno de los clientes que está accediendo pueda meter el usuario
            lock (lockUnirSalir)
            {
                if(LobbyInfo.listadoLobbies.ContainsKey(nombreLobby))
                {
                    List<Ficha> fichasCogidas = new List<Ficha>();
                    bool contieneFicha = false;
                    foreach (Jugador jug in LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores)
                    {
                        if (jug.ficha.nombre == jugador.ficha.nombre)
                        {
                            contieneFicha = true;
                            break;
                        }
                    }

                    //Comprobamos que el jugador no esté metido en la sala ya
                    if (LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.ContainsKey(jugador.nombre) || contieneFicha)
                        Clients.Caller.unirALobby(null);
                    else
                    {
                        //Añadimos el jugador al listado de jugadores del lobby
                        LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.AddOrUpdate(jugador.nombre, Context.ConnectionId, (key, value) => value);
                        LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Add(jugador);
                        puedeContinuar = true;
                    }
                }
            }

            if(puedeContinuar)
            {
                //Obtenemos la connectionID del jugador creador
                string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Key == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre).Value;

                //Avisamos a los otros jugadores de que se ha unido
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                {
                    if (connectionId != Context.ConnectionId)
                    {
                        Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby, connectionId == connectionIDCreador);
                        Clients.Client(connectionId).imprimirMensajeLobby(new Mensaje($"[LOBBY] {jugador.nombre} has joined 😄", "#2196F3"));
                    }
                    else
                    {
                        Clients.Caller.unirALobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
                        Thread.Sleep(500);
                        Clients.Client(connectionId).imprimirMensajeLobby(new Mensaje($"[SYSTEM] Remember not to share your password with anyone 🤫", "Red"));
                    }

                }
            }
        }

        //Función para empezar la partida
        public void empezarPartida(string nombreLobby)
        {
            if (LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Count)
            {
                //Crear una partida nueva
                Partida partida = GestoraPartida.generarPartidaNueva();
                Random random = new Random();
                
                //Generar el índice del jugador que va a salir primero
                partida.turnoActual = random.Next(0, LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores);

                //Asignar la partida al lobby
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida = partida;
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partidaEmpezada = true;

                //Avisamos a los otros jugadores para empezar la partida
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                {
                    Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
                    Thread.Sleep(200); //Para evitar dobles llamadas
                    Clients.Client(connectionId).empezarPartida();
                }
            }
        }
        
        //Función para salir del lobby
        public void salirDeLobby(string nombreLobby)
        {
            //Bloqueamos el acceso por si dos personas le han dado a salir a la vez
            lock(lockUnirSalir)
            {
                if(LobbyInfo.listadoLobbies.ContainsKey(nombreLobby))
                {
                    //Buscamos el jugador
                    string connection = Context.ConnectionId;
                    string nombreJugador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Value == Context.ConnectionId).Key;
                    Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Single(x => x.nombre == nombreJugador);

                    //Quitamos en 1 el número de jugadores
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores--;

                    //Obtenemos la connectionID del jugador creador
                    string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Key == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre).Value;

                    //Si es el creador el que se ha desconectado
                    if (connectionIDCreador == Context.ConnectionId)
                    {
                        DatosLobby datosLobby;
                        LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out datosLobby);
                        foreach (string connectionId in datosLobby.listadoJugadoresConnection.Values)
                        {
                            Clients.Client(connectionId).salirDeLobby();
                        }
                    }
                    else
                    {
                        //Borramos al jugador de la lista de jugadores y de las conexiones
                        string connectionID;
                        LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Remove(jugador);
                        LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.TryRemove(jugador.nombre, out connectionID);

                        //Avisamos a los otros jugadores de que se ha desconectado
                        foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        {
                            Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby, connectionId == connectionIDCreador);
                            Clients.Client(connectionId).imprimirMensajeLobby(new Mensaje($"[LOBBY] {nombreJugador} has left the lobby 😭", "Gray"));
                        }

                        Clients.Caller.salirDeLobby();
                    }
                }
            }
        }

        //Método para obtener datos de un jugador
        public void obtenerJugador()
        {
            string connectionId = Context.ConnectionId;
            Jugador jugador;
            string nombreJugador = null;
            string nombreLobby = null;

            foreach (DatosLobby datos in LobbyInfo.listadoLobbies.Values)
            {
                try
                {
                    nombreJugador = datos.listadoJugadoresConnection.Single(x => x.Value == connectionId).Key;
                    nombreLobby = datos.lobby.nombre;
                    break;
                }
                catch (InvalidOperationException) { }
            }

            if (nombreJugador != null)
            {
                jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Single(x => x.nombre == nombreJugador);
                Clients.Caller.obtenerJugador(jugador);
            }
            else
                Clients.Caller.obtenerJugador(null);
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

        #region Chats
        //Método para hacer saber a todos que alguien ha entrado en Search mostrando un mensaje en el chat
        public void unirChatGlobal()
        {
            Groups.Add(Context.ConnectionId, nombreChatGlobal);
            Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje("[GLOBAL] Someone joined the global chat, say 👋!", "#8BC34A"));
        }

        //Método para hacer saber a todos que alguien se ha salido de Search. Si el nombreLobby es null, se ha salido al menú, si no, ha entrado a un lobby
        public void salirChatGlobal(string nombreLobby = null)
        {
            Groups.Remove(Context.ConnectionId, nombreChatGlobal);

            if(nombreLobby == null)
                Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje("[GLOBAL] Someone left the global chat 😭", "#8BC34A"));
            else
                Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje($"[GLOBAL] Someone joined {nombreLobby}", "#2196F3"));

        }

        public void enviarMensaje(string mensaje, bool esGlobal = false)
        {
            Random random = new Random();
            int chanceOfUwu = random.Next(1, 11);

            if(esGlobal)
                Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje($"[GLOBAL] {mensaje}{(chanceOfUwu == 10 ? " UwU" : "")}", "Black"));
            else
            {
                string connectionId = Context.ConnectionId;
                ConcurrentDictionary<string, string> conexiones = null;
                string nombreJugador = null;

                foreach (DatosLobby datos in LobbyInfo.listadoLobbies.Values)
                {
                    try
                    {
                        nombreJugador = datos.listadoJugadoresConnection.Single(x => x.Value == connectionId).Key;
                        conexiones = datos.listadoJugadoresConnection;
                        break;
                    }
                    catch (InvalidOperationException) { }
                }

                if (nombreJugador != null)
                {
                    foreach (string conId in conexiones.Values)
                        Clients.Client(conId).imprimirMensajeLobby(new Mensaje($"[LOBBY] {nombreJugador}: {mensaje}{(chanceOfUwu == 10 ? " UwU" : "")}", "Black"));
                }
                else
                    Clients.Caller.imprimirMensajeLobby(new Mensaje($"[SYSTEM] There was an error sending the message", "Red"));

            }
        }
        #endregion

        //Cuando un jugador se desconecte
        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, nombreChatGlobal);

            //Bloqueamos el acceso por si dos personas le han dado a salir a la vez
            lock (lockDisconnect)
            {
                #region Obtener jugador

                string myConnectionId = Context.ConnectionId;
                Jugador jugador = null;
                string nombreJugador = null;
                string nombreLobby = null;

                foreach (DatosLobby datos in LobbyInfo.listadoLobbies.Values)
                {
                    try
                    {
                        nombreJugador = datos.listadoJugadoresConnection.Single(x => x.Value == myConnectionId).Key;
                        nombreLobby = datos.lobby.nombre;
                        break;
                    }
                    catch (InvalidOperationException) { }
                }

                if (nombreJugador != null)
                    jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Single(x => x.nombre == nombreJugador);

                #endregion

                if (jugador != null)
                {
                    //Quitamos en 1 el número de jugadores
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores--;

                    //Obtenemos la connectionID del jugador creador
                    string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Key == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre).Value;

                    //Si es el creador el que se ha desconectado
                    if (connectionIDCreador == myConnectionId)
                    {
                        DatosLobby datosLobby;
                        LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out datosLobby);
                        foreach (string connectionId in datosLobby.listadoJugadoresConnection.Values)
                        {
                            Clients.Client(connectionId).salirDeLobby();
                        }
                    }
                    else
                    {
                        //Borramos al jugador de la lista de jugadores y de las conexiones
                        string connectionID;
                        LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Remove(jugador);
                        LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.TryRemove(jugador.nombre, out connectionID);

                        //Avisamos a los otros jugadores de que se ha desconectado
                        foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        {
                            Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby, connectionId == connectionIDCreador);
                        }
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
        
    }
}