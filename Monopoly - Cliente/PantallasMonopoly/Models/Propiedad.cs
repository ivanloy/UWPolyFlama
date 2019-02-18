
using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PantallasMonopoly.Models
{
    public class Propiedad
    {
        public int precio { get; set; }
        public int nCasas { get; set; }
        public bool tieneHotel { get; set; }
        public bool estaComprada { get; set; }
        public ColorPropiedad color { get; set; }

        public Brush brushColor {
            get {
                Brush ret = new SolidColorBrush(Colors.Black);
                if (this.estaComprada)
                {
                    switch (this.color)
                    {
                        case ColorPropiedad.AMARILLO:
                            ret = new SolidColorBrush(Colors.Yellow);
                            break;
                        case ColorPropiedad.AZUL:
                            ret = new SolidColorBrush(Colors.Blue);
                            break;
                        case ColorPropiedad.CELESTE:
                            ret = new SolidColorBrush(Colors.AliceBlue);
                            break;
                        case ColorPropiedad.ESTACION:
                            ret = new SolidColorBrush(Colors.White);
                            break;
                        case ColorPropiedad.MARRON:
                            ret = new SolidColorBrush(Colors.Brown);
                            break;
                        case ColorPropiedad.NARANJA:
                            ret = new SolidColorBrush(Colors.Orange);
                            break;
                        case ColorPropiedad.ROJO:
                            ret = new SolidColorBrush(Colors.Red);
                            break;
                        case ColorPropiedad.ROSA:
                            ret = new SolidColorBrush(Colors.Pink);
                            break;
                        case ColorPropiedad.SERVICIO:
                            ret = new SolidColorBrush(Colors.LightGray);
                            break;
                        case ColorPropiedad.VERDE:
                            ret = new SolidColorBrush(Colors.Green);
                            break;
                    }
                }
                return ret;
            }
        }

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