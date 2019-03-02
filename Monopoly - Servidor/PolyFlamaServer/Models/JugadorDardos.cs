using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class JugadorDardos
    {
        public string nombre { get; set; }
        public int puntuacion { get; set; }
        public Ficha ficha { get; set; }

        public JugadorDardos()
        {
            this.nombre = "";
            this.puntuacion = 0;
            this.ficha = new Ficha();
        }

        public JugadorDardos(string nombre, int puntuacion, Ficha ficha)
        {
            this.nombre = nombre;
            this.puntuacion = puntuacion;
            this.ficha = ficha;
        }
    }
}