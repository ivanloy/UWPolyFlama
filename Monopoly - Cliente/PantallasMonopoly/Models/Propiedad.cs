
using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class Propiedad
    {
        public int precio { get; set; }
        public int nCasas { get; set; }
        public bool tieneHotel { get; set; }
        public Jugador comprador { get; set; }
        public ColorPropiedad color { get; set; }

        public Propiedad(int precio, int nCasas, bool tieneHotel, Jugador comprador, ColorPropiedad color)
        {
            this.precio = precio;
            this.nCasas = nCasas;
            this.tieneHotel = tieneHotel;
            this.comprador = comprador;
            this.color = color;
        }

        /*
         * Constructor para crear propiedades más fácilmente
         */ 
        public Propiedad(int precio, ColorPropiedad color)
        {
            this.precio = precio;
            this.nCasas = 0;
            this.tieneHotel = false;
            this.comprador = null;
            this.color = color;
        }
    }
}