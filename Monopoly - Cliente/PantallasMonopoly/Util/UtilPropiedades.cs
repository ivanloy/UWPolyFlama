using PantallasMonopoly.Models;
using PantallasMonopoly.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantallasMonopoly.Util
{
    class UtilPropiedades
    {

        /// <summary>
        /// Genera la lista inicial de propiedades, poniendole los colores correspondientes a cada indice
        /// y el bool de si esta comprado a false
        /// </summary>
        /// <returns>La lista de Propiedades</returns>
        public static List<Propiedad> generarListaPropiedadesInicial()
        {
            List<Propiedad> propiedades = new List<Propiedad>();

            propiedades.Add(new Propiedad(ColorPropiedad.MARRON));
            propiedades.Add(new Propiedad(ColorPropiedad.MARRON));

            propiedades.Add(new Propiedad(ColorPropiedad.CELESTE));
            propiedades.Add(new Propiedad(ColorPropiedad.CELESTE));
            propiedades.Add(new Propiedad(ColorPropiedad.CELESTE));

            propiedades.Add(new Propiedad(ColorPropiedad.ROSA));
            propiedades.Add(new Propiedad(ColorPropiedad.ROSA));
            propiedades.Add(new Propiedad(ColorPropiedad.ROSA));

            propiedades.Add(new Propiedad(ColorPropiedad.NARANJA));
            propiedades.Add(new Propiedad(ColorPropiedad.NARANJA));
            propiedades.Add(new Propiedad(ColorPropiedad.NARANJA));

            propiedades.Add(new Propiedad(ColorPropiedad.ROJO));
            propiedades.Add(new Propiedad(ColorPropiedad.ROJO));
            propiedades.Add(new Propiedad(ColorPropiedad.ROJO));

            propiedades.Add(new Propiedad(ColorPropiedad.AMARILLO));
            propiedades.Add(new Propiedad(ColorPropiedad.AMARILLO));
            propiedades.Add(new Propiedad(ColorPropiedad.AMARILLO));

            propiedades.Add(new Propiedad(ColorPropiedad.VERDE));
            propiedades.Add(new Propiedad(ColorPropiedad.VERDE));
            propiedades.Add(new Propiedad(ColorPropiedad.VERDE));

            propiedades.Add(new Propiedad(ColorPropiedad.AZUL));
            propiedades.Add(new Propiedad(ColorPropiedad.AZUL));

            propiedades.Add(new Propiedad(ColorPropiedad.ESTACION));
            propiedades.Add(new Propiedad(ColorPropiedad.ESTACION));
            propiedades.Add(new Propiedad(ColorPropiedad.ESTACION));
            propiedades.Add(new Propiedad(ColorPropiedad.ESTACION));

            propiedades.Add(new Propiedad(ColorPropiedad.SERVICIO));
            propiedades.Add(new Propiedad(ColorPropiedad.SERVICIO));

            return propiedades;
        }

    }
}
