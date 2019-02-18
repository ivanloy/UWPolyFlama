using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PantallasMonopoly.Models
{
    public class Casilla
    {
        public Uri imagen { get; set; }
        public TipoCasilla tipo { get; set; }
        public List<Jugador> listadoJugadores { get; set; }

        public Casilla(TipoCasilla tipo, List<Jugador> listadoJugadores)
        {
            this.tipo = tipo;
            this.listadoJugadores = listadoJugadores;
        }

        public Casilla()
        {

        }
    }
}