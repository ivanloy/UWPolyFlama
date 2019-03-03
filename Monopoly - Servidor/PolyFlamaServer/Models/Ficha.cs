using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyFlamaServer.Models
{
    public class Ficha
    {
        public string nombre { get; set; }
        public Uri imagen { get; set; }
        public Uri imagenBordes { get; set; }

        public Ficha(string nombre, Uri imagen, Uri imagenBordes)
        {
            this.nombre = nombre;
            this.imagen = imagen;
            this.imagenBordes = imagenBordes;
        }

        public Ficha()
        {

        }
    }
}