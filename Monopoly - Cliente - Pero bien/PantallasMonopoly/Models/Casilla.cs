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
        public float opacidad {
            get {
                if (listadoJugadores.Count >= 1) return 0.6f;
                return 1;
            }
        }

        public string tamanoImagenFichaJugador {
            get {
                string ret = "";
                if (listadoJugadores.Count > 1) ret = "*";
                else ret = "0";
                return ret;
            }
        }


        public Casilla(TipoCasilla tipo, List<Jugador> listadoJugadores)
        {
            this.tipo = tipo;
            this.listadoJugadores = listadoJugadores;
        }

        public Casilla(TipoCasilla tipo)
        {
            this.imagen = null;
            this.tipo = tipo;
            this.listadoJugadores = new List<Jugador>();
        }

        public Casilla()
        {
            this.imagen = null;
            this.tipo = TipoCasilla.PROPIEDAD;
            this.listadoJugadores = new List<Jugador>();
        }
    }
}