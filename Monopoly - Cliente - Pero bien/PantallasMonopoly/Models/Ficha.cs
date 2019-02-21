using System;
using System.Collections.Generic;
using System.Linq;


namespace PantallasMonopoly.Models
{
    public class Ficha
    {
        public string nombre { get; set; }
        public Uri imagen { get; set; }

        public Ficha(string nombre, Uri imagen)
        {
            this.nombre = nombre;
            this.imagen = imagen;
        }

        public Ficha()
        {

        }
    }
}