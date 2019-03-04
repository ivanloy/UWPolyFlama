using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;
using PolyFlamaServer.Models.Enums;
using System.Threading;

namespace PolyFlamaServer.Hubs
{
    public class GameHub : Hub
    {
        private object lockConnection = new object();

        public void tirarDados(string nombreLobby)
        {
            Lobby lobby = LobbyInfo.listadoLobbies[nombreLobby].lobby;
            int turnoActual = lobby.partida.turnoActual;
            bool calcularNuevoTurno = false;

            //Lockeaso guapo 🔐🔐🔐
            lock (Locks.lockTirarDados[nombreLobby])
            {
                //Comprobamos por si acaso le ha dado varias veces al botón de tirar dados
                if(turnoActual == LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual)
                {
                    int turnoNuevo;
                    Jugador jugador = lobby.listadoJugadores[turnoActual];
                    string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugador.nombre];
                    Random random = new Random();
                    
                    //Tirar los dados
                    int dado1 = random.Next(1, 7);
                    int dado2 = random.Next(1, 7);

                    //Avisamos a los otros jugadores de los cambios
                    foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        Clients.Client(connectionId).actualizarLobby(lobby);

                    //Si el jugador está en la cárcel
                    if (jugador.estaEnCarcel && jugador.turnosEnCarcel < 2)
                    {
                        //Si ha sacado dobles
                        if(dado1 == dado2)
                        {
                            //Lo sacamos de la carcel
                            jugador.estaEnCarcel = false;
                            jugador.turnosEnCarcel = 0;

                            Clients.Caller.mostrarMensaje($"You pulled out double {dado1}s! You're out of jail by the power vested in these dice");
                        }
                    }

                    //Si el jugador no está en la carcel o si está en la carcel pero ya ha hecho 2 tiradas
                    if (!jugador.estaEnCarcel || (jugador.estaEnCarcel && jugador.turnosEnCarcel == 2))
                    {
                        int posicionActual = lobby.listadoJugadores[turnoActual].posicion;
                        int posicionAnterior = posicionActual;

                        //Le quitamos el estado de estar en la carcel, independiente de si está o no, why not? 🤷🤷🤷🤷🤷🤷🤷🤷
                        jugador.estaEnCarcel = false;
                        jugador.turnosEnCarcel = 0;

                        //Actualizamos el lobby con los dados
                        lobby.partida.arrayDados = new int[] { dado1, dado2 };
                        lobby.listadoJugadores[turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
                        posicionActual = lobby.listadoJugadores[turnoActual].posicion;

                        //Actualizamos las fichas del jugador
                        lobby.partida.listadoCasillas[posicionAnterior].listadoJugadores.Remove(jugador);
                        lobby.partida.listadoCasillas[posicionActual].listadoJugadores.Add(jugador);

                        //Avisamos a los otros jugadores de los cambios
                        foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                            Clients.Client(connectionId).actualizarLobby(lobby);

                        //Comprobar de que tipo es la casilla actual
                        Casilla casilla = lobby.partida.listadoCasillas[posicionActual];
                        if (casilla is Propiedad)
                        {
                            Propiedad propiedad = (Propiedad)casilla;
                            //Si la propiedad no está comprada y el jugador tiene dinero para comprarla
                            if (!propiedad.estaComprado && jugador.dinero > propiedad.precio)
                                Clients.Caller.comprarPropiedad(propiedad);
                            else if (propiedad.estaComprado && propiedad.comprador.nombre != jugador.nombre) //Comprobar que si la propiedad esté comprada, que no sea del mismo jugador que ha caído
                            {
                                //Empezamos cogiendo el dinero del dinero a pagar de la propiedad
                                int dineroAPagar = propiedad.dineroAPagar;

                                //Cogemos el jugador
                                Jugador jugadorDueno = lobby.listadoJugadores.Single(x => x.nombre == propiedad.comprador.nombre);

                                //Si la propiedad en la que ha caído es una estación o un servicio, hay que comprobar cuántas estaciones o servicios tiene compradas el dueño
                                if (propiedad.color == ColorPropiedad.ESTACION)
                                {
                                    int nEstacionesCompradas = 0;

                                    //Cogemos todas las estaciones
                                    bool estacion1 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 5).estaComprado;
                                    bool estacion2 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 15).estaComprado;
                                    bool estacion3 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 25).estaComprado;
                                    bool estacion4 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 35).estaComprado;

                                    //Contar cuántas estaciones tiene compradas
                                    if (estacion1)
                                        nEstacionesCompradas++;
                                    if (estacion2)
                                        nEstacionesCompradas++;
                                    if (estacion3)
                                        nEstacionesCompradas++;
                                    if (estacion4)
                                        nEstacionesCompradas++;

                                    //Si tiene 1 estación, paga 25; 2, 50; 3, 100; 4, 200.
                                    dineroAPagar = 25 * (int)Math.Pow(2, nEstacionesCompradas);

                                    //Mandarle mensajito al jugador
                                    Clients.Caller.mostrarMensaje($"You landed on {propiedad.nombre} and {jugador.nombre} owns this station. He owns {nEstacionesCompradas} station(s), therefore, you paid him {dineroAPagar}$");
                                }
                                else if (propiedad.color == ColorPropiedad.SERVICIO)
                                {
                                    //Cogemos todas sus estaciones
                                    bool servicio1 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 12).estaComprado;
                                    bool servicio2 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 28).estaComprado;
                                    int nServicios = 0;
                                    if (servicio1)
                                        nServicios++;
                                    if (servicio2)
                                        nServicios++;

                                    //Si tiene los dos servicios comprados, paga 10 veces la tirada; si solo tiene un servicio, 4 veces la tirada
                                    dineroAPagar = (dado1 + dado2) * (nServicios == 1 ? 4 : 10);

                                    //Mandarle mensajito al jugador
                                    Clients.Caller.mostrarMensaje($"You landed on {propiedad.nombre} and {jugadorDueno.nombre} owns this service. He {(nServicios == 1 ? "only owns this service" : "owns both services")}, therefore, you paid him {(nServicios == 1 ? "4" : "10")} times the roll: {dineroAPagar}$");
                                }
                                else
                                {
                                    //Mandarle mensajito al jugador
                                    Clients.Caller.mostrarMensaje($"You landed on {propiedad.nombre} and {jugadorDueno.nombre} owns this property. You paid him {dineroAPagar}$");
                                }

