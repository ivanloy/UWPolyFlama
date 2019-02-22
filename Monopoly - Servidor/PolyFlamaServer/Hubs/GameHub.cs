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

                //Avisamos a los otros jugadores de los cambios
                foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                {
                    Clients.Client(connectionId).actualizarLobby(lobby);
                    Clients.Client(connectionId).moverCasillas();
                }

                //Esperamos un poquisho
                Thread.Sleep(1000);

                //Comprobar de que tipo es la casilla actual
                Object casilla = lobby.partida.listadoCasillas[posicionActual];
                if (casilla is Propiedad)
                {
                    Propiedad propiedad = (Propiedad)casilla;
                    if (!propiedad.estaComprado)
                        Clients.Caller.comprarPropiedad();
                    else
                    {
                        lobby.listadoJugadores[turnoActual].dinero -= propiedad.dineroAPagar;
                    }
                }
                else
                {
                    Casilla cas = (Casilla)casilla;
                    Random rnd = new Random();
                    Carta carta;
                    int cartaRandom;

                    switch (cas.tipo)
                    {
                        case TipoCasilla.SUERTE:
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
                            break;

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

            lobby.partida.turnoActual = GestoraPartida.calcularNuevoTurno(turnoActual, lobby.maxJugadores);

            //Avisamos a los otros jugadores de los cambios
            foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
            {
                Clients.Client(connectionId).actualizarLobby(lobby);
                if(connectionId != connectionIDCreador)
                    Clients.Client(connectionId).siguienteTurno();
            }
        }

        public void comprarPropiedad(string nombreLobby, bool quiereComprar)
        {
            //Buscamos la información de 💩💩💩
            Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual];
            Propiedad propiedad = (Propiedad)LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[jugador.posicion];

            //Cambiamos la información en el jugador
            jugador.listadoPropiedades[jugador.posicion].estaComprado = true;
            jugador.listadoPropiedades[jugador.posicion].comprador = jugador;

            //Cambiamos la información en la propiedad de la partida
            propiedad.comprador = jugador;
            propiedad.estaComprado = true;

            //Llamamos a todo el grupo y les actualizamos el lobby
            string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre];

            //Avisamos a los otros jugadores de los cambios
            foreach (string connectionId in LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.Values)
                Clients.Client(connectionId).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);

        }

        public void connected(string nombreLobby, string nombreJugador)
        {
            lock(lockConnection)
            {
                //Actualizamos el connectionID del jugador en la lista de conexiones
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.AddOrUpdate(nombreJugador, Context.ConnectionId, (key, value) => value);

                //Actualizamos el número de personas que ya se han conectado y actualizado su listado de conexiones
                if (GameInfo.conexionesEstablecidas.ContainsKey(nombreLobby))
                    GameInfo.conexionesEstablecidas[nombreLobby]++;
                else
                    GameInfo.conexionesEstablecidas.AddOrUpdate(nombreLobby, 1, (key, value) => value);
                
            }

        }

        //Cuando un jugador se conecte, actualizamos su connectionID de la lista
        public override Task OnConnected()
        {
            Clients.Caller.connected();

            return base.OnConnected();
        }
    }
}