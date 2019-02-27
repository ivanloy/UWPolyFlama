using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class Casilla
    {
        public Uri imagen { get; set; }
        public string tipo { get; set; }
        public List<Jugador> listadoJugadores { get; set; }

        public Casilla(string tipo, List<Jugador> listadoJugadores)
        {
            this.tipo = tipo;
            this.listadoJugadores = listadoJugadores;
        }

        public Casilla(string tipo)
        {
            this.imagen = null;
            this.tipo = tipo;
            this.listadoJugadores = new List<Jugador>();
        }

        public Casilla()
        {
            this.imagen = null;
            this.tipo = TipoCasilla.PROPIEDAD.ToString();
            this.listadoJugadores = new List<Jugador>();
        }
    }
}