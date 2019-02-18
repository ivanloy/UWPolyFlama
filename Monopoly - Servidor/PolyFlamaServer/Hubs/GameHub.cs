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
            if (!LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].estaEnCarcel || LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].estaEnCarcel && LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].turnosEnCarcel == 2)
            {
                int posicionActual = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].posicion;
                Random random = new Random();

                //Le quitamos el estado de estar en la carcel, independiente de si está o no, why not? 🤷🤷🤷🤷
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].estaEnCarcel = false;
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].turnosEnCarcel = 0;

                //Tirar los dados
                int dado1 = random.Next(1, 7);
                int dado2 = random.Next(1, 7);

                //Actualizamos el lobby con los dados y lo pasamos a todos
                LobbyInfo.listadoLobbies[nombreLobby].partida.arrayDados = new int[] { dado1, dado2 };
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
                Clients.Group(nombreLobby).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
                Clients.Group(nombreLobby).moverCasillas();
                Object casilla = LobbyInfo.listadoLobbies[nombreLobby].partida.listadoCasillas[posicionActual];

                Thread.Sleep(1000);

                //Comprobar de que tipo es la casilla actual
                if (casilla is Propiedad)
                {
                    Propiedad propiedad = (Propiedad)casilla;
                    if (!propiedad.estaComprado)
                        Clients.Caller.comprarPropiedad();
                    else
                    {
                        LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].dinero -= propiedad.dineroAPagar;
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
                            cartaRandom = rnd.Next(0, LobbyInfo.listadoLobbies[nombreLobby].partida.listadoCartasSuerte.Count);
                            carta = LobbyInfo.listadoLobbies[nombreLobby].partida.listadoCartasSuerte[cartaRandom];
                            break;

                        case TipoCasilla.COMUNIDAD:
                            cartaRandom = rnd.Next(0, LobbyInfo.listadoLobbies[nombreLobby].partida.listadoCartasSuerte.Count);
                            carta = LobbyInfo.listadoLobbies[nombreLobby].partida.listadoCartasComunidad[cartaRandom];
                            break;

                        case TipoCasilla.IRALACARCEL:
                            LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].posicion = 9;
                            LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].estaEnCarcel = true;
                            break;

                        case TipoCasilla.IMPUESTOAPPLE:
                            LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].dinero -= 200;
                            break;

                        case TipoCasilla.IMPUESTOAZURE:
                            LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].dinero -= 100;
                            break;
                    }
                }
            }
            else
                LobbyInfo.listadoLobbies[nombreLobby].listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual].turnosEnCarcel++;

            LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual = GestoraPartida.calcularNuevoTurno(LobbyInfo.listadoLobbies[nombreLobby].partida.turnoActual, LobbyInfo.listadoLobbies[nombreLobby].maxJugadores);
            Clients.Group(nombreLobby).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
            Clients.Caller.terminarTurno();
            Clients.OthersInGroup(nombreLobby).siguienteTurno();
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