using PantallasMonopoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.Util
{
    public class generadorFichas
    {

        public static List<Ficha> listadoFichas()
        {

            List<Ficha> listado = new List<Ficha>();

            listado.Add(new Ficha("Bread", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Bread.png")));
            listado.Add(new Ficha("Hat", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Hat.png")));
            listado.Add(new Ficha("Nacho", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Nacho.png")));
            listado.Add(new Ficha("Penny", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Penny.png")));
            listado.Add(new Ficha("Thermo", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Thermo.png")));

            return listado;
        }

    }
}
