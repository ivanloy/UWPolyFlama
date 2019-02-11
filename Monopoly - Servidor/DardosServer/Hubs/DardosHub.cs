using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;
using PolyFlamaServer.Models;

namespace PolyFlamaServer.Hubs
{
    public class DardosHub : Hub
    {
        public void press(int posBalloon)
        {
            //En caso de que alguien haya pulsado antes, nos aseguramos de que el globo sigue sin ser explotado
            if(!GameInfo.casillas[posBalloon].isPopped)
            {
                string balloonColor = GameInfo.jugadores[Context.ConnectionId].color;

                //Explotamos el globo en los otros clientes
                GameInfo.casillas[posBalloon].popBalloon(balloonColor);
                Clients.All.popBalloon(posBalloon, balloonColor);
                GameInfo.poppedBalloons++;

                //Le damos el punto por explotar el globo al jugador que lo haya explotado
                GameInfo.jugadores[Context.ConnectionId].puntuacion++;
                Clients.Caller.updatePersonalScore(GameInfo.jugadores[Context.ConnectionId].puntuacion);

                //Actualizamos la puntuación global de todos los jugadores
                GameInfo.globalScore++;
                Clients.All.updateGlobalScore(GameInfo.globalScore);

                //Actualizamos el ranking de todos los jugadores ordenado por puntuación
                Clients.All.updateRanking(GameInfo.jugadores.Values.ToList().OrderByDescending(x => x.puntuacion));

                //Si ya no quedan globos por explotar
                if (GameInfo.poppedBalloons == GameInfo.numberOfBalloons)
                {
                    GameInfo.poppedBalloons = 0;
                    GestoraCasilla.generarCasillas();
                    Clients.All.loadBalloons(GameInfo.casillas);
                }
            }
        }

        //Cuando un jugador se desconecte
        public override Task OnDisconnected(bool stopCalled)
        {
            //Cuando se desconecte, quitamos su entrada del diccionario y actualizamos el ranking de todos
            GameInfo.jugadores.Remove(Context.ConnectionId);
            Clients.All.updateRanking(GameInfo.jugadores.Values.ToList().OrderByDescending(x => x.puntuacion));

            //Restamos uno al número de jugadores concurrentes
            GameInfo.numberOfPlayers--;
            Clients.All.updateNumberOfPlayers(GameInfo.numberOfPlayers);

            return base.OnDisconnected(stopCalled);
        }

        //Cuando un jugador se conecte
        public override Task OnConnected()
        {
            //Cuando se conecte, agregamos su entrada al diccionario
            GameInfo.jugadores.Add(Context.ConnectionId, new Jugador(Context.QueryString["username"], 0, Context.QueryString["color"]));

            //Le mandamos la información de la partida
            Clients.Caller.loadBalloons(GameInfo.casillas);
            Clients.Caller.updateGlobalScore(GameInfo.globalScore);
            Clients.Caller.updateRanking(GameInfo.jugadores.Values.ToList().OrderByDescending(x => x.puntuacion));

            //Y llamamos al método que indica que todo ha cargado
            Clients.Caller.onConnectedIsDone();

            return base.OnConnected();
        }
    }
}