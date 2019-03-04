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
            if (turnoActual == 0) return colores.PLAYER_1B;
            if (turnoActual == 1) return colores.PLAYER_2B;
            if (turnoActual == 2) return colores.PLAYER_3B;
            if (turnoActual == 3) return colores.PLAYER_4B;
            return colores.PLAYER_4B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException(); //Jaja para que quieres eso salu2
        }
    }
}
