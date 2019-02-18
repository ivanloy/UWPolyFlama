using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Models
{
    public class Casilla
    {
        public TipoCasilla tipo { get; set; }
        public List<Jugador> listadoJugadores { get; set; }

        public Ficha[] fichas {
            get {
                Ficha[] ret = new Ficha[4];
                for (int i = 0; i < 4; i++)
                {
                    if (listadoJugadores.Count >= i + 1) ret[i] = listadoJugadores[i].ficha;
                    else ret[i] = new Ficha();
                }
                return ret;
            }
        }

        public string bigImage {
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

        public Casilla()
        {
            listadoJugadores = new List<Jugador>();
        }
    }
}