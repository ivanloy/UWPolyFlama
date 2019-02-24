using PantallasMonopoly.ViewModels;
using System.Collections.Generic;

namespace PantallasMonopoly.Models
{
    public class Jugador : clsVMBase
    {
        public string nombre { get; set; }
        public Ficha ficha { get; set; }
        public double dinero { get; set; }
        public List<Propiedad> listadoPropiedades { get; set; }
        private int _posicion;
        public int posicion {
            get { return _posicion; }
            set {
                _posicion = value;
                NotifyPropertyChanged("posicion");
            }
        }
        public bool carcelGratisSuerte { get; set; }
        public bool carcelGratisComunidad { get; set; }
        public bool estaEnCarcel { get; set; }
        public int turnosEnCarcel { get; set; }
        public string dineroConSimboloDolar { get { return "$ " + this.dinero; } }

        public Jugador(string nombre, Ficha ficha, double dinero, List<Propiedad> listadoPropiedades, int posicion, bool carcelGratisSuerte, bool carcelGratisComunidad, bool estaEnCarcel, int turnosEnCarcel)
        {
            this.nombre = nombre;
            this.ficha = ficha;
            this.dinero = dinero;
            this.listadoPropiedades = listadoPropiedades;
            this.posicion = posicion;
            this.carcelGratisSuerte = carcelGratisSuerte;
            this.carcelGratisComunidad = carcelGratisComunidad;
            this.estaEnCarcel = estaEnCarcel;
            this.turnosEnCarcel = turnosEnCarcel;
        }

        /*
         * Constructor para hacer la creación de jugadores en el lobby más sencilla
         */ 
        public Jugador(string nombre, Ficha ficha)
        {
            this.nombre = nombre;
            this.ficha = ficha;
            this.dinero = 1500;
            this.listadoPropiedades = new List<Propiedad>();
            this.posicion = 0;
            this.carcelGratisSuerte = false;
            this.carcelGratisComunidad = false;
            this.estaEnCarcel = false;
            this.turnosEnCarcel = 0;
        }

        public Jugador()
        {

        }
    }
}