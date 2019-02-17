using System;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class Partida
    {
        public List<Carta> listadoCartasSuerte { get; set; }
        public List<Carta> listadoCartasComunidad { get; set; }
        public List<Propiedad> listadoPropiedades { get; set; }
        public List<Casilla> listadoCasillas { get; set; }
        public int turnoActual { get; set; }
        public int[] arrayDados { get; set; }
        public int nTiradasDobles { get; set; }

        public Partida(List<Carta> listadoCartasSuerte, List<Carta> listadoCartasComunidad, List<Propiedad> listadoPropiedades, List<Casilla> listadoCasillas, int turnoActual, int[] arrayDados, int nTiradasDobles)
        {
            this.listadoCartasSuerte = listadoCartasSuerte;
            this.listadoCartasComunidad = listadoCartasComunidad;
            this.listadoPropiedades = listadoPropiedades;
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