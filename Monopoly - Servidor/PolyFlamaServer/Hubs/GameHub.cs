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
            int turnoActual = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual;
            string connectionIDCreator = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection.First().Value;

            //Si el jugador no está en la carcel o si está en la carcel pero ya ha hecho 2 tiradas
            if (!LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].estaEnCarcel || LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].estaEnCarcel && LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].turnosEnCarcel == 2)
            {
                Random random = new Random();
                int posicionActual = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].posicion;

                //Le quitamos el estado de estar en la carcel, independiente de si está o no, why not? 🤷🤷🤷🤷🤷🤷🤷🤷
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].estaEnCarcel = false;
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].turnosEnCarcel = 0;

                //Tirar los dados
                int dado1 = random.Next(1, 7);
                int dado2 = random.Next(1, 7);

                //Actualizamos el lobby con los dados y lo pasamos a todos
                LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.arrayDados = new int[] { dado1, dado2 };
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].posicion = GestoraPartida.calcularNuevaPosicion(posicionActual, dado1 + dado2);
                Clients.Group(connectionIDCreator).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby]);
                Clients.Group(connectionIDCreator).moverCasillas();
                Object casilla = LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[posicionActual];

                //Esperamos un poquisho
                Thread.Sleep(1000);

                //Comprobar de que tipo es la casilla actual
                if (casilla is Propiedad)
                {
                    Propiedad propiedad = (Propiedad)casilla;
                    if (!propiedad.estaComprado)
                        Clients.Caller.comprarPropiedad();
                    else
                    {
                        LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].dinero -= propiedad.dineroAPagar;
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
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].posicion = 9;
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].estaEnCarcel = true;
                            break;

                        case TipoCasilla.IMPUESTOAPPLE:
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].dinero -= 200;
                            break;

                        case TipoCasilla.IMPUESTOAZURE:
                            LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].dinero -= 100;
                            break;
                    }
                }
            }
            else
                LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[turnoActual].turnosEnCarcel++;

            LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual = GestoraPartida.calcularNuevoTurno(turnoActual, LobbyInfo.listadoLobbies[nombreLobby].lobby.maxJugadores);
            Clients.Group(connectionIDCreator).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
            Clients.Caller.terminarTurno();
            Clients.OthersInGroup(connectionIDCreator).siguienteTurno();
        }

        public void comprarPropiedad(string nombreLobby, bool quiereComprar)
        {
            //Buscamos la información de 💩💩💩
            Jugador jugador = LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.turnoActual];
            Propiedad propiedad = (Propiedad)LobbyInfo.listadoLobbies[nombreLobby].lobby.partida.listadoCasillas[jugador.posicion];

            //Cambiamos la información en el jugador
            jugador.listadoPropiedades.Single(x => x.posicionCasilla == jugador.posicion).estaComprado = true;
            jugador.listadoPropiedades.Single(x => x.posicionCasilla == jugador.posicion).comprador = jugador;

            //Cambiamos la información en la propiedad de la partida
            propiedad.comprador = jugador;
            propiedad.estaComprado = true;

            //Llamamos a todo el grupo y les actualizamos el lobby
            string connectionIDCreador = LobbyInfo.listadoLobbies[nombreLobby].listadoJugadoresConnection[LobbyInfo.listadoLobbies[nombreLobby].lobby.listadoJugadores[0].nombre];
            Clients.Group(connectionIDCreador).actualizarLobby(LobbyInfo.listadoLobbies[nombreLobby].lobby);
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