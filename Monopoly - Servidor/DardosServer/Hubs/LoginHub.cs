using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PolyFlamaServer.Models;
using PolyFlamaServer.Gestora;
using System.Threading.Tasks;

namespace PolyFlamaServer.Hubs
{
    public class LoginHub : Hub
    {
        public void checkUsernameAvailability(string username)
        {
            bool isAvailable = GestoraJugadores.checkUsernameAvailability(username);

            Clients.Caller.checkUsernameAvailability(isAvailable);
        }

        public void reserveColor(string color)
        {
            //Reservamos el color
            GameInfo.colors.Single(s => s.color == color).isAvailable = false;

            //Y actualizamos la lista de colores disponibles para todo el mundo
            Clients.Others.updateColors(GameInfo.colors);
        }

        //Cuando un jugador se desconecte
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        //Cuando un jugador se conecte
        public override Task OnConnected()
        {
            Clients.Caller.updateColors(GameInfo.colors);

            return base.OnConnected();
        }
    }
}