using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.ViewModels
{
    public class JugadorConLobby
    {
        public Jugador jugador { get; set; }
        public Lobby lobby { get; set; }

        public JugadorConLobby()
        {
        }
    }
}
