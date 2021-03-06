﻿using PolyFlamaServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class Propiedad : Casilla
    {
        public string nombre { get; set; }
        public int precio { get; set; }
        public int nCasas { get; set; }
        public bool tieneHotel { get; set; }
        public bool estaComprado { get; set; }
        public ColorPropiedad color { get; set; }
        public int dineroAPagar { get; set; }
        public Jugador comprador { get; set; }
        public int posicionEnTablero { get; set; }

        public Propiedad(string nombre, int precio, int nCasas, bool tieneHotel, bool estaComprado, ColorPropiedad color, int dineroAPagar, Jugador comprador, int posicionEnTablero) : base()
        {
            this.nombre = nombre;
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
        public Propiedad(string nombre, int precio, ColorPropiedad color, int posicionEnTablero, int dineroAPagar) : base()
        {
            this.nombre = nombre;
            this.precio = precio;
            this.nCasas = 0;
            this.tieneHotel = false;
            this.estaComprado = false;
            this.color = color;
            this.dineroAPagar = dineroAPagar;
            this.comprador = null;
            this.posicionEnTablero = posicionEnTablero;
        }

        //ME CAGO EN TU PUTA VIDA PUTO CONSTRUCTOR DE LOS GÜEBOS
        public Propiedad() //🔫🔫🔫
        {

        }
    }
}