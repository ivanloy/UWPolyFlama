using PantallasMonopoly.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PantallasMonopoly.Models
{
    class BarraConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int turnoActual = (int)value;
            Colores colores = new Colores();
            if (turnoActual == 0) return colores.PLAYER_1C;
            if (turnoActual == 1) return colores.PLAYER_2C;
            if (turnoActual == 2) return colores.PLAYER_3C;
            if (turnoActual == 3) return colores.PLAYER_4C;
            return colores.PLAYER_4C;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException(); //Jaja para que quieres eso salu2
        }
    }
}
