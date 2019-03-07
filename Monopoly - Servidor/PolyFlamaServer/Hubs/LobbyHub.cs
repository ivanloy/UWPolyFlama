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

        /*
         * Para crear un nuevo lobby, se requiere que Lobby tenga ya dentro el jugador
         * que lo ha creado (posición 0 en la lista)
         */ 
        public void crearNuevoLobby(Lobby lobby)
        {
            //Bloquear el acceso por si dos personas crean lobbies a la vez
            lock (Locks.lockCrear)
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

                    //Añadimos un nuevo lock al diccionario
                    Locks.lockUnirSalir.Add(datosLobby.lobby.nombre, new object());
                    Locks.lockChatLobby.Add(datosLobby.lobby.nombre, new object());

                    //Llamamos al creador indicándole que todo ha ido bien 👌👌👌
                    Clients.Caller.crearLobby(true);
                    Thread.Sleep(500);
                    Clients.Caller.imprimirMensajeLobby(new Mensaje("[SYSTEM] Remember not to share your password with anyone 🤫", "Red"));
                    Clients.Caller.imprimirMensajeLobby(new Mensaje("[LOBBY] Lobby created successfully 👍", "#2196F3"));

                    //Avisamos a todos los que están en el chat global de que un lobby se ha creado
                    Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje($"[SYSTEM] Someone created \"{lobby.nombre}\" just now! Refresh to see it! 🔄", "#2196F3"));
                }
            }
        }

        //Método para comprobar la contraseña
        public void comprobarContrasena(string nombreLobby, string contrasena)
        {
            //Si hay menos jugadores conectados que el maxJugadores
            if (LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores < LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores)
            {
                //Si la contraseña es correcta
                if (LobbyInfo.listadoLobbies[nombreLobby].lobby.contrasena == contrasena)
                {
                    //Añadimos el jugador al grupo
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores++;
                    LobbyInfo.listadoUsuariosCreandoPersonaje.AddOrUpdate(Context.ConnectionId, nombreLobby, (key, value) => value);
                    Clients.Caller.contrasena(1);
                }
                else
                    Clients.Caller.contrasena(0);
            }
            else
                Clients.Caller.contrasena(-1);

        }

        //Método para unir al lobby
        public void unirALobby(Jugador jugador)
        {
            bool puedeContinuar = false;

            //Quitamos al jugador de la lista temporal y cogemos el nombre del lobby
            string nombreLobby;
            LobbyInfo.listadoUsuariosCreandoPersonaje.TryRemove(Context.ConnectionId, out nombreLobby);

            //Asegurar que solo uno de los clientes que está accediendo pueda meter el usuario
            lock (Locks.lockUnirSalir[nombreLobby])
            {

                if (LobbyInfo.listadoLobbies.ContainsKey(nombreLobby))
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

                    //Comprobamos que ni el nombre de ese jugador ni la ficha estén cogidas ya
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
        public void entrarEnPartida(string nombreLobby)
        {
            if (LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Count)
            {
                //Crear una partida nueva
                Partida partida = GestoraPartida.generarPartidaNueva(new List<Jugador>(LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores));
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida = partida;
                Random random = new Random();
                
                //Generar el índice del jugador que va a salir primero
                partida.turnoActual = random.Next(0, LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores);

                //Actualizar cada jugador el dinero y el listado de propiedades
                foreach(Jugador jugador2 in LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores)
                {
                    jugador2.dinero = 1500;
                    jugador2.listadoPropiedades = GestoraPartida.generarPropiedadesJugador();
                }

                //Asignar la partida al lobby
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida = partida;

                string nombreJugador;
                Jugador jugador;
                string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Key == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre).Value;
                //Avisamos a los otros jugadores para empezar la partida
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                {
                    nombreJugador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Value == connectionId).Key;
                    jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Single(x => x.nombre == nombreJugador);
                    Clients.Client(connectionId).entrarEnPartida(jugador);
                    Thread.Sleep(200); //Para evitar dobles llamadas
                    Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby, connectionId == connectionIDCreador);
                }
            }
        }
        
        //Función para salir del lobby
        public void salirDeLobby(string nombreLobby)
        {
            //Bloqueamos el acceso por si dos personas le han dado a salir a la vez
            lock(Locks.lockUnirSalir[nombreLobby])
            {
                //Si el lobby aún existe (puede que el creador se haya salido antes y lo haya borrado)
                if(LobbyInfo.listadoLobbies.ContainsKey(nombreLobby))
                {
                    //Quitamos en 1 el número de jugadores
                    LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores--;
                    List<KeyValuePair<string, string>> listadoNombres = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Where(x => x.Value == Context.ConnectionId).ToList();

                    //Si se ha salido estando dentro del lobby (no es que le ha dado atrás en la creacìón de personajes)
                    if (listadoNombres.Count != 0)
                    {
                        //Buscamos el jugador
                        string connection = Context.ConnectionId;
                        string nombreJugador = listadoNombres.First().Key;
                        Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores.Single(x => x.nombre == nombreJugador);

                        //Obtenemos la connectionID del jugador creador
                        string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Single(x => x.Key == LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre).Value;

                        //Si es el creador el que se ha desconectado
                        if (connectionIDCreador == Context.ConnectionId)
                        {
                            //Sacamos a todos del lobby
                            DatosLobby datosLobby;
                            LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out datosLobby);
                            foreach (string connectionId in datosLobby.listadoJugadoresConnection.Values)
                                Clients.Client(connectionId).salirDeLobby();

                            //Borramos los locks del lobby
                            Locks.lockChatLobby.Remove(nombreLobby);
                            Locks.lockUnirSalir.Remove(nombreLobby);
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
                    else
                        //Lo quitamos de la lista temporal
                        LobbyInfo.listadoUsuariosCreandoPersonaje.TryRemove(Context.ConnectionId, out nombreLobby);
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
                listadoLobbies.Add(datosLobby.lobby);

            Clients.Caller.actualizarListadoLobbies(listadoLobbies);
        }

        #region Chats
        //Método para hacer saber a todos que alguien ha entrado en Search mostrando un mensaje en el chat
        public void unirChatGlobal()
        {
            lock (Locks.lockChatGlobal)
            {
                Groups.Add(Context.ConnectionId, nombreChatGlobal);
                Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje("[GLOBAL] Someone joined the global chat, say 👋!", "#8BC34A"));
            }
        }

        //Método para hacer saber a todos que alguien se ha salido de Search. Si el nombreLobby es null, se ha salido al menú, si no, ha entrado a un lobby
        public void salirChatGlobal(string nombreLobby = null)
        {
            lock(Locks.lockChatGlobal)
            {
                Groups.Remove(Context.ConnectionId, nombreChatGlobal);

                if (nombreLobby == null)
                    Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje("[GLOBAL] Someone left the global chat 😭", "#8BC34A"));
                else
                    Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje($"[GLOBAL] Someone joined {nombreLobby}", "#2196F3"));
            }

        }

        public void enviarMensaje(string mensaje, bool esGlobal = false)
        {
            Random random = new Random();
            int chanceOfUwu = random.Next(1, 201);

            if(esGlobal)
            {
                lock(Locks.lockChatGlobal)
                {
                    Clients.Group(nombreChatGlobal).imprimirMensajeGlobal(new Mensaje($"[GLOBAL] {mensaje}{(chanceOfUwu == 1 ? " UwU" : "")}", "Black"));
                }
            }
            else
            {
                string connectionId = Context.ConnectionId;
                ConcurrentDictionary<string, string> conexiones = null;
                string nombreJugador = null;
                string nombreLobby = null;

                foreach (DatosLobby datos in LobbyInfo.listadoLobbies.Values)
                {
                    try
                    {
                        nombreLobby = datos.lobby.nombre;
                        nombreJugador = datos.listadoJugadoresConnection.Single(x => x.Value == connectionId).Key;
                        conexiones = datos.listadoJugadoresConnection;
                        break;
                    }
                    catch (InvalidOperationException) { }
                }

                if (nombreJugador != null)
                {
                    lock(Locks.lockChatLobby[nombreLobby])
                    {
                        foreach (string conId in conexiones.Values)
                            Clients.Client(conId).imprimirMensajeLobby(new Mensaje($"[LOBBY] {nombreJugador}: {mensaje}{(chanceOfUwu == 1 ? " UwU" : "")}", "Black"));
                    }
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
            if(LobbyInfo.listadoUsuariosCreandoPersonaje.ContainsKey(Context.ConnectionId))
            {
                string nombreLobby;
                LobbyInfo.listadoUsuariosCreandoPersonaje.TryRemove(Context.ConnectionId, out nombreLobby);
                LobbyInfo.listadoLobbies[nombreLobby].numeroJugadores--;
            }
            else
            {
                //Bloqueamos el acceso por si dos personas le han dado a salir a la vez
                lock (Locks.lockDisconnect)
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

                            //Borramos los locks del lobby
                            Locks.lockChatLobby.Remove(nombreLobby);
                            Locks.lockUnirSalir.Remove(nombreLobby);
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
                                Clients.Client(connectionId).imprimirMensajeLobby(new Mensaje($"[LOBBY] {nombreJugador} left the lobby ~10 seconds ago 🕒", "Gray"));
                            }
                        }
                    }
                }
            }
            
            return base.OnDisconnected(stopCalled);
        }
        
    }
}