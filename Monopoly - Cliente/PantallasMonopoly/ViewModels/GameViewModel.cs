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
        private Casilla[] _casillas;
        
        public Jugador[] Jugadores { get { return _jugadores; } set { _jugadores = value; } }
        public Casilla[] Casillas { get { return _casillas; } set { _casillas = value; } }

        public GameViewModel()
        {
            _jugadores = new Jugador[4];
            _jugadores[0] = new Jugador();
            _jugadores[0].dinero = 123;
            _jugadores[0].nombre = "Penisless guy";
            _jugadores[0].ficha = new Ficha("Cosa", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Nacho.png"));
            _jugadores[1] = new Jugador();
            _jugadores[1].dinero = 2132;
            _jugadores[1].nombre = "La manta raya";
            _jugadores[1].ficha = new Ficha("Cosa", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Hat.png"));
            _jugadores[2] = new Jugador();
            _jugadores[2].dinero = 10;
            _jugadores[2].nombre = "No se";
            _jugadores[2].ficha = new Ficha("Cosa", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Thermo.png"));
            _jugadores[3] = new Jugador();
            _jugadores[3].dinero = 980;
            _jugadores[3].nombre = "Dora";
            _jugadores[3].ficha = new Ficha("Cosa", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Penny.png"));

            _casillas = new Casilla[36];
            for(int i = 0; i < _casillas.Length; i++)
            {
                _casillas[i] = new Casilla();
            }
            _casillas[18].listadoJugadores.Add(_jugadores[0]);
            _casillas[18].listadoJugadores.Add(_jugadores[1]);
            _casillas[24].listadoJugadores.Add(_jugadores[2]);
            _casillas[26].listadoJugadores.Add(_jugadores[3]);
            _casillas[25].listadoJugadores.Add(_jugadores[1]);
            _casillas[25].listadoJugadores.Add(_jugadores[0]);
        }

    }
}
