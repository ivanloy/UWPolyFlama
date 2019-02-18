using PantallasMonopoly.Util;
using System.Collections.Generic;

namespace PantallasMonopoly.Models
{
    public class Jugador
    {
        public string nombre { get; set; }
        public Ficha ficha { get; set; }
        public double dinero { get; set; }
        public List<Propiedad> listadoPropiedades { get; set; }
        public int posicion { get; set; }
        public bool carcelGratisSuerte { get; set; }
        public bool carcelGratisComunidad { get; set; }
        public bool estaEnCarcel { get; set; }
        public string dineroConSimboloDolar { get { return "$ " + this.dinero; } }

        public Jugador(string nombre, Ficha ficha, double dinero, int posicion, bool carcelGratisSuerte, bool carcelGratisComunidad, bool estaEnCarcel)
        {
            this.nombre = nombre;
            this.ficha = ficha;
            this.dinero = dinero;
            this.listadoPropiedades = UtilPropiedades.generarListaPropiedadesInicial();
            this.posicion = posicion;
            this.carcelGratisSuerte = carcelGratisSuerte;
            this.carcelGratisComunidad = carcelGratisComunidad;
            this.estaEnCarcel = estaEnCarcel;
        }

        /*
         * Constructor para hacer la creación de jugadores en el lobby más sencilla
         */ 
        public Jugador(string nombre, Ficha ficha)
        {
            this.nombre = nombre;
            this.ficha = ficha;
            this.dinero = 0;
            this.listadoPropiedades = UtilPropiedades.generarListaPropiedadesInicial();
            this.posicion = 0;
            this.carcelGratisSuerte = false;
            this.carcelGratisComunidad = false;
            this.estaEnCarcel = false;
        }

        public Jugador()
        {
            this.listadoPropiedades = UtilPropiedades.generarListaPropiedadesInicial();
        }
    }
}