                                //Le quitamos el dinero al jugador
                                lobby.listadoJugadores[turnoActual].dinero -= dineroAPagar;
                                //Le damos el dinero al jugador dueño
                                jugadorDueno.dinero += dineroAPagar;
                                
                                //Avisamos de que se debe calcular nuevo turno
                                calcularNuevoTurno = true;

                            }
                            else //Se pasa el turno
                            {
                                //Avisamos de que se debe calcular nuevo turno
                                calcularNuevoTurno = true;
                            }

                        }
                        else
                        {
                            //Miramos en qué tipo de casilla ha caído
                            Random rnd = new Random();
                            Carta carta = null;
                            int cartaRandom;

                            switch (casilla.tipo)
                            {
                                case TipoCasilla.SUERTE:
                                    cartaRandom = rnd.Next(0, lobby.partida.listadoCartasSuerte.Count);
                                    carta = lobby.partida.listadoCartasSuerte[cartaRandom];
                                    Clients.Caller.mostrarMensaje(carta.texto);
                                    break;

                                case TipoCasilla.COMUNIDAD:
                                    cartaRandom = rnd.Next(0, lobby.partida.listadoCartasSuerte.Count);
                                    carta = lobby.partida.listadoCartasComunidad[cartaRandom];
                                    Clients.Caller.mostrarMensaje(carta.texto);
                                    break;

                                case TipoCasilla.IRALACARCEL:
                                    lobby.listadoJugadores[turnoActual].posicion = 10;
                                    lobby.listadoJugadores[turnoActual].estaEnCarcel = true;
                                    lobby.partida.listadoCasillas[10].listadoJugadores.Add(jugador);
                                    lobby.partida.listadoCasillas[posicionActual].listadoJugadores.Remove(jugador);
                                    break;

                                case TipoCasilla.IMPUESTOAPPLE:
                                    lobby.listadoJugadores[turnoActual].dinero -= 200;
                                    break;

                                case TipoCasilla.IMPUESTOAZURE:
                                    lobby.listadoJugadores[turnoActual].dinero -= 100;
                                    break;
                            }

                            if(casilla.tipo == TipoCasilla.SUERTE || casilla.tipo == TipoCasilla.COMUNIDAD)
                            {
                                string[] efectoString = carta.efecto.ToString().Split('_');
                                string efecto = efectoString[0];
                                int valor = int.Parse(efectoString[1]);

                                switch(efecto)
                                {
                                    case "DAR":
                                        jugador.dinero += valor;
                                        break;

                                    case "QUITAR":
                                        jugador.dinero -= valor;
                                        break;

                                    case "MOVER":
                                        lobby.listadoJugadores[turnoActual].posicion = 10;
                                        lobby.listadoJugadores[turnoActual].estaEnCarcel = true;
                                        lobby.partida.listadoCasillas[10].listadoJugadores.Add(jugador);
                                        lobby.partida.listadoCasillas[posicionActual].listadoJugadores.Remove(jugador);
                                        break;

                                    case "GLOBAL":
                                        foreach(Jugador jugadorGlobal in lobby.listadoJugadores)
                                        {
                                            if(jugadorGlobal.dinero > 0)
                                            {
                                                jugadorGlobal.dinero += valor;
                                                jugador.dinero -= valor;
                                            }
                                        }
                                        break;
                                }
                            }

                            //Avisamos de que se debe calcular nuevo turno
                            calcularNuevoTurno = true;

                        }

                        //Le damos 200$ si ha pasado por la casilla de SALIDA
                        if (posicionActual < posicionAnterior)
                            jugador.dinero += 200;

                        if (jugador.dinero <= 0)
                            Clients.Caller.partidaPerdida();

                    }
                    else
                    {
                        lobby.listadoJugadores[turnoActual].turnosEnCarcel++;

                        //Avisamos de que se debe calcular nuevo turno
                        calcularNuevoTurno = true;
                    }

                    //Avisamos a los otros jugadores de los cambios
                    foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        Clients.Client(connectionId).actualizarLobby(lobby);

                    //Comprobamos si hay que calcular un nuevo turno
                    if (calcularNuevoTurno)
                    {
                        //Se calcula el nuevo turno
                        turnoNuevo = GestoraPartida.calcularNuevoTurno(lobby);

                        //Comprobamos si, al generar un nuevo turno, el turno que ha salido es el mismo que había antes
                        //En ese caso, significa que solo queda 1 persona con dinero > 0, el ganador
                        //TODO Quitarlo pal release
                        if (turnoNuevo == turnoActual)
                        {
                            foreach (Jugador jugadorGanador in LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores)
                            {
                                if (jugadorGanador.dinero > 0)
                                {
                                    Clients.Client(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorGanador.nombre]).partidaGanada();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Avisamos al jugador de que es su turno nuevo
                            Jugador jugadorNuevo = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoNuevo];
                            string connectionIDNuevo = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorNuevo.nombre];
                            lobby.partida.turnoActual = turnoNuevo;

                            //Avisamos al jugador
                            Clients.Client(connectionIDNuevo).esTuTurno();
                        }
                    }

                }
            }
        }

        public void comprarPropiedad(string nombreLobby, bool quiereComprar)
        {
            if(quiereComprar)
            {
                //Buscamos la información de 💩💩💩
                Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual];
                Propiedad propiedad = (Propiedad)LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[jugador.posicion];
                Propiedad propiedadJugador = jugador.listadoPropiedades.Single(x => x.posicionEnTablero == jugador.posicion);

                //Le quitamos el dinero al jugador, taría bien la verdad
                jugador.dinero -= propiedad.precio;

                //Cambiamos la información en el jugador
                propiedadJugador.estaComprado = true;

                //Cambiamos la información en la propiedad de la partida
                propiedad.estaComprado = true;
                propiedad.comprador = jugador;

                //Llamamos a todo el grupo y les actualizamos el lobby
                string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre];

                //Avisamos a los otros jugadores de los cambios
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                    Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
            }

            Lobby lobby = LobbyInfo.listadoLobbies[nombreLobby].lobby;

            //Se calcula el nuevo turno
            int turnoActual = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual;
            int turnoNuevo = GestoraPartida.calcularNuevoTurno(lobby);

            //Comprobamos si, al generar un nuevo turno, el turno que ha salido es el mismo que había antes
            //En ese caso, significa que solo queda 1 persona con dinero > 0, el ganador
            if (turnoNuevo == turnoActual)
            {
                foreach (Jugador jugadorGanador in LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores)
                {
                    if (jugadorGanador.dinero > 0)
                    {
                        Clients.Client(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorGanador.nombre]).partidaGanada();
                        break;
                    }
                }
            }
            else
            {
                //Avisamos al jugador de que es su turno nuevo
                Jugador jugadorNuevo = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoNuevo];
                string connectionIDNuevo = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorNuevo.nombre];
                lobby.partida.turnoActual = turnoNuevo;

                //Avisamos al jugador
                Clients.Client(connectionIDNuevo).esTuTurno();
            }

            //Avisamos a los otros jugadores de los cambios
            foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                Clients.Client(connectionId).actualizarLobby(lobby);

        }

        public void conectar(string nombreLobby, string nombreJugador)
        {
            lock(lockConnection)
            {
                //Actualizamos el connectionID del jugador en la lista de conexiones
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[nombreJugador] = Context.ConnectionId;

                //Actualizamos el número de personas que ya se han conectado y actualizado su listado de conexiones
                if (GameInfo.conexionesEstablecidas.ContainsKey(nombreLobby))
                    GameInfo.conexionesEstablecidas[nombreLobby]++;
                else
                    GameInfo.conexionesEstablecidas.AddOrUpdate(nombreLobby, 1, (key, value) => value);
                
                if(GameInfo.conexionesEstablecidas[nombreLobby] == LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores)
                {
                    //Creamos el lock
                    Locks.lockTirarDados.Add(nombreLobby, new object());

                    //La partida ha empezado
                    LobbyInfo.listadoLobbies[nombreLobby].lobby.partidaEmpezada = true;

                    //Cogemos el jugador que vaya a ser el primero en tirar
                    int turnoActual = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual;
                    Jugador jugadorNuevo = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual];
                    string connectionIDNuevo = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorNuevo.nombre];

                    //Avisamos a todos de que todos se han conectado correctamente, y al que tire primero, le avisamos de que es su turno
                    foreach (string connectionID in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                    {
                        //Si la connectionID es la del jugador que va a tirar primero, le avisamos de que es su turno
                        if(connectionID == connectionIDNuevo)
                            Clients.Client(connectionIDNuevo).esTuTurno();
                        else //Si no, avisamos a los demás de que todos se han conectado ya y la partida va a empezar
                            Clients.Client(connectionID).todosConectados();

                        Clients.Client(connectionID).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
                    }

                }
            }

        }

        //Cuando un jugador se conecte, actualizamos su connectionID de la lista
        public override Task OnConnected()
        {
            Clients.Caller.conectar();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Bloqueamos el acceso por si dos personas le han dado a salir a la vez
            lock (Locks.lockDisconnectGame)
            {
                #region Obtener lobby

                string myConnectionId = Context.ConnectionId;
                string nombreLobby = null;
                string nombreJugador = null;

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

                #endregion

                if (nombreJugador != null && nombreLobby != null)
                {
                    //Borramos el lobby
                    DatosLobby datosLobby = null;
                    LobbyInfo.listadoLobbies.TryRemove(nombreLobby, out datosLobby);
                    
                    foreach (string connectionId in datosLobby.listadoJugadoresConnection.Values)
                        Clients.Client(connectionId).salirDePartida();

                    //Borramos los locks del lobby
                    Locks.lockChatLobby.Remove(nombreLobby);
                    Locks.lockUnirSalir.Remove(nombreLobby);
                    Locks.lockTirarDados.Remove(nombreLobby);
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}