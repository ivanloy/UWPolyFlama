using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;
using System.Collections.Concurrent;
using PolyFlamaServer.Models.Enums;
using System.Threading;

namespace PolyFlamaServer.Hubs
{
    public class GameHub : Hub
    {
        public void tirarDados(string nombreLobby)
        {
            //Si el jugador no está en la carcel o si está en la carcel pero ya ha hecho 2 tiradas
            if (!LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].estaEnCarcel || LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].estaEnCarcel && LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].turnosEnCarcel == 2)
            {
                int posicionActual = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].posicion;
                Random random = new Random();

                //Le quitamos el estado de estar en la carcel, independiente de si está o no, why not? 🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷🤷
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].estaEnCarcel = false;
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].turnosEnCarcel = 0;

                //Tirar los dados
                int dado1 = random.Next(1, 7);
                int dado2 = random.Next(1, 7);

                //Actualizamos el lobby con los dados y lo pasamos a todos
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.arrayDados = new int[] { dado1, dado2 };
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
                Clients.Group(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
                Clients.Group(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).moverCasillas();
                Object casilla = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[posicionActual];

                Thread.Sleep(1000);

                //Comprobar de que tipo es la casilla actual
                if (casilla is Propiedad)
                {
                    Propiedad propiedad = (Propiedad)casilla;
                    if (!propiedad.estaComprado)
                        Clients.Caller.comprarPropiedad();
                    else
                    {
                        LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].dinero -= propiedad.dineroAPagar;
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
                            cartaRandom = rnd.Next(0, LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCartasSuerte.Count);
                            carta = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCartasSuerte[cartaRandom];
                            break;

                        case TipoCasilla.COMUNIDAD:
                            cartaRandom = rnd.Next(0, LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCartasSuerte.Count);
                            carta = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCartasComunidad[cartaRandom];
                            break;

                        case TipoCasilla.IRALACARCEL:
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].posicion = 9;
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].estaEnCarcel = true;
                            break;

                        case TipoCasilla.IMPUESTOAPPLE:
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].dinero -= 200;
                            break;

                        case TipoCasilla.IMPUESTOAZURE:
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].dinero -= 100;
                            break;
                    }
                }
            }
            else
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual].turnosEnCarcel++;

            LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual = GestoraPartida.calcularNuevoTurno(LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual, LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores);
            Clients.Group(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
            Clients.Caller.terminarTurno();
            Clients.OthersInGroup(LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value).siguienteTurno();
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