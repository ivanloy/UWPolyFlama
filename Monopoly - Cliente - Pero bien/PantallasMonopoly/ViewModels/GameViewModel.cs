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

        }

    }
}
