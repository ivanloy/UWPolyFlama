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

            listado.Add(new Ficha("Bread", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Bread.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Bread_B.png")));
            listado.Add(new Ficha("Hat", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Hat.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Hat_B.png")));
            listado.Add(new Ficha("Nacho", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Nacho.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Nacho_B.png")));
            listado.Add(new Ficha("Penny", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Penny.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Penny_B.png")));
            listado.Add(new Ficha("Thermo", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Thermo.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Thermo_B.png")));
            listado.Add(new Ficha("Rupee", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Rupee.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Rupee_B.png")));
            listado.Add(new Ficha("Danboard", new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Danboard.png"), new Uri("ms-appx://ExamenDI/CustomAssets/Fichas/Danboard_B.png")));

            return listado;
        }

    }
}
