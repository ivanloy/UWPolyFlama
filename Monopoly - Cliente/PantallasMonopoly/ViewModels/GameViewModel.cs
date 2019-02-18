using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.ViewModels
{
    public class GameViewModel
    {
        private Jugador[] _jugadores;
        
        public Jugador[] Jugadores { get { return _jugadores; } set { _jugadores = value; } }

        public GameViewModel()
        {
            _jugadores = new Jugador[1];
            _jugadores[0] = new Jugador();
            _jugadores[0].dinero = 123;
            _jugadores[0].nombre = "Penisless guy";
        }

    }
}
