using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PantallasMonopoly.Models
{
    class ContrasenaConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibilidad;
            String contrasena = value.ToString();

            if (contrasena.Equals("")) {

                visibilidad = Visibility.Collapsed;

            }
            else
            {

                visibilidad = Visibility.Visible;
            }

            return visibilidad;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException(); //Jaja para que quieres eso salu2
        }
    }
}
