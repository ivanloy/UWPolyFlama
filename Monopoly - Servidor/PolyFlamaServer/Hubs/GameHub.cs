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
                    if (!propiedad.estaComprado)
                        Clients.Caller.comprarPropiedad();
                    else
                        lobby.listadoJugadores[turnoActual].dinero -= propiedad.dineroAPagar;

                }
                else
                {
                    //Miramos en qué tipo de casilla ha caído
                    Random rnd = new Random();
                    Carta carta;
                    int cartaRandom;

                    Enum.TryParse(casilla.tipo, out TipoCasilla tipo);

                    switch (tipo)
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
            }
            else
                lobby.listadoJugadores[turnoActual].turnosEnCarcel++;

            //Se calcula el nuevo turno
            lobby.partida.turnoActual = GestoraPartida.calcularNuevoTurno(turnoActual, lobby.maxJugadores);

            //Avisamos a los otros jugadores de los cambios
            foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
            {
                Clients.Client(connectionId).actualizarLobby(lobby);
                if(connectionId != connectionIDCreador)
                    Clients.Client(connectionId).siguienteTurno();
            }
        }

        public void comprarPropiedad(string nombreLobby)
        {
            //Buscamos la información de 💩💩💩
            Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual];
            Propiedad propiedad = (Propiedad)LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[jugador.posicion];
            Propiedad propiedadJugador = jugador.listadoPropiedades.Single(x => x.posicionEnTablero == jugador.posicion);
            
            //Cambiamos la información en el jugador
            propiedadJugador.estaComprado = true;
            propiedadJugador.comprador = jugador;

            //Cambiamos la información en la propiedad de la partida
            propiedad.comprador = jugador;
            propiedad.estaComprado = true;

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
                    //La partida ha empezado
                    LobbyInfo.listadoLobbies[nombreLobby].lobby.partidaEmpezada = true;

                    //Avisamos a todos de que todos se han conectado correctamente
                    foreach (string connectionID in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                        Clients.Client(connectionID).todosConectados();
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