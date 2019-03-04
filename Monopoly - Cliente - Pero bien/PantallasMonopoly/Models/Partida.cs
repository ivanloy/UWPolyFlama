using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Models
{
    public class Partida
    {
        public List<Carta> listadoCartasSuerte { get; set; }
        public List<Carta> listadoCartasComunidad { get; set; }
        public List<Casilla> listadoCasillas { get; set; }
        public int turnoActual { get; set; }
        public int[] arrayDados { get; set; }
        public int nTiradasDobles { get; set; }
        public Brush colorBarra {
            get { 
                Colores colores = new Colores();
                if (turnoActual == 0) return colores.PLAYER_1C;
                if (turnoActual == 1) return colores.PLAYER_2C;
                if (turnoActual == 2) return colores.PLAYER_3C;
                if (turnoActual == 3) return colores.PLAYER_4C;
                return colores.PLAYER_4C;
            }
        }

        public Partida(List<Carta> listadoCartasSuerte, List<Carta> listadoCartasComunidad, List<Casilla> listadoCasillas, int turnoActual, int[] arrayDados, int nTiradasDobles)
        {
            this.listadoCartasSuerte = listadoCartasSuerte;
            this.listadoCartasComunidad = listadoCartasComunidad;
            this.listadoCasillas = listadoCasillas;
            this.turnoActual = turnoActual;
            this.arrayDados = arrayDados;
            this.nTiradasDobles = nTiradasDobles;
        }

        public Partida()
        {

        }
    }
}