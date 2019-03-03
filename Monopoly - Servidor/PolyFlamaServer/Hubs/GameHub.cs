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

            //Lockeaso guapo 🔐🔐🔐
            lock (Locks.lockTirarDados[nombreLobby])
            {
                //Comprobamos por si acaso le ha dado varias veces al botón de tirar dados
                if(turnoActual == LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual)
                {
                    int turnoNuevo;
                    Jugador jugador = lobby.listadoJugadores[turnoActual];
                    string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugador.nombre];

                    //Si el jugador no está en la carcel o si está en la carcel pero ya ha hecho 2 tiradas
                    if (!jugador.estaEnCarcel || (jugador.estaEnCarcel && jugador.turnosEnCarcel == 2))
                    {
                        Random random = new Random();
                        int posicionActual = lobby.listadoJugadores[turnoActual].posicion;
                        int posicionAnterior = posicionActual;

                        //Le quitamos el estado de estar en la carcel, independiente de si está o no, why not? 🤷🤷🤷🤷🤷🤷🤷🤷
                        jugador.estaEnCarcel = false;
                        jugador.turnosEnCarcel = 0;

                        //Tirar los dados
                        int dado1 = random.Next(1, 7);
                        int dado2 = random.Next(1, 7);

                        //Actualizamos el lobby con los dados
                        lobby.partida.arrayDados = new int[] { dado1, dado2 };
                        lobby.listadoJugadores[turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
                        posicionActual = lobby.listadoJugadores[turnoActual].posicion;

                        //Actualizamos las fichas del jugador
                        lobby.partida.listadoCasillas[posicionAnterior].listadoJugadores.Remove(jugador);
                        lobby.partida.listadoCasillas[posicionActual].listadoJugadores.Add(jugador);

                        //Avisamos a los otros jugadores de los cambios
                        foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        {
                            Clients.Client(connectionId).actualizarLobby(lobby);
                            Clients.Client(connectionId).moverCasillas();
                        }

                        //Comprobar de que tipo es la casilla actual
                        Casilla casilla = lobby.partida.listadoCasillas[posicionActual];
                        if (casilla is Propiedad)
                        {
                            Propiedad propiedad = (Propiedad)casilla;
                            //Si la propiedad no está comprada y el jugador tiene dinero para comprarla
                            if (!propiedad.estaComprado && jugador.dinero >= propiedad.precio)
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
                                }
                                else if (propiedad.color == ColorPropiedad.SERVICIO)
                                {
                                    //Cogemos todas sus estaciones
                                    bool servicio1 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 12).estaComprado;
                                    bool servicio2 = jugadorDueno.listadoPropiedades.Single(x => x.posicionEnTablero == 28).estaComprado;

                                    //Si tiene los dos servicios comprados, paga 10 veces la tirada; si solo tiene un servicio, 4 veces la tirada
                                    dineroAPagar = (dado1 + dado2) * (servicio1 && servicio2 ? 10 : 4);
                                }

                                //Le quitamos el dinero al jugador
                                lobby.listadoJugadores[turnoActual].dinero -= dineroAPagar;
                                //Le damos el dinero al jugador dueño
                                jugadorDueno.dinero += dineroAPagar;
                            }
                        }
                        else
                        {
                            //Miramos en qué tipo de casilla ha caído
                            Random rnd = new Random();
                            Carta carta;
                            int cartaRandom;

                            switch (casilla.tipo)
                            {
                                /*case TipoCasilla.SUERTE:
                                    cartaRandom = rnd.Next(0, lobby.partida.listadoCartasSuerte.Count);
                                    carta = lobby.partida.listadoCartasSuerte[cartaRandom];
                                    break;

                                case TipoCasilla.COMUNIDAD:
                                    cartaRandom = rnd.Next(0, lobby.partida.listadoCartasSuerte.Count);
                                    carta = lobby.partida.listadoCartasComunidad[cartaRandom];
                                    break;

                                case TipoCasilla.IRALACARCEL:
                                    lobby.listadoJugadores[turnoActual].posicion = 9;
                                    lobby.listadoJugadores[turnoActual].estaEnCarcel = true;
                                    break;*/

                                case TipoCasilla.IMPUESTOAPPLE:
                                    lobby.listadoJugadores[turnoActual].dinero -= 200;
                                    break;

                                case TipoCasilla.IMPUESTOAZURE:
                                    lobby.listadoJugadores[turnoActual].dinero -= 100;
                                    break;
                            }
                        }

                        //Le damos 200$ si ha pasado por la casilla de SALIDA
                        if (posicionActual < posicionAnterior)
                            jugador.dinero += 200;

                        if (jugador.dinero <= 0)
                        {
                            Clients.Caller.partidaPerdida();
                        }
                        else
                        {
                            //TODO El jugador no ha perdido
                        }

                    }
                    else
                        lobby.listadoJugadores[turnoActual].turnosEnCarcel++;
                    
                    //Se calcula el nuevo turno
                    turnoNuevo = GestoraPartida.calcularNuevoTurno(lobby);

                    //Comprobamos si, al generar un nuevo turno, el turno que ha salido es el mismo que había antes
                    //En ese caso, significa que solo queda 1 persona con dinero > 0, el ganador
                    //TODO Quitarlo pal release
                    /*if (turnoNuevo == turnoActual)
                    {
                        //TODO Se ha decidido un ganador
                    }
                    else
                    {*/
                        //Avisamos al jugador de que es su turno nuevo
                        Jugador jugadorNuevo = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoNuevo];
                        string connectionIDNuevo = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[jugadorNuevo.nombre];
                        Clients.Client(connectionIDNuevo).esTuTurno();
                    //}

                    //Avisamos a los otros jugadores de los cambios
                    foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                    {
                        Clients.Client(connectionId).actualizarLobby(lobby);
                        if (connectionId != connectionIDCreador)
                            Clients.Client(connectionId).siguienteTurno();
                    }
                }
            }
        }

        public void comprarPropiedad(string nombreLobby)
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
    }
}