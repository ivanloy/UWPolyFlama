using PantallasMonopoly.Models.Enums;
using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Models
{
    public class Propiedad : Casilla
    {
        public int precio { get; set; }
        public int nCasas { get; set; }
        public bool tieneHotel { get; set; }
        public bool estaComprado { get; set; }
        public ColorPropiedad color { get; set; }
        public int dineroAPagar { get; set; }
        public Jugador comprador { get; set; }
        public int posicionEnTablero { get; set; }
        public string nombre { get; set; }
        public float opacidad {
            get {
                if (listadoJugadores.Count >= 1) return 0.6f;
                return 1;
            }
        }
        public Brush brushColor {
            get {
                Brush ret = new SolidColorBrush(Colors.Black);
                Colores colores = new Colores();
                if(comprador != null)
                {
                    ret = new SolidColorBrush(Colors.Gray);
                }
                else if (this.estaComprado)
                {
                    switch (this.color)
                    {
                        case ColorPropiedad.AMARILLO:
                            ret = colores.AMARILLO;
                            break;
                        case ColorPropiedad.AZUL:
                            ret = colores.AZUL;
                            break;
                        case ColorPropiedad.CELESTE:
                            ret = colores.CELESTE;
                            break;
                        case ColorPropiedad.ESTACION:
                            ret = new SolidColorBrush(Colors.White);
                            break;
                        case ColorPropiedad.MARRON:
                            ret = colores.MARRON;
                            break;
                        case ColorPropiedad.NARANJA:
                            ret = colores.NARANJA;
                            break;
                        case ColorPropiedad.ROJO:
                            ret = colores.ROJO;
                            break;
                        case ColorPropiedad.ROSA:
                            ret = colores.ROSA;
                            break;
                        case ColorPropiedad.SERVICIO:
                            ret = new SolidColorBrush(Colors.LightGray);
                            break;
                        case ColorPropiedad.VERDE:
                            ret = colores.VERDE;
                            break;
                    }
                }
                return ret;
            }
        }
        public Propiedad(int precio, int nCasas, bool tieneHotel, bool estaComprado, ColorPropiedad color, int dineroAPagar, Jugador comprador, int posicionEnTablero) : base()
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
        public Propiedad(int precio, ColorPropiedad color, int posicionEnTablero) : base()
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

        public Propiedad()
        {

        }
    }
}