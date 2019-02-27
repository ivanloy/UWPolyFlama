using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PantallasMonopoly.Models
{
    public class Propiedad : Casilla
    {
        public int precio { get; set; }
        public int nCasas { get; set; }
        public bool tieneHotel { get; set; }
        public bool estaComprado { get; set; }
        public string color { get; set; }
        public int dineroAPagar { get; set; }
        public Jugador comprador { get; set; }
        public int posicionEnTablero { get; set; }

        public Propiedad(int precio, int nCasas, bool tieneHotel, bool estaComprado, string color, int dineroAPagar, Jugador comprador, int posicionEnTablero) : base()
        {
            this.precio = precio;
            this.nCasas = nCasas;
            this.tieneHotel = tieneHotel;
            this.estaComprado = estaComprado;
            this.color = color;
            this.dineroAPagar = dineroAPagar;
            this.comprador = comprador;
            this.posicionEnTablero = posicionEnTablero;
        }

        /*
         * Constructor para crear propiedades más fácilmente
         */ 
        public Propiedad(int precio, string color, int posicionEnTablero) : base()
        {
            this.precio = precio;
            this.nCasas = 0;
            this.tieneHotel = false;
            this.estaComprado = false;
            this.color = color;
            this.dineroAPagar = 0;
            this.comprador = null;
            this.posicionEnTablero = posicionEnTablero;
        }
    }
}